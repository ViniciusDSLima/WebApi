using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApplication1.Context;
using WebApplication1.Extensions;
using WebApplication1.Filters;
using WebApplication1.services;
using WebApplication1.services.implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options => 
        options.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddTransient<IMeuServico, MeuServico>();
builder.Services.AddScoped<ApiLoggerFilter>();

/*builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions
    .DefaultIgnoreCondition = JsonIgnoreCondition <condition>);*/

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(mySqlConnection,
        ServerVersion.AutoDetect(mySqlConnection)));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
