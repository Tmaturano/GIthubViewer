using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using System.Threading.Tasks;
using Android.Support.V7.App;

namespace GithubViewerXamarin.Droid.Activities
{
    [Activity(Label = "GithubViewer", Icon = "@drawable/gitviewer_logo", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreenActivity : Activity
    {        
        //NoHistory means that the user cant "back" to this activity
        protected override void OnCreate(Bundle savedInstanceState)     
        {
            base.OnCreate(savedInstanceState);            
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() =>
            {                
                Task.Delay(2000); 
            });

            startupWork.ContinueWith(t =>
            {                   
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }
    }
}