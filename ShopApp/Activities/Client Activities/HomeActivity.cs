using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
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
        TextView tvWelcomeUser;
       
        BottomNavigationView bnvClient;
        User u;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.layout_home);

            this.tvWelcomeUser = FindViewById<TextView>(Resource.Id.tvWelcomeUser);

            this.bnvClient = FindViewById<BottomNavigationView>(Resource.Id.bottomNavigationViewClient);
            MenuInflater.Inflate(Resource.Menu.menu_client_home, this.bnvClient.Menu);
            //ColorStateList myCsl = new ColorStateList(
            //    new int[][]
            //    {
            //        new int [] {Android.Resource.Attribute.StateSelected},
            //        new int [] {-Android.Resource.Attribute.StateSelected }
            //    },
            //    new int[]
            //    {
            //        ContextCompat.GetColor(this, Resource.Color.white),
            //        ContextCompat.GetColor(this,Resource.Color.bottomNavigationSelectorFalse)
            //    }
            //    );
            //this.bnvClient.ItemIconTintList = myCsl;
            //this.bnvClient.ItemTextColor = myCsl;

            this.bnvClient.NavigationItemSelected += BnvClient_NavigationItemSelected;

            this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            string usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש
            this.u = await User.GetUser(usernameloged);

            if (this.u == null)
            {
                Toast.MakeText(this, "משתמש לא מחובר", ToastLength.Long).Show();
                Finish();
                return;
            }

            this.tvWelcomeUser.Text = u.Username + " ברוך הבא "; //מציג  הודעת  ברוך הבא בתוספת השם של המשתמש
            FragmentHelper.LoadFragment(this, new Client_HomeFragment()); //the first fragment that wiil be shown 
        }


        private void BnvClient_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)//מעבר בין הפרגמנטים על ידי לחיצה על האייקונים בתפריט
        {
            if (e.Item.ItemId == Resource.Id.action_BnvClientHome)
            {
                FragmentHelper.LoadFragment(this, new Client_HomeFragment()); //load homepage fragment
            }

            else if (e.Item.ItemId == Resource.Id.action_BnvClientHistory)
            {
                FragmentHelper.LoadFragment(this, new Client_HistoryOrders_Fragment()); 
            }

            else if (e.Item.ItemId == Resource.Id.action_BnvClientCurrentOrder)
            {
                FragmentHelper.LoadFragment(this, new ClientOrder_Fragment());
            }

            else if (e.Item.ItemId == Resource.Id.action_BnvClientSettings)
            {
                FragmentHelper.LoadFragment(this, new HomeSetting_Fragment());
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


      

        public override void OnBackPressed()
        {

        }

    }



}
