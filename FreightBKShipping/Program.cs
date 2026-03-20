using FreightBKShipping.Data;
using FreightBKShipping.SignalR;
using FreightBKShipping.Interfaces;
using FreightBKShipping.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sieve.Models;
using Sieve.Services;
using System.Security.Claims;
using System.Text;

namespace FreightBKShipping
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── CORS ──────────────────────────────────────────────────────────
            // SignalR requires AllowCredentials, so AllowAnyOrigin is not allowed.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", policy =>
                {
                    policy
                        .WithOrigins(
                            "https://localhost:7226",
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

            builder.Services.AddControllers();

            // ── Database ──────────────────────────────────────────────────────
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 36))));

            builder.Services.AddScoped<ISieveProcessor, SieveProcessor>();
            builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));

            // ── Application services ──────────────────────────────────────────
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

            // ── SignalR ───────────────────────────────────────────────────────
            builder.Services.AddSignalR();

            builder.Services.AddHttpContextAccessor();

            // ── JWT Authentication ─────────────────────────────────────────────
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

                    // Allow SignalR to read JWT from query string (required for WS transport)
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
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CompanyManagementOnly", policy =>
                    policy.RequireRole("SuperAdmin"));
            });

            // ✅ Removed duplicate: AddEndpointsApiExplorer and AddSwaggerGen
            // were each registered twice in the original — once is enough.
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
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/error");

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            // Order matters: CORS → Authentication → Authorization
            app.UseCors("AllowAnyOrigin");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.MapControllers();
            app.MapHub<TicketHub>("/tickethub");

            app.Run();
        }
    }
}