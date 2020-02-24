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
    class Orders_History
    {
        public DateTime Date { get; set;}
        public int Total_Price { get; set;}
        public Orders_History() { }// הפעולה הריקה בשביל הפייר בייס על מנת שלא תיווצר שגיאה
        public Orders_History(DateTime date , int total_price)
        {
            this.Date = date;
            this.Total_Price = total_price;
        }

        public static async void AddOrderToHistory(string username  ,Orders_History order)
        {
            try
            {
                string file_name = order.Date.Day.ToString() + order.Date.Month.ToString() + order.Date.Year.ToString(); //השם של ההזמנה בקולקשיין הזמנות של אותו לקוח יהיה התאריך של ההזמנה ,בשל בעיה שצצה בשמירת עצם מסוג תאריך כשם של מסמך בפייר בייס אילתרתי 

                await AppData.orders_historyCollection.GetDocument(username).GetCollection("Orders").GetDocument(file_name).SetDataAsync(order);//מוסיף הזמנה  לקולקשיין היסטוריית הזמנות של אותו אדם בפיירבייס
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        public static async Task<Orders_History> GetOrderInHistory(string username, DateTime date)//הפעולה מחזירה את הזמנה שנעשתה 
        {
            try
            {
                IDocumentSnapshot reference = await AppData.orders_historyCollection.GetDocument(username).GetCollection("Orders").GetDocument(date.ToString()).GetDocumentAsync();

                return reference.ToObject<Orders_History>();//מחזירה עצם מסוג הזמנה
            }

            catch (Exception)
            {
                return null;
            }
        }


        public static async Task<List<Orders_History>> GetAllOrders(string username)
        {
            try
            {
                IQuerySnapshot snapshot = await AppData.orders_historyCollection.GetDocument(username).GetCollection("Orders").GetDocumentsAsync();//לוקח את כל ההזמנות שביצע אותו משתמש
                List<Orders_History> orders_history = snapshot.ToObjects<Orders_History>().ToList(); //ממיר לליסט את כל הזמנות 
                return orders_history;
            }
            catch (Exception)
            {
                List<Orders_History> emptyList = new List<Orders_History>();
                return emptyList;
            }

        }
    }
}