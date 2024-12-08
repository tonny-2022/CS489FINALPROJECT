using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.mappings;
using UserService.repository;
using UserService.service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserDbContext>(options=>
                 options.UseNpgsql(builder.Configuration.GetConnectionString("UserServiceConnectionString")));
builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();
builder.Services.AddAutoMapper(typeof(UserMapper));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
