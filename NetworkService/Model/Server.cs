using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    class Server : BindableBase
    {
        private int id;
        private string name;
        private string ipadress;
        private string img_src;
        private double value;

        public Server()
        {
            img_src = "download.png";
        }

        public Server(int id, string name, string ipadress, string img_src, double value)
        {
            this.id = id;
            this.name = name;
            this.ipadress = ipadress;
            this.img_src = img_src;
            this.value = value;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Ipadress { get => ipadress; set => ipadress = value; }
        public string Img_src { get => img_src; set => img_src = value; }
        public double Value { get => value; set => this.value = value; }
    }
}
