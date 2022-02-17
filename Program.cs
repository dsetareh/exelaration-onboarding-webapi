using Microsoft.EntityFrameworkCore;
using CountryApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<CountryContext>(opt =>
    opt.UseInMemoryDatabase("CountryList"));
builder.Services.AddDbContext<StatesContext>(opt =>
    opt.UseInMemoryDatabase("StatesList"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();