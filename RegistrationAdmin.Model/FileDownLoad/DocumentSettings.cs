namespace RegistrationAdmin.Models.FileDownLoad
{
    public interface IDocumentSettings
    {
        string SourceSystem { get; set; }
        string DocumentSystem { get; set; }
        string FileExtension { get; set; }
        string DocumentUrl { get; set; }
        string PublicApiKey { get; set; }
        string InformationManagementApiKey { get; set; }
        string CsvDownloadUrl { get; set; }
        string SubmissionFeedbackCsvUrl { get; set; } 
    }
    public class DocumentSettings : IDocumentSettings
    {
        public string SourceSystem { get; set; }
        public string DocumentSystem { get; set; }
        public string FileExtension { get; set; }
        public string DocumentUrl { get; set; }
        public string PublicApiKey { get; set; }
        public string InformationManagementApiKey { get; set; }
        public string CsvDownloadUrl { get; set; }
        public string SubmissionFeedbackCsvUrl { get; set; } 
    }
}
