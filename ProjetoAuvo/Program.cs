using Amazon.DynamoDBv2;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProjetoAuvo.Services.Interface;
using ProjetoAuvo.Services;
using ProjetoAuvo.Repository.Interface;
using ProjetoAuvo.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetoAuvo.Data.Config;
using AutoMapper;
using System.Reflection;
using Amazon.CloudWatchLogs;
using Amazon;
using Amazon.CloudWatchLogs.Model;
using Amazon.CloudWatch.Model;
using Amazon.CloudWatch;
using Amazon.Runtime;
using System.Net.Sockets;
using System.Net; 

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Logging.AddAWSProvider(builder.Configuration.GetAWSLoggingConfigSection());

var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Credentials = new BasicAWSCredentials("test", "test");
awsOptions.DefaultClientConfig.ServiceURL = "http://localhost:4566";

builder.Services.AddDefaultAWSOptions(awsOptions);

var cwCredentials = new BasicAWSCredentials("test", "test");

var cwClient = new AmazonCloudWatchClient(cwCredentials, new AmazonCloudWatchConfig
{
    ServiceURL = "http://localhost:4566",
    RegionEndpoint = RegionEndpoint.USEast1
});

builder.Services.AddControllers();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<AlvoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AlvoContextConnection"))
);

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AlvoContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "ProjetoAuvo",
            ValidAudience = "ProjetoAuvoUser",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30"))
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Projeto Auvo", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira o token JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddTransient<IPaisRepository, PaisRepository>();

builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Projeto Auvo v1");
});


app.MapGet("/metric", async () =>
{
    var metricDatum = new MetricDatum
    {
        MetricName = "PaginaInicialAcessada",
        Unit = Amazon.CloudWatch.StandardUnit.Count,
        Value = 1,
        Dimensions = new List<Dimension>
        {
            new Dimension
            {
                Name = "Aplicacao",
                Value = "ProjetoAuvo"
            }
        }
    };

    var request = new PutMetricDataRequest
    {
        Namespace = "ProjetoAuvo/MetricaCustomizada",
        MetricData = new List<MetricDatum> { metricDatum }
    };

    await cwClient.PutMetricDataAsync(request);

    return Results.Ok("Métrica enviada com sucesso!");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
