using Admin.Portal.API.Core.Models.Base;
using Admin.Portal.API.Extentions;
using Admin.Portal.API.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<Settings>(builder.Configuration.GetSection("Settings"));
builder.Services.AddDbContext<DataContext>(options =>
{ 
 options.UseSqlite(builder.Configuration.GetValue<string>("Settings:DataAccess:ConnectionString"));
 options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
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

app.UseEnableRequestRewindMiddleware();

app.UseExceptionMiddleware();

app.Run();
