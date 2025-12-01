 
using Loan___Emi_Repayment.DATAACCESS.ApplicationDbContext;
using Loan___Emi_Repayment.DATAACCESS.IRepository;
using Loan___Emi_Repayment.DATAACCESS.Repository;
using Loan___Emi_Repayment.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace Loan___Emi_Repayment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ------------------- CONTROLLERS -------------------
            builder.Services.AddControllers();





            // ------------------- SWAGGER -------------------


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Loan & EMI Repayment API",
                    Version = "v1"
                });





                //----------------JWT SECURITY IN SWAGGER-----------------------
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter token like: **Bearer your_token_here**"
                });




                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                      new OpenApiSecurityScheme
                {
                      Reference = new OpenApiReference
                {
                       Type = ReferenceType.SecurityScheme,
                       Id = "Bearer"
                }
            },
                       new string[]{}
                }
       });
                 });







            // ------------ Database ------------
            builder.Services.AddDbContext<ApplicationDBContext>(o =>
            {
                var cs = builder.Configuration.GetConnectionString("Default");
                o.UseMySql(cs, ServerVersion.AutoDetect(cs));
            });







            // ------------------- DEPENDENCY INJECTION -------------------



            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ILoanService, LoanService>();
            builder.Services.AddScoped<IEnquiryService, EnquiryService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();







            // ------------------- JWT CONFIG -------------------

            builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
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
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        ),

        RoleClaimType = ClaimTypes.Role
    };
});

            builder.Services.AddAuthorization();
            var app = builder.Build();







            // ------------------- MIDDLEWARE -------------------
            
            app.UseMiddleware<ExceptionMiddleware>();
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
        }
    }
}
