using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ShopApp
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "image_download_firebase" })]
    public class ImageBrodcastReceiver : BroadcastReceiver
    {
       
        private ImageView iv_Product;
        private Activity activity;

        public ImageBrodcastReceiver() { }
        public  ImageBrodcastReceiver(Activity activity, ImageView imageViewProduct)
        {

            this.iv_Product = imageViewProduct;
            this.activity = activity;
            
        }
        public override void OnReceive(Context context, Intent intent)
        {
            string image_url = intent.GetStringExtra("filePath");

            Bitmap image_Bitmap = BitmapFactory.DecodeFile(image_url);

            if (image_Bitmap != null)
            {
                this.iv_Product.SetImageBitmap(image_Bitmap);

            }

            else
            {
                Toast.MakeText(context, "התרחשה השגיאה ", ToastLength.Short).Show();

            }
        }

    }

    
}