using Microsoft.EntityFrameworkCore;
using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Services;
using AgenticGreenthumbApi.Repos;
using AgenticGreenthumbApi.Client;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Agents;
using AgenticGreenthumbApi.Semantic.Plugins;
using AgenticGreenthumbApi.Semantic.Orchestrations;



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
builder.Services.AddSingleton<AgenticMemoryService>();
builder.Services.AddSingleton<KernelFactoryHelper>();
builder.Services.AddSingleton<AdafruitAPIClient>();
builder.Services.AddSingleton<AdafruitService>();
builder.Services.AddSingleton<UserChatHistoryService>();
builder.Services.AddSingleton<ChatCompletionService>();

//Should Only Persist for the lifespan of a single request
builder.Services.AddScoped<RagService>();
builder.Services.AddScoped<PlantInfoService>();
builder.Services.AddScoped<PlantInfoRepo>();
builder.Services.AddScoped<ProjectInfoPlugin>();
builder.Services.AddScoped<ProjectInfoAgentRegistry>();
builder.Services.AddScoped<AdafruitPlugin>();
builder.Services.AddScoped<AdafruitFeedAgentRegistry>();
builder.Services.AddScoped<ChatModeratorAgentRegistry>();
builder.Services.AddScoped<PlantInfoAgentRegistry>();
builder.Services.AddScoped<ChatMagenticOrchestration>();
builder.Services.AddScoped<ChatHandoffOrchestration>();

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
