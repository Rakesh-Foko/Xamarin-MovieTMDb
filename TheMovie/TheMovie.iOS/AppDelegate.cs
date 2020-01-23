using FFImageLoading.Forms.Platform;
using Foundation;
using PanCardView.iOS;
using Syncfusion.SfRating.XForms.iOS;
using UIKit;

namespace TheMovie.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			CachedImageRenderer.Init();
			CardsViewRenderer.Preserve();
			new SfRatingRenderer();
			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
