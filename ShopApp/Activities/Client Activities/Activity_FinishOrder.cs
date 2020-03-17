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

namespace ShopApp.Activities
{
    [Activity(Label = "Activity_FinishOrder")]
    public class Activity_FinishOrder : Activity
    {
        Dialog dialog_conrife_Order;
        string userName;
        int Total_Price;
        bool Is_Payed;
        ISharedPreferences sp;
        ListView lv_Selected_Products;
        TextView tv_Total_Price,tv_toolbar_title; 
        Button btn_conrife_order,btn_BackPage;
        List<SelectedProduct> list_selectedProducts;
        ProgressDialog pd;


      
        Adapter_FinishOrder_SelectedProducts adapter_selected_products;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.layout_FinishOrder);

                this.tv_toolbar_title = FindViewById<TextView>(Resource.Id.tv_toolbar_title);
                this.tv_toolbar_title.Text = " סיום הזמנה";

                pd = ProgressDialog.Show(this, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true); //progress daialog....
                pd.SetProgressStyle(ProgressDialogStyle.Horizontal);//סוג הדיאלוג שיהיה
                pd.SetCancelable(false);//שלוחצים מחוץ לדיאלוג האם הוא יסגר

                this.lv_Selected_Products = FindViewById<ListView>(Resource.Id.listViewFinishOrder);
                this.btn_conrife_order = FindViewById<Button>(Resource.Id.btnFinishOrderConrifeOrder);
                this.btn_BackPage = FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
                this.tv_Total_Price = FindViewById<TextView>(Resource.Id.tvFinishOrderTotalPrice);
                this.sp = GetSharedPreferences("details", FileCreationMode.Private);
                this.userName = this.sp.GetString("Username", "");

                Total_Price = await SelectedProduct.Calculate_TotalOrderPrice(userName);//מחשב את המחיר הסופי של הקנייה של אותו משתמש 
                this.tv_Total_Price.Text = Total_Price.ToString() + " :מחיר סופי" ;
                CreateDialog(this);

                btn_BackPage.Click += Btn_BackPage_Click;
                btn_conrife_order.Click += Btn_conrife_order_Click;
                list_selectedProducts = new List<SelectedProduct>();
                list_selectedProducts = await SelectedProduct.GetAllProductInCart(userName);//מביא  רשימה של כל המוצרים שיש לאותו משתמש בעגלה 
                if (list_selectedProducts == null)
                {

                    Toast.MakeText(this, "אירעה שגיאה  נסה שנית", ToastLength.Long).Show();
                    pd.Cancel();
                }

                else
                {

                    List<Product> list_products = new List<Product>();//רשימה של  כל המוצרים שקיימים בחנות
                    list_products = await Product.GetAllProduct();

                    if (list_products == null)
                    {
                        Toast.MakeText(this, "אירעה שגיאה נסה שנית", ToastLength.Long).Show();
                        pd.Cancel();
                    }

                    else
                    {

                        this.adapter_selected_products = new Adapter_FinishOrder_SelectedProducts(this, list_selectedProducts, list_products);//מקבל אקטיביטי ואת רשימת המוצרים בחנות ואת רשימת המוצרים שיש למשתמש הנוכחי בעגלה
                        this.lv_Selected_Products.Adapter = this.adapter_selected_products;//אומר לליסט ויואו שהוא עובד עם המתאם הזה
                        this.adapter_selected_products.NotifyDataSetChanged(); //הפעלת המתאם
                        pd.Cancel();
                    }

                }

            }

            catch (Exception)
            {
                Toast.MakeText(this, "אירעה שגיאה נסה שנית", ToastLength.Long).Show();
                pd.Cancel();
            }

        }




        
        private void Btn_BackPage_Click(object sender, EventArgs e)
        {
            Finish(); //מוחק את האקטיביטי מהמחסנית של האקטיביטים
        }






        public void CreateDialog(Activity activity)
        {
             dialog_conrife_Order = new Dialog(this);

            Button btn_conrife_dialog_save;

            dialog_conrife_Order.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            dialog_conrife_Order.SetContentView(Resource.Layout.layout_DialogFinishOrder_Conrife);
            dialog_conrife_Order.SetTitle("אישור סופי");
            dialog_conrife_Order.SetCancelable(true);

      
           
            btn_conrife_dialog_save = dialog_conrife_Order.FindViewById<Button>(Resource.Id.btn_DialogFinishOrderConrife); //כפתור השמירה

            btn_conrife_dialog_save.Click += async (senderD, eD) =>
            {
                try
                {
                    DateTime correct_order_date = DateTime.Now; //הזמן הנוכחי 
                    string order_id = await Manager_Order.Add_Order(this, Total_Price, correct_order_date, userName, false,list_selectedProducts);//שולח את ההזמנה 

                    if (order_id != null)
                    {
                        Manager_Order order_check = await Manager_Order.GetOrder(order_id);//במידה וחוזר עצם מסוג הזמנה ההזמנה נשלחה בהצלחה 
                        if (order_check == null)
                        {
                            Toast.MakeText(this, "אירעה שגיאה!!", ToastLength.Long).Show();
                        }

                        else
                        {
                            Toast.MakeText(this, "!!ההזמנה בוצעה בהצלחה", ToastLength.Long).Show();
                            Intent intent = new Intent(this, typeof(HomeActivity)); 
                            this.StartActivity(intent);
                        }
                    }


                }

                catch (Exception)
                {
                    Toast.MakeText(this, "אירעה שגיאה!!", ToastLength.Long).Show();
                }
            };
        }


        private void Btn_conrife_order_Click(object sender, EventArgs e)
        {
            dialog_conrife_Order.Show(); //מפעיל את הדיאלוג


        }

        public override void OnBackPressed()
        {

        }
    }
}