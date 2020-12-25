
namespace BaseProject.WebAPI.Core.Identity
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpirationTime { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
