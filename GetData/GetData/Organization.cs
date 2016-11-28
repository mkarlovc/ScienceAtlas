using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetData
{
    class Organization
    {
        private int id;
        private int mstid;
        private string name;
        private int head;
        private string city;

        public Organization(int _id, int _mstid, string _name, int _head, string _city)
        {
            this.id = _id;
            this.mstid = _mstid;
            this.name = _name;
            this.head = _head;
            this.city = _city;
        }

        public int Id
        {
            get{ return id; }
            set{ id = value; }
        }

        public int Mstid
        {
            get{ return mstid; }
            set{ mstid = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Head
        {
            get { return head; }
            set { head = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }
    }
}
