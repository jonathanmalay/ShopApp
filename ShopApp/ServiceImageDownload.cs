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

namespace ShopApp
{
    [Service]
    public class ServiceImageDownload : Service
    {
        ISharedPreferences sp;
       
       
        public override void OnCreate()
        {
            base.OnCreate();
            this.sp = this.GetSharedPreferences("details", FileCreationMode.Private);

        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }


       

    }
}