using Microsoft.EntityFrameworkCore;
using TestTask.Data.Models;
using TestTask.Models;
using TestTask.Repository;
using TestTask.Repository.Interfaces;
using TestTask.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ApplicationDb");

builder.Services.AddDbContext<ApiDBContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<JsonSerializerCustom<Person>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
