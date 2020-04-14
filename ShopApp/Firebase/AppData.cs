using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Plugin.CloudFirestore;
using Plugin.FirebaseStorage;

namespace ShopApp
{
    class AppData
    {
        public static AppData appData { get; set; }
        //public static FirebaseStorage FirebaseStorage;
        //public static FirebaseStorageReference ProductsStorage;
        public static IStorage FirebaseStorage;
        public static IStorageReference ProductsStorage;


        public static IFirestore FireStore { get; set; }
        public static ICollectionReference usersCollection, shopCollection ,  paymentCollection, productCollection, cartCollection, orders_historyCollection,managersCollection , manager_ordersCollection;
        private AppData(Activity activity)
        { 
            FirebaseOptions options = new FirebaseOptions.Builder()//מגדיר את הנתונים של הפיירבייס שלי כדי שאוכל להתחבר אליו וליצור תקשורת 
                                            .SetApiKey("AIzaSyAC6Q1GPP0AjB17AkkVI1m0PI03t53_ABY")//
                                            .SetProjectId("bagrutproject-dbe7a")
                                            .SetApplicationId("com.malay.shopapp")//
                                            .SetDatabaseUrl("https://bagrutproject-dbe7a.firebaseio.com")//  הקישור למיקום של המסד נתונים של הפיירבייס 
                                            .SetStorageBucket("bagrutproject-dbe7a.appspot.com")
                                            .Build();

            FirebaseApp app = FirebaseApp.InitializeApp(activity, options);//יוצר את החיבור  עם השרת של הפייר בייס ברגע שאני פונה למשתנה אפפ   
            FireStore = CrossCloudFirestore.Current.Instance;
            usersCollection = FireStore.GetCollection("Users");
            paymentCollection = FireStore.GetCollection("Payment");
            productCollection = FireStore.GetCollection("Product");
            cartCollection = FireStore.GetCollection("Cart");
            shopCollection = FireStore.GetCollection("Shop");
            orders_historyCollection = FireStore.GetCollection("OrdersHistory");
            managersCollection = FireStore.GetCollection("Managers");
            manager_ordersCollection = FireStore.GetCollection("ManagerOrders");
       

            FirebaseStorage = CrossFirebaseStorage.Current.Instance;
            ProductsStorage = FirebaseStorage.RootReference.GetChild("ProductsNew");
        }



        public static void Initialize(Activity activity)//reload the connection withe the firebase database(with the server)
        {    
                  
         
            if(appData == null)
            {
                appData = new AppData(activity);
            }
        }

        public  static async Task DeleteAllDocumentsInCollection(ICollectionReference collectionReference) // delete all documents in the collection(i created this because that the isnt function to delete collection in the fire base Api) 
        {
            try
            {
                IQuerySnapshot documetns = await collectionReference.GetDocumentsAsync();


                foreach (IDocumentSnapshot document in documetns.Documents)
                {
                    Console.WriteLine("Deleting document {0}", document.Id);
                    await document.Reference.DeleteDocumentAsync();
                }

            }

            catch (Exception)
            {

            }
        }


    }
}