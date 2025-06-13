using System.Text;
using EventWebApp.Application.Interfaces;
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

namespace EventWebApp.WebAPI
{
    class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ////////////////////////////////////

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddValidatorsFromAssemblyContaining(
                typeof(CreateEventRequestValidator)
            );
            builder.Services.AddValidatorsFromAssemblyContaining(
                typeof(UpdateEventRequestValidator)
            );
            builder.Services.AddValidatorsFromAssemblyContaining(
                typeof(UserRegistrationRequestValidator)
            );
            builder.Services.AddValidatorsFromAssemblyContaining(
                typeof(RegisterUserToEventRequestValidator)
            );

            builder.Services.AddAutoMapper(typeof(MappingProfile));

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

            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
            builder.Services.AddScoped<INotificationService, EmailNotificationService>();

            builder.Services.AddScoped<ITokenService, TokenService>();

            var jwtConfig = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]!);

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

            ////////////////////////////////////

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();

            var app = builder.Build();

            ///////////////////////////////////

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

            ///////////////////////////////////

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();
            app.MapStaticAssets();
            app.MapRazorPages().WithStaticAssets();

            app.Run();
        }
    }
}
