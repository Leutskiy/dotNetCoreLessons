using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Sso.IdentityServer.Configuration
{
	public static class InMemoryConfig
	{
		public static IEnumerable<Client> GetClients()
		{
			return new List<Client>
			{
				new Client
				{
					ClientId = "client-leujo-id",
					ClientName = "Swagger Demo Test :)",
					ClientSecrets = new [] { new Secret("client-leujo-secret".Sha512()) },
					
					RequirePkce = true, 
					RequireClientSecret = false,

					RedirectUris = { "https://localhost:5001/swagger/oauth2-redirect.html" },
					AllowedCorsOrigins = { "https://localhost:5001" },
					AllowedGrantTypes = GrantTypes.Code,
					AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "leujo-api-scope-1", "leujo-api-scope-2" }
				}
			};
		}

		public static List<TestUser> GetTestUsers()
		{
			return new List<TestUser>
			{
				new TestUser
				{
					SubjectId = "0649725b-41e3-4834-b808-e0de40aba8e1",
					Username = "leujo",
					Password = "P@ssw0rd",
					Claims = new List<Claim>
					{
						new Claim("surnamename", "Leutskiy"),
						new Claim("name", "John")
					}
				}
			};
		}

		public static IEnumerable<ApiResource> GetApiResources()
		{
			return new List<ApiResource>
			{
				new ApiResource("leujo-api-resource-name", "leujo-api-resource-displayname")
				{
					Scopes = { "leujo-api-scope-1", "leujo-api-scope-2" }
				}
			};
		}

		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile()
			};
		}

		public static IEnumerable<ApiScope> GetApiScopes()
		{
			return new List<ApiScope>
			{ 
				new ApiScope("leujo-api-scope-1", "LeuJo API Scope №1"),
				new ApiScope("leujo-api-scope-2", "LeuJo API Scope №2")
			};
		}
	}
}
