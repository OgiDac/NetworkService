using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }
        private BindableBase currentViewModel;
        private MeasurementGraphViewModel measurementGraphViewModel = new MeasurementGraphViewModel();
        private NetworkDisplayViewModel networkDisplayViewModel = new NetworkDisplayViewModel();
        private NetworkEntitiesViewModel networkEntitiesViewModel = new NetworkEntitiesViewModel();

        private string path = Directory.GetCurrentDirectory() + @"\log.txt";


        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }
        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "graph":
                    CurrentViewModel = measurementGraphViewModel;
                    break;
                case "display":
                    CurrentViewModel = networkDisplayViewModel;
                    break;
                case "entities":
                    CurrentViewModel = networkEntitiesViewModel;
                    break;
            }
        }


        public MainWindowViewModel()
        {
            NavCommand = new MyICommand<string>(OnNav);
            CurrentViewModel = measurementGraphViewModel;
            createListener(); 
        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                     
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        if (incomming.Equals("Need object count"))
                        {
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(NetworkEntitiesViewModel.Serveri.Count().ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            Console.WriteLine(incomming); 

                            string[] dio = incomming.Split('_', ':');
                            NetworkEntitiesViewModel.Serveri[Int32.Parse(dio[1])].Value = Int32.Parse(dio[2]);
                            string ispis = "OBJECT " + dio[1] + ", " + DateTime.Now + ", CHANGED STATE: " + dio[2] + Environment.NewLine;

                            File.AppendAllText(path, ispis);

                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }
    }
}
