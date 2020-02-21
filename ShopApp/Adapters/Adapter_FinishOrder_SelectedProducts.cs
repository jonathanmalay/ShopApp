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
    class Adapter_FinishOrder_SelectedProducts : BaseAdapter<SelectedProduct>
    {
        ISharedPreferences sp;
        string userName;

        public List<SelectedProduct> list_selected_products { get; set; }
        public List<Product> list_AllProducts;
        Activity activity;

        public Adapter_FinishOrder_SelectedProducts(Activity activity, List<SelectedProduct> list_selectedProducts, List<Product> list_all_Products)
        {
            activity = activity;
            this.list_selected_products = list_selectedProducts;
            this.list_AllProducts = list_all_Products;
            this.sp = activity.GetSharedPreferences("details", FileCreationMode.Private);
            userName = this.sp.GetString("Username", "");

        }


        public List<SelectedProduct> GetList()
        {
            return this.list_selected_products;

        }

        public override int Count    // מחזיר את כמות האיברים שיש 
        {
            get { return this.list_selected_products.Count; }
        }


        public override long GetItemId(int position)
        {
            return position;
        }

        public override SelectedProduct this[int position]
        {
            get
            {
                return this.list_selected_products[position]; //מחזיר את האיבר במקום מסוים

            }

        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
         
            
            if (convertView == null)
            {
                convertView = this.activity.LayoutInflater.Inflate(Resource.Layout.layout_selectedProductInCart, parent, false);/*מגדיר לו איזה לייאוט להפוך לוויאו*/
            }
            TextView tvProductName = convertView.FindViewById<TextView>(Resource.Id.tvSelectedProductInCartName);
            TextView tvAmount = convertView.FindViewById<TextView>(Resource.Id.tvSelectedProductInCartAmount);
            TextView tvProduct_Price = convertView.FindViewById<TextView>(Resource.Id.tvSelectedProductInCartPrice);

            SelectedProduct temp_SelectedProduct = list_selected_products[position];
            Product currentProduct = GetProduct(temp_SelectedProduct.ProductName);

            tvProductName.Text = temp_SelectedProduct.ProductName;
            tvAmount.Text = temp_SelectedProduct.Amount + "כמות: ";

            if (currentProduct != null)
            {
                tvProduct_Price.Text = "מחיר כולל: " + temp_SelectedProduct.Amount * currentProduct.Price;//מציג את   המחיר של אותו מוצר 
            }

            return convertView;
        }

        private Product GetProduct(string productName)
        {
            for (int i = 0; i < list_AllProducts.Count; i++)
            {
                Product currentProduct = list_AllProducts[i];
                if (currentProduct.Name == productName)
                {
                    return currentProduct;
                }
            }

            return null;
        }

    }
}

   