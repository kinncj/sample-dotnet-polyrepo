using Api.Configuration;
using Api.Services;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddEnvironmentVariables(s => s.Prefix = "MY_ORG__")
    .Build();

// Add services to the container.
builder.Services.Configure<Configuration>(builder.Configuration.GetSection("API"));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IWeatherForecastService<WeatherForecast>, WeatherForecastService>();
builder.Services.AddHostedService<SampleConsumer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();
await app.RunAsync();