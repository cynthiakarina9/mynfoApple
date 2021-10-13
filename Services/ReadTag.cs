namespace mynfo.Services
{
    using CoreNFC;
    using Foundation;
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ReadTag : NSObject, INFCNdefReaderSessionDelegate
    {
        public string ErrorText { get; private set; }

        public static NFCNdefReaderSession _session;
        public static TaskCompletionSource<string> _tcs;

        public async Task<string> ScanAsync()
        {
            try
            {
                if (!NFCNdefReaderSession.ReadingAvailable)
                {
                    throw new InvalidOperationException("Reading NDEF is not available");
                }

                _tcs = new TaskCompletionSource<string>();
                _session = new NFCNdefReaderSession(this, null, true);
                _session.BeginSession();

                return await _tcs.Task;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public void DidInvalidate(NFCNdefReaderSession session, NSError error)
        {
            try
            {
                Console.WriteLine("ServiceToolStandard DidInvalidate: " + error.ToString());
                _tcs.TrySetException(new Exception(error?.LocalizedFailureReason));
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return;
            }
        }

        public void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
        {
            try
            {
                Console.WriteLine("ServiceToolStandard DidDetect msgs " + messages.Length);
                var bytes = messages[0].Records[0].Payload.Skip(3).ToArray();
                var message = Encoding.UTF8.GetString(bytes);
                Console.WriteLine("ServiceToolStandard DidDetect msg " + message);
                _tcs.SetResult(message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return;
            }
        }

        public IntPtr Handle { get; }

        public void Dispose()
        {
            Console.WriteLine("ServiceToolStandard Dispose");
        }
    }
}