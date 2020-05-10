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
        int Activity_id;  

        public List<Product> AllProducts { get; set; }
        public List<SelectedProduct> CartProductsList { get; set; }


        public ProductAdapter(Activity activity, List<Product> allProducts,List<SelectedProduct>cartProductsList , int activity_id)//מקבלת אקטיביטי ומקבלת  רשימה של מוצרים ומספר זיהוי של אקטיביטי כדי שידע איזה ויאו להציג מכיוון שמשתמשים במתאם זה בכמה מסכים שלכל אחד מהם צרכים שונים
        {
            this.activity = activity;
            this.AllProducts = allProducts;
            this.CartProductsList = cartProductsList;
            this.Activity_id = activity_id;  
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


        public override View GetView(int position, View convertView, ViewGroup parent)//יוצרת את התצוגה בכל תא , ובהתאם למזהה המסך שקיבלנו בפעולה הבונה מציגה את הויאו המתאים לאותה אקטיביטי
        {
            if (this.Activity_id == 0)
            {
                if (convertView == null)
                {
                    convertView = this.activity.LayoutInflater.Inflate(Resource.Layout.Cell_Product, parent, false);/*מגדיר לו איזה לייאוט להפוך לוויאו*/
                }

                TextView tv_ProductQuantity = convertView.FindViewById<TextView>(Resource.Id.tv_CellClientOrdeProductQuantity);
                TextView tv_ProductName = convertView.FindViewById<TextView>(Resource.Id.tv_CellClientOrdeProductName);
                TextView tv_amount = convertView.FindViewById<TextView>(Resource.Id.tv_CellClientOrdeProductAmount);
                TextView tvPrice = convertView.FindViewById<TextView>(Resource.Id.tvProductRawPrice);
                ImageView ivProduct = convertView.FindViewById<ImageView>(Resource.Id.ivProductRaw);

                Product tempProduct = AllProducts[position];
                tv_amount.Text = "  כמות בעגלה: " + "0";
                tv_ProductQuantity.Text = " כמות במארז: " + AllProducts[position].Quantity + " קילו ";  
                for (int i = 0; i < CartProductsList.Count; i++)
                {
                    SelectedProduct currentSelectedProduct = CartProductsList[i];
                    if (currentSelectedProduct.ProductName == tempProduct.Name)
                    {
                        tv_amount.Text = "  כמות בעגלה: " + currentSelectedProduct.Amount.ToString();//מציג את   הכמות של אותו מוצר שהמשתמש הוסיף כבר
                        break;
                    }
                }

                tvPrice.Text = " מחיר למארז: " + tempProduct.Price.ToString() + "‏₪";
                tv_ProductName.Text = tempProduct.Name;

                Picasso.With(this.activity).Load(tempProduct.ImageUrl).Into(ivProduct); //insert the pphoto to cell (from firbase Storage)

                return convertView;
            }

           if(Activity_id == 1 ) // if the view is mananager edit product 
            {

                if (convertView == null)
                {
                    convertView = this.activity.LayoutInflater.Inflate(Resource.Layout.Cell_Product, parent, false);/*מגדיר לו איזה לייאוט להפוך לוויאו*/
                }

                TextView tv_ProductName = convertView.FindViewById<TextView>(Resource.Id.tv_CellClientOrdeProductName);
                TextView tv_amount = convertView.FindViewById<TextView>(Resource.Id.tv_CellClientOrdeProductAmount);
                TextView tvPrice = convertView.FindViewById<TextView>(Resource.Id.tvProductRawPrice);
                TextView tv_ProductQuantity = convertView.FindViewById<TextView>(Resource.Id.tv_CellClientOrdeProductQuantity);  
                ImageView ivProduct = convertView.FindViewById<ImageView>(Resource.Id.ivProductRaw);

                tv_ProductQuantity.Visibility = ViewStates.Invisible; 
                Product tempProduct = AllProducts[position];
                tv_amount.Text = "כמות במארז: " + tempProduct.Quantity + " קילו ";

                 
                tvPrice.Text = " מחיר למארז: " + tempProduct.Price.ToString() + "‏₪";
                tv_ProductName.Text = tempProduct.Name;

                Picasso.With(this.activity).Load(tempProduct.ImageUrl).Into(ivProduct); //insert the pphoto to cell (from firbase Storage)

                return convertView;


               

            }
            else
            {
                return convertView; 
            }
        }

    }
}

