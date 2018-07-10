using CinderellaCore.Data.Repositories;
using CinderellaCore.Model.Models;
using CinderellaCore.Services.Services;
using CinderellaCore.Services.Services.Interfaces;
using CinderellaCore.Services.Services.Statistics;
using CinderellaCore.Web.Authorization;
using CinderellaCore.Web.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;

namespace CinderellaCore.Web
{
    public class Startup
    {
        private readonly Container _container;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _container = new Container();
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

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddDetection();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSession();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            IntegrateSimpleInjector(services);
            services.AddAuthorization(x => x.AddPolicy("Import", policy => policy.Requirements.Add(new ImportRequirement())));
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(_container));
            services.AddSingleton<IAuthorizationHandler>(new ImportAuthorizationHandler(Configuration.GetSection("GlobalSettings").Get<GlobalSettings>()));
            services.EnableSimpleInjectorCrossWiring(_container);
            services.UseSimpleInjectorAspNetRequestScoping(_container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app);
            _container.Verify();
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
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            _container.RegisterMvcControllers(app);
            _container.RegisterMvcViewComponents(app);

            _container.Register(GetAspNetServiceProvider<UserManager<ApplicationUser>>(app), Lifestyle.Scoped);
            _container.Register(GetAspNetServiceProvider<SignInManager<ApplicationUser>>(app), Lifestyle.Scoped);

            // Add application services. For instance:
            _container.Register<GlobalSettings>(() => Configuration.GetSection("GlobalSettings").Get<GlobalSettings>(), Lifestyle.Singleton);
            _container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Singleton);
            _container.Register<IAlbumService>(() => new AlbumService(_container.GetInstance<IUnitOfWork>(), _container.GetInstance<ApplicationUser>()), Lifestyle.Scoped);
            _container.Register<IBookService>(() => new BookService(_container.GetInstance<IUnitOfWork>(), _container.GetInstance<ApplicationUser>()), Lifestyle.Scoped);
            _container.Register<IMovieService>(() => new MovieService(_container.GetInstance<IUnitOfWork>(), _container.GetInstance<ApplicationUser>()), Lifestyle.Scoped);
            _container.Register<IGameService>(() => new GameService(_container.GetInstance<IUnitOfWork>(), _container.GetInstance<ApplicationUser>()), Lifestyle.Scoped);
            _container.Register<IPopService>(() => new PopService(_container.GetInstance<IUnitOfWork>(), _container.GetInstance<ApplicationUser>()), Lifestyle.Scoped);
            _container.Register<IWishService>(() => new WishService(_container.GetInstance<IUnitOfWork>(), _container.GetInstance<ApplicationUser>()), Lifestyle.Scoped);
            _container.Register<IDiscogsService, DiscogsService>(Lifestyle.Scoped);
            _container.Register<IClientService>(() => new Google.Apis.Books.v1.BooksService(), Lifestyle.Scoped);
            _container.Register<IGoogleBookService>(() => new GoogleBookService(_container.GetInstance<IClientService>()), Lifestyle.Scoped);
            _container.Register<ITMDBService, TMDBService>(Lifestyle.Scoped);
            _container.Register<IBGGService, BGGService>(Lifestyle.Scoped);
            _container.Register<IComicVineService, ComicVineService>(Lifestyle.Scoped);
            _container.Register<IGiantBombService, GiantBombService>(Lifestyle.Scoped);
            _container.Register<IStatisticService>(() => new StatisticService(_container.GetInstance<IAlbumService>(), _container.GetInstance<IBookService>(), _container.GetInstance<IGameService>(), _container.GetInstance<IMovieService>(), _container.GetInstance<IPopService>(), _container.GetInstance<IWishService>()), Lifestyle.Scoped);
            _container.Register<IAlbumStatisticService, AlbumStatisticService>(Lifestyle.Scoped);
            _container.Register<IBookStatisticService, BookStatisticService>(Lifestyle.Scoped);
            _container.Register<IMovieStatisticService, MovieStatisticService>(Lifestyle.Scoped);
            _container.Register<IGameStatisticService, GameStatisticService>(Lifestyle.Scoped);
            _container.Register<IPopStatisticService, PopStatisticService>(Lifestyle.Scoped);

            // Allow Simple Injector to resolve services from ASP.NET Core.
            _container.AutoCrossWireAspNetComponents(app);
        }

        private Func<T> GetAspNetServiceProvider<T>(IApplicationBuilder app)
        {
            var appServices = app.ApplicationServices;
            var accessor = appServices.GetRequiredService<IHttpContextAccessor>();
            return () =>
            {
                var services = accessor.HttpContext != null
                    ? accessor.HttpContext.RequestServices
                    : _container.IsVerifying ? appServices : null;
                if (services == null) throw new InvalidOperationException("No HttpContext");
                return services.GetRequiredService<T>();
            };
        }
    }
}