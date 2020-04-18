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
    class Adapter_ManagerClients : BaseAdapter<User>
    {

        Activity activity;
        public List<User> Manager_Clients_list { get; set; }

        public Adapter_ManagerClients(Activity activity, List<User> clients_list)
        {
            this.activity = activity;
            this.Manager_Clients_list = clients_list;

        }

        public override int Count    // מחזיר את כמות האיברים שיש 
        {
            get { return this.Manager_Clients_list.Count; }
        }

        public override User this[int position]//return the user in the position
        {
            get
            {
                return this.Manager_Clients_list[position];
            }
        }


        public override long GetItemId(int position)
        {
            return position;
        }



        public override View GetView(int position, View convertView, ViewGroup parent)//return the view of the cell in the list view 
        {
            if (convertView == null)
            {
                convertView = this.activity.LayoutInflater.Inflate(Resource.Layout.Cell_ManagerClientsAdapter, parent, false);
            }

            TextView tv_clientEmail = convertView.FindViewById<TextView>(Resource.Id.tv_costum_ManagerClientsAdapterEmail);
            TextView tv_ClientName = convertView.FindViewById<TextView>(Resource.Id.tv_costum_ManagerClientsAdapterFullName);
            TextView tv_ClientCity = convertView.FindViewById<TextView>(Resource.Id.tv_costum_ManagerClientsAdapterCity);
            TextView tv_ClientAddress = convertView.FindViewById<TextView>(Resource.Id.tv_costum_ManagerClientsAdapterAddress);
            Button btn_callClient = convertView.FindViewById<Button>(Resource.Id.btn_costum_ManagerClientsAdapterCallToClient);

            User temp_user = Manager_Clients_list[position];

            tv_ClientName.Text = " שם הלקוח: " + temp_user.FullName  ;
            tv_ClientCity.Text = " עיר מגורים: " + temp_user.City ;
            tv_ClientAddress.Text = " כתובת: " + temp_user.StreetAddress  ;
            tv_clientEmail.Text = " מייל: "+ temp_user.Email  ;
            

            btn_callClient.Click += (object sender, EventArgs e) =>
            {

                Intent intent = new Intent();
                intent.SetAction(Intent.ActionDial);
                Android.Net.Uri data = Android.Net.Uri.Parse("tel:" + temp_user.PhoneNum.ToString()); //חייוג
                intent.SetData(data);
                activity.StartActivity(intent);
            };

            return convertView;
        }

    }

    
}