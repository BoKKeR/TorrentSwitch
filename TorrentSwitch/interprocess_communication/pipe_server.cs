using System.Windows;
using TinyIpc.Messaging;

class pipe_server
{
    
    public static void messaging_server() {
        using (var messagebus1 = new TinyMessageBus("ExampleChannel"))

        {
            //(sender, e) => ((MainWindow)Application.Current.MainWindow).DataGridLoadTarget("aaaaa")(Encoding.UTF8.GetString(e.Message));
            //xxx.DataGridLoadTarget("aa");
            //while (1 == 1) { 
            //messagebus1.MessageReceived +=
            //(sender, e) => ((MainWindow)Application.Current.MainWindow).DataGridLoadTarget(Encoding.UTF8.GetString(e.Message));

            //messagebus1.MessageReceived +=
            //(sender, e) => return(Encoding.UTF8.GetString(e.Message));



            //messagebus1.MessageReceived +=
            //(sender, e) => Debug.WriteLine(Encoding.UTF8.GetString(e.Message));


            //msgbox("aaaaaaaa");

            while (true)
                {
                    //var message = Console.ReadLine();


            }
        }
    }

    public static void msgbox(string zzz)
    {
        MessageBox.Show("There is already an instance running.",
        zzz,
        MessageBoxButton.OK, MessageBoxImage.Information);
    }
}