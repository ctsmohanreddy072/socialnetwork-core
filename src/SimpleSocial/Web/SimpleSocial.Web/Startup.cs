﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleSocia.Services.Models.Account;
using SimpleSocia.Services.Models.Comments;
using SimpleSocial.Data;
using SimpleSocial.Data.Common;
using SimpleSocial.Data.Models;
using SimpleSocial.Services.DataServices.Account;
using SimpleSocial.Services.DataServices.CommentsServices;
using SimpleSocial.Services.DataServices.FollowersDataServices;
using SimpleSocial.Services.DataServices.LikesDataServices;
using SimpleSocial.Services.DataServices.PostsServices;
using SimpleSocial.Services.DataServices.ReportsDataServices;
using SimpleSocial.Services.DataServices.SearchDataServices;
using SimpleSocial.Services.DataServices.SignUpDetails;
using SimpleSocial.Services.DataServices.UsersDataServices;
using SimpleSocial.Services.Mapping;
using SimpleSocial.Web.Areas.Administration.Services;
using SimpleSocial.Web.Middlewares;
using SimpleSocial.Web.Utilities;


namespace SimpleSocial.Web
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
            AutoMapperConfig.RegisterMappings(
                typeof(CreatePostInputModel).Assembly,
                typeof(MyProfileViewModel).Assembly,
                typeof(CommentInputModel).Assembly
                );

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddDbContext<SimpleSocialContext>(options =>
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("DefaultConnection")));
     
            services.AddIdentity<SimpleSocialUser,IdentityRole>(
                    options =>
                    {
                        options.Password.RequiredLength = 6;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireDigit = false;
                    })
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<SimpleSocialContext>();

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Admin");
                    });
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddSessionStateTempDataProvider();

            services.AddSession();

            //Application services

            services.AddScoped(typeof(IRepository<>), typeof(DbRepository<>));
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<IMyProfileServices, MyProfileServices>();
            services.AddScoped<IPostServices, PostServices>();
            services.AddScoped<ICommentsServices, CommentsServices>();
            services.AddScoped<IProfileDetailsServices, ProfileDetailsServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddTransient<ILikesServices, LikesServices>();
            services.AddScoped<IFollowersServices, FollowersServices>();
            services.AddScoped<ISearchServices,SearchServices>();
            services.AddScoped<IReportsService, ReportsService>();
            services.AddScoped<IAdministrationServices, AdministrationServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseSession();

            app.UseAuthentication();

            app.UseMiddleware<SeedRolesMiddleware>();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
           
        }
    }
}
