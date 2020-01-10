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
    [Activity(Label = "ClientOrder_Activity")]
    public class ClientOrder_Activity : Activity
    {
        ISharedPreferences sp;
        string userName;

        ListView lvProducts;

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

        private void LvProducts_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int position = e.Position;
            Product selectedProduct = this.pa[position];

            EditText etAmount = new EditText(this);

            AlertDialog dialog = new AlertDialog.Builder(this).SetTitle("בחר כמות").SetView(etAmount).SetPositiveButton("הוסף לעגלה ", (senderD, eD) =>
            {
                if (etAmount.Text == "")
                {
                    Toast.MakeText(this, "אנא כתוב כמות", ToastLength.Long).Show();
                    return;
                }

          


                    if (!etAmount.Text.Any(char.IsDigit))//בודק האם בשם יש רק מספרים
                    {
                        etAmount.SetError("יש להזין רק ספרות", null);
                         etAmount.RequestFocus();

                        return ;
                    }


                SelectedProduct sp = new SelectedProduct(selectedProduct.Name, int.Parse(etAmount.Text));//יוצר עצם מסוג מוצר נבחר ומכניס לפעולה הבונה שלו את הערכים שהתקבלו על ידי המשתמש בדיאלוג כלומר הכמות  של אותו מוצר

                SelectedProduct.AddSelectedProduct(this.userName, sp); //מוסיף את המוצר לעגלת הקניות כלומר לקולקשיין  עגלה בפיירבייס שבו יש מסמך עם השם של המשתמש שמחובר  לאפליקציה ובתוך המסמך יש את המוצרים שהזמין 

                Toast.MakeText(this, "הפריט נוסף לעגלת הקניות (:", ToastLength.Long).Show();
            })
                .SetNegativeButton("ביטול", listener: null).Create();


            dialog.Show();


        }

     
    }
}