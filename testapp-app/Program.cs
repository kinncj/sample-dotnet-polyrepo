using App.Configuration;
using App.Services;
using Shared;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddEnvironmentVariables(s => s.Prefix = "MY_ORG__")
    .Build();

builder.Services.Configure<Configuration>(builder.Configuration.GetSection("APP"));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<ISampleProducer<WeatherForecast, IEnumerable<WeatherForecast>>, SampleProducer>();
builder.Services.AddSingleton<IForecastService, ForecastService>();

// Add services to the container.
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

app.Run();