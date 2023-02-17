namespace IdentityServer.Application
{
    public class IdentityServerSettings
    {
        public string IdentityDiscoveryUrl { get; set; }
        public string IdentityWellKnown { get; set; }
        public string IdentityClientId { get; set; }
        public string IdentityClientPassword { get; set; }
        public bool IdentityUseHttps { get; set; }
        public string IdentityScope { get; set; }
    }
}
