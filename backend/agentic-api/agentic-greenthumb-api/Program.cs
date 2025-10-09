using Microsoft.EntityFrameworkCore;
using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Services;
using AgenticGreenthumbApi.Repos;
using AgenticGreenthumbApi.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Should Persists for the entirety for the lifespan of an object
builder.Services.AddSingleton<AgenticMemoryService>();
builder.Services.AddSingleton<KernelFactoryService>();
builder.Services.AddSingleton<AdafruitAPIClient>();
builder.Services.AddSingleton<AdafruitService>();

//Should Only Persist for the lifespan of a single request
builder.Services.AddScoped<ChatCompletionService>();
builder.Services.AddScoped<UserChatHistoryService>();
builder.Services.AddScoped<RagService>();
builder.Services.AddScoped<PlantInfoService>();
builder.Services.AddScoped<PlantInfoRepo>();

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
