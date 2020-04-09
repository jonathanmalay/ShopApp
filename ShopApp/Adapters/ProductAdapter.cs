using System.Collections.Generic;
using Android.Content;
using Android.App;
using Android.Views;
using Android.Widget;
using Square.Picasso;

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

            TextView tv_ProductName = convertView.FindViewById<TextView>(Resource.Id.tv_CellClientOrdeProductName);
            TextView tv_amount = convertView.FindViewById<TextView>(Resource.Id.tv_CellClientOrdeProductAmount);
            TextView tvPrice = convertView.FindViewById<TextView>(Resource.Id.tvProductRawPrice);
            ImageView ivProduct = convertView.FindViewById<ImageView>(Resource.Id.ivProductRaw);

            Product tempProduct = AllProducts[position];
            tv_amount.Text = "כמות:" + "0";

            for (int i=0 ;i<CartProductsList.Count;i++)
            {
               SelectedProduct currentSelectedProduct = CartProductsList[i];
                if (currentSelectedProduct.ProductName == tempProduct.Name)
                {
                    tv_amount.Text = " כמות: " + currentSelectedProduct.Amount.ToString() + " קילו ";//מציג את   הכמות של אותו מוצר שהמשתמש הוסיף כבר
                    break;
                }
            }
            
            tvPrice.Text = " מחיר לקילו: " + tempProduct.Price.ToString() + "‏₪";
            tv_ProductName.Text = tempProduct.Name;

            Picasso.With(this.activity).Load(tempProduct.ImageUrl).Into(ivProduct); //insert the pphoto to cell (from firbase Storage)
           
            return convertView;
        }

    }
}

