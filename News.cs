using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace basicWeb
{
    public class News
    {
        private double id;
        private string title;
        private string date;
        private string body;
        private string uri;
        private string image;
        private string language;

        public News(double _id, string _date, string _title, string _body, string _url, string _image, string _language)
        {
            this.id = _id;
            this.title = _title;
            this.date = _date;
            this.body = _body;
            this.uri = _url;
            this.image = _image;
            this.language = _language;
        }

        public double Id
        {
            get{ return id; }
            set{ id = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        public string Lang
        {
            get { return language; }
            set { language = value; }
        }
    }
}