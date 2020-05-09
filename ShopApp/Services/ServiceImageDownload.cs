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
using System.Net;
using System.IO;
using System.Threading;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;

namespace ShopApp
{
    [Service]
    public class ServiceImageDownload : Service
    {
        

        private string strUrl = "";
        Thread thread;

        public override void OnCreate()
        {
            base.OnCreate();
          

        }


        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)//moving a link to download the image for the theard action
        {
           
                this.strUrl = intent.GetStringExtra("strUrl");
                ThreadStart thread_Start = new ThreadStart(this.DownloadImage);
                this.thread = new Thread(thread_Start);
                this.thread.Start();

                return base.OnStartCommand(intent, flags, startId);
            
        }


        public override IBinder OnBind(Intent intent)
        {
            return null;
        }


        //public override void OnDestroy()
        //{
        //    base.OnDestroy();
        //}



        private void DownloadImage()//makes a connection with the net and than download the image to the smartphone
        { 
            WebClient web_Client = new WebClient();
            Uri uri = new Uri(this.strUrl); //the link of the photo in the phone 

             byte[] image_bytes = null;
            try
            {

                image_bytes = web_Client.DownloadData(uri); //מוריד את התמונה מהקישור לסוג בייטים בכך שמשתמש בפעולה 
            
            }

            catch(Exception e)
            {
                
                return;
            }

            string Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);//
            string File_name_in_phone = "ImageProduct.png";
            string local_Path = System.IO.Path.Combine(Path, File_name_in_phone);//משלב את שני הסטרינגים לסטרינג אחד של קישור בעזרת הפעולה קומביין

            FileStream fStream = new FileStream(local_Path, FileMode.OpenOrCreate  , FileAccess.ReadWrite);//פותח את הקובץ שנמצא במשתנה לוקאלפת ומשאיר אותו פתוח באופן זמני

            fStream.Write(image_bytes, 0, image_bytes.Length); //מתחיל לכתוב לקובץ את כל הבייטים של התמונה שהורדנו   //the problem is here (dont write to the path )
            fStream.Close();//שומר את התמונה
            this.StopSelf();

            Intent intent = new Intent("ImageDownloadStaion"); //the intent filter
            intent.PutExtra("file_Path", local_Path);//מעביר לאינטנט את המיקום של התמונה שהורדה בתוך הטלפון

            
         
            this.SendBroadcast(intent);


        }

        
    }
    
    
}