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
using Android.Graphics;
using Plugin.CloudFirestore;
using System.Threading.Tasks;

namespace ShopApp
{
     public class Product
    {  
        public int ProductId { get; set; } //
        public string Name { get; set;} //שם המוצר
        public int Price { get; set; } //מחיר המוצר 

        public Bitmap Image { get; set; } //תמונה של המוצר

        public double  Quantity { get; set; }  //כמות המוצר
        
        
        public Product()
        {

        }

        public Product(int productId, string name, int price, Bitmap image, double quantity)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Image = image;
            Quantity = quantity;
        }

        public static async void AddProduct(Activity activity, int product_id,string product_name,int product_price,Bitmap product_image,double product_Quantity)
        {//פעולה אשר מוסיפה מוצר 
            Product p = new Product();
            p.ProductId = product_id;
            p.Name = product_name;
            p.Price = product_price;
            p.Image = product_image;
            p.Quantity = product_Quantity;

            await AppData.productCollection.GetDocument(product_name).SetDataAsync(p);//מוסיף לפיירבייס מסמך בפרודוקט קולקשיין עם הערכים של המוצר

        }


        public static async Task<Product> GetProduct (string name)
        {//הפעולה מחזירה עצם מסוג מוצר
            try
            {

                IDocumentSnapshot reference = await AppData.FireStore.GetCollection("Product").GetDocument(name).GetDocumentAsync();

                return reference.ToObject<Product>();
            }

            catch (Exception)
            {

                return null;
            }

        }



        public static async Task<List<Product>> GetAllProduct()
        {
            IQuerySnapshot snapshot = await AppData.productCollection.GetDocumentsAsync();
            List<Product> products = snapshot.ToObjects<Product>().ToList();
            return products;

        }
        


    }
}