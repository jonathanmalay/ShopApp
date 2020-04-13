using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;

namespace ShopApp
{

    public class Fragment_ManagerOrders : Android.Support.V4.App.Fragment
    {
        ISharedPreferences sp;
        string userName;
        FloatingActionButton fab_add_NewOrder;
        Manager_Order currentOrder; 

        ListView lv_ManagerOrders;
        Adapter_ManagerOrders orders_adapter;

        //Dialog order
        Dialog dialog_order;
        ListView lvCartDialog;

        Switch switch_DialogEditOrderIsOrderSentToClient; 
        TextView tvHeaderCartDialog;
        Button btnCloseCartDialog , btnCartDialogOrderCallClient, btnCartDialogDeleteOrder;
        int selected_order;
        TextView tv_toolbar_title;
        List<Product> allProducts;
        ProgressDialog pd;
        List<Manager_Order> orders;
        TextView tv_dialog_ClientCreditCard_Number , tv_dialog_ClientCreditCard_ExpirisionDate , tv_dialog_ClientCreditCard_CVV; 
      



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

            pd = ProgressDialog.Show(Activity, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true); //progress daialog....
            pd.SetProgressStyle(ProgressDialogStyle.Horizontal);//סוג הדיאלוג שיהיה
            pd.SetCancelable(false);//שלוחצים מחוץ לדיאלוג האם הוא יסגר

            this.fab_add_NewOrder = view.FindViewById<FloatingActionButton>(Resource.Id.fab_Manager_addNewOrder);
            this.lv_ManagerOrders = view.FindViewById<ListView>(Resource.Id.listView_ManagerOrders);
            
            this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");


            this.orders = new List<Manager_Order>();//רשימה של  כל המוצרים שקיימים בחנות
            this.orders = await Manager_Order.GetAllOrders();
            
            

           this.orders_adapter = new Adapter_ManagerOrders(Activity,orders); //מכניס לתוך האדפטר את הרשימה עם כל ההזמנות של החנות 

            this.lv_ManagerOrders.Adapter = this.orders_adapter;//אומר לליסט ויואו שהוא עובד עם המתאם הזה

            this.orders_adapter.NotifyDataSetChanged(); //הפעלת המתאם
            this.pd.Hide();  
            this.lv_ManagerOrders.ItemClick += Lv_ManagerOrders_ItemClick;
            this.fab_add_NewOrder.Click += Fab_add_NewOrder_Click;


            CreateDialogOrder();
            


            
        }

        public async void CreateDialogOrder()//create the order dialog
        {
            //Dialog order
            this.dialog_order = new Dialog(Activity);
            this.dialog_order.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            this.dialog_order.SetCancelable(false); //לא ניתן לסגור אותו על ידי לחיצה מחוץ לדיאלוג
            this.dialog_order.SetContentView(Resource.Layout.layout_ManagerDailogOrderCart);
            this.lvCartDialog = this.dialog_order.FindViewById<ListView>(Resource.Id.listViewManagerDialogOrderCart);
            this.tvHeaderCartDialog = this.dialog_order.FindViewById<TextView>(Resource.Id.tv_ManagerDialogOrderCartHeader);
            this.btnCloseCartDialog = this.dialog_order.FindViewById<Button>(Resource.Id.btn_ManagerDialogOrderCartClose);
            this.btnCartDialogDeleteOrder = this.dialog_order.FindViewById<Button>(Resource.Id.btn_ManagerDialogOrderCartDeleteOrder);
            this.btnCartDialogOrderCallClient = this.dialog_order.FindViewById<Button>(Resource.Id.btn_ManagerDialogOrderCartCallClient);
            this.tv_dialog_ClientCreditCard_Number = this.dialog_order.FindViewById<TextView>(Resource.Id.tv_ManagerDialogOrderCart_ClientCreditCardNumber);
            this.tv_dialog_ClientCreditCard_ExpirisionDate = this.dialog_order.FindViewById<TextView>(Resource.Id.tv_ManagerDialogOrderCart_ClientCreditCardDate);
            this.tv_dialog_ClientCreditCard_CVV = this.dialog_order.FindViewById<TextView>(Resource.Id.tv_ManagerDialogOrderCart_ClientCreditCardCvv);
            this.switch_DialogEditOrderIsOrderSentToClient = this.dialog_order.FindViewById<Switch>(Resource.Id.switch_manager_dialog_EditOrder_IsSent);

            this.btnCartDialogOrderCallClient.Click += BtnCartDialogOrderCallClient_Click;

            this.btnCartDialogDeleteOrder.Click += BtnCartDialogDeleteOrder_Click;
            this.btnCloseCartDialog.Click += BtnCloseCartDialog_Click;
            this.switch_DialogEditOrderIsOrderSentToClient.CheckedChange += Switch_DialogEditOrderIsOrderSentToClient_CheckedChange;

            this.allProducts = await Product.GetAllProduct();
        }

