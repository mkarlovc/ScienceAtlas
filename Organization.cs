using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace basicWeb
{
    class Org
    {
        private string id;
        private string mstid;
        private string name;
        private string orgtype;
        private string head;
        private string city;
        private string n;
        private string x;
        private string y;
        private double vd;

        public Org(string _id, string _mstid, string _name, string _orgtype, string _head, string _city, string _n, string _x, string _y, double _vd)
        {
            this.id = _id;
            this.mstid = _mstid;
            this.name = _name;
            this.head = _head;
            this.city = _city;
            this.orgtype = _orgtype;
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

        public string Mstid
        {
            get { return mstid; }
            set { mstid = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Orgtype
        {
            get { return orgtype; }
            set { orgtype = value; }
        }

        public string Head
        {
            get { return head; }
            set { head = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
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
