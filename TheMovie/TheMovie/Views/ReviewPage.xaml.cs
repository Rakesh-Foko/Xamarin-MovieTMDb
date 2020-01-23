using System;
using System.Collections.Generic;
using Plugin.Media;
using Xamarin.Forms;

namespace TheMovie.Views
{
    public partial class ReviewPage : ContentPage
    {
        public ReviewPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            movieReviewslistView.ItemsSource = await App.Database.GetReviewsAsync();
        }

       
       
    }
}
