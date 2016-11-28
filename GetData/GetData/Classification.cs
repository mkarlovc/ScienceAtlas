using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetData
{
    class Classification
    {
        private int rsrId;
        private string science;       
        private string field;
        private string subfield;
        private string weight;
        private string type;

        public Classification(int _rsrId, string _science, string _field, string _subfield, string _weight, string _type) {
            this.rsrId = _rsrId;
            this.science = _science;
            this.field = _field;
            this.subfield = _subfield;
            this.weight = _weight;
            this.type = _type;
        }

        public int ResearcherId
        {
            get { return rsrId; }
            set { rsrId = value; }
        }
        public string Science
        {
            get { return science;}
            set { science = value; }
        }
        public string Field
        {
            get { return field; }
            set { field = value; }
        }
        public string Subfield
        {
            get { return subfield; }
            set { subfield = value; }
        }
        public string Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }

   
}
