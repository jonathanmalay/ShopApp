using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.Design;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ShopApp
{
    
    public class HomeSetting_Fragment : Android.Support.V4.App.Fragment
    {
        Dialog changePasswordDialog;
        TextView tv_toolbar_title;
        Button btnChangePassword, btnEditDetails, btnDialogChangePassword, btnPaymentMethods;
        EditText etNewPassword, etNewPasswordConrife,etOldPassword;     

        ISharedPreferences sp;
        User u;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.Fragment_ClientSettingHome, container, false);
        }



        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if(hidden == false)
            {
                this.tv_toolbar_title.Text = "הגדרות";
            }
        }


        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            string usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש
            this.u = await User.GetUser(usernameloged);

            this.tv_toolbar_title = Activity.FindViewById<TextView>(Resource.Id.tv_toolbar_title); //change the toolbar title to the name of the fragment 
            this.tv_toolbar_title.Text = "הגדרות";

            this.btnEditDetails = view.FindViewById<Button>(Resource.Id.btnSettingEtitdetails);
            this.btnChangePassword = view.FindViewById<Button>(Resource.Id.btnSettingChangePassword);
            this.btnPaymentMethods = view.FindViewById<Button>(Resource.Id.btnSettingPaymentMethods);

            this.btnEditDetails.Click += BtnEditDetails_Click;
            this.btnPaymentMethods.Click += BtnPaymentMethods_Click;
            this.btnChangePassword.Click += BtnChangePassword_Click;
         
            //Dialog
            this.changePasswordDialog = new Dialog(Activity);
            this.changePasswordDialog.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
            this.changePasswordDialog.SetContentView(Resource.Layout.Dialog_ClientChangePassword);
            this.changePasswordDialog.SetTitle("Change Password");
            this.changePasswordDialog.SetCancelable(true);
            etOldPassword = this.changePasswordDialog.FindViewById<EditText>(Resource.Id.et_ClientChangePasswordOldPassword);
            etNewPassword = this.changePasswordDialog.FindViewById<EditText>(Resource.Id.et_ClientChangePasswordNew);
            etNewPasswordConrife = this.changePasswordDialog.FindViewById<EditText>(Resource.Id.et_ClientChangePasswordConrife);
            btnDialogChangePassword = this.changePasswordDialog.FindViewById<Button>(Resource.Id.btn_ClientChangePasswordSave);
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