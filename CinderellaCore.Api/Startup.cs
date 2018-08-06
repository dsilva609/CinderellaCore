using CinderellaCore.Data.Repositories;
using CinderellaCore.Model.Models;
using CinderellaCore.Services.Services;
using CinderellaCore.Services.Services.Interfaces;
using CinderellaCore.Services.Services.Statistics;
using Google.Apis.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

namespace CinderellaCore.Api
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
            IntegrateSimpleInjector(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app);
            _container.Verify();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(_container));
            //services.AddSingleton<IAuthorizationHandler>(new ImportAuthorizationHandler(Configuration.GetSection("GlobalSettings").Get<GlobalSettings>()));
            services.EnableSimpleInjectorCrossWiring(_container);
            services.UseSimpleInjectorAspNetRequestScoping(_container);
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            _container.RegisterMvcControllers(app);
            _container.RegisterMvcViewComponents(app);

            //   _container.Register(GetAspNetServiceProvider<UserManager<ApplicationUser>>(app), Lifestyle.Scoped);
            //   _container.Register(GetAspNetServiceProvider<SignInManager<ApplicationUser>>(app), Lifestyle.Scoped);

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
            //      _container.Register<IImportService, ImportService>(Lifestyle.Scoped);

            // Allow Simple Injector to resolve services from ASP.NET Core.
            _container.AutoCrossWireAspNetComponents(app);
        }
    }
}