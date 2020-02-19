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
    [IntentFilter(new[] { FILTER })]
    public class ImageBrodcastReceiver : BroadcastReceiver
    {
        public const string FILTER = "image_download_firebase";


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
            if(iv_Product == null)
            {
                return;
            }
            string image_url = intent.GetStringExtra("filePath");//לוקח מתוך האינטנט את המיקום של הקובץ

            Bitmap image_Bitmap = BitmapFactory.DecodeFile(image_url);//לוקח את התמונה והופך אותה לביטמאפ 

            if (image_Bitmap != null)
            {
                this.iv_Product.SetImageBitmap(image_Bitmap);//מכניס לתוך האימ'ג ויאו  את התמונה
            }

            else
            {
                Toast.MakeText(context, "התרחשה השגיאה", ToastLength.Short).Show();
            }
        }

    }

    
}