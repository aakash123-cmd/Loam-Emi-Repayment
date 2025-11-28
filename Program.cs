
using Loan___Emi_Repayment.DATAACCESS.ApplicationDbContext;
using Loan___Emi_Repayment.DATAACCESS.IRepository;
using Loan___Emi_Repayment.DATAACCESS.Repository;
using Loan___Emi_Repayment.Middleware;
using Microsoft.EntityFrameworkCore;

namespace Loan___Emi_Repayment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ------------ Database ------------
            builder.Services.AddDbContext<ApplicationDBContext>(o =>
            {
                var cs = builder.Configuration.GetConnectionString("Default");
                o.UseMySql(cs, ServerVersion.AutoDetect(cs));
            });
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ILoanService, LoanService>();
            builder.Services.AddScoped<IEnquiryService, EnquiryService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            var app = builder.Build();


            app.UseMiddleware<ExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
