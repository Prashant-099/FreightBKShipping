using FreightBKShipping.Data;
using FreightBKShipping.SignalR;                     // ← added
using FreightBKShipping.Interfaces;
using FreightBKShipping.Services;
using FreightBKShipping.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Security.Claims;
using System.Text;


namespace FreightBKShipping
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── CORS ── SignalR requires AllowCredentials, so AllowAnyOrigin won't work.
            // Replace your old CORS block with this one.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", policy =>
                {
                    policy
                        // ⚠️ Put your exact Blazor app URLs here (both http and https)
                        .WithOrigins(
                            "https://localhost:7226",   // ← your actual Blazor port
    "http://localhost:5171",
    "https://localhost:7001",
    "http://localhost:5001"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed(_ => true)
                        .WithExposedHeaders("Access-Control-Allow-Origin");
                });
            });

            // Add services to the container.
            builder.Services.AddControllers();

            // ✅ 2. Register AppDbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 36))));

            builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();
            builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));

            // ✅ 3. Add services
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<SentWpEmailService>();
            builder.Services.AddScoped<AuditLogService>();
            builder.Services.AddScoped<CompanySetupService>();
            builder.Services.AddScoped<ISasUrlService, SasUrlService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<ISuperAdminService, SuperAdminService>();
            builder.Services.AddScoped<ICompanySubscriptionService, CompanySubscriptionService>();
            builder.Services.AddScoped<ILrService, LrService>();
            builder.Services.AddScoped<ITicketService, TicketService>();

            // ✅ SignalR                                       // ← added
            builder.Services.AddSignalR();                     // ← added

            builder.Services.AddHttpContextAccessor();

            // ✅ 4. Add JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                        RoleClaimType = ClaimTypes.Role
                    };

                    // ── Allow SignalR to read JWT from query string ──   // ← added
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/tickethub"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };                                                    // ← added
                });

            builder.Services.AddAuthorization();

            // ✅ 5. Swagger + JWT support
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreightBKShipping API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT like: Bearer <token>"
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
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CompanyManagementOnly", policy =>
                    policy.RequireRole("SuperAdmin"));
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.UseCors("AllowAnyOrigin");                     // ← must be BEFORE UseAuthorization

            app.UseAuthentication();                           // ← must come before UseAuthorization
            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.MapControllers();
            app.MapHub<TicketHub>("/tickethub");
            app.Run();
        }
    }
}