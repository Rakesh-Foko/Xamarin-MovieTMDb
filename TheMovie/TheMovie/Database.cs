using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace TheMovie
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;
        public static string testFavMovie;
        public static string singleFavMovie;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Models.FavMovies>().Wait();
            _database.CreateTableAsync<Models.MovieReviews>().Wait();


        }

        public Task<List<Models.MovieReviews>> GetReviewsAsync()
        {
            return _database.Table<Models.MovieReviews>().ToListAsync();
        }

        public Task<int> SaveReviewAsync(Models.MovieReviews review)
        {
            return _database.InsertAsync(review);
        }



        public Task<List<Models.FavMovies>> GetFavoriteMoviesAsync()
        {
            return _database.Table<Models.FavMovies>().ToListAsync();

        }

        public Task<List<Models.MovieReviews>> GetMovieReviewsAsync()
        {
            return _database.Table<Models.MovieReviews>().ToListAsync();

        }

        public async Task<List<Models.FavMovies>> GetAllFavMoviesAsync()
        {
            // SQL queries are also possible
            var data = await _database.Table<Models.FavMovies>().ToListAsync();
            Debug.WriteLine(data.ToString());
            if (data != null)
            {
                var dataString = new List<Models.FavMovies>(data);
                //var fAll = dataString.;

                return new List<Models.FavMovies>(data);
            }

            else
            {
                return null;
            }
        }

        public async Task<int> SaveFavMoviesAsync(Models.FavMovies favMovie)
        {
            var data = await _database.Table<Models.FavMovies>().ToListAsync();
            Debug.WriteLine(data.ToString());
            if (data != null)
            {
                var dataString = new List<Models.FavMovies>(data);
                foreach (var movieName in dataString)
                {
                    if (favMovie.FavMovieTitleInListView == movieName.FavMovieTitleInListView)
                    {
                        await Application.Current.MainPage.DisplayAlert("The Movie", "Movie removed from Favorites", "Ok");

                        return await _database.DeleteAsync(movieName);
                    }
                }

            }
            await Application.Current.MainPage.DisplayAlert("The Movie", "Movie added to Favorites", "Ok");

            return await _database.InsertAsync(favMovie);
        }

        public async Task<int> SaveMoviewReviewsAsync(Models.MovieReviews movieReviews)
        {
            var data = await _database.Table<Models.MovieReviews>().ToListAsync();
            Debug.WriteLine(data.ToString());
            if (data != null)
            {
                var dataString = new List<Models.MovieReviews>(data);
                return await _database.InsertAsync(movieReviews);

                //foreach (var movieName in dataString)
                //{
                //    if (movieReviews.FavMovieTitleInListView == movieName.FavMovieTitleInListView)
                //    {
                //        return await _database.DeleteAsync(movieName);
                //    }
                //}

            }

            return await _database.InsertAsync(movieReviews);
        }
    }
}
   


