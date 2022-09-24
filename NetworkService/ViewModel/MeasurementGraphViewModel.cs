using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.ViewModel
{
    class MeasurementGraphViewModel : BindableBase
    {
        public static ObservableCollection<string> CBsource { get; set; }

        private string selectedID = null;

        private double top1;
        private double top2;
        private double top3;
        private double top4;
        private double top5;

        private string text1;
        private string text2;
        private string text3;
        private string text4;
        private string text5;

        private string time1;
        private string time2;
        private string time3;
        private string time4;
        private string time5;

        public string Text1
        {
            get { return text1; }
            set
            {
                text1 = value;
                OnPropertyChanged("Text1");
            }
        }

        public string Text2
        {
            get { return text2; }
            set
            {
                text2 = value;
                OnPropertyChanged("Text2");
            }
        }

        public string Text3
        {
            get { return text3; }
            set
            {
                text3 = value;
                OnPropertyChanged("Text3");
            }
        }
        public string Text4
        {
            get { return text4; }
            set
            {
                text4 = value;
                OnPropertyChanged("Text4");
            }
        }

        public string Text5
        {
            get { return text5; }
            set
            {
                text5 = value;
                OnPropertyChanged("Text5");
            }
        }

        public string Time1
        {
            get { return time1; }
            set
            {
                time1 = value;
                OnPropertyChanged("Time1");
            }
        }

        public string Time2
        {
            get { return time2; }
            set
            {
                time2 = value;
                OnPropertyChanged("Time2");
            }
        }

        public string Time3
        {
            get { return time3; }
            set
            {
                time3 = value;
                OnPropertyChanged("Time3");
            }
        }
        public string Time4
        {
            get { return time4; }
            set
            {
                time4 = value;
                OnPropertyChanged("Time4");
            }
        }

        public string Time5
        {
            get { return time5; }
            set
            {
                time5 = value;
                OnPropertyChanged("Time5");
            }
        }

        public MyICommand ShowGraph { get; set; }
        public string SelectedID
        {
            get { return selectedID; }
            set
            {
                selectedID = value;
                OnPropertyChanged("SelectedID");
            }
        }

        public void OnShow()
        {
            if (SelectedID != null)
            {
                Top1 = 200;
                Top2 = 200;
                Top3 = 200;
                Top4 = 200;
                Top5 = 200;
                Text1 = "0";
                Text2 = "0";
                Text3 = "0";
                Text4 = "0";
                Text5 = "0";
                Time1 = "";
                Time2 = "";
                Time3 = "";
                Time4 = "";
                Time5 = "";
                string line;
                List<string> vrijednosti = new List<string>();
                List<string> vremena = new List<string>();
                int indeks = Convert.ToInt32(SelectedID.Split('_')[0]);
                using (StreamReader reader = new StreamReader("log.txt"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("OBJECT " + indeks))
                        {
                            string[] s = line.Split(',');
                            vrijednosti.Add(line.Substring(line.IndexOf("STATE") + 7));

                            vremena.Add(s[1]);
                        }
                    }
                }

                switch (vrijednosti.Count())
                {
                    case 0:
                        break;
                    case 1:
                        Top5 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 1]);
                        Text5 = vrijednosti[vrijednosti.Count - 1];
                        Time5 = vremena[vrijednosti.Count - 1];
                        break;
                    case 2:
                        Top5 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 1]);
                        Top4 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 2]);
                        Text5 = vrijednosti[vrijednosti.Count - 1];
                        Text4 = vrijednosti[vrijednosti.Count - 2];
                        Time5 = vremena[vrijednosti.Count - 1];
                        Time4 = vremena[vrijednosti.Count - 2];
                        break;
                    case 3:
                        Top5 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 1]);
                        Top4 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 2]);
                        Top3 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 3]);
                        Text5 = vrijednosti[vrijednosti.Count - 1];
                        Text4 = vrijednosti[vrijednosti.Count - 2];
                        Text3 = vrijednosti[vrijednosti.Count - 3];
                        Time5 = vremena[vrijednosti.Count - 1];
                        Time4 = vremena[vrijednosti.Count - 2];
                        Time3 = vremena[vrijednosti.Count - 3];
                        break;
                    case 4:
                        Top5 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 1]);
                        Top4 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 2]);
                        Top3 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 3]);
                        Top2 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 4]);
                        Text5 = vrijednosti[vrijednosti.Count - 1];
                        Text4 = vrijednosti[vrijednosti.Count - 2];
                        Text3 = vrijednosti[vrijednosti.Count - 3];
                        Text2 = vrijednosti[vrijednosti.Count - 4];
                        Time5 = vremena[vrijednosti.Count - 1];
                        Time4 = vremena[vrijednosti.Count - 2];
                        Time3 = vremena[vrijednosti.Count - 3];
                        Time2 = vremena[vrijednosti.Count - 4];
                        break;
                    default:
                        Top5 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 1]);
                        Top4 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 2]);
                        Top3 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 3]);
                        Top2 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 4]);
                        Top1 = 200 - 2 * Convert.ToDouble(vrijednosti[vrijednosti.Count - 5]);
                        Text5 = vrijednosti[vrijednosti.Count - 1];
                        Text4 = vrijednosti[vrijednosti.Count - 2];
                        Text3 = vrijednosti[vrijednosti.Count - 3];
                        Text2 = vrijednosti[vrijednosti.Count - 4];
                        Text1 = vrijednosti[vrijednosti.Count - 5];
                        Time5 = vremena[vrijednosti.Count - 1];
                        Time4 = vremena[vrijednosti.Count - 2];
                        Time3 = vremena[vrijednosti.Count - 3];
                        Time2 = vremena[vrijednosti.Count - 4];
                        Time1 = vremena[vrijednosti.Count - 5];
                        break;
                }
            }
        }

        public double Top1
        {
            get { return top1; }
            set
            {
                top1 = value;
                OnPropertyChanged("Top1");
            }
        }

        public double Top2
        {
            get { return top2; }
            set
            {
                top2 = value;
                OnPropertyChanged("Top2");
            }
        }

        public double Top3
        {
            get { return top3; }
            set
            {
                top3 = value;
                OnPropertyChanged("Top3");
            }
        }

        public double Top4
        {
            get { return top4; }
            set
            {
                top4 = value;
                OnPropertyChanged("Top4");
            }
        }

        public double Top5
        {
            get { return top5; }
            set
            {
                top5 = value;
                OnPropertyChanged("Top5");
            }
        }

        public MeasurementGraphViewModel()
        {
            CBsource = new ObservableCollection<string>();
            Top1 = 200;
            Top2 = 200;
            Top3 = 200;
            Top4 = 200;
            Top5 = 200;
            Time1 = "";
            Time2 = "";
            Time3 = "";
            Time4 = "";
            Time5 = "";
            ShowGraph = new MyICommand(OnShow);
        }
    }
}
