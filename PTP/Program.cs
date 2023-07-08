using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PTP.Database;
using PTP.Extensions;
using PTP.Middlewares;

namespace PTP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers().AddNewtonsoftJson(options =>  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
  
            builder.Services.AddDbContext<PTPContext>(
                Options => Options.UseSqlServer(@"Data Source=localhost; Initial Catalog=PTP;TrustServerCertificate=true; Integrated Security=True;").UseExceptionProcessor());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            builder.Services.Register();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors();
            app.MapControllers();
            var loggerFactory = app.Services.GetService<ILoggerFactory>();
            loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());
            app.Run();
        }

    }
}