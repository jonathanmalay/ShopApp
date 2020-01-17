﻿using System;
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
    class Payment
    {
        public string CardNum { get; set; }
        public string Date { get; set; }
        public string CVV { get; set; }

        public Payment()
        {

        }

        public static async void AddPaymentMethod(Activity activity, string cardnum, string date, string cvv, string username)
        { //הפעולה מקבלת את כל התכונות של האשראי של המשתמש ויוצרת קובץ בפיירבייס

            Payment p = new Payment();
            p.CardNum = cardnum;
            p.Date = date;
            p.CVV = cvv;

            await AppData.paymentCollection.GetDocument(username).SetDataAsync(p);


        }


        public static async void ChangePaymentMethod(string username, string cardnum,string datecard ,string card_cvv)
        {  //הפעולה מעדכנת את פרטי האשראי של המשתמש
            await AppData.FireStore.GetCollection("Payment").GetDocument(username).UpdateDataAsync("CardNum", cardnum, "Date", datecard , "CVV", card_cvv);
 
        }



    }

}