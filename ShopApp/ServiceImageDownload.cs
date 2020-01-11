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

       /* private void Download()
        { 
            WebClient webClient = new WebClient();
            Uri uri = new Uri(this.strUrl); //קישור עמוק 

            
             byte[] bytes = null;
            try
            {
                bytes = webClient.DownloadData(uri); //מוריד את התמונה מהקישור לסוג בייטים בכך שמשתמש בפעולה 
            }

            catch(Exception e)
            {
                return;
            }

            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);//
            string localFilename = "ImageProduct.png";
            string localPath = System.IO.Path.Combine(documentsPath, localFilename);//משלב את שני הסטרינגים לסטרינג אחד של קישור בעזרת הפעולה קומביין

            FileStream fStream = new FileStream(localPath, FileMode.OpenOrCreate);//פותח את הקובץ שנמצא במשתנה לוקאלפת ומשאיר אותו פתוח באופן זמני
            fStream.Write(bytes, 0, bytes.Length);//מתחיל לכתוב לקובץ את כל הבייטים של התמונה שהורדנו
            fStream.Close();//שומר את התמונה

            sp.Edit().PutString("filePath", localPath).Apply();
            this.StopSelf();

            Intent  intent = new Intent("download")

        }
        */

       

    }
}