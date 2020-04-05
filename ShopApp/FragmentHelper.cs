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
        public static void LoadFragment(FragmentActivity activity, Android.Support.V4.App.Fragment fragmentToLoad, bool Is_manager_side)
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
                    if (fragmentToLoad != null)
                    {
                        if (!fragmentToLoad.IsAdded)
                        {
                            if (Is_manager_side)
                            {
                                fragmentTransaction
                                    .Add(Resource.Id.frameLayoutContainerManager, fragmentToLoad, tag)//אם זה הצד מנהל אז משתמש בקונטיינר של הצד מנהל
                                    .AddToBackStack(tag);
                            }
                            else
                            {
                                fragmentTransaction
                                    .Add(Resource.Id.frameLayoutContainerClient, fragmentToLoad, tag)//קונטיינר של צד לקוח 
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
            }
            catch (Exception e)
            {
                Console.WriteLine("Fragment Helper Error: " + e.Message);
            }
        }
    }
}