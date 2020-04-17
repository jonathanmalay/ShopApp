using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;//for the  bottom nevigation 
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ShopApp.Activities;

namespace ShopApp
{
    [Activity(Label = "Activity_ManagerHome")]
    public class Activity_ManagerHome : AppCompatActivity  //הגרסה החדשה של אקטיביטי זה  אפפקומפאטאקטיביטי
    {
        Button btn_Logout;
        ImageButton btn_toolbar_menu;
        ISharedPreferences sp;
        Manager m;
        BottomNavigationView bnv_Manager_Home;
        TextView tv_welcome_manager; 



        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout_ManagerHome);

            this.bnv_Manager_Home = FindViewById<BottomNavigationView>(Resource.Id.bottomNavigationViewManager);
            this.btn_toolbar_menu = FindViewById<ImageButton>(Resource.Id.btn_toolbar_menu);
            this.tv_welcome_manager = FindViewById<TextView>(Resource.Id.tvManagerHomeWelcomeManager); 
            this.sp = GetSharedPreferences("details", FileCreationMode.Private);
            string manager_usernameloged = this.sp.GetString("Username", "");
            this.m = await Manager.GetManager(manager_usernameloged);

            this.tv_welcome_manager.Text = " ברוך הבא " + m.FullName ; 

            this.bnv_Manager_Home.NavigationItemSelected += Bnv_Manager_Home_NavigationItemSelected;
            MenuInflater.Inflate(Resource.Menu.menu_bnv_Manager, this.bnv_Manager_Home.Menu); //set wich conteiner for the fragments to use(client or Maneger)
            FragmentHelper.LoadFragment(this, new Manager_Home_Fragment(), true); //the first fragment that wiil be shown after Login


            btn_toolbar_menu.Click += (s, arg) =>
            {  //יוצר את התפריט
                PopupMenu Manager_home_Menu = new PopupMenu(this, btn_toolbar_menu); 
                Manager_home_Menu.Inflate(Resource.Menu.menu_ManagerHome);
                Manager_home_Menu.MenuItemClick += Manager_home_Menu_MenuItemClick; ; 
                Manager_home_Menu.Show();

            };
        }

        private void Manager_home_Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)//הפעולות שמתבצעות כתוצאה מלחיצה על האפשרויות השונות בתפריט
        {
            ISharedPreferencesEditor editor = sp.Edit();

            switch (e.Item.ItemId)
            {
                case Resource.Id.menu_ManagerHomeLogOut:

                    editor.PutString("Username", "").Apply();
                    editor.PutBoolean("isManager", false).Apply();
                    Toast.MakeText(this, "you selected to log out", ToastLength.Long).Show();
                    Intent intentLogin = new Intent(this, typeof(MainActivity));
                    this.StartActivity(intentLogin);
                    break;

                case Resource.Id.action_register:

                    Intent intentRegister = new Intent(this, typeof(RegisterActivity));
                    this.StartActivity(intentRegister);
                    break;

                case Resource.Id.menu_ManagerHomeAccountSetting:

                    FragmentHelper.LoadFragment(this, new Fragment_ManagerHomeSetting(), true);
                    break;
            }
        }

        private void Bnv_Manager_Home_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
             if(e.Item.ItemId == Resource.Id.action_BnvManagerHome)
            {
                FragmentHelper.LoadFragment(this, new Manager_Home_Fragment(), true); //true because its manager conteiner and dosent the  client conteiner
            }


            if (e.Item.ItemId == Resource.Id.action_BnvManagerOrders)
            {
                FragmentHelper.LoadFragment(this, new Fragment_ManagerOrders(), true);

            }


            if (e.Item.ItemId == Resource.Id.action_BnvManagerProducts)
            {
                FragmentHelper.LoadFragment(this, new  Fragment_ManagerProducts(), true);

            }

            if (e.Item.ItemId == Resource.Id.action_BnvManagerSettings)
            {
                FragmentHelper.LoadFragment(this, new Fragment_ManagerHomeSetting(), true);

            }




        }

       

        public override void OnBackPressed() // diaable the back press button 
        {

        }

        public void Method()
        {
            throw new System.NotImplementedException();
        }
    }
}