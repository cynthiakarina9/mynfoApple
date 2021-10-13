namespace mynfo
{
    using CoreNFC;
    using Foundation;
    using Interfaces;
    using mynfo.ViewModels;
    using mynfo.Views;
    using NdefLibrary.Ndef;
    using Plugin.NFC;
    using Rg.Plugins.Popup.Services;
    using Services;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using UIKit;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, INFCNdefReaderSessionDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        public NFCNdefReaderSession Session { get; set; }
        public static string user_id_tag = "?";
        public string nameUser { get; set; }
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //Set DB root
            string dbName = "Mynfo.db3";
            string dbBinder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));//, "..", "Library", "Databases");
            string dbRoot = Path.Combine(dbBinder, dbName);
            Rg.Plugins.Popup.Popup.Init();

            global::Xamarin.Forms.Forms.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            DetectShakeTest();
            ToggleAccelerometer();
            LoadApplication(new App(dbRoot));
            DependencyService.Register<ILocalize>();
            return base.FinishedLaunching(app, options);
            //UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
            //if (statusBar != null && statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
            //{
            //    // change to your desired color 
            //    statusBar.BackgroundColor = Color.FromHex("#7f6550").ToUIColor();
            //}
        }

        #region Trigger nfc
        // Set speed delay for monitoring changes.
        Xamarin.Essentials.SensorSpeed speed = Xamarin.Essentials.SensorSpeed.Game;

        public void DetectShakeTest()
        {
            // Register for reading changes, be sure to unsubscribe when finished
            Xamarin.Essentials.Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
        }
        public void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            try
            {
                Vibration.Vibrate();

                invoke_lector();
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        public void invoke_lector()
        {
            InvokeOnMainThread(() =>
            {
                Session = new NFCNdefReaderSession(this, null, true);
                if (Session != null)
                {
                    Session.BeginSession();
                }
            });
        }

        public void ToggleAccelerometer()
        {
            try
            {
                if (Xamarin.Essentials.Accelerometer.IsMonitoring)
                    Xamarin.Essentials.Accelerometer.Stop();
                else
                    Xamarin.Essentials.Accelerometer.Start(speed);
            }
            catch (Xamarin.Essentials.FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
                Console.WriteLine("falla");
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                Console.WriteLine("falla");
            }
        }
        #endregion Trigger nfc

        public void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
        {            
            int user_id = 0;
            try
            {                
                if (messages != null && messages.Length > 0)
                {
                    var first = messages[0];
                    string messa = GetRecords(first.Records);
                    string[] variables2 = messa.Split("?");
                    string[] variables = messa.Split('=');
                    string[] depura_userid = variables[1].Split('&');
                    string tag_id = variables[2];
                    string name_user = variables2[1];
                    if (depura_userid[0] == "1")
                    {
                        user_id = Convert.ToInt32(depura_userid[0]);
                        nameUser = "mynfo";
                    }
                    else
                    {
                        var id_user = Encoding.UTF8.GetString(Convert.FromBase64String(variables2[2]));
                        user_id = Convert.ToInt32(id_user);
                        nameUser = Encoding.UTF8.GetString(Convert.FromBase64String(name_user));
                    }
                    
                    MainViewModel.GetInstance().Imprime_box = new Imprime_box();
                    var A = MainViewModel.GetInstance().Imprime_box;
                    if (write_tag.modo_escritura == false) 
                    { 
                        A.Consulta_user(user_id.ToString(), tag_id, nameUser); 
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                user_id = 0;
            }

            user_id_tag = user_id.ToString();
            if (write_tag.modo_escritura == true)
            {
                session.InvalidateSession();
                session.Dispose();
                Thread.Sleep(7000);
                write_tag.modo_escritura = false;
                MainViewModel.GetInstance().Write_tag = new write_tag();
                MainViewModel.GetInstance().Write_tag.ScanWriteAsync();                
            }
            session.InvalidateSession();
            Session.InvalidateSession();
        }

        public void DidInvalidate(NFCNdefReaderSession session, NSError error)
        {
            var readerError = (NFCReaderError)(long)error.Code;

            if (readerError != NFCReaderError.ReaderSessionInvalidationErrorFirstNDEFTagRead &&
                readerError != NFCReaderError.ReaderSessionInvalidationErrorUserCanceled)
            {
                InvokeOnMainThread(() =>
                {
                    var alertController = UIAlertController.Create("Session Invalidated", error.LocalizedDescription, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    //DispatchQueue.MainQueue.DispatchAsync(() =>
                    //{
                    //    this.PresentViewController(alertController, true, null);
                    //});
                });
            }
            session.InvalidateSession();
            Session.InvalidateSession();
            session.Dispose();
        }

        string GetRecords(NFCNdefPayload[] records)
        {
            string record = null;
            var results = new NFCNdefRecord[records.Length];
            for (var i = 0; i < records.Length; i++)
            {
                record = records[i].Payload.ToString();
            }
            return record;
        }

        public static String ProcessNFCRecord(NdefRecord record)
        {
            //Define the tag content we want to return.
            String tagContent = null;
            //Make sure we have a record.
            if (record != null)
            {
                //Check if the record is a URL.
                if (record.CheckSpecializedType(true) == typeof(NdefUriRecord))
                {
                    //The content is a URL.
                    tagContent = new NdefUriRecord(record).Uri;
                }
                else if (record.CheckSpecializedType(true) == typeof(NdefMailtoRecord))
                {
                    //The content is a mailto record.
                    tagContent = new NdefMailtoRecord(record).Uri;
                }
                else if (record.CheckSpecializedType(true) == typeof(NdefTelRecord))
                {
                    //The content is a tel record.
                    tagContent = new NdefTelRecord(record).Uri;
                }
                else if (record.CheckSpecializedType(true) == typeof(NdefSmsRecord))
                {
                    //The content is a sms record.
                    tagContent = new NdefSmsRecord(record).Uri;
                }
                else if (record.CheckSpecializedType(true) == typeof(NdefTextRecord))
                {
                    //The content is a text record.
                    tagContent = new NdefTextRecord(record).Text;
                }
                else
                {
                    //Try and force a pure text conversion.
                    tagContent = Encoding.UTF8.GetString(record.Payload);
                }
            }

            //Return the tag content.
            return tagContent;
        }
    }
}