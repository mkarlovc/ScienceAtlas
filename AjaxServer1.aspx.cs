using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using basicWeb.SicrisWS;
using NetDocAtlasLib;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System.Text.RegularExpressions;
using System.Text;

namespace basicWeb
{
    public partial class AjaxServer1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                SicrisWS.Service1 cd = new SicrisWS.Service1();

                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "text/json";

                string d = Request["data"];
                string language = Request["language"];
                string method = Request["method"];

                if (method == "getNews") { 
                    
                    using (var webClient = new System.Net.WebClient())
                    {
                        string data = webClient.DownloadString(d);
                        string data1 = parseJsonNews(data);
                        Response.Write(data1);
                    }

                }
                if (method == "collCenter") {
                    string data = cd.CollaborationOfRsrOnPrj1Json(d);
                    string data2 = parseJsonCircle(data);
                    string data3 = parseJsonCircleGraph(data);
                    Response.Write("[" + data2 + "," + data3 + "]");
                }
                if (method == "collMap")
                {
                    string data = cd.CollaborationOfRsrOnPrj1Json(d);
                    string data2 = parseJsonAtlasCoordinates(data);
                    string data1 = parseJsonAlltoallClosed(data);
                    data2 = parseJsonVd(data2, data1);
                    Response.Write("[" + data2 + "," + data1 + "]");
                }
                if (method == "collMapArray") {
                    string[] strArrQuerystring = d.Split(',');
                    string data = parseJsonAtlasCoordinates(cd.RsrByIdsJson(strArrQuerystring));
                    string data1 = parseJsonAlltoallClosed(data);
                    string data2 = cd.OrgByRsrIdsJson(strArrQuerystring);
                    string[] data3 = parseJsonAlltoAllClosedOrg(data,data2,data1);
                    data = parseJsonVd(data, data1);
                    Response.Write("[" + data + "," + data1 + ","+ data3[1] + ","+ data3[0] +"]");
                }
                if (method == "prjCollArray")
                {
                    string data0 = cd.RsrOnPrjJson(d);
                    string data = parseJsonAtlasCoordinates(data0);
                    string data1 = parseJsonAlltoallClosed(data);
                    string data2 = cd.OrgByRsrIdsJson(parseReturnRsrIdString(data0));
                    string[] data3 = parseJsonAlltoAllClosedOrg(data, data2, data1);
                    string data4 = cd.PrjByIdJson(d);
                    data = parseJsonVd(data, data1);
                    Response.Write("[" + data + "," + data1 + "," + data3[1] + "," + data3[0] + "," + data4 + "]");
                }
                if (method == "orgCollArray")
                {
                    string data0 = cd.RsrInOrgJson(d);
                    string data = parseJsonAtlasCoordinates(data0);
                    string data1 = parseJsonAlltoallClosed(data);
                    string data2 = cd.OrgByRsrIdsJson(parseReturnRsrIdString(data0));
                    string[] data3 = parseJsonAlltoAllClosedOrg(data, data2, data1);
                    string data4 = cd.OrgByIdJson(d);
                    data = parseJsonVd(data, data1);
                    Response.Write("[" + data + "," + data1 + "," + data3[1] + "," + data3[0] + "," + data4 + "]");
                }
                if (method == "rsrCollArray")
                {
                    string[] strArrQuerystring = d.Split(',');
                    string data = cd.CollaborationOfRsrOnPrj1Json(strArrQuerystring[0]);
                    string data1 = parseJsonCircle(data);
                    string data2 = parseJsonCircleGraph(data1);

                    string data3 = parseJsonAtlasCoordinates(data);
                    string data4 = parseJsonAlltoallClosed(data3);
                    string[] s = toRsrIdString(data);
                    string data5 = cd.OrgByRsrIdsJson(s);
                    string[] data6 = parseJsonAlltoAllClosedOrg(data3, data5, data4);
                    data3 = parseJsonVd(data3, data4);
                    Response.Write("[" + data1 + "," + data2 + "," + data3 + "," + data4 +"," +data6[1] + "," + data6[0] + "]");
                }
                if (method == "rsrColl")
                {
                    string data = cd.CollaborationOfRsrOnPrj1Json(d);
                    string data1 = parseJsonCircle(data);
                    string data2 = parseJsonCircleGraph(data1);

                    string data3 = parseJsonAtlasCoordinates(data);
                    string data4 = parseJsonAlltoallClosed(data3);

                    string data5 = cd.OrgByRsrIdsJson(new string[]{d});
                    string[] data6 = parseJsonAlltoAllClosedOrg(data3, data5, data4);
                    data3 = parseJsonVd(data3, data4);
                    Response.Write("[" + data1 + "," + data2 + "," + data3 + "," + data4 + "," + data6[1] + "," + data6[0] + "]");
                }
                if (method == "collOrgArray")
                {
                    string[] strArrQuerystring = d.Split(',');
                    string data = parseJsonAtlasCoordinates(cd.RsrByIdsJson(strArrQuerystring));
                    string data1 = parseJsonAlltoallClosed(data);
                    string data2 = cd.OrgByRsrIdsJson(strArrQuerystring);
                    string[] data3 = parseJsonAlltoAllClosedOrg(data, data2, data1);
                    data = parseJsonVd(data, data1);
                    Response.Write("[" +data3[1] + "," + data3[0] + "]");
                }
                if (method == "compMapArray" || method == "compMapArraySearch")
                {
                    string[] strArrQuerystring = d.Split(',');
                    string data = cd.PrjByIdsJson(strArrQuerystring);
                    string[] data1 = parseJsonAtlasCoordinatesKeywsConnectedPrj(data,language);
                    Response.Write("[" + data1[0] + "," + data1[1] + "," + data1[2] + "]");
                }
                if (method == "compMapSearch")
                {
                    string[] strArrQuerystring = d.Split(',');
                    string data = cd.PrjByIdsJson(strArrQuerystring);
                    string[] data1 = parseJsonAtlasCoordinatesKeywsConnectedPrj(data, language);
                    Response.Write("[" + data1[0] + "," + data1[1] + "," + data1[2] + "]");
                }
                if (method == "rsrCompMap")
                {
                    string data = cd.PrjOfRsrJson(d);
                    string[] data1 = parseJsonAtlasCoordinatesKeywsConnectedPrj(data, language);
                    string data2 = cd.ProfileOfRsrJson(d);
                    Response.Write("[" + data1[0] + "," + data1[1] + "," + data1[2] + "," + data2 + "]");
                }
                if (method == "orgCompMap")
                {
                    string data = cd.PrjOfOrgJson(d);
                    string[] data1 = parseJsonAtlasCoordinatesKeywsConnectedPrj(data, language);
                    string data2 = cd.OrgByIdJson(d);
                    Response.Write("[" + data1[0] + "," + data1[1] + "," + data1[2] + "," + data2 + "]");
                }
                if (method == "collCenterArray")
                {
                    string data1 = cd.CollaborationOfRsrOnPrj1Json(d);
                    string data2 = parseJsonCircle(data1);
                    string data3 = parseJsonCircleGraph(data2);
                    Response.Write("[" + data2 + "," + data3 + "]");
                }
                if (method == "query")
                {
                    try
                    {
                        string data = searchRsr(d);
                        string data1 = searchPrj(d);
                        string data2 = searchOrg(d);
                        Response.Write("[" + data + "," + data1 + "," + data2 + "]");
                    }
                    catch { Response.Write("[[],[],[]]"); }
                }
                if (method == "profileOrg")
                {
                    string data = cd.RsrInOrgJson(d);
                    string data1 = cd.PrjOfOrgJson(d);
                    string data2 = cd.OrgByIdJson(d);
                    Response.Write("[" + data + "," + data1 + "," + data2 + "]");
                }
                if (method == "profilePrj") 
                {
                    string data = cd.RsrOnPrjJson(d);
                    string data1 = cd.PrjByIdJson(d);
                    Response.Write("[" + data + "," + data1 + "]");
                }
                if (method == "profileRsr")
                {
                    string data = cd.CollaborationOfRsrOnPrj1Json(d);
                    string data3 = cd.ProfileOfRsrJson(d);
                    string data1 = cd.PrjOfRsrJson(d);
                    data = parseJsonCircle(data);
                    string data2 = parseJsonCircleGraph(data);
                    string data4 = cd.LecOfRsrJson(d);
                    Response.Write("[" + data + "," + data1 + "," + data2 + "," + data3 + "," + data4 + "]");
                }
                Response.End();
            }

        }

        /*
         * Functions that searches for researchers from the database
         */
        private string searchRsr(string data) { 

            Directory directory;
            Analyzer analyzer;

            int totDocs;
            IndexReader indexReader;
            Searcher indexSearcher;

            IndexSearcher searcher;

            String rootPath = Server.MapPath("~");
            directory = FSDirectory.GetDirectory(rootPath + "/index\\rsrIndex");

            string[] searchfields = new string[] { "name", "name", "keywsRsr" };
            
            string[] resultfields = new string[] { "idrsr", "firstName", "lastName","keywsRsr" };

            analyzer = new StandardAnalyzer();

            //data = RemoveSpecialCharacters(data);
            data = data.Trim();

            indexReader = IndexReader.Open(directory);
            totDocs = indexReader.MaxDoc();
            indexSearcher = new IndexSearcher(indexReader);

            string strVal = replacePalatals(data);

            //Setup searcher
            searcher = new IndexSearcher(directory);

            QueryParser queryParserKeywords = new QueryParser(searchfields[2], analyzer);
            MultiFieldQueryParser queryParserName = new MultiFieldQueryParser(new string[] { searchfields[0], searchfields[1] }, analyzer);

            Hits hits = searcher.Search(queryParserName.Parse(searchfields[0] + ":" + strVal.Replace(" ", " AND ")));

            //Display results

            List<Rsr> rsr = new List<Rsr>();

            for (int i = 0; i < hits.Length(); i++)
            {   
                Document doc = hits.Doc(i);
                rsr.Add(new Rsr(doc.Get(resultfields[0]), "", doc.Get(resultfields[1]), doc.Get(resultfields[2]), "", "", "","", "","","", "", "", "", "", doc.Get(resultfields[3]), "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }

            Hits hits1 = searcher.Search(queryParserKeywords.Parse(searchfields[2] + ":" + strVal.Replace(" ", " AND ")));

            for (int i = 0; i < hits1.Length(); i++)
            {
                Document doc = hits1.Doc(i);
                rsr.Add(new Rsr(doc.Get(resultfields[0]), "", doc.Get(resultfields[1]), doc.Get(resultfields[2]), "", "", "", "", "","","","", "", "", "", doc.Get(resultfields[3]), "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(rsr);
        }

        /*
         * Functions that searches for projects from the database
         */
        private string searchPrj(string data)
        {

            Directory directory;
            Analyzer analyzer;

            int totDocs;
            IndexReader indexReader;
            Searcher indexSearcher;

            IndexSearcher searcher;

            String rootPath = Server.MapPath("~");
            directory = FSDirectory.GetDirectory(rootPath + "/index\\prjIndex");

            string[] searchfields = new string[] { "all_desc" };

            string[] resultfields = new string[] { "idprj", "name", "startdate", "enddate", "keyws", "abstract" };

            analyzer = new StandardAnalyzer();

            //data = RemoveSpecialCharacters(data);
            data = data.Trim();

            indexReader = IndexReader.Open(directory);
            totDocs = indexReader.MaxDoc();
            indexSearcher = new IndexSearcher(indexReader);

            string strVal = "";

            for (int i = 0; i < searchfields.Length; i++)
            {
                strVal += searchfields[i] + ":" + data.Replace(" ", " AND ") + " ";
                if (i < searchfields.Length - 1)
                    strVal += "OR ";
            }

            //Setup searcher
            searcher = new IndexSearcher(directory);

            MultiFieldQueryParser queryParserName = new MultiFieldQueryParser(searchfields, analyzer);

            Hits hits = searcher.Search(queryParserName.Parse(strVal));

            //Display results

            List<Prj> prj = new List<Prj>();

            for (int i = 0; i < hits.Length(); i++)
            {
                Document doc = hits.Doc(i);
                prj.Add(new Prj(doc.Get(resultfields[0]), "", doc.Get(resultfields[1]), doc.Get(resultfields[1]), 0, doc.Get(resultfields[2]), doc.Get(resultfields[3]), doc.Get(resultfields[4]), "", doc.Get(resultfields[5]), "", "", "", "", "", "", "","","","","","","","","","", 0.0));
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(prj);
        }

        /*
         * Functions that searches for organizations from the database
         */
        private string searchOrg(string data)
        {

            Directory directory;
            Analyzer analyzer;

            int totDocs;
            IndexReader indexReader;
            Searcher indexSearcher;

            IndexSearcher searcher;

            String rootPath = Server.MapPath("~");
            directory = FSDirectory.GetDirectory(rootPath + "/index\\orgIndex");

            string[] searchfields = new string[] { "name" };

            string[] resultfields = new string[] { "idorg", "name", "city" };

            analyzer = new StandardAnalyzer();

            //data = RemoveSpecialCharacters(data);
            data = data.Trim();

            indexReader = IndexReader.Open(directory);
            totDocs = indexReader.MaxDoc();
            indexSearcher = new IndexSearcher(indexReader);

            string strVal = "";

            for (int i = 0; i < searchfields.Length; i++)
            {
                strVal += searchfields[i] + ":" + data.Replace(" ", " AND ") + " ";
                if (i < searchfields.Length - 1)
                    strVal += "OR ";
            }

            //Setup searcher
            searcher = new IndexSearcher(directory);

            MultiFieldQueryParser queryParserName = new MultiFieldQueryParser(searchfields, analyzer);

            Hits hits = searcher.Search(queryParserName.Parse(strVal));

            //Display results

            List<Org> org = new List<Org>();

            string[] stringArray = { "d.o.o.", "d.d." };
            string type = "";

            for (int i = 0; i < hits.Length(); i++)
            {
                Document doc = hits.Doc(i);
                if (stringArray.Any(s => (doc.Get(resultfields[1]).ToLower()).Contains(s)))
                    type = "b";
                else
                    type = "r";

                org.Add(new Org(doc.Get(resultfields[0]), "", doc.Get(resultfields[1]), type, "", doc.Get(resultfields[2]),"","","",0));
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(org);

        }

        /* Generating graph with all the connections between nodes that collaborate with a choosen node.
         * IDs of edges are IDs of researchers
         */
        private string parseJsonAlltoallClosedId(string data)
        {
            SicrisWS.Service1 cd = new SicrisWS.Service1();
            List<Rsr> myDeserializedObjList = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Rsr>));
            List<Graph> g = new List<Graph>();
            bool newEdge = true;
            string tmpData = "";

            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            for (int i = 0; i < myDeserializedObjList.Count; i++)
            {

                tmpData = cd.CollaborationOfRsrOnPrj1Json(myDeserializedObjList[i].Id);
                List<Rsr> tmpDeserialized = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(tmpData, typeof(List<Rsr>));

                for (int j = 1; j < tmpDeserialized.Count; j++)
                {

                    if (myDeserializedObjList.Any(item => item.Id == tmpDeserialized[j].Id))
                    {
                        for (int k = 0; k < g.Count; k++)
                        {
                            if ((g[k].N1 == tmpDeserialized[0].Id && g[k].N2 == tmpDeserialized[j].Id) || (g[k].N2 == tmpDeserialized[0].Id && g[k].N1 == tmpDeserialized[j].Id))
                                newEdge = false;
                        }

                        if (newEdge)
                            g.Add(new Graph(tmpDeserialized[0].Id, tmpDeserialized[j].Id, Convert.ToDouble(tmpDeserialized[j].N)));

                        newEdge = true;
                    }
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(g);
        }

        /* Generating graph with all the connections between nodes that collaborate with a choosen node.
         * IDs of edges are array positions of researchers
         */
        private string parseJsonAlltoallClosed(string data)
        {
            SicrisWS.Service1 cd = new SicrisWS.Service1();
            List<Rsr> myDeserializedObjList = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Rsr>));
            List<Graph> g = new List<Graph>();
            bool newEdge = true;
            string tmpData = "";

            for (int i = 0; i < myDeserializedObjList.Count; i++)
            {

                tmpData = cd.CollaborationOfRsrOnPrj1Json(myDeserializedObjList[i].Id);
                List<Rsr> tmpDeserialized = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(tmpData, typeof(List<Rsr>));

                for (int j = 1; j < tmpDeserialized.Count; j++)
                {
                    int pos = -1;
                    for (int l = 0; l < myDeserializedObjList.Count; l++)
                        if (tmpDeserialized[j].Id == myDeserializedObjList[l].Id)
                            pos = l;

                    if (pos != -1)
                    {
                        for (int k = 0; k < g.Count; k++)
                        {
                            if ((g[k].N1 == i.ToString() && g[k].N2 == pos.ToString()) || (g[k].N2 == i.ToString() && g[k].N1 == pos.ToString()))
                                newEdge = false;
                        }

                        if (newEdge)
                            g.Add(new Graph(i.ToString(), pos.ToString(), Convert.ToDouble(tmpDeserialized[j].N)));

                        newEdge = true;
                    }
                }
            }
            /*List<Graph> g1 = new List<Graph>();
            g1 = g.GroupBy(x => new { x.N1, x.N2 }).Select(y => y.First()).ToList<Graph>();*/
            return Newtonsoft.Json.JsonConvert.SerializeObject(g);
        }

        private string[] parseReturnRsrIdString(string data)
        {
            List<Rsr> myDeserializedObjList = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Rsr>));

            string[] tmpData = new string[myDeserializedObjList.Count];

            for (int i = 0; i < myDeserializedObjList.Count; i++)
            {
                tmpData[i] = myDeserializedObjList[i].Id;
            }

            return tmpData;
        }

        /* Generating graph with all the connections between organizations that collaborate
         */
        private string[] parseJsonAlltoAllClosedOrg(string data1, string data2, string data3)
        {
            List<Rsr> rsr = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(data1, typeof(List<Rsr>));
            List<Org> org = (List<Org>)Newtonsoft.Json.JsonConvert.DeserializeObject(data2, typeof(List<Org>));
            List<Graph> g = (List<Graph>)Newtonsoft.Json.JsonConvert.DeserializeObject(data3, typeof(List<Graph>));

            List<Graph> g1 = new List<Graph>();

            for (int i = 0; i < g.Count; i++)
            {
                bool newEdge1 = true, newEdge2 = true, newEdge3 = true;
                int a1=-1, a2=-1, a3=-1, b1=-1, b2=-1, b3=-1;
                for (int j = 0; j < org.Count; j++){
                    if (rsr[Convert.ToInt16(g[i].N1)].OrgId1 == org[j].Id) a1 = j;
                    if (rsr[Convert.ToInt16(g[i].N2)].OrgId1 == org[j].Id) b1 = j;
                    if (rsr[Convert.ToInt16(g[i].N1)].OrgId2 == org[j].Id) a2 = j;
                    if (rsr[Convert.ToInt16(g[i].N2)].OrgId2 == org[j].Id) b2 = j;
                    if (rsr[Convert.ToInt16(g[i].N1)].OrgId3 == org[j].Id) a3 = j;
                    if (rsr[Convert.ToInt16(g[i].N2)].OrgId3 == org[j].Id) b3 = j;
                }

                for (int k = 0; k < g1.Count; k++) {
                    if ((a1.ToString() == g1[k].N1 && b1.ToString() == g1[k].N2) || (a1.ToString() == g1[k].N2 && b1.ToString() == g1[k].N1))
                    { g1[k].Weight++; newEdge1 = false; org[Convert.ToInt16(g1[k].N1)].Vd++; org[Convert.ToInt16(g1[k].N2)].Vd++; }
                    if ((a2.ToString() == g1[k].N1 && b2.ToString() == g1[k].N2) || (a2.ToString() == g1[k].N2 && b2.ToString() == g1[k].N1))
                    { g1[k].Weight++; newEdge2 = false; org[Convert.ToInt16(g1[k].N1)].Vd++; org[Convert.ToInt16(g1[k].N2)].Vd++; }
                    if ((a3.ToString() == g1[k].N1 && b3.ToString() == g1[k].N2) || (a3.ToString() == g1[k].N2 && b3.ToString() == g1[k].N1))
                    { g1[k].Weight++; newEdge3 = false; org[Convert.ToInt16(g1[k].N1)].Vd++; org[Convert.ToInt16(g1[k].N2)].Vd++; }
                }
                if (newEdge1 && a1 != -1 && b1 != -1)
                {
                    g1.Add(new Graph(a1.ToString(), b1.ToString(), 1));
                    org[a1].Vd++; org[b1].Vd++;
                }
                if (newEdge2 && a2 != -1 && b2 != -1)
                {
                    g1.Add(new Graph(a2.ToString(), b2.ToString(), 1));
                    org[a2].Vd++; org[b2].Vd++;
                }
                if (newEdge3 && a3 != -1 && b3 != -1)
                {
                    g1.Add(new Graph(a3.ToString(), b3.ToString(), 1));
                    org[a3].Vd++; org[b3].Vd++;
                }
            }

            return new string[] { Newtonsoft.Json.JsonConvert.SerializeObject(g1), Newtonsoft.Json.JsonConvert.SerializeObject(org)};
        }

        /* Generating graph with all the connections between nodes that are given by the array.
         * IDs of edges are array positions of researchers
         */
        private string parseJsonAlltoallArrayIDs(string[] data)
        {
            SicrisWS.Service1 cd = new SicrisWS.Service1();
            List<Graph> g = new List<Graph>();
            bool newEdge = true;
            string tmpData = "";

            for (int i = 0; i < data.Length; i++)
            {

                tmpData = cd.CollaborationOfRsrOnPrj1Json(data[i]);
                List<Rsr> tmpDeserialized = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(tmpData, typeof(List<Rsr>));

                for (int j = 1; j < tmpDeserialized.Count; j++)
                {
                    int pos = -1;
                    for (int l = 0; l < data.Length; l++)
                        if (tmpDeserialized[j].Id == data[l])
                            pos = l;

                    if (pos != -1)
                    {
                        for (int k = 0; k < g.Count; k++)
                        {
                            if ((g[k].N1 == i.ToString() && g[k].N2 == pos.ToString()) || (g[k].N2 == i.ToString() && g[k].N1 == pos.ToString()))
                                newEdge = false;
                        }

                        if (newEdge)
                            g.Add(new Graph(i.ToString(), pos.ToString(), Convert.ToDouble(tmpDeserialized[j].N)));

                        newEdge = true;
                    }
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(g);
        }

        /* Determining vd (vertex degree) of the nodes of the graph
         */
        private string parseJsonVd(string data, string data1)
        {
            List<Graph> g = (List<Graph>)Newtonsoft.Json.JsonConvert.DeserializeObject(data1, typeof(List<Graph>));

            List<Rsr> myDeserializedObjList = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Rsr>));

            int vd = 0;

            for (int i = 0; i < myDeserializedObjList.Count; i++)
            {
                for (int j = 0; j < g.Count; j++)
                    if (g[j].N1 == i.ToString() || g[j].N2 == i.ToString())
                        vd++;

                myDeserializedObjList[i].Vd = vd.ToString();
                vd = 0;
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(myDeserializedObjList);
        }

        /*
         * Determining the coordinates of reseasrchers nodes using R
         */
        /*private string parseJsonRCoordinates(string data)
        {
            SicrisWS.Service1 cd = new SicrisWS.Service1();
            List<Rsr> myDeserializedObjList = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Rsr>));

            List<Graph> g = new List<Graph>();
            string tmpData = ""; string documents = "";
            string[] stringArray = new string[myDeserializedObjList.Count];

            for (int i = 0; i < myDeserializedObjList.Count; i++){
                
                tmpData = cd.PrjOfRsrJson(myDeserializedObjList[i].Id);
                List<Prj> tmpDeserialized = (List<Prj>)Newtonsoft.Json.JsonConvert.DeserializeObject(tmpData, typeof(List<Prj>));
                documents = "";
                for (int j = 1; j < tmpDeserialized.Count; j++)
                {
                    documents += " " + tmpDeserialized[j].Name + " " + tmpDeserialized[j].Keywords + " " + tmpDeserialized[j].Abstract + " " + tmpDeserialized[j].SignificanceWorld + " " + tmpDeserialized[j].SignificanceDomestic;
                }
                stringArray[i] = documents;
            }
            var locations = (dynamic)null;
            REngine.SetDllDirectory(@"C:\Program Files\R\R-2.14.1\bin\i386");
            using (REngine engine = REngine.CreateInstance("RDotNet", new[] { "-q" }))
            {

                var a = engine.CreateCharacterVector(stringArray);
                engine.SetSymbol("a", a);
                engine.EagerEvaluate("library(tm);");
                
                var e = engine.Evaluate("corpus <- Corpus(VectorSource(a));"+
                "corpus <-tm_map(corpus,removeWords,stopwords(\"english\"))"+
                "corpus <- tm_map(corpus,stemDocument);"+
                "corpus <- tm_map(corpus, removePunctuation);"+
                "corpus <- tm_map(corpus, removeNumbers);"+
                "corpus <- tm_map(corpus, stripWhitespace);");
                e.ToArray();
                var corpus = engine.GetSymbol("corpus");
                engine.SetSymbol("corpus", corpus);

                var f = engine.Evaluate("tdm <- TermDocumentMatrix(corpus);");
                f.ToArray();
                var tdm = engine.GetSymbol("tdm");
                engine.SetSymbol("tdm", tdm);

                var g1 = engine.Evaluate("mat <- dissimilarity(tdm, method = \"cosine\")");
                g1.ToArray();
                var mat = engine.GetSymbol("mat").AsDataFrame();
                engine.SetSymbol("mat", mat);
                
                var h = engine.Evaluate("city <- cmdscale(mat, k=2);");
                h.ToArray();
                var city = engine.GetSymbol("city");
                engine.SetSymbol("city", city);

                locations = engine.GetSymbol("tdm");
                //locations = engine.GetSymbol("city.location").AsNumericMatrix();
   
            }

            for (int i = 0; i < myDeserializedObjList.Count; i++)
            {
                myDeserializedObjList[i].X = locations[i,1].ToString();
                myDeserializedObjList[i].Y = locations[i,2].ToString();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(myDeserializedObjList);

        }*/

        /* Coordinates of project nodes, keywords nodes and connections between prj and keyws are determined with NetDocAtlasLib.cs
         * 
         */
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, Unrestricted = true)]
        private string[] parseJsonAtlasCoordinatesKeywsPrj(string data)
        {
            List<Prj> myDeserializedObjList = (List<Prj>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Prj>));
            List<Keyword> keyws = new List<Keyword>();
            List<Graph> g = new List<Graph>();
            
            try
            {
                if (myDeserializedObjList.Count > 1 && 1>2)
                {
                    int BowDocBsId = DocAtlasLib.NewEmptyBow(false, false);

                    int keywords_count = 0;
                    string[] keywords_array;
                    for (int i = 0; i < myDeserializedObjList.Count && i < 200; i++)
                    {
                        
                        string documents = myDeserializedObjList[i].Name + " ";
                        if (i % 2 == 0)
                            documents += " safe ";

                        documents += myDeserializedObjList[i].Keywords + " " + myDeserializedObjList[i].Abstract + " " + myDeserializedObjList[i].SignificanceWorld + " " + myDeserializedObjList[i].SignificanceDomestic;
                        documents = documents.Replace(',', ' ');
                        StringBuilder sb = new StringBuilder(documents, 500);
                        DocAtlasLib.AddHtmlDoc(BowDocBsId, i.ToString()+"_document", sb.ToString());

                    }

                    int VizMapId = DocAtlasLib.NewFromBowId(BowDocBsId);
                    Random rand = new Random();

                    for (int i = 0; i < myDeserializedObjList.Count; i++)
                    {
                        myDeserializedObjList[i].X = DocAtlasLib.GetKeyWdPosX(Convert.ToInt16(VizMapId), Convert.ToInt16(i)).ToString().Replace(',', '.');
                        myDeserializedObjList[i].Y = DocAtlasLib.GetKeyWdPosY(Convert.ToInt16(VizMapId), Convert.ToInt16(i)).ToString().Replace(',', '.');
                        //myDeserializedObjList[i].X = rand.NextDouble().ToString();
                        //myDeserializedObjList[i].Y = rand.NextDouble().ToString();
                        
                        //Adding keywords to the nodes
                        keywords_array = myDeserializedObjList[i].Keywords.Trim().Split(',');

                        myDeserializedObjList[i].Vd = 1 + keywords_array.Length;

                        for (int j=0; j<keywords_array.Length; j++){
                            if ((keywords_array[j].Trim()) != "")
                            {
                                keyws.Add(new Keyword(keywords_count.ToString() + "k", keywords_array[j].Trim(), "1", getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].X) - 0.1, Convert.ToDouble(myDeserializedObjList[i].X) + 0.1).ToString(), getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].Y) - 0.1, Convert.ToDouble(myDeserializedObjList[i].Y) + 0.1).ToString(), 1));
                                g.Add(new Graph(i.ToString(), keywords_count.ToString(), 1.0));
                                keywords_count++;
                            }
                        }
                    }
                }
                else
                {
                    Random rand = new Random();
                    int keywords_count = 0;
                    string[] keywords_array;
                    for (int i = 0; i < myDeserializedObjList.Count; i++)
                    {
                        myDeserializedObjList[i].X = rand.NextDouble().ToString();
                        myDeserializedObjList[i].Y = rand.NextDouble().ToString();

                        keywords_array = myDeserializedObjList[i].Keywords.Trim().Split(',');
                        myDeserializedObjList[i].Vd = 1 + keywords_array.Length;

                        for (int j = 0; j < keywords_array.Length; j++)
                        {
                            if ((keywords_array[j].Trim()) != "")
                            {
                                keyws.Add(new Keyword(keywords_count.ToString() + "k", keywords_array[j].Trim(), "1", getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].X) - 0.1, Convert.ToDouble(myDeserializedObjList[i].X) + 0.1).ToString(), getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].Y) - 0.1, Convert.ToDouble(myDeserializedObjList[i].Y) + 0.1).ToString(), 1));
                                g.Add(new Graph(i.ToString(), keywords_count.ToString(), 1.0));
                                keywords_count++;
                            }
                        }
                    }
                }
            }
            catch
            {
                Random rand = new Random();
                int keywords_count = 0;
                string[] keywords_array;

                for (int i = 0; i < myDeserializedObjList.Count; i++)
                {
                    myDeserializedObjList[i].X = rand.NextDouble().ToString();
                    myDeserializedObjList[i].Y = rand.NextDouble().ToString();

                    keywords_array = myDeserializedObjList[i].Keywords.Trim().Split(',');
                    myDeserializedObjList[i].Vd = 1 + keywords_array.Length;

                    for (int j = 0; j < keywords_array.Length; j++)
                    {
                        if ((keywords_array[j].Trim()) != "")
                        {
                            keyws.Add(new Keyword(keywords_count.ToString() + "k", keywords_array[j].Trim(), "1", getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].X) - 0.1, Convert.ToDouble(myDeserializedObjList[i].X) + 0.1).ToString(), getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].Y) - 0.1, Convert.ToDouble(myDeserializedObjList[i].Y) + 0.1).ToString(), 1));
                            g.Add(new Graph(i.ToString(), keywords_count.ToString(), 1.0));
                            keywords_count++;
                        }
                    }

                }    
            }
  
            return new string[] { Newtonsoft.Json.JsonConvert.SerializeObject(myDeserializedObjList), Newtonsoft.Json.JsonConvert.SerializeObject(keyws),  Newtonsoft.Json.JsonConvert.SerializeObject(g)};
        }

        /* Coordinates of project nodes, keywords nodes and connections between prj and keyws are determined with NetDocAtlasLib.cs
 * 
 */
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, Unrestricted = true)]
        private string[] parseJsonAtlasCoordinatesKeywsConnectedPrj(string data, string language)
        {
            List<Prj> myDeserializedObjList = (List<Prj>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Prj>));
            List<Keyword> keyws = new List<Keyword>();
            List<Graph> g = new List<Graph>();

            try
            {
                if (myDeserializedObjList.Count > 1 && 1 > 2)
                {
                    int BowDocBsId = DocAtlasLib.NewEmptyBow(false, false);

                    int keywords_count = 0;
                    string[] keywords_array;
                    for (int i = 0; i < myDeserializedObjList.Count && i < 200; i++)
                    {

                        string documents = myDeserializedObjList[i].Name + " ";
                        if (i % 2 == 0)
                            documents += " safe ";

                        documents += myDeserializedObjList[i].Keywords + " " + myDeserializedObjList[i].Abstract + " " + myDeserializedObjList[i].SignificanceWorld + " " + myDeserializedObjList[i].SignificanceDomestic;
                        documents = documents.Replace(',', ' ');
                        StringBuilder sb = new StringBuilder(documents, 500);
                        DocAtlasLib.AddHtmlDoc(BowDocBsId, i.ToString() + "_document", sb.ToString());

                    }

                    int VizMapId = DocAtlasLib.NewFromBowId(BowDocBsId);
                    Random rand = new Random();

                    for (int i = 0; i < myDeserializedObjList.Count; i++)
                    {
                        myDeserializedObjList[i].X = DocAtlasLib.GetKeyWdPosX(Convert.ToInt16(VizMapId), Convert.ToInt16(i)).ToString().Replace(',', '.');
                        myDeserializedObjList[i].Y = DocAtlasLib.GetKeyWdPosY(Convert.ToInt16(VizMapId), Convert.ToInt16(i)).ToString().Replace(',', '.');
                        //myDeserializedObjList[i].X = rand.NextDouble().ToString();
                        //myDeserializedObjList[i].Y = rand.NextDouble().ToString();

                        //Adding keywords to the nodes
                        keywords_array = myDeserializedObjList[i].Keywords.Trim().Split(',');

                        myDeserializedObjList[i].Vd = 1 + keywords_array.Length;

                        for (int j = 0; j < keywords_array.Length; j++)
                        {
                            if ((keywords_array[j].Trim()) != "")
                            {
                                int oldKeyw = -1;
                                for (int k = 0; k < keyws.Count; k++) {
                                    if (keywords_array[j].Trim() == keyws[k].Word)
                                        oldKeyw = k;
                                }
                                if (oldKeyw == -1)
                                {
                                    keyws.Add(new Keyword(keywords_count.ToString() + "k", keywords_array[j].Trim(), "1", getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].X) - 0.1, Convert.ToDouble(myDeserializedObjList[i].X) + 0.1).ToString(), getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].Y) - 0.1, Convert.ToDouble(myDeserializedObjList[i].Y) + 0.1).ToString(), 1));
                                    g.Add(new Graph(i.ToString(), keywords_count.ToString(), 1.0));
                                    keywords_count++;
                                }
                                else 
                                {
                                    g.Add(new Graph(i.ToString(), oldKeyw.ToString(), 1.0));
                                }
                            }
                        }
                    }
                }
                else
                {
                    Random rand = new Random();
                    int keywords_count = 0;
                    string[] keywords_array;
                    for (int i = 0; i < myDeserializedObjList.Count; i++)
                    {
                        myDeserializedObjList[i].X = rand.NextDouble().ToString();
                        myDeserializedObjList[i].Y = rand.NextDouble().ToString();

                        if(language=="slv")
                            keywords_array = myDeserializedObjList[i].Keywords.Trim().Split(',');
                        else if (language == "eng")
                            keywords_array = myDeserializedObjList[i].Keywords_en.Trim().Split(',');
                        else {
                            keywords_array = myDeserializedObjList[i].Keywords.Trim().Split(',');
                        }

                        myDeserializedObjList[i].Vd = 1 + keywords_array.Length;

                        for (int j = 0; j < keywords_array.Length; j++)
                        {
                            if ((keywords_array[j].Trim()) != "")
                            {
                                int oldKeyw = -1;
                                for (int k = 0; k < keyws.Count; k++)
                                {
                                    if (ComputeLevenshteinDistance(keywords_array[j].Replace('.', ' ').Trim().ToLower(),keyws[k].Word)<3)
                                        oldKeyw = k;
                                }
                                if (oldKeyw == -1)
                                {
                                    keyws.Add(new Keyword(keywords_count.ToString() + "k", keywords_array[j].Replace('.', ' ').Trim().ToLower(), "1", getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].X) - 0.1, Convert.ToDouble(myDeserializedObjList[i].X) + 0.1).ToString(), getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].Y) - 0.1, Convert.ToDouble(myDeserializedObjList[i].Y) + 0.1).ToString(), 1));
                                    g.Add(new Graph(i.ToString(), keywords_count.ToString(), 1.0));
                                    keywords_count++;
                                }
                                else
                                {
                                    g.Add(new Graph(i.ToString(), oldKeyw.ToString(), 1.0));
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                Random rand = new Random();
                int keywords_count = 0;
                string[] keywords_array;

                for (int i = 0; i < myDeserializedObjList.Count; i++)
                {
                    myDeserializedObjList[i].X = rand.NextDouble().ToString();
                    myDeserializedObjList[i].Y = rand.NextDouble().ToString();

                    keywords_array = myDeserializedObjList[i].Keywords.Trim().Split(',');
                    myDeserializedObjList[i].Vd = 1 + keywords_array.Length;

                    for (int j = 0; j < keywords_array.Length; j++)
                    {
                        if ((keywords_array[j].Trim()) != "")
                        {
                            int oldKeyw = -1;
                            for (int k = 0; k < keyws.Count; k++)
                            {
                                if (keywords_array[j].Trim() == keyws[k].Word)
                                    oldKeyw = k;
                            }
                            if (oldKeyw == -1)
                            {
                                keyws.Add(new Keyword(keywords_count.ToString() + "k", keywords_array[j].Trim(), "1", getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].X) - 0.1, Convert.ToDouble(myDeserializedObjList[i].X) + 0.1).ToString(), getRandomNumber(rand.NextDouble(), Convert.ToDouble(myDeserializedObjList[i].Y) - 0.1, Convert.ToDouble(myDeserializedObjList[i].Y) + 0.1).ToString(), 1));
                                g.Add(new Graph(i.ToString(), keywords_count.ToString(), 1.0));
                                keywords_count++;
                            }
                            else
                            {
                                g.Add(new Graph(i.ToString(), oldKeyw.ToString(), 1.0));
                            }
                        }
                    }

                }
            }
            List<Graph> g1 = new List<Graph>();
            g1  = g.GroupBy(x => new { x.N1, x.N2 }).Select(y => y.First()).ToList<Graph>();
            return new string[] { Newtonsoft.Json.JsonConvert.SerializeObject(myDeserializedObjList), Newtonsoft.Json.JsonConvert.SerializeObject(keyws), Newtonsoft.Json.JsonConvert.SerializeObject(g1) };
        }

        /* Coordinates of project nodes are determined with NetDocAtlasLib.cs
 * 
 */
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, Unrestricted = true)]
        private string parseJsonAtlasCoordinatesPrj(string data)
        {
            List<Prj> myDeserializedObjList = (List<Prj>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Prj>));

            try
            {
                if (myDeserializedObjList.Count > 1)
                {
                    int BowDocBsId = DocAtlasLib.NewEmptyBow(false, false);

                    for (int i = 0; i < myDeserializedObjList.Count && i < 200; i++)
                    {
                        string documents = myDeserializedObjList[i].Name + " ";
                        if (i % 2 == 0)
                            documents += " safe ";

                        documents += myDeserializedObjList[i].Keywords + " " + myDeserializedObjList[i].Abstract + " " + myDeserializedObjList[i].SignificanceWorld + " " + myDeserializedObjList[i].SignificanceDomestic;
                        documents = documents.Replace(',', ' ');
                        DocAtlasLib.AddHtmlDoc(BowDocBsId, i.ToString(), documents);

                    }

                    int VizMapId = DocAtlasLib.NewFromBowId(BowDocBsId);
                    Random rand = new Random();

                    for (int i = 0; i < myDeserializedObjList.Count; i++)
                    {
                        myDeserializedObjList[i].X = DocAtlasLib.GetKeyWdPosX(VizMapId, i).ToString().Replace(',', '.');
                        myDeserializedObjList[i].Y = DocAtlasLib.GetKeyWdPosY(VizMapId, i).ToString().Replace(',', '.');
                    }
                }
                else
                {
                    Random rand = new Random();
                    for (int i = 0; i < myDeserializedObjList.Count; i++)
                    {
                        myDeserializedObjList[i].X = rand.NextDouble().ToString();
                        myDeserializedObjList[i].Y = rand.NextDouble().ToString();
                    }
                }
            }
            catch
            {
                Random rand = new Random();
                for (int i = 0; i < myDeserializedObjList.Count; i++)
                {
                    myDeserializedObjList[i].X = rand.NextDouble().ToString();
                    myDeserializedObjList[i].Y = rand.NextDouble().ToString();
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(myDeserializedObjList);
        }

        /* 
         * Coordinates of nodes are determined with NetDocAtlasLib.cs
         */
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, Unrestricted = true)] 
        private string parseJsonAtlasCoordinates(string data)
        {
            SicrisWS.Service1 cd = new SicrisWS.Service1();
            List<Rsr> myDeserializedObjList = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Rsr>));

            try
            {
                if (myDeserializedObjList.Count > 1 /*&& 1>2*/)
                {

                    List<Graph> g = new List<Graph>();
                    string tmpData = "";
                    int BowDocBsId = DocAtlasLib.NewEmptyBow(false, false);

                    string a = "";

                    for (int i = 0; i < myDeserializedObjList.Count; i++)
                    {
                        tmpData = cd.PrjOfRsrJson(myDeserializedObjList[i].Id);
                        List<Prj> tmpDeserialized = (List<Prj>)Newtonsoft.Json.JsonConvert.DeserializeObject(tmpData, typeof(List<Prj>));
                        string documents = myDeserializedObjList[i].First_name +" "+ myDeserializedObjList[i].Last_name +" "+myDeserializedObjList[i].Keyws+" ";
                        
                        if (tmpDeserialized.Count < 1)
                            a += myDeserializedObjList[i].Id+",";

                        for (int j = 0; j < tmpDeserialized.Count; j++)
                        {
                            documents += tmpDeserialized[j].Keywords + " " + tmpDeserialized[j].Abstract + " " + tmpDeserialized[j].SignificanceWorld + " " + tmpDeserialized[j].SignificanceDomestic;
                        }
                        if (i%2==0)
                            documents += " safe ";
                        documents = documents.Replace(',',' ');
                        
                        DocAtlasLib.AddHtmlDoc(BowDocBsId, i.ToString(), documents);
                    }

                    int VizMapId = DocAtlasLib.NewFromBowId(BowDocBsId);

                    for (int i = 0; i < myDeserializedObjList.Count; i++)
                    {
                        myDeserializedObjList[i].X = DocAtlasLib.GetKeyWdPosX(VizMapId, i).ToString().Replace(',', '.');
                        myDeserializedObjList[i].Y = DocAtlasLib.GetKeyWdPosY(VizMapId, i).ToString().Replace(',', '.');
                    }
                }
                else {
                    Random rand = new Random();
                    for (int i = 0; i < myDeserializedObjList.Count; i++)
                    {
                        myDeserializedObjList[i].X = rand.NextDouble().ToString();
                        myDeserializedObjList[i].Y = rand.NextDouble().ToString();
                    }
                }
            }
            catch
            {
                Random rand = new Random();
                for (int i = 0; i < myDeserializedObjList.Count; i++)
                {
                    myDeserializedObjList[i].X = rand.NextDouble().ToString();
                    myDeserializedObjList[i].Y = rand.NextDouble().ToString();
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(myDeserializedObjList);
        }

        /* 
         * News from newsfeed webservice
         */
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, Unrestricted = true)]
        private string parseJsonNews(string data)
        {
            Newtonsoft.Json.Linq.JObject o = Newtonsoft.Json.Linq.JObject.Parse(data);
            int hits = (int)o["hits"];
            Newtonsoft.Json.Linq.JArray articles = (Newtonsoft.Json.Linq.JArray)o["articles"];
            List<News> myDeserializedObjList = new List<News>(articles.Count); 
            string uri, date, image="", title, body, language="";
            double id = -1.0;

            for (int i = 0; i < articles.Count; i++) {
                id = (double)((articles[i])["id"]);
                title = (string)(articles[i])["title"];
                date = (string)(articles[i])["date"];
                body = (string)(articles[i])["body"];
                uri = (string)(articles[i])["uri"];
                Newtonsoft.Json.Linq.JArray images = (Newtonsoft.Json.Linq.JArray)(articles[i])["images"];
                if (images.Count > 0)
                    image = (string)(images[0])["src"];
                else
                    image = "";
                language = (string)(articles[i])["language"];
                myDeserializedObjList.Add(new News(id,date,title,body,uri,image,language));

            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(myDeserializedObjList);
        }

        /* Determining coordinates for the circle layout graph end returning the json with nodes 
         * 
         */
        private string parseJsonCircle(string data)
        {
            List<Rsr> myDeserializedObjList = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Rsr>));

            if (myDeserializedObjList.Count > 0)
            {
                myDeserializedObjList[0].X = 0.5.ToString();
                myDeserializedObjList[0].Y = 0.5.ToString();

                double w = 0;
                double wmax = 0;
                int k = 0, br = 0;
                int[] arr = new int[myDeserializedObjList.Count];

                for (int i = 0; i < myDeserializedObjList.Count; i++)
                {

                    if (i == 0)
                        wmax = Convert.ToDouble(myDeserializedObjList[i].N);

                    else
                    {
                        if (w != Convert.ToDouble(myDeserializedObjList[i].N))
                        {
                            arr[k] = br + 1;
                            k++;
                            br = 0;
                            w = Convert.ToDouble(myDeserializedObjList[i].N);
                        }
                        else
                            br++;
                    }
                }

                arr[k] = br + 1;
                double n_of_levels = k;
                w = 0;
                wmax = 0;
                double k1 = 0;
                br = 1;
                Random rand = new Random();
                double ran = 0;

                for (int i = 0; i < myDeserializedObjList.Count; i++)
                {

                    if (i == 0)
                        wmax = Convert.ToDouble(myDeserializedObjList[i].N);

                    else
                    {
                        if (w != Convert.ToDouble(myDeserializedObjList[i].N))
                        {
                            k1 = arr[br];
                            br++;
                            ran = Convert.ToDouble(rand.Next(0, 360));
                        }
                        w = Convert.ToDouble(myDeserializedObjList[i].N);
                    }

                    if (i > 0)
                    {
                        double s = Math.Sin(DegreeToRadian(90));

                        myDeserializedObjList[i].X = (0.5 + (((Math.Sin(DegreeToRadian(Convert.ToDouble(360.0 / k1) * i + ran)) / 2.0) * Math.Abs(Convert.ToDouble(Convert.ToDouble(br - 1) / n_of_levels))))).ToString();

                        myDeserializedObjList[i].Y = ((0.5 + (((Math.Cos(DegreeToRadian(Convert.ToDouble(360.0 / k1) * i + ran)) / 2.0) * Math.Abs(Convert.ToDouble(Convert.ToDouble(br - 1) / n_of_levels)))))*0.9).ToString();

                    }

                }

            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(myDeserializedObjList);

        }

        /* Returning edges of circle layouted graph 
         *
         */
        private string parseJsonCircleGraph(string data)
        {
            List<Rsr> myDeserializedObjList = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Rsr>));
            List<Graph> g = new List<Graph>();

            for (int i = 1; i < myDeserializedObjList.Count; i++)
            {
                g.Add(new Graph(0.ToString(), i.ToString(), Convert.ToDouble(myDeserializedObjList[i].N)));
            }
            /*List<Graph> g1 = new List<Graph>();
            g1 = g.GroupBy(x => new { x.N1, x.N2 }).Select(y => y.First()).ToList<Graph>();*/
            return Newtonsoft.Json.JsonConvert.SerializeObject(g);
        }

        /*
         * Function for replacing potential palatals
         */
        private string replacePalatals(string strVal)
        {
            string word = strVal;
            string first = "";
            for (int i = 0; i < strVal.Split(' ').Length; i++)
                first += strVal.Split(' ')[i].Substring(0, 1);

            if (!first.Contains('c'))
                word = word.Replace('c', '?');
            if (!first.Contains('C'))
                word = word.Replace('C', '?');
            if (!first.Contains('s'))
                word = word.Replace('s', '?');
            if (!first.Contains('S'))
                word = word.Replace('S', '?');
            if (!first.Contains('z'))
                word = word.Replace('z', '?');
            if (!first.Contains('Z'))
                word = word.Replace('Z', '?');

            return word;
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public double getRandomNumber(double rn, double minimum, double maximum)
        {
            return minimum + rn * (maximum - minimum);
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        public static int ComputeLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public static string[] toRsrIdString(string data) { 
            List<Rsr> tmpDeserialized = (List<Rsr>)Newtonsoft.Json.JsonConvert.DeserializeObject(data, typeof(List<Rsr>));
            string[] ids = new string[tmpDeserialized.Count];
            for (int j = 0; j < tmpDeserialized.Count; j++)
                ids[j] = tmpDeserialized[j].Id;
            return ids;
        } 
           

    }
}