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
    using Momento.Services.Contracts.Other;
    using Momento.Services.Contracts.Directory;
    using Momento.Services.Contracts.ListRemind;
    using Momento.Services.Implementations.Other;
    using Momento.Services.Implementations.Directory;
    using Momento.Services.Implementations.ListRemind;
    using Momento.Services.Contracts.Video;
    using Momento.Services.Implementations.Video;
    using Momento.Models.Users;
    using Momento.Services.Contracts.View;
    using Momento.Services.Implementations.View;
    using Momento.Web.Middleware;
    using Momento.Services.Contracts.ListToDo;
    using Momento.Services.Implementations.ListToDo;
    using Microsoft.AspNetCore.Identity;
    using Momento.Services.Contracts.Shared;
    using Momento.Services.Implementations.Shared;
    using React.AspNet;
    using System;
    using Momento.Services.Mapping;
    using Momento.Services.Models.VideoModels;
    using Momento.Services.Implementations.Comparisons;
    using Momento.Services.Contracts.Comparisons;
    using Momento.Services.Contracts.Notes;
    using Momento.Services.Implementations.Notes;
    using Momento.Services.Contracts.Utilities;
    using Momento.Services.Implementations.Utilities;

    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            this.Env = env;
        }

        public IHostingEnvironment Env { get; set; }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ///Momento.Services.Models assembly
            AutoMapperConfig.RegisterMappings(typeof(VideoCreate).Assembly);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            if (!Env.IsEnvironment("Testing"))
            {
                services.AddDbContext<MomentoDbContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection")));
            }

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

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IVideoService, VideoService>();
            services.AddTransient<IListRemindService, ListRemindService>();
            services.AddTransient<IListRemindItemService, ListRemindItemService>();
            services.AddTransient<IDirectoryService, DirectoryService>();
            services.AddTransient<IReorderingService, ReorderingService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ISaveData, SaveData>();
            services.AddTransient<IListToDoService, ListToDoService>();
            services.AddTransient<ITrackableService, TrackableService>();
            services.AddTransient<IComparisonService, ComparisonService>();
            services.AddTransient<INoteService, NoteService>();
            services.AddTransient<IUtilitiesService, UtilitiesService>();

            services.AddScoped<ILayoutViewService, LayoutViewService>();

            services.AddAutoMapper(); ///TODO: Make the automapper fancy like in the lecture

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(options =>
            {
                options.Filters.Add<AddDataToLayoutServiceActionFilter>();
                options.Filters.Add<AddDataToLayoutServicePageFilter>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddReact();

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder applicationBuilder, IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Testing"))
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
            app.UseReact(config =>
            {
                config
                    .SetReuseJavaScriptEngines(true)
                    .SetLoadBabel(false)
                    .SetLoadReact(false)
                    .AddScriptWithoutTransform("~/js/ReactApps/Navigation/components.bundle.js");
            });
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //routes.MapSpaFallbackRoute(
                //    name: "spa-fallback",
                //    defaults: new { controller = "Directory", action = "IndexReact" });
            });
        }
    }
}
