using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Diagnostics;

using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using System.Net.Http;

namespace Scanner {

    // Hello T3:smac. This is a generic scanner page I usually use when I have a reactive ui (like ng-rx or react.js))

    public partial class Scanner : ContentPage {
        public ZXing.Net.Mobile.Forms.ZXingScannerView scanner = new ZXing.Net.Mobile.Forms.ZXingScannerView();
    public HttpClient _Client = new HttpClient();  
       
                        
        public Scanner() {
            InitializeComponent();
            Scan();
        }

        public void OnClickStart(object sender, EventArgs e) {
            Button button = sender as Button;
            if(button.Text.Equals("Start")) {
                button.BackgroundColor = Color.Red;
                button.Text = "Stop";
                scanner.IsVisible = true;
            } else if(button.Text.Equals("Stop")) {
                scanner.IsVisible = false;
                button.BackgroundColor = Color.Green;
                button.Text = "Start";
            }
        }

         public void Scan() {
            try {
                scanner.Options = new MobileBarcodeScanningOptions() {
                    UseFrontCameraIfAvailable = false, //update later to come from settings
                    PossibleFormats = new List<BarcodeFormat>(),
                    TryHarder = true,
                    AutoRotate = false,
                    TryInverted = true,
                    DelayBetweenContinuousScans = 2000
                };

                scanner.IsVisible = false;
                scanner.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
                scanner.Options.PossibleFormats.Add(BarcodeFormat.DATA_MATRIX);
                scanner.Options.PossibleFormats.Add(BarcodeFormat.EAN_13);


                scanner.OnScanResult += (result) => {

                    // Stop scanning
                    scanner.IsAnalyzing = false;
                    if(scanner.IsScanning) {
                        scanner.AutoFocus();
                    }

                    // Pop the page and show the result
                    Device.BeginInvokeOnMainThread(async () => {
                        if(result != null) {
                            System.Diagnostics.Debug.WriteLine("Contacting server");

                            await DisplayAlert("Scan Value", result.Text, "OK");
                         }
                       
                             
                      
                    });
                };

                mainGrid.Children.Add(scanner, 1, 0);

            } catch(Exception ex) {
                Console.Write(ex);
            }
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            scanner.IsScanning = true;
        }

        protected override void OnDisappearing() {
            scanner.IsScanning = false;

            base.OnDisappearing();
        }


      
    }
}
