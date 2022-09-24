using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    class NetworkEntitiesViewModel : BindableBase
    {
        public static int indeks = 0;
        public static ObservableCollection<Server> Serveri { get; set; }
        public static ObservableCollection<Server> temp { get; set; }

        public static ObservableCollection<Server> temp2 = new ObservableCollection<Server>();
        public static ObservableCollection<string> recent { get; set; }

        private string selectedIt = "";
        private string greska = "";
        public string SelectedIt
        {
            get { return selectedIt; }
            set
            {
                selectedIt = value;
                OnPropertyChanged("SelectedIt");
            }
        }

        public string Greska
        {
            get { return greska; }
            set
            {
                greska = value;
                OnPropertyChanged("Greška!");
            }
        }

        private Server selectedServer;


        private string id;
        private string name;
        private string ipAdress;

        public string Filter = "ip";

        private string search = "";
        public MyICommand AddCommand { get; set; }
        public MyICommand DeleteCommand { get; set; }

        public MyICommand SearchCommand { get; set; }
        public MyICommand RecentCommand { get; set; }
        public MyICommand<string> FilterCommand { get; set; }

        public Server SelectedServer
        {
            get { return selectedServer; }
            set
            {
                selectedServer = value;
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string IpAdress
        {
            get { return ipAdress; }
            set
            {
                ipAdress = value;
                OnPropertyChanged("IpAdress");
            }
        }


        public string Search
        {
            get { return search; }
            set
            {
                search = value;
                OnPropertyChanged("Search");
            }
        }

        private void OnAdd()
        {
            if (!String.IsNullOrWhiteSpace(IpAdress))
            {
                string ime = "Server: " + IpAdress;
                Server s = new Server(indeks, ime, IpAdress, "download.png", 0);
                Serveri.Add(s);
                temp.Add(s);
                NetworkDisplayViewModel.lista.Add(s);
                MeasurementGraphViewModel.CBsource.Add(indeks + "_" + IpAdress);
                Id = "";
                IpAdress = "";
                Name = "";
                indeks++;
                Greska = "";
            }
            else Greska = "Popuni polje!";
        }

        private void OnDelete()
        {
            int idd = SelectedServer.Id;
            Server s = SelectedServer;
            MeasurementGraphViewModel.CBsource.Remove(idd.ToString() + "_" + SelectedServer.Ipadress);
            NetworkDisplayViewModel.lista.Remove(SelectedServer);
            Serveri.Remove(SelectedServer);
            temp.Remove(SelectedServer);
        }

        private bool CanDelete()
        {
            return SelectedServer != null;
        }

        private void OnSearch()
        {
            if (Search.Trim() != "")
            {
                if (!recent.Contains(Filter + ">" + search)) recent.Add(Filter + ">" + search);
                Serveri.Clear();
                foreach (Server s in temp)
                {
                    switch (Filter)
                    {
                        case "ip":
                            if (s.Ipadress == Search) Serveri.Add(s);
                            break;
                        case "name":
                            if (s.Name == Search) Serveri.Add(s);
                            break;
                    }
                }
            }
            else
            {
                Serveri.Clear();
                foreach (Server s in temp) Serveri.Add(s);
            }
        }

        public void OnFilter(string t)
        {
            Filter = t;
        }

        public void OnRecent()
        {
            Search = selectedIt.Split('>')[1];
        }

        public NetworkEntitiesViewModel()
        {
            Serveri = new ObservableCollection<Server>();
            temp = new ObservableCollection<Server>();
            recent = new ObservableCollection<string>();

            AddCommand = new MyICommand(OnAdd);
            DeleteCommand = new MyICommand(OnDelete, CanDelete);
            SearchCommand = new MyICommand(OnSearch);
            FilterCommand = new MyICommand<string>(OnFilter);
            RecentCommand = new MyICommand(OnRecent);
        }
    }
}
