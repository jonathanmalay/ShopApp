﻿using System;
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

namespace ShopApp
{
    [Activity(Label = "FirstActivity", Theme = "@style/AppTheme", MainLauncher = true)]   //המסך הראשון שיוצג בפתיחת האפליקציה
    public class FirstActivity : AppCompatActivity //הגרסה החדשה של אקטיביטי זה  אפפקומפאטאקטיביטי
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {                 
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_first);
            Thread.Sleep(1500); //מחכהה שנייה וחצי
            Intent intentToLogin = new Intent(this, typeof(MainActivity));//עובר למסך ההתחברות 
            this.StartActivity(intentToLogin);

            Finish(); //סוגר את המסך הנוכחי

        }
    }
}