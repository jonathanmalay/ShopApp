﻿using System;
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
        public SelectedProduct() { }// הפעולה הריקה בשביל הפייר בייס על מנת שלא תיווצר שגיאה 
        public SelectedProduct(string productName)
        {
            ProductName = productName;
            Amount = 0;
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


        public static async Task<SelectedProduct> GetProductInCart(string   product_name ,string username)//הפעולה מחזירה את המוצר שיש למשתמש בעגלה 
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

        public static async Task<bool> CheckIfProductInCart( string product_name,string username)
        {//פעולה אשר בודקת האם המוצר  קיים בעגלה כלומר במערכת של הפיירבייס
            SelectedProduct returned_product = await GetProductInCart(product_name,username);

            if (returned_product == null) // אם המוצר עדיין לא נבחר כלומר לא קיים 
            {
                return false;
            }
            return true;   // אמת אם קיים במערכת
        }


    }
}