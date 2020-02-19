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
    [Activity(Label = "Activity_ManagerHome")]
    public class Activity_ManagerHome : Activity

    {  
        Button btnAddProduct, btnOrders, btnSetting;
        ISharedPreferences sp;
        Manager m;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout_ManagerHome);

            this.btnAddProduct = FindViewById<Button>(Resource.Id.btnManagerHomeAddProduct);
            this.btnOrders = FindViewById<Button>(Resource.Id.btnManagerHomeOrders);
            this.btnSetting = FindViewById<Button>(Resource.Id.btnManagerHomeSetting);
            this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            string manager_usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש
            this.m = await Manager.GetManager(manager_usernameloged);


            this.btnAddProduct.Click += BtnAddProduct_Click;
            this.btnOrders.Click += BtnOrders_Click;
            this.btnSetting.Click += BtnSetting_Click;



        }

        private void BtnSetting_Click(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent(this, typeof(Activity_ManagerHomeSetting));
                this.StartActivity(intent);
            }


            catch (Exception )
            {
               
            }
        }


        private void BtnOrders_Click(object sender, EventArgs e)
        {
            try
            {
                //Intent intent = new Intent(this, typeof());
                //this.StartActivity(intent);
            }

            catch(Exception)
            {

            }
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                Intent intent = new Intent(this, typeof(Activity_ManagerAddProduct));
                this.StartActivity(intent);
            }


        catch(Exception)
            {
                
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

                    Intent intentAccountSetting = new Intent(this, typeof(HomeSetting_Activityt));//עובר לאקטיביטי הגדרות משתמש
                    this.StartActivity(intentAccountSetting);
                    break;
            }


            return base.OnOptionsItemSelected(item);
        }



    }
}