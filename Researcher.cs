using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace basicWeb
{
    class Rsr
    {
        private string id;
        private string mstid;
        private string first_name;
        private string last_name;
        private string status;
        private string abbrev;
        private string science;
        private string science_en;
        private string scienceCode;
        private string field;
        private string field_en;
        private string fieldCode;
        private string subfield;
        private string subfield_en;
        private string subfieldCode;
        private string keyws;
        private string keyws_en;
        private string tel;
        private string fax;
        private string email;
        private string url;
        private string n;
        private string orgId1;
        private string orgId2;
        private string orgId3;
        private string orgName1;
        private string orgName2;
        private string orgName3;
        private string x;
        private string y;
        private string vd;

        public Rsr(string _id, string _mstid, string fname, string lname, string _status, string _abbrev, string _science, string _science_en, string _scienceCode, string _field, string _field_en, string _fieldCode, string _subfield, string _subfield_en, string _subfieldCode, string _keyws, string _keyws_en, string _tel, string _fax, string _email, string _url, string _orgId1, string _orgName1, string _orgId2, string _orgName2, string _orgId3, string _orgName3, string _N, string _x, string _y, string _vd)
        {
            this.id = _id;
            this.mstid = _mstid;
            this.first_name = fname;
            this.last_name = lname;
            this.status = _status;
            this.abbrev = _abbrev;
            this.science = _science;
            this.science_en = _science_en;
            this.scienceCode = _scienceCode;
            this.field = _field;
            this.field_en = _field_en;
            this.fieldCode = _fieldCode;
            this.subfield = _subfield;
            this.subfield_en = _subfield_en;
            this.subfieldCode = _subfieldCode;
            this.keyws = _keyws;
            this.keyws_en = _keyws_en;
            this.tel = _tel;
            this.fax = _fax;
            this.email = _email;
            this.url = _url;
            this.orgId1 = _orgId1;
            this.orgName1 = _orgName1;
            this.orgId2 = _orgId2;
            this.orgName2 = _orgName2;
            this.orgId3 = _orgId3;
            this.orgName3 = _orgName3;
            this.n = _N;
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

        public string First_name
        {
            get { return first_name; }
            set { first_name = value; }
        }

        public string Last_name
        {
            get { return last_name; }
            set { last_name = value; }
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
        public string OrgName1
        {
            get { return orgName1; }
            set { orgName1 = value; }
        }
        public string OrgId1
        {
            get { return orgId1; }
            set { orgId1 = value; }
        }
        public string OrgName2
        {
            get { return orgName2; }
            set { orgName2 = value; }
        }
        public string OrgId2
        {
            get { return orgId2; }
            set { orgId2 = value; }
        }
        public string OrgName3
        {
            get { return orgName3; }
            set { orgName3 = value; }
        }
        public string OrgId3
        {
            get { return orgId3; }
            set { orgId3 = value; }
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

        public string Vd
        {
            get { return vd; }
            set { vd = value; }
        }
    }
}
