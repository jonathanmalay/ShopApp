using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;

namespace ShopApp
{
    class  ProductAdapter : BaseAdapter<Product> //הורשה 
    {
        Activity activity;
        ISharedPreferences sp;
        string userName;

        public List<Product> AllProducts { get; set; }
        public List<SelectedProduct> CartProductsList { get; set; }

        public ProductAdapter(Activity activity, List<Product> allProducts,List<SelectedProduct>cartProductsList)//מקבלת אקטיביטי ומקבלת  רשימה של מוצרים
        {
            this.activity = activity;
            this.AllProducts = allProducts;
            this.CartProductsList = cartProductsList;
             
            this.sp = activity.GetSharedPreferences("details", FileCreationMode.Private);
            userName = this.sp.GetString("Username", "");

        }




        public List<Product> GetList()
        {
            return this.AllProducts;

        }

        public override long GetItemId(int position)  //  שהוא אותו מיקום שהכניס לפעולה ID מקבלת מיקום ומחזירה 
        {

            return position;
        }

        public override int Count    // מחזיר את כמות האיברים שיש 
        {
            get { return this.AllProducts.Count; }
        }

     

        public override Product this[int position]
        {
            get
            {
                return this.AllProducts[position]; //מחזיר את האיבר במקום מסוים

            }

        }


        public override View GetView(int position, View convertView, ViewGroup parent)//הפעולה מקבלת שלוש דברים -מיקום שאליו יכנס הויאו בלולאת הפור ,
        {

          



            if (convertView == null)

            {
                convertView = this.activity.LayoutInflater.Inflate(Resource.Layout.costum_Layout, parent, false);/*מגדיר לו איזה לייאוט להפוך לוויאו*/
            }



            TextView tvTitle = convertView.FindViewById<TextView>(Resource.Id.tvProductRawTitle);
            TextView tvSubTitle = convertView.FindViewById<TextView>(Resource.Id.tvSubTitle);
            TextView tvPrice = convertView.FindViewById<TextView>(Resource.Id.tvProductRawPrice);
            ImageView ivProduct = convertView.FindViewById<ImageView>(Resource.Id.ivProductRaw);

            Product tempProduct = AllProducts[position];
            tvSubTitle.Text = "0";

            for (int i=0 ;i<CartProductsList.Count;i++)
            {
               SelectedProduct currrentSelectedProduct = CartProductsList[i];
                if (currrentSelectedProduct.ProductName == tempProduct.Name)
                {
                    tvSubTitle.Text = currrentSelectedProduct.Amount.ToString();//מציג את   הכמות של אותו מוצר שהמשתמש הוסיף כבר
                    break;
                }


            }

            
            tvPrice.Text = "מחיר לקילו " + tempProduct.Price;
            tvTitle.Text = tempProduct.Name;
           
            return convertView;
        }

    }
}

