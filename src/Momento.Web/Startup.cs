namespace Momento.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Momento.Data;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using AutoMapper;
    using Momento.Services.Contracts.CheatSheet;
    using Momento.Services.Contracts.Other;
    using Momento.Services.Contracts.Directory;
    using Momento.Services.Contracts.ListRemind;
    using Momento.Services.Implementations.CheatSheet;
    using Momento.Services.Implementations.Other;
    using Momento.Services.Implementations.Directory;
    using Momento.Services.Implementations.ListRemind;
    using Momento.Services.Contracts.Video;
    using Momento.Services.Implementations.Video;
    using Momento.Services.Contracts.Code;
    using Momento.Services.Implementations.Code;
    using Momento.Models.Users;
    using Momento.Services.Contracts.View;
    using Momento.Services.Implementations.View;
    using Momento.Web.Middleware;
    using Momento.Services.Contracts.ListToDo;
    using Momento.Services.Implementations.ListToDo;
    using Microsoft.AspNetCore.Identity;
    using Momento.Web.Utilities;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<MomentoDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDefaultIdentity<User>(options => 
            //{
            //    options.Password.RequireDigit = false;
            //    options.Password.RequiredLength = 3;
            //    options.Password.RequiredUniqueChars = 0;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireDigit = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;
            //})
            //    .AddEntityFrameworkStores<MomentoDbContext>();

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<MomentoDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddTransient<ICheatSheetService, CheatSheetService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITopicService, TopicService>();
            services.AddTransient<IPointService, PointService>();
            services.AddTransient<IVideoService, VideoService>();
            services.AddTransient<IListRemindService, ListRemindService>();
            services.AddTransient<IListRemindItemService, ListRemindItemService>();
            services.AddTransient<IDirectoryService, DirectoryService>();
            services.AddTransient<IReorderingService, ReorderingService>();
            services.AddTransient<ICodeService, CodeService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ISaveData, SaveData>();
            services.AddTransient<IListToDoService, ListToDoService>();

            services.AddScoped<ILayoutViewService, LayoutViewService>();

            services.AddAutoMapper();

            services.AddMvc(o=> {
                o.Filters.Add<AddDataToLayoutServiceActionFilter>();
                o.Filters.Add<AddDataToLayoutServicePageFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
