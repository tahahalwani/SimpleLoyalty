using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(name: "api1", displayName: "SimpleLoyalty API"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<ApiResource> ApiResources =>
       new ApiResource[]
       {
                new ApiResource("api1", "SimpleLoyalty API")
                {
                    Scopes = {
                        "api1"
                    }
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
       };

        public static IEnumerable<Client> Clients(IConfiguration config) =>
            new Client[]
             {
                new Client
                {
                    ClientId = "client",
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {
                        $"{config["SwaggerClient"]}/swagger/oauth2-redirect.html",
                    },
                    PostLogoutRedirectUris = {config["SwaggerClient"] },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api1", "role", IdentityServerConstants.LocalApi.ScopeName },
                    AllowedCorsOrigins = new List<string> { config["SwaggerClient"] },
                     AllowAccessTokensViaBrowser = true,
                    RequireConsent = false
                }
             };
    }
}
