using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ShopApp
{
    public class Manager_Home_Fragment : Android.Support.V4.App.Fragment
    {
        Button btn_EditProducts, btnOrders, btnSetting , btn_backPage,btn_ClientsList;
        TextView tv_toolbar_title;




        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.Fragment_ManagerHome, container, false);
        }

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if (hidden == false)
            {

                this.tv_toolbar_title.Text = "מסך הבית";

            }
        }



        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            this.tv_toolbar_title = Activity.FindViewById<TextView>(Resource.Id.tv_toolbar_title);
            this.tv_toolbar_title.Text = "דף הבית"; //set the activity title
            this.btn_EditProducts = view.FindViewById<Button>(Resource.Id.btnManagerHomeEditProducts);
            this.btn_backPage = Activity.FindViewById<Button>(Resource.Id.btn_toolbar_backPage);
            this.btn_backPage.Visibility = ViewStates.Invisible; //hide this button from the toolbar 

            this.btnOrders = view.FindViewById<Button>(Resource.Id.btnManagerHomeOrders);
            this.btnSetting = view.FindViewById<Button>(Resource.Id.btnManagerHomeSetting);
            this.btn_ClientsList = view.FindViewById<Button>(Resource.Id.btn_ManagerHomeClientsList); 

            this.btn_EditProducts.Click += Btn_EditProducts_Click;
            this.btnOrders.Click += BtnOrders_Click;
            this.btn_ClientsList.Click += Btn_ClientsList_Click;
            this.btnSetting.Click += BtnSetting_Click;

        }

        private void Btn_ClientsList_Click(object sender, EventArgs e) // open the client list Activity 
        {
            Intent intentRegister = new Intent(Activity , typeof(Activity_ManagerClientsList));
            this.StartActivity(intentRegister);
        }

        private void Btn_EditProducts_Click(object sender, EventArgs e)//open the  ManagerProduct Fragment 
        {
            try
            {
                FragmentHelper.LoadFragment(Activity, new Fragment_ManagerProducts(), true);
            }

            catch (Exception)
            {

            }

        }


        private void BtnSetting_Click(object sender, EventArgs e) //open the  ManagerSetting  Fragment 
        {
            try
            {
                FragmentHelper.LoadFragment(Activity, new Fragment_ManagerHomeSetting(), true);

            }


            catch (Exception)
            {

            }
        }


        private void BtnOrders_Click(object sender, EventArgs e) //open the  Manager Orders   Fragment 
        {
            try
            {
                FragmentHelper.LoadFragment(Activity, new Fragment_ManagerOrders(), true);

            }

            catch (Exception)
            {

            }
        }
    }


  }