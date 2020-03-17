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
    [Activity(Label = "Activity_ManagerProducts")]
    public class Activity_ManagerProducts : AppCompatActivity
    {
        ISharedPreferences sp;
        string userName;

        Dialog dialogRemoveProduct;
        ListView lvProducts;
        FloatingActionButton fab_add_NewProduct;
        Product selected_product;
        Button btn_backPage;
        TextView tv_toolbar_title;
        ProductAdapter pa;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_ManagerEditProducts);


            this.fab_add_NewProduct = FindViewById<FloatingActionButton>(Resource.Id.fab_Manager_addNewProduct);
            this.lvProducts = FindViewById<ListView>(Resource.Id.listviewManagerRemoveProduct);
            this.btn_backPage = FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
            this.tv_toolbar_title = FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.sp = GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");

            CreateDialog(this);

            List<SelectedProduct> selectedProducts = new List<SelectedProduct>();

            List<Product> products = new List<Product>();//רשימה של  כל המוצרים שקיימים בחנות
            products = await Product.GetAllProduct();

            this.pa = new ProductAdapter(this, products, selectedProducts);//מקבל אקטיביטי ואת רשימת המוצרים בחנות ואת רשימת המוצרים שיש למשתמש הנוכחי בעגלה
            this.lvProducts.Adapter = this.pa;//אומר לליסט ויואו שהוא עובד עם המתאם הזה
            this.pa.NotifyDataSetChanged(); //הפעלת המתאם
            this.lvProducts.ItemClick += LvProducts_ItemClick; ;
            this.fab_add_NewProduct.Click += Fab_add_NewProduct_Click;
            this.btn_backPage.Click += Btn_backPage_Click;
        }

        private void Btn_backPage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activity_ManagerHome));
            StartActivity(intent);
        }

        private void Fab_add_NewProduct_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activity_ManagerAddProduct));
            StartActivity(intent);

        }

        private void LvProducts_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;//מיקום המוצר בליסט ויאו
            selected_product = this.pa[position];//מכניס לעצם מסוג מוצר  את המוצר שנמצא בתא שנלחץ בליסט ויאו 
            dialogRemoveProduct.Show(); //מפעיל את הדיאלוג
        }

        public void CreateDialog(Activity activity)
        {
            dialogRemoveProduct = new Dialog(this);

            Button btn_remove_product;

            dialogRemoveProduct.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            dialogRemoveProduct.SetContentView(Resource.Layout.layout_ManagerRemoveProductDialog);
            dialogRemoveProduct.SetTitle("הוספת מוצר");
            dialogRemoveProduct.SetCancelable(true);


            btn_remove_product = dialogRemoveProduct.FindViewById<Button>(Resource.Id.btnManagerRemoveProductDialog); //כפתור ההסרה של מוצר
            btn_remove_product.Click += Btn_remove_product_Click; ;

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

                    Toast.MakeText(this, "הפריט הוסר בהצלחה (:", ToastLength.Long).Show();

                }

                else
                {  //במידה והמוצר לא הוסר בהצלחה
                    Toast.MakeText(this, "אירעה שגיאה במהלך הסרת המוצר . אנא נסה שנית", ToastLength.Long).Show();

                }

                dialogRemoveProduct.Dismiss();
                pa.NotifyDataSetChanged();
            }


            catch (Exception)
            {

            }
        }
    }
}