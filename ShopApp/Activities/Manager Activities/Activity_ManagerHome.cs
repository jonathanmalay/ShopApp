﻿using System;
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
        Button  btn_Logout;
        ISharedPreferences sp;
        Manager m;
        BottomNavigationView bnv_Manager_Home;



        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout_ManagerHome);

            this.bnv_Manager_Home = FindViewById<BottomNavigationView>(Resource.Id.bottomNavigationViewManager);
            this.btn_Logout = FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
            this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            string manager_usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש
            this.m = await Manager.GetManager(manager_usernameloged);


            this.btn_Logout.Click += Btn_Logout_Click;
            this.bnv_Manager_Home.NavigationItemSelected += Bnv_Manager_Home_NavigationItemSelected;
            FragmentHelper.LoadFragment(this, new Manager_Home_Fragment()); //the first fragment that wiil be shown after Login
        }

        private void Bnv_Manager_Home_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
             if(e.Item.ItemId == Resource.Id.action_BnvManagerHome)
            {
                FragmentHelper.LoadFragment(this, new Manager_Home_Fragment());
            }


            if (e.Item.ItemId == Resource.Id.action_BnvManagerOrders)
            {
                FragmentHelper.LoadFragment(this, new Fragment_ManagerOrders());

            }


            if (e.Item.ItemId == Resource.Id.action_BnvManagerProducts)
            {
                FragmentHelper.LoadFragment(this, new  Fragment_ManagerProducts());

            }

            if (e.Item.ItemId == Resource.Id.action_BnvManagerSettings)
            {
                FragmentHelper.LoadFragment(this, new Fragment_ManagerHomeSetting());

            }




        }

        private void Btn_Logout_Click(object sender, EventArgs e)
        {
            this.sp.Edit().PutString("Username","").Apply();
            this.sp.Edit().PutBoolean("isManager", false).Apply();
            Intent intent = new Intent(this, typeof(MainActivity));

            this.StartActivity(intent);
        }


        public override bool OnCreateOptionsMenu(IMenu menu) //רשום אובררייד בגלל שתפריט  קיים וערכו נולל לכן אנחנו דורסים את הערך הקודם ויוצרים תפריט חדש
        {
            MenuInflater.Inflate(Resource.Menu.menu_ManagerHome, menu);//הופכים את המניו מאקאםאל לעצם מסוג  תפריט 

            return base.OnCreateOptionsMenu(menu);

        }



        public override bool OnOptionsItemSelected(IMenuItem item)
        { //פעולות המתרחשות כתוצאה מלחיצה על כפתורים בתפריט(אינטנטים) ב.
            this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            ISharedPreferencesEditor editor = sp.Edit();

            switch (item.ItemId)
            {

                case Resource.Id.menu_ManagerHomeLogOut:

                    editor.PutString("Username", "").Apply();
                    editor.PutBoolean("isManager", false).Apply();
                    Toast.MakeText(this, "you selected to log out", ToastLength.Long).Show();
                    Intent intentLogin = new Intent(this, typeof(MainActivity));//עובר למסך ההתחברות 
                    this.StartActivity(intentLogin);
                    break;


          

                case Resource.Id.menu_ManagerHomeAccountSetting:

                    Intent intentAccountSetting = new Intent(this, typeof(Activity_ManagerHome));//עובר לאקטיביטי הגדרות משתמש
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