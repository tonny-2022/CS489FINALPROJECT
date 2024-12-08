using System.Text;
using FoundItemService.data;
using FoundItemService.mapping;
using FoundItemService.repository;
using FoundItemService.service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
	// Add JWT Bearer authentication definition to Swagger
	options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
		Scheme = "bearer",
		BearerFormat = "JWT",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Description = "Please enter token in the format 'Bearer {token}'"
	});
	options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] { }
		}
	});
});
builder.Services.AddDbContext<ItemFoundDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("ItemFoundServiceConnectionString")));

builder.Services.AddAutoMapper(typeof(ItemFoundMapping));
builder.Services.AddScoped<ItemFoundRepository,ItemFoundServiceImpl>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["jwt:Issuer"],
			//ValidAudience = builder.Configuration["jwt:Audience"],// single audince, we use Audience
			ValidAudiences = builder.Configuration.GetSection("jwt:Audiences").Get<string[]>(),//multiple audinces , we use Audiences
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"]))
		};
		// Add event handlers for debugging
		options.Events = new JwtBearerEvents
		{
			OnAuthenticationFailed = context =>
			{
				Console.WriteLine($"Authentication failed: {context.Exception.Message}");
				return Task.CompletedTask;
			},
			OnTokenValidated = context =>
			{
				Console.WriteLine("Token validated successfully");
				return Task.CompletedTask;
			}
		};
	});
	builder.Services.AddCors(options =>
	{
		options.AddDefaultPolicy(builder =>
		{
			builder.AllowAnyOrigin()
				   .AllowAnyHeader()
				   .AllowAnyMethod();
		});
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();
