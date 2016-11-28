using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetData
{
    class Education
    {
        private int rsrId;
        private string weight;
        private string lvlcode;
        private string uni;
        private string faculty;
        private int year;
        private string countrycode;

        public Education(int _rsrId, string _weight, string _lvlcode, string _uni, string _faculty, int _year, string _countrycode)
        { 
            this.rsrId = _rsrId;
            this.weight = _weight;
            this.lvlcode = _lvlcode;
            this.uni = _uni;
            this.faculty = _faculty;
            this.year = _year;
            this.countrycode = _countrycode;
        }

        public int RsrId 
        {
            get { return rsrId; }
            set { rsrId = value; }
        }
        public string Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        public string Lvlcode
        {
            get { return lvlcode; }
            set { lvlcode = value; }
        }
        public string Uni
        {
            get { return uni; }
            set { uni = value; }
        }
        public string Faculty
        {
            get { return faculty; }
            set { faculty = value; }
        }
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        public string Countrycode
        {
            get { return countrycode; }
            set { countrycode = value; }
        }
    }
}
