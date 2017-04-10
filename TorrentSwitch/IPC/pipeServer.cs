using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TinyIpc.Messaging;
using TorrentSwitch;
using TorrentSwitch.logic;

class pipeServer
{

    /// <summary>
    /// Starts a inter process communication server.
    /// </summary>
    public static async void StartAsyncServer()
    {
        var makeTask = Task<string>.Factory.StartNew(() => pipeServer.messagingServer0());

        await pipeServer.messagingServer();
        {
            dataGrid.LoadTarget(makeTask.Result);
            StartAsyncServer();
        }
    }

    public static async Task<string> messagingServer() {
        using (var messagebus1 = new TinyMessageBus("ExampleChannel"))

        {
            var taskCompletition = new TaskCompletionSource<string>();

            messagebus1.MessageReceived +=
                (sender, e) =>
                {
                    var ret = Encoding.UTF8.GetString(e.Message);
                    
                    taskCompletition.TrySetResult(ret);
                };
                return await taskCompletition.Task;
        
        }

    }

    public static string  messagingServer0()
    {
        using (var messagebus1 = new TinyMessageBus("ExampleChannel"))

        {
            string ret = null;

            messagebus1.MessageReceived +=
            (sender, e) => ret = Encoding.UTF8.GetString(e.Message);


            while (true)
            {

                if (ret != null)
                {
                    return ret;

                }
            }
        }

    }
}