        private async void Switch_DialogEditOrderIsOrderSentToClient_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {

            currentOrder = this.orders[this.selected_order];

            string order_deliverd_id = currentOrder.ID.ToString(); // מספר ההזמנה 

            if (e.IsChecked)
            {

                bool check_flag = await Manager_Order.OrderDeliverd(order_deliverd_id, true); //משנה את הסטטוס של ההזמנה בצד השרת לבוצעה
                if (check_flag)
                {
                    this.orders[this.selected_order].IsDelivered = true;

                    this.orders_adapter.NotifyDataSetChanged(); //הפעלת המתאם
                 
                }
                else
                {
                    Toast.MakeText(this.Activity, "!!אירעה שגיאה אנא בדוק את החיבור לרשת האינטרנט", ToastLength.Long).Show();
                }

            }
           

            else
            {

                bool check_flag = await Manager_Order.OrderDeliverd(order_deliverd_id , false ); //משנה את הסטטוס של ההזמנה בצד השרת ללא נשלחה
                if (check_flag)
                {
                    this.orders[this.selected_order].IsDelivered = false;

                    this.orders_adapter.NotifyDataSetChanged(); //הפעלת המתאם
                  
                }
                else
                {
                    Toast.MakeText(this.Activity, "!!אירעה שגיאה אנא בדוק את החיבור לרשת האינטרנט", ToastLength.Long).Show();
                }

            }

        }

        private void BtnCartDialogOrderCallClient_Click(object sender, EventArgs e)//צחייג ללקוח שביצע את ההזמנה 
        {
            Intent intent = new Intent();

            intent.SetAction(Intent.ActionDial);

      
            Android.Net.Uri data = Android.Net.Uri.Parse("tel:" + orders[selected_order].ClientPhone.ToString()); //חייוג

            intent.SetData(data);

            StartActivity(intent);
        }

        private async  void Fab_add_NewOrder_Click(object sender, EventArgs e)// open a dialog of set new order
        {
            pd.Show();
            bool flag_is_deleted = await Manager_Order.DeleteManagerCart(userName);//delete the manager products documents in  cart from the server before moving the next page .....
            if(flag_is_deleted)
            {
                Intent intent = new Intent(Activity, typeof(Activity_ManagerAddNewOrder));
                this.StartActivity(intent);
                pd.Hide();

            }

            else
            {
                pd.Hide();  
                Toast.MakeText(this.Activity, "אירעה שגיאה , אנא בדוק את החיבור לאינטרנט", ToastLength.Long).Show();

            }

        }

        private async  void BtnCartDialogDeleteOrder_Click(object sender, EventArgs e)
        {
            Manager_Order currentOrder = this.orders[this.selected_order];
            string order_deliverd_id = currentOrder.ID.ToString(); // מספר ההזמנה 
            bool check_flag = await Manager_Order.DeleteOrder(order_deliverd_id); //מוחק את ההזמנה ממאגר הנתונים בשרת 
            if (check_flag)
            {
                Toast.MakeText(this.Activity, "!! הזמנה נמחקה בהצלחה", ToastLength.Long).Show();

                this.orders.RemoveAt(this.selected_order);
                this.orders_adapter.NotifyDataSetChanged(); //הפעלת המתאם
                this.dialog_order.Dismiss();
            }
            else
            {
                this.orders_adapter.NotifyDataSetChanged(); //הפעלת המתאם

                Toast.MakeText(this.Activity, "!!אירעה שגיאה אנא בדוק את החיבור לרשת האינטרנט", ToastLength.Long).Show();

            }
        }


     

        private void BtnCloseCartDialog_Click(object sender, EventArgs e)
        {
            this.dialog_order.Dismiss();
        }



        private async void Lv_ManagerOrders_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;//מיקום המוצר בליסט ויאו
            Manager_Order Selected_order = this.orders[position];//מכניס לעצם מסוג מוצר  את המוצר שנמצא בתא שנלחץ בליסט ויאו 
            List<SelectedProduct> orderCart = Selected_order.CartList;
            this.selected_order = position;

            if(Selected_order.IsDelivered)
            {
                this.switch_DialogEditOrderIsOrderSentToClient.Checked = true;//אם ההזמנה נשלחה ללקוח הוא יהיה מסומן 
                
            }

            else
            {
                this.switch_DialogEditOrderIsOrderSentToClient.Checked = false;//אם ההזמנה עדיין לא נשלחה הוא לא יהיה מסומן
            }
            
            this.tvHeaderCartDialog.Text = Selected_order.ID + ":מזהה הזמנה";

            Payment user_payment_details = await Payment.GetUserPaymentDetails(Selected_order.ClientUsername);

            this.tv_dialog_ClientCreditCard_Number.Text = user_payment_details.CardNum;
            this.tv_dialog_ClientCreditCard_ExpirisionDate.Text = user_payment_details.Date;
            this.tv_dialog_ClientCreditCard_CVV.Text = user_payment_details.CVV; 

            Adapter_FinishOrder_SelectedProducts adapter_cart = new Adapter_FinishOrder_SelectedProducts(Activity, orderCart, allProducts);//אדפטר שמציג את כל המוצרים שהמשתמש הזמין בהזמנה 

            this.lvCartDialog.Adapter = adapter_cart;

            adapter_cart.NotifyDataSetChanged();
            dialog_order.Show(); //מפעיל את הדיאלוג
        }
    }
}