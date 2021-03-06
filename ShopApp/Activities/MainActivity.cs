﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Android.Text.Method;
using System;

namespace ShopApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity //הגרסה החדשה של אקטיביטי זה  אפפקומפאטאקטיביטי
    {

        EditText etUsername, etPassword;
        Button btnLogin, btnLogin_As_Manager;
        TextView tvRegister;
        ISharedPreferences sp;
        ImageView ivShowPassword;
        ProgressDialog pd;
        bool PasswordIsVisibale ;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                this.SetContentView(Resource.Layout.Activity_Login);
                this.PasswordIsVisibale = false;
                this.etUsername = this.FindViewById<EditText>(Resource.Id.etLoginUsername);
                this.etPassword = this.FindViewById<EditText>(Resource.Id.etLoginPassword);
                this.tvRegister = this.FindViewById<TextView>(Resource.Id.tvMainActivityRegister);
                this.ivShowPassword = this.FindViewById<ImageView>(Resource.Id.ivLoginHidePassword);
                this.btnLogin = this.FindViewById<Button>(Resource.Id.btnLogin);
                this.btnLogin_As_Manager = this.FindViewById<Button>(Resource.Id.btnLoginAsManagerMain);


                this.btnLogin.Click += BtnLogin_Click;
                this.btnLogin_As_Manager.Click += BtnLogin_As_Manager_Click;
                this.tvRegister.Click += TvRegister_Click;
                this.ivShowPassword.Click += IvShowPassword_Click;


                this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
                string usernameloged = this.sp.GetString("Username", "");
                bool isManager = this.sp.GetBoolean("isManager", false);
                if (isManager)
                {
                    Intent intent = new Intent(this, typeof(Activity_ManagerHome));
                    this.StartActivity(intent);
                }
                else if (usernameloged != "")
                {
                    //כשהמשתמש מחובר
                    Intent intent = new Intent(this, typeof(HomeActivity));
                    this.StartActivity(intent);
                }


                AppData.Initialize(this);

            }

            catch (Exception)
            {

            }

        }

        private void BtnLogin_As_Manager_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activity_LoginManager));
            this.StartActivity(intent);
        }

        private void IvShowPassword_Click(object sender, System.EventArgs e)
        {
            if (PasswordIsVisibale) // when  the password is visibale (after clicking the button in the first time)
               etPassword.InputType = Android.Text.InputTypes.TextVariationVisiblePassword;
            else
                etPassword.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;

            etPassword.SetSelection(etPassword.Text.Length);
            PasswordIsVisibale = !PasswordIsVisibale; 
        }

        private void TvRegister_Click(object sender, System.EventArgs e)// move to the  register activity 
        { 

            Intent intent = new Intent(this, typeof(RegisterActivity));
            this.StartActivity(intent);

        }




        private async void BtnLogin_Click(object sender, System.EventArgs e)
        {
            pd = ProgressDialog.Show(this, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true); 
            pd.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pd.SetCancelable(false);


            User u = await User.ConrifePassword(etPassword.Text, etUsername.Text);//בודק האם הסיסמא שייכת למשתמש והאם הוא קיים ובמידה וכן מחזירה את המשתמש כעצם
            if (u != null) //במידה ותהליך ההזדהות צלח
            {
                this.sp.Edit().PutString("Username", etUsername.Text).Apply();//שומר בשרד רפרנס את השם של המשתמש שהתחבר
                Intent intent = new Intent(this, typeof(HomeActivity));
                this.StartActivity(intent);

            }

            else
            {
                Toast.MakeText(this, "שם משתמש או סיסמא שגויים!", ToastLength.Long).Show();
            }
            pd.Cancel();

        }


        public override void OnBackPressed()//disable the back page button  on the smartphone 
        {

        }
    }

    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class CopyOfMainActivity : AppCompatActivity //הגרסה החדשה של אקטיביטי זה  אפפקומפאטאקטיביטי
    {

        EditText etUsername, etPassword;
        Button btnLogin, btnLogin_As_Manager;
        TextView tvRegister;
        ISharedPreferences sp;
        ImageView ivShowPassword;
        ProgressDialog pd;
        bool PasswordIsVisibale;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                this.SetContentView(Resource.Layout.Activity_Login);
                this.PasswordIsVisibale = false;
                this.etUsername = this.FindViewById<EditText>(Resource.Id.etLoginUsername);
                this.etPassword = this.FindViewById<EditText>(Resource.Id.etLoginPassword);
                this.tvRegister = this.FindViewById<TextView>(Resource.Id.tvMainActivityRegister);
                this.ivShowPassword = this.FindViewById<ImageView>(Resource.Id.ivLoginHidePassword);
                this.btnLogin = this.FindViewById<Button>(Resource.Id.btnLogin);
                this.btnLogin_As_Manager = this.FindViewById<Button>(Resource.Id.btnLoginAsManagerMain);


                this.btnLogin.Click += BtnLogin_Click;
                this.btnLogin_As_Manager.Click += BtnLogin_As_Manager_Click;
                this.tvRegister.Click += TvRegister_Click;
                this.ivShowPassword.Click += IvShowPassword_Click;


                this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
                string usernameloged = this.sp.GetString("Username", "");
                bool isManager = this.sp.GetBoolean("isManager", false);
                if (isManager)
                {
                    Intent intent = new Intent(this, typeof(Activity_ManagerHome));
                    this.StartActivity(intent);
                }
                else if (usernameloged != "")
                {
                    //כשהמשתמש מחובר
                    Intent intent = new Intent(this, typeof(HomeActivity));
                    this.StartActivity(intent);
                }


                AppData.Initialize(this);

            }

            catch (Exception)
            {

            }

        }

        private void BtnLogin_As_Manager_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activity_LoginManager));
            this.StartActivity(intent);
        }

        private void IvShowPassword_Click(object sender, System.EventArgs e)
        {
            if (PasswordIsVisibale) // when  the password is visibale (after clicking the button in the first time)
                etPassword.InputType = Android.Text.InputTypes.TextVariationVisiblePassword;
            else
                etPassword.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;

            etPassword.SetSelection(etPassword.Text.Length);
            PasswordIsVisibale = !PasswordIsVisibale;
        }

        private void TvRegister_Click(object sender, System.EventArgs e)// move to the  register activity 
        {

            Intent intent = new Intent(this, typeof(RegisterActivity));
            this.StartActivity(intent);

        }




        private async void BtnLogin_Click(object sender, System.EventArgs e)
        {
            pd = ProgressDialog.Show(this, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true);
            pd.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pd.SetCancelable(false);


            User u = await User.ConrifePassword(etPassword.Text, etUsername.Text);//בודק האם הסיסמא שייכת למשתמש והאם הוא קיים ובמידה וכן מחזירה את המשתמש כעצם
            if (u != null) //במידה ותהליך ההזדהות צלח
            {
                this.sp.Edit().PutString("Username", etUsername.Text).Apply();//שומר בשרד רפרנס את השם של המשתמש שהתחבר
                Intent intent = new Intent(this, typeof(HomeActivity));
                this.StartActivity(intent);

            }

            else
            {
                Toast.MakeText(this, "שם משתמש או סיסמא שגויים!", ToastLength.Long).Show();
            }
            pd.Cancel();

        }


        public override void OnBackPressed()//disable the back page button  on the smartphone 
        {

        }

        public void Method()
        {
            throw new System.NotImplementedException();
        }
    }
}