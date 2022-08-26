using CItyInfo.API.Contract;
using CItyInfo.API.DbContexts;
using CItyInfo.API.Model;
using CItyInfo.API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers( options =>
{
    //options.OutputFormatters.Add(IOutputFormatter, "application/json");
    options.ReturnHttpNotAcceptable = false;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters(); //For Accepting XML Format Values

builder.Services.AddDbContext<CityInfoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen( options =>
{
    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);

    options.IncludeXmlComments(xmlCommentPath);

    options.AddSecurityDefinition("CityInfoApiBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a Valid token to access this API"

    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "CityInfoApiBearerAuth"
                }
            },new List<string>()
        }

    });

    
});

builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddSingleton<CitiesDataStore>();
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication("Bearer").AddJwtBearer(option =>
{
    //bearer.Audience = builder.Configuration["Authetication:Audience"];
    //bearer.Authority = builder.Configuration["Authetication:Issuer"];
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authetication:Issuer"],
        ValidAudience = builder.Configuration["Authetication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Authetication:SecretForKey"]))
    };
    
});

builder.Services.AddApiVersioning(setup =>
{
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.ReportApiVersions = true;
});


#if DEBUG
builder.Services.AddTransient<ILocalMailService , LocalMailService>();
#else
builder.Services.AddTransient<ILocalMailService , CloudMailService>();
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
