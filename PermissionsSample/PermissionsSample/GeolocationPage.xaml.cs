using System;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PermissionsSample
{
    public partial class GeolocationPage : ContentPage
    {
        public GeolocationPage()
        {
            InitializeComponent();
        }

        bool busy;
        
        async void Button_OnClicked(object sender, EventArgs e)
        {
            if (busy)
                return;

            busy = true;
            ((Button) sender).IsEnabled = false;

			try
			{
				var status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
				if (status != PermissionStatus.Granted)
				{
					if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
					{
						await DisplayAlert("Need location", "Gunna need that location", "OK");
					}

					// THIS LINE OF CODE DOESN'T WAIT IT BREAKS OUT OF HERE WITH NO EXCEPTION, IT DOES GET PERMISSION THOUGH.
					status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
				}

				if (status == PermissionStatus.Granted)
				{
					//Query permission
					var request = new GeolocationRequest(GeolocationAccuracy.Medium);
					var location = await Geolocation.GetLocationAsync(request);

					LabelGeolocation.Text = "Lat: " + location.Latitude + " Long: " + location.Longitude;
				}
				else if (status != PermissionStatus.Unknown)
				{
					//location denied
					await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
				}
			}
			catch (Exception ex)
			{
				//Something
				LabelGeolocation.Text = "Error: " + ex;
			}

            ((Button)sender).IsEnabled = true;
            busy = false;
        }
    }
}
