using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LoginCodeCase.Model;
using LoginCodeCase.Interfaces;
using LoginCodeCase.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Text;
using LoginCodeCase.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace LoginCodeCase
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
            services.AddCors();
            //Automapper injection nuget i kurulduktan sonra tan�mlanabilir.
            services.AddAutoMapper();
            //CodeFirst yakla��m� ile olu�turulan DBContext s�n�f� arac�l��� ile TestDb ad�nda memoryde db olu�turuluyor.
            services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("TestDb"));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //routing i�lemleri
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/UserForms", "");
                //form componentlar�n�n OnGet'lerini ayr��t�rmaya yarayan parametre {handler}
                options.Conventions.AddPageRoute("/UserForms", "forms/{handler}");
                options.Conventions.AddPageRoute("/Register", "new-user");
                options.Conventions.AddPageRoute("/UserDetails", "user-details/{id}");
            });
            //{handler}'a eri�im izni...
            services.AddMvc().AddRazorPagesOptions(options =>
                {
                    options.AllowMappingHeadRequestsToGetHandler = true;
                });
            services.AddDistributedMemoryCache();
            //Kullan�c� login stateini ve user id sini tuttu�umuz Session'�n startup tan�mlamas�
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            //IUser soyut s�n�f�n�n servis tan�mlamas� bilgisi.
            services.AddScoped<IUser, UserService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //configure jwt auth
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            //AKT�F ED�LMED�
            //var appSettings = appSettingsSection.Get<AppSettings>();
            //var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(x =>
            //{
            //    x.Events = new JwtBearerEvents
            //    {
            //        OnTokenValidated = context =>
            //        {
            //            var userService = context.HttpContext.RequestServices.GetRequiredService<IUser>();
            //            if (context.Principal.Identity.Name != null)
            //            {
            //                var userId = int.Parse(context.Principal.Identity.Name);
            //                var user = userService.GetByID(userId);
            //                if (user == null)
            //                {
            //                    context.Fail("Unauthorized");
            //                }
            //            }
            //            return Task.CompletedTask;
            //        }
            //    };
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //session kullan�m�
            app.UseSession();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
