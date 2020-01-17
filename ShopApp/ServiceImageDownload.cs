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

namespace ShopApp
{/*
    [Service]
    public class ServiceImageDownload : Service
    {/*
        ISharedPreferences sp;

        private string strUrl = "";


        public override void OnCreate()
        {
            base.OnCreate();
            this.sp = this.GetSharedPreferences("details", FileCreationMode.Private);

        }


        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            this.strUrl = intent.GetStringExtra("strUrl");
            ThreadStart thread_Start = new ThreadStart(this.DownloadImage);
            Thread thread = new Thread(thread_Start);
            thread.Start();
            return base.OnStartCommand(intent, flags, startId);
        }


        public override IBinder OnBind(Intent intent)
        {
            return null;
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
        }



        private void DownloadImage()
        { 
            WebClient web_Client = new WebClient();
            Uri uri = new Uri(this.strUrl); //קישור עמוק 

            
             byte[] bytes = null;
            try
            {
                bytes = web_Client.DownloadData(uri); //מוריד את התמונה מהקישור לסוג בייטים בכך שמשתמש בפעולה 
            }

            catch(Exception e)
            {
                return;
            }

            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);//
            string localFilename = "ImageProduct.png";
            string local_Path = System.IO.Path.Combine(documentsPath, localFilename);//משלב את שני הסטרינגים לסטרינג אחד של קישור בעזרת הפעולה קומביין

            FileStream fStream = new FileStream(local_Path, FileMode.OpenOrCreate);//פותח את הקובץ שנמצא במשתנה לוקאלפת ומשאיר אותו פתוח באופן זמני
            fStream.Write(bytes, 0, bytes.Length);//מתחיל לכתוב לקובץ את כל הבייטים של התמונה שהורדנו
            fStream.Close();//שומר את התמונה

            sp.Edit().PutString("filePath", local_Path).Apply();
            ISharedPreferencesEditor editor = sp.Edit();

            editor.PutString("filePath", local_Path);//מוסיף למסמך של השרד פרפרנס עוד שדה עם הנתיב של המיקום של התמונה 
            editor.Commit();

            Intent intent = new Intent("download");
            intent.PutExtra("filePath", local_Path);

            this.StopSelf();
            this.SendBroadcast(intent);

        }



        
    }
    */
    
}