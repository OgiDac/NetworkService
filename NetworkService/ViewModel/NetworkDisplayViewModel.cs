using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NetworkService.ViewModel
{
    class NetworkDisplayViewModel : BindableBase
    {
        DependencyObject kanvas = null;

        public Canvas glavniKanvas = new Canvas();

        private int flag = 0;
        private bool dragging = false;

        public MyICommand<DependencyObject> ChangedCommand { get; set; }
        public MyICommand<DependencyObject> MouseUp { get; set; }

        public MyICommand<DependencyObject> MouseDownCommand { get; set; }
        public MyICommand<DependencyObject> CanvasUp { get; set; }
        public MyICommand<DependencyObject> AutoPlace { get; set; }
        public MyICommand DropList { get; set; }
        public MyICommand<object> DropCommand { get; set; }
        private Server selectedItem = null;
        private double draggedValue;
        public double uri;
        private Server draggedServer = null;
        private string x1;
        private string x2;
        private string y1;
        private string y2;

        private double[,] cords = new double[12, 12];
        private List<Line> linije = new List<Line>();

        public static ObservableCollection<Line> source { get; set; }

        public static ObservableCollection<Server> lista { get; set; }
        public static ObservableCollection<Server> Kanvasi { get; set; }
        public static ObservableCollection<SolidColorBrush> Okviri { get; set; }
        public static ObservableCollection<string> Natpisi { get; set; }

        public static ObservableCollection<string> Pozadine { get; set; }

        public MyICommand<DependencyObject> ButtonCommand { get; set; }

        private int[,] komsije = new int[12, 12];
        public List<int> tempKomsije = new List<int>();

        private Dictionary<string, Line> skladiste = new Dictionary<string, Line>();

        private string tekstGreska = "";
        public string TekstGreska
        {
            get { return tekstGreska; }
            set
            {
                tekstGreska = value;
                OnPropertyChanged("TekstGreska");
            }
        }

        private string slikaDefault = "green.png";
        private string slikaServer = "download.png";

        public string X1
        {
            get { return x1; }
            set
            {
                x1 = value;
                OnPropertyChanged("X1");
            }
        }
        public string X2
        {
            get { return x2; }
            set
            {
                x2 = value;
                OnPropertyChanged("X2");
            }
        }

        public string Y1
        {
            get { return y1; }
            set
            {
                y1 = value;
                OnPropertyChanged("Y1");
            }
        }

        public string Y2
        {
            get { return y2; }
            set
            {
                y2 = value;
                OnPropertyChanged("Y2");
            }
        }
        public Server SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public void OnChanged(DependencyObject sender)
        {
            if (!dragging)
            {
                kanvas = null;
                dragging = true;
                draggedValue = ((Server)((ListBox)sender).SelectedItem).Value;
                draggedServer = (Server)((ListBox)sender).SelectedItem;
           
                DragDrop.DoDragDrop(sender, draggedServer.Value, DragDropEffects.Move);
            }
        }

        public void OnDropList()
        {
            if (draggedServer != null)
            {
                if (!lista.Contains(draggedServer))
                {

                    lista.Add(draggedServer);
                    SelectedItem = null;
                    draggedServer = null;
                    dragging = false;
                    tempKomsije.Clear();
                }
            }
        }

        public void OnDrop(object sender)
        {

            if (draggedServer != null && Natpisi[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1] == "")
            {
                Pozadine[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1] = slikaServer;
                if (draggedServer.Value > 75 || draggedServer.Value < 45)
                {
                    Okviri[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1] = Brushes.Red;
                }
                else
                {
                    Okviri[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1] = Brushes.LightGray;
                }
                Kanvasi[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1] = draggedServer;
                Kanvasi[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1].Img_src = draggedServer.Img_src;
                Kanvasi[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1].Ipadress = draggedServer.Ipadress;
               
                Natpisi[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1] = draggedServer.Ipadress;
                SelectedItem = null;
                foreach (int i in tempKomsije) NacrtajLinije(i, Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1);
                foreach (int i in tempKomsije) NacrtajLinije(Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1, i);
                tempKomsije.Clear();
                dragging = false;
                lista.Remove(draggedServer);
                draggedServer = null;
            }
            else if (kanvas != null) OnDrop(kanvas);


        }

        public void OnUp(DependencyObject sender)
        {
            draggedServer = null;
            ((ListBox)sender).SelectedItem = null;
            dragging = false;
        }

        public void OnUpCanvas(DependencyObject sender)
        {
            draggedServer = null;
            dragging = false;
        }

        public void OnDown(DependencyObject sender)
        {
            if (!dragging && Natpisi[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1] != "")
            {
                SakupiKomsije(Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1);
                ObrisiLinije(Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1);
                kanvas = sender;
                dragging = true;
                draggedServer = Kanvasi[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1];
            
                Okviri[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1] = Brushes.LightGray;
                Natpisi[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1] = "";
    
                draggedValue = draggedServer.Value;
                ((Canvas)sender).Background = Brushes.LightGray;
                Pozadine[Convert.ToInt32(((TextBlock)((Canvas)sender).Children[1]).Text) - 1] = slikaDefault;
               
                if (DragDrop.DoDragDrop(sender, draggedServer, DragDropEffects.Move) == DragDropEffects.None)
                    OnDrop(kanvas);
            }
        }

        public void OnPovezi(DependencyObject objekt)
        {
            glavniKanvas = (Canvas)objekt;

            if (!String.IsNullOrWhiteSpace(X1) && !String.IsNullOrWhiteSpace(X2) && !String.IsNullOrWhiteSpace(Y1) && !String.IsNullOrWhiteSpace(Y2))
            {
                if (Provjera())
                {
                    if (komsije[(Convert.ToInt32(X1) - 1) * 3 + (Convert.ToInt32(Y1) - 1), (Convert.ToInt32(X2) - 1) * 3 + (Convert.ToInt32(Y2) - 1)] == 0)
                    {
                        TekstGreska = "";
                        Line myLine = new Line();
                        int startPoint = (Convert.ToInt32(X1) - 1) * 3 + (Convert.ToInt32(Y1) - 1);
                        int endPoint = (Convert.ToInt32(X2) - 1) * 3 + (Convert.ToInt32(Y2) - 1);
                        if (Natpisi[startPoint] == "" || Natpisi[endPoint] == "")
                        {
                            TekstGreska = "Nema entiteta!!!";
                            return;
                        }
                        else
                        {
                            myLine.Name = "_" + startPoint.ToString() + "_" + endPoint.ToString();
                            myLine.X1 = (Convert.ToDouble(Y1) - 1) * 100 + 50;
                            myLine.Y1 = (Convert.ToDouble(X1) - 1) * 100 + 50;
                            myLine.X2 = (Convert.ToDouble(Y2) - 1) * 100 + 50;
                            myLine.Y2 = (Convert.ToDouble(X2) - 1) * 100 + 50;
                            myLine.Stroke = Brushes.DarkOrange;
                            myLine.StrokeThickness = 4;

                            glavniKanvas.Children.Add(myLine);
                            
                            komsije[startPoint, endPoint] = 1;
        
                            skladiste.Add(myLine.Name, myLine);
                            TekstGreska = "";
                        }
                    }
                    else
                        TekstGreska = "Linija vec postoji";
                }
                else TekstGreska = "Neispravno unesena polja!!";
            }
            else TekstGreska = "Polja ne mogu biti prazna!!";

        }

        private void ObrisiLinije(int indeks)
        {
            double id = (double)indeks;
            List<Line> zaBrisanje = new List<Line>();
            foreach (Line l in skladiste.Values)
            {
                if (((Line)l).Name.Split('_')[1] == id.ToString())
                {
                    glavniKanvas.Children.Remove(l);
                    zaBrisanje.Add(l);
                    komsije[(int)id, Convert.ToInt32(((Line)l).Name.Split('_')[2])] = 0;
                }
                if (((Line)l).Name.Split('_')[2] == id.ToString())
                {
                    glavniKanvas.Children.Remove(l);
                    zaBrisanje.Add(l);
                    komsije[Convert.ToInt32(((Line)l).Name.Split('_')[1]), (int)id] = 0;
                }
            }
            foreach (Line l in zaBrisanje) skladiste.Remove(l.Name);
        }

        public void NacrtajLinije(double indeks1, double indeks2)
        {
            Line myLine = new Line();
            int ind1 = (int)indeks1;
            int ind2 = (int)indeks2;

            myLine.Name = "_" + ind1.ToString() + "_" + ind2.ToString();
            myLine.X1 = (ind1 % 3) * 100 + 50;
            myLine.Y1 = (ind1 / 3) * 100 + 50;
            myLine.X2 = (ind2 % 3) * 100 + 50;
            myLine.Y2 = (ind2 / 3) * 100 + 50;
            myLine.Stroke = Brushes.DarkOrange;
            myLine.StrokeThickness = 4;
            komsije[ind1, ind2] = 1;
            glavniKanvas.Children.Add(myLine);
            if (skladiste.ContainsKey(myLine.Name))
            {
                skladiste.Remove(myLine.Name);
                skladiste.Add(myLine.Name, myLine);
            }
            else
            {
                skladiste.Add(myLine.Name, myLine);
            }
        }

        private void SakupiKomsije(int indeks)
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (komsije[indeks, j] == 1)
                    {
                        komsije[indeks, j] = 0;
                        if (!tempKomsije.Contains(j))
                            tempKomsije.Add(j);
                    }
                    if (komsije[i, indeks] == 1)
                    {
                        komsije[i, indeks] = 0;
                        if (!tempKomsije.Contains(i))
                            tempKomsije.Add(i);
                    }
                }
            }
        }

        private bool Provjera()
        {
            foreach (char c in X1) if (c > '9' || c < '0') return false;
            foreach (char c in Y1) if (c > '9' || c < '0') return false;
            foreach (char c in X2) if (c > '9' || c < '0') return false;
            foreach (char c in Y2) if (c > '9' || c < '0') return false;
            if (Convert.ToInt32(X1) > 12 || Convert.ToInt32(X2) > 12 || Convert.ToInt32(Y1) > 12 || Convert.ToInt32(Y2) > 12) return false;
            return true;
        }

        public void OnAuto(DependencyObject objekat)
        {
            if (glavniKanvas == null) glavniKanvas = (Canvas)objekat;
            List<Server> lis = new List<Server>();

            foreach (Server s in lista)
            {
                lis.Add(s);
            }
            int indeks;
            foreach (Server s in lis)
            {
                draggedServer = s;
                indeks = ImaSlobodno(glavniKanvas);
                if (indeks != -1)
                    OnDrop(glavniKanvas.Children[indeks]);
            }
        }

        private int ImaSlobodno(DependencyObject kanvas)
        {
            for (int i = 0; i < 12; i++)
            {
                if (Natpisi[i] == "") return i;

            }
            return -1;
        }

        public NetworkDisplayViewModel()
        {
            Kanvasi = new ObservableCollection<Server>();
            Okviri = new ObservableCollection<SolidColorBrush>();
            Natpisi = new ObservableCollection<string>();
            Pozadine = new ObservableCollection<string>();
            source = new ObservableCollection<Line>();
            for (int i = 0; i < 12; i++) Kanvasi.Add(new Server());
            for (int i = 0; i < 12; i++) Okviri.Add(Brushes.LightGray);
            for (int i = 0; i < 12; i++) Natpisi.Add("");
            for (int i = 0; i < 12; i++) Pozadine.Add(slikaDefault);
            glavniKanvas = null;
            flag++;

            draggedServer = new Server();
            lista = new ObservableCollection<Server>();

            ChangedCommand = new MyICommand<DependencyObject>(OnChanged);
            MouseUp = new MyICommand<DependencyObject>(OnUp);
            CanvasUp = new MyICommand<DependencyObject>(OnUpCanvas);
            DropCommand = new MyICommand<object>(OnDrop);
            MouseDownCommand = new MyICommand<DependencyObject>(OnDown);
            DropList = new MyICommand(OnDropList);
            ButtonCommand = new MyICommand<DependencyObject>(OnPovezi);

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    komsije[i, j] = 0;
                }
            }

            AutoPlace = new MyICommand<DependencyObject>(OnAuto);
        }
    }
}