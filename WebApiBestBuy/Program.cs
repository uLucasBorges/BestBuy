using Microsoft.EntityFrameworkCore;
using WebApiBestBuy.Api.ExtensionServices;
using WebApiBestBuy.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ExtensionDI.ConfigureServices(builder.Services,builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
