using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetData
{
    class RsrPrj
    {
        private int rsrid;
        private int prjid;

        public RsrPrj(int _rsrid, int _prjid)
        {
            this.rsrid = _rsrid;
            this.prjid = _prjid;

        }

        public int Rsrid
        {
            get{ return rsrid; }
            set{ rsrid = value; }
        }

        public int Prjid
        {
            get{ return prjid; }
            set{ prjid = value; }
        }

    }
}
