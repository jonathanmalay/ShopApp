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
    class User
    {
        public string Username { get; set; } //שם משתמש
        public string Password { get; set; } //סיסמא

        public string PhoneNum { get; set; } //מספר טלפון של בעל החשבון

        public string Email { get; set; } //כתובת אימייל של בעל החשבון

        public string FullName { get; set; } //שם של בעל החשבון

        public string City { get; set; } //עיר מגורים

        public string StreetAddress { get; set; } //רחוב מגורים ומספר בית 

        public User()
        {


        }
        public static async Task<bool> AddUser(Activity activity, string username, string password, string email, string phonenum, string fullname, string city ,string streetaddress)//הפעולה מקבלת את כל התכונות של המשתמש החדש שרוצים ליצור 
        { 
            try
            {

                //יוצר עצם חדש מסוג משתמש
                User u = new User();
                //מגדיר את התכונות של העצם החדש מסוג משתמש
                u.Username = username;
                u.Password = password;
                u.Email = email;
                u.FullName = fullname;
                u.PhoneNum = phonenum;
                u.City = city;
                u.StreetAddress = streetaddress;

                bool checkIfExist = await UserExist(username);
                // ועם לא קיים מחזירה שקר  username   מפעיל את הפעולה יוזרהקזיסט שמחזירה אמת אם קיים משתמש עם כבר קיים משתמש עם אותו 

                if (checkIfExist == false)
                {
                    await AppData.usersCollection.GetDocument(username).SetDataAsync(u);
                    //מוסיף את המשתמש החדש לפיירבייס במידה ולא קיים משתמש עם אותו שם משתמש 
                    return true;
                }

                else
                {  //הקפצת הודעה למשתמש ששם משתמש זה כבר קיים והוא צריך לבחור אחד חדש
                    Toast.MakeText(activity, "שם המשתמש קיים ,נא לבחור שם משתמש חדש !", ToastLength.Long).Show();
                    return false;
                }
            }
            catch(Exception)
            {
                Toast.MakeText(activity, "אירעה שגיאה", ToastLength.Long).Show();
               
            }
            return false; 
        }


        public static async Task<bool> UserExist(string username)//פעולה אשר בודקת האם המשתמש קיים במערכת של הפיירבייס
        {
            User returnedUser = await GetUser(username);

            if (returnedUser == null) // אם המשתמש לא קיים 
            {
                return false;
            }
            return true; //מחזירה אמת אם המשתמש קיים במערכת
        }




        public static async Task<User> GetUser(string username)
        {//פעולה אשר לוקחת משתמש מהפיירבייס
            try { 

            IDocumentSnapshot reference = await AppData.FireStore.GetCollection("Users").GetDocument(username).GetDocumentAsync();

            return reference.ToObject<User>();
                }

            catch(Exception)
            {
              
                return null;
            }

        }


        public static async Task<string> GetUserPassword(string username)//הפעולה מחזירה את הסיסמא של השם משתמש שהפעולה קיבלה
        {
            IDocumentSnapshot reference = await AppData.FireStore.GetCollection("Users").GetDocument(username).GetDocumentAsync();
            string passwordConrife = reference.ToObject<User>().Password; //מכניס לתוך משתנה מחרוזת את הערך של הסיסמא שנלקח מהפיירבייס

            return passwordConrife; //מחזיר את הסיסמא כמחרוזת

        }


        public static async void ChangeUserPassword(string username,string newPassword)
        {
            await AppData.FireStore.GetCollection("Users").GetDocument(username).UpdateDataAsync("Password",newPassword);


        }



        public static async void ChangeUserDetails(string username, string email,  string phoneNumber, string fullName, string city, string streetAddress)
        {
            await AppData.FireStore.GetCollection("Users").GetDocument(username).UpdateDataAsync("FullName", fullName ,"PhoneNum",phoneNumber,
                "Email",email , "City", city , "StreetAddress",streetAddress); //מעדכן את הפרטים של המשתמש על פי הערכים שהכניס בשדות


        }



        public static async Task<User> ConrifePassword(string enteredpassword, string username)//פעולה אשר מקבלת סיסמה ושם משתמש ובודקת האם הסיסמה היא הסיסמא השמורה בפיירבייס תחת אותו שם משתמש
        { 
            try
            {
                IDocumentSnapshot reference = await AppData.FireStore.GetCollection("Users").GetDocument(username).GetDocumentAsync();
                User currentUser = reference.ToObject<User>(); //
                if (currentUser.Password == enteredpassword)
                {
                    User u = await GetUser(username);
                    if (u == null)
                    {
                        return null;
                    }
                    else
                    {             //מחבר את המשתמש שסיסמתו נכונה לחשבון שלו
                        return u;
                    }                 //הפעולה מחזירה את המשתמש
                }
                return null; //אם הסיסמא לא נכונה מחזיר נאל

            }

            catch(Exception e)

            {
                return null;
            }
  
           

        }

    }

}
