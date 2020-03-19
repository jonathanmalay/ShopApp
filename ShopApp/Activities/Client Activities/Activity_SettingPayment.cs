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
    [Activity(Label = "Activity_SettingPayment")]
    public class Activity_SettingPayment : Activity
    {
        EditText etCardNum, etDate, etCVV;
        Button btnPaymentSave,btn_BackPage;
        Payment p;
        ISharedPreferences sp;
        ProgressDialog pd;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout_SettingPayment);

            this.etCardNum = FindViewById<EditText>(Resource.Id.etSettingPaymentCardNum);
            this.etDate = FindViewById<EditText>(Resource.Id.etSettingPaymentDate);
            this.etCVV = FindViewById<EditText>(Resource.Id.etSettingPaymentCvv);
            this.btnPaymentSave = FindViewById<Button>(Resource.Id.btnSettingPaymentSave);
            this.btn_BackPage = FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
            this.btn_BackPage.Visibility = ViewStates.Visible; //show the button 



            this.btnPaymentSave.Click += BtnPaymentSave_Click;
            this.btn_BackPage.Click += Btn_BackPage_Click;

        }


        private void Btn_BackPage_Click(object sender, EventArgs e)
        {
            this.btn_BackPage.Visibility = ViewStates.Invisible; //hide the button 
            Finish(); //delete the Activity from the stack
        }

        private async void BtnPaymentSave_Click(object sender, EventArgs e)
        {
            try
            {
                pd = ProgressDialog.Show(this, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true); //progress daialog....
                pd.SetProgressStyle(ProgressDialogStyle.Horizontal);//סוג הדיאלוג שיהיה
                pd.SetCancelable(false);//שלוחצים מחוץ לדיאלוג האם הוא יסגר

                this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
                string username = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש



                if (etCVV.Length() == 3)//   חוקי  cvvבודק האם הקלט של ה
                { 
                    Payment.AddPaymentMethod(this, etCardNum.Text, etDate.Text, etCVV.Text, username);
                    Toast.MakeText(this, "פרטי האשראי נקלטו בהצלחה !", ToastLength.Long).Show();
                    Intent intent = new Intent(this, typeof(HomeActivity));//עובר להום אקטיביטי
                    this.StartActivity(intent);

                }


                else
                {
                    Toast.MakeText(this, "cvv  Needs to be 3 charecters!", ToastLength.Long).Show();
                }

                pd.Cancel();

            }



            catch (Exception)
            {   
                Toast.MakeText(this, "אין חיבור לרשת!", ToastLength.Long).Show();
                pd.Cancel();
            }
        }
    }
}