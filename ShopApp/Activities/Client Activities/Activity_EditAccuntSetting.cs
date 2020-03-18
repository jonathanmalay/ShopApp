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
    [Activity(Label = "Activity_EditAccuntSetting")]
    public class Activity_EditAccuntSetting : Activity
    {
        TextView tv_toolbar_title;
        EditText etEditFullName, etEditPhoneNumber, etEditCity ,etEditStreetAddress , etEditEmail, etEditUsername;
        Button btnSaveDetails, btn_backPage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Layout_EditAccuntSetting);

            this.tv_toolbar_title = FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.tv_toolbar_title.Text = "פרטי משתמש";
            this.etEditUsername = FindViewById<EditText>(Resource.Id.etEditAccuntSettingUsername);
            this.etEditFullName = FindViewById<EditText>(Resource.Id.etEditAccuntSettingFullName);
            this.etEditPhoneNumber = FindViewById<EditText>(Resource.Id.etEditAccuntSettingPhoneNumber);
            this.etEditEmail = FindViewById<EditText>(Resource.Id.etEditAccuntSettingEmail);
            this.etEditCity = FindViewById<EditText>(Resource.Id.etEditAccuntSettingCity);
            this.etEditStreetAddress = FindViewById<EditText>(Resource.Id.etEditAccuntSettingStreetAddress);
            this.btnSaveDetails = FindViewById<Button>(Resource.Id.btnEditAccuntSettingConrifeEdit);
            this.btn_backPage = FindViewById<Button>(Resource.Id.btn_toolbar_backPage);


            this.btnSaveDetails.Click += BtnSaveDetails_Click;
            this.btn_backPage.Click += Btn_backPage_Click;
        
           
        }

      
    
        private void Btn_backPage_Click(object sender, EventArgs e)
        {
            Finish(); //delete this activity from the stack
            Intent intent = new Intent(this, typeof(HomeSetting_Fragment));
            this.StartActivity(intent);

        }

        private void BtnSaveDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string fullName = this.etEditFullName.Text;
                string phoneNumber = this.etEditPhoneNumber.Text;
                string userName = this.etEditUsername.Text;
                string email = this.etEditEmail.Text;
                string city = this.etEditCity.Text;
                string streetAddress = this.etEditStreetAddress.Text;


                if (!CheckEditFields())
                {
                    return; //לא ממשיך בפעולה
                }

                User.ChangeUserDetails(userName, email, phoneNumber, fullName, city, streetAddress);
                //מוסיף משתמש חדש 
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


            if (this.etEditPhoneNumber.Length() != 9)//בודק אם מספר הספרות שהמשתמש הזין חוקי לכתובת טלפון
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



    }
}