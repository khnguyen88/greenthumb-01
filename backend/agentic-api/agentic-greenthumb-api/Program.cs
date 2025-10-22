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

builder.Services.AddDbContext<PlantInfoContext>(opt =>
    opt.UseInMemoryDatabase("PlantInfo"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AdafruitPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
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

app.UseRouting();
app.UseCors();
// app.UseCors("AdafruitPolicy");

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCookiePolicy();


app.UseAuthorization();

app.MapControllers();

app.Run();
