using Data;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;

using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddData(optionsBuilder => optionsBuilder.UseSqlite(@"Data Source=mydb.db;"));

var app = builder.Build();

app.UseLogMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();
