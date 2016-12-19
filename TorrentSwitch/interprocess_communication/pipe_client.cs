using System.Text;
using TinyIpc.Messaging;

class pipe_client
{
    public static void messaging_client()
    {
        using (var messagebus1 = new TinyMessageBus("ExampleChannel"))

        {
            //messagebus1.MessageReceived +=
            //    (sender, e) => Console.WriteLine(Encoding.UTF8.GetString(e.Message));

            //while (true)
            //{
                string message = @"X:\one.torrent";
                messagebus1.PublishAsync(Encoding.UTF8.GetBytes(message));
            //}
        }
    }
}