using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name= "role",   
                    UserClaims=new List<string>(){"role"}
                }

            };
        public static IEnumerable<ApiScope> ApiScopes =>
            new[] { new ApiScope("apidemoscope","ApiDemo Service")};

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("WebApiDemo","WebApiDemo Service")
                {
                    Scopes= new List<string> { "apidemoscope"},
                    UserClaims=new List<string>{"role"}
                }
            };

  //      public static List<TestUser> TestUsers =>
  //new List<TestUser>
  //{
  //    new TestUser
  //    {
  //        SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
  //        Username = "Mick",
  //        Password = "MickPassword",
  //        Claims = new List<Claim>
  //        {
  //            new Claim("given_name", "Mick"),
  //            new Claim("family_name", "Mining")
  //        }
  //    },
  //    new TestUser
  //    {
  //        SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
  //        Username = "Jane",
  //        Password = "JanePassword",
  //        Claims = new List<Claim>
  //        {
  //            new Claim("given_name", "Jane"),
  //            new Claim("family_name", "Downing")
  //        }
  //    }
  //};
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client()
                {
                    ClientId="m2m.client",
                    ClientName="Client Credentials Client",
                    ClientSecrets={new Secret("ClientSecret1".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId }
                },
                new Client()
                {
                    ClientId="interactive",
                    ClientSecrets={new Secret("ClientSecret1".Sha256())},
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    RedirectUris={"https://localhost:5444/signin-oidc" },
                    FrontChannelLogoutUri="https://localhost:5444/signout-oidc",
                    PostLogoutRedirectUris={"https://localhost:5444/signout-callback-oidc"},
                    AllowedScopes=
                    {
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                        "apidemoscope"
                    }
                },
                new Client()
                {
                    ClientId="263475687100-jc58omhajnv9ui7fnen35p5160okj5nm.apps.googleusercontent.com",
                    ClientName="Google",
                    ClientSecrets={new Secret("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".Sha256()) },
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    RequireConsent = false,
                    RedirectUris = { "http://localhost:4200/product" },
                    AllowedScopes=
                    {
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Email,
                        "apidemoscope"
                    },
                    UpdateAccessTokenClaimsOnRefresh = true,
                    BackChannelLogoutUri = "https://accounts.google.com/Logout",

                },
            };
    }
}
