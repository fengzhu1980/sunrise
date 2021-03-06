using API.Extensions;
using API.Helpers;
using API.Middleware;
using Core.DataModels.Models;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();

            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
            _configuration.Bind("Token", authenticationConfiguration);
            services.AddSingleton(authenticationConfiguration);

            WebConfiguration webConfiguration = new WebConfiguration();
            _configuration.Bind("WebConfig", webConfiguration);
            services.AddSingleton(webConfiguration);

            var sunriseConnectionString = _configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SunriseContext>(x => x.UseMySql(sunriseConnectionString, ServerVersion.AutoDetect(sunriseConnectionString)));
            var identityConnectionString = _configuration.GetConnectionString("IdentityConnection");
            services.AddDbContext<AppIdentityDbContext>(x =>
            {
                x.UseMySql(identityConnectionString, ServerVersion.AutoDetect(identityConnectionString), t => t.EnableStringComparisonTranslations());
            });

            services.AddApplicationServices();
            services.AddIdentityServices(_configuration);
            services.AddSwaggerDocumentation();
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    // policy.AllowAnyHeader().AllowAnyMethod();
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:9527");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
