using CinderellaCore.Model.Models;
using CinderellaCore.Services.Features.Book;
using CinderellaCore.Services.Features.Statistics;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CinderellaCore.Web.Controllers
{
    public class StatisticsController : CinderellaCoreBaseController
    {
        private readonly IStatisticService _statisticService;
        private readonly IAlbumStatisticService _albumStatisticService;
        private readonly IBookStatisticService _bookStatisticService;
        private readonly IMovieStatisticService _movieStatisticService;
        private readonly IGameStatisticService _gameStatisticService;
        private readonly IPopStatisticService _popStatisticService;
        private const int NUM_OF_TOP_TO_TAKE = 10;

        public StatisticsController(UserManager<ApplicationUser> userManager, IStatisticService statisticService, IAlbumStatisticService albumStatisticService,
            IBookStatisticService bookStatisticService, IMovieStatisticService movieStatisticService, IGameStatisticService gameStatisticService,
            IPopStatisticService popStatisticService) : base(userManager)
        {
            _statisticService = statisticService;
            _albumStatisticService = albumStatisticService;
            _bookStatisticService = bookStatisticService;
            _movieStatisticService = movieStatisticService;
            _gameStatisticService = gameStatisticService;
            _popStatisticService = popStatisticService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var universalStats = new StatsViewModel
            {
                Title = "Global",
                CollectionCount = _statisticService.GetCollectionCount(),
                Condition = new Condition
                {
                    Title = "Global Item Conditions",
                    ID = "global-item-conditions",
                    NumNew = _statisticService.GetNumNew(),
                    NumUsed = _statisticService.GetNumUsed()
                },
                MediaTypes = new MediaTypes
                {
                    Title = "Global Media Types",
                    ID = "global-media-types",
                    NumPhysical = _statisticService.GetNumPhysical(),
                    NumDigital = _statisticService.GetNumDigital()
                },
                TimesCompleted = _statisticService.GetTimesCompleted(),
                CompletionStatus = new CompletionStatus
                {
                    Title = "Global Completion Status",
                    ID = "global-completion-status",
                    NumCompleted = _statisticService.GetNumCompleted(),
                    NumInProgress = _statisticService.GetNumInProgress(),
                    NumNotStarted = _statisticService.GetNumNotStarted(),
                },
                NumCheckedOut = _statisticService.GetNumCheckedOut(),
                ItemTypes = new ItemTypes
                {
                    Title = "Global Item Types",
                    ID = "global-item-types",
                    NumAlbums = _statisticService.GetNumAlbums(),
                    NumBooks = _statisticService.GetNumBooks(),
                    NumMoviesAndShows = _statisticService.GetNumMoviesShows(),
                    NumGames = _statisticService.GetNumGames(),
                    NumPops = _statisticService.GetNumPops(),
                    NumWishes = _statisticService.GetNumWishes(),
                    NumShowcased = _statisticService.GetNumShowcased()
                }
            };

            var userID = _user.Id;
            var userStats = string.IsNullOrWhiteSpace(userID)
                ? null
                : new StatsViewModel
                {
                    Title = "User",
                    CollectionCount = _statisticService.GetCollectionCount(userID),
                    Condition = new Condition
                    {
                        Title = "User Item Conditions",
                        ID = "user-item-conditions",
                        NumNew = _statisticService.GetNumNew(userID),
                        NumUsed = _statisticService.GetNumUsed(userID)
                    },
                    MediaTypes = new MediaTypes
                    {
                        Title = "User Media Types",
                        ID = "user-media-types",
                        NumPhysical = _statisticService.GetNumPhysical(userID),
                        NumDigital = _statisticService.GetNumDigital(userID)
                    },
                    TimesCompleted = _statisticService.GetTimesCompleted(userID),
                    CompletionStatus = new CompletionStatus
                    {
                        Title = "User Completion Status",
                        ID = "user-completion-status",
                        NumCompleted = _statisticService.GetNumCompleted(userID),
                        NumInProgress = _statisticService.GetNumInProgress(userID),
                        NumNotStarted = _statisticService.GetNumNotStarted(userID)
                    },
                    NumCheckedOut = _statisticService.GetNumCheckedOut(userID),
                    ItemTypes = new ItemTypes
                    {
                        Title = "User Item Types",
                        ID = "user-item-types",
                        NumAlbums = _statisticService.GetNumAlbums(userID),
                        NumBooks = _statisticService.GetNumBooks(userID),
                        NumMoviesAndShows = _statisticService.GetNumMoviesShows(userID),
                        NumGames = _statisticService.GetNumGames(userID),
                        NumPops = _statisticService.GetNumPops(userID),
                        NumWishes = _statisticService.GetNumWishes(userID),
                        NumShowcased = _statisticService.GetNumShowcased(userID)
                    }
                };

            var model = new StatisticsViewModel
            {
                Universal = universalStats,
                User = userStats
            };

            ViewBag.Title = "Global Statistics";

            return View(model);
        }

        [HttpGet]
        public IActionResult AlbumStats()
        {
            ViewBag.Title = "Album Stats";

            var model = new AlbumStatsViewModel
            {
                Global = new AlbumStatsModel
                {
                    Types = new AlbumTypesModel
                    {
                        NumVinyl = _albumStatisticService.NumVinyl(),
                        NumCD = _albumStatisticService.NumCD(),
                        Title = "Global Album Types",
                        ID = "global-album-types"
                    },
                    Speeds = new AlbumSpeedModel
                    {
                        Num33RPM = _albumStatisticService.Num3313RPM(),
                        Num45RPM = _albumStatisticService.Num45RPM(),
                        Num78RPM = _albumStatisticService.Num78RPM(),
                        Title = "Global Album Speeds",
                        ID = "global-album-speeds"
                    },
                    Formats = new AlbumFormatModel
                    {
                        Num12Inch = _albumStatisticService.Num12Inch(),
                        Num10Inch = _albumStatisticService.Num10Inch(),
                        Num7Inch = _albumStatisticService.Num7Inch(),
                        Title = "Global Album Formats",
                        ID = "global-album-formats"
                    },
                    TopArtists = _albumStatisticService.TopArtists(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopGenres = _albumStatisticService.TopGenres(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopRecordLabels = _albumStatisticService.TopRecordLabels(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopCountriesOfOrigin = _albumStatisticService.TopCountriesOfOrigin(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPurchaseCountries = _albumStatisticService.TopPurchaseCountries(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    MostCompleted = _albumStatisticService.MostCompleted(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLocationsPurchased = _albumStatisticService.TopLocationsPurchased(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopReleaseYears = _albumStatisticService.TopReleaseYears(numToTake: NUM_OF_TOP_TO_TAKE).ToList()
                },
                User = new AlbumStatsModel
                {
                    Types = new AlbumTypesModel
                    {
                        NumVinyl = _albumStatisticService.NumVinyl(_user.Id),
                        NumCD = _albumStatisticService.NumCD(_user.Id),
                        Title = "User Album Types",
                        ID = "user-album-types"
                    },
                    Speeds = new AlbumSpeedModel
                    {
                        Num33RPM = _albumStatisticService.Num3313RPM(_user.Id),
                        Num45RPM = _albumStatisticService.Num45RPM(_user.Id),
                        Num78RPM = _albumStatisticService.Num78RPM(_user.Id),
                        Title = "User Album Speeds",
                        ID = "user-album-speeds"
                    },
                    Formats = new AlbumFormatModel
                    {
                        Num12Inch = _albumStatisticService.Num12Inch(_user.Id),
                        Num10Inch = _albumStatisticService.Num10Inch(_user.Id),
                        Num7Inch = _albumStatisticService.Num7Inch(_user.Id),
                        Title = "User Album Formats",
                        ID = "user-album-formats"
                    },
                    TopArtists = _albumStatisticService.TopArtists(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopGenres = _albumStatisticService.TopGenres(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopRecordLabels = _albumStatisticService.TopRecordLabels(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopCountriesOfOrigin = _albumStatisticService.TopCountriesOfOrigin(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPurchaseCountries = _albumStatisticService.TopPurchaseCountries(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    MostCompleted = _albumStatisticService.MostCompleted(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLocationsPurchased = _albumStatisticService.TopLocationsPurchased(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopReleaseYears = _albumStatisticService.TopReleaseYears(_user.Id, NUM_OF_TOP_TO_TAKE).ToList()
                }
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult BookStats()
        {
            ViewBag.Title = "Book Stats";

            var model = new BookStatsViewModel
            {
                Global = new BookStatsModel
                {
                    Types = new BookTypesModel
                    {
                        NumNovel = _bookStatisticService.NumNovel(),
                        NumComic = _bookStatisticService.NumComic(),
                        NumManga = _bookStatisticService.NumManga(),
                        Title = "Global Book Types",
                        ID = "global-book-types"
                    },
                    NumHardcover = _bookStatisticService.NumHardcover(),
                    NumFirstEdition = _bookStatisticService.NumFirstEdition(),
                    TotalPageCount = _bookStatisticService.TotalPageCount(),
                    TopPublishers = _bookStatisticService.TopPublishers(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopCountriesOfOrigin = _bookStatisticService.TopCountriesOfOrigin(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPurchaseCountries = _bookStatisticService.TopPurchaseCountries(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    MostCompleted = _bookStatisticService.MostCompleted(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLocationsPurchased = _bookStatisticService.TopLocationsPurchased(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopReleaseYears = _bookStatisticService.TopReleaseYears(numToTake: NUM_OF_TOP_TO_TAKE).ToList()
                },
                User = new BookStatsModel
                {
                    Types = new BookTypesModel
                    {
                        NumNovel = _bookStatisticService.NumNovel(_user.Id),
                        NumComic = _bookStatisticService.NumComic(_user.Id),
                        NumManga = _bookStatisticService.NumManga(_user.Id),
                        Title = "User Book Types",
                        ID = "user-book-types"
                    },
                    NumHardcover = _bookStatisticService.NumHardcover(_user.Id),
                    NumFirstEdition = _bookStatisticService.NumFirstEdition(_user.Id),
                    TotalPageCount = _bookStatisticService.TotalPageCount(_user.Id),
                    TopPublishers = _bookStatisticService.TopPublishers(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopCountriesOfOrigin = _bookStatisticService.TopCountriesOfOrigin(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPurchaseCountries = _bookStatisticService.TopPurchaseCountries(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    MostCompleted = _bookStatisticService.MostCompleted(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLocationsPurchased = _bookStatisticService.TopLocationsPurchased(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopReleaseYears = _bookStatisticService.TopReleaseYears(_user.Id, NUM_OF_TOP_TO_TAKE).ToList()
                }
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult GameStats()
        {
            ViewBag.Title = "Game Stats";

            var model = new GameStatsViewModel
            {
                Global = new GameStatsModel
                {
                    TopDevelopers = _gameStatisticService.TopDevelopers(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPublishers = _gameStatisticService.TopPublishers(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    Types = new GameTypesModel
                    {
                        NumFullGame = _gameStatisticService.NumFullGame(),
                        NumDLC = _gameStatisticService.NumDLC(),
                        NumExpansion = _gameStatisticService.NumExpansion(),
                        Title = "Global Game Types",
                        ID = "global-game-types"
                    },
                    Ratings = new GameRatingModel
                    {
                        NumRatedEC = _gameStatisticService.NumRatedEC(),
                        NumRatedE = _gameStatisticService.NumRatedE(),
                        NumRatedE10 = _gameStatisticService.NumRatedE10(),
                        NumRatedT = _gameStatisticService.NumRatedT(),
                        NumRatedM = _gameStatisticService.NumRatedM(),
                        NumRatedA = _gameStatisticService.NumRatedA(),
                        Title = "Global Game Ratings",
                        ID = "global-game-ratings"
                    },
                    Platforms = new GamePlatformsModel
                    {
                        CurrentGeneration = new CurrentGeneration
                        {
                            NumPlayStation4 = _gameStatisticService.NumPlayStation4(),
                            NumXboxOne = _gameStatisticService.NumXboxOne(),
                            NumNintendoSwitch = _gameStatisticService.NumNintendoSwitch(),
                            Title = "Global Current Generation",
                            ID = "global-current-generation"
                        },
                        Other = new OtherPlatforms
                        {
                            NumBoardGame = _gameStatisticService.NumBoardGame(),
                            NumPC = _gameStatisticService.NumPC(),
                            Title = "Global Other",
                            ID = "global-other"
                        },
                        Sony = new Sony
                        {
                            NumPlayStation = _gameStatisticService.NumPlayStation(),
                            NumPlayStation2 = _gameStatisticService.NumPlayStation2(),
                            NumPlayStation3 = _gameStatisticService.NumPlayStation3(),
                            NumPlayStation4 = _gameStatisticService.NumPlayStation4(),
                            NumPSP = _gameStatisticService.NumPSP(),
                            NumPSVita = _gameStatisticService.NumPSVita(),
                            Title = "Global Sony",
                            ID = "global-sony"
                        },
                        Microsoft = new Models.Microsoft
                        {
                            NumXbox = _gameStatisticService.NumXbox(),
                            NumXbox360 = _gameStatisticService.NumXbox360(),
                            NumXboxOne = _gameStatisticService.NumXboxOne(),
                            Title = "Global Microsoft",
                            ID = "global-microsoft"
                        },
                        Nintendo = new Nintendo
                        {
                            NumNintendo64 = _gameStatisticService.NumNintendo64(),
                            NumGameCube = _gameStatisticService.NumGameCube(),
                            NumWii = _gameStatisticService.NumWii(),
                            NumWiiU = _gameStatisticService.NumWiiU(),
                            NumNintendoSwitch = _gameStatisticService.NumNintendoSwitch(),
                            NumGameBoy = _gameStatisticService.NumGameBoy(),
                            NumGameBoyAdvance = _gameStatisticService.NumGameBoyAdvance(),
                            NumNintendoDS = _gameStatisticService.NumNintendoDS(),
                            NumNintendo3DS = _gameStatisticService.NumNintendo3DS(),
                            Title = "Global Nintendo",
                            ID = "global-nintendo"
                        },
                        Handhelds = new Handhelds
                        {
                            NumGameBoy = _gameStatisticService.NumGameBoy(),
                            NumGameBoyAdvance = _gameStatisticService.NumGameBoyAdvance(),
                            NumNintendoDS = _gameStatisticService.NumNintendoDS(),
                            NumNintendo3DS = _gameStatisticService.NumNintendo3DS(),
                            NumPSP = _gameStatisticService.NumPSP(),
                            NumPSVita = _gameStatisticService.NumPSVita(),
                            NumSwitch = _gameStatisticService.NumNintendoSwitch(),
                            Title = "Global Handhelds",
                            ID = "global-handhelds"
                        }
                    },
                    TopCountriesOfOrigin = _gameStatisticService.TopCountriesOfOrigin(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPurchaseCountries = _gameStatisticService.TopPurchaseCountries(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    MostCompleted = _gameStatisticService.MostCompleted(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLocationsPurchased = _gameStatisticService.TopLocationsPurchased(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopReleaseYears = _gameStatisticService.TopReleaseYears(numToTake: NUM_OF_TOP_TO_TAKE).ToList()
                },
                User = new GameStatsModel
                {
                    TopDevelopers = _gameStatisticService.TopDevelopers(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPublishers = _gameStatisticService.TopPublishers(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    Types = new GameTypesModel
                    {
                        NumFullGame = _gameStatisticService.NumFullGame(_user.Id),
                        NumDLC = _gameStatisticService.NumDLC(_user.Id),
                        NumExpansion = _gameStatisticService.NumExpansion(_user.Id),
                        Title = "User Game Types",
                        ID = "user-game-types"
                    },
                    Ratings = new GameRatingModel
                    {
                        NumRatedEC = _gameStatisticService.NumRatedEC(_user.Id),
                        NumRatedE = _gameStatisticService.NumRatedE(_user.Id),
                        NumRatedE10 = _gameStatisticService.NumRatedE10(_user.Id),
                        NumRatedT = _gameStatisticService.NumRatedT(_user.Id),
                        NumRatedM = _gameStatisticService.NumRatedM(_user.Id),
                        NumRatedA = _gameStatisticService.NumRatedA(_user.Id),
                        Title = "User Game Ratings",
                        ID = "user-game-ratings"
                    },
                    Platforms = new GamePlatformsModel
                    {
                        CurrentGeneration = new CurrentGeneration
                        {
                            NumPlayStation4 = _gameStatisticService.NumPlayStation4(_user.Id),
                            NumXboxOne = _gameStatisticService.NumXboxOne(_user.Id),
                            NumNintendoSwitch = _gameStatisticService.NumNintendoSwitch(_user.Id),
                            Title = "User Current Generation",
                            ID = "user-current-generation"
                        },
                        Other = new OtherPlatforms
                        {
                            NumBoardGame = _gameStatisticService.NumBoardGame(_user.Id),
                            NumPC = _gameStatisticService.NumPC(_user.Id),
                            Title = "User Other",
                            ID = "user-other"
                        },
                        Sony = new Sony
                        {
                            NumPlayStation = _gameStatisticService.NumPlayStation(_user.Id),
                            NumPlayStation2 = _gameStatisticService.NumPlayStation2(_user.Id),
                            NumPlayStation3 = _gameStatisticService.NumPlayStation3(_user.Id),
                            NumPlayStation4 = _gameStatisticService.NumPlayStation4(_user.Id),
                            NumPSP = _gameStatisticService.NumPSP(_user.Id),
                            NumPSVita = _gameStatisticService.NumPSVita(_user.Id),
                            Title = "User Sony",
                            ID = "user-sony"
                        },
                        Microsoft = new Models.Microsoft
                        {
                            NumXbox = _gameStatisticService.NumXbox(_user.Id),
                            NumXbox360 = _gameStatisticService.NumXbox360(_user.Id),
                            NumXboxOne = _gameStatisticService.NumXboxOne(_user.Id),
                            Title = "User Microsoft",
                            ID = "user-microsoft"
                        },
                        Nintendo = new Nintendo
                        {
                            NumNintendo64 = _gameStatisticService.NumNintendo64(_user.Id),
                            NumGameCube = _gameStatisticService.NumGameCube(_user.Id),
                            NumWii = _gameStatisticService.NumWii(_user.Id),
                            NumWiiU = _gameStatisticService.NumWiiU(_user.Id),
                            NumNintendoSwitch = _gameStatisticService.NumNintendoSwitch(_user.Id),
                            NumGameBoy = _gameStatisticService.NumGameBoy(_user.Id),
                            NumGameBoyAdvance = _gameStatisticService.NumGameBoyAdvance(_user.Id),
                            NumNintendoDS = _gameStatisticService.NumNintendoDS(_user.Id),
                            NumNintendo3DS = _gameStatisticService.NumNintendo3DS(_user.Id),
                            Title = "User Nintendo",
                            ID = "user-nintendo"
                        },
                        Handhelds = new Handhelds
                        {
                            NumGameBoy = _gameStatisticService.NumGameBoy(_user.Id),
                            NumGameBoyAdvance = _gameStatisticService.NumGameBoyAdvance(_user.Id),
                            NumNintendoDS = _gameStatisticService.NumNintendoDS(_user.Id),
                            NumNintendo3DS = _gameStatisticService.NumNintendo3DS(_user.Id),
                            NumPSP = _gameStatisticService.NumPSP(_user.Id),
                            NumPSVita = _gameStatisticService.NumPSVita(_user.Id),
                            Title = "User Handhelds",
                            ID = "user-handhelds"
                        }
                    },
                    TopCountriesOfOrigin = _gameStatisticService.TopCountriesOfOrigin(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPurchaseCountries = _gameStatisticService.TopPurchaseCountries(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    MostCompleted = _gameStatisticService.MostCompleted(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLocationsPurchased = _gameStatisticService.TopLocationsPurchased(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopReleaseYears = _gameStatisticService.TopReleaseYears(_user.Id, NUM_OF_TOP_TO_TAKE).ToList()
                }
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult MovieStats()
        {
            ViewBag.Title = "Movie Stats";

            var model = new MovieStatsViewModel
            {
                Global = new MovieStatsModel
                {
                    TopDirectors = _movieStatisticService.TopDirectors(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    Types = new MovieTypesModel
                    {
                        NumDVD = _movieStatisticService.NumDVD(),
                        NumBluRay = _movieStatisticService.NumBluRay(),
                        Title = "Global Movie Types",
                        ID = "global-movie-types"
                    },
                    Ratings = new MovieRatingModel
                    {
                        NumRatedG = _movieStatisticService.NumRatedG(),
                        NumRatedPG = _movieStatisticService.NumRatedPG(),
                        NumRatedPG13 = _movieStatisticService.NumRatedPG13(),
                        NumRatedR = _movieStatisticService.NumRatedR(),
                        NumRatedNR = _movieStatisticService.NumRatedNR(),
                        Title = "Global Movie Ratings",
                        ID = "global-movie-ratings"
                    },
                    TopCountriesOfOrigin = _movieStatisticService.TopCountriesOfOrigin(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPurchaseCountries = _movieStatisticService.TopPurchaseCountries(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    MostCompleted = _movieStatisticService.MostCompleted(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLocationsPurchased = _movieStatisticService.TopLocationsPurchased(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopReleaseYears = _movieStatisticService.TopReleaseYears(numToTake: NUM_OF_TOP_TO_TAKE).ToList()
                },
                User = new MovieStatsModel
                {
                    TopDirectors = _movieStatisticService.TopDirectors(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    Types = new MovieTypesModel
                    {
                        NumDVD = _movieStatisticService.NumDVD(),
                        NumBluRay = _movieStatisticService.NumBluRay(),
                        Title = "User Movie Types",
                        ID = "user-movie-types"
                    },
                    Ratings = new MovieRatingModel
                    {
                        NumRatedG = _movieStatisticService.NumRatedG(),
                        NumRatedPG = _movieStatisticService.NumRatedPG(),
                        NumRatedPG13 = _movieStatisticService.NumRatedPG13(),
                        NumRatedR = _movieStatisticService.NumRatedR(),
                        NumRatedNR = _movieStatisticService.NumRatedNR(),
                        Title = "User Movie Ratings",
                        ID = "user-movie-ratings"
                    },
                    TopCountriesOfOrigin = _movieStatisticService.TopCountriesOfOrigin(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPurchaseCountries = _movieStatisticService.TopPurchaseCountries(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    MostCompleted = _movieStatisticService.MostCompleted(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLocationsPurchased = _movieStatisticService.TopLocationsPurchased(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopReleaseYears = _movieStatisticService.TopReleaseYears(_user.Id, NUM_OF_TOP_TO_TAKE).ToList()
                }
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult PopStats()
        {
            ViewBag.Title = "Pop Stats";

            var model = new PopStatsViewModel
            {
                Global = new PopStatsModel
                {
                    TopSeries = _popStatisticService.TopSeries(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLines = _popStatisticService.TopLines(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopCountriesOfOrigin = _popStatisticService.TopCountriesOfOrigin(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPurchaseCountries = _popStatisticService.TopPurchaseCountries(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLocationsPurchased = _popStatisticService.TopLocationsPurchased(numToTake: NUM_OF_TOP_TO_TAKE).ToList(),
                    TopReleaseYears = _popStatisticService.TopReleaseYears(numToTake: NUM_OF_TOP_TO_TAKE).ToList()
                },
                User = new PopStatsModel
                {
                    TopSeries = _popStatisticService.TopSeries(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLines = _popStatisticService.TopLines(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopCountriesOfOrigin = _popStatisticService.TopCountriesOfOrigin(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopPurchaseCountries = _popStatisticService.TopPurchaseCountries(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopLocationsPurchased = _popStatisticService.TopLocationsPurchased(_user.Id, NUM_OF_TOP_TO_TAKE).ToList(),
                    TopReleaseYears = _popStatisticService.TopReleaseYears(_user.Id, NUM_OF_TOP_TO_TAKE).ToList()
                }
            };

            return View(model);
        }
    }
}