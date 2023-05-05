namespace CMS_Common
{
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Create { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
    }
}
