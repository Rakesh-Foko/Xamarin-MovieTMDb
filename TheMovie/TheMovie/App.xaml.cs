using System;
using System.IO;
using Prism.DryIoc;
using Prism.Ioc;
using TheMovie.Views;
using Xamarin.Forms;

namespace TheMovie
{
    public partial class App : PrismApplication
    {
        static Database database;

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}");
        }

        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FavMoviesDB.db3"));
                }
                return database;
            }
        }

       

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<SearchMoviesPage>();
            containerRegistry.RegisterForNavigation<MovieDetailPage>();
            containerRegistry.RegisterForNavigation<FavoritesPage>();
            containerRegistry.RegisterForNavigation<ReviewPage>();
        }

        protected override void OnStart()
        {
            base.OnStart();            
        }
    }
}
