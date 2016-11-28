using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetData
{
    class RsrOrg
    {
        private int rsrid;
        private int orgid;

        public RsrOrg(int _rsrid, int _orgid)
        {
            this.rsrid = _rsrid;
            this.orgid = _orgid;

        }

        public int Rsrid
        {
            get{ return rsrid; }
            set{ rsrid = value; }
        }

        public int Orgid
        {
            get{ return orgid; }
            set{ orgid = value; }
        }
    }
}
