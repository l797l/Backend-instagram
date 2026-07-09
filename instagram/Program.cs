
using instagram.DB;
using instagram.DB.Moduls;
using instagram.DTO;
using instagram.Mail;
using instagram.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace instagram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddIdentityCore<User>(op =>
            {
                op.User.RequireUniqueEmail = true;
                op.SignIn.RequireConfirmedEmail = true;

            })
                 .AddRoles<IdentityRole>()
                 .AddEntityFrameworkStores<AppDBContext>()
                 .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(op =>
                {
                    op.SaveToken = true;
                    op.RequireHttpsMetadata = false;
                    op.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration["JWT:Key"]

                                )
                            )
                    };
                });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddMemoryCache();
            builder.Services.AddAuthorization();

            builder.Services.Configure<ImageKitSettings>(builder.Configuration.GetSection("ImageKitSettings"));
           
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IImageService, ImageService>();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("ReactPolicy");
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
