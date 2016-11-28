using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetData
{
    class AuthorLecture
    {
        private int rsrid;
        private string lectid;
        private string role;

        public AuthorLecture(int _rsrid, string _lectid, string _role)
        {
            this.rsrid = _rsrid;
            this.lectid = _lectid;
            this.role = _role;

        }

        public int Rsrid
        {
            get{ return rsrid; }
            set{ rsrid = value; }
        }

        public string Lectid
        {
            get{ return lectid; }
            set{ lectid = value; }
        }

        public string Role
        {
            get { return role; }
            set { role = value; }
        }
    }
}
