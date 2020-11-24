using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using EFCoreTutorial.Lesson_01.Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace EFCoreTutorial.Lesson_01
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
			// зарегистрировать контроллеры, унаследованные от ControllerBase
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.IgnoreNullValues = true;
					options.JsonSerializerOptions.WriteIndented = true;
				});
			// зарегистрировать сервисы, отвечающие за проверку работоспосбности webapi-приложения
			services.AddHealthChecks();

			// получить строку подключения к БД
			var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
				?? Configuration.GetConnectionString("PostgreSQLConnection");

			services
				// .AddEntityFrameworkNpgsql() Depricated
				.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));

			// включить swagger для обслуживания одного и более документов
			services.AddSwaggerGen(g =>
			{
				g.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "EFCore Demo API",
					Description = "This demo service for EFCore",
					TermsOfService = new Uri("http://test.ru"),
					Contact = new OpenApiContact
					{
						Name = "Leu Jo",
						Email = "statusqwr@gmail.com",
						Url = new Uri("http://www.leujo.com")
					},
					License = new OpenApiLicense
					{
						Name = "Super LICX",
						Url = new Uri("http://leujo.com/products/license")
					}
				});
				
				// Задать путь к файлу с комментариями для Swagger JSON и UI
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				g.IncludeXmlComments(xmlPath);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// включить middleware для обслуживания json-документа сгенерированного swagger через специалбный endpoint
			app.UseSwagger();

			// включить middleware для обслуживания swagger-ui (HTML, JSS, CSS),
			// которая соответствует конечной точке swagger
			app.UseSwaggerUI(options =>
			{
				options.RoutePrefix = string.Empty;
				options.SwaggerEndpoint("swagger/v1/swagger.json", "EFDemo API");
			});

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseHealthChecks("/health", new HealthCheckOptions { ResponseWriter = JsonResponseWriter });

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private async Task JsonResponseWriter(HttpContext context, HealthReport report)
		{
			context.Response.ContentType = "application/json";
			await JsonSerializer.SerializeAsync(
				context.Response.Body,
				new
				{
					Status = report.Status.ToString()
				},
				new JsonSerializerOptions
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				});
		}
	}
}
