

using System.Text;
using LoginService.data;
using LoginService.mapping;
using LoginService.model;
using LoginService.repository;
using LoginService.service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AuthDbContext>(
	options => options.UseNpgsql(builder.Configuration.GetConnectionString("AuthServiceConnectionString")));
builder.Services.AddScoped<ITokenRepository,TokenServiceImpl>();
builder.Services.AddScoped<IUserRepository,UserServiceImpl>();
builder.Services.AddAutoMapper(typeof(UserMapping));
// Configure JWT Authentication with Multiple Audiences

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
	AddJwtBearer(options => {
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["jwt:Issuer"],
			// ValidAudience = builder.Configuration["jwt:Audience"],
			ValidAudiences = builder.Configuration.GetSection("jwt:Audiences").Get<string[]>(), // This works because string[] implements IEnumerable<string>,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"])),
			RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"

		};

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
/*builder.Services.AddIdentityCore<IdentityUser>()
   .AddRoles<IdentityRole>()
   .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Auth")
   .AddEntityFrameworkStores<AuthDbContext>()
   .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options => {

   options.Password.RequireDigit = false;
   options.Password.RequireLowercase = false;
   options.Password.RequireNonAlphanumeric = false;
   options.Password.RequireUppercase = false;
   options.Password.RequiredLength = 8;
   options.Password.RequiredUniqueChars = 1;

   }); */

/*builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
  .AddEntityFrameworkStores<AuthDbContext>()
   .AddDefaultTokenProviders();*/


//Adding custom fields or Applicationuser
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 8;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = true;
	options.Password.RequireLowercase = true;
})
   .AddRoles<IdentityRole>()
   .AddSignInManager<SignInManager<ApplicationUser>>() // Add SignInManager explicitly
   .AddEntityFrameworkStores<AuthDbContext>();          

// Add the full Identity service to enable UserManager and other related services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<AuthDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(builder =>
	{
		builder.AllowAnyOrigin()
			   .AllowAnyHeader()
			   .AllowAnyMethod();
	});
});
builder.Logging.AddConsole();

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
