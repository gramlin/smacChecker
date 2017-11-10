using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Settings;
using ZXing.Net.Mobile.Forms;


namespace Scanner
{
    public partial class ConfigPage : ContentPage
    {
       
        public ConfigPage()
        {
            InitializeComponent();
            aurl.Text = Settings.ScanUrl;
        }
        public void OnClickOk(object sender, EventArgs e)
        {
            Settings.ScanUrl = aurl.Text;
            Navigation.PopAsync();
        }
        public void OnClickScan(object sender, EventArgs e) {
            ScanAsync();
        }
        public async void ScanAsync()
        {

            var scanPage = new ZXingScannerPage();

            scanPage.OnScanResult += (result) => {
                // Stop scanning
                scanPage.IsScanning = false;
                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();

                    Settings.ScanUrl = result.ToString();
                    aurl.Text = Settings.ScanUrl;
                });
               
            };

            // Navigate to our scanner page
            await Navigation.PushAsync(scanPage);

        }
    }
}
