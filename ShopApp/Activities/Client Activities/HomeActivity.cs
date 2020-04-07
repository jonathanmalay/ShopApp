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
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AlertDialog = Android.App.AlertDialog;
using PopupMenu = Android.Widget.PopupMenu;

namespace ShopApp
{
    public interface IHomeActivity1
    {
        void OnBackPressed();
        bool OnCreateOptionsMenu(IMenu menu);
        bool OnOptionsItemSelected(IMenuItem item);
    }

    [Activity(Label = "HomeActivity")]


    public class HomeActivity : AppCompatActivity  //הגרסה החדשה של אקטיביטי זה  אפפקומפאטאקטיביטי
, IHomeActivity1
    {
        ISharedPreferences sp;
        TextView tvWelcomeUser;
        BottomNavigationView bnvClient;
        User u;
   
        Button btn_toolbar_backPage , btn_toolbar_menu; 
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.layout_home);

            this.tvWelcomeUser = FindViewById<TextView>(Resource.Id.tvWelcomeUser);
        
            this.btn_toolbar_backPage = FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
            this.btn_toolbar_menu = FindViewById<Button>(Resource.Id.btn_toolbar_menu);

            this.bnvClient = FindViewById<BottomNavigationView>(Resource.Id.bottomNavigationViewClient);
            this.bnvClient.NavigationItemSelected += BnvClient_NavigationItemSelected;


            this.btn_toolbar_backPage.Visibility = ViewStates.Invisible; //hide the back page button cause i dont need him here

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
            FragmentHelper.LoadFragment(this, new Client_HomeFragment(), false); //the first fragment that wiil be shown 

            btn_toolbar_menu.Click += (s, arg) => 
            {  //יוצר את התפריט
                PopupMenu Client_home_Menu = new PopupMenu(this, btn_toolbar_menu); // מקשר את התפריט לכפתור שלו ב toolbar
                Client_home_Menu.Inflate(Resource.Menu.menu_home);
                Client_home_Menu.MenuItemClick += Client_home_Menu_MenuItemClick; //הפעולות שמתבצעות כתוצאה מלחיצה על האפשרויות השונות בתפריט
                Client_home_Menu.Show(); 
                
            };
        }

   

        private void BnvClient_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)//מעבר בין הפרגמנטים על ידי לחיצה על האייקונים בתפריט
        {
            if (e.Item.ItemId == Resource.Id.action_BnvClientHome)
            {
                FragmentHelper.LoadFragment(this, new Client_HomeFragment(), false); //load homepage fragment
            }

            else if (e.Item.ItemId == Resource.Id.action_BnvClientHistory)
            {
                FragmentHelper.LoadFragment(this, new Client_HistoryOrders_Fragment(), false);
            }

            else if (e.Item.ItemId == Resource.Id.action_BnvClientCurrentOrder)
            {
                FragmentHelper.LoadFragment(this, new ClientOrder_Fragment(), false);
            }

            else if (e.Item.ItemId == Resource.Id.action_BnvClientSettings)
            {
                FragmentHelper.LoadFragment(this, new HomeSetting_Fragment(), false);
            }
        }


        //public override bool OnCreateOptionsMenu(IMenu menu) //רשום אובררייד בגלל שתפריט  קיים וערכו נולל לכן אנחנו דורסים את הערך הקודם ויוצרים תפריט חדש
        //{
        //    MenuInflater.Inflate(Resource.Menu.menu_home, menu); //הופכים את המניו מאקאםאל לעצם מסוג  תפריט
        //    return base.OnCreateOptionsMenu(menu);
        //}

        private void Client_home_Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)//פעולות המתרחשות כתוצאה מלחיצה על כפתורים בתפריט(אינטנטים
        {
            ISharedPreferencesEditor editor = sp.Edit();

            switch (e.Item.ItemId)
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

                    FragmentHelper.LoadFragment(this, new HomeSetting_Fragment(), true);
                    break;
            }
        }


        //public override bool OnOptionsItemSelected( IMenuItem item)//פעולות המתרחשות כתוצאה מלחיצה על כפתורים בתפריט(אינטנטים).
        //{ 
            
        //    ISharedPreferencesEditor editor = sp.Edit();
        //    switch (item.ItemId)
        //    {
        //        case Resource.Id.action_logout:

        //            editor.PutString("Username", "").Apply();
        //            Toast.MakeText(this, "you selected to log out", ToastLength.Long).Show();
        //            Intent intentLogin = new Intent(this, typeof(MainActivity));//עובר למסך ההתחברות 
        //            this.StartActivity(intentLogin);
        //            break;


        //        case Resource.Id.action_register:

        //            Intent intentRegister = new Intent(this, typeof(RegisterActivity));//עובר לאקטיביטי הרשמה
        //            this.StartActivity(intentRegister);
        //            break;

        //        case Resource.Id.action_accountSetting:

        //            FragmentHelper.LoadFragment(this, new HomeSetting_Fragment(), true);
        //            break;
        //    }
        //    return base.OnOptionsItemSelected(item);
        //}


        public override void OnBackPressed()
        {
           
        }

    }



}
