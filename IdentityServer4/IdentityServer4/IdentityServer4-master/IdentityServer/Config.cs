using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {
        // 定义被保护的API范围（Scope）
        public static IEnumerable<ApiScope> ApiScopes => new[]
        {
            new ApiScope{Name = "api_1",DisplayName = "sample_api_1"},
            new ApiScope{Name = "api_2",DisplayName = "sample_api_2"},
        };
        // 定义客户端
        public static IEnumerable<Client> Clients => new[]
        {
            new Client
            {
                ClientId = "Client_1",
                // 一个ID可以有多个密钥
                ClientSecrets ={new Secret("Secret_1".Sha256())},
                //指定客户端凭据许可模式
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                //客户端允许的API范围,可以多组
                AllowedScopes ={"api_1"}
            },
            new Client
            {
                ClientId = "Client_role",
                // 一个ID可以有多个密钥
                ClientSecrets ={new Secret("Secret_rote".Sha256())},
                //指定客户端凭据许可模式
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                //客户端允许的API范围,可以多组
                AllowedScopes ={"api_1"}
            },
            new Client
            {
                ClientId = "Client_mvc",
                ClientName = "Client MVC",
                // 一个ID可以有多个密钥
                ClientSecrets ={new Secret("Secret_mvc".Sha256())},
                //指定客户端凭据许可模式
                AllowedGrantTypes = GrantTypes.Code,
                // MVC客户端回调地址
                RedirectUris = {"https://localhost:4001/signin-oidc"},
                // 登出地址
                PostLogoutRedirectUris = { "https://localhost:4001/signin-callback-oidc" },
                // 允许那些身份访问资源
                //客户端允许的API范围,可以多组
                AllowedScopes ={
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                },
                // 是否需要用户同意
                RequireConsent = true
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static List<TestUser> GetUsers => new List<TestUser>
        {
            new TestUser
            {
                SubjectId ="1",// 用户的唯一标识
                Username = "louhoin",
                Password = "plouhoin",
                Claims = new List<Claim>
                {
                    new Claim("scope","api_1"),
                    new Claim("scope","api_2"),
                }
            }
        };
    }
}

// 客户端通过验证后可以获得访问令牌（access token），api范围会作为访问令牌中的信息，
// 当客户端携带访问令牌的时候就可以访问api资源，授权服务会验证携带的api范围是否正确