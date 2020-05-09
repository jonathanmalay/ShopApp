using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Square.Picasso;

namespace ShopApp
{
     
    public class Fragment_ManagerProducts : Android.Support.V4.App.Fragment
    {
        ISharedPreferences sp;
        string userName;
        ProgressDialog pd; 
        Dialog dialogEditProduct;
        GridView gridview_products;
        FloatingActionButton fab_add_NewProduct;
        Product selected_product;
        Button btn_backPage;
        TextView tv_toolbar_title;
        ProductAdapter pa;


        List<SelectedProduct> selectedProducts;
        List<Product> products;
        int position; 
        //Dialog Edit Product

        TextView tv_DialogEditProduct_ProductName; 
        Button btn_DialogEditProduct_remove_product;
        Button btn_DialogEditProduct_save_changes;
        EditText et_DialogEditProduct_ProductPrice;
        EditText et_DialogEditProduct_productName;
        EditText et_DialogEditProduct_productQuantity;
        EditText et_DialogEditProduct_productCode;
        ImageView iv_DialogEditProduct_ProductImage;
        Button btn_DialogEditProduct_PickImage; 
        Android.Net.Uri product_image_uri;

        public ImageBrodcastReceiver DownloadImage_Brodcast_Receiver { get; private set; }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) //return a View from this method that is the root of the  fragment's layout.
        {
            return LayoutInflater.Inflate(Resource.Layout.Fragment_ManagerEditProducts, container, false);
        }

        public override void OnHiddenChanged(bool hidden)//whats happen every time that the fragment view again 
        {
            base.OnHiddenChanged(hidden);
            if (hidden == false)
            {

                this.tv_toolbar_title.Text = "מוצרים";
                this.pa.NotifyDataSetChanged();


            }
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState) // this method called just after onCreateView and get has parameter the inflated view. Its return type is void
        {
            base.OnViewCreated(view, savedInstanceState);

            this.fab_add_NewProduct = view.FindViewById<FloatingActionButton>(Resource.Id.fab_Manager_addNewProduct);
            this.gridview_products = view.FindViewById<GridView>(Resource.Id.GreedViewManagerEditProducts);
            this.btn_backPage = Activity.FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
            this.btn_backPage.Visibility = ViewStates.Invisible; //hide this button from the toolbar 
            this.tv_toolbar_title = Activity.FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");

           
            CreateDialogEditProduct(Activity); 

             selectedProducts = new List<SelectedProduct>();
             products = new List<Product>();//רשימה של  כל המוצרים שקיימים בחנות
             products = await Product.GetAllProduct();

            this.pa = new ProductAdapter(Activity, products, selectedProducts , 1); //מקבל אקטיביטי ואת רשימת המוצרים בחנות ואת רשימת המוצרים שיש למשתמש הנוכחי בעגלה
            this.gridview_products.Adapter = this.pa;//אומר לליסט ויואו שהוא עובד עם המתאם הזה
            this.pa.NotifyDataSetChanged(); //הפעלת המתאם
            this.gridview_products.ItemClick += GridViewProducts_ItemClick; 
            this.fab_add_NewProduct.Click += Fab_add_NewProduct_Click;

            this.DownloadImage_Brodcast_Receiver = new ImageBrodcastReceiver(Activity, this.iv_DialogEditProduct_ProductImage);


            if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.ReadExternalStorage) != Permission.Granted || ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                string[] permissions = new string[2];
                permissions[0] = Manifest.Permission.ReadExternalStorage;
                permissions[1] = Manifest.Permission.WriteExternalStorage;

