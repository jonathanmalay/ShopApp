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
using AlertDialog = Android.App.AlertDialog;

namespace ShopApp
{
    [Activity(Label = "HomeActivity")]
    public class HomeActivity : AppCompatActivity  //הגרסה החדשה של אקטיביטי זה  אפפקומפאטאקטיביטי
    {
        ISharedPreferences sp;
        Button btnStartOrder, btnPruchesHistory, btnSetting,btnContectUs;
        TextView tvWelcomeUser;
        User u;
        AlertDialog dialog_error_history;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.layout_home);

            AlertDialog.Builder builder_error_history = new AlertDialog.Builder(this);
            builder_error_history.SetTitle("Y.Malay Software");
            builder_error_history.SetMessage("לא ניתן לגשת ליהסטוריית ההזמנות מכיוון שטרם ביצעת הזמנות");
            builder_error_history.SetCancelable(true);
           dialog_error_history = builder_error_history.Create();//יוצר את הדיאלוג אך עדיין לא מציג אותו

            this.btnStartOrder = FindViewById<Button>(Resource.Id.btnStartShop);
            this.btnSetting = FindViewById<Button>(Resource.Id.btnHomeSetting);
            this.btnPruchesHistory = FindViewById<Button>(Resource.Id.btnPruchesHistory);
            this.btnContectUs = FindViewById<Button>(Resource.Id.btnContectUs);
            this.tvWelcomeUser = FindViewById<TextView>(Resource.Id.tvWelcomeUser);
            

            this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            string usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש
            this.u = await User.GetUser(usernameloged);

            if (this.u == null)
            {
                Toast.MakeText(this, "משתמש לא מחובר", ToastLength.Long).Show();
                Finish();
                return;
            }
            tvWelcomeUser.Text = u.Username + " ברוך הבא "; //מציג  הודעת  ברוך הבא בתוספת השם של המשתמש

            this.btnSetting.Click += BtnSetting_Click;
            this.btnStartOrder.Click += BtnStartOrder_Click;
            this.btnPruchesHistory.Click += BtnPruchesHistory_Click;
            this.btnContectUs.Click += BtnContectUs_Click;
        


        }

        private void BtnContectUs_Click(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent();

                intent.SetAction(Intent.ActionCall);

                Android.Net.Uri data = Android.Net.Uri.Parse("tel:" + "053-8285819"); //חייוג

                intent.SetData(data);

                StartActivity(intent);

            }

            catch(Exception)
            {
                Toast.MakeText(this, "אירעה שגיאה נסה שנית", ToastLength.Long).Show();

            }
        }

        private async void BtnPruchesHistory_Click(object sender, EventArgs e)
        {

            List<Orders_History> client_orders = await Orders_History.GetAllOrders(u.Username);

            if (client_orders != null && client_orders.Count > 0)//בודק האם יש ללקוח הזמנותקודמות ובמידה וכן עובר לאקטיביטי הזמנות קודמות
            {
                Intent intent = new Intent(this, typeof(Client_HistoryOrdersActivity));//עובר להאקיביטי של היסטוריית הזמנות 
                this.StartActivity(intent);
            }

            else
            {

                dialog_error_history.Show();//מראה את הדיאלוג שמסביר כי לא ניתן לגשת להיסטוריית ההזמנות מכיוון שהלקוח עדיין לא ביצע שום הזמנות 
               // Toast.MakeText(this, "אין לך הזמנות קודמות!", ToastLength.Long).Show();

            }

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


                case Resource.Id.action_register:

                    Intent intentRegister = new Intent(this, typeof(RegisterActivity));//עובר לאקטיביטי הרשמה
                    this.StartActivity(intentRegister);
                    break;

                case Resource.Id.action_accountSetting:

                    Intent intentAccountSetting = new Intent(this, typeof(Activity_ManagerHomeSetting));//עובר לאקטיביטי הגדרות משתמש
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
