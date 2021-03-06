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
    [Activity(Label = "Activity_EditAccuntSetting")]
    public class Activity_EditAccuntSetting : Activity
    {
        TextView tv_toolbar_title , tv_Username;
        EditText etEditFullName, etEditPhoneNumber, etEditCity ,etEditStreetAddress , etEditEmail;
        Button btnSaveDetails, btn_backPage;
        ISharedPreferences sp; 
        ImageButton btn_toolbar_menu;
        string usernameloged;
        User user;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity_ClientEditAccuntSetting);

            this.tv_toolbar_title = FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.tv_toolbar_title.Text = "פרטי משתמש";
            this.btn_toolbar_menu = FindViewById<ImageButton>(Resource.Id.btn_toolbar_menu); 
            this.tv_Username = FindViewById<TextView>(Resource.Id.tv_EditAccuntSettingUsername);
            this.etEditFullName = FindViewById<EditText>(Resource.Id.etEditAccuntSettingFullName);
            this.etEditPhoneNumber = FindViewById<EditText>(Resource.Id.etEditAccuntSettingPhoneNumber);
            this.etEditEmail = FindViewById<EditText>(Resource.Id.etEditAccuntSettingEmail);
            this.etEditCity = FindViewById<EditText>(Resource.Id.etEditAccuntSettingCity);
            this.etEditStreetAddress = FindViewById<EditText>(Resource.Id.etEditAccuntSettingStreetAddress);
            this.btnSaveDetails = FindViewById<Button>(Resource.Id.btnEditAccuntSettingConrifeEdit);
            this.btn_backPage = FindViewById<Button>(Resource.Id.btn_toolbar_backPage);


            this.sp = GetSharedPreferences("details", FileCreationMode.Private);
             usernameloged = this.sp.GetString("Username", "");

             user = await User.GetUser(usernameloged);

            this.tv_Username.Text = user.Username;
            this.etEditFullName.Text = user.FullName;
            this.etEditPhoneNumber.Text = user.PhoneNum;
            this.etEditStreetAddress.Text = user.StreetAddress;
            this.etEditCity.Text = user.City;
            this.etEditEmail.Text = user.Email; 
            

            this.btnSaveDetails.Click += BtnSaveDetails_Click;
            this.btn_backPage.Click += Btn_backPage_Click;

            btn_toolbar_menu.Click += (s, arg) =>
            {  //יוצר את התפריט
                PopupMenu Client_home_Menu = new PopupMenu(this, btn_toolbar_menu);
                Client_home_Menu.Inflate(Resource.Menu.menu_home);
                Client_home_Menu.MenuItemClick += Client_home_Menu_MenuItemClick; 
                Client_home_Menu.Show();

            };


        }

      
    
        private void Btn_backPage_Click(object sender, EventArgs e)
        {
            Finish(); //delete this activity from the stack
        }

        private void BtnSaveDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string fullName = this.etEditFullName.Text;
                string phoneNumber = this.etEditPhoneNumber.Text;
                //string userName = this.tv_Username.Text;
                string email = this.etEditEmail.Text;
                
                string city = this.etEditCity.Text;
                string streetAddress = this.etEditStreetAddress.Text;


                if (!CheckEditFields())
                {
                    return; //לא ממשיך בפעולה
                }

                User.ChangeUserDetails(usernameloged, email, phoneNumber, fullName, city, streetAddress);
                // מעדכן את פרטי המשתמש החדשים 
                Toast.MakeText(this, "!!פרטי המשתמש החדשים עודכנו בהצלחה ", ToastLength.Long).Show();

            }

            catch (Exception)
            {
                Toast.MakeText(this, "נסה שנית אירעה שגיאה", ToastLength.Long).Show();
            }

        }


        public bool CheckEditFields()
        {

            if (this.etEditFullName.Text.Length < 2)//בודק האם השם קטן משתי תווים
            {
                this.etEditFullName.SetError("שם קצר מידי !", null);
                this.etEditFullName.RequestFocus();

                return false;
            }


            if (this.etEditFullName.Text.Any(char.IsDigit))//בודק האם בשם יש רק תווים חוקיים ולא מספרים
            {
                this.etEditFullName.SetError("אין לרשום מספר בשם!", null);
                this.etEditFullName.RequestFocus();

                return false;
            }


            if (this.etEditPhoneNumber.Length() != 10)//בודק אם מספר הספרות שהמשתמש הזין חוקי לכתובת טלפון
            {
                this.etEditPhoneNumber.SetError("מספר ספרות לא חוקי!", null);
                this.etEditPhoneNumber.RequestFocus();
                return false;
            }



            if (this.etEditEmail.Text.IndexOf('@') < 1)//בודק האם הכתובת אימייל חוקית במידה ולא מחזיר שקר
            {
                this.etEditEmail.SetError("כתובת אימייל אינה חוקית", null);
                this.etEditEmail.RequestFocus();
                return false;
            }


            if (this.etEditCity.Text.Any(char.IsDigit))//בודק האם בשם של העיר יש רק תווים חוקיים ולא מספרים
            {
                this.etEditCity.SetError("אין לרשום מספר בשם עיר !", null);
                this.etEditCity.RequestFocus();

                return false;
            }



            return true;

        }


        private void Client_home_Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)//פעולות המתרחשות כתוצאה מלחיצה על כפתורים בתפריט(אינטנטים
        {
            ISharedPreferencesEditor editor = sp.Edit();

            switch (e.Item.ItemId)
            {
                case Resource.Id.action_logout:

                    editor.PutString("Username", "").Apply();
                    Toast.MakeText(this, "you selected to log out", ToastLength.Long).Show();
                    Intent intentLogin = new Intent(this, typeof(MainActivity));
                    this.StartActivity(intentLogin);
                    break;

                case Resource.Id.action_register:

                    Intent intentRegister = new Intent(this, typeof(RegisterActivity));
                    this.StartActivity(intentRegister);
                    break;

            }
        }




    }
}