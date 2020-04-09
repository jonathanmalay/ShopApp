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
using System.IO;

namespace ShopApp
{
    public class Product
    {
        public int ProductId { get; set; } //
        public string Name { get; set; } //שם המוצר
        public int Price { get; set; } //מחיר המוצר 

        public string ImageUrl { get; set; } //תמונה של המוצר

        public double Quantity { get; set; }  //כמות המוצר




        public Product()
        {

        }

        public Product(int productId, string name, int price, string imageUrl, double quantity)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            ImageUrl = imageUrl;
            Quantity = quantity;
        }

    //    public static async Task<string> Upload_Image()

       public static async void AddProduct(Activity activity, int product_id, string product_name, int product_price, Android.Net.Uri product_image, double product_Quantity)//פעולה אשר מוסיפה מוצר 
        {

            try
            {
                //העלאת התמונה וקבלת הקישור של התמונה
                string imageUrl = "";

                using (var stream = activity.ContentResolver.OpenInputStream(product_image))
                {
                   var task= AppData.FirebaseStorage.Child("Products").Child("image.jpg").PutAsync(stream);//upload the image from the phone to the storage in the server 
                    task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %"); //for help me to debug 

                    // await the task to wait until upload completes and get the download url
                    imageUrl = await task;
                }

                if (imageUrl == "")
                {
                    Toast.MakeText(activity, "קיימת בעיה בתמונה, אנא נסה שוב", ToastLength.Long).Show();
                    imageUrl = "";
                }


                //העלאת המוצר לאחר שיש לנו את הקישור של התמונה
                Product p = new Product();
                p.ProductId = product_id;
                p.Name = product_name;
                p.Price = product_price;
                p.ImageUrl = imageUrl; // the link of the image in the storage 
                p.Quantity = product_Quantity;

                await AppData.productCollection.GetDocument(product_name).SetDataAsync(p);//מוסיף לפיירבייס מסמך בפרודוקט קולקשיין עם הערכים של המוצר


                Toast.MakeText(activity, "המוצר הועלה בהצלחה!", ToastLength.Long).Show();

            }
            catch(Exception e)
            {  
                Toast.MakeText(activity, "חלה שגיאה, אנא נסה שוב", ToastLength.Long).Show();
            }

        }


                public static async Task RemoveProduct( Product product)//הפעולה מוחקת את המוצר מהפייר בייס
        {
            try
            {
                await AppData.ProductsStorage.Child(product.Name).DeleteAsync(); // remove the product image from the firebase storage 
                await AppData.FireStore.GetCollection("Product").GetDocument(product.Name).DeleteDocumentAsync();//ההסרה של המוצר מהמסד נתונים
            }

            catch(Exception)
            {
               
            }
        }

        public static async Task<Product> GetProduct(string name)//הפעולה מחזירה עצם מסוג מוצר
        {
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