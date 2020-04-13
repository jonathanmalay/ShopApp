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
    class Manager : User //יורש  את כל התכונות מיוזר מכיוון שגם מנהל הוא משתמש רק עם תכונות נוספות 
    {
       
        public string ShopName { get; set; } //שם החנות
       


        public Manager()
        {
          

        }




        public static async void AddManager(Activity activity, string username, string password, string email, string phone_num, string fullname, string city, string street_address,string shop_name)//adds new manager to the firebase database
        { 
            try
            {

                Manager m = new Manager();
                m.Username = username;
                m.Password = password;
                m.ShopName = shop_name;
                m.PhoneNum = phone_num;
                m.Email = email;
                m.FullName = fullname;
                m.StreetAddress = street_address;
                m.City = city;

               bool  manager_check = await ManagerExist(username);
                // ועם לא קיים מחזירה שקר  username   מפעיל את הפעולה יוזרהקזיסט שמחזירה אמת אם קיים משתמש עם כבר קיים משתמש עם אותו 

                if (manager_check == false)
                {
                    await AppData.managersCollection.GetDocument(username).SetDataAsync(m);
                    //מוסיף את המשתמש החדש לפיירבייס במידה ולא קיים משתמש עם אותו שם משתמש 
                }

                else
                {  //הקפצת הודעה למשתמש ששם משתמש זה כבר קיים והוא צריך לבחור אחד חדש
                    Toast.MakeText(activity, "שם המשתמש קיים ,נא לבחור שם משתמש חדש !", ToastLength.Long).Show();
                }
            }

            catch (Exception)
            {
                Toast.MakeText(activity, "אירעה שגיאה", ToastLength.Long).Show();

            }


        }

        public static async Task<bool> ManagerExist(string manager_username)//פעולה אשר בודקת האם המשתמש קיים במערכת של הפיירבייס
        {
            Manager returnedManager = await GetManager(manager_username);

            if (returnedManager == null) // אם המשתמש לא קיים 
            {
                return false;
            }
            return true; //מחזירה אמת אם המשתמש קיים במערכת
        }


        public static async Task<Manager> GetManager(string username)//returns manager object from the firebase database
        {
            try
            {
                IDocumentSnapshot reference = await AppData.managersCollection.GetDocument(username).GetDocumentAsync();

                return reference.ToObject<Manager>();
            }

            catch (Exception)
            {

                return null;
            }

        }




        public static async  Task<Manager> ConrifeManagerPassword(string enteredpassword, string manager_username)//פעולה אשר מקבלת סיסמה ושם משתמש ובודקת האם הסיסמה היא הסיסמא השמורה בפיירבייס תחת אותו שם משתמש
        { 
            try
            {
                IDocumentSnapshot reference = await AppData.managersCollection.GetDocument(manager_username).GetDocumentAsync();
                Manager current_manager = reference.ToObject<Manager>();
             
                if (current_manager.Password == enteredpassword)
                {
                    Manager manager = await GetManager(manager_username);
                    if (manager == null)
                    {
                        return null;
                    }
                    else
                    {             //מחבר את המנהל שסיסמתו נכונה לחשבון שלו
                        return manager;
                    }                 
                }
                return null; //אם הסיסמא לא נכונה מחזיר נאל

            }

            catch (Exception)
            {
                return null;
            }

        }





        public static async void ChangeManagerPassword(string username, string newPassword)//update the manager password in the firebase database
        {
            await AppData.managersCollection.GetDocument(username).UpdateDataAsync("Password", newPassword);


        }

    }
}