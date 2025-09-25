using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // Add this using directive 
using WPM.Management.Api.DataAccess; // Add this using directive 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Api", Version = "v1" }); });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ManagementDbContext>(options => { options.UseInMemoryDatabase("WpmManagement"); });

var app = builder.Build();
app.EnsureDbIsCreated();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
