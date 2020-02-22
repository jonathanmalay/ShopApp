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
    public class ClientOrder_Activity : Activity
    {
        ISharedPreferences sp;
        string userName;

        Dialog dialogAddProduct;
        TextView tvcurrentAmountProduct; //הכמות הנוכחית של מוצר בקנייה
        Button btnMoveToPayment;
        ListView lvProducts;
        List<SelectedProduct> selectedProducts;
        SelectedProduct cartSelectedProduct;
        ProductAdapter pa;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ClientOrder_Layout);

            this.lvProducts = FindViewById<ListView>(Resource.Id.listViewProducts);
            this.btnMoveToPayment = FindViewById<Button>(Resource.Id.btnClientOrderLayoutMoveToPayment);
            this.sp = GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");

            CreateDialog(this);

            selectedProducts = new List<SelectedProduct>();
            selectedProducts = await SelectedProduct.GetAllProductInCart(userName);//מביא  רשימה של כל המוצרים שיש לאותו משתמש בעגלה 

            List<Product> products = new List<Product>();//רשימה של  כל המוצרים שקיימים בחנות
            products = await Product.GetAllProduct();
            
            this.pa = new ProductAdapter(this, products,selectedProducts);//מקבל אקטיביטי ואת רשימת המוצרים בחנות ואת רשימת המוצרים שיש למשתמש הנוכחי בעגלה
            this.lvProducts.Adapter = this.pa;//אומר לליסט ויואו שהוא עובד עם המתאם הזה
            this.pa.NotifyDataSetChanged(); //הפעלת המתאם
            this.lvProducts.ItemClick += LvProducts_ItemClick;
            this.btnMoveToPayment.Click += BtnMoveToPayment_Click;
        }

        private void BtnMoveToPayment_Click(object sender, EventArgs e)
        {
            if (selectedProducts == null)//אם עגלת הקניות של הקונה ריקה הוא לא יוכל לעבור לאקטיביטי ביצוע תשלום 
            {
                Toast.MakeText(this, "!!עליך להוסיף לפחות פריט אחד על מנת לעבור לתשלום", ToastLength.Long).Show();

            }
            else
            {
                Intent intent = new Intent(this, typeof(Activity_FinishOrder));//עובר לאקטיביטי תשלום וסיום הזמנה 
                this.StartActivity(intent);
            }
        }

        public  void CreateDialog(Activity activity)
        {
            dialogAddProduct = new Dialog(this);

            Button btnPlusProduct, btnMinusProduct, btnSaveProductAmount;

            dialogAddProduct.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            dialogAddProduct.SetContentView(Resource.Layout.layoutAddProductDialog);
            dialogAddProduct.SetTitle("הוספת מוצר");
            dialogAddProduct.SetCancelable(true);

            tvcurrentAmountProduct = dialogAddProduct.FindViewById<TextView>(Resource.Id.tvDialogAddProductCurrentAmount);
            btnMinusProduct = dialogAddProduct.FindViewById<Button>(Resource.Id.btnDialogAddProductMinus); //כפתור ההורדה של הכמות
            btnPlusProduct = dialogAddProduct.FindViewById<Button>(Resource.Id.btnDialogAddProductPlus); //כפתור ההוספה של הכמות 
            btnSaveProductAmount = dialogAddProduct.FindViewById<Button>(Resource.Id.btnDialogAddProductSave); //כפתור השמירה

            btnPlusProduct.Click += (senderD, eD) =>
            {
                cartSelectedProduct.Amount++; //מוסיף אחד לכמות
                tvcurrentAmountProduct.Text = cartSelectedProduct.Amount.ToString();
            };
            btnMinusProduct.Click += (senderD, eD) =>
            {
                if (cartSelectedProduct.Amount > 0)//מחסר מהכמות רק אם היא גדולה מ0 על מנת למוע כמות שלילית של מוצר.
                {
                    cartSelectedProduct.Amount--; //מוריד אחד לכמות
                    tvcurrentAmountProduct.Text = cartSelectedProduct.Amount.ToString();

                }
                else
                {
                    Toast.MakeText(this, "אנא הכנס כמות גדולה מ-0", ToastLength.Long).Show();

                }
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
            SelectedProduct.AddSelectedProduct(this.userName, cartSelectedProduct); //מוסיף את המוצר לעגלת הקניות כלומר לקולקשיין  עגלה בפיירבייס שבו יש מסמך עם השם של המשתמש שמחובר  לאפליקציה ובתוך המסמך יש את המוצרים שהזמין 
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

            if(!exist)
            {
                pa.CartProductsList.Add(cartSelectedProduct);
            }

            dialogAddProduct.Dismiss();
            pa.NotifyDataSetChanged();
        }
    }
}