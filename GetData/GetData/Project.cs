using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetData
{
    class Project
    {
        private int id;
        private string mstid;
        private string name;
        private string name_en;
        private int head;
        private string startdate;
        private string enddate;
        private string keyws;
        private string keyws_en;
        private string abst;
        private string abst_en;
        private string sign_dom;
        private string sign_dom_en;
        private string sign_world;
        private string sign_world_en;
        private string mst_rank;
        private string avfte;
        private string uplimit;
        private string type;
        private string mstid_science;

        public Project(int _id, string _mstid, string _name, string _name_en, int _head, string _startdate, string _enddate, string _keyws, string _keyws_en, string _abst, string _abst_en, string _sign_dom, string _sign_dom_en, string _sign_world, string _sign_world_en, string _mst_rank, string _avfte, string _uplimit, string _type, string _mstid_science)
        {
            this.id = _id;
            this.mstid = _mstid;
            this.name = _name;
            this.head = _head;
            this.startdate = _startdate;
            this.enddate = _enddate;
            this.keyws = _keyws;
            this.keyws_en = _keyws_en;
            this.abst = _abst;
            this.abst_en = _abst_en;
            this.sign_dom = _sign_dom;
            this.sign_dom_en = _sign_dom_en;
            this.sign_world = _sign_world;
            this.sign_world_en = _sign_world_en;
            this.mst_rank = _mst_rank;
            this.avfte = _avfte;
            this.uplimit = _uplimit;
            this.type = _type;
            this.mstid_science = _mstid_science;
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

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Name_en
        {
            get { return name_en; }
            set { name_en = value; }
        }

        public int Head
        {
            get { return head; }
            set { head = value; }
        }

        public string Startdate
        {
            get { return startdate; }
            set { startdate = value; }
        }

        public string Enddate
        {
            get { return enddate; }
            set { enddate = value; }
        }
        public string Keywords
        {
            get { return keyws; }
            set { keyws = value; }
        }
        public string Keywords_en
        {
            get { return keyws_en; }
            set { keyws_en = value; }
        }
        public string Abstract
        {
            get { return abst; }
            set { abst = value; }
        }
        public string Abstract_en
        {
            get { return abst_en; }
            set { abst_en = value; }
        }
        public string SignificanceDomestic
        {
            get { return sign_dom; }
            set { sign_dom = value; }
        }
        public string SignificanceDomestic_en
        {
            get { return sign_dom_en; }
            set { sign_dom_en = value; }
        }
        public string SignificanceWorld
        {
            get { return sign_world; }
            set { sign_world = value; }
        }
        public string SignificanceWorld_en
        {
            get { return sign_world_en; }
            set { sign_world_en = value; }
        }
        public string Mst_rank
        {
            get { return mst_rank; }
            set { mst_rank = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Avfte
        {
            get { return avfte; }
            set { avfte = value; }
        }
        public string Uplimit
        {
            get { return uplimit; }
            set { uplimit = value; }
        }
        public string Mstid_science
        {
            get { return mstid_science; }
            set { mstid_science = value; }
        }
    }
}
