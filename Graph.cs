using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace basicWeb
{
    public class Graph
    {
        private string n1;
        private string n2;
        private double weight;

        public Graph(string _n1, string _n2, double _weight)
        {
            this.n1 = _n1;
            this.n2 = _n2;
            this.weight = _weight;
        }

        public string N1
        {
            get { return n1; }
            set { n1 = value; }
        }

        public string N2
        {
            get { return n2; }
            set { n2 = value; }
        }

        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

    }
}