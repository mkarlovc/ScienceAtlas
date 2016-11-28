using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json;

namespace GetData
{
    public partial class frmGetData : Form
    {
        AtlasWebService.Service1 atlas = new AtlasWebService.Service1();

        CrisWebService.CrisData cd = new CrisWebService.CrisData();
        CrisWebService.SearchResults sr;

        List<Int32> rsr_ = new List<Int32>();
        List<Int32> prj_ = new List<Int32>();

        List<Researcher> rsr = new List<Researcher>();
        List<Researcher> rsrAtlas = new List<Researcher>();
        List<Researcher> rsrVideo = new List<Researcher>();
        List<Organization> org = new List<Organization>();
        List<Project> prj = new List<Project>();

        List<Classification> classi = new List<Classification>();
        List<Education> edu = new List<Education>();
        List<Lecture> lec = new List<Lecture>();

        List<RsrPrj> rsr_prj = new List<RsrPrj>();
        List<RsrOrg> rsr_org = new List<RsrOrg>();
        List<PrjOrg> prj_org = new List<PrjOrg>();
        List<AuthorLecture> rsr_lec = new List<AuthorLecture>();
            

        public frmGetData()
        {
            InitializeComponent();
        }

        private void researchersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //sr = cd.Retrieve("si", "RSR", "Sicris_app_UI.Researcher.GetEngagementAll.slv.7167", "");
            sr = cd.SearchRetrieve("si", "slv", "RSR", "mstid=%", 1, 50000, "", "");
            parseXML(sr.Records, "RSR","SICRIS");
            MessageBox.Show(sr.Records);
        }

