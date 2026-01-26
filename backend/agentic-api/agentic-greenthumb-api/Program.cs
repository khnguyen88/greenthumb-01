using AgenticGreenthumbApi.Client;
using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Factory;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Repos;
using AgenticGreenthumbApi.Semantic.Orchestrations;
using AgenticGreenthumbApi.Semantic.Plugins;
using AgenticGreenthumbApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;



var builder = WebApplication.CreateBuilder(args);

// Read environment variables
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var url = $"http://0.0.0.0:{port}";
var target = Environment.GetEnvironmentVariable("TARGET") ?? "World";


// Determine environment
var environment = builder.Environment.EnvironmentName;

// Load configuration based on environment
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

if (environment == "Development")
{
    builder.Configuration.AddUserSecrets<Program>(optional: true);
}

builder.Configuration.AddEnvironmentVariables();



// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Should Persists for the entirety for the lifespan of an object
builder.Services.AddScoped<AgenticMemoryService>();
builder.Services.AddScoped<KernelFactoryHelper>();
builder.Services.AddScoped<AdafruitAPIClient>();
builder.Services.AddScoped<AdafruitService>();
builder.Services.AddScoped<UserChatHistoryService>();
builder.Services.AddScoped<ChatCompletionService>();

//Should Only Persist for the lifespan of a single request
builder.Services.AddScoped<RagService>();
builder.Services.AddScoped<PlantInfoService>();
builder.Services.AddScoped<PlantInfoRepo>();
builder.Services.AddScoped<PlantInfoPlugin>();
builder.Services.AddScoped<ProjectInfoPlugin>();
builder.Services.AddScoped<AdafruitPlugin>();
builder.Services.AddSingleton<AgentRegistry>();
builder.Services.AddSingleton<KernelFactory>();
builder.Services.AddSingleton<AgentFactory>();

//Initialize Static Class
KernelFactoryHelper.Initialize(builder.Configuration);


builder.Services.AddDbContext<PlantInfoContext>(opt =>
    opt.UseInMemoryDatabase("PlantInfo"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AdafruitPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://nguyen-iot-prototype.uc.r.appspot.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
        });

    options.AddPolicy("ChatPolicy",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://nguyen-iot-prototype.uc.r.appspot.com")
          .AllowAnyHeader()
          .AllowAnyMethod(); 
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors();
// app.UseCors("AdafruitPolicy");

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCookiePolicy();


app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello from Cloud Run!");

//For deployment
//app.Run(url);

//For Local run
app.Run();
