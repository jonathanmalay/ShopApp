using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ShopApp
{
    [Activity(Label = "Activity_ManagerAddNewOrder")]
    public class Activity_ManagerAddNewOrder : Activity
    {

        string userName;
        int Total_Price;
        ProgressDialog pd;
        ISharedPreferences sp;

        Dialog dialogAddProduct; 
        TextView tvcurrentAmountProduct, tv_toolbar_title; //הכמות הנוכחית של מוצר בקנייה
        Button btnMovetoOrderdetails, btn_toolbar_menu , btn_toolbar_backPage;
        List<SelectedProduct> list_selectedProducts;
        SelectedProduct cartSelectedProduct;
        ProductAdapter pa;
        GridView gridview_products;

        Toolbar toolbar;



        //dialog add order by manager
        Dialog dialog_AddNewOrder;
        Adapter_FinishOrder_SelectedProducts adapter_selected_products;
        ListView lv_AddOrderDialogCartDialog;
        Button btn_AddOrderDialogCartDialogOrderSaveOrder, btn_AddOrderDialogCloseCartDialog ;
        EditText et_AddOrderDialogUserNameOfCustomer , et_AddOrderDialogCustomerAddress, et_AddOrderDialogCustomerCity, et_AddOrderDialogCustomerPhone;
        TextView tv_AddOrderDialogOrderTotalPrice; 
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ClientOrder_Layout);


            this.gridview_products = this.FindViewById<GridView>(Resource.Id.gridView_ClientOrder);
            this.btnMovetoOrderdetails = this.FindViewById<Button>(Resource.Id.btnClientOrderLayoutMoveToPayment);
            this.btnMovetoOrderdetails.Text = "הבא";
            this.sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");


            CreateAddProductToCartDialog();
      

            list_selectedProducts = new List<SelectedProduct>(); //בפעולה של הדיאלוג שורה לפני הוא מקבל את כל הערכים 
            
            List<Product> products = new List<Product>();//רשימה של  כל המוצרים שקיימים בחנות
            products = await Product.GetAllProduct();

            this.pa = new ProductAdapter(this, products, list_selectedProducts);//מקבל אקטיביטי ואת רשימת המוצרים בחנות ואת רשימת המוצרים (שיש למשתמש שהמנהל בחר לשלוח לו את המוצר ) בעגלה
            this.gridview_products.Adapter = this.pa;//אומר לגריד ויואו שהוא עובד עם המתאם הזה
            this.pa.NotifyDataSetChanged(); //הפעלת המתאם

            this.gridview_products.ItemClick += Gridview_products_ItemClick; ;
            this.btnMovetoOrderdetails.Click += BtnMovetoOrderdetails_Click; ;
          //  this.btn_toolbar_backPage.Click += Btn_toolbar_backPage_Click;
        }

        private void Btn_toolbar_backPage_Click(object sender, EventArgs e)
        {
            this.Finish(); 
        }

        private async void BtnMovetoOrderdetails_Click(object sender, EventArgs e)//move to the addproduct dialog to set the orders details
        {
            bool Is_Okay = await Conrife_Order_Minimum_Price();
            if (Is_Okay)//אם סכום ההזמנה קטן מחמישים שקלים  לא יוכל לעבור לאקטיביטי ביצוע תשלום
            {
                CreateConrifeOrderDialog();
                dialog_AddNewOrder.Show();

            }

            else
            {
                Toast.MakeText(this, " סכום ההזמנה המינימלי הינו 50 שקלים,על מנת לבצע תשלום אנא הוסף פריטים על מנת להגיע לסכום זה!!", ToastLength.Long).Show();

            }
        }

        public async Task<bool> Conrife_Order_Minimum_Price()
        {
            int price_check = await SelectedProduct.Calculate_TotalOrderPrice(userName);
            if (price_check < 50)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private  async void Gridview_products_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;//מיקום המוצר בליסט ויאו
            Product selectedProduct = this.pa[position];//מכניס לעצם מסוג מוצר  את המוצר שנמצא בתא שנלחץ בליסט ויאו 
            SelectedProduct productFromFirebase = await SelectedProduct.GetProductInCart(selectedProduct.Name, userName);

            if (productFromFirebase != null) //אם המוצר כבר קיים בעגלה
            {
                cartSelectedProduct = productFromFirebase;//מכניס לעצם את העצם שבפייר בייס שהוא המוצר בתוך העגלה 
            }
            else
            {
                cartSelectedProduct = new SelectedProduct(selectedProduct.Name);
            }

            tvcurrentAmountProduct.Text = cartSelectedProduct.Amount.ToString();

            dialogAddProduct.Show(); 

        }


        public void CreateAddProductToCartDialog()
        {
            dialogAddProduct = new Dialog(this);

            Button btnPlusProduct, btnMinusProduct, btnSaveProductAmount, btn_close_dialog;

            dialogAddProduct.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            this.dialogAddProduct.SetCancelable(false); 

            dialogAddProduct.SetContentView(Resource.Layout.layoutAddProductDialog);
            dialogAddProduct.SetTitle("הוספת מוצר");
           

            tvcurrentAmountProduct = dialogAddProduct.FindViewById<TextView>(Resource.Id.tvDialogAddProductCurrentAmount);
            btnMinusProduct = dialogAddProduct.FindViewById<Button>(Resource.Id.btnDialogAddProductMinus); //כפתור ההורדה של הכמות
            btnPlusProduct = dialogAddProduct.FindViewById<Button>(Resource.Id.btnDialogAddProductPlus); //כפתור ההוספה של הכמות 
            btnSaveProductAmount = dialogAddProduct.FindViewById<Button>(Resource.Id.btnDialogAddProductSave); //כפתור השמירה
            btn_close_dialog = dialogAddProduct.FindViewById<Button>(Resource.Id.btn_CloentOrderActivity_dialogAddProduct_CloseDialog);
            btnPlusProduct.Click += (senderD, eD) =>
            {
                cartSelectedProduct.Amount++; //מוסיף אחד לכמות
                tvcurrentAmountProduct.Text = cartSelectedProduct.Amount.ToString();
            };
            btnMinusProduct.Click += (senderD, eD) =>
            {
                if (cartSelectedProduct.Amount > 0)//מחסר מהכמות רק אם היא גדולה מ0 על מנת למנוע כמות שלילית של מוצר.
                {
                    cartSelectedProduct.Amount--; //מוריד אחד לכמות
                    tvcurrentAmountProduct.Text = cartSelectedProduct.Amount.ToString();

                }
                else
                {
                    cartSelectedProduct.Amount = 0;

                    Toast.MakeText(this, "אנא הכנס כמות גדולה מ-0", ToastLength.Long).Show();

                }
            };

            btn_close_dialog.Click += (senderD, eD) =>
            {
                dialogAddProduct.Dismiss();  

            };

            btnSaveProductAmount.Click += BtnSaveProductAmount_Click;
        }


        private void BtnSaveProductAmount_Click(object sender, EventArgs e)//save the amount of the products that the user choose
        {

            SelectedProduct.AddSelectedProduct(this, this.userName, cartSelectedProduct); //מוסיף את המוצר לעגלת הקניות כלומר לקולקשיין  עגלה בפיירבייס שבו יש מסמך עם השם של המשתמש שמחובר  לאפליקציה ובתוך המסמך יש את המוצרים שהזמין 
            Toast.MakeText(this, "הפריט נוסף לעגלת הקניות (:", ToastLength.Long).Show();

            bool exist = false;

            for (int i = 0; i < pa.CartProductsList.Count; i++)
            {
                SelectedProduct currentProduct = pa.CartProductsList[i];
                if (currentProduct.ProductName == cartSelectedProduct.ProductName)
                {
                    currentProduct.Amount = cartSelectedProduct.Amount;
                    exist = true;
                }
            }

            if (!exist && cartSelectedProduct.Amount > 0)
            {
                pa.CartProductsList.Add(cartSelectedProduct);
            }

            else if (cartSelectedProduct.Amount == 0)
            {
                SelectedProduct.Remove_Product_From_Cart(userName, cartSelectedProduct.ProductName);//מסירה את המוצר שכמותו 0 מהעגלה כי במידה ולא אעשה זאת הוא יוצג בסוף ההזמנה
                pa.CartProductsList.Remove(cartSelectedProduct); //מסיר את המוצר גם מהרשימה באדפטר מכיוון שהכמות של אותה מוצר היא 0

            }

            dialogAddProduct.Dismiss();
            pa.NotifyDataSetChanged();
        }


        public async void CreateConrifeOrderDialog()//create the dialog of conride dialog
        {
            pd = ProgressDialog.Show(this, "מאמת נתונים", "...אנא המתן", true); 
            pd.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pd.SetCancelable(false);

            this.dialog_AddNewOrder = new Dialog(this);
            this.dialog_AddNewOrder.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            this.dialogAddProduct.SetCancelable(false); 

            this.dialog_AddNewOrder.SetContentView(Resource.Layout.layout_ManagerOrdersDailogAddNewOrder);
            this.lv_AddOrderDialogCartDialog = this.dialog_AddNewOrder.FindViewById<ListView>(Resource.Id.listview_ManagerOrdersDailogAddNewOrderCart);
            this.et_AddOrderDialogUserNameOfCustomer = this.dialog_AddNewOrder.FindViewById<EditText>(Resource.Id.et_ManagerOrdersDailogAddNewOrderCustomerName);
            this.et_AddOrderDialogCustomerPhone = this.dialog_AddNewOrder.FindViewById<EditText>(Resource.Id.et_ManagerOrdersDailogAddNewOrderCustomerPhone);
            this.tv_AddOrderDialogOrderTotalPrice = this.dialog_AddNewOrder.FindViewById<TextView>(Resource.Id.tv_ManagerOrdersDailogAddNewOrderOrderTotalPrice);
            this.et_AddOrderDialogCustomerAddress = this.dialog_AddNewOrder.FindViewById<EditText>(Resource.Id.et_ManagerOrdersDailogAddNewOrderCustomerAddress);
            this.et_AddOrderDialogCustomerCity = this.dialog_AddNewOrder.FindViewById<EditText>(Resource.Id.et_ManagerOrdersDailogAddNewOrderCustomerCity);
            this.btn_AddOrderDialogCloseCartDialog = this.dialog_AddNewOrder.FindViewById<Button>(Resource.Id.btn_ManagerOrdersDailogAddNewOrderClose);
            this.btn_AddOrderDialogCartDialogOrderSaveOrder = this.dialog_AddNewOrder.FindViewById<Button>(Resource.Id.btn_ManagerOrdersDailogAddNewOrderSaveOrder);

            this.btn_AddOrderDialogCloseCartDialog.Click += Btn_AddOrderDialogCloseCartDialog_Click; ;
            this.btn_AddOrderDialogCartDialogOrderSaveOrder.Click += Btn_AddOrderDialogCartDialogOrderSaveOrder_Click; ;

            Total_Price = await SelectedProduct.Calculate_TotalOrderPrice(userName);//מחשב את המחיר הסופי של הקנייה של אותו משתמש 
            this.tv_AddOrderDialogOrderTotalPrice.Text = "מחיר סופי: " + Total_Price.ToString() + "‏₪";

     

            if (list_selectedProducts == null )
            {

                Toast.MakeText(this, "אנא הוסף מוצרים להזמנה", ToastLength.Long).Show();//מכיוון שהמנהל מבצע הזמנה בשביל משתמש שלא קיים באפליקציה לכן למשתמש הזה אין עדיין מוצרים בעגלת הקניות
           
            }

            else
            {

                List<Product> list_products = new List<Product>();//רשימה של  כל המוצרים שקיימים בחנות
                list_products = await Product.GetAllProduct();

                if (list_products == null)
                {
                    Toast.MakeText(this, "אירעה שגיאה נסה שנית", ToastLength.Long).Show();
                    
                }

                else
                {

                    this.adapter_selected_products = new Adapter_FinishOrder_SelectedProducts(this, list_selectedProducts, list_products);//מקבל אקטיביטי ואת רשימת המוצרים בחנות ואת רשימת המוצרים שיש למשתמש הנוכחי בעגלה
                    this.lv_AddOrderDialogCartDialog.Adapter = this.adapter_selected_products;
                    this.adapter_selected_products.NotifyDataSetChanged(); 
                    
                }

               
            }

            pd.Hide();
        }

        private void Btn_AddOrderDialogCartDialogOrderSaveOrder_Click(object sender, EventArgs e)//save the new  order to the database
        {
            pd = ProgressDialog.Show(this, "מאמת נתונים", "...אנא המתן", true); 
            pd.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pd.SetCancelable(false);
            btn_AddOrderDialogCartDialogOrderSaveOrder.Click += async (senderD, eD) =>
            {
                try
                {
                    DateTime correct_order_date = DateTime.Now; 
                    string order_id = await Manager_Order.Add_Order_NonExistUser(this, Total_Price, correct_order_date, et_AddOrderDialogUserNameOfCustomer.Text, false, list_selectedProducts , et_AddOrderDialogCustomerCity.Text , et_AddOrderDialogCustomerAddress.Text  , et_AddOrderDialogCustomerPhone.Text );//שולח את ההזמנה  ומכיוון שצריך ששם המסמך בפייר בייס שמכיל את ההזמנה יהיה השם של הלקוח 

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
                            Finish();
                        }
                    }
                }
                catch (Exception)
                {
                    Toast.MakeText(this, "!אנא מלא את כל השדות", ToastLength.Long).Show();
                }
            };

            pd.Hide(); 
        }

        private void Btn_AddOrderDialogCloseCartDialog_Click(object sender, EventArgs e)//close the  add order dialog 
        {
            dialogAddProduct.Dismiss();
        }
    }
}