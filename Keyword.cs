using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace basicWeb
{
    public class Keyword
    {

        private string id;
        private string word;
        private string n;
        private string x;
        private string y;
        private double vd;

        public Keyword(string _id, string _word, string _n, string _x, string _y, double _vd)
        {
            this.id = _id;
            this.word = _word;
            this.n = _n;
            this.x = _x;
            this.y = _y;
            this.vd = _vd;
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Word
        {
            get { return word; }
            set { word = value; }
        }

        public string N
        {
            get { return n; }
            set { n = value; }
        }

        public string X
        {
            get { return x; }
            set { x = value; }
        }

        public string Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Vd
        {
            get { return vd; }
            set { vd = value; }
        }
    }
}
