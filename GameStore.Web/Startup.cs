using GameStore.Domain.Abstract;
using GameStore.Domain.Concrete.ContextMongoDB;
using GameStore.Web.Infrastructure;
using GameStore.Web.Middlewares.Extensions;
using GameStore.Web.Services.Abstract;
using GameStore.Web.Services.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.AddDbContext<EFDbContext>(options => options.UseSqlServer
            //     (Configuration.GetSection("ConnectionStrings").GetValue<string>("EFDbContext")
            //));

            services.AddScoped<MongoContext>(s =>
            {
                var connectionString = Configuration.GetSection("ConnectionStrings").GetValue<string>("MongoDbContext");
                return new MongoContext(connectionString);

            });

            //services.AddScoped<IGameRepository, EFGameRepository>();
            services.AddScoped<IGameRepository, MongoGameRepository>();
            services.AddScoped<IOrderProcessor>(s =>
            {
                EmailSettings emailSettings = new EmailSettings
                {
                    WriteAsFile = Configuration.GetSection("Email").GetValue<bool>("WriteAsFile")
                };
                return new EmailOrderProcessor(emailSettings, s.GetService<ICartProvider>(), s.GetService<IHostingEnvironment>());
            });
            services.AddScoped<IAuthProvider, CookieAuthProvider>();

            services.AddCookieCartProvider();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.Cookie.Name = "GameStore.Auth";
            });
            services.AddHttpContextAccessor();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseCookieCartProvider();
            app.UseMvc(RouteCollection.BuildRouter);
        }
    }
}
