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
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        ISharedPreferences sp;
        EditText etFullName, etPhoneNumber ,etPassword,etEmail,etUsername,etCity,etStreetAddress;
        Button btnConrifRegister;
        TextView tv_toolbar_title;
        ProgressDialog pd;
        
        Button btn_backPage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.layout_Register);

            this.tv_toolbar_title = FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.tv_toolbar_title.Text = "הרשמה";
            this.btn_backPage = FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
            this.etFullName = FindViewById<EditText>(Resource.Id.etNameRegister);
            this.etPhoneNumber = FindViewById<EditText>(Resource.Id.etPhoneRegister);
            this.etPassword = FindViewById<EditText>(Resource.Id.etPasswordRegister);
            this.etUsername = FindViewById<EditText>(Resource.Id.etRegisterUsername);
            this.etEmail = FindViewById<EditText>(Resource.Id.etEmailRegister);
            this.etCity = FindViewById<EditText>(Resource.Id.etRegisterCity);
            this.etStreetAddress = FindViewById<EditText>(Resource.Id.etRegisterStreetAddress);
            this.btnConrifRegister = FindViewById<Button>(Resource.Id.btnConrifeRegister);
            this.btnConrifRegister.Click += BtnConrifRegister_Click;
            this.btn_backPage.Click += Btn_backPage_Click;
           
        }

        private void Btn_backPage_Click(object sender, EventArgs e)
        {
           Finish(); //עובר לאקטיביטי 
        }

        private async void BtnConrifRegister_Click(object sender, EventArgs e)
        { 
            try

            {
                pd = ProgressDialog.Show(this, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true); //progress daialog....
                pd.SetProgressStyle(ProgressDialogStyle.Horizontal);//סוג הדיאלוג שיהיה
                pd.SetCancelable(true);//שלוחצים מחוץ לדיאלוג האם הוא יסגר

                string fullName = this.etFullName.Text;
                string phoneNumber = this.etPhoneNumber.Text;
                string userName = this.etUsername.Text;
                string password = this.etPassword.Text;
                string email = this.etEmail.Text;
                string city = this.etCity.Text;
                string streetAddress = this.etStreetAddress.Text;
                

                if (!CheckFields())
                {
                    return; //לא ממשיך בפעולה
                }


               bool flag = await  User.AddUser(this, userName ,password, email, phoneNumber, fullName, city, streetAddress);
                if (flag)
                {
                    //מוסיף משתמש חדש
                    Toast.MakeText(this, "ההרשמה בוצעה בהצלחה!", ToastLength.Long).Show();

                    this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
                    ISharedPreferencesEditor editor = sp.Edit();
                    editor.PutString("Username", "").Apply();//מאפס את השרד רפרנס על מנת שלא יתחבר שוב לאותו משתמש שהיה מחובר בעת יצירת החשבון החדש


                    Intent intent = new Intent(this, typeof(MainActivity));//עובר לאקטיביטי login
                    this.StartActivity(intent);
                    pd.Cancel();//סוגר את הדיאלוג
                }
                else
                {
                    pd.Cancel(); 
                    return;  
                    //if there was some logic error in the thedetails that entered(for example a username alraedy exist )
                }
 
            }

            catch(Exception)
            {
                Toast.MakeText(this, "נסה שנית אירעה שגיאה", ToastLength.Long).Show();
                pd.Cancel();
            }
        }





        public bool CheckFields()
        {
           
                if (this.etFullName.Text.Length < 2)//בודק האם השם קטן משתי תווים
                {
                    this.etFullName.SetError("שם קצר מידי !", null);
                    this.etFullName.RequestFocus();

                    return false;
                }


                if (this.etFullName.Text.Any(char.IsDigit))//בודק האם בשם יש רק תווים חוקיים ולא מספרים
                {
                    this.etFullName.SetError("אין לרשום מספר בשם!", null);
                    this.etFullName.RequestFocus();

                    return false;
                }


                if (this.etPhoneNumber.Length() !=10 )//בודק אם מספר הספרות שהמשתמש הזין חוקי לכתובת טלפון
                {
                    this.etPhoneNumber.SetError("מספר ספרות לא חוקי!", null);
                    this.etPhoneNumber.RequestFocus();
                    return false;
                }


                

                if (this.etEmail.Text.IndexOf('@') < 1)//בודק האם הכתובת אימייל חוקית במידה ולא מחזיר שקר
                {
                    this.etEmail.SetError("כתובת אימייל אינה חוקית", null);
                    this.etEmail.RequestFocus();
                    return false;
                }


            if (this.etCity.Text.Any(char.IsDigit))//בודק האם בשם של העיר יש רק תווים חוקיים ולא מספרים
            {
                this.etCity.SetError("אין לרשום מספר בשם עיר !", null);
                this.etCity.RequestFocus();

                return false;
            }


            return true;
           
        }
    }
}