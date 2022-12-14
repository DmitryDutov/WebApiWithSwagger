//using Serilog;

using WebApiWithSwagger.Models;

var builder = WebApplication.CreateBuilder(args);

//????????? ???????????
//builder.Logging.ClearProviders();                          //??????? ??????????
//Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger(); //??????? ?????

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Settings.Init();
app.UseAuthorization();
app.MapControllers();
app.Run();