                ActivityCompat.RequestPermissions(Activity, permissions, 1000);
            }

        }

        

        private void Fab_add_NewProduct_Click(object sender, EventArgs e) //move to add product screen 
        {   

            Intent intent = new Intent(Activity, typeof(Activity_ManagerAddProduct));
            this.StartActivity(intent);
        }

        private void GridViewProducts_ItemClick(object sender, AdapterView.ItemClickEventArgs e)//whats happen whan clicked on cell in the Grid view(open the EditProduct Dialog)
        {   

             position = e.Position;//מיקום המוצר בגריד ויאו
            selected_product = this.pa[position];//מכניס לעצם מסוג מוצר  את המוצר שנמצא בתא שנלחץ בגריד ויאו 

            tv_DialogEditProduct_ProductName.Text = this.selected_product.Name; 
            et_DialogEditProduct_ProductPrice.Text = this.selected_product.Price.ToString();
            et_DialogEditProduct_productQuantity.Text = this.selected_product.Quantity.ToString();
            et_DialogEditProduct_productCode.Text = this.selected_product.ProductId.ToString();

            Picasso.With(Activity).Load(this.selected_product.ImageUrl).Into(iv_DialogEditProduct_ProductImage); //insert the pphoto to cell (from firbase Storage)
            dialogEditProduct.Show(); //מפעיל את הדיאלוג
        }



        public void CreateDialogEditProduct(Activity activity)//create the edit product dialog 
        {
            
            dialogEditProduct = new Dialog(Activity);
            dialogEditProduct.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            dialogEditProduct.SetContentView(Resource.Layout.Dialog_ManagerEditProduct);
            dialogEditProduct.SetTitle("עריכת מוצר");
            dialogEditProduct.SetCancelable(true);

            tv_DialogEditProduct_ProductName = dialogEditProduct.FindViewById<TextView>(Resource.Id.tv_manager_dialog_editProduct_ProductName);
            
            et_DialogEditProduct_ProductPrice = dialogEditProduct.FindViewById<EditText>(Resource.Id.et_ManagerEditProductDialogProductPrice);
            et_DialogEditProduct_productCode = dialogEditProduct.FindViewById<EditText>(Resource.Id.et_ManagerEditProductDialogProductCode);
            et_DialogEditProduct_productQuantity = dialogEditProduct.FindViewById<EditText>(Resource.Id.et_ManagerEditProductDialogProductQuantity);
            btn_DialogEditProduct_save_changes = dialogEditProduct.FindViewById<Button>(Resource.Id.btn_ManagerEditProductDialogSave); //כפתור שמירת המוצר לאחר השינוים שביצע המנהל
            btn_DialogEditProduct_remove_product = dialogEditProduct.FindViewById<Button>(Resource.Id.btn_ManagerEditProductDialogDeleteProduct); //כפתור ההסרה של מוצר
            btn_DialogEditProduct_PickImage = dialogEditProduct.FindViewById<Button>(Resource.Id.btn_Managerdialog_edit_product_ChooseImage); 
            iv_DialogEditProduct_ProductImage = dialogEditProduct.FindViewById<ImageView>(Resource.Id.iv_dialog_edit_product_image);
            
            btn_DialogEditProduct_remove_product.Click += Btn_remove_product_Click;
            btn_DialogEditProduct_save_changes.Click += Btn_save_changes_Click;
            btn_DialogEditProduct_PickImage.Click += Btn_DialogEditProduct_PickImage_Click;
        }

        private void Btn_DialogEditProduct_PickImage_Click(object sender, EventArgs e)//move the user to the gallery for choosing the product image
        {
            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");
            StartActivityForResult(intent, 200);
        }

        private async void Btn_save_changes_Click(object sender, EventArgs e) // save the new changes of the product in the  firebase data base
        {
            try
            {
                pd = ProgressDialog.Show(Activity, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true); 
                pd.SetProgressStyle(ProgressDialogStyle.Horizontal);
                pd.SetCancelable(false);
                if (selected_product.Name.Length>2)
                {
                    selected_product.Price = int.Parse(et_DialogEditProduct_ProductPrice.Text);//המרה של מחרוזת למספר
                    selected_product.Quantity = int.Parse(et_DialogEditProduct_productQuantity.Text);//המרה של המחרוזת למספר
                    selected_product.ProductId = int.Parse(et_DialogEditProduct_productCode.Text);
                  
                    if (product_image_uri == null)//if the uri(the link to the place of the image in the phone files) is null end the method
                    {
                        await Product.AddProductWithoutImage(Activity , selected_product);
                        this.pa.NotifyDataSetChanged();
                    }

                    else
                    {

                        Product product_with_newImageURl = await Product.AddProduct(Activity, selected_product.ProductId, selected_product.Name, selected_product.Price, product_image_uri, selected_product.Quantity);//הוספת המוצר לפייר בייס

                        if (product_with_newImageURl!=null)
                        {
                            this.pa[position].ImageUrl = product_with_newImageURl.ImageUrl; 
                            this.pa.NotifyDataSetChanged();
                            Toast.MakeText(Activity, "המוצר עודכן בהצלחה ", ToastLength.Short).Show();

                        }

                        else
                        {

                        }

                   
                    }

                   
                }

                else
                {
                    Toast.MakeText(Activity, "אנא הכנס שם מוצר גדול משני תווים  ", ToastLength.Short).Show();
                    pd.Cancel();
                    return; 
                }
               
            }

            catch (Exception)
            {

                Toast.MakeText(Activity, "ישנה שגיאה נסה שנית", ToastLength.Short).Show();
                pd.Cancel();
                return;

            }

            selected_product = new Product();
            dialogEditProduct.Dismiss();
            pd.Cancel();

        }
        private async void Btn_remove_product_Click(object sender, EventArgs e)//remove the selected product from the database and the firebase storage 
        {
            try
            {
                await Product.RemoveProduct( selected_product); //מסיר את המוצר ממסד התונים.
                Product check_product = await Product.GetProduct(selected_product.Name);//בדיקה האם המוצר הוסר בהצלחה
                //נבדוק עם לאחר ההסרה של המוצר יחזור נאל ממסד הנתונים משמע שהמוצר הוסר בהצלחה
                if (check_product == null)
                {
                    Toast.MakeText(Activity, "הפריט הוסר בהצלחה (:", ToastLength.Long).Show();
                    this.pa.AllProducts.Remove(selected_product); //update the products list 
                }

                else
                {  //במידה והמוצר לא הוסר בהצלחה
                    Toast.MakeText(Activity, "אירעה שגיאה במהלך הסרת המוצר . אנא נסה שנית", ToastLength.Long).Show();

                }

                dialogEditProduct.Dismiss();
                pa.NotifyDataSetChanged();
            }


            catch (Exception)
            {

            }
        }




       public override void OnActivityResult(int requestCode, int resultCode, Intent data)//when the  manager return from the  gallary after he pick image for product
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 200)
            {
                Result convert_ResultCode = (Result)resultCode;
                if ( convert_ResultCode == Result.Ok)
                {
                    if (data != null)//בודק שבאמת קיבלתי תמונה
                    {
                        product_image_uri = data.Data;//מביא את המיקום של התמונה(הקישור) 
                        Bitmap Bitmap_Image = MediaStore.Images.Media.GetBitmap(Activity.ContentResolver, product_image_uri);//מביא את התמונה באמצעות הקישור

                        if (Bitmap_Image != null)
                        {
                            this.iv_DialogEditProduct_ProductImage.SetImageBitmap(Bitmap_Image);
                        }
                        else
                        {
                            Toast.MakeText(Activity, "ישנה שגיאה נסה שנית", ToastLength.Short).Show();
                        }
                    }

                }
            }

            else
            {
                Toast.MakeText(Activity, "ישנה שגיאה במעבר לגלריה אנא  נסה שנית", ToastLength.Short).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)//check if the manager smartphone give the app premission to the storage in the phone
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == 1000)
            {
                if (grantResults[0] == Permission.Denied || grantResults[0] == Permission.Denied)
                {
                    Toast.MakeText(Activity, "אנא אפשר גישה כדי להעלות מוצר לחנות", ToastLength.Long).Show();
                   // Activity.Finish();
                    return;
                }
            }
        }


    }
}