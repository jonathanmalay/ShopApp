using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CloudFirestore;

namespace ShopApp
{
    class SelectedProduct
    {
        public string ProductName { get; set; }
        public int Amount { get; set; }


        public SelectedProduct() { }// הפעולה הריקה בשביל הפייר בייס על מנת שלא תיווצר שגיאה 
        public SelectedProduct(string productName)
        {
            this.ProductName = productName;
            this.Amount = 0;
        }



        public static async void AddSelectedProduct(string username, SelectedProduct sp)
        {
            try
            {
                await AppData.cartCollection.GetDocument(username).GetCollection("SelectedProduct").GetDocument(sp.ProductName).SetDataAsync(sp);//מוסיף מוצר  לקולקשיין עגלה של אותו אדם בפיירבייס
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public static async Task<SelectedProduct> GetProductInCart(string product_name, string username)//הפעולה מחזירה את המוצר שיש למשתמש בעגלה 
        {
            try
            {
                IDocumentSnapshot reference = await AppData.cartCollection.GetDocument(username).GetCollection("SelectedProduct").GetDocument(product_name).GetDocumentAsync();

                return reference.ToObject<SelectedProduct>();//מחזירה עצם מסוג המוצר שנבחר
            }

            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<bool> CheckIfProductInCart(string product_name, string username)
        {//פעולה אשר בודקת האם המוצר  קיים בעגלה כלומר במערכת של הפיירבייס
            SelectedProduct returned_product = await GetProductInCart(product_name, username);

            if (returned_product == null) // אם המוצר עדיין לא נבחר כלומר לא קיים 
            {
                return false;
            }
            return true;   // אמת אם קיים במערכת
        }



        public static async Task<List<SelectedProduct>> GetAllProductInCart(string username)
        {
            List<SelectedProduct> productsInCart = new List<SelectedProduct>();
            try
            {
                IQuerySnapshot snapshot = await AppData.cartCollection.GetDocument(username).GetCollection("SelectedProduct").GetDocumentsAsync();//לוקח את כל המוצרים מהעגלה של אותו בן אדם ומכניס אותם לרשימה מסוג מוצר נבחר
                productsInCart = snapshot.ToObjects<SelectedProduct>().ToList();
            }
            catch(Exception e)
            {
            }
            return productsInCart;
        }




        public static async Task<int> Calculate_TotalOrderPrice(string username)
        {
            int total_price = 0;
            List<SelectedProduct> productsInCart = new List<SelectedProduct>();
            try
            {
                IQuerySnapshot snapshot = await AppData.cartCollection.GetDocument(username).GetCollection("SelectedProduct").GetDocumentsAsync();//לוקח את כל המוצרים מהעגלה של אותו בן אדם ומכניס אותם לרשימה מסוג מוצר נבחר
                productsInCart = snapshot.ToObjects<SelectedProduct>().ToList();

                for(int i=0; i<productsInCart.Count;i++)
                {
                    Product p = await Product.GetProduct(productsInCart[i].ProductName);//מחזיר עצם  מסוג מוצר של המוצר שנבחר
                    int current_product_price = p.Price;//לוקח ממנו את המחיר
                    int checkout_product_price = productsInCart[i].Amount * current_product_price; //מוסיף לסכום את מחיר המוצר הנוכחי כפול הכמות שבחר המשתמש
                    total_price += checkout_product_price;
                }
            }
            catch (Exception e)
            {
            }
            return total_price;
        }

    }
}