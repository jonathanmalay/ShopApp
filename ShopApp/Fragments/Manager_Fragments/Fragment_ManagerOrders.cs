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

namespace ShopApp
{

    public class Fragment_ManagerOrders : Android.Support.V4.App.Fragment
    {
        ISharedPreferences sp;
        string userName;

        ListView lv_ManagerOrders;
        Adapter_ManagerOrders orders_adapter;

        //Dialog
        Dialog dialog_order;
        ListView lvCartDialog;
        TextView tvHeaderCartDialog;
        Button btnCloseCartDialog;

        TextView tv_toolbar_title;

        List<Product> allProducts;





        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            return LayoutInflater.Inflate(Resource.Layout.layout_ManagerOrders, container, false);
        }

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if (hidden == false)
            {

                this.tv_toolbar_title.Text = "הזמנות";

            }
        }



        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            this.tv_toolbar_title = Activity.FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.tv_toolbar_title.Text = "הזמנות";

            this.lv_ManagerOrders = view.FindViewById<ListView>(Resource.Id.listView_ManagerOrders);
            this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");


            List<Manager_Order> orders = new List<Manager_Order>();//רשימה של  כל המוצרים שקיימים בחנות
            orders  = await Manager_Order.GetAllOrders();

           this.orders_adapter = new Adapter_ManagerOrders(Activity,orders); //מכניס לתוך האדפטר את הרשימה עם כל ההזמנות של החנות 

            this.lv_ManagerOrders.Adapter = this.orders_adapter;//אומר לליסט ויואו שהוא עובד עם המתאם הזה
            this.orders_adapter.NotifyDataSetChanged(); //הפעלת המתאם
            this.lv_ManagerOrders.ItemClick += Lv_ManagerOrders_ItemClick;



            //Dialog
            this.dialog_order = new Dialog(Activity);
            this.dialog_order.SetContentView(Resource.Layout.layout_ManagerDailogOrderCart);
            this.lvCartDialog = this.dialog_order.FindViewById<ListView>(Resource.Id.listViewManagerDialogOrderCart);
            this.tvHeaderCartDialog = this.dialog_order.FindViewById<TextView>(Resource.Id.btn_ManagerDialogOrderCartClose);
            this.btnCloseCartDialog = this.dialog_order.FindViewById<Button>(Resource.Id.tv_ManagerDialogOrderCartHeader);

            this.btnCloseCartDialog.Click += BtnCloseCartDialog_Click;

            this.allProducts = await Product.GetAllProduct();
        }

        private void BtnCloseCartDialog_Click(object sender, EventArgs e)
        {
            this.dialog_order.Dismiss();
        }

        private void Lv_ManagerOrders_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;//מיקום המוצר בליסט ויאו
            Manager_Order selected_order = this.orders_adapter[position];//מכניס לעצם מסוג מוצר  את המוצר שנמצא בתא שנלחץ בליסט ויאו 
            List<SelectedProduct> orderCart = selected_order.CartList;

            this.tvHeaderCartDialog.Text = selected_order.ID + " Cart";

            Adapter_FinishOrder_SelectedProducts adapter_cart = new Adapter_FinishOrder_SelectedProducts(Activity, orderCart, allProducts);//אדפטר שמציג את כל המוצרים שהמשתמש הזמין בהזמנה 

            this.lvCartDialog.Adapter = adapter_cart;

            adapter_cart.NotifyDataSetChanged();

            dialog_order.Show(); //מפעיל את הדיאלוג
        }
    }
}