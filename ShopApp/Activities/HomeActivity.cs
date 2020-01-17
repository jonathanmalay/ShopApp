using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ShopApp
{
    [Activity(Label = "HomeActivity")]
    public class HomeActivity : AppCompatActivity  //הגרסה החדשה של אקטיביטי זה  אפפקומפאטאקטיביטי
    {
        ISharedPreferences sp;
        Button btnStartOrder,btnPruchesHistory,btnSetting;
        TextView tvWelcomeUser;
        User u;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.layout_home);

                this.btnStartOrder = FindViewById<Button>(Resource.Id.btnStartShop);
                this.btnSetting = FindViewById<Button>(Resource.Id.btnHomeSetting);
                this.btnPruchesHistory = FindViewById<Button>(Resource.Id.btnPruchesHistory);
                this.tvWelcomeUser = FindViewById<TextView>(Resource.Id.tvWelcomeUser);
          
                this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
                string usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש
                this.u = await User.GetUser(usernameloged);


                tvWelcomeUser.Text =  u.Username + " ברוך הבא " ; //מציג  הודעת  ברוך הבא בתוספת השם של המשתמש

                this.btnSetting.Click += BtnSetting_Click;
                this.btnStartOrder.Click += BtnStartOrder_Click;
            this.btnPruchesHistory.Click += BtnPruchesHistory_Click;
           

        }

        private void BtnPruchesHistory_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Client_HistoryOrdersActivity));//עובר להאקיביטי של היסטוריית הזמנות 
            this.StartActivity(intent);

        }

        public override bool OnCreateOptionsMenu(IMenu menu) //רשום אובררייד בגלל שתפריט  קיים וערכו נולל לכן אנחנו דורסים את הערך הקודם ויוצרים תפריט חדש
        {
            MenuInflater.Inflate(Resource.Menu.menu_home, menu);//הופכים את המניו מאקאםאל לעצם מסוג  תפריט 

            return base.OnCreateOptionsMenu(menu);

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        { //פעולות המתרחשות כתוצאה מלחיצה על כפתורים בתפריט(אינטנטים) ב.
            this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            ISharedPreferencesEditor editor = sp.Edit();


            switch (item.ItemId)
            {

                case Resource.Id.action_logout:

                    editor.PutString("Username", "").Apply();
                    Toast.MakeText(this, "you selected to log out", ToastLength.Long).Show();
                    Intent intentLogin = new Intent(this, typeof(MainActivity));//עובר למסך ההתחברות 
                    this.StartActivity(intentLogin);
                    break;


                case  Resource.Id.action_register:
                    
                     Intent intentRegister = new Intent(this, typeof(RegisterActivity));//עובר לאקטיביטי הרשמה
                     this.StartActivity(intentRegister);
                     break;

                case  Resource.Id.action_accountSetting:

                    Intent intentAccountSetting = new Intent(this, typeof(HomeSetting_Activityt));//עובר לאקטיביטי הגדרות משתמש
                    this.StartActivity(intentAccountSetting);
                    break;
            }
     

            return base.OnOptionsItemSelected(item);
        }


        private void BtnStartOrder_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ClientOrder_Activity));//עובר לאקטיביטי של הזמנה 
            this.StartActivity(intent);

        }




        private void BtnSetting_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(HomeSetting_Activityt));//עובר להגדרות
            this.StartActivity(intent);

        }

        public override void OnBackPressed()
        {
            
        }

    }


 
}
