using JobCandidateHub.Managers;
using JobCandidateHub.Models;
using JobCandidateHub.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Retrieve the CSV file path from configuration
var csvFilePath = builder.Configuration.GetValue<string>("CsvFilePath");

builder.Services.AddScoped<ICsvFileService>(provider => new CsvFileService(Path.Combine(Directory.GetCurrentDirectory(), csvFilePath)));
builder.Services.AddScoped<ICandidateDetailsManager, CandidateDetailsManager>();


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
