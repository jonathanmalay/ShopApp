﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Provider;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using Android.Content.PM;

namespace ShopApp
{
    [Activity(Label = "Activity_ManagerAddProduct")]
    public class Activity_ManagerAddProduct : Activity
    {
        Android.Net.Uri product_image_uri;
        EditText et_Price_Product, et_Dialog_url, et_Name_Product,et_Id_Product;
        Button btn_Pick_Product_Image, btn_Add_New_Product, btn_toolbar_backpage ;
        ImageButton imagebtn_toolbar_menu; 
        ImageView iv_Product_Image;
        Dialog dialog_Pick_Product_Image;
        Spinner spiner_type;
        int dialog_product_quantity; //כמות המוצר שנבחרת בספינר בדיאלוג של הוספת המוצר 
        Button btn_Dialog_Pick_Image_From_Gallery, btn_Dialog_Save_Image, btn_Dialog_Download_Url;//הפקדים שבתוך הדיאלוג
        ImageView iv_Dialog_Image; //התמונה הנבחרת של המוצר שתוצג בדיאלוג
       

        ImageBrodcastReceiver DownloadImage_Brodcast_Receiver; 


        protected override void OnCreate(Bundle savedInstanceState)
        {   
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.layout_ManagerAddProduct);

            this.et_Name_Product = FindViewById<EditText>(Resource.Id.etManagerAddProductName);
            this.et_Price_Product = FindViewById<EditText>(Resource.Id.etManagerAddProductPrice);
            this.et_Id_Product = FindViewById<EditText>(Resource.Id.etManagerAddProductId);
            this.btn_Add_New_Product = FindViewById<Button>(Resource.Id.btnManagerAddProductSave);
            this.btn_Pick_Product_Image = FindViewById<Button>(Resource.Id.btnManagerAddProductPickImage);
            this.btn_toolbar_backpage = FindViewById<Button>(Resource.Id.btn_toolbar_backPage); 
            this.imagebtn_toolbar_menu = FindViewById<ImageButton>(Resource.Id.btn_toolbar_menu); 
            this.iv_Product_Image = FindViewById<ImageView>(Resource.Id.ivManagerAddProductImage);
            this.spiner_type = FindViewById<Spinner>(Resource.Id.spinnerManagerAddProduct);




            this.btn_Add_New_Product.Click += Btn_Add_New_Product_ClickAsync;
            this.btn_Pick_Product_Image.Click += Btn_Pick_Product_Image_Click;

            string[] arr_spinner = new string[] { "1" , "2"   , "3" , "4" };
            ArrayAdapter arrayAdapter_spinner = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, arr_spinner);
            this.spiner_type.Adapter = arrayAdapter_spinner;
            this.spiner_type.ItemSelected += Spiner_type_ItemSelected;
            dialog_product_quantity = 1;  //כמות ברירת המחדל של מוצר חדש שמוסף 
            //Dialog Select Image
            this.dialog_Pick_Product_Image = new Dialog(this);

            dialog_Pick_Product_Image.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            this.dialog_Pick_Product_Image.SetContentView(Resource.Layout.layout_dialogPickProductImage);
            
            this.et_Dialog_url = dialog_Pick_Product_Image.FindViewById<EditText>(Resource.Id.et_DialogPickProductImage_EnterImageUrl);
            this.btn_Dialog_Download_Url = dialog_Pick_Product_Image.FindViewById<Button>(Resource.Id.btn_DialogPickProductImage_GetImageFromUrl);
            this.btn_toolbar_backpage.Visibility = ViewStates.Visible;
            this.imagebtn_toolbar_menu.Visibility = ViewStates.Invisible; 

            this.btn_Dialog_Pick_Image_From_Gallery = dialog_Pick_Product_Image.FindViewById<Button>(Resource.Id.btn_DialogPickProductImage_PickFromGallery);

            this.btn_Dialog_Save_Image = dialog_Pick_Product_Image.FindViewById<Button>(Resource.Id.btn_DialogPickProductImage_SaveDialog);
            this.iv_Dialog_Image = dialog_Pick_Product_Image.FindViewById<ImageView>(Resource.Id.imageView_DialogPickProductImage);

            this.btn_Dialog_Save_Image.Click += Btn_Dialog_Save_Image_Click;
            this.btn_Dialog_Pick_Image_From_Gallery.Click += Btn_Dialog_Pick_Image_From_Gallery_Click;
            this.btn_Dialog_Download_Url.Click += Btn_Dialog_Download_Url_Click;
            this.btn_toolbar_backpage.Click += Btn_toolbar_backpage_Click;


            this.DownloadImage_Brodcast_Receiver = new ImageBrodcastReceiver(this, this.iv_Dialog_Image);


            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted || ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                string[] permissions = new string[2];
                permissions[0] = Manifest.Permission.ReadExternalStorage;
                permissions[1] = Manifest.Permission.WriteExternalStorage;

