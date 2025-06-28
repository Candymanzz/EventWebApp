using System.Text;
using EventWebApp.Application.Exceptions;
using EventWebApp.Application.Interfaces;
using EventWebApp.Core.Interfaces;
using EventWebApp.Application.Mappings;
using EventWebApp.Application.UseCases.Event;
using EventWebApp.Application.UseCases.User;
using EventWebApp.Application.Validators;
using EventWebApp.Infrastructure.Date;
using EventWebApp.Infrastructure.Repositories;
using EventWebApp.Infrastructure.Services;
using EventWebApp.Infrastructure.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using EventWebApp.Infrastructure.UnitOfWork;

namespace EventWebApp.WebAPI
{
  class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // === Database ===
      builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
      );

      // builder.Services.AddDbContextFactory<AppDbContext>(options =>
      //     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
      // );

      // === Repositories ===
      builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

      // === Validators ===
      builder.Services.AddValidatorsFromAssemblyContaining<CreateEventRequestValidator>();
      builder.Services.AddValidatorsFromAssemblyContaining<UpdateEventRequestValidator>();
      builder.Services.AddValidatorsFromAssemblyContaining<UserRegistrationRequestValidator>();
      builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserToEventRequestValidator>();

      // === AutoMapper ===
      builder.Services.AddAutoMapper(typeof(EventMappingProfile));
      builder.Services.AddAutoMapper(typeof(UserMappingProfile));

      // === Use Cases ===
      builder.Services.AddScoped<CreateEventUseCase>();
      builder.Services.AddScoped<DeleteEventUseCase>();
      builder.Services.AddScoped<FilterEventsUseCase>();
      builder.Services.AddScoped<GetAllEventsUseCase>();
      builder.Services.AddScoped<GetEventByIdUseCase>();
      builder.Services.AddScoped<GetByTitleUseCase>();
      builder.Services.AddScoped<UpdateEventUseCase>();
      builder.Services.AddScoped<UploadEventImageUseCase>();
      builder.Services.AddScoped<GetPagedEventsUseCase>();
      builder.Services.AddScoped<CancelUserFromEventUseCase>();
      builder.Services.AddScoped<GetUserByIdUseCase>();
      builder.Services.AddScoped<GetUsersByEventUseCase>();
      builder.Services.AddScoped<RegisterUserToEventUseCase>();
      builder.Services.AddScoped<RegisterUserUseCase>();
      builder.Services.AddScoped<GetUserEventsUseCase>();
      builder.Services.AddScoped<UpdateRefreshTokenUseCase>();

      // === Email Notification ===
      builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
      builder.Services.AddScoped<INotificationService, EmailNotificationService>();

      // === JWT Authentication ===
      builder.Services.AddScoped<ITokenService, TokenService>();
      builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

      var jwtConfig = builder.Configuration.GetSection("JwtSettings");
      var jwtKey = jwtConfig["Secret"];
      if (string.IsNullOrWhiteSpace(jwtKey))
      {
        throw new BadRequestException(
            "JWT Secret is not configured.",
            ErrorCodes.JwtSecretNotConfigured
        );
      }

      var key = Encoding.UTF8.GetBytes(jwtKey);

      builder
          .Services.AddAuthentication(options =>
          {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          })
          .AddJwtBearer(opt =>
          {
            opt.RequireHttpsMetadata = false;
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuer = true,
              ValidIssuer = jwtConfig["Issuer"],
              ValidateAudience = true,
              ValidAudience = jwtConfig["Audience"],
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(key),
            };
          });

      builder.Services.AddAuthorization(options =>
      {
        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
      });

      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();

      builder.Services.AddCors(options =>
      {
        options.AddPolicy(
                  "AllowFrontend",
                  policy =>
                  {
                    policy
                              .WithOrigins("http://localhost:3000")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                  }
              );
      });

      // === Controllers & Razor ===
      builder.Services.AddRazorPages();
      builder.Services.AddControllers();

      var app = builder.Build();

      // === Middleware ===
      app.UseCors("AllowFrontend");

      app.UseMiddleware<Middleware.ExceptionHandlingMiddleware>();

      app.UseStaticFiles(
          new StaticFileOptions
          {
            OnPrepareResponse = ctx =>
                  {
                    ctx.Context.Response.Headers.Append(
                              "Cache-Control",
                              "public, max-age=604800"
                          );
                  },
          }
      );

      app.UseSwagger();
      app.UseSwaggerUI();

      if (!app.Environment.IsDevelopment())
      {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.MapControllers();

      using (var scope = app.Services.CreateScope())
      {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
      }

      app.Run();
    }
  }
}
