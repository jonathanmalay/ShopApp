using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace ShopApp
{
    public static class FragmentHelper
    {
        public static void LoadFragment(FragmentActivity activity, Android.Support.V4.App.Fragment fragmentToLoad)
        {
            try
            {
                Android.Support.V4.App.FragmentManager fragmentManager = activity?.SupportFragmentManager;
                Android.Support.V4.App.FragmentTransaction fragmentTransaction = fragmentManager?.BeginTransaction();

                if (fragmentManager.Fragments != null)
                {
                    foreach (var frag in fragmentManager.Fragments)
                    {
                        if (frag != null && frag.IsVisible)
                        {
                            fragmentTransaction?.Hide(frag);
                        }
                    }
                }

                string tag = fragmentToLoad?.GetType()?.ToString();

                Android.Support.V4.App.Fragment savedFragment = fragmentManager?.FindFragmentByTag(tag);

                if (savedFragment == null)
                {
                    if(fragmentToLoad != null)
                    {
                        if (!fragmentToLoad.IsAdded)
                        {
                            fragmentTransaction
                                .Add(Resource.Id.frameLayoutContainerClient, fragmentToLoad, tag)
                                .AddToBackStack(tag);
                        }
                    }
                }
                else
                {
                    fragmentTransaction.Show(savedFragment).AddToBackStack(tag);
                }

                fragmentTransaction.Commit();
            }
            catch(Exception e)
            {
                Console.WriteLine("Fragment Helper Error: " + e.Message);
            }
        }
    }
}