using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetData
{
    class PrjOrg
    {
        private int prjid;
        private int orgid;

        public PrjOrg(int _prjid, int _orgid)
        {
            this.prjid = _prjid;
            this.orgid = _orgid;

        }

        public int Prjid
        {
            get{ return prjid; }
            set{ prjid = value; }
        }

        public int Orgid
        {
            get{ return orgid; }
            set{ orgid = value; }
        }
    }
}
