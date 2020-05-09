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
    [IntentFilter(new[] { "ImageDownloadStaion" })]
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

        public override void OnReceive(Context context, Intent intent)//get the path to the image and than convert to image 
            {
            //    if(iv_Product == null)
            //   {
            ////        return;
            ////    }

            //    string image_uri = intent.GetStringExtra("file_Path");//לוקח מתוך האינטנט את המיקום של הקובץ

            //    Bitmap image_Bitmap = BitmapFactory.DecodeFile(image_uri);//לוקח את התמונה והופך אותה לביטמאפ 

            //    if (image_Bitmap != null)
            //    {       

            //            this.iv_Product.SetImageBitmap(image_Bitmap);//מכניס לתוך האימ'ג ויאו  את התמונה
            //    }

            //    else
            //    {

            //        Toast.MakeText(context, "!התרחשה שגיאה אנא בדוק את תקינות הקישור", ToastLength.Short).Show();
            //    }


            string urlNativ = intent.GetStringExtra("file_Path"); //the problem is that there is no image file exist in this path

            Bitmap image = BitmapFactory.DecodeFile(urlNativ);  
             if(iv_Product != null)
            {

                if(image == null )
                {
                    Toast.MakeText(context, "יש תקלה אנא אפשר הורדת תמונות מהרשת בהרשאות האפליקציה", ToastLength.Short).Show();
                }

                else
                {
                    iv_Product.SetImageBitmap(image);
                }

            }




        }

    }

    
}