namespace Kulinarka.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public TimeSpan Duration { get; set; }
        public string UserAgent { get; set; }
        public string Referer { get; set; }

    }
}
