namespace Momento.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using AutoMapper;
    using Services.Contracts.Other;
    using Services.Contracts.Directory;
    using Services.Implementations.Other;
    using Services.Implementations.Directory;
    using Services.Contracts.Video;
    using Services.Implementations.Video;
    using Momento.Models.Users;
    using Services.Contracts.View;
    using Services.Implementations.View;
    using Middleware;
    using Services.Contracts.ListToDo;
    using Services.Implementations.ListToDo;
    using Microsoft.AspNetCore.Identity;
    using Services.Contracts.Shared;
    using Services.Implementations.Shared;
    using React.AspNet;
    using System;
    using Services.Mapping;
    using Momento.Services.Models.VideoModels;
    using Services.Implementations.Comparisons;
    using Services.Contracts.Comparisons;
    using Services.Contracts.Notes;
    using Services.Implementations.Notes;
    using Services.Contracts.Utilities;
    using Services.Implementations.Utilities;
    using Services.Implementations.Admin;
    using Services.Contracts.Admin;

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
            services.AddTransient<IDirectoryService, DirectoryService>();
            services.AddTransient<IReorderingService, ReorderingService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IListToDoService, ListToDoService>();
            services.AddTransient<ITrackableService, TrackableService>();
            services.AddTransient<IComparisonService, ComparisonService>();
            services.AddTransient<INoteService, NoteService>();
            services.AddTransient<IUtilitiesService, UtilitiesService>();
            services.AddTransient<IAdminService, AdminService>();

            services.AddScoped<ILayoutViewService, LayoutViewService>();

            services.AddAutoMapper();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(options =>
            {
                options.Filters.Add<AddDataToLayoutServiceActionFilter>();
                options.Filters.Add<AddDataToLayoutServicePageFilter>();
                //options.Filters.Add<ValidateModelStateAttribute>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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

                routes.MapSpaFallbackRoute(
                    name: "SPA-fallback",
                    defaults: new { controller = "Directory", action = "IndexReact" });
            });
        }
    }
}
