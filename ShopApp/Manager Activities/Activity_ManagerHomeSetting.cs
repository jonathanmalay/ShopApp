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
    [Activity(Label = "Activity_ManagerHomeSetting")]
    public class Activity_ManagerHomeSetting : Activity
    {

        Button btnChangePassword, btnEditDetails, btnDialogChangePassword, btnPaymentMethods;
        EditText etNewPassword, etNewPasswordConrife, etOldPassword;

        ISharedPreferences sp;
        Manager manager;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout_ManagerSettings);


            this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            string manager_usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש
            this.manager = await Manager.GetManager(manager_usernameloged);

            this.btnEditDetails = FindViewById<Button>(Resource.Id.btnManagerSettingEtitdetails);
            this.btnChangePassword = this.FindViewById<Button>(Resource.Id.btnManagerSettingChangePassword);
            this.btnPaymentMethods = this.FindViewById<Button>(Resource.Id.btnManagerSettingPaymentMethods);

            this.btnEditDetails.Click += BtnEditDetails_Click;
            this.btnPaymentMethods.Click += BtnPaymentMethods_Click;
            this.btnChangePassword.Click += (senderD, eD) =>//הקפצת מסך דיאלוג שמכיל לייאוט לשינוי סיסמא
            {

                Dialog d = new Dialog(this);
                d.SetContentView(Resource.Layout.layout_ManagerChangePassword); 
                d.SetTitle("שינוי סיסמה");
                d.SetCancelable(true);
                etOldPassword = d.FindViewById<EditText>(Resource.Id.etManagerChangePasswordOldPassword);
                etNewPassword = d.FindViewById<EditText>(Resource.Id.etManagerChangePasswordNew);
                etNewPasswordConrife = d.FindViewById<EditText>(Resource.Id.etManagerChangePasswordConrife);
                this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
                string username = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש

                btnDialogChangePassword = d.FindViewById<Button>(Resource.Id.btnManagerChangePasswordSave);
                btnDialogChangePassword.Click += BtnDialogChangePassword_Click;


                d.Show();


            };


        }

        private void BtnPaymentMethods_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnEditDetails_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnDialogChangePassword_Click(object sender, EventArgs e)
        {
           
        }
    }
}