using System;
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
        ISharedPreferences sp;
         public static   Android.Net.Uri product_image_uri;
        EditText et_Price_Product, et_Dialog_url, et_Name_Product,et_Id_Product;
        Button btn_Pick_Product_Image, btn_Add_New_Product, btn_toolbar_backpage ;
        TextView tv_toolbar_title; 
        ImageButton btn_toolbar_menu; 
        ImageView iv_Product_Image;
        Dialog dialog_Pick_Product_Image;
        Spinner spiner_type;
        int dialog_product_quantity; //כמות המוצר שנבחרת בספינר בדיאלוג של הוספת המוצר 
        Button btn_Dialog_Pick_Image_From_Gallery, btn_Dialog_Save_Image, btn_Dialog_Download_Url;//הפקדים שבתוך הדיאלוג
        ImageView iv_Dialog_Image; //התמונה הנבחרת של המוצר שתוצג בדיאלוג
        ProgressDialog pd;
        bool flag_choosefromgallery ; 
        ImageBrodcastReceiver DownloadImage_Brodcast_Receiver; 

        public static bool flag_choose_from_UrlLink ; 

        

        protected override void OnCreate(Bundle savedInstanceState)
        {   
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.Activity_ManagerAddProduct);

            this.et_Name_Product = FindViewById<EditText>(Resource.Id.etManagerAddProductName);
            this.et_Price_Product = FindViewById<EditText>(Resource.Id.etManagerAddProductPrice);
            this.et_Id_Product = FindViewById<EditText>(Resource.Id.etManagerAddProductId);
            this.btn_Add_New_Product = FindViewById<Button>(Resource.Id.btnManagerAddProductSave);
            this.btn_Pick_Product_Image = FindViewById<Button>(Resource.Id.btnManagerAddProductPickImage);
            this.btn_toolbar_backpage = FindViewById<Button>(Resource.Id.btn_toolbar_backPage); 
            this.btn_toolbar_menu = FindViewById<ImageButton>(Resource.Id.btn_toolbar_menu);
            this.iv_Product_Image = FindViewById<ImageView>(Resource.Id.ivManagerAddProductImage);
            this.spiner_type = FindViewById<Spinner>(Resource.Id.spinnerManagerAddProduct);
            this.tv_toolbar_title = FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.flag_choosefromgallery = false; //if manager choose photo from gallery 
            flag_choose_from_UrlLink = false;  //if manager download image by url link 
            this.tv_toolbar_title.Text = "הוספת מוצר";

            this.sp = GetSharedPreferences("details", FileCreationMode.Private);
            string manager_usernameloged = this.sp.GetString("Username", "");


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
            this.dialog_Pick_Product_Image.SetContentView(Resource.Layout.Dialog_ManagerPickProductImage);
            
            this.et_Dialog_url = dialog_Pick_Product_Image.FindViewById<EditText>(Resource.Id.et_DialogPickProductImage_EnterImageUrl);
            this.btn_Dialog_Download_Url = dialog_Pick_Product_Image.FindViewById<Button>(Resource.Id.btn_DialogPickProductImage_GetImageFromUrl);
            this.btn_toolbar_backpage.Visibility = ViewStates.Visible;
             

            this.btn_Dialog_Pick_Image_From_Gallery = dialog_Pick_Product_Image.FindViewById<Button>(Resource.Id.btn_DialogPickProductImage_PickFromGallery);

            this.btn_Dialog_Save_Image = dialog_Pick_Product_Image.FindViewById<Button>(Resource.Id.btn_DialogPickProductImage_SaveDialog);
            this.iv_Dialog_Image = dialog_Pick_Product_Image.FindViewById<ImageView>(Resource.Id.imageView_DialogPickProductImage);

            this.btn_Dialog_Save_Image.Click += Btn_Dialog_Save_Image_Click;
            this.btn_Dialog_Pick_Image_From_Gallery.Click += Btn_Dialog_Pick_Image_From_Gallery_Click;
            this.btn_Dialog_Download_Url.Click += Btn_Dialog_Download_Url_Click;
            this.btn_toolbar_backpage.Click += Btn_toolbar_backpage_Click;


            this.DownloadImage_Brodcast_Receiver = new ImageBrodcastReceiver(this, this.iv_Dialog_Image);
            this.RegisterReceiver(this.DownloadImage_Brodcast_Receiver, new IntentFilter("ImageDownloadStaion")); //fix the problem (was missing )

            //if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != Permission.Granted || ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            //{
            //    string[] permissions = new string[2];
            //    permissions[0] = Manifest.Permission.ReadExternalStorage;
            //    permissions[1] = Manifest.Permission.WriteExternalStorage;

            //    ActivityCompat.RequestPermissions(this, permissions, 1000);
            //}


            btn_toolbar_menu.Click += (s, arg) =>
            {  //יוצר את התפריט
                PopupMenu Manager_home_Menu = new PopupMenu(this, btn_toolbar_menu);
                Manager_home_Menu.Inflate(Resource.Menu.menu_ManagerHome);
                Manager_home_Menu.MenuItemClick += Manager_home_Menu_MenuItemClick; ;
                Manager_home_Menu.Show();

            };


           

        }

        private void Manager_home_Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)//הפעולות שמתבצעות כתוצאה מלחיצה על האפשרויות השונות בתפריט
        {
            ISharedPreferencesEditor editor = sp.Edit();

            switch (e.Item.ItemId)
            {
                case Resource.Id.menu_ManagerHomeLogOut:

                    editor.PutString("Username", "").Apply();
                    editor.PutBoolean("isManager", false).Apply();
                    Toast.MakeText(this, "you selected to log out", ToastLength.Long).Show();
                    Intent intentLogin = new Intent(this, typeof(MainActivity));
                    this.StartActivity(intentLogin);
                    break;

                case Resource.Id.action_register:

                    Intent intentRegister = new Intent(this, typeof(RegisterActivity));
                    this.StartActivity(intentRegister);
                    break;

                case Resource.Id.menu_ManagerHomeAccountSetting:

                    Intent intent = new Intent(this, typeof(Activity_ManagerHome));
                    this.StartActivity(intent);
                    break;


            }
        }

        private void Spiner_type_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
           dialog_product_quantity =  int.Parse(spinner.GetItemAtPosition(e.Position).ToString()); //ממיר את הכמות ממחזרוזת למספר שלם  
            
            
        }

        private void Btn_toolbar_backpage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activity_ManagerHome)); 
            this.StartActivity(intent);
        }

        private void Btn_Dialog_Save_Image_Click(object sender, EventArgs e)//save the image from the dialog(url or from gallery)
        {
            if(flag_choosefromgallery == false && flag_choose_from_UrlLink==false ) //if the manager didnt choose picture for the product
            {
                Toast.MakeText(this, "אנא בחר תמונה !", ToastLength.Short).Show();
                return;  
            }

            
            BitmapDrawable bitmap_drawable = ((BitmapDrawable)iv_Dialog_Image.Drawable);//convert the image view to bitmap
            Bitmap Bitmap_Image = bitmap_drawable.Bitmap;
            this.iv_Product_Image.SetImageBitmap(Bitmap_Image); //set the imageview to the selected image from thegallery/Url
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
            //איפוס לדגלים
            this.flag_choosefromgallery = false;
            flag_choose_from_UrlLink = false;

            this.dialog_Pick_Product_Image.Show(); 
        }



        private async void Btn_Add_New_Product_ClickAsync(object sender, EventArgs e)// מוסיף מוצר חדש לפייר בייס עם כל הנתונים שהמוכר הוסיף
        {
            try                            
            {
                pd = ProgressDialog.Show(this, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true);
                pd.SetProgressStyle(ProgressDialogStyle.Horizontal);
                pd.SetCancelable(false);

                string product_name = et_Name_Product.Text;//שם המוצר
                Product chek = await Product.GetProduct(product_name);
                if (chek == null)//במידה ולא קיים מוצר עם השם הזה יוסיף את המוצר
                {
                    int product_id = int.Parse(et_Id_Product.Text);//המרה של מחרוזת למספר
                    int product_price = int.Parse(et_Price_Product.Text);//המרה של המחרוזת למספר

                    
                    BitmapDrawable bitmap_drawable = (BitmapDrawable)iv_Dialog_Image.Drawable;
                    Bitmap product_Image = bitmap_drawable.Bitmap; //תמונת המוצר
                    if (product_image_uri == null)//if the uri(the link to the place of the image in the phone files) is null end the method
                    {
                        Toast.MakeText(this, "ישנה שגיאה נסה שנית", ToastLength.Short).Show();
                        pd.Cancel();
                        return;
                    }


                        Product p = await Product.AddProduct(this, product_id, product_name, product_price, product_image_uri, dialog_product_quantity);

            


                    if (p!=null)
                    {
                        Toast.MakeText(this, "המוצר הועלה בהצלחה  ", ToastLength.Short).Show();
                        pd.Cancel(); 
                        Intent intent = new Intent(this, typeof(Activity_ManagerHome));
                        StartActivity(intent); 

                    }
                    pd.Cancel();

                }
                else
                {
                    pd.Cancel(); 
                    Toast.MakeText(this, "קיים מוצר עם שם זה , אנא הוסף מוצר עם שם שונה ", ToastLength.Short).Show();

                }
            }

            catch (Exception)
            {
              
                Toast.MakeText(this, "ישנה שגיאה נסה שנית", ToastLength.Short).Show();
                pd.Cancel();

            }
            pd.Cancel();

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
                            flag_choosefromgallery = true; 
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