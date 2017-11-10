using System;
using Xamarin.Forms;
namespace Scanner {
    public partial class App : Application {
        public App() {

            // T3:SMACATHON SCANNER
            var startPage = new PopUpPage();
            //  Generic scan page:
           // var startPage = new Scanner(); 


            MainPage = new NavigationPage(startPage);
        }


    }
}
