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
    public class HomeSetting_Activityt : Activity

    {
        Button btnChangePassword, btnEditDetails,btnDialogChangePassword,btnPaymentMethods;
        EditText etNewPassword, etNewPasswordConrife,etOldPassword;     

        ISharedPreferences sp;
        User u;
        
        protected async override void OnCreate(Bundle savedInstanceState)
        { Button btnChangePassword, btnEditDetails;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SettingHome_Layout);

            
            this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            string usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש
            this.u = await User.GetUser(usernameloged);

            this.btnEditDetails = FindViewById<Button>(Resource.Id.btnSettingEtitdetails);
            this.btnChangePassword = this.FindViewById<Button>(Resource.Id.btnSettingChangePassword);
            this.btnPaymentMethods = this.FindViewById<Button>(Resource.Id.btnSettingPaymentMethods);


            this.btnEditDetails.Click += BtnEditDetails_Click;
            this.btnPaymentMethods.Click += BtnPaymentMethods_Click;
            this.btnChangePassword.Click += (senderD, eD) =>//הקפצת מסך דיאלוג שמכיל לייאוט לשינוי סיסמא
            {
                
                Dialog d = new Dialog(this);
                d.SetContentView(Resource.Layout.Layout_SettingChangePassword);
                d.SetTitle("Change Password");
                d.SetCancelable(true);
                etOldPassword = d.FindViewById<EditText>(Resource.Id.etChangePasswordOldPassword);
                etNewPassword = d.FindViewById<EditText>(Resource.Id.etChangePasswordNew);
                etNewPasswordConrife = d.FindViewById<EditText>(Resource.Id.etChangePasswordConrife);
                this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
                string username = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש

                btnDialogChangePassword = d.FindViewById<Button>(Resource.Id.btnChangePasswordSave);
                btnDialogChangePassword.Click += BtnDialogChangePassword_Click;

               
                d.Show();


            };

       


            // Create your application here
        }

        private void BtnPaymentMethods_Click(object sender, EventArgs e)
        {  
            
            Intent intent = new Intent(this, typeof(Activity_SettingPayment));//עובר לאקטיביטי הגדרות תשלום 
           this.StartActivity(intent);

        }

        private void BtnEditDetails_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activity_EditAccuntSetting));
           this.StartActivity(intent);



        }



        private async void  BtnDialogChangePassword_Click(object sender, EventArgs e)
        {  
                User u;
                ISharedPreferences sp;

                this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
                string usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש

                //מתבצעת בדיקה האם הסיסמא הישנה שהמשתמש הזין נכונה. במידה וכן הוא יוכל לשנות סיסמא לסיסמא חדשה.הבדיקה מלוות בהקפצת הודעות בהתאם  

                u = await User.ConrifePassword(etOldPassword.Text, usernameloged);//אם הסיסמא שהישנה שהמשתמש הזין היא נכונה אז יוחזר עצם מסוג יוזר

                if (u != null)
                {


                    if (etNewPassword.Text == etNewPasswordConrife.Text)
                    {

                        User.ChangeUserPassword(usernameloged, etNewPassword.Text);
                        //הקפצת הודעה למשתמש שהסיסמא שונתה בהצלחה 
                        Toast.MakeText(this, "! הסיסמא שונתה בהצלחה", ToastLength.Long).Show();
                    }

                    else
                    {
                        //הקפצת הודעה למשתמש שהסיסמאות שהזין אינם זהות 
                        Toast.MakeText(this, "שם משתמש או סיסמא שגויים!", ToastLength.Long).Show();

                    }
                }



                else
                {
                    //הקפצת הודעה למשתמש שהסיסמא הישנה שהזין שגויה 
                    Toast.MakeText(this, "סיסמא ישנה שגויה!", ToastLength.Long).Show();

                }
            

            


        }
    }
}