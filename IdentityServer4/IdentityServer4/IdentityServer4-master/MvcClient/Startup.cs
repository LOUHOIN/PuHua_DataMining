using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient
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
            services.AddControllersWithViews();
            // 关闭身份信息类型映射,把部分属性映射成命名空间，影响信息交互
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            // 添加认证服务
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies"; // 默认认证方案名称，把令牌保存在cookies，通过cookies传递令牌
                options.DefaultChallengeScheme = "oldc"; // 咨询方案，MVC与IDS交互使用的协议
            })
                    .AddCookie("Cookies")
                    .AddOpenIdConnect("oidc", options =>
                    {
                        options.Authority = "https://localhost:5001";
                        options.ClientId = "Client_mvc";
                        options.ClientSecret = "Secret_mvc";
                        options.ResponseType = "code"; //授权码流程
                        options.SaveTokens = true; //令牌保存到cookies
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // 添加认证中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
