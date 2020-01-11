﻿using System;
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

namespace ShopApp
{
    [Activity(Label = "ClientOrder_Activity")]
    public class ClientOrder_Activity : Activity
    {
        ISharedPreferences sp;
        string userName;

        ListView lvProducts;
        SelectedProduct cartSelectedProduct;

        ProductAdapter pa;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ClientOrder_Layout);

            this.lvProducts = FindViewById<ListView>(Resource.Id.listViewProducts);

            List<Product> products = new List<Product>();
            products = await Product.GetAllProduct();
            this.pa = new ProductAdapter(this, products);//מקבל אקטיביטי ואת רשימת המוצרים

            this.lvProducts.Adapter = this.pa;//אומר לליסט ויואו שהוא עובד עם המתאם הזה


            this.pa.NotifyDataSetChanged(); //הפעלת המתאם

            this.sp = GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");
            this.lvProducts.ItemClick += LvProducts_ItemClick;
        }

        public async void LvProducts_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Button btnPlusProduct, btnMinusProduct, btnSaveProductAmount;
            TextView tvcurrentAmountProduct; //הכמות הנוכחית של מוצר בקנייה
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

            Dialog dialogAddProduct = new Dialog(this);
            dialogAddProduct.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            dialogAddProduct.SetContentView(Resource.Layout.layoutAddProductDialog);
            dialogAddProduct.SetTitle("הוספת מוצר");
            dialogAddProduct.SetCancelable(true);

            tvcurrentAmountProduct = dialogAddProduct.FindViewById<TextView>(Resource.Id.tvDialogAddProductCurrentAmount);//טקסט עם הכמות הנוכחית מאותו מוצר 
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
                cartSelectedProduct.Amount--; //מוריד אחד לכמות
                tvcurrentAmountProduct.Text = cartSelectedProduct.Amount.ToString();
            };


            btnSaveProductAmount.Click += BtnSaveProductAmount_Click;
            tvcurrentAmountProduct.Text = cartSelectedProduct.Amount.ToString();//הטקסט שיוצג יהיה הכמות הנוכחית של אותו מוצר שמשתמש שם בעגלה שלו

            dialogAddProduct.Show(); //מפעיל את הדיאלוג

        }

        private void BtnSaveProductAmount_Click(object sender, EventArgs e)
        {
            // SelectedProduct sp = new SelectedProduct(selectedProduct.Name, AmountProduct);//יוצר עצם מסוג מוצר נבחר ומכניס לפעולה הבונה שלו את הערכים שהתקבלו על ידי המשתמש בדיאלוג כלומר הכמות  של אותו מוצר
            SelectedProduct.AddSelectedProduct(this.userName, cartSelectedProduct); //מוסיף את המוצר לעגלת הקניות כלומר לקולקשיין  עגלה בפיירבייס שבו יש מסמך עם השם של המשתמש שמחובר  לאפליקציה ובתוך המסמך יש את המוצרים שהזמין 
            Toast.MakeText(this, "הפריט נוסף לעגלת הקניות (:", ToastLength.Long).Show();

        }
    }
}