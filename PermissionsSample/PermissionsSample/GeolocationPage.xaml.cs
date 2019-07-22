﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        async void ButtonPermission_OnClicked(object sender, EventArgs e)
        {
            if (busy)
                return;
            
            busy = true;
            ((Button) sender).IsEnabled = false;

            var status = PermissionStatus.Unknown;
            switch (((Button)sender).StyleId)
            {
                case "Calendar":
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<CalendarPermission>();
                    break;
                case "Camera":
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                    break;
                case "Contacts":
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<ContactsPermission>();
                    break;
                case "Microphone":
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<MicrophonePermission>();
					break;
				case "Geolocation":
					status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
					break;
				case "Phone":
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<PhonePermission>();
					break;
                case "Photos":
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>();
					break;
                case "Reminders":
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<RemindersPermission>();
					break;
                case "Sensors":
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<SensorsPermission>();
					break;
                case "Sms":
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<SmsPermission>();
					break;
                case "Storage":
                    status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
					break;
                case "Settings":
                    CrossPermissions.Current.OpenAppSettings();
					((Button)sender).IsEnabled = true;
					busy = false;
					return;
            }

            await DisplayAlert("Pre - Results", status.ToString(), "OK");

            if (status != PermissionStatus.Granted)
            {
                switch (((Button)sender).StyleId)
                {
                    case "Calendar":
                        status = await Utils.CheckPermissions(Permission.Calendar);
                        break;
                    case "Camera":
                        status = await Utils.CheckPermissions(Permission.Camera);
                        break;
                    case "Contacts":
                        status = await Utils.CheckPermissions(Permission.Contacts);
                        break;
					case "Geolocation":
						status = await Utils.CheckPermissions(Permission.Location);
						break;
					case "Microphone":
                        status = await Utils.CheckPermissions(Permission.Microphone);
                        break;
                    case "Phone":
                        status = await Utils.CheckPermissions(Permission.Phone);
                        break;
                    case "Photos":
                        status = await Utils.CheckPermissions(Permission.Photos);
                        break;
                    case "Reminders":
                        status = await Utils.CheckPermissions(Permission.Reminders);
                        break;
                    case "Sensors":
                        status = await Utils.CheckPermissions(Permission.Sensors);
						break;
                    case "Sms":
                        status = await Utils.CheckPermissions(Permission.Sms);
                        break;
                    case "Storage":
                        status = await Utils.CheckPermissions(Permission.Storage);
                        break;
                }

                await DisplayAlert("Results", status.ToString(), "OK");

            }

            busy = false;
            ((Button) sender).IsEnabled = true;
        }

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
