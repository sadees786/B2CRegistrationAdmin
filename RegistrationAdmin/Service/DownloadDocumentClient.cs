using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using RegistrationAdmin.Models.Constants;
using RegistrationAdmin.Models.Enum;
using RegistrationAdmin.Models.FileDownLoad;
using RegistrationAdmin.ViewModels;

//TODO : Create a new service project and move it
namespace RegistrationAdmin.Service
{
    public interface IDownloadDocumentClient
    {
        Task<Stream> DownloadFile(string refenceNo);
        Task<Stream> DownloadFileCSV();
        Task<Stream> PreSubmissionFeedbackCSV(ClaimsPrincipal User);
        Task<Stream> PostSubmissionFeedbackCSV(ClaimsPrincipal User);
    }
    public class DownloadDocumentClient : IDownloadDocumentClient
    {
        private readonly HttpClient _httpClient;
        private readonly IDocumentSettings _documentSettings;
        private readonly IAuthorizationService _authorizationService;


        public DownloadDocumentClient(HttpClient httpClient
                                        , IDocumentSettings documentSettings
                                        , IAuthorizationService authorizationService)
        {
            _httpClient = httpClient;
            _documentSettings = documentSettings;
            _authorizationService = authorizationService;


        }
        public async Task<Stream> DownloadFile(string refenceNo)
        {
            var applicationReference = refenceNo + _documentSettings.DocumentSystem;
            var fileModel = new FileViewModel()
            {
                SourceSystem = _documentSettings.SourceSystem,
                ReferencNumber = applicationReference
            };
            var model = JsonConvert.SerializeObject(fileModel);
            var nameValueCollection = new List<KeyValuePair<string, string>>
                                          {new KeyValuePair<string, string>("jsonParameters", model)};
            _httpClient.DefaultRequestHeaders.Add("x-functions-key", _documentSettings.PublicApiKey);
            var response = await _httpClient.PostAsync($"{_documentSettings.DocumentUrl}/api/DownLoadFile/{applicationReference}", new FormUrlEncodedContent(nameValueCollection));

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsByteArrayAsync();
            var fileStream = new MemoryStream();
            fileStream.Write(responseBody, 0, responseBody.Length);
            fileStream.Seek(0, SeekOrigin.Begin);
            return fileStream;
        }

        public async Task<Stream> DownloadFileCSV()
        { 
            var webUrl = new Uri(_documentSettings.CsvDownloadUrl);
            var body = new StringContent("{}", Encoding.UTF8, "application/json");

            var responseBody = await RegistrationClient( webUrl, body);

            if (string.IsNullOrEmpty(responseBody))
            {
                return Stream.Null;
            }

            var responses = JsonConvert.DeserializeObject<List<SubmissionViewModel>>(responseBody);
            var csvString = WriteTocsv(responses);

            var fileOutput = Encoding.UTF8.GetBytes(csvString);

            var fileStream = CreateFileStream(fileOutput);
            return fileStream;
        }
        public async Task<Stream> PreSubmissionFeedbackCSV(ClaimsPrincipal user)
        { 
            var allowSensitiveDataAccess = _authorizationService.AuthorizeAsync(user,
                RegistrationTransformationGroupPolicy.SensitiveDataAccess).Result.Succeeded;

            var bodyContent = CreateBodyContent(allowSensitiveDataAccess, FeedbackSubmissionType.PreSubmission);

            var body = new StringContent(bodyContent, Encoding.UTF8, "application/json");

            var webUrl = new Uri(_documentSettings.SubmissionFeedbackCsvUrl);

            var responseBody = await RegistrationClient(webUrl, body);

            if (string.IsNullOrEmpty(responseBody))
            {
                return Stream.Null;
            }
            var responses = JsonConvert.DeserializeObject<List<Feedback>>(responseBody);
            var csvString = WriteTocsv(responses);

            var fileOutput = Encoding.UTF8.GetBytes(csvString);

            var fileStream = CreateFileStream(fileOutput);
            return fileStream;
        }
        public async Task<Stream> PostSubmissionFeedbackCSV(ClaimsPrincipal user)
        {
            var allowSensitiveDataAccess = _authorizationService.AuthorizeAsync(user,
                RegistrationTransformationGroupPolicy.SensitiveDataAccess).Result.Succeeded;

            var bodyContent = CreateBodyContent( allowSensitiveDataAccess, FeedbackSubmissionType.PostSubmission);

            var body = new StringContent(bodyContent, Encoding.UTF8, "application/json");
            var webUrl = new Uri(_documentSettings.SubmissionFeedbackCsvUrl);

            var responseBody = await RegistrationClient(webUrl, body);

            if (string.IsNullOrEmpty(responseBody))
            {
                return Stream.Null;
            }
            var responses = JsonConvert.DeserializeObject<List<Feedback>>(responseBody);
            var csvString = WriteTocsv(responses);

            var fileOutput = Encoding.UTF8.GetBytes(csvString);

            var fileStream = CreateFileStream(fileOutput);
            return fileStream;
        }

        private string CreateBodyContent(bool allowSensitiveDataAccess, FeedbackSubmissionType submissionType )
        { 
            var bodyBuilder = new StringBuilder();

            bodyBuilder.AppendLine("{");
            bodyBuilder.AppendFormat("\"FeedbackSubmissionType\":{0},", (int)submissionType);
            bodyBuilder.AppendLine("");
            bodyBuilder.AppendFormat("\"AllowSensitiveDataAccess\":{0}", allowSensitiveDataAccess.ToString().ToLower());
            bodyBuilder.AppendLine("");
            bodyBuilder.AppendLine("}");

            return bodyBuilder.ToString(); 
        }

        private async Task<string> RegistrationClient(Uri webUrl, StringContent body)
        {
            string responseBody;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Management-information-Key", _documentSettings.InformationManagementApiKey);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = webUrl,
                    Content = body
                };

                var result = await client.SendAsync(request);

                if (result.StatusCode == HttpStatusCode.BadRequest
                     || result.StatusCode == HttpStatusCode.NotFound)
                {
                    return string.Empty;
                }

                result.EnsureSuccessStatusCode(); 

                responseBody = await result.Content.ReadAsStringAsync();
            }

            return responseBody;
        }

        private static MemoryStream CreateFileStream(byte[] bArray)
        {
            var fileStream = new MemoryStream();
            fileStream.Write(bArray, 0, bArray.Length);
            fileStream.Seek(0, SeekOrigin.Begin);
            return fileStream;
        }

        private string WriteTocsv<T>(IEnumerable<T> data)
        {
            var output = new StringBuilder();
            var props = TypeDescriptor.GetProperties(typeof(T));

            foreach (PropertyDescriptor prop in props)
            {
                output.Append(prop.DisplayName); 
                output.Append(",");
            }

            output.AppendLine();

            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.AppendFormat("\"{0}\"",prop.Converter.ConvertToString(prop.GetValue(item)));
                    output.Append(",");
                }
                output.AppendLine();
            }
            return output.ToString();
        }
    }
}

   


