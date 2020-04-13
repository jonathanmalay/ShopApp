using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Threading;
using System.Drawing;

namespace ShopApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]   //המסך הראשון שיוצג בפתיחת האפליקציה
    public class FirstActivity : AppCompatActivity 
    {
        Bitmap shopLogo;
        protected override void OnCreate(Bundle savedInstanceState)
        {                 
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_first);
            Thread.Sleep(1500); //מחכה שנייה וחצי
            Intent intentToLogin = new Intent(this, typeof(MainActivity));
            this.StartActivity(intentToLogin);

            Finish(); 

        }
    }
}