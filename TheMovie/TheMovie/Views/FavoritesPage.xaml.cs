using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TheMovie.Views
{
    public partial class FavoritesPage : ContentPage
    {
        public FavoritesPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            favMoviesListView.ItemsSource = await App.Database.GetFavoriteMoviesAsync();
        }
    }
}
