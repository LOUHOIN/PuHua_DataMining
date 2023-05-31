using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // 注册MVC服务
            services.AddControllersWithViews();
            // 注册Identity Server 4 的服务
            var builder = services.AddIdentityServer();
            builder.AddDeveloperSigningCredential();// 在启动应用的时候为开发者提供一个临时密钥，以文件形式保存
            // 保存在内存中，之后改成DB
            builder.AddInMemoryApiScopes(Config.ApiScopes)
                   .AddInMemoryClients(Config.Clients)
                   .AddTestUsers(Config.GetUsers)
                   .AddInMemoryIdentityResources(Config.IdentityResources);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //MVC
            app.UseStaticFiles(); // 静态资源中间件
            app.UseRouting(); // 注册路由中间件

            app.UseIdentityServer();
            app.UseAuthorization(); //注册授权中间件

            // 注册端点中间件
            app.UseEndpoints(enptoints =>
            {
                enptoints.MapDefaultControllerRoute(); // 默认控制器路由
            });
        }
    }
}
