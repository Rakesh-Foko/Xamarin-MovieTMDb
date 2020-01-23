using Plugin.Media;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TheMovie.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieDetailPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public MovieDetailPage()
        {
            InitializeComponent();

            VideosListView.ItemSelected += (sender, e) => {
                // Manually deselect item
                ((ListView)sender).SelectedItem = null;
            };
        }

        
    }
}
