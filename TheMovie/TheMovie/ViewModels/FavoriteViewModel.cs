using Prism.Navigation;
using TheMovie.Helpers;

namespace TheMovie.ViewModels
{
    public class FavoriteViewModel : BaseViewModel
    {

        private readonly INavigationService navigationService;
        public ObservableRangeCollection<Models.FavMovies> FavMovies  { get; set; }


        public FavoriteViewModel(INavigationService navigationService)
        {
            Title = "Favorite Movies";
            this.navigationService = navigationService;
        }
       
    }
        }

