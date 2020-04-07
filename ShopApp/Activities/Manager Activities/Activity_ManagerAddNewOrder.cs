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

namespace ShopApp.Activities.Manager_Activities
{
    [Activity(Label = "Activity_ManagerAddNewOrder")]
    public class Activity_ManagerAddNewOrder : Activity
    {

        string userName;
        Adapter_FinishOrder_SelectedProducts adapter_selected_products;
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



        //dialog add order by manager
        Dialog dialog_AddNewOrder;
        ListView lv_AddOrderDialogCartDialog;
        Button btn_AddOrderDialogCartDialogOrderSaveOrder, btn_AddOrderDialogCloseCartDialog ;
        EditText et_AddOrderDialogUserNameOfCustomer, et_AddOrderDialogOrderTotalPrice;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ClientOrder_Layout);


            this.btn_toolbar_backPage = this.FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
            this.btn_toolbar_menu = this.FindViewById<Button>(Resource.Id.btn_toolbar_menu) ; 
            this.tv_toolbar_title = this.FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.tv_toolbar_title.Text = "הוספת הזמנה ";
            this.btn_toolbar_backPage.Visibility = ViewStates.Visible; 
            this.btn_toolbar_menu.Visibility = ViewStates.Invisible; //hide this button from the toolbar 




            this.gridview_products = this.FindViewById<GridView>(Resource.Id.gridView_ClientOrder);
            this.btnMovetoOrderdetails = this.FindViewById<Button>(Resource.Id.btnClientOrderLayoutMoveToPayment);
            this.sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");

            CreateAddProductToCartDialog();
            CreateConrifeOrderDialog();

            list_selectedProducts = new List<SelectedProduct>(); //בפעולה של הדיאלוג שורה לפני הוא מקבל את כל הערכים 
            

            List<Product> products = new List<Product>();//רשימה של  כל המוצרים שקיימים בחנות
            products = await Product.GetAllProduct();

            this.pa = new ProductAdapter(this, products, list_selectedProducts);//מקבל אקטיביטי ואת רשימת המוצרים בחנות ואת רשימת המוצרים (שיש למשתמש שהמנהל בחר לשלוח לו את המוצר ) בעגלה
            this.gridview_products.Adapter = this.pa;//אומר לגריד ויואו שהוא עובד עם המתאם הזה
            this.pa.NotifyDataSetChanged(); //הפעלת המתאם

            this.gridview_products.ItemClick += Gridview_products_ItemClick; ;
            this.btnMovetoOrderdetails.Click += BtnMovetoOrderdetails_Click; ;
            this.btn_toolbar_backPage.Click += Btn_toolbar_backPage_Click;
        }

        private void Btn_toolbar_backPage_Click(object sender, EventArgs e)
        {
            this.Finish(); 
        }

        private void BtnMovetoOrderdetails_Click(object sender, EventArgs e)
        {
            dialog_AddNewOrder.Show(); 
        }

        private  async void Gridview_products_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;//מיקום המוצר בגריד ויאו
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

            dialogAddProduct.Show(); //מפעיל את הדיאלוג
        }


        public void CreateAddProductToCartDialog()
        {
            dialogAddProduct = new Dialog(this);
            Button btnPlusProduct, btnMinusProduct, btnSaveProductAmount, btn_close_dialog;

            dialogAddProduct.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            dialogAddProduct.SetContentView(Resource.Layout.layoutAddProductDialog);
            dialogAddProduct.SetTitle("הוספת מוצר");
            dialogAddProduct.SetCancelable(true);

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
                dialogAddProduct.Dismiss(); // סוגר את הדיאלוג 

            };

            btnSaveProductAmount.Click += BtnSaveProductAmount_Click; ;
        }

        private void BtnSaveProductAmount_Click(object sender, EventArgs e)//move to the addproduct dialog to set the orders details
        {
            dialog_AddNewOrder.Show();
        }


        public async void CreateConrifeOrderDialog()
        {
            //dialog add new order  conrife
            this.dialog_AddNewOrder = new Dialog(this);
            this.dialog_AddNewOrder.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);

            this.dialog_AddNewOrder.SetContentView(Resource.Layout.layout_ManagerOrdersDailogAddNewOrder);
            this.lv_AddOrderDialogCartDialog = this.dialog_AddNewOrder.FindViewById<ListView>(Resource.Id.listview_ManagerOrdersDailogAddNewOrderCart);
            this.et_AddOrderDialogUserNameOfCustomer = this.dialog_AddNewOrder.FindViewById<EditText>(Resource.Id.et_ManagerOrdersDailogAddNewOrderCustomerName);
            this.et_AddOrderDialogOrderTotalPrice = this.dialog_AddNewOrder.FindViewById<EditText>(Resource.Id.et_ManagerOrdersDailogAddNewOrderOrderTotalPrice);
            this.btn_AddOrderDialogCloseCartDialog = this.dialog_AddNewOrder.FindViewById<Button>(Resource.Id.btn_ManagerOrdersDailogAddNewOrderClose);

            this.btn_AddOrderDialogCartDialogOrderSaveOrder = this.dialog_AddNewOrder.FindViewById<Button>(Resource.Id.btn_ManagerOrdersDailogAddNewOrderSaveOrder);

            this.btn_AddOrderDialogCloseCartDialog.Click += Btn_AddOrderDialogCloseCartDialog_Click; ;
            this.btn_AddOrderDialogCartDialogOrderSaveOrder.Click += Btn_AddOrderDialogCartDialogOrderSaveOrder_Click; ;



            Total_Price = await SelectedProduct.Calculate_TotalOrderPrice(userName);//מחשב את המחיר הסופי של הקנייה של אותו משתמש 
            this.tv_Total_Price.Text = "מחיר סופי: " + Total_Price.ToString() + "‏₪";

            CreateConrifeOrderDialog();

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
                    this.gridview_products.Adapter = this.adapter_selected_products;//אומר לליסט ויואו שהוא עובד עם המתאם הזה
                    this.adapter_selected_products.NotifyDataSetChanged(); //הפעלת המתאם
                    pd.Cancel();
                }

            }
        }

        private void Btn_AddOrderDialogCartDialogOrderSaveOrder_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Btn_AddOrderDialogCloseCartDialog_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}