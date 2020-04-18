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
    
    public class Fragment_ManagerHomeSetting : Android.Support.V4.App.Fragment
    {

        Button btnChangePassword, btnEditDetails, btnDialogChangePassword, btnPaymentMethods;
        EditText etNewPassword, etNewPasswordConrife, etOldPassword;

        TextView tv_toolbar_title;
        ISharedPreferences sp;
        Manager manager;
        ProgressDialog pd;
        Dialog d;

        //Dialog edit acount Details
        Dialog dialogEditAccountDetails;
        Button btnDialogUpdateDetails;
        EditText  et_ShopPhoneNumber;
        TextView tv_ManagerUsername; 


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)//return a View from this method that is the root of the  fragment's layout.
        {

            return LayoutInflater.Inflate(Resource.Layout.Fragment_ManagerSettings, container, false);
        }

        public override void OnHiddenChanged(bool hidden)//whats happen every time that the fragment view again 
        {
            base.OnHiddenChanged(hidden);
            if (hidden == false)
            {

                this.tv_toolbar_title.Text = "הגדרות ";

            }
        }



        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);


            this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);
            string manager_usernameloged = this.sp.GetString("Username", "");
            this.manager = await Manager.GetManager(manager_usernameloged);
            this.tv_toolbar_title = Activity.FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.btnEditDetails = view.FindViewById<Button>(Resource.Id.btnManagerSettingEtitdetails);
            this.btnChangePassword = view.FindViewById<Button>(Resource.Id.btnManagerSettingChangePassword);

            this.tv_toolbar_title.Text = "הגדרות  ";
            this.btnEditDetails.Click += BtnEditDetails_Click;
            
            this.btnChangePassword.Click += (senderD, eD) =>//הקפצת מסך דיאלוג שמכיל לייאוט לשינוי סיסמא
            {

                d = new Dialog(Activity);

                d.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent); 
                d.SetContentView(Resource.Layout.Dialog_ManagerSettingChangePassword); 
                d.SetTitle("שינוי סיסמה");
                d.SetCancelable(true);
                etOldPassword = d.FindViewById<EditText>(Resource.Id.etManagerChangePasswordOldPassword);
                etNewPassword = d.FindViewById<EditText>(Resource.Id.etManagerChangePasswordNew);
                etNewPasswordConrife = d.FindViewById<EditText>(Resource.Id.etManagerChangePasswordConrife);
                this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);
                string username = this.sp.GetString("Username", "");

                btnDialogChangePassword = d.FindViewById<Button>(Resource.Id.btnManagerChangePasswordSave);
                btnDialogChangePassword.Click += BtnDialogChangePassword_Click;
                d.Show();
            };


            CreateDialogEditAccountDetails(); 
        }


        public async void CreateDialogEditAccountDetails() //create the dialog 
        {
            try
            {
                dialogEditAccountDetails = new Dialog(Activity);

                dialogEditAccountDetails.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
                dialogEditAccountDetails.SetContentView(Resource.Layout.Dialog_ManagerEditAccuntSetting);
                dialogEditAccountDetails.SetTitle("פרטי חנות");
                dialogEditAccountDetails.SetCancelable(true);
                tv_ManagerUsername = dialogEditAccountDetails.FindViewById<TextView>(Resource.Id.tvManagerDialogEditDetailsUsername);
                et_ShopPhoneNumber = dialogEditAccountDetails.FindViewById<EditText>(Resource.Id.etManagerDialogEditDetailsShopPhoneNumber);
                btnDialogUpdateDetails = dialogEditAccountDetails.FindViewById<Button>(Resource.Id.btnManagerEditAccuntSettingConrifeEdit);
   
                this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);
               
                this.tv_ManagerUsername.Text = this.sp.GetString("Username", ""); 
                this.et_ShopPhoneNumber.Text = await Manager.GetShopPhone(); 

                btnDialogUpdateDetails.Click += BtnDialogUpdateDetails_Click;
            }

            catch(Exception)
            {

            }
        }


        private async void BtnDialogUpdateDetails_Click(object sender, EventArgs e)//Update the  shop phone number
        {
            pd = ProgressDialog.Show(Activity, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true); //progress daialog....
            pd.SetProgressStyle(ProgressDialogStyle.Horizontal);
            pd.SetCancelable(false);

            try
            {
                if(CheckFields())
                {
                    await Manager.UpdateShopPhone(Activity ,et_ShopPhoneNumber.Text);

                }
            }
            catch(Exception)
            {
                Toast.MakeText(Activity, "אירעה שגיאה אנא בדוק את החיבור לרשת האינטרנט ", ToastLength.Long).Show();

            }
            pd.Hide(); 

        }

        private void BtnEditDetails_Click(object sender, EventArgs e)//make the edit details dialog visibale
        {

            dialogEditAccountDetails.Show(); 
        }


        private async void BtnDialogChangePassword_Click(object sender, EventArgs e)//save the new manager password 
        {
            try
            {

                pd = ProgressDialog.Show(Activity, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true); //progress daialog....
                pd.SetProgressStyle(ProgressDialogStyle.Horizontal);
                pd.SetCancelable(false);

        
                this.sp = Context.GetSharedPreferences("details", FileCreationMode.Private);
                string manager_usernameloged = this.sp.GetString("Username", "");

                //מתבצעת בדיקה האם הסיסמא הישנה שהמשתמש הזין נכונה. במידה וכן הוא יוכל לשנות סיסמא לסיסמא חדשה.הבדיקה מלוות בהקפצת הודעות בהתאם  

                manager = await Manager.ConrifeManagerPassword(etOldPassword.Text, manager_usernameloged);//אם הסיסמא שהישנה שהמשתמש הזין היא נכונה אז יוחזר עצם מסוג יוזר

                if (manager != null)
                {
                    if (manager.Password == etOldPassword.Text)
                    {
                        if (etNewPassword.Text == etNewPasswordConrife.Text)
                        {

                            Manager.ChangeManagerPassword(manager_usernameloged, etNewPassword.Text);
                            //הקפצת הודעה למשתמש שהסיסמא שונתה בהצלחה 
                            Toast.MakeText(Activity, "! הסיסמא שונתה בהצלחה", ToastLength.Long).Show();
                            pd.Cancel();
                            d.Dismiss();//close the dialog
                            
                        }

                        else
                        {
                            //הקפצת הודעה למשתמש שהסיסמאות שהזין אינם זהות 
                            Toast.MakeText(Activity, "הסיסמאות שהזנת  אינן זהות !", ToastLength.Long).Show();
                            pd.Cancel();
                        }
                    }

                    else
                    {

                        //הקפצת הודעה למשתמש שהסיסמא הישנה שהזין שגויה 
                        Toast.MakeText(Activity, "סיסמא ישנה שגויה!", ToastLength.Long).Show();
                        pd.Cancel();


                    }
                }


                else
                {
                    //הקפצת הודעה למשתמש כי הפרטים שהזין שגויים  
                    Toast.MakeText(Activity, "פרטים שגויים נסה שנית!", ToastLength.Long).Show();
                    pd.Cancel();

                }
            }


            catch(Exception)
            {
                Toast.MakeText(Activity, "ישנה שגיאה אנא בדוק את החיבור של מכשירך לרשת האינטרנט!", ToastLength.Long).Show();
                pd.Cancel();

            }
        }



        public bool CheckFields() // cheack if the values that the manager enterded are vaildes  
        {


            if (this.et_ShopPhoneNumber.Length() != 10)//בודק אם מספר הספרות שהמשתמש הזין חוקי לכתובת טלפון
            {
                this.et_ShopPhoneNumber.SetError("מספר ספרות לא חוקי!", null);
                this.et_ShopPhoneNumber.RequestFocus();
                return false;
            }

            return true; 
        }


    }
}