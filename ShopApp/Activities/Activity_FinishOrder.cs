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
        string userName;
        double Total_Price;
        bool Is_Payed;
        ISharedPreferences sp;
        ListView lv_Selected_Products;
        TextView tvcurrentAmountProduct; //הכמות הנוכחית של מוצר בקנייה
        Button btn_conrife_order;
       
        SelectedProduct cartSelectedProduct;
        Adapter_FinishOrder_SelectedProducts adapter_selected_products;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_FinishOrder);
         
            this.lv_Selected_Products = FindViewById<ListView>(Resource.Id.listViewFinishOrder);
            this.btn_conrife_order = FindViewById<Button>(Resource.Id.btnFinishOrderConrifeOrder);
            this.sp = GetSharedPreferences("details", FileCreationMode.Private);
            this.userName = this.sp.GetString("Username", "");

        

            List<SelectedProduct> list_selectedProducts = new List<SelectedProduct>();
            list_selectedProducts = await SelectedProduct.GetAllProductInCart(userName);//מביא  רשימה של כל המוצרים שיש לאותו משתמש בעגלה 

            List<Product> list_products = new List<Product>();//רשימה של  כל המוצרים שקיימים בחנות
            list_products = await Product.GetAllProduct();

            this.adapter_selected_products = new Adapter_FinishOrder_SelectedProducts(this, list_selectedProducts, list_products);//מקבל אקטיביטי ואת רשימת המוצרים בחנות ואת רשימת המוצרים שיש למשתמש הנוכחי בעגלה
            this.lv_Selected_Products.Adapter = this.adapter_selected_products;//אומר לליסט ויואו שהוא עובד עם המתאם הזה
            this.adapter_selected_products.NotifyDataSetChanged(); //הפעלת המתאם


            





        }
    }
}