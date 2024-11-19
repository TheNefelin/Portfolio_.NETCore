using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_Services.Auth;
using ClassLibrary_Services.PasswordManager;
using ClassLibrary_Services.Portfolio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Text;
using WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Dependency injection filter ---------------------------------------
builder.Services.AddScoped<AuthApiKeyService>();
builder.Services.AddScoped<AuthApiKeyFilter>();

// Dependency injection ----------------------------------------------
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICoreService, CoreService>();
builder.Services.AddScoped<IPublicService, PublicService>();

builder.Services.AddScoped<IBaseCRUDService<UrlGrpDTO>, UrlGrpService>();
builder.Services.AddScoped<IBaseCRUDService<UrlDTO>, UrlService>();
builder.Services.AddScoped<IBaseCRUDService<ProjectDTO>, ProjectService>();
builder.Services.AddScoped<IBaseCRUDService<YoutubeDTO>, YoutubeService>();
builder.Services.AddScoped<IBaseCRUDService<LanguageDTO>, LanguageService>();
builder.Services.AddScoped<IBaseCRUDService<TechnologyDTO>, TechnologyService>();

builder.Services.AddScoped<ISimpleCRUDService<ProjectLanguageDTO>, ProjectLanguageService>();
builder.Services.AddScoped<ISimpleCRUDService<ProjectTechnologyDTO>, ProjectTechnologyService>();
// -------------------------------------------------------------------

// Servicio Conexion -------------------------------------------------
builder.Services.AddTransient<IDbConnection>(options =>
    new SqlConnection(builder.Configuration.GetConnectionString("RutaWebSQL"))
);
// -------------------------------------------------------------------

// Servicio JWT ------------------------------------------------------
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtOptions =>
    {
        JwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
        };
    });
// -------------------------------------------------------------------

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Modificar Servicio Swagger ----------------------------------------
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Portfolio .NETCore7", Version = "v1" });

    options.AddSecurityDefinition(
        name: JwtBearerDefaults.AuthenticationScheme,
        securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Ingrese Token Bearer",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = JwtBearerDefaults.AuthenticationScheme
        }
    );

    options.OperationFilter<SwaggerApiPadLockFilter>();
});
// -------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// swagger as Default  -----------------------------------------------
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("./swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});
// -------------------------------------------------------------------

// Cors --------------------------------------------------------------
app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowCredentials();
    options.SetIsOriginAllowed(origin => true);
});
// -------------------------------------------------------------------

app.UseHttpsRedirection();

// Usar JWT ----------------------------------------------------------
app.UseAuthentication();
// -------------------------------------------------------------------
app.UseAuthorization();

app.MapControllers();

app.Run();
