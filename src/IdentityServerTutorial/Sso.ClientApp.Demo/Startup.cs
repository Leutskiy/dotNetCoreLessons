using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace Sso.ClientApp.Demo
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "Sso.ClientApp.Demo", Version = "v1" });

				options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.OAuth2,
					Flows = new OpenApiOAuthFlows
					{
						AuthorizationCode = new OpenApiOAuthFlow
						{
							AuthorizationUrl = new Uri("https://localhost:5005/connect/authorize"),
							TokenUrl = new Uri("https://localhost:5005/connect/token"),
							Scopes = new Dictionary<string, string>
								{
									{"leujo-api-scope-1", "Demo API - full access"}
								}
						}
					}
				});

				options.OperationFilter<AuthorizeCheckOperationFilter>();
			});

			services.AddAuthentication("Bearer")
				.AddIdentityServerAuthentication("Bearer", options =>
				{
					// required audience of access tokens
					options.ApiName = "leujo-api-resource-name";

					// auth server base endpoint (this will be used to search for disco doc)
					options.Authority = "https://localhost:5005";
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "Sso.ClientApp.Demo v1");

					options.OAuthClientId("client-leujo-id");
					options.OAuthAppName("Demo API - Swagger");
					options.OAuthUsePkce();
				});
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
