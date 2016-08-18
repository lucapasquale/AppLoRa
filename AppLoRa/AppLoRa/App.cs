using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AppLoRa
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application         
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            await DataResources.node.PegarDados(StringResources.devEUIarduino);
            MainPage = new Pages.GraficosPage();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
