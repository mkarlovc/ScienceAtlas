using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetData
{
    class Researcher
    {
        private int id;
        private string mstid;
        private string type;
        private string first_name;
        private string last_name;
        private string status;
        private string abbrev;
        private string keyws;
        private string keyws_en;
        private string tel;
        private string fax;
        private string email;
        private string url;
        private string conorId;

        public Researcher(int _id, string _mstid, string _type, string fname, string lname, string _status, string _abbrev, string _keyws, string _keyws_en, string _tel, string _fax, string _email, string _url, string _conorId)
        {
            this.id = _id;
            this.mstid = _mstid;
            this.type = _type;
            this.first_name = fname;
            this.last_name = lname;
            this.status = _status;
            this.abbrev = _abbrev;
            this.keyws = _keyws;
            this.keyws_en = _keyws_en;
            this.tel = _tel;
            this.fax = _fax;
            this.email = _email;
            this.url = _url;
            this.conorId = _conorId;
        }

        public int Id
        {
            get{ return id; }
            set{ id = value; }
        }

        public string Mstid
        {
            get{ return mstid; }
            set{ mstid = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string First_name
        {
            get{ return first_name; }
            set{ first_name = value; }
        }

        public string Last_name
        {
            get{ return last_name; }
            set{ last_name = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public string Abbrev
        {
            get { return abbrev; }
            set { abbrev = value; }
        }

        public string Keyws
        {
            get { return keyws; }
            set { keyws = value; }
        }
        public string Keyws_en
        {
            get { return keyws_en; }
            set { keyws_en = value; }
        }
        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        public string ConorId
        {
            get { return conorId; }
            set { conorId = value; }
        }
    }
}
