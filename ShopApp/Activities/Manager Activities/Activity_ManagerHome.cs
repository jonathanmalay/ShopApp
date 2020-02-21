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
    [Activity(Label = "Activity_ManagerHome")]
    public class Activity_ManagerHome : AppCompatActivity  //הגרסה החדשה של אקטיביטי זה  אפפקומפאטאקטיביטי
    {  
        Button btnAddProduct, btnOrders, btnSetting,btnRemoveProduct;
        ISharedPreferences sp;
        Manager m;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout_ManagerHome);

            this.btnAddProduct = FindViewById<Button>(Resource.Id.btnManagerHomeAddProduct);
            this.btnOrders = FindViewById<Button>(Resource.Id.btnManagerHomeOrders);
            this.btnSetting = FindViewById<Button>(Resource.Id.btnManagerHomeSetting);
            this.btnRemoveProduct = FindViewById<Button>(Resource.Id.btnManagerHomeRemoveProduct);
            this.sp = GetSharedPreferences("details", FileCreationMode.Private);//sp הגדרת
            string manager_usernameloged = this.sp.GetString("Username", "");//לוקח מהשרד רפרנס את השם משתמש
            this.m = await Manager.GetManager(manager_usernameloged);


            this.btnAddProduct.Click += BtnAddProduct_Click;
            this.btnOrders.Click += BtnOrders_Click;
            this.btnSetting.Click += BtnSetting_Click;
            this.btnRemoveProduct.Click += BtnRemoveProduct_Click;
            

        }

        private void BtnRemoveProduct_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Activity_ManagerRemoveProduct));
            this.StartActivity(intent);
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
                Intent intent = new Intent(this, typeof(Activity_ManagerOrders));
                this.StartActivity(intent);
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