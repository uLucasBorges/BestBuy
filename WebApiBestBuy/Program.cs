using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApiBestBuy.Api.ExtensionServices;
using WebApiBestBuy.Domain.Interfaces;
using WebApiBestBuy.DTOS;
using WebApiBestBuy.Infra.Data;
using WebApiBestBuy.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region JwtBearer


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
     .AddJwtBearer(options =>
     {
         options.RequireHttpsMetadata = false;
         options.SaveToken = true;
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = false,
             ValidateAudience = false,
             ValidateLifetime = true,
             ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
             ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
             ValidateIssuerSigningKey = false,
             IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(builder.Configuration["Jwt:key"])),
             ClockSkew = TimeSpan.Zero
         };
     });


builder.Services.AddAuthorization(x =>
{

    x.AddPolicy("Admin", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireRole("Admin","Dev");
    });

    x.AddPolicy("Client", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireRole("Client");
    });

});



#endregion


#region Documentar o swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Api Cadastro de Alunos/Professores",
        Version = "V1",
        Contact = new OpenApiContact
        {
            Name = "Email",
            Email = "lucas.carvalho@clear.sale"
        }
    });



    // Documentar o token no swaager
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",

        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        BearerFormat = "JWT",
        Description = "Header de autorização JWT usando o esquema Bearer.\r\n\r\nInforme 'Bearer'[espaço] e o seu token.\r\n\r\nExamplo: \'Bearer 12345abcdef\'",
    });

    c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });

});

#endregion


#region Mapper
IMapper mapper = Config.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
