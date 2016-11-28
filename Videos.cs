using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace basicWeb
{
    public class Videos
    {
        private string url;
        private string lang;
        private string enabled;
        private string ispublic;
        private string title;
        private string description;
        private string type;
        private string recorded;
        private string published;
        private string views;
        private string role;
        public Videos(string _url, string _lang, string _enabled, string _ispublic, string _title, string _description, string _type, string _recorded, string _published, string _views, string _role)
        {
            this.url = _url;
            this.lang = _lang;
            this.enabled = _enabled;
            this.ispublic = _ispublic;
            this.title = _title;
            this.description = _description;
            this.type = _type;
            this.recorded = _recorded;
            this.published = _published;
            this.views = _views;
            this.role = _role;
        }
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        public string Lang
        {
            get { return lang; }
            set { lang = value; }
        }
        public string Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        public string Ispublic
        {
            get { return ispublic; }
            set { ispublic = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
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
        public string Views
        {
            get { return views; }
            set { views = value; }
        }
        public string Role
        {
            get { return role; }
            set { role = value; }
        }
    }
}