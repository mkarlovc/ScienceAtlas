using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Xml;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;


namespace AtlasWebService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {
        //Setup indexer
        Directory directory;
        Analyzer analyzer;

        int totDocs;
        IndexReader indexReader;
        Searcher indexSearcher;

        IndexWriter writer;
        IndexReader red;

        //Setup searcher
        IndexSearcher searcher;
        MultiFieldQueryParser parser;

        [WebMethod]
        public XmlDocument AllRsr()
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT id, mstid, firstName, lastName, status, keyws, (select tblScienceCodes.description from tblScienceCodes where tblResearchers.science = tblScienceCodes.scienceId) as science,(select tblFieldCodes.description from tblFieldCodes where tblResearchers.science = tblFieldCodes.scienceId and tblFieldCodes.fieldId = field) as field,(select tblSubfieldCodes.description from tblSubfieldCodes where science = tblSubfieldCodes.scienceId and tblSubfieldCodes.fieldId = field and tblSubfieldCodes.subfieldId = subfield) as subfield, tell, fax, email, url FROM dbo.tblResearchers;", con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();

                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlNode researchersNode = doc.CreateElement("Researchers");
                doc.AppendChild(researchersNode);

                while (dtr.Read())
                {
                    XmlNode rsrNode = doc.CreateElement("RSR");
                    researchersNode.AppendChild(rsrNode);

                    //////////////////attributes/////////////////////

                    XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                    mstidAtt.Value = dtr["mstid"].ToString();
                    rsrNode.Attributes.Append(mstidAtt);

                    XmlAttribute idAtt = doc.CreateAttribute("id");
                    idAtt.Value = dtr["id"].ToString();
                    rsrNode.Attributes.Append(idAtt);

                    XmlAttribute statusAtt = doc.CreateAttribute("status");
                    statusAtt.Value = dtr["status"].ToString();
                    rsrNode.Attributes.Append(statusAtt);

                    ///////////////////elements////////////////////////

                    XmlNode firstnameNode = doc.CreateElement("firstName");
                    firstnameNode.AppendChild(doc.CreateTextNode(dtr["firstName"].ToString()));
                    rsrNode.AppendChild(firstnameNode);

                    XmlNode lastNameNode = doc.CreateElement("lastName");
                    lastNameNode.AppendChild(doc.CreateTextNode(dtr["lastName"].ToString()));
                    rsrNode.AppendChild(lastNameNode);

                    if (dtr["keyws"].ToString() != "")
                    {
                        XmlNode keywsNode = doc.CreateElement("keyws");
                        keywsNode.AppendChild(doc.CreateTextNode(dtr["keyws"].ToString()));
                        rsrNode.AppendChild(keywsNode);
                    }

                    if (dtr["science"].ToString() != "")
                    {
                        XmlNode scienceNode = doc.CreateElement("science");
                        scienceNode.AppendChild(doc.CreateTextNode(dtr["science"].ToString()));
                        rsrNode.AppendChild(scienceNode);
                    }

                    if (dtr["field"].ToString() != "")
                    {
                        XmlNode fieldNode = doc.CreateElement("field");
                        fieldNode.AppendChild(doc.CreateTextNode(dtr["field"].ToString()));
                        rsrNode.AppendChild(fieldNode);
                    }

                    if (dtr["subfield"].ToString() != "")
                    {
                        XmlNode subfieldNode = doc.CreateElement("subfield");
                        subfieldNode.AppendChild(doc.CreateTextNode(dtr["subfield"].ToString()));
                        rsrNode.AppendChild(subfieldNode);
                    }

                    if (dtr["tell"].ToString() != "")
                    {
                        XmlNode telNode = doc.CreateElement("tel");
                        telNode.AppendChild(doc.CreateTextNode(dtr["tell"].ToString()));
                        rsrNode.AppendChild(telNode);
                    }

                    if (dtr["fax"].ToString() != "")
                    {
                        XmlNode faxNode = doc.CreateElement("fax");
                        faxNode.AppendChild(doc.CreateTextNode(dtr["fax"].ToString()));
                        rsrNode.AppendChild(faxNode);
                    }

                    if (dtr["email"].ToString() != "")
                    {
                        XmlNode emailNode = doc.CreateElement("email");
                        emailNode.AppendChild(doc.CreateTextNode(dtr["email"].ToString()));
                        rsrNode.AppendChild(emailNode);
                    }

                    if (dtr["url"].ToString() != "")
                    {
                        XmlNode urlNode = doc.CreateElement("url");
                        urlNode.AppendChild(doc.CreateTextNode(dtr["url"].ToString()));
                        rsrNode.AppendChild(urlNode);
                    }
                }

                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }

            return doc;
        }

        [WebMethod]
        public XmlDocument RsrByName(string fname, string lname)
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            fname += "%";
            lname += "%";

            fname = fname.Replace("c", "[č|ć|c]");
            lname = lname.Replace("c","[č|ć|c]"); 
            fname = fname.Replace("z","[ž|z]");
            lname = lname.Replace("z", "[ž|z]");
            fname = fname.Replace("s", "[š|s]");
            lname = lname.Replace("s", "[š|s]");

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT id, mstid, firstName, lastName, status, keyws, science as scienceCode, field as fieldCode, subfield as subfieldCode, (select tblScienceCodes.description from tblScienceCodes where tblResearchers.science = tblScienceCodes.scienceId) as science,(select tblFieldCodes.description from tblFieldCodes where tblResearchers.science = tblFieldCodes.scienceId and tblFieldCodes.fieldId = field) as field,(select tblSubfieldCodes.description from tblSubfieldCodes where science = tblSubfieldCodes.scienceId and tblSubfieldCodes.fieldId = field and tblSubfieldCodes.subfieldId = subfield) as subfield, tell, fax, email, url FROM dbo.tblResearchers WHERE firstName like '" + fname + "' and lastName like '" + lname + "';", con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();
                
                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlNode researchersNode = doc.CreateElement("Researchers");
                doc.AppendChild(researchersNode);
                
                while (dtr.Read())
                {
                    XmlNode rsrNode = doc.CreateElement("RSR");
                    researchersNode.AppendChild(rsrNode);

                    //////////////////attributes/////////////////////

                    XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                    mstidAtt.Value = dtr["mstid"].ToString();
                    rsrNode.Attributes.Append(mstidAtt);

                    XmlAttribute idAtt = doc.CreateAttribute("id");
                    idAtt.Value = dtr["id"].ToString();
                    rsrNode.Attributes.Append(idAtt);

                    XmlAttribute statusAtt = doc.CreateAttribute("status");
                    statusAtt.Value = dtr["status"].ToString();
                    rsrNode.Attributes.Append(statusAtt);

                    ///////////////////elements////////////////////////

                    XmlNode firstnameNode = doc.CreateElement("firstName");
                    firstnameNode.AppendChild(doc.CreateTextNode(dtr["firstName"].ToString()));
                    rsrNode.AppendChild(firstnameNode);

                    XmlNode lastNameNode = doc.CreateElement("lastName");
                    lastNameNode.AppendChild(doc.CreateTextNode(dtr["lastName"].ToString()));
                    rsrNode.AppendChild(lastNameNode);

                    if (dtr["keyws"].ToString() != "")
                    {
                        XmlNode keywsNode = doc.CreateElement("keyws");
                        keywsNode.AppendChild(doc.CreateTextNode(dtr["keyws"].ToString()));
                        rsrNode.AppendChild(keywsNode);
                    }

                    if (dtr["science"].ToString() != "")
                    {
                        XmlNode scienceNode = doc.CreateElement("science");
                        scienceNode.AppendChild(doc.CreateTextNode(dtr["science"].ToString()));
                        rsrNode.AppendChild(scienceNode);

                        XmlAttribute scienceCodeAtt = doc.CreateAttribute("scienceCode");
                        scienceCodeAtt.Value = dtr["scienceCode"].ToString();
                        scienceNode.Attributes.Append(scienceCodeAtt);
                    }

                    if (dtr["field"].ToString() != "")
                    {
                        XmlNode fieldNode = doc.CreateElement("field");
                        fieldNode.AppendChild(doc.CreateTextNode(dtr["field"].ToString()));
                        rsrNode.AppendChild(fieldNode);

                        XmlAttribute fieldCodeAtt = doc.CreateAttribute("fieldCode");
                        fieldCodeAtt.Value = dtr["fieldCode"].ToString();
                        fieldNode.Attributes.Append(fieldCodeAtt);
                    }

                    if (dtr["subfield"].ToString() != "")
                    {
                        XmlNode subfieldNode = doc.CreateElement("subfield");
                        subfieldNode.AppendChild(doc.CreateTextNode(dtr["subfield"].ToString()));
                        rsrNode.AppendChild(subfieldNode);

                        XmlAttribute subfieldCodeAtt = doc.CreateAttribute("subfieldCode");
                        subfieldCodeAtt.Value = dtr["subfieldCode"].ToString();
                        subfieldNode.Attributes.Append(subfieldCodeAtt);
                    }

                    if (dtr["tell"].ToString() != "")
                    {
                        XmlNode telNode = doc.CreateElement("tel");
                        telNode.AppendChild(doc.CreateTextNode(dtr["tell"].ToString()));
                        rsrNode.AppendChild(telNode);
                    }

                    if (dtr["fax"].ToString() != "")
                    {
                        XmlNode faxNode = doc.CreateElement("fax");
                        faxNode.AppendChild(doc.CreateTextNode(dtr["fax"].ToString()));
                        rsrNode.AppendChild(faxNode);
                    }

                    if (dtr["email"].ToString() != "")
                    {
                        XmlNode emailNode = doc.CreateElement("email");
                        emailNode.AppendChild(doc.CreateTextNode(dtr["email"].ToString()));
                        rsrNode.AppendChild(emailNode);
                    }

                    if (dtr["url"].ToString() != "")
                    {
                        XmlNode urlNode = doc.CreateElement("url");
                        urlNode.AppendChild(doc.CreateTextNode(dtr["url"].ToString()));
                        rsrNode.AppendChild(urlNode);
                    }
                }
                
                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }
            
            return doc;
        }
        
        [WebMethod]
        public XmlDocument RsrById(string id)
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT id, mstid, firstName, lastName, status, keyws, science as scienceCode, field as fieldCode, subfield as subfieldCode, (select tblScienceCodes.description from tblScienceCodes where tblResearchers.science = tblScienceCodes.scienceId) as science,(select tblFieldCodes.description from tblFieldCodes where tblResearchers.science = tblFieldCodes.scienceId and tblFieldCodes.fieldId = field) as field,(select tblSubfieldCodes.description from tblSubfieldCodes where science = tblSubfieldCodes.scienceId and tblSubfieldCodes.fieldId = field and tblSubfieldCodes.subfieldId = subfield) as subfield, tell, fax, email, url FROM dbo.tblResearchers WHERE id = '" + id + "';", con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();

                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlNode researchersNode = doc.CreateElement("Researchers");
                doc.AppendChild(researchersNode);

                while (dtr.Read())
                {
                    XmlNode rsrNode = doc.CreateElement("RSR");
                    researchersNode.AppendChild(rsrNode);

                    //////////////////attributes/////////////////////

                    XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                    mstidAtt.Value = dtr["mstid"].ToString();
                    rsrNode.Attributes.Append(mstidAtt);

                    XmlAttribute idAtt = doc.CreateAttribute("id");
                    idAtt.Value = dtr["id"].ToString();
                    rsrNode.Attributes.Append(idAtt);

                    XmlAttribute statusAtt = doc.CreateAttribute("status");
                    statusAtt.Value = dtr["status"].ToString();
                    rsrNode.Attributes.Append(statusAtt);

                    ///////////////////elements////////////////////////

                    XmlNode firstnameNode = doc.CreateElement("firstName");
                    firstnameNode.AppendChild(doc.CreateTextNode(dtr["firstName"].ToString()));
                    rsrNode.AppendChild(firstnameNode);

                    XmlNode lastNameNode = doc.CreateElement("lastName");
                    lastNameNode.AppendChild(doc.CreateTextNode(dtr["lastName"].ToString()));
                    rsrNode.AppendChild(lastNameNode);

                    if (dtr["keyws"].ToString() != "")
                    {
                        XmlNode keywsNode = doc.CreateElement("keyws");
                        keywsNode.AppendChild(doc.CreateTextNode(dtr["keyws"].ToString()));
                        rsrNode.AppendChild(keywsNode);
                    }

                    if (dtr["science"].ToString() != "")
                    {
                        XmlNode scienceNode = doc.CreateElement("science");
                        scienceNode.AppendChild(doc.CreateTextNode(dtr["science"].ToString()));
                        rsrNode.AppendChild(scienceNode);

                        XmlAttribute scienceCodeAtt = doc.CreateAttribute("scienceCode");
                        scienceCodeAtt.Value = dtr["scienceCode"].ToString();
                        scienceNode.Attributes.Append(scienceCodeAtt);
                    }

                    if (dtr["field"].ToString() != "")
                    {
                        XmlNode fieldNode = doc.CreateElement("field");
                        fieldNode.AppendChild(doc.CreateTextNode(dtr["field"].ToString()));
                        rsrNode.AppendChild(fieldNode);

                        XmlAttribute fieldCodeAtt = doc.CreateAttribute("fieldCode");
                        fieldCodeAtt.Value = dtr["fieldCode"].ToString();
                        fieldNode.Attributes.Append(fieldCodeAtt);
                    }

                    if (dtr["subfield"].ToString() != "")
                    {
                        XmlNode subfieldNode = doc.CreateElement("subfield");
                        subfieldNode.AppendChild(doc.CreateTextNode(dtr["subfield"].ToString()));
                        rsrNode.AppendChild(subfieldNode);

                        XmlAttribute subfieldCodeAtt = doc.CreateAttribute("subfieldCode");
                        subfieldCodeAtt.Value = dtr["subfieldCode"].ToString();
                        subfieldNode.Attributes.Append(subfieldCodeAtt);
                    }

                    if (dtr["tell"].ToString() != "")
                    {
                        XmlNode telNode = doc.CreateElement("tel");
                        telNode.AppendChild(doc.CreateTextNode(dtr["tell"].ToString()));
                        rsrNode.AppendChild(telNode);
                    }

                    if (dtr["fax"].ToString() != "")
                    {
                        XmlNode faxNode = doc.CreateElement("fax");
                        faxNode.AppendChild(doc.CreateTextNode(dtr["fax"].ToString()));
                        rsrNode.AppendChild(faxNode);
                    }

                    if (dtr["email"].ToString() != "")
                    {
                        XmlNode emailNode = doc.CreateElement("email");
                        emailNode.AppendChild(doc.CreateTextNode(dtr["email"].ToString()));
                        rsrNode.AppendChild(emailNode);
                    }

                    if (dtr["url"].ToString() != "")
                    {
                        XmlNode urlNode = doc.CreateElement("url");
                        urlNode.AppendChild(doc.CreateTextNode(dtr["url"].ToString()));
                        rsrNode.AppendChild(urlNode);
                    }
                }

                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }

            return doc;
        }

        [WebMethod]
        public XmlDocument RsrByClassification(string science, string field, string subfield)
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            if (subfield=="") subfield = "%";
            if (field == "") field = "%";
            

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT id, mstid, firstName, lastName, status, keyws, science as scienceCode, field as fieldCode, subfield as subfieldCode, (select tblScienceCodes.description from tblScienceCodes where tblResearchers.science = tblScienceCodes.scienceId) as science,(select tblFieldCodes.description from tblFieldCodes where tblResearchers.science = tblFieldCodes.scienceId and tblFieldCodes.fieldId = field) as field,(select tblSubfieldCodes.description from tblSubfieldCodes where science = tblSubfieldCodes.scienceId and tblSubfieldCodes.fieldId = field and tblSubfieldCodes.subfieldId = subfield) as subfield, tell, fax, email, url FROM dbo.tblResearchers WHERE science = '" + science + "' and field like '"+field+"' and subfield like '"+subfield+"';", con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();

                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlNode researchersNode = doc.CreateElement("Researchers");
                doc.AppendChild(researchersNode);

                while (dtr.Read())
                {
                    XmlNode rsrNode = doc.CreateElement("RSR");
                    researchersNode.AppendChild(rsrNode);

                    //////////////////attributes/////////////////////

                    XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                    mstidAtt.Value = dtr["mstid"].ToString();
                    rsrNode.Attributes.Append(mstidAtt);

                    XmlAttribute idAtt = doc.CreateAttribute("id");
                    idAtt.Value = dtr["id"].ToString();
                    rsrNode.Attributes.Append(idAtt);

                    XmlAttribute statusAtt = doc.CreateAttribute("status");
                    statusAtt.Value = dtr["status"].ToString();
                    rsrNode.Attributes.Append(statusAtt);

                    ///////////////////elements////////////////////////

                    XmlNode firstnameNode = doc.CreateElement("firstName");
                    firstnameNode.AppendChild(doc.CreateTextNode(dtr["firstName"].ToString()));
                    rsrNode.AppendChild(firstnameNode);

                    XmlNode lastNameNode = doc.CreateElement("lastName");
                    lastNameNode.AppendChild(doc.CreateTextNode(dtr["lastName"].ToString()));
                    rsrNode.AppendChild(lastNameNode);

                    if (dtr["keyws"].ToString() != "")
                    {
                        XmlNode keywsNode = doc.CreateElement("keyws");
                        keywsNode.AppendChild(doc.CreateTextNode(dtr["keyws"].ToString()));
                        rsrNode.AppendChild(keywsNode);
                    }

                    if (dtr["science"].ToString() != "")
                    {
                        XmlNode scienceNode = doc.CreateElement("science");
                        scienceNode.AppendChild(doc.CreateTextNode(dtr["science"].ToString()));
                        rsrNode.AppendChild(scienceNode);

                        XmlAttribute scienceCodeAtt = doc.CreateAttribute("scienceCode");
                        scienceCodeAtt.Value = dtr["scienceCode"].ToString();
                        scienceNode.Attributes.Append(scienceCodeAtt);
                    }

                    if (dtr["field"].ToString() != "")
                    {
                        XmlNode fieldNode = doc.CreateElement("field");
                        fieldNode.AppendChild(doc.CreateTextNode(dtr["field"].ToString()));
                        rsrNode.AppendChild(fieldNode);

                        XmlAttribute fieldCodeAtt = doc.CreateAttribute("fieldCode");
                        fieldCodeAtt.Value = dtr["fieldCode"].ToString();
                        fieldNode.Attributes.Append(fieldCodeAtt);
                    }

                    if (dtr["subfield"].ToString() != "")
                    {
                        XmlNode subfieldNode = doc.CreateElement("subfield");
                        subfieldNode.AppendChild(doc.CreateTextNode(dtr["subfield"].ToString()));
                        rsrNode.AppendChild(subfieldNode);

                        XmlAttribute subfieldCodeAtt = doc.CreateAttribute("subfieldCode");
                        subfieldCodeAtt.Value = dtr["subfieldCode"].ToString();
                        subfieldNode.Attributes.Append(subfieldCodeAtt);
                    }

                    if (dtr["tell"].ToString() != "")
                    {
                        XmlNode telNode = doc.CreateElement("tel");
                        telNode.AppendChild(doc.CreateTextNode(dtr["tell"].ToString()));
                        rsrNode.AppendChild(telNode);
                    }

                    if (dtr["fax"].ToString() != "")
                    {
                        XmlNode faxNode = doc.CreateElement("fax");
                        faxNode.AppendChild(doc.CreateTextNode(dtr["fax"].ToString()));
                        rsrNode.AppendChild(faxNode);
                    }

                    if (dtr["email"].ToString() != "")
                    {
                        XmlNode emailNode = doc.CreateElement("email");
                        emailNode.AppendChild(doc.CreateTextNode(dtr["email"].ToString()));
                        rsrNode.AppendChild(emailNode);
                    }

                    if (dtr["url"].ToString() != "")
                    {
                        XmlNode urlNode = doc.CreateElement("url");
                        urlNode.AppendChild(doc.CreateTextNode(dtr["url"].ToString()));
                        rsrNode.AppendChild(urlNode);
                    }
                }

                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }

            return doc;
        }

        [WebMethod]
        public XmlDocument PrjByName(string name, string id, string mstid)
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            if (name == "") name = "%";
            if (id == "") id = "%";
            if (mstid == "") mstid = "%";

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.tblProjects WHERE id like '" + id + "' and mstid like '" + mstid + "' and name like '" + name + "';", con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();

                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlNode projectsNode = doc.CreateElement("Projects");
                doc.AppendChild(projectsNode);

                while (dtr.Read())
                {
                    XmlNode prjNode = doc.CreateElement("PRJ");
                    projectsNode.AppendChild(prjNode);

                    //////////////////attributes/////////////////////

                    XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                    mstidAtt.Value = dtr["mstid"].ToString();
                    prjNode.Attributes.Append(mstidAtt);

                    XmlAttribute idAtt = doc.CreateAttribute("id");
                    idAtt.Value = dtr["id"].ToString();
                    prjNode.Attributes.Append(idAtt);

                    ///////////////////elements////////////////////////

                    XmlNode nameNode = doc.CreateElement("name");
                    nameNode.AppendChild(doc.CreateTextNode(dtr["name"].ToString()));
                    prjNode.AppendChild(nameNode);

                    XmlNode abstractNode = doc.CreateElement("abstract");
                    abstractNode.AppendChild(doc.CreateTextNode(dtr["abstract"].ToString()));
                    prjNode.AppendChild(abstractNode);

                    XmlNode keywsNode = doc.CreateElement("keyws");
                    keywsNode.AppendChild(doc.CreateTextNode(dtr["keyws"].ToString()));
                    prjNode.AppendChild(keywsNode);

                    XmlNode signWorldNode = doc.CreateElement("worldSign");
                    signWorldNode.AppendChild(doc.CreateTextNode(dtr["sign_world"].ToString()));
                    prjNode.AppendChild(signWorldNode);

                    XmlNode signDomNode = doc.CreateElement("domSign");
                    signDomNode.AppendChild(doc.CreateTextNode(dtr["sign_dom"].ToString()));
                    prjNode.AppendChild(signDomNode);
                }

                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }

            return doc;
        }

        [WebMethod]
        public XmlDocument PrjOfRsr(string name, string id, string mstid)
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            if (name == "") name = "%";
            if (id == "") id = "%";
            if (mstid == "") mstid = "%";

            try
            {
                SqlCommand cmd = new SqlCommand("select * from tblProjects where id IN( select prjId from tblRsrHasPrj where rsrId IN( select id from tblResearchers where id = "+ id +") ) order by name;", con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();

                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlNode projectsNode = doc.CreateElement("Projects");
                doc.AppendChild(projectsNode);

                while (dtr.Read())
                {
                    XmlNode prjNode = doc.CreateElement("PRJ");
                    projectsNode.AppendChild(prjNode);

                    //////////////////attributes/////////////////////

                    XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                    mstidAtt.Value = dtr["mstid"].ToString();
                    prjNode.Attributes.Append(mstidAtt);

                    XmlAttribute idAtt = doc.CreateAttribute("id");
                    idAtt.Value = dtr["id"].ToString();
                    prjNode.Attributes.Append(idAtt);

                    XmlAttribute startdate = doc.CreateAttribute("startdate");
                    startdate.Value = dtr["startdate"].ToString();
                    prjNode.Attributes.Append(startdate);

                    XmlAttribute enddate = doc.CreateAttribute("enddate");
                    enddate.Value = dtr["enddate"].ToString();
                    prjNode.Attributes.Append(enddate);

                    ///////////////////elements////////////////////////

                    XmlNode nameNode = doc.CreateElement("name");
                    nameNode.AppendChild(doc.CreateTextNode(dtr["name"].ToString()));
                    prjNode.AppendChild(nameNode);

                    XmlNode abstractNode = doc.CreateElement("abstract");
                    abstractNode.AppendChild(doc.CreateTextNode(dtr["abstract"].ToString()));
                    prjNode.AppendChild(abstractNode);

                    XmlNode keywsNode = doc.CreateElement("keyws");
                    keywsNode.AppendChild(doc.CreateTextNode(dtr["keyws"].ToString()));
                    prjNode.AppendChild(keywsNode);

                    XmlNode signWorldNode = doc.CreateElement("worldSign");
                    signWorldNode.AppendChild(doc.CreateTextNode(dtr["sign_world"].ToString()));
                    prjNode.AppendChild(signWorldNode);

                    XmlNode signDomNode = doc.CreateElement("domSign");
                    signDomNode.AppendChild(doc.CreateTextNode(dtr["sign_dom"].ToString()));
                    prjNode.AppendChild(signDomNode);
                }

                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }

            return doc;
        }
  
        [WebMethod]
        public XmlDocument OrgOfRsr(string name, string id, string mstid)
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            if (name == "") name = "%";
            if (id == "") id = "%";
            if (mstid == "") mstid = "%";

            try
            {
                SqlCommand cmd = new SqlCommand("select * from tblOrganizations where id IN( select orgId from tblRsrIsinOrg where rsrId IN( select id from tblResearchers where id = " + id + ") ) order by name;", con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();

                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlNode organizationsNode = doc.CreateElement("Organizations");
                doc.AppendChild(organizationsNode);

                while (dtr.Read())
                {
                    XmlNode orgNode = doc.CreateElement("ORG");
                    organizationsNode.AppendChild(orgNode);

                    //////////////////attributes/////////////////////

                    XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                    mstidAtt.Value = dtr["mstid"].ToString();
                    orgNode.Attributes.Append(mstidAtt);

                    XmlAttribute idAtt = doc.CreateAttribute("id");
                    idAtt.Value = dtr["id"].ToString();
                    orgNode.Attributes.Append(idAtt);

                    ///////////////////elements////////////////////////

                    XmlNode nameNode = doc.CreateElement("name");
                    nameNode.AppendChild(doc.CreateTextNode(dtr["name"].ToString()));
                    orgNode.AppendChild(nameNode);

                    XmlNode headNode = doc.CreateElement("head");
                    headNode.AppendChild(doc.CreateTextNode(dtr["head"].ToString()));
                    orgNode.AppendChild(headNode);

                    XmlNode cityNode = doc.CreateElement("city");
                    cityNode.AppendChild(doc.CreateTextNode(dtr["city"].ToString()));
                    orgNode.AppendChild(cityNode);
                }

                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }

            return doc;
        }

        [WebMethod]
        public XmlDocument LecOfRsr(string id)
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            try
            {
                SqlCommand cmd = new SqlCommand("select *, (select tblRsrHasLec.role from tblRsrHasLec where tblLectures.url = tblRsrHasLec.lecUrl and tblRsrHasLec.rsrId = '"+id+"' ) as role from tblLectures where url IN(select lecUrl from tblRsrHasLec where rsrId IN( select id from tblResearchers where id = '"+id+"')) order by title;", con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();

                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlNode lecturesNode = doc.CreateElement("Lectures");
                doc.AppendChild(lecturesNode);

                while (dtr.Read())
                {
                    XmlNode lecNode = doc.CreateElement("LEC");
                    lecturesNode.AppendChild(lecNode);

                    //////////////////attributes/////////////////////

                    XmlAttribute urlAtt = doc.CreateAttribute("url");
                    urlAtt.Value = dtr["url"].ToString();
                    lecNode.Attributes.Append(urlAtt);

                    XmlAttribute langAtt = doc.CreateAttribute("lang");
                    langAtt.Value = dtr["lang"].ToString();
                    lecNode.Attributes.Append(langAtt);

                    XmlAttribute enabledAtt = doc.CreateAttribute("enabled");
                    enabledAtt.Value = dtr["enabled"].ToString();
                    lecNode.Attributes.Append(enabledAtt);

                    XmlAttribute ispubAtt = doc.CreateAttribute("ispublic");
                    ispubAtt.Value = dtr["ispublic"].ToString();
                    lecNode.Attributes.Append(ispubAtt);

                    ///////////////////elements////////////////////////

                    XmlNode titleNode = doc.CreateElement("title");
                    titleNode.AppendChild(doc.CreateTextNode(dtr["title"].ToString()));
                    lecNode.AppendChild(titleNode);

                    XmlNode descNode = doc.CreateElement("description");
                    descNode.AppendChild(doc.CreateTextNode(dtr["description"].ToString()));
                    lecNode.AppendChild(descNode);

                    XmlNode typeNode = doc.CreateElement("type");
                    typeNode.AppendChild(doc.CreateTextNode(dtr["type"].ToString()));
                    lecNode.AppendChild(typeNode);

                    XmlNode recordedNode = doc.CreateElement("recorded");
                    recordedNode.AppendChild(doc.CreateTextNode(dtr["recorded"].ToString()));
                    lecNode.AppendChild(recordedNode);

                    XmlNode publishedNode = doc.CreateElement("published");
                    publishedNode.AppendChild(doc.CreateTextNode(dtr["published"].ToString()));
                    lecNode.AppendChild(publishedNode);

                    XmlNode viewsNode = doc.CreateElement("views");
                    viewsNode.AppendChild(doc.CreateTextNode(dtr["views"].ToString()));
                    lecNode.AppendChild(viewsNode);

                    XmlNode roleNode = doc.CreateElement("role");
                    roleNode.AppendChild(doc.CreateTextNode(dtr["role"].ToString()));
                    lecNode.AppendChild(roleNode);

                }

                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }

            return doc;
        }

        [WebMethod]
        public XmlDocument CollaborationOfRsrOnPrj(string id)
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            if (id == "") id = "%";

            try
            {
                SqlCommand cmd = new SqlCommand("select id, mstid, firstName, lastName, status, keyws, tell, fax, email, url, science as scienceCode, field as fieldCode, subfield as subfieldCode, (select name from tblOrganizations where id IN(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId)) as orgName,(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId) as orgId, (select tblScienceCodes.description from tblScienceCodes where tblResearchers.science = tblScienceCodes.scienceId) as science,(select tblFieldCodes.description from tblFieldCodes where tblResearchers.science = tblFieldCodes.scienceId and tblFieldCodes.fieldId = field) as field,(select tblSubfieldCodes.description from tblSubfieldCodes where science = tblSubfieldCodes.scienceId and tblSubfieldCodes.fieldId = field and tblSubfieldCodes.subfieldId = subfield) as subfield, N from  tblResearchers,(select rsrId, count(*) as N from tblRsrHasPrj where prjId in( SELECT prjId FROM tblRsrHasPrj where rsrId = '" + id + "') group by rsrId) as t2 where tblResearchers.id = t2.rsrId order by N desc, CASE rsrId WHEN '" + id + "' THEN 1 ELSE 100 END, lastName", con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();

                if (dtr.FieldCount > 0)
                {

                    XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    doc.AppendChild(docNode);

                    XmlNode researchersNode = doc.CreateElement("Researchers");
                    doc.AppendChild(researchersNode);

                    while (dtr.Read())
                    {
                        XmlNode prjNode = doc.CreateElement("RSR");
                        researchersNode.AppendChild(prjNode);

                        //////////////////attributes/////////////////////

                        XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                        mstidAtt.Value = dtr["mstid"].ToString();
                        prjNode.Attributes.Append(mstidAtt);

                        XmlAttribute idAtt = doc.CreateAttribute("id");
                        idAtt.Value = dtr["id"].ToString();
                        prjNode.Attributes.Append(idAtt);

                        ///////////////////elements////////////////////////

                        XmlNode firstnameNode = doc.CreateElement("firstName");
                        firstnameNode.AppendChild(doc.CreateTextNode(dtr["firstName"].ToString()));
                        prjNode.AppendChild(firstnameNode);

                        XmlNode lastNameNode = doc.CreateElement("lastName");
                        lastNameNode.AppendChild(doc.CreateTextNode(dtr["lastName"].ToString()));
                        prjNode.AppendChild(lastNameNode);

                        if (dtr["keyws"].ToString() != "")
                        {
                            XmlNode keywsNode = doc.CreateElement("keyws");
                            keywsNode.AppendChild(doc.CreateTextNode(dtr["keyws"].ToString()));
                            prjNode.AppendChild(keywsNode);
                        }

                        XmlNode countNode = doc.CreateElement("collaborationCount");
                        countNode.AppendChild(doc.CreateTextNode(dtr["N"].ToString()));
                        prjNode.AppendChild(countNode);

                        if (dtr["orgName"].ToString() != "")
                        {
                            XmlNode orgNameNode = doc.CreateElement("orgName");
                            orgNameNode.AppendChild(doc.CreateTextNode(dtr["orgName"].ToString()));
                            prjNode.AppendChild(orgNameNode);
                        }

                        if (dtr["orgId"].ToString() != "")
                        {
                            XmlNode orgIdNode = doc.CreateElement("orgId");
                            orgIdNode.AppendChild(doc.CreateTextNode(dtr["orgId"].ToString()));
                            prjNode.AppendChild(orgIdNode);
                        }

                        if (dtr["science"].ToString() != "")
                        {
                            XmlNode scienceNode = doc.CreateElement("science");
                            scienceNode.AppendChild(doc.CreateTextNode(dtr["science"].ToString()));
                            prjNode.AppendChild(scienceNode);

                            XmlAttribute scienceCodeAtt = doc.CreateAttribute("scienceCode");
                            scienceCodeAtt.Value = dtr["scienceCode"].ToString();
                            scienceNode.Attributes.Append(scienceCodeAtt);
                        }

                        if (dtr["field"].ToString() != "")
                        {
                            XmlNode fieldNode = doc.CreateElement("field");
                            fieldNode.AppendChild(doc.CreateTextNode(dtr["field"].ToString()));
                            prjNode.AppendChild(fieldNode);

                            XmlAttribute fieldCodeAtt = doc.CreateAttribute("fieldCode");
                            fieldCodeAtt.Value = dtr["fieldCode"].ToString();
                            fieldNode.Attributes.Append(fieldCodeAtt);
                        }

                        if (dtr["subfield"].ToString() != "")
                        {
                            XmlNode subfieldNode = doc.CreateElement("subfield");
                            subfieldNode.AppendChild(doc.CreateTextNode(dtr["subfield"].ToString()));
                            prjNode.AppendChild(subfieldNode);

                            XmlAttribute subfieldCodeAtt = doc.CreateAttribute("subfieldCode");
                            subfieldCodeAtt.Value = dtr["subfieldCode"].ToString();
                            subfieldNode.Attributes.Append(subfieldCodeAtt);
                        }

                        if (dtr["tell"].ToString() != "")
                        {
                            XmlNode telNode = doc.CreateElement("tel");
                            telNode.AppendChild(doc.CreateTextNode(dtr["tell"].ToString()));
                            prjNode.AppendChild(telNode);
                        }

                        if (dtr["fax"].ToString() != "")
                        {
                            XmlNode faxNode = doc.CreateElement("fax");
                            faxNode.AppendChild(doc.CreateTextNode(dtr["fax"].ToString()));
                            prjNode.AppendChild(faxNode);
                        }

                        if (dtr["email"].ToString() != "")
                        {
                            XmlNode emailNode = doc.CreateElement("email");
                            emailNode.AppendChild(doc.CreateTextNode(dtr["email"].ToString()));
                            prjNode.AppendChild(emailNode);
                        }

                        if (dtr["url"].ToString() != "")
                        {
                            XmlNode urlNode = doc.CreateElement("url");
                            urlNode.AppendChild(doc.CreateTextNode(dtr["url"].ToString()));
                            prjNode.AppendChild(urlNode);
                        }
                    }
                }
                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }

            return doc;
        }

        [WebMethod]
        public XmlDocument CollaborationOfRsrOnPrj1(string id)
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            if (id == "") id = "%";

            try
            {
                SqlCommand cmd = new SqlCommand("select id, mstid, firstName, lastName, status, keyws, tell, fax, email, url, science as scienceCode, field as fieldCode, subfield as subfieldCode,(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId) as orgId1,( select name from tblOrganizations where id IN( select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId)) as orgName1,(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId and tblRsrIsinOrg.orgId not in (select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId order by tblRsrIsinOrg.orgId) order by tblRsrIsinOrg.orgId) as orgId2, (select name from tblOrganizations where id  IN(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId and tblRsrIsinOrg.orgId not in (select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId order by tblRsrIsinOrg.orgId)order by tblRsrIsinOrg.orgId)) as orgName2,(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId and tblRsrIsinOrg.orgId not in (select TOP 2 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId order by tblRsrIsinOrg.orgId) order by tblRsrIsinOrg.orgId) as orgId3,(select name from tblOrganizations where id  IN(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId and tblRsrIsinOrg.orgId not in (select TOP 2 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId order by tblRsrIsinOrg.orgId)order by tblRsrIsinOrg.orgId)) as orgName3,(select tblScienceCodes.description from tblScienceCodes where tblResearchers.science = tblScienceCodes.scienceId) as science,(select tblFieldCodes.description from tblFieldCodes where tblResearchers.science = tblFieldCodes.scienceId and tblFieldCodes.fieldId = field) as field,(select tblSubfieldCodes.description from tblSubfieldCodes where science = tblSubfieldCodes.scienceId and tblSubfieldCodes.fieldId = field and tblSubfieldCodes.subfieldId = subfield) as subfield, N from  tblResearchers,(select rsrId, count(*) as N from tblRsrHasPrj where prjId in( SELECT prjId FROM tblRsrHasPrj where rsrId = '" + id + "') group by rsrId) as t2 where tblResearchers.id = t2.rsrId order by N desc, CASE rsrId WHEN '" + id + "' THEN 1 ELSE 100 END, lastName", con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();

                if (dtr.FieldCount > 0)
                {

                    XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    doc.AppendChild(docNode);

                    XmlNode researchersNode = doc.CreateElement("Researchers");
                    doc.AppendChild(researchersNode);

                    while (dtr.Read())
                    {
                        XmlNode prjNode = doc.CreateElement("RSR");
                        researchersNode.AppendChild(prjNode);

                        //////////////////attributes/////////////////////

                        XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                        mstidAtt.Value = dtr["mstid"].ToString();
                        prjNode.Attributes.Append(mstidAtt);

                        XmlAttribute idAtt = doc.CreateAttribute("id");
                        idAtt.Value = dtr["id"].ToString();
                        prjNode.Attributes.Append(idAtt);

                        ///////////////////elements////////////////////////

                        XmlNode firstnameNode = doc.CreateElement("firstName");
                        firstnameNode.AppendChild(doc.CreateTextNode(dtr["firstName"].ToString()));
                        prjNode.AppendChild(firstnameNode);

                        XmlNode lastNameNode = doc.CreateElement("lastName");
                        lastNameNode.AppendChild(doc.CreateTextNode(dtr["lastName"].ToString()));
                        prjNode.AppendChild(lastNameNode);

                        if (dtr["keyws"].ToString() != "")
                        {
                            XmlNode keywsNode = doc.CreateElement("keyws");
                            keywsNode.AppendChild(doc.CreateTextNode(dtr["keyws"].ToString()));
                            prjNode.AppendChild(keywsNode);
                        }

                        XmlNode countNode = doc.CreateElement("collaborationCount");
                        countNode.AppendChild(doc.CreateTextNode(dtr["N"].ToString()));
                        prjNode.AppendChild(countNode);
                        
                        if (dtr["orgName1"].ToString() != "")
                        {
                            XmlNode orgNameNode1 = doc.CreateElement("orgName1");
                            orgNameNode1.AppendChild(doc.CreateTextNode(dtr["orgName1"].ToString()));
                            prjNode.AppendChild(orgNameNode1);
                        }

                        if (dtr["orgId1"].ToString() != "")
                        {
                            XmlNode orgIdNode1 = doc.CreateElement("orgId1");
                            orgIdNode1.AppendChild(doc.CreateTextNode(dtr["orgId1"].ToString()));
                            prjNode.AppendChild(orgIdNode1);
                        }

                        if (dtr["orgName2"].ToString() != "")
                        {
                            XmlNode orgNameNode2 = doc.CreateElement("orgName2");
                            orgNameNode2.AppendChild(doc.CreateTextNode(dtr["orgName2"].ToString()));
                            prjNode.AppendChild(orgNameNode2);
                        }

                        if (dtr["orgId2"].ToString() != "")
                        {
                            XmlNode orgIdNode2 = doc.CreateElement("orgId2");
                            orgIdNode2.AppendChild(doc.CreateTextNode(dtr["orgId2"].ToString()));
                            prjNode.AppendChild(orgIdNode2);
                        }

                        if (dtr["orgName3"].ToString() != "")
                        {
                            XmlNode orgNameNode3 = doc.CreateElement("orgName3");
                            orgNameNode3.AppendChild(doc.CreateTextNode(dtr["orgName3"].ToString()));
                            prjNode.AppendChild(orgNameNode3);
                        }

                        if (dtr["orgId3"].ToString() != "")
                        {
                            XmlNode orgIdNode3 = doc.CreateElement("orgId3");
                            orgIdNode3.AppendChild(doc.CreateTextNode(dtr["orgId3"].ToString()));
                            prjNode.AppendChild(orgIdNode3);
                        }

                        if (dtr["science"].ToString() != "")
                        {
                            XmlNode scienceNode = doc.CreateElement("science");
                            scienceNode.AppendChild(doc.CreateTextNode(dtr["science"].ToString()));
                            prjNode.AppendChild(scienceNode);

                            XmlAttribute scienceCodeAtt = doc.CreateAttribute("scienceCode");
                            scienceCodeAtt.Value = dtr["scienceCode"].ToString();
                            scienceNode.Attributes.Append(scienceCodeAtt);
                        }

                        if (dtr["field"].ToString() != "")
                        {
                            XmlNode fieldNode = doc.CreateElement("field");
                            fieldNode.AppendChild(doc.CreateTextNode(dtr["field"].ToString()));
                            prjNode.AppendChild(fieldNode);

                            XmlAttribute fieldCodeAtt = doc.CreateAttribute("fieldCode");
                            fieldCodeAtt.Value = dtr["fieldCode"].ToString();
                            fieldNode.Attributes.Append(fieldCodeAtt);
                        }

                        if (dtr["subfield"].ToString() != "")
                        {
                            XmlNode subfieldNode = doc.CreateElement("subfield");
                            subfieldNode.AppendChild(doc.CreateTextNode(dtr["subfield"].ToString()));
                            prjNode.AppendChild(subfieldNode);

                            XmlAttribute subfieldCodeAtt = doc.CreateAttribute("subfieldCode");
                            subfieldCodeAtt.Value = dtr["subfieldCode"].ToString();
                            subfieldNode.Attributes.Append(subfieldCodeAtt);
                        }

                        if (dtr["tell"].ToString() != "")
                        {
                            XmlNode telNode = doc.CreateElement("tel");
                            telNode.AppendChild(doc.CreateTextNode(dtr["tell"].ToString()));
                            prjNode.AppendChild(telNode);
                        }

                        if (dtr["fax"].ToString() != "")
                        {
                            XmlNode faxNode = doc.CreateElement("fax");
                            faxNode.AppendChild(doc.CreateTextNode(dtr["fax"].ToString()));
                            prjNode.AppendChild(faxNode);
                        }

                        if (dtr["email"].ToString() != "")
                        {
                            XmlNode emailNode = doc.CreateElement("email");
                            emailNode.AppendChild(doc.CreateTextNode(dtr["email"].ToString()));
                            prjNode.AppendChild(emailNode);
                        }

                        if (dtr["url"].ToString() != "")
                        {
                            XmlNode urlNode = doc.CreateElement("url");
                            urlNode.AppendChild(doc.CreateTextNode(dtr["url"].ToString()));
                            prjNode.AppendChild(urlNode);
                        }
                    }
                }
                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }

            return doc;
        }

        [WebMethod]
        public XmlDocument RsrByIds(string[] ids)
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            string queryText = "select id, mstid, firstName, lastName, status, keyws, tell, fax, email, url, science as scienceCode, field as fieldCode, subfield as subfieldCode,(select name from tblOrganizations where id IN(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId)) as orgName,(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId) as orgId, (select tblScienceCodes.description from tblScienceCodes where tblResearchers.science = tblScienceCodes.scienceId) as science,(select tblFieldCodes.description from tblFieldCodes where tblResearchers.science = tblFieldCodes.scienceId and tblFieldCodes.fieldId = field) as field,(select tblSubfieldCodes.description from tblSubfieldCodes where science = tblSubfieldCodes.scienceId and tblSubfieldCodes.fieldId = field and tblSubfieldCodes.subfieldId = subfield) as subfield from  tblResearchers where ";  

            for (int i = 0; i < ids.Length; i++)
            {
                if (i>0) queryText += " or ";
                queryText += "id = " + ids[i].ToString();
            }
            queryText += " order by lastName;";
            
            try
                {
                    SqlCommand cmd = new SqlCommand(queryText, con);
                    con.Open();
                    SqlDataReader dtr = cmd.ExecuteReader();

                    if (dtr.FieldCount > 0)
                    {

                        XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                        doc.AppendChild(docNode);

                        XmlNode researchersNode = doc.CreateElement("Researchers");
                        doc.AppendChild(researchersNode);

                        while (dtr.Read())
                        {
                            XmlNode prjNode = doc.CreateElement("RSR");
                            researchersNode.AppendChild(prjNode);

                            //////////////////attributes/////////////////////

                            XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                            mstidAtt.Value = dtr["mstid"].ToString();
                            prjNode.Attributes.Append(mstidAtt);

                            XmlAttribute idAtt = doc.CreateAttribute("id");
                            idAtt.Value = dtr["id"].ToString();
                            prjNode.Attributes.Append(idAtt);

                            ///////////////////elements////////////////////////

                            XmlNode firstnameNode = doc.CreateElement("firstName");
                            firstnameNode.AppendChild(doc.CreateTextNode(dtr["firstName"].ToString()));
                            prjNode.AppendChild(firstnameNode);

                            XmlNode lastNameNode = doc.CreateElement("lastName");
                            lastNameNode.AppendChild(doc.CreateTextNode(dtr["lastName"].ToString()));
                            prjNode.AppendChild(lastNameNode);

                            if (dtr["keyws"].ToString() != "")
                            {
                                XmlNode keywsNode = doc.CreateElement("keyws");
                                keywsNode.AppendChild(doc.CreateTextNode(dtr["keyws"].ToString()));
                                prjNode.AppendChild(keywsNode);
                            }

                            if (dtr["orgName"].ToString() != "")
                            {
                                XmlNode orgNameNode = doc.CreateElement("orgName");
                                orgNameNode.AppendChild(doc.CreateTextNode(dtr["orgName"].ToString()));
                                prjNode.AppendChild(orgNameNode);
                            }

                            if (dtr["orgId"].ToString() != "")
                            {
                                XmlNode orgIdNode = doc.CreateElement("orgId");
                                orgIdNode.AppendChild(doc.CreateTextNode(dtr["orgId"].ToString()));
                                prjNode.AppendChild(orgIdNode);
                            }

                            if (dtr["science"].ToString() != "")
                            {
                                XmlNode scienceNode = doc.CreateElement("science");
                                scienceNode.AppendChild(doc.CreateTextNode(dtr["science"].ToString()));
                                prjNode.AppendChild(scienceNode);

                                XmlAttribute scienceCodeAtt = doc.CreateAttribute("scienceCode");
                                scienceCodeAtt.Value = dtr["scienceCode"].ToString();
                                scienceNode.Attributes.Append(scienceCodeAtt);
                            }

                            if (dtr["field"].ToString() != "")
                            {
                                XmlNode fieldNode = doc.CreateElement("field");
                                fieldNode.AppendChild(doc.CreateTextNode(dtr["field"].ToString()));
                                prjNode.AppendChild(fieldNode);

                                XmlAttribute fieldCodeAtt = doc.CreateAttribute("fieldCode");
                                fieldCodeAtt.Value = dtr["fieldCode"].ToString();
                                fieldNode.Attributes.Append(fieldCodeAtt);
                            }

                            if (dtr["subfield"].ToString() != "")
                            {
                                XmlNode subfieldNode = doc.CreateElement("subfield");
                                subfieldNode.AppendChild(doc.CreateTextNode(dtr["subfield"].ToString()));
                                prjNode.AppendChild(subfieldNode);

                                XmlAttribute subfieldCodeAtt = doc.CreateAttribute("subfieldCode");
                                subfieldCodeAtt.Value = dtr["subfieldCode"].ToString();
                                subfieldNode.Attributes.Append(subfieldCodeAtt);
                            }

                            if (dtr["tell"].ToString() != "")
                            {
                                XmlNode telNode = doc.CreateElement("tel");
                                telNode.AppendChild(doc.CreateTextNode(dtr["tell"].ToString()));
                                prjNode.AppendChild(telNode);
                            }

                            if (dtr["fax"].ToString() != "")
                            {
                                XmlNode faxNode = doc.CreateElement("fax");
                                faxNode.AppendChild(doc.CreateTextNode(dtr["fax"].ToString()));
                                prjNode.AppendChild(faxNode);
                            }

                            if (dtr["email"].ToString() != "")
                            {
                                XmlNode emailNode = doc.CreateElement("email");
                                emailNode.AppendChild(doc.CreateTextNode(dtr["email"].ToString()));
                                prjNode.AppendChild(emailNode);
                            }

                            if (dtr["url"].ToString() != "")
                            {
                                XmlNode urlNode = doc.CreateElement("url");
                                urlNode.AppendChild(doc.CreateTextNode(dtr["url"].ToString()));
                                prjNode.AppendChild(urlNode);
                            }
                        }
                    }
                    con.Close();
                }
                catch { }
                finally
                {
                    con.Close();
                }

            return doc;
        }

        [WebMethod]
        public XmlDocument RsrByIds1(string[] ids)
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            string queryText = "select id, mstid, firstName, lastName, status, keyws, tell, fax, email, url, science as scienceCode, field as fieldCode, subfield as subfieldCode, (select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId) as orgId1,( select name from tblOrganizations where id IN( select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId)) as orgName1,(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId and tblRsrIsinOrg.orgId not in (select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId order by tblRsrIsinOrg.orgId) order by tblRsrIsinOrg.orgId) as orgId2, (select name from tblOrganizations where id  IN(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId and tblRsrIsinOrg.orgId not in (select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId order by tblRsrIsinOrg.orgId)order by tblRsrIsinOrg.orgId)) as orgName2,(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId and tblRsrIsinOrg.orgId not in (select TOP 2 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId order by tblRsrIsinOrg.orgId) order by tblRsrIsinOrg.orgId) as orgId3,(select name from tblOrganizations where id  IN(select TOP 1 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId and tblRsrIsinOrg.orgId not in (select TOP 2 tblRsrIsinOrg.orgId from tblRsrIsinOrg where tblResearchers.id = tblRsrIsinOrg.rsrId order by tblRsrIsinOrg.orgId)order by tblRsrIsinOrg.orgId)) as orgName3, (select tblScienceCodes.description from tblScienceCodes where tblResearchers.science = tblScienceCodes.scienceId) as science,(select tblFieldCodes.description from tblFieldCodes where tblResearchers.science = tblFieldCodes.scienceId and tblFieldCodes.fieldId = field) as field,(select tblSubfieldCodes.description from tblSubfieldCodes where science = tblSubfieldCodes.scienceId and tblSubfieldCodes.fieldId = field and tblSubfieldCodes.subfieldId = subfield) as subfield from  tblResearchers where ";  

            for (int i = 0; i < ids.Length; i++)
            {
                if (i>0) queryText += " or ";
                queryText += "id = " + ids[i].ToString();
            }
            queryText += " order by lastName;";
            
            try
                {
                    SqlCommand cmd = new SqlCommand(queryText, con);
                    con.Open();
                    SqlDataReader dtr = cmd.ExecuteReader();

                    if (dtr.FieldCount > 0)
                    {

                        XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                        doc.AppendChild(docNode);

                        XmlNode researchersNode = doc.CreateElement("Researchers");
                        doc.AppendChild(researchersNode);

                        while (dtr.Read())
                        {
                            XmlNode prjNode = doc.CreateElement("RSR");
                            researchersNode.AppendChild(prjNode);

                            //////////////////attributes/////////////////////

                            XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                            mstidAtt.Value = dtr["mstid"].ToString();
                            prjNode.Attributes.Append(mstidAtt);

                            XmlAttribute idAtt = doc.CreateAttribute("id");
                            idAtt.Value = dtr["id"].ToString();
                            prjNode.Attributes.Append(idAtt);

                            ///////////////////elements////////////////////////

                            XmlNode firstnameNode = doc.CreateElement("firstName");
                            firstnameNode.AppendChild(doc.CreateTextNode(dtr["firstName"].ToString()));
                            prjNode.AppendChild(firstnameNode);

                            XmlNode lastNameNode = doc.CreateElement("lastName");
                            lastNameNode.AppendChild(doc.CreateTextNode(dtr["lastName"].ToString()));
                            prjNode.AppendChild(lastNameNode);

                            if (dtr["keyws"].ToString() != "")
                            {
                                XmlNode keywsNode = doc.CreateElement("keyws");
                                keywsNode.AppendChild(doc.CreateTextNode(dtr["keyws"].ToString()));
                                prjNode.AppendChild(keywsNode);
                            }

                            if (dtr["orgId1"].ToString() != "")
                            {
                                XmlNode orgIdNode1 = doc.CreateElement("orgId1");
                                orgIdNode1.AppendChild(doc.CreateTextNode(dtr["orgId1"].ToString()));
                                prjNode.AppendChild(orgIdNode1);
                            }

                            if (dtr["orgName1"].ToString() != "")
                            {
                                XmlNode orgNameNode1 = doc.CreateElement("orgName1");
                                orgNameNode1.AppendChild(doc.CreateTextNode(dtr["orgName1"].ToString()));
                                prjNode.AppendChild(orgNameNode1);
                            }

                            if (dtr["orgName2"].ToString() != "")
                            {
                                XmlNode orgNameNode2 = doc.CreateElement("orgName2");
                                orgNameNode2.AppendChild(doc.CreateTextNode(dtr["orgName2"].ToString()));
                                prjNode.AppendChild(orgNameNode2);
                            }

                            if (dtr["orgId2"].ToString() != "")
                            {
                                XmlNode orgIdNode2 = doc.CreateElement("orgId2");
                                orgIdNode2.AppendChild(doc.CreateTextNode(dtr["orgId2"].ToString()));
                                prjNode.AppendChild(orgIdNode2);
                            }

                            if (dtr["orgName3"].ToString() != "")
                            {
                                XmlNode orgNameNode3 = doc.CreateElement("orgName3");
                                orgNameNode3.AppendChild(doc.CreateTextNode(dtr["orgName3"].ToString()));
                                prjNode.AppendChild(orgNameNode3);
                            }

                            if (dtr["orgId3"].ToString() != "")
                            {
                                XmlNode orgIdNode3 = doc.CreateElement("orgId3");
                                orgIdNode3.AppendChild(doc.CreateTextNode(dtr["orgId3"].ToString()));
                                prjNode.AppendChild(orgIdNode3);
                            }

                            if (dtr["science"].ToString() != "")
                            {
                                XmlNode scienceNode = doc.CreateElement("science");
                                scienceNode.AppendChild(doc.CreateTextNode(dtr["science"].ToString()));
                                prjNode.AppendChild(scienceNode);

                                XmlAttribute scienceCodeAtt = doc.CreateAttribute("scienceCode");
                                scienceCodeAtt.Value = dtr["scienceCode"].ToString();
                                scienceNode.Attributes.Append(scienceCodeAtt);
                            }

                            if (dtr["field"].ToString() != "")
                            {
                                XmlNode fieldNode = doc.CreateElement("field");
                                fieldNode.AppendChild(doc.CreateTextNode(dtr["field"].ToString()));
                                prjNode.AppendChild(fieldNode);

                                XmlAttribute fieldCodeAtt = doc.CreateAttribute("fieldCode");
                                fieldCodeAtt.Value = dtr["fieldCode"].ToString();
                                fieldNode.Attributes.Append(fieldCodeAtt);
                            }

                            if (dtr["subfield"].ToString() != "")
                            {
                                XmlNode subfieldNode = doc.CreateElement("subfield");
                                subfieldNode.AppendChild(doc.CreateTextNode(dtr["subfield"].ToString()));
                                prjNode.AppendChild(subfieldNode);

                                XmlAttribute subfieldCodeAtt = doc.CreateAttribute("subfieldCode");
                                subfieldCodeAtt.Value = dtr["subfieldCode"].ToString();
                                subfieldNode.Attributes.Append(subfieldCodeAtt);
                            }

                            if (dtr["tell"].ToString() != "")
                            {
                                XmlNode telNode = doc.CreateElement("tel");
                                telNode.AppendChild(doc.CreateTextNode(dtr["tell"].ToString()));
                                prjNode.AppendChild(telNode);
                            }

                            if (dtr["fax"].ToString() != "")
                            {
                                XmlNode faxNode = doc.CreateElement("fax");
                                faxNode.AppendChild(doc.CreateTextNode(dtr["fax"].ToString()));
                                prjNode.AppendChild(faxNode);
                            }

                            if (dtr["email"].ToString() != "")
                            {
                                XmlNode emailNode = doc.CreateElement("email");
                                emailNode.AppendChild(doc.CreateTextNode(dtr["email"].ToString()));
                                prjNode.AppendChild(emailNode);
                            }

                            if (dtr["url"].ToString() != "")
                            {
                                XmlNode urlNode = doc.CreateElement("url");
                                urlNode.AppendChild(doc.CreateTextNode(dtr["url"].ToString()));
                                prjNode.AppendChild(urlNode);
                            }
                        }
                    }
                    con.Close();
                }
                catch { }
                finally
                {
                    con.Close();
                }

            return doc;
        }

        [WebMethod]
        public XmlDocument PrjByRsrIds(string[] ids) 
        {
            XmlDocument doc = new XmlDocument();
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());

            string queryText = "select * from tblProjects where id IN( select prjId from tblRsrHasPrj where rsrId IN( select id from tblResearchers where ";
            for (int i = 0; i < ids.Length; i++)
            {
                if (i > 0) queryText += " or ";
                queryText += "id = " + ids[i].ToString();
            }
            queryText += ") ) order by name;";

            try
            {
                SqlCommand cmd = new SqlCommand(queryText, con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();

                XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(docNode);

                XmlNode projectsNode = doc.CreateElement("Projects");
                doc.AppendChild(projectsNode);

                while (dtr.Read())
                {
                    XmlNode prjNode = doc.CreateElement("PRJ");
                    projectsNode.AppendChild(prjNode);

                    //////////////////attributes/////////////////////

                    XmlAttribute mstidAtt = doc.CreateAttribute("mstid");
                    mstidAtt.Value = dtr["mstid"].ToString();
                    prjNode.Attributes.Append(mstidAtt);

                    XmlAttribute idAtt = doc.CreateAttribute("id");
                    idAtt.Value = dtr["id"].ToString();
                    prjNode.Attributes.Append(idAtt);

                    XmlAttribute startdate = doc.CreateAttribute("startdate");
                    startdate.Value = dtr["startdate"].ToString();
                    prjNode.Attributes.Append(startdate);

                    XmlAttribute enddate = doc.CreateAttribute("enddate");
                    enddate.Value = dtr["enddate"].ToString();
                    prjNode.Attributes.Append(enddate);

                    ///////////////////elements////////////////////////

                    XmlNode nameNode = doc.CreateElement("name");
                    nameNode.AppendChild(doc.CreateTextNode(dtr["name"].ToString()));
                    prjNode.AppendChild(nameNode);

                    XmlNode abstractNode = doc.CreateElement("abstract");
                    abstractNode.AppendChild(doc.CreateTextNode(dtr["abstract"].ToString()));
                    prjNode.AppendChild(abstractNode);

                    XmlNode keywsNode = doc.CreateElement("keyws");
                    keywsNode.AppendChild(doc.CreateTextNode(dtr["keyws"].ToString()));
                    prjNode.AppendChild(keywsNode);

                    XmlNode signWorldNode = doc.CreateElement("worldSign");
                    signWorldNode.AppendChild(doc.CreateTextNode(dtr["sign_world"].ToString()));
                    prjNode.AppendChild(signWorldNode);

                    XmlNode signDomNode = doc.CreateElement("domSign");
                    signDomNode.AppendChild(doc.CreateTextNode(dtr["sign_dom"].ToString()));
                    prjNode.AppendChild(signDomNode);
                }

                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }

            return doc;
        }

        [WebMethod]
        public void CreateIndex(string indName) {

            directory = FSDirectory.GetDirectory("C://"+indName);
            analyzer = new StandardAnalyzer();
            writer = new IndexWriter(directory, analyzer);

            red = IndexReader.Open(directory);
            totDocs = red.MaxDoc();
            red.Close();
            
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());
            SqlConnection con1 = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["TopSecret"].ToString());
            try
            {
                //SqlCommand cmd = new SqlCommand("SELECT id, firstName, lastName FROM dbo.tblResearchers WHERE firstName like 'd%' and lastName like 'm%';", con);
                SqlCommand cmd = new SqlCommand("SELECT id, firstName, lastName, keyws FROM dbo.tblResearchers;", con);
                con.Open();
                SqlDataReader dtr = cmd.ExecuteReader();

                while (dtr.Read())
                {
                    Document doc = new Document();

                    doc.Add(new Lucene.Net.Documents.Field("idrsr", dtr["id"].ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.NO));

                    Lucene.Net.Documents.Field firstNameField = new Lucene.Net.Documents.Field("firstName", dtr["firstName"].ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.UN_TOKENIZED);
                    firstNameField.SetBoost(8.0f);
                    doc.Add(firstNameField);

                    Lucene.Net.Documents.Field lastNameField = new Lucene.Net.Documents.Field("lastName", dtr["lastName"].ToString(), Lucene.Net.Documents.Field.Store.YES, Lucene.Net.Documents.Field.Index.UN_TOKENIZED);
                    lastNameField.SetBoost(8.0f);
                    doc.Add(lastNameField);

                    Lucene.Net.Documents.Field keywsRsrField = new Lucene.Net.Documents.Field("keywsRsr", dtr["keyws"].ToString(), Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.TOKENIZED);
                    keywsRsrField.SetBoost(6.0f);
                    doc.Add(keywsRsrField);

                    SqlCommand cmd1 = new SqlCommand("select * from tblProjects where id IN( select prjId from tblRsrHasPrj where rsrId IN( select id from tblResearchers where id = '" + dtr["id"].ToString() + "') ) order by name;", con1);
                    con1.Open();
                    SqlDataReader dtr1 = cmd1.ExecuteReader();

                    string title="", abst="", keyws="", sign_dom="", sign_world="";

                        while (dtr1.Read()) 
                        {
                            title += dtr1["name"].ToString() + " ";
                            abst += dtr1["abstract"].ToString() + " ";
                            keyws += dtr1["keyws"].ToString() + " ";
                            sign_dom += dtr1["sign_world"].ToString() + " ";
                            sign_world += dtr1["sign_dom"].ToString() + " ";
                        }

                        Lucene.Net.Documents.Field projectTitleField = new Lucene.Net.Documents.Field("projectTitle", title, Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.TOKENIZED);
                        projectTitleField.SetBoost(4.0f);
                        doc.Add(projectTitleField);

                        doc.Add(new Lucene.Net.Documents.Field("abstract", abst, Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.TOKENIZED, Lucene.Net.Documents.Field.TermVector.YES));
                        doc.Add(new Lucene.Net.Documents.Field("keyws", keyws, Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.TOKENIZED, Lucene.Net.Documents.Field.TermVector.YES));
                        doc.Add(new Lucene.Net.Documents.Field("sign_world", sign_dom, Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.TOKENIZED, Lucene.Net.Documents.Field.TermVector.YES));
                        doc.Add(new Lucene.Net.Documents.Field("sign_dom", sign_world, Lucene.Net.Documents.Field.Store.NO, Lucene.Net.Documents.Field.Index.TOKENIZED, Lucene.Net.Documents.Field.TermVector.YES));

                    writer.AddDocument(doc);
                    
                    dtr1.Close();
                    con1.Close();

                }

                writer.Optimize();
                //Close the writer
                writer.Flush();
                writer.Close();
                
                con.Close();
            }
            catch { }
            finally
            {
                con.Close();
            }
        }
    }
}
        