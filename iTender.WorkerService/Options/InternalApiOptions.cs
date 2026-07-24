namespace iTender.WorkerService.Options
{
    public class InternalApiOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string? ApiKey { get; set; }
        public bool UseApiKey { get; set; } = false;
    }
}
