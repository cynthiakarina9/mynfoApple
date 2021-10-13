using CoreFoundation;
using CoreNFC;
using Foundation;
using mynfo.Helpers;
using mynfo.Services;
using mynfo.ViewModels;
using mynfo.Views;
using Plugin.NFC;
using Rg.Plugins.Popup.Services;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using static mynfo.Views.TAGPage;

[assembly: Dependency(typeof(write_tag))]
namespace mynfo.Services
{
    public class write_tag : NFCNdefReaderSessionDelegate, IBackgroundDependency
    {
        public static NFCNdefReaderSession _tagSession;
        public static TaskCompletionSource<string> _tcs;
        public static bool modo_escritura = false;//

        public Task ScanWriteAsync()
        {
            try
            {

                if (!NFCNdefReaderSession.ReadingAvailable)
                {
                    throw new InvalidOperationException("Reading NDEF is not available");
                }
                _tcs = new TaskCompletionSource<string>();
                //var pollingOption = NFCPollingOption.Iso14443;
               // _tagSession = new NFCNdefReaderSession(this, DispatchQueue.CurrentQueue, true);
                _tagSession = new NFCNdefReaderSession(this, DispatchQueue.CurrentQueue, true);

                _tagSession.AlertMessage = Languages.ScanTag;

                _tagSession?.BeginSession();
                return _tcs.Task;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public override void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
        {
        }
        [Foundation.Export("readerSession:didDetectTags:")]
        [Foundation.Preserve(Conditional = true)]
        public override void DidDetectTags(NFCNdefReaderSession session, INFCNdefTag[] tags)
        {
            try
            {
                string user_id_tag = "0";
                if (user_id_tag == "?")
                {
                    Task task = App.DisplayAlertAsync("¡Escanea este Tag para escribirlo!");
                    session.InvalidateSession();
                    session.Dispose();
                    AppDelegate.user_id_tag = "?";
                }
                else
                {
                    if ((Convert.ToInt32(user_id_tag) == Convert.ToInt32(MainViewModel.GetInstance().User.UserId.ToString())) || (Convert.ToInt32(user_id_tag) == 0))
                    {
                        try
                        {
                            var nFCNdefTag = tags[0];
                            session.ConnectToTag(nFCNdefTag, CompletionHandler);
                            string dominio = "http://boxweb.azurewebsites.net/";
                            string user = MainViewModel.GetInstance().User.UserId.ToString();
                            string tag_id = "";
                            string url = "";
                            if (user == "1")
                            {
                                url = dominio + "index3.aspx?user_id=" + user + "&tag_id=" + tag_id;
                            }
                            else
                            {
                                var Fullname = Convert.ToBase64String(Encoding.UTF8.GetBytes(MainViewModel.GetInstance().User.FullName));
                                var UserName = Convert.ToBase64String(Encoding.UTF8.GetBytes(Convert.ToString(MainViewModel.GetInstance().User.UserId)));
                                url = dominio + "index3.aspx?" + Fullname + "?" + UserName + "?" + Convert.ToBase64String(Encoding.UTF8.GetBytes("&tag_id")) + "?";
                            }
                            NFCNdefPayload payload = NFCNdefPayload.CreateWellKnownTypePayload(url);
                            NFCNdefMessage nFCNdefMessage = new NFCNdefMessage(new NFCNdefPayload[] { payload });
                            nFCNdefTag.WriteNdef(nFCNdefMessage, delegate
                            {
                                Console.WriteLine("ok");
                            });
                            //Task task = App.DisplayAlertAsync(user_id_tag);
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                    else
                    {
                        Task task2 = App.DisplayAlertAsync("¡Este Tag esta vinculado con otro usuario!");
                        session.Dispose();
                        session.InvalidateSession();
                    }
                    AppDelegate.user_id_tag = "?";
                    if(PopupNavigation.PopupStack.Count != 0)
                    {
                        PopupNavigation.Instance.PopAsync();
                    }
                    App.Current.MainPage.DisplayAlert(
                       " ",
                       Languages.TagReady,
                       Languages.Accept);
                    //session.InvalidateSession();
                    //_tagSession.InvalidateSession();
                    Thread.Sleep(4000);
                    session.InvalidateSession();
                    _tagSession.InvalidateSession();
                    session.Dispose();
                    _tagSession.Dispose();
                }                           
            }
            catch (Exception ex)
            {                
                Console.Write(ex);
            }
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
        public static void CompletionHandler(NSError obj)
        {
            //add code here
        }
        public override void DidInvalidate(NFCNdefReaderSession session, NSError error)
        {                        
            session.InvalidateSession();
            session.Dispose();
        }

        //public static bool modo_escritura = false;

        public void ExecuteCommand()
        {
            try
            {
                modo_escritura = true;
                PopupNavigation.Instance.PushAsync(new ConfigStikerPage());

                //var j = new AppDelegate();
                ScanWriteAsync();
                //j.invoke_lector();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return;
            }
        }
    }
}