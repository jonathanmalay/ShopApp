using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ShopApp.Activities;

namespace ShopApp
{
    [Activity(Label = "ClientOrder_Activity")]
    public class ClientOrder_Activity : Android.Support.V4.App.Fragment
    {
        ISharedPreferences sp;
        string userName;
        Button btn_back_page;
        Dialog dialogAddProduct;
        TextView tvcurrentAmountProduct,tv_toolbar_title; //הכמות הנוכחית של מוצר בקנייה
        Button btnMoveToPayment;
        ListView lvProducts;
        List<SelectedProduct> selectedProducts;
        SelectedProduct cartSelectedProduct;
        ProductAdapter pa;

          
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.ClientOrder_Layout, container, false);
        }
        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            this.tv_toolbar_title = view.FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.tv_toolbar_title.Text = "הזמנה";

            this.lvProducts = view.FindViewById<ListView>(Resource.Id.listViewProducts);
            this.btnMoveToPayment = view.FindViewById<Button>(Resource.Id.btnClientOrderLayoutMoveToPayment);
            this.btn_back_page = view.FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
            this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");

            CreateDialog();

            selectedProducts = new List<SelectedProduct>();
            selectedProducts = await SelectedProduct.GetAllProductInCart(userName);//מביא  רשימה של כל המוצרים שיש לאותו משתמש בעגלה 

            List<Product> products = new List<Product>();//רשימה של  כל המוצרים שקיימים בחנות
            products = await Product.GetAllProduct();

            this.pa = new ProductAdapter(Activity, products, selectedProducts);//מקבל אקטיביטי ואת רשימת המוצרים בחנות ואת רשימת המוצרים שיש למשתמש הנוכחי בעגלה
            this.lvProducts.Adapter = this.pa;//אומר לליסט ויואו שהוא עובד עם המתאם הזה
            this.pa.NotifyDataSetChanged(); //הפעלת המתאם
            this.lvProducts.ItemClick += LvProducts_ItemClick;
            this.btnMoveToPayment.Click += BtnMoveToPayment_ClickAsync;
            this.btn_back_page.Click += Btn_back_page_Click;
        }

        private void Btn_back_page_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(HomeActivity)); 
            this.StartActivity(intent);
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

        private async void BtnMoveToPayment_ClickAsync(object sender, EventArgs e)
        {
            bool Is_Okay = await Conrife_Order_Minimum_Price();
            if (Is_Okay)//אם סכום ההזמנה קטן מחמישים שקלים  לא יוכל לעבור לאקטיביטי ביצוע תשלום 
            {
                Intent intent = new Intent(Activity, typeof(Activity_FinishOrder));//עובר לאקטיביטי תשלום וסיום הזמנה 
                this.StartActivity(intent);
            }
            else
            {
                Toast.MakeText(Activity, " סכום ההזמנה המינימלי הינו 50 שקלים,על מנת לבצע תשלום אנא הוסף פריטים על מנת להגיע לסכום זה!!", ToastLength.Long).Show();

            }
        }

        public  void CreateDialog()
        {
            dialogAddProduct = new Dialog(Activity);

            Button btnPlusProduct, btnMinusProduct, btnSaveProductAmount , btn_close_dialog;

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
                    
                    Toast.MakeText(Activity, "אנא הכנס כמות גדולה מ-0", ToastLength.Long).Show();
                    
                }
            };

            btn_close_dialog.Click += (senderD, eD) =>
            {
                dialogAddProduct.Dismiss(); // סוגר את הדיאלוג 

            };

            btnSaveProductAmount.Click += BtnSaveProductAmount_Click;
        }




        public async void LvProducts_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
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

            dialogAddProduct.Show(); //מפעיל את הדיאלוג

        }

        private void BtnSaveProductAmount_Click(object sender, EventArgs e)
        {
            // SelectedProduct sp = new SelectedProduct(selectedProduct.Name, AmountProduct);//יוצר עצם מסוג מוצר נבחר ומכניס לפעולה הבונה שלו את הערכים שהתקבלו על ידי המשתמש בדיאלוג כלומר הכמות  של אותו מוצר
            SelectedProduct.AddSelectedProduct(Activity, this.userName, cartSelectedProduct); //מוסיף את המוצר לעגלת הקניות כלומר לקולקשיין  עגלה בפיירבייס שבו יש מסמך עם השם של המשתמש שמחובר  לאפליקציה ובתוך המסמך יש את המוצרים שהזמין 
            Toast.MakeText(Activity, "הפריט נוסף לעגלת הקניות (:", ToastLength.Long).Show();

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

            if(!exist && cartSelectedProduct.Amount>0)
            {
                pa.CartProductsList.Add(cartSelectedProduct);
            }

            else if(cartSelectedProduct.Amount==0)
            {
             SelectedProduct.Remove_Product_From_Cart(userName, cartSelectedProduct.ProductName);//מסירה את המוצר שכמותו 0 מהעגלה כי במידה ולא אעשה זאת הוא יוצג בסוף ההזמנה
            }

            dialogAddProduct.Dismiss();
            pa.NotifyDataSetChanged();
        }
    }
}