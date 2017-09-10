using AdminLTE.Demo.Domain.IRepositories;
using AdminLTE.Demo.Filters;
using AdminLTE.Demo.Repositories.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace AdminLTE.Demo
{
	public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            //appsetting
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();


        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //获取数据库连接字符串
            var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");

            //添加数据上下文
            services.AddDbContext<DefaultDbContext>(options =>
            options.UseSqlServer(sqlConnectionString),ServiceLifetime.Singleton);

			//仓储
			services.AddScoped<IUserRepository, UserRepository>();

			//登录拦截服务
			services.AddScoped<LoginActionFilter>();

            //Session服务
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //使用静态文件
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory())
            });

            //Session
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
    
            });
            SeedData.Initialize(app.ApplicationServices); //初始化数据

        }
    }
}
