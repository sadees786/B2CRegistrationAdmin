using Newtonsoft.Json;

using System;
using System.ComponentModel;
using RegistrationAdmin.Models.Constants;

namespace RegistrationAdmin.ViewModels
{
    public class SubmissionViewModel
    { 
        public string ApplicationStatusHistoryId { get; set; }  
        public int ApplicationId { get; set; }       
        public int ReSubmissionValue { get; set; }        
        public string ApplicationReference { get; set; }       
        public int Status { get; set; }      
        public string StatusDescription { get; set; }  
         
        private string _timeStamp;
        [JsonProperty("TimeStamp")]
        [DisplayName("Time Stamp")] 
        public string TimeStamp
        { 
            get => _timeStamp;
            set
            {
                DateTime.TryParse(value, out var tmp);
                _timeStamp = tmp.ToString(GlobalAccess.DateTimeFormatPattern);
            }
        }    
        private string _createdDate;
        [JsonProperty("CreateDate")]
        [DisplayName("Created Date")] 
        public string CreatedDate
        { 
            get => _createdDate;
            set
            {
                DateTime.TryParse(value, out var tmp);
                _createdDate = tmp.ToString(GlobalAccess.DateTimeFormatPattern);
            }
        }    
        private string _modifiedDate;
        [JsonProperty("ModifiedDate")]
        [DisplayName("Modified Date")] 
        public string ModifiedDate
        { 
            get => _modifiedDate;
            set
            {
                DateTime.TryParse(value, out var tmp);
                _modifiedDate = tmp.ToString(GlobalAccess.DateTimeFormatPattern);
            }
        }   
    }
}

  
