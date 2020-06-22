using CinderellaCore.Data.Context;
using CinderellaCore.Data.Repositories;
using CinderellaCore.Model.Models;
using CinderellaCore.Services.Services;
using CinderellaCore.Services.Services.Interfaces;
using CinderellaCore.Services.Services.Statistics;
using CinderellaCore.Web.Authorization;
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
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
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

            services.AddDbContext<CinderellaCoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<CinderellaCoreContext>()
                .AddDefaultTokenProviders();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()
                        .Build());
            });
            //services.AddAuthentication(options =>
            //    {
            //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(options =>
            //    {
            //        options.Audience = Configuration["GlobalSettings:Issuer"];
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = false,
            //            ValidateAudience = false,
            //            ValidateLifetime = false,
            //            ValidateIssuerSigningKey = false,
            //            ValidIssuer = Configuration["GlobalSettings:Issuer"],
            //            ValidAudience = Configuration["GlobalSettings:Issuer"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["GlobalSettings:JwtKey"]))
            //        };
            //    });

            services.AddSession();
            services.AddDetection();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest).AddNewtonsoftJson();
            services.AddHttpContextAccessor();
            services.AddSimpleInjector(_container, options =>
                {
                    // AddAspNetCore() wraps web requests in a Simple Injector scope.
                    options.AddAspNetCore()
                        // Ensure activation of a specific framework type to be created by
                        // Simple Injector instead of the built-in configuration system.
                        .AddControllerActivation()
                        .AddViewComponentActivation()
                        .AddPageModelActivation()
                        .AddTagHelperActivation();

                    options.AutoCrossWireFrameworkComponents = true;
                }
            );

            IntegrateSimpleInjector(services);

            services.AddAuthorization(x =>
            {
                x.AddPolicy("Api", policy => policy.Requirements.Add(new ApiRequirement()));
            });
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            // _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(_container));
            services.AddSingleton<IAuthorizationHandler>(new ApiAuthorizationHandler(Configuration.GetSection("GlobalSettings").Get<GlobalSettings>()));
            services.UseSimpleInjectorAspNetRequestScoping(_container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitializeContainer(app);
            app.UseSimpleInjector(_container);
            _container.Verify();

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
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseCookiePolicy();
            app.UseSimpleInjector(_container);
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
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
            _container.Register<IImportService, ImportService>(Lifestyle.Scoped);
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