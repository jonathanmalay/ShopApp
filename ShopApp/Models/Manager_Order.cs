using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CloudFirestore;
using System.Threading.Tasks;
using Firebase.Firestore;

namespace ShopApp
{

    class Manager_Order
    {
        public string ID { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }

        public string ClientUsername { get; set; }

        public string City { get; set; }


        public bool IsDelivered { get; set; } //האם נשלחה ללקוח

        public string Address { get; set; }

        public string ClientPhone { get; set; }


        public List<SelectedProduct> CartList { get; set; }



        public Manager_Order()
        {

        }



        public static async Task<string> Add_Order(Activity activity, int price, DateTime date, string client_username, bool is_deliverd,List<SelectedProduct> products_list)    
        {
            try
            {
                Manager_Order order = new Manager_Order();

                order.Price = price;
                order.Date = date;
                order.ClientUsername = client_username;
                order.IsDelivered = is_deliverd;

                User u = await User.GetUser(client_username);
                if (u == null)
                {

                }

                else
                {
                    order.Address = u.StreetAddress;
                    order.City = u.City;
                    order.ClientPhone = u.PhoneNum;
                    order.CartList = products_list;//עגלת הקניות של המשתמש 
                }

                var document = AppData.manager_ordersCollection.CreateDocument();
                   
                order.ID = document.Id;

                await AppData.manager_ordersCollection.GetDocument(order.ID).SetDataAsync(order);//add the order to the manager_ordersCollection d

                 

                string username_client = order.ClientUsername;
                DateTime order_date = order.Date;
                int order_price = order.Price;
                Orders_History order_history_client = new Orders_History(order_date,order_price);//יצירת עצם מסוג היסטוריית הזמנה של לקוח
                Orders_History.AddOrderToHistory(username_client, order_history_client);//הוספת ההזמנה להיסטוריית ההזמנות של הלקוח 

                SelectedProduct.ClearAllProductFromCart(order.ClientUsername);//מנקה את עגלת הקניות של המשתמש על מנת שתהיה ריקה בקנייה הבאה



                return order.ID;
            }

            catch (Exception e)
            {
                Toast.MakeText(activity, "אירעה שגיאה נסה שנית", ToastLength.Long).Show();
                return null;
            }

        }


        public static async Task<string> Add_Order_NonExistUser(Activity activity, int price, DateTime date, string client_username, bool is_deliverd, List<SelectedProduct> products_list, string city, string address, string phone_number)//נשתמש בפונקציה זאת כאשר המנהל מוסיף הזמנה ללקוח שלא קיים באפליקציה לכן השתמשתי בהעמסת פונקציות
        {
            try
            {
                Manager_Order order = new Manager_Order();

                order.Price = price;
                order.Date = date;
                order.ClientUsername = client_username;
                order.IsDelivered = is_deliverd;
                order.Address = address;
                order.City = city;
                order.ClientPhone = phone_number;
                order.CartList = products_list;//עגלת הקניות של המשתמש 
             

                var document = AppData.manager_ordersCollection.CreateDocument();

                order.ID = document.Id;

                await AppData.manager_ordersCollection.GetDocument(order.ID).SetDataAsync(order);//add the order to the manager_ordersCollection


           
                return order.ID;
            }

            catch (Exception e)
            {
                Toast.MakeText(activity, "אירעה שגיאה נסה שנית", ToastLength.Long).Show();
                return null;
            }



        }



        public static async Task<List<Manager_Order>> GetAllOrders()//return all the orders of the shop 
        { 
            List<Manager_Order> allOrders = new List<Manager_Order>();
            try
            {
                IQuerySnapshot snapshot = await AppData.manager_ordersCollection.GetDocumentsAsync();
                List<Manager_Order> receivedOrders = snapshot.ToObjects<Manager_Order>().ToList();
                if (receivedOrders != null)
                {
                    allOrders = receivedOrders;
                }
            }
            catch (Exception)
            {
            }
            return allOrders;
        }




        public static async Task<Manager_Order> GetOrder(string orderId)//return the order object  ofb the order id that enterd by parameter to the function 
        { 
            try
            {   //TODO:  

                IDocumentSnapshot reference = await AppData.manager_ordersCollection.GetDocument(orderId).GetDocumentAsync();   //מחזיר מהשרת את ההזמנה עם אותו מספר זיהוי 

                return reference.ToObject<Manager_Order>();
            }

            catch (Exception)
            {

                return null;
            }
        }

        public static async Task<bool> OrderDeliverd(string order_id , bool is_sent) //משנה את סטטוס ההזמנה בהתאם למשתנה הבוליאני שהפעולה קיבלה אם שקר ההזמנה לא נשלחה ואם אמת ההזמנה נשלחה ללקוח 
        {
            try
            {
                   
                   await AppData.manager_ordersCollection.GetDocument(order_id).UpdateDataAsync("IsDelivered",is_sent ); 
                    return true; 
            }
            catch(Exception)
            {
                return false; 
            }
        }



        public static async Task<bool> DeleteOrder(string order_id)//delete the order from the firbase database 
        {
            try
            {
                await AppData.manager_ordersCollection.GetDocument(order_id).DeleteDocumentAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

      public static async Task<bool>  DeleteManagerCart(string manager_username)//מוחק את העגלה של המנהל בא נמצאים המוצרים של הזמנה ללקוח לא רשום
        {
            try
            {
                ICollectionReference  collection_reference  =   AppData.cartCollection.GetDocument(manager_username).GetCollection("SelectedProduct"); // קישור למסמך בשרת 
                await AppData.DeleteAllDocumentsInCollection(collection_reference); 
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        
    }

}
  