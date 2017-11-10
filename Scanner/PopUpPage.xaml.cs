using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Diagnostics;
using System.Net.Http;


namespace Scanner {
    public partial class PopUpPage : ContentPage {
        public HttpClient _Client = new HttpClient();

        public String baseurl ="";
        //"http://rrr.se/t3?breakout=123&text=";
            
   
        public PopUpPage() {
            InitializeComponent();
        }

        public void OnClickScan(object sender, EventArgs e) {
            ScanAsync();
        }
        public void OnClickConfig(object sender, EventArgs e)
        {
            var configPage = new ConfigPage();
          Navigation.PushAsync(configPage); 
        }

        public bool MyRemoteCertificateValidationCallback(System.Object sender,
             X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // OK, so this is what we need to allow for https links. In Ios10 there are
            // a couple of caveats with this. Go to the url with safari and download the 
            // certificate if does not work. 

            bool isOk = true;
            // If there are errors in the certificate chain,
            // look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        continue;
                    }
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                        break;
                    }
                }
            }
            return isOk;
        }

        
        public async void ContactServer(string theInput, string baseurl)
        {


            ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
            baseurl = Settings.ScanUrl;
            var uri = new Uri(string.Format(baseurl+theInput, string.Empty));
            try
            {
                var response = await _Client.GetAsync(uri);
           
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Device.BeginInvokeOnMainThread(() =>
                {
                    // Close the scanner
                        Navigation.PopAsync();

                        // Pop a webview where the pretty result can be loaded
                    var browser = new WebView();
                    var htmlSource = new HtmlWebViewSource();
                    htmlSource.Html = content.ToString();
                    webView.Source = htmlSource;
                });

            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();

                    DisplayAlert("Server problem", response.ToString(), "OK");
                });
            }
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
               Navigation.PopAsync();

                DisplayAlert("Network connection problem", e.ToString(), "OK");
                });
            }

        }
 
        public async void ScanAsync() {

            var scanPage = new ZXingScannerPage();

            scanPage.OnScanResult += (result) => {
                // Stop scanning
                scanPage.IsScanning = false;
                 // Pop the page and show the result
                ContactServer(result.Text,baseurl);
                      
            };

            // Navigate to our scanner page
            await Navigation.PushAsync(scanPage);

        }
    }
}
