using Newtonsoft.Json;

using System;
using System.ComponentModel;
using RegistrationAdmin.Models.Constants;

namespace RegistrationAdmin.ViewModels
{
    public class Feedback
    { 
        [DisplayName("Feedback Id")]
        public int FeedbackId { get; set; }

        [DisplayName("Source System")]
        public string SourceSystem { get; set; }

        [DisplayName("Transaction Type")]
        public string TransactionType { get; set; }

        [JsonProperty("FeedbackSubmissionType")]
        [DisplayName("Feedback Type")]
        public string FeedbackType { get; set; }

        private string _dateSubmitted;
        [JsonProperty("CreateDate")]
        [DisplayName("Date Submitted")] 
        public string DateSubmitted
        {
            get => _dateSubmitted;
            set
            {
                DateTime.TryParse(value, out var tmp); 
                _dateSubmitted = tmp.ToString(GlobalAccess.DateTimeFormatPattern);
            }
        } 

        [DisplayName("Calling Screen URL")]
        public string CallingScreen { get; set; }

        [DisplayName("User Feedback")]
        public string UserFeedback { get; set; }

        [DisplayName("How You Feel About The Service")]
        public string HowYouFeelAboutTheService { get; set; }

        [DisplayName("Overall Experience")]
        public string OverallExperience { get; set; }

        [JsonProperty("AllowContactDesc")]
        [DisplayName("Allow Contact")]
        public string AllowContact { get; set; }

        [DisplayName("User Email")]
        public string UserEmail { get; set; }

        [DisplayName("Version")]
        public string Version { get; set; }  

    }
}