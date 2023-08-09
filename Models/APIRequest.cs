namespace Be_My_Voice_Backend.Models
{
    public class APIRequest
    {
        public string url { get; set; }
        public string method { get; set; }
        public object data { get; set; }
        public string token { get; set; }
    }
}
