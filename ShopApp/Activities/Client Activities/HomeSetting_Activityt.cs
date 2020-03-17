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
    [Activity(Label = "HomeSetting_Activityt")]
    public class HomeSetting_Activityt : Android.Support.V4.App.Fragment
    {
        Dialog changePasswordDialog;
        TextView tv_toolbar_title;
        Button btnChangePassword, btnEditDetails, btnDialogChangePassword, btnPaymentMethods;
        EditText etNewPassword, etNewPasswordConrife,etOldPassword;     

        ISharedPreferences sp;
        User u;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.SettingHome_Layout, container, false);
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            this.tv_toolbar_title = view.FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.tv_toolbar_title.Text = "הגדרות";

            this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            string usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש
            this.u = await User.GetUser(usernameloged);

            this.btnEditDetails = view.FindViewById<Button>(Resource.Id.btnSettingEtitdetails);
            this.btnChangePassword = view.FindViewById<Button>(Resource.Id.btnSettingChangePassword);
            this.btnPaymentMethods = view.FindViewById<Button>(Resource.Id.btnSettingPaymentMethods);

            this.btnEditDetails.Click += BtnEditDetails_Click;
            this.btnPaymentMethods.Click += BtnPaymentMethods_Click;
            this.btnChangePassword.Click += BtnChangePassword_Click;
         
            //Dialog
            this.changePasswordDialog = new Dialog(Activity);
            this.changePasswordDialog.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            this.changePasswordDialog.SetContentView(Resource.Layout.Layout_SettingChangePassword);
            this.changePasswordDialog.SetTitle("Change Password");
            this.changePasswordDialog.SetCancelable(true);
            etOldPassword = this.changePasswordDialog.FindViewById<EditText>(Resource.Id.etManagerChangePasswordOldPassword);
            etNewPassword = this.changePasswordDialog.FindViewById<EditText>(Resource.Id.etManagerChangePasswordNew);
            etNewPasswordConrife = this.changePasswordDialog.FindViewById<EditText>(Resource.Id.etManagerChangePasswordConrife);
            btnDialogChangePassword = this.changePasswordDialog.FindViewById<Button>(Resource.Id.btnManagerChangePasswordSave);
            btnDialogChangePassword.Click += BtnDialogChangePassword_Click;
        }

     

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            this.changePasswordDialog.Show();
        }

        private void BtnPaymentMethods_Click(object sender, EventArgs e)
        {  
            
            Intent intent = new Intent(Activity, typeof(Activity_SettingPayment));//עובר לאקטיביטי הגדרות תשלום 
           this.StartActivity(intent);

        }

        private void BtnEditDetails_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(Activity_EditAccuntSetting));
           this.StartActivity(intent);



        }



        private async void  BtnDialogChangePassword_Click(object sender, EventArgs e)
        {  
                User u;
            ISharedPreferences sp;

                this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
                string usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש

                //מתבצעת בדיקה האם הסיסמא הישנה שהמשתמש הזין נכונה. במידה וכן הוא יוכל לשנות סיסמא לסיסמא חדשה.הבדיקה מלוות בהקפצת הודעות בהתאם  

                u = await User.ConrifePassword(etOldPassword.Text, usernameloged);//אם הסיסמא שהישנה שהמשתמש הזין היא נכונה אז יוחזר עצם מסוג יוזר

                if (u != null)
                {


                    if (etNewPassword.Text == etNewPasswordConrife.Text)
                    {

                        User.ChangeUserPassword(usernameloged, etNewPassword.Text);
                        //הקפצת הודעה למשתמש שהסיסמא שונתה בהצלחה 
                        Toast.MakeText(Activity, "! הסיסמא שונתה בהצלחה", ToastLength.Long).Show();
                    }

                    else
                    {
                        //הקפצת הודעה למשתמש שהסיסמאות שהזין אינם זהות 
                        Toast.MakeText(Activity, "שם משתמש או סיסמא שגויים!", ToastLength.Long).Show();

                    }
                }



                else
                {
                    //הקפצת הודעה למשתמש שהסיסמא הישנה שהזין שגויה 
                    Toast.MakeText(Activity, "סיסמא ישנה שגויה!", ToastLength.Long).Show();

                }
            

            


        }
    }
}