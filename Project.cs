using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace basicWeb
{
    class Prj
    {
        private string id;
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
        private string science;
        private string science_en;
        private string scienceCode;
        private string field;
        private string field_en;
        private string fieldCode;
        private string subfield;
        private string subfield_en;
        private string subfieldCode;
        private string x;
        private string y;
        private double vd;

        public Prj(string _id, string _mstid, string _name, string _name_en, int _head, string _startdate, string _enddate, string _keyws, string _keyws_en, string _abst, string _abst_en, string _sign_dom, string _sign_dom_en, string _sign_world, string _sign_world_en, string _science, string _science_en, string _scienceCode, string _field, string _field_en, string _fieldCode, string _subfield, string _subfield_en, string _subfieldCode, string _x, string _y, double _vd)
        {
            this.id = _id;
            this.mstid = _mstid;
            this.name = _name;
            this.name_en = _name_en;
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
            this.science = _science;
            this.science_en = _science_en;
            this.scienceCode = _scienceCode;
            this.field = _field;
            this.field_en = _field_en;
            this.fieldCode = _fieldCode;
            this.subfield = _subfield;
            this.subfield_en = _subfield_en;
            this.subfieldCode = _subfieldCode;
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

        public string Science
        {
            get { return science; }
            set { science = value; }
        }

        public string Science_en
        {
            get { return science_en; }
            set { science_en = value; }
        }

        public string ScienceCode
        {
            get { return scienceCode; }
            set { scienceCode = value; }
        }

        public string Field
        {
            get { return field; }
            set { field = value; }
        }

        public string Field_en
        {
            get { return field_en; }
            set { field_en = value; }
        }

        public string FieldCode
        {
            get { return fieldCode; }
            set { fieldCode = value; }
        }

        public string Subfield
        {
            get { return subfield; }
            set { subfield = value; }
        }

        public string Subfield_en
        {
            get { return subfield_en; }
            set { subfield_en = value; }
        }

        public string SubfieldCode
        {
            get { return subfieldCode; }
            set { subfieldCode = value; }
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
