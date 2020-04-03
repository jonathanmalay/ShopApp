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
    [Activity(Label = "Activity_LoginManager")]
    public class Activity_LoginManager : Activity
    {
        EditText etShopName;
        EditText etPassword;
        Button btnLoginManager;
        ISharedPreferences sp;
        ProgressDialog pd;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout_LoginManager);

            this.etShopName = FindViewById<EditText>(Resource.Id.etLoginManagerUsername);
            this.etPassword = FindViewById<EditText>(Resource.Id.etLoginManagerPassword);
            this.btnLoginManager = FindViewById<Button>(Resource.Id.btnLoginManagerLogin);
           

            this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            string usernameloged = this.sp.GetString("Username", "");//
            if (usernameloged != "")
            {
                //כשהמשתמש מחובר
                Intent intent = new Intent(this, typeof(Activity_ManagerHome));
                this.StartActivity(intent);
            }

            this.btnLoginManager.Click += BtnLoginManager_Click;

            AppData.Initialize(this);

        }

        private async void BtnLoginManager_Click(object sender, EventArgs e)
        {
            try
            {
                pd = ProgressDialog.Show(this, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true); //progress daialog....
                pd.SetProgressStyle(ProgressDialogStyle.Horizontal);//סוג הדיאלוג שיהיה
                pd.SetCancelable(false);//שלוחצים מחוץ לדיאלוג האם הוא יסגר


                string manager_username = this.etShopName.Text;
                string manager_password = this.etPassword.Text;



                Manager manager = await Manager.ConrifeManagerPassword(manager_password, manager_username);//בודק האם הסיסמא שייכת למשתמש והאם הוא קיים ובמידה וכן מחזירה את המשתמש כעצם
                if (manager != null) //במידה ותהליך ההזדהות צלח
                {
                    this.sp.Edit().PutString("Username", manager_username).Apply();//שומר בשרד רפרנס את השם של המשתמש שהתחבר
                    this.sp.Edit().PutBoolean("isManager", true).Apply();//כותב לשרד רפרנס שהמנהל מחובר  לצורך התחברות אוטומטית עתידית 
                    Intent intent = new Intent(this, typeof(Activity_ManagerHome));//עובר להום  מנהל אקטיביטי
                    this.StartActivity(intent);
                    Manager m = new Manager();
                    
                }

                else
                {
                    //הקפצת הודעה למשתמש ששם המשתמש או הסיסמא שגויים והוא צריך לבחור אחד חדש
                    Toast.MakeText(this, "שם משתמש או סיסמא שגויים!", ToastLength.Long).Show();
                }
                pd.Cancel();

            }

            catch(Exception)
            {
                Toast.MakeText(this, "!שגיאה", ToastLength.Long).Show();

            }




        }


    }
    
}
