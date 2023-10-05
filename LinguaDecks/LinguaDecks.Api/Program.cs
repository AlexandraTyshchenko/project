using LinguaDecks.Api.Mappers;
using LinguaDecks.Api.Middleware;
using LinguaDecks.Business.Interfaces;
using LinguaDecks.Business.Services;
using LinguaDecks.DataAccess.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace LinguaDecks.Api
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services
			  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			  .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
			  {
				  ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
				  ValidAudience = builder.Configuration["JwtSettings:Audience"],
				  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
				  ValidateIssuer = true,
				  ValidateAudience = true,
				  ValidateLifetime = true,
				  ValidateIssuerSigningKey = true
			  });

			builder.Services.AddAuthorization();

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
				{
					Name = "JWT Authentication",
					Description = "Enter a valid JWT bearer token",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT"
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
		{
		  {
			new OpenApiSecurityScheme
			{
			  Reference = new OpenApiReference
			  {
				Id = JwtBearerDefaults.AuthenticationScheme,
				Type = ReferenceType.SecurityScheme
			  }
			},
			System.Array.Empty<string>()
		  }
		});
			});

			builder.Services.AddAutoMapper(typeof(MapperConfig));

			builder.Services.AddDbContext<ApplicationContext>(options =>
			  options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
			builder.Services.AddScoped<ITokenService, TokenService>();

			builder.Services.AddAutoMapper(typeof(MapperConfig));
			builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
			builder.Services.AddScoped<IDecksService, DecksService>();
			builder.Services.AddScoped<IDeckSearcher, DeckSearcher>();
			builder.Services.AddScoped<ICategoriesProvider, CategoriesProvider>();
			builder.Services.AddScoped<IRequestsProvider, RequestsProvider>();
			builder.Services.AddScoped<IRequestManager, RequestManager>();
			builder.Services.AddScoped<ICategoriesCreator, CategoriesCreator>();
			builder.Services.AddScoped<ICategoryDeletter, CategoryDeletter>();
			builder.Services.AddScoped<ITeacherRequestCreator, TeacherRequestCreator>();
			builder.Services.AddScoped<ICardsService, CardsService>();
			builder.Services.AddScoped<IProgressService, ProgressService>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IImageService, ImageService>();

			builder.Services.AddScoped<ICommentService, CommentService>();
			builder.Services.AddScoped<ICommentDeletter, CommentDeletter>();
			var app = builder.Build();
			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseCors(options => options
			  .AllowAnyHeader()
			  .AllowAnyMethod()
			  .WithOrigins(builder.Configuration.GetSection("Urls:FrontEndUrl").Value)
			  .AllowCredentials());

			app.UseHttpsRedirection();

			app.UseMiddleware<ExceptionMiddleware>();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			UpdateDatabase(app);

			app.Run();
		}

		public static void UpdateDatabase(WebApplication app)
		{
			using (var serviceScope = app.Services.CreateScope())
			{
				ApplicationContext context = serviceScope.ServiceProvider.GetService<ApplicationContext>();
				context.Database.Migrate();
			}
		}
	}
}