using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetData
{
    class Lecture
    {
        private string url;
        private string title;
        private string desc;
        private string lang;
        private string type;
        private string recorded;
        private string published;
        private string enabled;
        private string ispublic;
        private int views;

        public Lecture(string _url, string _title, string _desc, string _lang, string _type, string _recorded, string _published, string _enabled, string _ispublic, int _views)
        {
            this.url = _url;
            this.title = _title;
            this.desc = _desc;
            this.lang = _lang;
            this.type = _type;
            this.recorded = _recorded;
            this.published = _published;
            this.enabled = _enabled;
            this.ispublic = _ispublic;
            this.views = _views;
        }


        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Description
        {
            get { return desc; }
            set { desc = value; }
        }

        public string Language
        {
            get { return lang; }
            set { lang = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Recorded
        {
            get { return recorded; }
            set { recorded = value; }
        }

        public string Published
        {
            get { return published; }
            set { published = value; }
        }

        public string Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public string Public
        {
            get { return ispublic; }
            set { ispublic = value; }
        }

        public int Views
        {
            get { return views; }
            set { views = value; }
        }


    }
}
