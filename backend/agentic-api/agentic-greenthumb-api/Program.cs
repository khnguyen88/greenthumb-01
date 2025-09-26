using Microsoft.EntityFrameworkCore;
using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ChatCompletionService>();
builder.Services.AddSingleton<AgenticMemoryService>();
builder.Services.AddSingleton<RagService>();
builder.Services.AddSingleton<PlantInfoService>();

builder.Services.AddDbContext<PlantInfoContext>(opt =>
    opt.UseInMemoryDatabase("PlantInfo"));

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
