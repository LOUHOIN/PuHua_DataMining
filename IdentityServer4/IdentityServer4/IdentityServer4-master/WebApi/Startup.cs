using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // 添加解决方案为“Bearer”的认证服务
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer",options =>
                {
                    options.Authority = "https://localhost:5001";//IDS的服务地址
                    // 设置验证参数，不验证Aud
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            // 授权策略
            services.AddAuthorization(option =>
            {
                option.AddPolicy("Api_Scope", builder =>
                {
                    // 鉴定用户
                    builder.RequireAuthenticatedUser();//是否通过认证
                    builder.RequireClaim("scope","api_1");
                });
            });
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
     
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();//认证

            app.UseAuthorization();//授权

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
