using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ShopApp
{
     
    public class Fragment_ManagerProducts : Android.Support.V4.App.Fragment
    {
        ISharedPreferences sp;
        string userName;

        Dialog dialogEditProduct, dialogAddProduct;
        ListView lvProducts;
        FloatingActionButton fab_add_NewProduct;
        Product selected_product;
        Button btn_backPage;
        TextView tv_toolbar_title;
        ProductAdapter pa;


        //Dialog Edit Product
        Button btn_DialogEditProduct_remove_product;
        Button btn_DialogEditProduct_save_changes;
        EditText et_DialogEditProduct_ProductPrice;
        EditText et_DialogEditProduct_productName;
        EditText et_DialogEditProduct_productQuantity;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.layout_ManagerEditProducts, container, false);
        }

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if (hidden == false)
            {

                this.tv_toolbar_title.Text = "מוצרים";

            }
        }



        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);



            this.fab_add_NewProduct = view.FindViewById<FloatingActionButton>(Resource.Id.fab_Manager_addNewProduct);
            this.lvProducts = view.FindViewById<ListView>(Resource.Id.listviewManagerEditProducts);
            this.btn_backPage = Activity.FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
            this.btn_backPage.Visibility = ViewStates.Invisible; //hide this button from the toolbar 
            this.tv_toolbar_title = Activity.FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");

            CreateDialogAddProduct(Activity);
            CreateDialogEditProduct(Activity); 

            List<SelectedProduct> selectedProducts = new List<SelectedProduct>();

            List<Product> products = new List<Product>();//רשימה של  כל המוצרים שקיימים בחנות
            products = await Product.GetAllProduct();

            this.pa = new ProductAdapter(Activity, products, selectedProducts);//מקבל אקטיביטי ואת רשימת המוצרים בחנות ואת רשימת המוצרים שיש למשתמש הנוכחי בעגלה
            this.lvProducts.Adapter = this.pa;//אומר לליסט ויואו שהוא עובד עם המתאם הזה
            this.pa.NotifyDataSetChanged(); //הפעלת המתאם
            this.lvProducts.ItemClick += LvProducts_ItemClick; ;
            this.fab_add_NewProduct.Click += Fab_add_NewProduct_Click;
           
        }

        

        private void Fab_add_NewProduct_Click(object sender, EventArgs e)
        {
            this.dialogAddProduct.Show(); 

        }

        private void LvProducts_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {   

            int position = e.Position;//מיקום המוצר בליסט ויאו
            selected_product = this.pa[position];//מכניס לעצם מסוג מוצר  את המוצר שנמצא בתא שנלחץ בליסט ויאו 

            et_DialogEditProduct_productName.Text = this.selected_product.Name;
            et_DialogEditProduct_ProductPrice.Text = this.selected_product.Price.ToString();
            et_DialogEditProduct_productQuantity.Text = this.selected_product.Quantity.ToString();
            
            dialogEditProduct.Show(); //מפעיל את הדיאלוג
        }



        public void CreateDialogEditProduct(Activity activity)
        {
            
            dialogEditProduct = new Dialog(Activity);
            dialogAddProduct.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            dialogAddProduct.SetContentView(Resource.Layout.view_ManagerEditProduct);
            dialogAddProduct.SetTitle("עריכת מוצר");
            dialogAddProduct.SetCancelable(true);

            et_DialogEditProduct_productName = dialogEditProduct.FindViewById<EditText>(Resource.Id.et_ManagerEditProductDialogProductName);
            et_DialogEditProduct_ProductPrice = dialogEditProduct.FindViewById<EditText>(Resource.Id.et_ManagerEditProductDialogProductPrice);
            et_DialogEditProduct_productQuantity = dialogEditProduct.FindViewById<EditText>(Resource.Id.et_ManagerEditProductDialogProductQuantity);
            btn_DialogEditProduct_save_changes = dialogEditProduct.FindViewById<Button>(Resource.Id.btn_ManagerEditProductDialogSave);
            btn_DialogEditProduct_remove_product = dialogEditProduct.FindViewById<Button>(Resource.Id.btn_ManagerEditProductDialogDeleteProduct); //כפתור ההסרה של מוצר

            btn_DialogEditProduct_remove_product.Click += Btn_remove_product_Click;
            btn_DialogEditProduct_save_changes.Click += Btn_save_changes_Click;

        }

        private void Btn_save_changes_Click(object sender, EventArgs e)
        {
          
        }

        public void CreateDialogAddProduct(Activity activity)
        {
            dialogAddProduct = new Dialog(Activity);

           

            dialogAddProduct.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            dialogAddProduct.SetContentView(Resource.Layout.layout_ManagerAddProduct);
            dialogAddProduct.SetTitle("הוספת מוצר");
            dialogAddProduct.SetCancelable(true);



        }

        private async void Btn_remove_product_Click(object sender, EventArgs e)
        {

            try
            {

                await Product.RemoveProduct(this.userName, selected_product); //מסיר את המוצר ממסד התונים.
                Product check_product = await Product.GetProduct(selected_product.Name);//בדיקה האם המוצר הוסר בהצלחה
                //נבדוק עם לאחר ההסרה של המוצר יחזור נאל ממסד הנתונים משמע שהמוצר הוסר בהצלחה
                if (check_product == null)
                {

                    Toast.MakeText(Activity, "הפריט הוסר בהצלחה (:", ToastLength.Long).Show();

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
    }
}