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
        private ISharedPreferences sp;
        private ImageView imageViewProduct;
        private Activity activity;

        public ImageBrodcastReceiver() { }
        public  ImageBrodcastReceiver(Activity activity, ImageView imageViewProduct)
        {

            this.imageViewProduct = imageViewProduct;
            this.activity = activity;
            this.sp = this.activity.GetSharedPreferences("details", FileCreationMode.Private);
        }
        public override void OnReceive(Context context, Intent intent)
        {
            string filePath = sp.GetString("filePath", "");

            Bitmap image_Bitmap = BitmapFactory.DecodeFile(filePath);

            if (image_Bitmap != null)
            {
                this.imageViewProduct.SetImageBitmap(image_Bitmap);

            }

            else
            {
                Toast.MakeText(context, "התרחשה השגיאה ", ToastLength.Short).Show();

            }
        }

    }

    
}