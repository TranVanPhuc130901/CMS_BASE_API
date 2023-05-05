namespace CMS_Common
{
    public class Token
    {
        public string newToken { get; set; } = string.Empty;
        public DateTime CreateToken { get; set; } = DateTime.Now;
        public DateTime ExpiresToken { get; set; }
    }
}
