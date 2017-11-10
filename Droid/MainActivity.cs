using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Scanner.Droid {
    [Activity(Label = "Scanner.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        protected override void OnCreate(Bundle bundle) {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) {
            base.OnActivityResult(requestCode, resultCode, data);
        }
      
        // Ok, so this does not work anymore on android. in the debugger it borks out a #noref on 
        // Reported to mono.net 
        //
        /* public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults) {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        } */ 

    }
}