                ActivityCompat.RequestPermissions(this, permissions, 1000);
            }
        }

        private void Spiner_type_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
           dialog_product_quantity =  int.Parse(spinner.GetItemAtPosition(e.Position).ToString()); //ממיר את הכמות ממחזרוזת למספר שלם  
            
            
        }

        private void Btn_toolbar_backpage_Click(object sender, EventArgs e)
        {
            this.imagebtn_toolbar_menu.Visibility = ViewStates.Visible;
            Intent intent = new Intent(this, typeof(Activity_ManagerHome)); 
            this.StartActivity(intent);
        }

        private void Btn_Dialog_Save_Image_Click(object sender, EventArgs e)//save the image from the dialog(url or from gallery)
        {
            BitmapDrawable bitmap_drawable = ((BitmapDrawable)iv_Dialog_Image.Drawable);//convert the image view to bitmap
            Bitmap Bitmap_Image = bitmap_drawable.Bitmap;
            this.iv_Product_Image.SetImageBitmap(Bitmap_Image);//set the imageview to the selected image from thegallery/Url

            dialog_Pick_Product_Image.Dismiss();
        }

        private void Btn_Dialog_Download_Url_Click(object sender, EventArgs e)//downloafd image  by link from the internet
        {
            if (this.et_Dialog_url.Text == "")
            {
                this.et_Dialog_url.SetError("שדה חובה", null);
                this.et_Dialog_url.RequestFocus();
                return;
            }

            if (!Android.Webkit.URLUtil.IsValidUrl(this.et_Dialog_url.Text)) // check if the link is vaild
            {
                this.et_Dialog_url.SetError("קישור לא חוקי", null);
                this.et_Dialog_url.RequestFocus();
                return;
            }

            Intent intent = new Intent(this, typeof(ServiceImageDownload)); 
            intent.PutExtra("strUrl", this.et_Dialog_url.Text);//  מעביר באינטנט את הקישור לתמונה מהאינטרנט
            StartService(intent);//start the dwonlowd of the image
        }

        private void Btn_Dialog_Pick_Image_From_Gallery_Click(object sender, EventArgs e)//move to the phone gallery
        {
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");
            StartActivityForResult(intent, 200);
        }


        protected override void OnDestroy()
        {   //מסיים את ההאזנה
            base.OnDestroy();
        }

        private void Btn_Pick_Product_Image_Click(object sender, EventArgs e)//מציג את הדיאלוג של בחירת תמונה 
        {
            this.dialog_Pick_Product_Image.Show(); 
        }



        private async void Btn_Add_New_Product_ClickAsync(object sender, EventArgs e)// מוסיף מוצר חדש לפייר בייס עם כל הנתונים שהמוכר הוסיף
        {
            try                            
            {
                string product_name = et_Name_Product.Text;//שם המוצר
                Product chek = await Product.GetProduct(product_name);
                if (chek == null)//במידה ולא קיים מוצר עם השם הזה יוסיף את המוצר
                {
                    int product_id = int.Parse(et_Id_Product.Text);//המרה של מחרוזת למספר
                    int product_price = int.Parse(et_Price_Product.Text);//המרה של המחרוזת למספר

                    
                    BitmapDrawable bitmap_drawable = ((BitmapDrawable)iv_Dialog_Image.Drawable);
                    Bitmap product_Image = bitmap_drawable.Bitmap; //תמונת המוצר
                    if (product_image_uri == null)//if the uri(the link to the place of the image in the phone files) is null end the method
                    {
                        Toast.MakeText(this, "ישנה שגיאה נסה שנית", ToastLength.Short).Show();
                        return;
                    }

                    Product.AddProduct(this, product_id, product_name, product_price, product_image_uri, dialog_product_quantity);

                }
                else
                {
                    Toast.MakeText(this, "קיים מוצר עם שם זה , אנא הוסף מוצר עם שם שונה ", ToastLength.Short).Show();
                }
            }

            catch (Exception)
            {
              
                Toast.MakeText(this, "ישנה שגיאה נסה שנית", ToastLength.Short).Show();

            }

        }



        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)//כאשר חוזרים מהגלריה
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 200)
            {
                if (resultCode == Result.Ok)
                {
                    if (data != null)//בודק שבאמת קיבלתי תמונה
                    {
                       product_image_uri = data.Data;//מביא את המיקום של התמונה(הקישור) 
                        Bitmap Bitmap_Image = MediaStore.Images.Media.GetBitmap(this.ContentResolver, product_image_uri);//מביא את התמונה באמצעות הקישור

                        if (Bitmap_Image != null)
                        {
                            this.iv_Dialog_Image.SetImageBitmap(Bitmap_Image);//מכניס את התמונה לדיאלוג
                        }
                        else
                        {
                            Toast.MakeText(this, "ישנה שגיאה נסה שנית", ToastLength.Short).Show();
                        }
                    }

                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if(requestCode == 1000)
            {
                if(grantResults[0] == Permission.Denied || grantResults[0] == Permission.Denied)
                {
                    Toast.MakeText(this, "אנא אפשר גישה כדי להעלות מוצר לחנות", ToastLength.Long).Show();
                    Finish();
                    return;
                }
            }
        }

    }
}