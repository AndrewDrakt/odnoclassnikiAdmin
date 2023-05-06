using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;

namespace odnoclassnikiAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket socket;
        private List<Socket> clients = new List<Socket>();
        public MainWindow()
        {
            InitializeComponent();
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1000);
            ListenClient();
        }
        private async Task ListenClient()
        {
            while (true)
            {
                var cleint = await socket.AcceptAsync();
                clients.Add(cleint);
                RecieveMassege(cleint);
            }

        }


        private void send_Click(object sender)
        {
            /* while (true) {*/
            /*SendMassage(*//*"НЕ СПАММЕР "*//*);
            *//*}*/
        }
        private async Task RecieveMassege(Socket client)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await client.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
               
                msgLB.Items.Add($"Некий {client.RemoteEndPoint} сказал: {message}");
                foreach (var item in clients)
                {
                    SendMassage(item, message);
                }
            }

        }
        private async Task SendMassage(Socket client, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(bytes, SocketFlags.None);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            SerializeJSON();
        }
        private void SerializeJSON()
        {
            string json = JsonConvert.SerializeObject(msgLB.Items);
            File.WriteAllText("C:\\Users\\LEGION\\Desktop\\ivents.json", json);
        }
    }
}
