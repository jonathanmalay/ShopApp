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
    class SelectedProduct
    {
        public SelectedProduct() { }// הפעולה הריקה בשביל הפייר בייס על מנת שלא תיווצר שגיאה 
        public SelectedProduct(string productName, int amount)
        {
            ProductName = productName;
            Amount = amount;
        }

        public string ProductName { get; set; }
        public int Amount { get; set; }

        public static async void AddSelectedProduct(string username, SelectedProduct sp)
        {
            try
            {
                await AppData.cartCollection.GetDocument(username).GetCollection("SelectedProduct").GetDocument(sp.ProductName).SetDataAsync(sp);//מוסיף מוצר  לקולקשיין עגלה של אותו אדם בפיירבייס
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}