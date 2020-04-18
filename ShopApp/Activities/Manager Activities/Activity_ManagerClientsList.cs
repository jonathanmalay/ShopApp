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
    [Activity(Label = "Activity_ManagerClientsList")]
    public class Activity_ManagerClientsList : Activity
    {
        ISharedPreferences sp;
       
        Manager_Order currentOrder;

        ListView lv_ClientsList;
        Adapter_ManagerClients clientsList_adapter;
        Button btn_toolbar_BackPage;
        TextView tv_toolbar_title;
        ProgressDialog pd;
        List<User> clients;
        ImageButton btn_toolbar_menu; 


        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            SetContentView(Resource.Layout.Activity_ManagerClientsList); 
            this.tv_toolbar_title = this.FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.tv_toolbar_title.Text = "רשימת לקוחות ";
            this.btn_toolbar_BackPage = this.FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
            this.btn_toolbar_menu = FindViewById<ImageButton>(Resource.Id.btn_toolbar_menu);
            
            this.sp = GetSharedPreferences("details", FileCreationMode.Private);
            string manager_usernameloged = this.sp.GetString("Username", "");

            this.btn_toolbar_BackPage.Click += Btn_toolbar_BackPage_Click;

            pd = ProgressDialog.Show(this, "מאמת נתונים", "מאמת פרטים  אנא המתן...", true); //progress daialog....
            pd.SetProgressStyle(ProgressDialogStyle.Horizontal);//סוג הדיאלוג שיהיה
            pd.SetCancelable(false);//שלוחצים מחוץ לדיאלוג האם הוא יסגר

            this.lv_ClientsList = FindViewById<ListView>(Resource.Id.listView_ManagerClientsList_AllClients);


            this.clients = new List<User>();//רשימה של  כל הלקוחות
            this.clients = await User.GetAllClients();



            this.clientsList_adapter = new Adapter_ManagerClients(this  , clients);  

            this.lv_ClientsList.Adapter = this.clientsList_adapter;

            this.clientsList_adapter.NotifyDataSetChanged(); 
            this.pd.Hide();

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

                    Intent intent = new Intent(this, typeof(Activity_ManagerHome));
                    this.StartActivity(intent);
                    break;


            }
        }


        private void Btn_toolbar_BackPage_Click(object sender, EventArgs e)//close this activity(remove from the activities stack )
        {
            Finish(); 
        }
    }
}