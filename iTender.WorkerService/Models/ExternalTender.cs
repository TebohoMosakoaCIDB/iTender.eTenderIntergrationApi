using System.Text.Json.Serialization;

namespace iTender.WorkerService.Models
{
    public class ExternalTender
    {
        [JsonPropertyName("tender_no")]
        public string Number { get; set; } = "";
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("delivery")]
        public string Delivery { get; set; }
        [JsonPropertyName("department")]
        public string Department { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } = "";
        [JsonPropertyName("category")]
        public string Category { get; set; } = "";
        [JsonPropertyName("dp")]
        public string PublishDate { get; set; }
        [JsonPropertyName("cd")]
        public string ClosingDate { get; set; }

        [JsonPropertyName("brief")]
        public string BriefDate { get; set; }
        [JsonPropertyName("cbrief")]
        public bool Compulsorybrief { get; set; }
        [JsonPropertyName("briefingVenue")]
        public string BriefVenue { get; set; }
        [JsonPropertyName("contactPerson")]
        public string ContactPerson { get; set; }
        [JsonPropertyName("email")]
        public string ContactEmail { get; set; }
        [JsonPropertyName("telephone")]
        public string ClosingTelephone { get; set; }

    }
}
