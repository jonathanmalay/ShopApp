﻿using System;
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

        public static async void Add_Order(Activity activity, int price, DateTime date, string client_username, bool is_deliverd)
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

                }

                var document = AppData.manager_ordersCollection.CreateDocument();

                order.ID = document.Id;

                await document.SetDataAsync(order);
            }

            catch (Exception)
            {
                Toast.MakeText(activity, "אירעה שגיאה נסה שנית", ToastLength.Long).Show();

            }

        }





        public static async Task<List<Manager_Order>> GetAllOrders()
        { //return all the orders of the shop 
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


    }
}