        private void organizationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sr = cd.SearchRetrieve("si", "slv", "ORG", "name=%", 1, 40000, "", "");
            parseXML(sr.Records, "ORG","SICRIS");
            MessageBox.Show(sr.Records);
        }        
        
        private void projectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sr = cd.SearchRetrieve("si", "slv", "PRJ", "name=\"\"", 1, 40000, "", "");
            parseXML(sr.Records, "PRJ","SICRIS");
            MessageBox.Show(sr.Records);
        }

        private void researchersKeywordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            rsr.Clear();

            for (int i = 0; i < rsr_.Count; i++)
            {
                sr = cd.Retrieve("si", "RSR_DATA", "Sicris_app_UI.Researcher.GetKeywords.slv." + rsr_[i], "");
                parseXMLid(sr.Records, "RSR_DATA",rsr_[i]);
                progressBar1.PerformStep();
            }
        }

        private void researchersProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            for (int i = 0; i < rsr.Count; i++)
            {
                sr = cd.Retrieve("si", "RSR-PRJ", "Sicris_app_UI.Researcher.GetProjects.PRJ.slv." + rsr[i].Id, "");
                parseXMLid(sr.Records, "RSR-PRJ", rsr[i].Id);
                progressBar1.PerformStep();
            }
        }

        private void researchersOrganizationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = org.Count;
            progressBar1.Step = 1;

            for (int i = 0; i < org.Count; i++)
            {
                sr = cd.Retrieve("si", "RSR-ORG", "Sicris_app_UI.Organization.GetResearchers.slv." + org[i].Id, "");
                parseXMLid(sr.Records, "RSR-ORG", org[i].Id);
                progressBar1.PerformStep();
            }

        }
        
        private void projectsOrganizationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = org.Count;
            progressBar1.Step = 1;

            for (int i = 0; i < org.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ-ORG", "Sicris_app_UI.Organization.GetProjects.PRJ.slv." + org[i].Id, "");
                parseXMLid(sr.Records, "PRJ-ORG", org[i].Id);
                progressBar1.PerformStep();
            }

        }

        private void parseXML(string xml, string entity, string source)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml); 

            if (entity == "ORG") 
            {
                string name = "", city="";
                int id = -1, mstid = -1, head = -1;

                XmlNodeList rsrl = doc.GetElementsByTagName(entity);

                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = rsrl.Count;
                progressBar1.Step = 1;

                foreach (XmlNode node in rsrl)
                {
                    name = "";mstid = -1; city = "";
                    id = -1; head = 0;

                    id = Convert.ToInt32(node.Attributes["id"].Value);
                    mstid = Convert.ToInt32(node.Attributes["mstid"].Value);

                    XmlNodeList chl = node.ChildNodes;

                    for (int i = 0; i < chl.Count; i++)
                    {
                        if (chl[i].Name == "name")
                        {
                            name = chl[i].InnerText.Replace("'", " "); ;
                        }

                        if (chl[i].Name == "city")
                        {
                            city = chl[i].InnerText.Replace("'", " "); ;
                        }

                        if (chl[i].Name == "RSR")
                        {
                            if (chl[i].Attributes["id"]!=null)
                            head = Convert.ToInt32(chl[i].Attributes["id"].Value);
                        }
                    }

                    org.Add(new Organization(id,mstid,name,head,city));
                    progressBar1.PerformStep();
                }
            }

            if (entity == "RSR") {

                int id = 0;
                string mstid="";
                string tel = "";
                string fax = "";
                string email = "";
                string url = "";
                string status = "";
                string type = "";
                string keyws = "";
                string keyws_en = "";
                string last_name = "";
                string first_name = "";
                string lang = "";

                XmlNodeList rsrl = doc.GetElementsByTagName(entity);

                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = rsrl.Count;
                progressBar1.Step = 1;


                foreach (XmlNode node in rsrl)
                {
                    id = 0;
                    mstid = "";
                    tel = "";
                    fax = "";
                    email = "";
                    url = "";
                    status = "";
                    type = "";
                    keyws = "";
                    keyws_en = "";
                    last_name = "";
                    first_name = "";
                    lang = "";

                    XmlNodeList chl = node.ChildNodes;

                    for (int i = 0; i < node.Attributes.Count; i++)
                    {
                        if (node.Attributes[i].Name == "id") id = Convert.ToInt32(node.Attributes["id"].Value);
                        if (node.Attributes[i].Name == "mstid") mstid = node.Attributes["mstid"].Value;
                        if (node.Attributes[i].Name == "tel1") tel = node.Attributes["tel1"].Value;
                        if (node.Attributes[i].Name == "fax") fax = node.Attributes["fax"].Value;
                        if (node.Attributes[i].Name == "email") email = node.Attributes["email"].Value;
                        if (node.Attributes[i].Name == "url") url = node.Attributes["url"].Value;
                        if (node.Attributes[i].Name == "lname") last_name = node.Attributes["lname"].Value.Replace("'", " ");
                        if (node.Attributes[i].Name == "fname") first_name = node.Attributes["fname"].Value.Replace("'", " ");
                        if (node.Attributes[i].Name == "mstid") mstid = node.Attributes["mstid"].Value;
                        if (node.Attributes[i].Name == "stat") status = node.Attributes["stat"].Value;
                        if (node.Attributes[i].Name == "type") type = node.Attributes["type"].Value;
                        if (node.Attributes[i].Name == "keyws" && lang == "slv") keyws = node.Attributes["keyws"].Value.Replace("'", " ");
                        if (node.Attributes[i].Name == "keyws" && lang == "eng") keyws_en = node.Attributes["keyws"].Value.Replace("'", " ");

                    }

                    rsr.Add(new Researcher(id,mstid,type,first_name,last_name,status,"",keyws,keyws_en,tel,fax,email,url,""));
                }
            }

            if (entity == "PRJ")
            {

                string id = "";
                string mstid = "";


                XmlNodeList rsrl = doc.GetElementsByTagName(entity);

                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = rsrl.Count;
                progressBar1.Step = 1;


                foreach (XmlNode node in rsrl)
                {
                    id = "";
                    mstid = "";

                    XmlNodeList chl = node.ChildNodes;

                    for (int i = 0; i < node.Attributes.Count; i++)
                    {
                        if (node.Attributes[i].Name == "id") id = node.Attributes["id"].Value;
                        if (node.Attributes[i].Name == "mstid") mstid = node.Attributes["mstid"].Value;

                    }

                    prj.Add(new Project(Convert.ToInt32(id), mstid,"","",0,"","","","","","","","","","","","","","",""));
                }
            }
        }

        private void parsePrj(string xml, string entity, string lang, int baseid, Project prj)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            string science = "";
            string field = "";
            string subfield = "";
            string weight = "";
    

            XmlNodeList rsrl = doc.GetElementsByTagName(entity);

            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsrl.Count;
            progressBar1.Step = 1;


            foreach (XmlNode node in rsrl)
            {
                XmlNodeList chl = node.ChildNodes;

                for (int i = 0; i < node.Attributes.Count; i++)
                {

                    if (node.Attributes[i].Name == "prj_title" && lang == "slv") prj.Name = node.Attributes["prj_title"].Value.Replace("'", " ");

                    if (node.Attributes[i].Name == "prj_title" && lang == "eng") prj.Name_en = node.Attributes["prj_title"].Value.Replace("'", " ");

                    if (node.Attributes[i].Name == "keyws" && lang == "slv") prj.Keywords = node.Attributes["keyws"].Value.Replace("'", " ");

                    if (node.Attributes[i].Name == "keyws" && lang == "eng") prj.Keywords_en = node.Attributes["keyws"].Value.Replace("'", " ");

                    if (node.Attributes[i].Name == "abstr" && lang == "slv") prj.Abstract = node.Attributes["abstr"].Value.Replace("'", " ");

                    if (node.Attributes[i].Name == "abstr" && lang == "eng") prj.Abstract_en = node.Attributes["abstr"].Value.Replace("'", " ");

                    if (node.Attributes[i].Name == "mstid") prj.Mstid = node.Attributes["mstid"].Value;

                    if (node.Attributes[i].Name == "rsrid") prj.Head = Convert.ToInt32(node.Attributes["rsrid"].Value);

                    if (node.Attributes[i].Name == "mstrank") prj.Mst_rank = node.Attributes["mstrank"].Value;

                    if (node.Attributes[i].Name == "type") prj.Type = node.Attributes["type"].Value;

                    if (node.Attributes[i].Name == "avfte") prj.Avfte = node.Attributes["avfte"].Value;

                    if (node.Attributes[i].Name == "uplimit") prj.Uplimit = node.Attributes["uplimit"].Value;

                    if (node.Attributes[i].Name == "startdate") prj.Startdate = node.Attributes["startdate"].Value;

                    if (node.Attributes[i].Name == "enddate") prj.Enddate = node.Attributes["startdate"].Value;

                    if (node.Attributes[i].Name == "mstid_science") prj.Mstid_science = node.Attributes["mstid_science"].Value;

                    if (node.Attributes[i].Name == "domestic")
                    {
                        prj.SignificanceDomestic = node.Attributes["domestic"].Value.Replace("'", " ");
                        if (node.Attributes.Count > 1 && node.Attributes[1].Name == "world") prj.SignificanceWorld = node.Attributes["world"].Value.Replace("'", " ");
                    }

                    if (node.Attributes[i].Name == "world" && lang == "slv")
                    {
                        prj.SignificanceWorld = node.Attributes["world"].Value.Replace("'", " ");
                        if (node.Attributes.Count > 1 && node.Attributes[1].Name == "domestic") prj.SignificanceDomestic = node.Attributes["domestic"].Value.Replace("'", " ");
                    }

                    if (node.Attributes[i].Name == "domestic" && lang == "slv")
                    {
                        prj.SignificanceDomestic_en = node.Attributes["domestic"].Value.Replace("'", " ");
                        if (node.Attributes.Count > 1 && node.Attributes[1].Name == "world") prj.SignificanceWorld_en = node.Attributes["world"].Value.Replace("'", " ");
                    }

                    if (node.Attributes[i].Name == "world" && lang == "eng")
                    {
                        prj.SignificanceWorld_en = node.Attributes["world"].Value.Replace("'", " ");
                        if (node.Attributes.Count > 1 && node.Attributes[1].Name == "domestic") prj.SignificanceDomestic_en = node.Attributes["domestic"].Value.Replace("'", " ");
                    }

                    if (node.Attributes[i].Name == "science" || node.Attributes[i].Name == "field" || node.Attributes[i].Name == "subfield")
                    {
                        science = node.Attributes["science"].Value; field = node.Attributes["field"].Value; subfield = node.Attributes["subfield"].Value; weight = node.Attributes["weight"].Value;
                        classi.Add(new Classification(baseid, science, field, subfield, weight, "p"));
                    }
                }             

                progressBar1.PerformStep();
            }
        }

        private void parseRsr(string xml, string entity, string lang, int baseid, Researcher rsr)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            string science = "";
            string field = "";
            string subfield = "";
            string weight = "";

            XmlNodeList rsrl = doc.GetElementsByTagName(entity);

            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsrl.Count;
            progressBar1.Step = 1;


            foreach (XmlNode node in rsrl)
            {
                science = "";
                field = "";
                subfield = "";
                weight = "";

                XmlNodeList chl = node.ChildNodes;

                for (int i = 0; i < node.Attributes.Count; i++)
                {
                    if (node.Attributes[i].Name == "tel1") rsr.Tel = node.Attributes["tel1"].Value;
                    if (node.Attributes[i].Name == "fax") rsr.Fax = node.Attributes["fax"].Value;
                    if (node.Attributes[i].Name == "email") rsr.Email = node.Attributes["email"].Value;
                    if (node.Attributes[i].Name == "url") rsr.Url = node.Attributes["url"].Value;
                    if (node.Attributes[i].Name == "lname") rsr.Last_name = node.Attributes["lname"].Value.Replace("'", " ");
                    if (node.Attributes[i].Name == "fname") rsr.First_name = node.Attributes["fname"].Value.Replace("'", " ");
                    if (node.Attributes[i].Name == "mstid") rsr.Mstid = node.Attributes["mstid"].Value;
                    if (node.Attributes[i].Name == "stat") rsr.Status = node.Attributes["stat"].Value;
                    if (node.Attributes[i].Name == "type") rsr.Type = node.Attributes["type"].Value;
                    if (node.Attributes[i].Name == "keyws" && lang == "slv") rsr.Keyws = node.Attributes["keyws"].Value.Replace("'", " ");
                    if (node.Attributes[i].Name == "keyws" && lang == "eng") rsr.Keyws_en = node.Attributes["keyws"].Value.Replace("'", " ");
                    if (node.Attributes[i].Name == "science" || node.Attributes[0].Name == "field" || node.Attributes[0].Name == "subfield")
                    {
                        science = node.Attributes["science"].Value; field = node.Attributes["field"].Value; subfield = node.Attributes["subfield"].Value; weight = node.Attributes["weight"].Value;
                    }
                }

                if (science != "" || field != "" || subfield != "" || weight != "")
                    classi.Add(new Classification(baseid, science, field, subfield, weight, "r"));

            }
        }

        private void parseXMLid(string xml, string entity, int baseid)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml); 
               
            if (entity == "RSR-PRJ")
            {
                XmlNodeList rsrl = doc.GetElementsByTagName("RSR-PRJ");
                foreach (XmlNode node in rsrl)
                {
                    rsr_prj.Add(new RsrPrj(baseid, Convert.ToInt16(node.Attributes["prjid"].Value)));
                }
            }
            
            if (entity == "RSR-ORG")
            {
                XmlNodeList rsrl = doc.GetElementsByTagName("RSR-ORG");
                foreach (XmlNode node in rsrl)
                {
                    rsr_org.Add(new RsrOrg(Convert.ToInt32(node.Attributes["rsrid"].Value),baseid));
                }
            }

            if (entity == "PRJ-ORG")
            {
                XmlNodeList rsrl = doc.GetElementsByTagName("PRJ-ORG");
                foreach (XmlNode node in rsrl)
                {
                    prj_org.Add(new PrjOrg(Convert.ToInt32(node.Attributes["prjid"].Value), baseid));
                }
            }


            if (entity == "PRJ_DATA")
            {
                string keyws = "";
                string abstr = "";
                string domestic = "";
                string world = "";
                string contr = "";
                string mst_science= "";
                string rank = "";
                string type = "";
                string avfte = "";
                string uplimit = "";
                string science = "";
                string field = "";
                string subfield = "";
                string weight = "";

                XmlNodeList rsrl = doc.GetElementsByTagName(entity);

                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = rsrl.Count;
                progressBar1.Step = 1;


                foreach (XmlNode node in rsrl)
                {
                    keyws = "";
                    abstr = "";
                    domestic = "";
                    world = "";

                    XmlNodeList chl = node.ChildNodes;

                    if (node.Attributes[0].Name == "keyws")
                        keyws = node.Attributes["keyws"].Value;
                    else if (node.Attributes[0].Name == "abstr")
                        abstr = node.Attributes["abstr"].Value.Replace("'", " ");
                    else if (node.Attributes[0].Name == "domestic")
                    {
                        domestic = node.Attributes["domestic"].Value.Replace("'", " ");
                        if (node.Attributes.Count > 1 && node.Attributes[1].Name == "world")
                            world = node.Attributes["world"].Value.Replace("'", " ");
                    }
                    else if (node.Attributes.Count > 1 && node.Attributes[1].Name == "domestic")
                    {
                        domestic = node.Attributes["domestic"].Value.Replace("'", " ");
                        if (node.Attributes[0].Name == "world")
                            world = node.Attributes["world"].Value.Replace("'", " ");
                    }
                    else if (node.Attributes[0].Name == "science" || node.Attributes[0].Name == "field" || node.Attributes[0].Name == "subfield")
                    {
                        science = node.Attributes["science"].Value; field = node.Attributes["field"].Value; subfield = node.Attributes["subfield"].Value; weight = node.Attributes["weight"].Value;
                    }

                    else if (node.Attributes[0].Name == "mst_contr")
                    {
                        contr = node.Attributes["mst_contr"].Value;
                    }
                    else if (node.Attributes[0].Name == "mst_science")
                    {
                        mst_science = node.Attributes["mst_science"].Value;
                    }
                    else if (node.Attributes[0].Name == "mst_rank")
                    {
                        rank = node.Attributes["mst_rank"].Value;
                    }
                    else if (node.Attributes[0].Name == "type")
                    {
                        type = node.Attributes["type"].Value;
                    }
                    else if (node.Attributes[0].Name == "avfte")
                    {
                        avfte = node.Attributes["avfte"].Value;
                    }
                    else if (node.Attributes[0].Name == "uplimit")
                    {
                        uplimit = node.Attributes["uplimit"].Value;
                    }


                    if (keyws!="" || abstr!="" || domestic!="" || world!="")
                        prj.Add(new Project(baseid, "", "", "", 0, "", "", keyws, "", abstr, "", domestic, "", world, "", "", "", "", "",""));

                    if (science != "" || field != "" || subfield != "" || weight != "")
                        classi.Add(new Classification(baseid, science, field, subfield, weight, "p"));

                    if (type != "" || rank != "" || mst_science != "" || contr != "")
                        prj.Add(new Project(baseid, "", "", "", 0, "", "", "", "", "", "", "", "", "", "", rank,avfte, uplimit,type, mst_science));

                    progressBar1.PerformStep();
                }
            }


        }

        private void crawlToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        /*private void crawl(){
        WebRequest request = WebRequest.Create("http://www.dailycoding.com/");
        using (WebResponse response = request.GetResponse())
        {
        using (StreamReader responseReader =
        new StreamReader(response.GetResponseStream()))
        {
        string responseData = responseReader.ReadToEnd();
        using (StreamWriter writer =
        new StreamWriter(@"C:\DailyCoding.html"))
        {
            writer.Write(responseData);
        }
        }
        }
        }*/

        private void researchersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "rsr.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < rsr.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblResearchers VALUES('" + rsr[i].Id + "','" + rsr[i].Mstid + "','" + rsr[i].First_name + "','" + rsr[i].Last_name + "','" + rsr[i].Status + "','" + rsr[i].Abbrev + "');");
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void organizationsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = org.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "org.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < org.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblOrganizations VALUES('" + org[i].Id + "','" + org[i].Mstid + "','" + org[i].Name + "','" + org[i].Head + "','" + org[i].City + "');");
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void projectsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = prj.Count;
            progressBar1.Step = 1;

             SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "prj.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < prj.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblProjects VALUES('" + prj[i].Id + "','" + prj[i].Mstid + "','" + prj[i].Name + "','" + prj[i].Head + "','" + prj[i].Startdate + "','" + prj[i].Enddate + "');");
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void researchersProjectsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr_prj.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "rsr_prj.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < rsr_prj.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblRsrPrj VALUES('" + rsr_prj[i].Rsrid + "','" + rsr_prj[i].Prjid + "');");
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void researchersOrganizationsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr_org.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "rsr_org.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < rsr_org.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblRsrOrg VALUES('" + rsr_org[i].Rsrid + "','" + rsr_org[i].Orgid + "');");
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void projectsOrganizationsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = prj_org.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "prj_org.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < prj_org.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblPrjOrg VALUES('" + prj_org[i].Prjid + "','" + prj_org[i].Orgid + "');");
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "txt";
            sfd.AddExtension = true;
            sfd.RestoreDirectory = true;
            sfd.InitialDirectory = @"C:/";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);

                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = rsr.Count;
                progressBar1.Step = 1;
                for (int i = 0; i < rsr.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblResearchers VALUES('" + rsr[i].Id + "','" + rsr[i].Mstid + "','" + rsr[i].First_name + "','" + rsr[i].Last_name + "','" + rsr[i].Status + "','" + rsr[i].Abbrev + "');");
                    progressBar1.PerformStep();
                }

                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = org.Count;
                progressBar1.Step = 1;
                for (int i = 0; i < org.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblOrganizations VALUES('" + org[i].Id + "','" + org[i].Mstid + "','" + org[i].Name + "','" + org[i].Head + "','" + org[i].City + "');");
                    progressBar1.PerformStep();
                }

                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = prj.Count;
                progressBar1.Step = 1;
                for (int i = 0; i < prj.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblProjects VALUES('" + prj[i].Id + "','" + prj[i].Mstid + "','" + prj[i].Name + "','" + prj[i].Head + "','" + prj[i].Startdate + "','" + prj[i].Enddate + "');");
                    progressBar1.PerformStep();
                }

                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = rsr_org.Count;
                progressBar1.Step = 1;
                for (int i = 0; i < rsr_prj.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblRsrPrj VALUES('" + rsr_prj[i].Rsrid + "','" + rsr_prj[i].Prjid + "');");
                    progressBar1.PerformStep();
                }

                for (int i = 0; i < rsr_org.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblRsrOrg VALUES('" + rsr_org[i].Rsrid + "','" + rsr_org[i].Orgid + "');");
                    progressBar1.PerformStep();
                }

                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = prj_org.Count;
                progressBar1.Step = 1;
                for (int i = 0; i < prj_org.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblPrjOrg VALUES('" + prj_org[i].Prjid + "','" + prj_org[i].Orgid + "');");
                    progressBar1.PerformStep();
                }

                sw.Close();
            }
            
            sfd.Dispose();
            sfd = null;
        }

        private void researchersToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            textBox1.Text = "";

            for (int i = 0; i < rsr.Count; i++)
            {
                textBox1.Text += "INSERT INTO tblResearchers VALUES('" + rsr[i].Id + "','" + rsr[i].Mstid + "','" + rsr[i].First_name + "','" + rsr[i].Last_name + "','" + rsr[i].Status + "','" + rsr[i].Abbrev + "');";
                progressBar1.PerformStep();
            }
        }

        private void oranizationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = org.Count;
            progressBar1.Step = 1;

            textBox1.Text = "";

            for (int i = 0; i < org.Count; i++)
            {
                textBox1.Text += "INSERT INTO tblOrganizations VALUES('" + org[i].Id + "','" + org[i].Mstid + "','" + org[i].Name + "','" + org[i].Head + "','" + org[i].City + "');";
                progressBar1.PerformStep();
            }
        }

        private void projectsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = prj.Count;
            progressBar1.Step = 1;

            textBox1.Text = "";

            for (int i = 0; i < prj.Count; i++)
            {
                textBox1.Text += "INSERT INTO tblProjects VALUES('" + prj[i].Id + "','" + prj[i].Mstid + "','" + prj[i].Name + "','" + prj[i].Head + "','" + prj[i].Startdate + "','" + prj[i].Enddate + "');";
                progressBar1.PerformStep();
            }
        }

        private void iDsOfResearchersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = new StreamReader(ofd.FileName);
                    rsr_.Clear();
                    while (sr.Peek() >= 0) 
                    {
                        rsr_.Add(Convert.ToInt32(sr.ReadLine().Split(',')[0]));
                    }
                }
        }

        private void researchersKeywordsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "rsr_keyws.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < rsr.Count; i++)
                {
                    sw.WriteLine("UPDATE tblResearchers SET keyws='" + rsr[i].Keyws + "' WHERE id= '" + rsr[i].Id + "';");
                    textBox1.Text += "UPDATE tblResearchers SET keyws='" + rsr[i].Keyws + "' WHERE id= '" + rsr[i].Id + "';";
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void iDsOfProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                prj_.Clear();
                while (sr.Peek() >= 0)
                {
                    prj_.Add(Convert.ToInt32(sr.ReadLine().Split(',')[0]));
                }
            }
        }

        private void projectsKeywordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            prj.Clear();

            for (int i = 0; i < prj_.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetKeywords.slv." + prj_[i], "");
                parseXMLid(sr.Records, "PRJ_DATA", prj_[i]);
                progressBar1.PerformStep();
            }
        }

        private void projectsKeywordsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = prj.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "prj_keyws.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < prj.Count; i++)
                {
                    if (prj[i].Keywords != "")
                    {
                        sw.WriteLine("UPDATE tblProjects SET keyws='" + prj[i].Keywords.Replace('\'', ' ') + "' WHERE id= '" + prj[i].Id + "';");
                        textBox1.Text += "UPDATE tblProjects SET keyws='" + prj[i].Keywords + "' WHERE id= '" + prj[i].Id + "';";
                    } progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void projectsAbstractsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            prj.Clear();

            for (int i = 4696; i < prj_.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetAbstract.slv." + prj_[i], "");
                parseXMLid(sr.Records, "PRJ_DATA", prj_[i]);
                progressBar1.PerformStep();
            }
        }

        private void projectsAbstractsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = prj.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "prj_abstr.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < prj.Count; i++)//1738
                {
                    if (prj[i].Abstract!="")
                    {
                    sw.WriteLine("UPDATE tblProjects SET abstract='" + prj[i].Abstract.Replace('\'',' ') + "' WHERE id= '" + prj[i].Id + "';");
                    textBox1.Text += "UPDATE tblProjects SET abstract='" + prj[i].Abstract + "' WHERE id= '" + prj[i].Id + "';";
                    }progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void projectSignificanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = prj.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "prj_sign.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < prj.Count; i++)
                {
                    sw.WriteLine("UPDATE tblProjects SET sign_dom='" + prj[i].SignificanceDomestic.Replace('\'',' ') + "', sign_world= '" + prj[i].SignificanceWorld.Replace('\'',' ') + "' WHERE id= '" + prj[i].Id + "';");
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void projectsSignificanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            prj.Clear();

            for (int i = 0; i < prj_.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetSignificance.slv." + prj_[i], "");
                parseXMLid(sr.Records, "PRJ_DATA", prj_[i]);
                progressBar1.PerformStep();
            }
        }

        private void researchersClassificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            rsr.Clear();

            for (int i = 0; i < rsr_.Count; i++)
            {
                sr = cd.Retrieve("si", "RSR_DATA", "Sicris_app_UI.Researcher.GetMSTClassification.slv." + rsr_[i], "");
                parseXMLid(sr.Records, "RSR_DATA", rsr_[i]);
                progressBar1.PerformStep();
            }
        }

        private void researchersClassificationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = prj.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "rsr_classi.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < classi.Count; i++)
                {
                    sw.WriteLine("INSERT into tblRsrClassification values ( '" + classi[i].ResearcherId + "','" + classi[i].Weight + "','" + classi[i].Science + "','" + classi[i].Field + "','" + classi[i].Subfield + "');");

                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void researchersEducationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            rsr.Clear();

            for (int i = 0; i < rsr_.Count; i++)
            {
                sr = cd.Retrieve("si", "RSR_DATA", "Sicris_app_UI.Researcher.GetEducation.slv." + rsr_[i]+".%", "");
                parseXMLid(sr.Records, "RSR_DATA", rsr_[i]);
                progressBar1.PerformStep();
            }
        }

        private void researchersEducationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = prj.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "rsr_edu.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < edu.Count; i++)
                {
                    sw.WriteLine("INSERT into tblRsrEducation values ( '" + edu[i].RsrId + "','" + edu[i].Weight + "','" + edu[i].Lvlcode + "','" + edu[i].Uni + "','" + edu[i].Faculty + "','" + edu[i].Year + "','" + edu[i].Countrycode + "');");

                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void researcherContactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            rsr.Clear();

            for (int i = 2500; i < rsr_.Count; i++)
            {
                sr = cd.Retrieve("si", "RSR_DATA", "Sicris_app_UI.Researcher.GetContacts.slv." + rsr_[i], "");
                parseXMLid(sr.Records, "RSR_DATA", rsr_[i]);
                progressBar1.PerformStep();
            }
        }

        private void researchersContactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "rsr_contact.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 6861; i < rsr.Count; i++)
                {
                    sw.WriteLine("UPDATE tblResearchers SET tell='" + rsr[i].Tel + "',fax='" + rsr[i].Fax + "',email='" + rsr[i].Email + "',url='" + rsr[i].Url + "' WHERE id= '" + rsr[i].Id + "';");
                    textBox1.Text += "UPDATE tblResearchers SET tell='" + rsr[i].Tel + "',fax='" + rsr[i].Fax + "',email='" + rsr[i].Email + "',url='" + rsr[i].Url + "' WHERE id= '" + rsr[i].Id + "';";
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void researchersAtlasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlDocument xml = new XmlDocument();
            XmlNode xml1;

            xml1 = atlas.AllRsr();
            //string a = Convert.ToString(xml1.OuterXml.ToString());
            parseXML(xml1.OuterXml.ToString(), "RSR", "Atlas");
        }

        private void researchersVideoLecturesNETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
                {

                    string name = "", firstName = "", lastName = "", email = "", url = "";
                    StreamReader sr = new StreamReader(ofd.FileName);
                    string file = sr.ReadToEnd();
                    JObject o = JObject.Parse(file);

                    for (int i = 0; i < o.Count; i++) {

                        try
                        {
                            JObject o1 = (JObject)o["A" + (i + 1).ToString()];

                            name = (string)o1["name"];
                            firstName = name.Split(' ')[0];
                            lastName = name.Split(' ')[1];

                            email = (string)o1["email"];

                            try
                            {
                                JObject refs = (JObject)o1["refs"];
                                url = (string)refs["homepage"];
                            }
                            catch { 
                        
                            }
                            rsrVideo.Add(new Researcher(-1,"","",firstName,lastName," "," "," "," "," "," ",email,url," "));
                        }
                        catch {
                            //string alert = "error";
                        }

                        

                        /*for (int j = 0; j < rsrAtlas.Count; j++)
                        {
                            if (rsrAtlas[j].First_name == firstName && rsrAtlas[j].Last_name == lastName)

                        }*/
                    }


                    }
            
        }

        private void lecturesVideoLecturesNETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                string id = "", title = "", description = "", type = "", lang = "", url = "", recorded="", published="", enabled=" ", ispublic=" ";
                int views;

                StreamReader sr = new StreamReader(ofd.FileName);
                string file = sr.ReadToEnd();
                JObject o = JObject.Parse(file);

                for (int i = 0; i < o.Count; i++)
                {
                    try
                    {
                        JObject o1 = (JObject)o["L" + (i + 1).ToString()];

                        id =  "L" + (i + 1).ToString();

                        try { lang = (string)o1["lang"]; }
                        catch{}

                        try { url = (string)o1["url"]; }
                        catch { }

                        try { views = (int)o1["views"]; }
                        catch { }

                        try { enabled = (string)o1["enabled"]; }
                        catch { }

                        try { ispublic = (string)o1["public"]; }
                        catch { }

                        try { recorded = (string)o1["recorded"]; }
                        catch { }

                        try { published = (string)o1["published"]; }
                        catch { }


                        try
                        {
                            JObject text = (JObject)o1["text"];
                            try{title = (string)text["title"];}
                            catch{}
                            try{description = (string)text["desc"];}
                            catch{}
                        }
                        catch
                        {

                        }
                        //rsrVideo.Add(new Researcher(-1, -1, firstName, lastName, " ", " ", " ", " ", " ", " ", " ", " ", email, url));
                    }
                    catch
                    {
                        //string alert = "error";
                    }

                }


            }
        }

        private void authorsLecturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            JObject oAuthors; JObject oLectures; JObject oEdges;
            string file1="", file2="", file3="";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                 file1 = sr.ReadToEnd();
                
            }// END ofd

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                 file2 = sr.ReadToEnd();
                
            }// END ofd

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                 file3 = sr.ReadToEnd();
                
            }// END ofd

            oAuthors = JObject.Parse(file1);
            oLectures = JObject.Parse(file2);
            oEdges = JObject.Parse(file3);

            System.Collections.Generic.IEnumerable<JProperty> edges_list = oEdges.Properties();
            for (int i = 0; i < oEdges.Count; i++)
            {
                JObject edge = (JObject)oEdges[edges_list.ElementAt<JProperty>(i).Name.ToString()];
                System.Collections.Generic.IEnumerable<JProperty> edge_properties = edge.Properties();
                for(int j=0; j<edge.Count; j++)
                {
                    if((string)edge_properties.ElementAt<JProperty>(j).Name=="authors")
                    {
                        JObject authors = (JObject)edge["authors"];
                        System.Collections.Generic.IEnumerable<JProperty> authors_properties = authors.Properties();
                        
                        for (int k=0; k<authors.Count; k++)
                        {
                            JObject author = (JObject)oAuthors[authors_properties.ElementAt<JProperty>(k).Name.ToString()];
                            System.Collections.Generic.IEnumerable<JProperty> author_properties = author.Properties();
                            JObject lecture = (JObject)oLectures[edges_list.ElementAt<JProperty>(i).Name.ToString()];
                            IEnumerable<JProperty> lecture_properties = lecture.Properties();

                            string url="", lang="", type="", enabled="", ispublic="", recorded="", published="",title="", description="";
                            int views=0;

                            for (int l = 0; l < lecture.Count; l++)
                            {
                                if ((string)lecture_properties.ElementAt<JProperty>(l).Name == "url")
                                    url = (string)lecture_properties.ElementAt<JProperty>(l).Value;

                                if ((string)lecture_properties.ElementAt<JProperty>(l).Name == "lang")
                                    lang = (string)lecture_properties.ElementAt<JProperty>(l).Value;

                                if ((string)lecture_properties.ElementAt<JProperty>(l).Name == "type")
                                    type = (string)lecture_properties.ElementAt<JProperty>(l).Value;

                                if ((string)lecture_properties.ElementAt<JProperty>(l).Name == "views")
                                    views = (int)lecture_properties.ElementAt<JProperty>(l).Value;

                                if ((string)lecture_properties.ElementAt<JProperty>(l).Name == "enabled")
                                    enabled = (lecture_properties.ElementAt<JProperty>(l).Value).ToString();

                                if ((string)lecture_properties.ElementAt<JProperty>(l).Name == "public")
                                    ispublic = (string)lecture_properties.ElementAt<JProperty>(l).Value.ToString();

                                if ((string)lecture_properties.ElementAt<JProperty>(l).Name == "recorded")
                                    recorded = (string)lecture_properties.ElementAt<JProperty>(l).Value;

                                if ((string)lecture_properties.ElementAt<JProperty>(l).Name == "published")
                                    published = (string)lecture_properties.ElementAt<JProperty>(l).Value;

                                if ((string)lecture_properties.ElementAt<JProperty>(l).Name == "text") 
                                {

                                    JObject text = (JObject)lecture["text"];
                                    IEnumerable<JProperty> text_properties = text.Properties();

                                    for (int m = 0; m < text.Count; m++)
                                    {
                                        if ((string)text_properties.ElementAt<JProperty>(m).Name == "title")
                                            title = (string)text_properties.ElementAt<JProperty>(m).Value;
                                        if ((string)text_properties.ElementAt<JProperty>(m).Name == "desc")
                                            description = ((string)text_properties.ElementAt<JProperty>(m).Value);
                                    }

                                }
                            }

                            lec.Add(new Lecture(url, title, description, lang, type, recorded, published, enabled, ispublic, views));
                            
                            string name = "", firstName = "", lastName = "", urlA = "";
                            
                            for (int l = 0; l < author.Count; l++) {
                                if ((string)author_properties.ElementAt<JProperty>(l).Name == "name")
                                {
                                    name = (string)author_properties.ElementAt<JProperty>(l).Value;
                                    if (name.Contains(" "))
                                    {
                                        firstName = name.Split(' ')[0];
                                        lastName = name.Split(' ')[1];
                                    }
                                }

                                if ((string)author_properties.ElementAt<JProperty>(l).Name == "url")
                                    urlA = (string)authors_properties.ElementAt<JProperty>(l).Value;
                            }
                            
                            int id = 0;
                            id = findAuthor(firstName, lastName, urlA);
                            
                            if (id > 0)
                            {
                                //JObject video = (JObject)oLectures[(edge_properties.ElementAt<JProperty>(i).Name.ToString())];
                                rsr_lec.Add(new AuthorLecture(id, url, authors_properties.ElementAt<JProperty>(k).Value.ToString()));
                            }
                        }
                    }
                }
            }
        }
        
        private int findAuthor(string firstName, string lastName, string email) {
            for (int i = 0; i < rsrAtlas.Count; i++) {
                if (rsrAtlas[i].Email == email)
                {
                    return rsrAtlas[i].Id;
                }
                else if (rsrAtlas[i].First_name == firstName && rsrAtlas[i].Last_name == lastName)
                {
                    return rsrAtlas[i].Id;
                }
            }
            return 0;
        }

        private void lecturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = lec.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "lec.txt";
            char apostrophe = '\u0027';
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < lec.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblLectures VALUES('" + lec[i].Url + "','" + lec[i].Title.Replace(apostrophe, ' ') + "','" + lec[i].Description.Replace(apostrophe, ' ').Replace("\n", " ") + "','" + lec[i].Language + "','" + lec[i].Type + "','" + lec[i].Recorded + "','" + lec[i].Published + "','" + lec[i].Enabled + "','" + lec[i].Public + "','" + lec[i].Views + "');");
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void researchersLecturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr_lec.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "rsr_lec.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < rsr_lec.Count; i++)
                {
                    sw.WriteLine("INSERT INTO tblRsrHasLec VALUES('" + rsr_lec[i].Rsrid + "','" + rsr_lec[i].Lectid + "','" + rsr_lec[i].Role + "');");
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void researchersEnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //sr = cd.Retrieve("si", "RSR", "Sicris_app_UI.Researcher.GetEngagementAll.slv.7167", "");
            //sr = cd.SearchRetrieve("en", "eng", "RSR", "mstid=%", 1, 40000, "", "");
            sr = cd.Retrieve("si","RSR","Sicris_app_UI.Researcher.SearchSimple.slv.public.utf-8.mstid.%.1.-1","");
            parseXML(sr.Records, "RSR", "SICRIS");
            MessageBox.Show(sr.Records);
        }

        private void researchersKeywordsEnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            rsr.Clear();

            for (int i = 0; i < rsr_.Count; i++)
            {
                sr = cd.Retrieve("si", "RSR_DATA", "Sicris_app_UI.Researcher.GetKeywords.eng." + rsr_[i], "");
                parseXMLid(sr.Records, "RSR_DATA", rsr_[i]);
                progressBar1.PerformStep();
            }
        }

        private void researchersKeywordsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "rsr_keyws_en.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < rsr.Count; i++)
                {
                    sw.WriteLine("UPDATE tblResearchers SET keyws_en='" + rsr[i].Keyws_en + "' WHERE id= '" + rsr[i].Id + "';");
                    textBox1.Text += "UPDATE tblResearchers SET keyws_en='" + rsr[i].Keyws_en + "' WHERE id= '" + rsr[i].Id + "';";
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void researchersIdsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "rsr_ids.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < rsr.Count; i++)
                {
                    sw.WriteLine(rsr[i].Id);
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void projectsAbstractsEnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            prj.Clear();

            for (int i = 4696; i < prj_.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetAbstract.eng." + prj_[i], "");
                parseXMLid(sr.Records, "PRJ_DATA", prj_[i]);
                progressBar1.PerformStep();
            }
        }

        private void projectsEnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sr = cd.Retrieve("si", "PRJ", "Sicris_app_UI.Project.SearchSimple.PRJ.eng.public.utf-8.mstid.%.1.-1", "");
            parseXML(sr.Records, "PRJ", "SICRIS");
            MessageBox.Show(sr.Records);
        }

        private void projectsIdsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "prj_ids.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < prj.Count; i++)
                {
                    sw.WriteLine(prj[i].Id);
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void projectsEnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = prj.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "prj.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < prj.Count; i++)
                {
                    sw.WriteLine("UPDATE tblProjects SET name_en = '" + prj[i].Name.Replace('\'',' ') + "' WHERE id = '" + prj[i].Id + "';");
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

        private void projectsKeywordsEnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            prj.Clear();

            for (int i = 0; i < prj_.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetKeywords.eng." + prj_[i], "");
                parseXMLid(sr.Records, "PRJ_DATA", prj_[i]);
                progressBar1.PerformStep();
            }
        }

        private void projectsSignificanceEnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            prj.Clear();

            for (int i = 0; i < prj_.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetSignificance.eng." + prj_[i], "");
                parseXMLid(sr.Records, "PRJ_DATA", prj_[i]);
                progressBar1.PerformStep();
            }
        }

        private void projectStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            prj.Clear();

            for (int i = 0; i < prj_.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetHeader.slv." + prj_[i], "");
                parseXMLid(sr.Records, "PRJ_DATA", prj_[i]);
                progressBar1.PerformStep();
            }
        }

        private void pprojectsClassificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            prj.Clear();

            for (int i = 0; i < prj_.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetMSTClassification.slv." + prj_[i], "");
                parseXMLid(sr.Records, "PRJ_DATA", prj_[i]);
                progressBar1.PerformStep();
            }
        }

        private void proectClassificationEnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            prj.Clear();

            for (int i = 0; i < prj_.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetMSTClassification.eng." + prj_[i], "");
                parseXMLid(sr.Records, "PRJ_DATA", prj_[i]);
                progressBar1.PerformStep();
            }
        }

        private void projectsStatusEnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = rsr.Count;
            progressBar1.Step = 1;

            prj.Clear();

            for (int i = 0; i < prj_.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetHeader.eng." + prj_[i], "");
                parseXMLid(sr.Records, "PRJ_DATA", prj_[i]);
                progressBar1.PerformStep();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            

        }

        private void getPrj() {
            sr = cd.Retrieve("si", "PRJ", "Sicris_app_UI.Project.SearchSimple.PRJ.slv.public.utf-8.mstid.%.1.-1", "");
            parseXML(sr.Records, "PRJ", "SICRIS");

            for (int i = 0; i < prj.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetHeader.slv." + prj[i].Id, "");
                parsePrj(sr.Records, "PRJ_DATA", "slv", prj[i].Id, prj[i]);

                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetHeader.eng." + prj[i].Id, "");
                parsePrj(sr.Records, "PRJ_DATA", "eng", prj[i].Id, prj[i]);

                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetMSTClassification.slv." + prj[i].Id, "");
                parsePrj(sr.Records, "PRJ_DATA", "slv", prj[i].Id, prj[i]);

                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetAbstract.slv." + prj[i].Id, "");
                parsePrj(sr.Records, "PRJ_DATA", "slv", prj[i].Id, prj[i]);

                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetAbstract.eng." + prj[i].Id, "");
                parsePrj(sr.Records, "PRJ_DATA", "eng", prj[i].Id, prj[i]);

                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetKeywords.slv." + prj[i].Id, "");
                parsePrj(sr.Records, "PRJ_DATA", "slv", prj[i].Id, prj[i]);

                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetKeywords.eng." + prj[i].Id, "");
                parsePrj(sr.Records, "PRJ_DATA", "eng", prj[i].Id, prj[i]);

                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetSignificance.slv." + prj[i].Id, "");
                parsePrj(sr.Records, "PRJ_DATA", "slv", prj[i].Id, prj[i]);

                sr = cd.Retrieve("si", "PRJ_DATA", "Sicris_app_UI.Project.GetSignificance.eng." + prj[i].Id, "");
                parsePrj(sr.Records, "PRJ_DATA", "eng", prj[i].Id, prj[i]);
            }
        }

        private void getRsr() {
            //sr = cd.Retrieve("si", "RSR", "Sicris_app_UI.Researcher.SearchSimple.slv.public.utf-8.mstid.%.1.-1", "");
            //parseXML(sr.Records, "RSR", "SICRIS");

            for (int i = 0; i < rsr.Count; i++)
            {
                sr = cd.Retrieve("si", "RSR_DATA", "Sicris_app_UI.Researcher.GetBasic.slv." + rsr[i].Id, "");
                parseRsr(sr.Records, "RSR_DATA", "slv", rsr[i].Id, rsr[i]);

                sr = cd.Retrieve("si", "RSR_DATA", "Sicris_app_UI.Researcher.GetKeywords.slv." + rsr[i].Id, "");
                parseRsr(sr.Records, "RSR_DATA","slv", rsr[i].Id, rsr[i]);

                sr = cd.Retrieve("si", "RSR_DATA", "Sicris_app_UI.Researcher.GetKeywords.eng." + rsr[i].Id, "");
                parseRsr(sr.Records, "RSR_DATA", "eng", rsr[i].Id, rsr[i]);

                sr = cd.Retrieve("si", "RSR_DATA", "Sicris_app_UI.Researcher.GetContacts.slv." + rsr[i].Id, "");
                parseRsr(sr.Records, "RSR_DATA", "slv", rsr[i].Id, rsr[i]);

                sr = cd.Retrieve("si", "RSR_DATA", "Sicris_app_UI.Researcher.GetMSTClassification.slv." + rsr[i].Id, "");
                parseRsr(sr.Records, "RSR_DATA","slv", rsr[i].Id, rsr[i]);
            }
        }

        private void getOrg() {
            sr = cd.SearchRetrieve("si", "slv", "ORG", "name=%", 1, 40000, "", "");
            parseXML(sr.Records, "ORG", "SICRIS");
        }

        private void getGrp() {
            sr = cd.SearchRetrieve("si", "slv", "RSR", "mstid=%", 1, 50000, "", "");
            parseXML(sr.Records, "RSR", "SICRIS");
        }

        private void getRsrPrj() {
            for (int i = 0; i < rsr.Count; i++)
            {
                sr = cd.Retrieve("si", "RSR-PRJ", "Sicris_app_UI.Researcher.GetProjects.PRJ.slv." + rsr[i].Id, "");
                parseXMLid(sr.Records, "RSR-PRJ", rsr[i].Id);
                progressBar1.PerformStep();
            }
        }

        private void getRsrOrg() {
            for (int i = 0; i < org.Count; i++)
            {
                sr = cd.Retrieve("si", "RSR-ORG", "Sicris_app_UI.Organization.GetResearchers.slv." + org[i].Id, "");
                parseXMLid(sr.Records, "RSR-ORG", org[i].Id);
                progressBar1.PerformStep();
            }
        }

        private void getPrjOrg() {
            for (int i = 0; i < org.Count; i++)
            {
                sr = cd.Retrieve("si", "PRJ-ORG", "Sicris_app_UI.Organization.GetProjects.PRJ.slv." + org[i].Id, "");
                parseXMLid(sr.Records, "PRJ-ORG", org[i].Id);
                progressBar1.PerformStep();
            }
        }

        private void writeRsr() {

            StreamWriter sw = new StreamWriter("rsr.sql");
            for (int i = 0; i < rsr.Count; i++)
            {
                sw.WriteLine("INSERT INTO tblResearchers VALUES('" + rsr[i].Id + "','" + rsr[i].Mstid + "','" + rsr[i].First_name + "','" + rsr[i].Last_name + "','" + rsr[i].Status + "','" + rsr[i].Abbrev + "','" + rsr[i].Keyws + "','" + rsr[i].Keyws_en + "','" + rsr[i].Type + "','" + rsr[i].Tel + "','" + rsr[i].Fax + "','" + rsr[i].Email + "','" + rsr[i].Url + "','" + rsr[i].ConorId + "');");
                progressBar1.PerformStep();
            }
            sw.Close();
        }

        private void writePrj()
        {

            StreamWriter sw = new StreamWriter("prj.sql");
            for (int i = 0; i < prj.Count; i++)
            {
                sw.WriteLine("INSERT INTO tblProjects VALUES('" + prj[i].Id + "','" + prj[i].Mstid + "','" + prj[i].Name + "','" + prj[i].Name_en + "','" + prj[i].Head + "','" + prj[i].Startdate + "','" + prj[i].Enddate + "','" + prj[i].Keywords + "','" + prj[i].Keywords_en + "','" + prj[i].Abstract + "','" + prj[i].Abstract_en + "','" + prj[i].SignificanceDomestic + "','" + prj[i].SignificanceDomestic_en + "','" + prj[i].SignificanceWorld + "','" + prj[i].SignificanceWorld_en + "','" + prj[i].Mst_rank + "','" + prj[i].Avfte + "','" + prj[i].Uplimit + "','" + prj[i].Type + "','" + prj[i].Mstid_science + "');");
                progressBar1.PerformStep();
            }
            sw.Close();

        }

        private void writeOrg()
        {
            StreamWriter sw = new StreamWriter("org.sql");
            for (int i = 0; i < org.Count; i++)
            {
                sw.WriteLine("INSERT INTO tblOrganizations VALUES('" + org[i].Id + "','" + org[i].Mstid + "','" + org[i].Name + "','" + org[i].Head + "','" + org[i].City + "');");
                progressBar1.PerformStep();
            }
            sw.Close();
        }
        private void writeRsrPrj()
        {
            StreamWriter sw = new StreamWriter("rsr_prj.sql");
            for (int i = 0; i < rsr_prj.Count; i++)
            {
                sw.WriteLine("INSERT INTO tblRsrHasPrj VALUES('" + rsr_prj[i].Rsrid + "','" + rsr_prj[i].Prjid + "');");
                progressBar1.PerformStep();
            }
            sw.Close();
        }
        private void writeRsrOrg()
        {
            StreamWriter sw = new StreamWriter("rsr_org.sql");
            for (int i = 0; i < rsr_org.Count; i++)
            {
                sw.WriteLine("INSERT INTO tblRsrIsinOrg VALUES('" + rsr_org[i].Rsrid + "','" + rsr_org[i].Orgid + "');");
                progressBar1.PerformStep();
            }
            sw.Close();
        }
        private void writePrjOrg()
        {
            StreamWriter sw = new StreamWriter("prj_org.sql");
            for (int i = 0; i < prj_org.Count; i++)
            {
                sw.WriteLine("INSERT INTO tblPrjOfOrg VALUES('" + prj_org[i].Prjid + "','" + prj_org[i].Orgid + "');");
                progressBar1.PerformStep();
            }
            sw.Close();
        }

        private void writeClassi()
        {

            StreamWriter sw = new StreamWriter("rsr_classi.sql");
            for (int i = 0; i < classi.Count; i++)
            {
                if (classi[i].Type == "r")
                    sw.WriteLine("INSERT into tblRsrClassification values ( '" + classi[i].ResearcherId + "','" + classi[i].Weight + "','" + classi[i].Science + "','" + classi[i].Field + "','" + classi[i].Subfield + "');");
                else
                    sw.WriteLine("INSERT into tblPrjClassification values ( '" + classi[i].ResearcherId + "','" + classi[i].Weight + "','" + classi[i].Science + "','" + classi[i].Field + "','" + classi[i].Subfield + "');");
                progressBar1.PerformStep();
            }
            sw.Close();
        }

        private void doToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string start = DateTime.Now.ToString();

            //getRsr();
            //getPrj();
            //getOrg();
            //getGrp();
            //getRsrPrj();
            //getRsrOrg();
            //getPrjOrg();
            //writeRsr();
            writePrj();
            //writeOrg();
            //writeRsrPrj();
            //writeRsrOrg();
            //writePrjOrg();
            //writeClassi();

            string end = DateTime.Now.ToString();
        }

        private void idToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                while (sr.Peek() >= 0)
                {
                    rsr.Add(new Researcher(Convert.ToInt32(sr.ReadLine().Split(',')[0]),"","","","","","","","","","","","",""));
                }
            }
        }

        private void idprjToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);

                while (sr.Peek() >= 0)
                {
                    prj.Add(new Project(Convert.ToInt32(sr.ReadLine().Split(',')[0]),"","","",0,"","","","","","","","","","","","","","",""));
                }
            }
        }

        private void idorgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(ofd.FileName);
                while (sr.Peek() >= 0)
                {
                    org.Add(new Organization(Convert.ToInt32(sr.ReadLine().Split(',')[0]), 0, "", 0, ""));
                }
            }
        }

        private void organizationsIdsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = org.Count;
            progressBar1.Step = 1;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.RestoreDirectory = true;
            sfd.FileName = "org_ids.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                for (int i = 0; i < org.Count; i++)
                {
                    sw.WriteLine(org[i].Id);
                    progressBar1.PerformStep();
                }
                sw.Close();
            }
        }

    }
}