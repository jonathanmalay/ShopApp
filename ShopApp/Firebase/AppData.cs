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
using Firebase.Storage;
using Plugin.CloudFirestore;

namespace ShopApp
{
    class AppData
    {
        public static AppData appData { get; set; }
        public static FirebaseStorage FirebaseStorage;
        public static FirebaseStorageReference ProductsStorage;

        public static IFirestore FireStore { get; set; }
        public static ICollectionReference usersCollection, paymentCollection, productCollection, cartCollection, orders_historyCollection,managersCollection , manager_ordersCollection;
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
            FireStore = CrossCloudFirestore.Current.Instance; //
            usersCollection = FireStore.GetCollection("Users");//מתחבר לקובץ משתמשים בפיירבייס
            paymentCollection = FireStore.GetCollection("Payment");//מתחבר לקובץ משתמשים בפיירבייס
            productCollection = FireStore.GetCollection("Product");//connect to the product files collection in the server.
            cartCollection = FireStore.GetCollection("Cart");//connect to the product files collection in the server.
            orders_historyCollection = FireStore.GetCollection("OrdersHistory");
            managersCollection = FireStore.GetCollection("Managers");//מתחבר לקולקשיין מנהלים
            manager_ordersCollection = FireStore.GetCollection("ManagerOrders");//connect to the managers orders collection
            FirebaseStorage = new FirebaseStorage("bagrutproject-dbe7a.appspot.com");
            ProductsStorage = FirebaseStorage.Child("Products");

        }



        public static void Initialize(Activity activity)
        {    
                  
            
            if(appData == null)
            {
                appData = new AppData(activity);
            }
        }

        public  static async Task DeleteAllDocumentsInCollection(ICollectionReference collectionReference) // made by me  , delete all documents in the collection 
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