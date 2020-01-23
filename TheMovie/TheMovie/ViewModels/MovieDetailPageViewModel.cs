using Mobile.Consts;
using Mobile.Extensions;
using Plugin.Media;
using Plugin.Share;
using Prism.Commands;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using TheMovie.Helpers;
using TheMovie.Models;
using Xamarin.Forms;
using Image = TheMovie.Models.Image;

namespace TheMovie.ViewModels
{
    public class MovieDetailPageViewModel : BaseViewModel, INavigationAware
    {
        private Movie movie;
        public Movie Movie
        {
            get { return movie; }
            set { SetProperty(ref movie, value); }
        }

        public string imgPath = "";

        private string _reviewTitle;
        public string reviewTitle
        {
            get { return _reviewTitle; }
            set
            {
                _reviewTitle = value;
                RaisePropertyChanged("_reviewTitle");
            }
        }

        private string _ratingStars;
        public string ratingStars
        {
            get { return _ratingStars; }
            set
            {
                _ratingStars = value;
                RaisePropertyChanged("_ratingStars");
            }
        }

        
        private string _descriptionText;
        public string descriptionText
        {
            get { return _descriptionText; }
            set
            {
                _descriptionText = value;
                RaisePropertyChanged("_descriptionText");
            }
        }

        private string _reviewCapturedImage;
        public string reviewCapturedImage
        {
            get { return _reviewCapturedImage; }
            set
            {
                _descriptionText = value;
                RaisePropertyChanged("_reviewCapturedImage");
            }
        }


        public ObservableRangeCollection<Movie> Movies { get; set; }

        private int heightVideos = 200;
        public int HeightVideos
        {
            get { return heightVideos; }
            set { SetProperty(ref heightVideos, value); }
        }

        private readonly INavigationService navigationService;


        public ObservableRangeCollection<Image> Backdrops { get; set; } = new ObservableRangeCollection<Image>();
        public ObservableRangeCollection<Video> Videos { get; set; } = new ObservableRangeCollection<Video>();

        public DelegateCommand FavoriteCommand { get; }
        public DelegateCommand TakePicCommand { get; }
        public DelegateCommand ReviewsCommand { get; }

        public DelegateCommand ShareCommand { get; }
        public DelegateCommand ReviewMoviesCommand { get; }

        public DelegateCommand<Video> OpenVideoCommand { get; }
        public DelegateCommand LoadMovieDetailCommand { get; }

        private string _currentImage;

        public string CurrentImage
        {
            get => _currentImage;
            set => SetProperty(ref _currentImage, value);
        }


        public MovieDetailPageViewModel(INavigationService navigationService)
        {
            //App.Database.GetAllFavMoviesAsync();
            this.navigationService = navigationService;           
            ShareCommand = new DelegateCommand(async () => await ShareCommandExecuteAsync());
            FavoriteCommand = new DelegateCommand(async () => await FavoriteCommandExecuteAsync());
            TakePicCommand = new DelegateCommand(async () => await TakePicCommandExecuteAsync());

            ReviewsCommand = new DelegateCommand(async () => await ReviewsCommandExecuteAsync());

            OpenVideoCommand = new DelegateCommand<Video>(async (Video video) => await ExecuteOpenVideoCommandAsync(video).ConfigureAwait(false));
        }

       
        private async Task FavoriteCommandExecuteAsync()
        {
            await App.Database.SaveFavMoviesAsync(new FavMovies
            {
                FavMovieTitleInListView = movie.Title
        });
        }

        private async Task TakePicCommandExecuteAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                //await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            });

            if (file == null)
                return;

            await Application.Current.MainPage.DisplayAlert("The Movie", "Image successfully added to Reviews", "Ok");

            imgPath = file.Path;
        }


        private async Task ReviewsCommandExecuteAsync()
        {
               await App.Database.SaveReviewAsync(new MovieReviews
                {
                     RatingStars = "Stars  :  " + ratingStars,
                    ReviewTitle = "Title  :  " + reviewTitle,
                    Description = " Description  :  " + descriptionText,
                    ReviewCapturedImage = "" + imgPath
                });
            await navigationService.GoBackToRootAsync();
        }

        private async Task ShareCommandExecuteAsync()
        {
                await CrossShare.Current.Share(
                    new Plugin.Share.Abstractions.ShareMessage
                    {
                        Text = movie.Title,
                        Title = ConfigApp.ProjectName,
                        Url = $"http://{ConfigApp.WebUrl}{ConfigApp.MovieSharedPath}{movie.Id}"
                    }
                );
            }        


        private async Task ExecuteOpenVideoCommandAsync(Video video)
        {
            await CrossShare.Current.OpenBrowserAsync($"https://www.youtube.com/watch?v={video.Key}");
        }

        private async Task LoadMovieDetailAsync(int movieId)
        {
            var movieDetail = await ApiService.GetMovieDetailAsync(movieId).ConfigureAwait(false);
            if (movieDetail != null)
            {
                Movie = movieDetail;
            }
        }

        private async Task LoadMovieImagesAsync(int movieId)
        {
            var movieImages = await ApiService.GetMovieImagesAsync(movieId).ConfigureAwait(false);
            if (movieImages != null)
            {
                Device.BeginInvokeOnMainThread(() => {
                    Backdrops.AddRange(movieImages.Backdrops.Where(x => x.FilePath != movie.BackdropPath));
                });
            }
        }

        private async Task LoadMovieVideosAsync(int movieId)
        {
            var movieVideos = await ApiService.GetMovieVideosAsync(movieId).ConfigureAwait(false);
            if (movieVideos != null)
            {
                Device.BeginInvokeOnMainThread(() => {
                    Videos.AddRange(movieVideos.Results);
                    HeightVideos = Videos.Count * HeightVideos;
                });
               
            }
        }

        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            Movie = parameters.GetValue<Movie>("movie");
            Title = Movie.Title;

            Backdrops.Clear();
            Backdrops.Add(new Image
            {
                FilePath = movie.BackdropPath
            });

            await LoadMovieDetailAsync(Movie.Id).ConfigureAwait(false);
            await LoadMovieImagesAsync(Movie.Id).ConfigureAwait(false);
            await LoadMovieVideosAsync(Movie.Id).ConfigureAwait(false);
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            Movie = parameters.GetValue<Movie>("movie");
            //Movie = OnBackMovie;

            if (Movie == null)
            {
            }
            Title = Movie.Title;

            Backdrops.Clear();
            Backdrops.Add(new Image
            {
                FilePath = movie.BackdropPath
            });
            await LoadMovieDetailAsync(Movie.Id).ConfigureAwait(false);
            await LoadMovieImagesAsync(Movie.Id).ConfigureAwait(false);
            await LoadMovieVideosAsync(Movie.Id).ConfigureAwait(false);

        }
    }
}
