using System;                         
using System.Collections.Generic;     
using System.Text;                    
using System.Runtime.InteropServices; 

namespace NetDocAtlasLib
{                                     
   public class DocAtlasLib
   {                                  
      private class HDocAtlasLib
      {                               
         [DllImport("DocAtlasLib.dll")]
         public static extern void DelCStr(IntPtr CStr);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GenIntV();

         [DllImport("DocAtlasLib.dll")]
         public static extern void DelIntV(int IntVId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetIntVLen(int IntVId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int AddIntVVal(int IntVId, int Val);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetIntVVal(int IntVId, int ValN);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GenStrV();

         [DllImport("DocAtlasLib.dll")]
         public static extern void DelStrV(int StrVId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetStrVLen(int StrVId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int AddStrVVal(int StrVId, String Val);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetStrVVal(int StrVId, int ValN);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GenIntFltPrV();

         [DllImport("DocAtlasLib.dll")]
         public static extern void DelIntFltPrV(int IntFltPrVId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetIntFltPrVLen(int IntFltPrVId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int AddIntFltPrVVal(int IntFltPrVId, int IntVal, double FltVal);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetIntFltPrVIntVal(int IntFltPrVId, int ValN);

         [DllImport("DocAtlasLib.dll")]
         public static extern double GetIntFltPrVFltVal(int IntFltPrVId, int ValN);

         [DllImport("DocAtlasLib.dll")]
         public static extern void GksBeginPaint(long Hdc, long Handle);

         [DllImport("DocAtlasLib.dll")]
         public static extern void GksEndPaint();

         [DllImport("DocAtlasLib.dll")]
         public static extern void PaintEmpty();

         [DllImport("DocAtlasLib.dll")]
         public static extern void PaintVizMap(int VizMapId, bool ShowPointNmP, int PointFontSize, int PointNmFontScale, double PointWgtThreshold, int CatId, bool ShowCatNmP, bool ShowKeyWdP, int KeyWdFontSize, bool ShowMgGlassP);

         [DllImport("DocAtlasLib.dll")]
         public static extern void MouseEnter(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern void MouseLeave(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         [return:MarshalAs(UnmanagedType.I1)]
         public static extern bool MouseMove(int VizMapId, int X, int Y, int Width, int Height, int MgGlassKeyWds);

         [DllImport("DocAtlasLib.dll")]
         public static extern void MouseLeftDown(int VizMapId, int X, int Y, int Width, int Height);

         [DllImport("DocAtlasLib.dll")]
         public static extern void MouseLeftUp(int VizMapId, int X, int Y, int Width, int Height);

         [DllImport("DocAtlasLib.dll")]
         public static extern void MouseWheel(int VizMapId, int Degree, int MgGlassKeyWds);

         [DllImport("DocAtlasLib.dll")]
         public static extern void SetZoomMode(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         [return:MarshalAs(UnmanagedType.I1)]
         public static extern bool IsZoomMode(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern void SetSelectMode(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         [return:MarshalAs(UnmanagedType.I1)]
         public static extern bool IsSelectMode(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         [return:MarshalAs(UnmanagedType.I1)]
         public static extern bool ZoomOut(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         [return:MarshalAs(UnmanagedType.I1)]
         public static extern bool ZoomAll(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetSelectDIdV(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern void UnselectAll(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetFrames(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetFrameN(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern void SetFrame(int VizMapId, int FrameN);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetFrameNm(int VizMapId, int FrameN);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetDocs(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetDocTitle(int VizMapId, int DId);

         [DllImport("DocAtlasLib.dll")]
         public static extern double GetDocPosX(int VizMapId, int DId);

         [DllImport("DocAtlasLib.dll")]
         public static extern double GetDocPosY(int VizMapId, int DId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetKeyWds(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern double GetKeyWdPosX(int VizMapId, int WId);

         [DllImport("DocAtlasLib.dll")]
         public static extern double GetKeyWdPosY(int VizMapId, int WId);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetKeyWdStr(int VizMapId, int WId);

         [DllImport("DocAtlasLib.dll")]
         [return:MarshalAs(UnmanagedType.I1)]
         public static extern bool IsSelDoc(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int SelDocId(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr SelDocTitle(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr SelDocBody(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr SelDocUrl(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern void SelDoc(int VizMapId, int DocId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetCats(int VizMapId);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetCatNm(int VizMapId, int CId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetSwSetTypeNmStrV();

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetSwSetTypeDNmStrV();

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetStemmerTypeNmStrV();

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetStemmerTypeDNmStrV();

         [DllImport("DocAtlasLib.dll")]
         public static extern int GenSwSet(String SwSetType);

         [DllImport("DocAtlasLib.dll")]
         public static extern void AddSwSetWord(int SwSetId, String WordCStr);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetXmlTypeStaticDoc();

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetXmlTypeDynamicDoc();

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetXmlTypeStaticAuthor();

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetXmlTypeDynamicAuthor();

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewFromBow(String BowFNm);

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewFromHtml(String HtmlPath, bool RecursiveP, int SwSetId, String StemmerType, int MxNGramLen, int MnNGramFq);

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewFromLnDoc(String NmLineDocFNm, int SwSetId, String StemmerType, int MxNGramLen, int MnNGramFq);

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewFromXml(String XmlFNm, int XmlMapType, int SwSetId, String StemmerType, int MxNGramLen, int MnNGramFq);

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewFromGoogleSearch(String WebQueryStr, int MxHits);

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewFromGoogleNewsSearch(String NewsQueryStr);

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewFromGoogleScholar(String ScholarQueryStr);

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewFromGoogleAuthorScholar(String ScholarAuthorQueryStr);

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewFromGooglePublicationScholar(String ScholarPublicationQueryStr);

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewEmptyBow(bool StemmerP, bool SwSetP);

         [DllImport("DocAtlasLib.dll")]
         [return:MarshalAs(UnmanagedType.I1)]
         public static extern bool AddWordStr(int BowDocBsId, String WordStr, int WId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int AddHtmlDoc(int BowDocBsId, String DocTitle, String DocBody);

         [DllImport("DocAtlasLib.dll")]
         public static extern int AddIntFltPrV(int BowDocBsId, String DocTitle, int IntFltPrVId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewFromBowId(int BowDocBsId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int NewFromKeyWdBowId(int BowDocBsId, int KeyWdBowDocBsId);

         [DllImport("DocAtlasLib.dll")]
         public static extern int LoadVizMap(String VizMapFNm);

         [DllImport("DocAtlasLib.dll")]
         public static extern void SaveVizMap(int VizMapId, String VizMapFNm);

         [DllImport("DocAtlasLib.dll")]
         public static extern void SaveBmp(int VizMapId, String BmpFNm, int Width, int Height, bool ShowPointNmP, int PointFontSize, int PointNmFontScale, double PointWgtThreshold, int CatId, bool ShowCatNmP, bool ShowKeyWdP, int KeyWdFontSize, bool ShowMgGlassP, int LegendGridWidth, int LegendGridHeight);

         [DllImport("DocAtlasLib.dll")]
         public static extern void SaveJpg(int VizMapId, String JpgFNm, int Width, int Height, bool ShowPointNmP, int PointFontSize, int PointNmFontScale, double PointWgtThreshold, int CatId, bool ShowCatNmP, bool ShowKeyWdP, int KeyWdFontSize, bool ShowMgGlassP, int LegendGridWidth, int LegendGridHeight);

         [DllImport("DocAtlasLib.dll")]
         public static extern void SaveTiff(int VizMapId, String TiffFNm, int Width, int Height, bool ShowPointNmP, int PointFontSize, int PointNmFontScale, double PointWgtThreshold, int CatId, bool ShowCatNmP, bool ShowKeyWdP, int KeyWdFontSize, bool ShowMgGlassP, int LegendGridWidth, int LegendGridHeight);

         [DllImport("DocAtlasLib.dll")]
         public static extern void SaveVrml(int VizMapId, String VrmlFNm, bool ShowPointNmP, bool ShowKeyWdP);

         [DllImport("DocAtlasLib.dll")]
         public static extern void SaveLegend(int VizMapId, String TxtFNm, int LegendGridWidth, int LegendGridHeight);

         [DllImport("DocAtlasLib.dll")]
         public static extern void LoadSearchBs(String InSearchBs);

         [DllImport("DocAtlasLib.dll")]
         public static extern void Search(String QueryStr);

         [DllImport("DocAtlasLib.dll")]
         [return:MarshalAs(UnmanagedType.I1)]
         public static extern bool IsSearch();

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetTotalHits();

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetHits();

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetDateStr(int HitN);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetTitleStr(int HitN);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetBodyStr(int HitN);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetStaticDocVizMap(String OutVizMapFPath);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetDynamicDocVizMap(String OutVizMapFPath);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetStaticNmEnVizMap(String OutVizMapFPath);

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetDynamicNmEnVizMap(String OutVizMapFPath);

         [DllImport("DocAtlasLib.dll")]
         public static extern void MakeTopics();

         [DllImport("DocAtlasLib.dll")]
         [return:MarshalAs(UnmanagedType.I1)]
         public static extern bool IsTopics();

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetTopics();

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetTopicNm(int TopicN);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetTopicFq(int TopicN);

         [DllImport("DocAtlasLib.dll")]
         public static extern int GetTopicFrames();

         [DllImport("DocAtlasLib.dll")]
         public static extern IntPtr GetTopicFrameNm(int FrameN);

         [DllImport("DocAtlasLib.dll")]
         public static extern double GetTopicFrameFq(int FrameN, int TopicN);

         [DllImport("DocAtlasLib.dll")]
         public static extern double GetTopicFrameAllFq(int FrameN);

         [DllImport("DocAtlasLib.dll")]
         public static extern double GetMxTopicFrameAllFq();

      }//inner class



      public static int GenIntV()
      {
         return HDocAtlasLib.GenIntV();
      }

      public static void DelIntV(int IntVId)
      {
          HDocAtlasLib.DelIntV(IntVId);
      }

      public static int GetIntVLen(int IntVId)
      {
         return HDocAtlasLib.GetIntVLen(IntVId);
      }

      public static int AddIntVVal(int IntVId, int Val)
      {
         return HDocAtlasLib.AddIntVVal(IntVId, Val);
      }

      public static int GetIntVVal(int IntVId, int ValN)
      {
         return HDocAtlasLib.GetIntVVal(IntVId, ValN);
      }

      public static int GenStrV()
      {
         return HDocAtlasLib.GenStrV();
      }

      public static void DelStrV(int StrVId)
      {
          HDocAtlasLib.DelStrV(StrVId);
      }

      public static int GetStrVLen(int StrVId)
      {
         return HDocAtlasLib.GetStrVLen(StrVId);
      }

      public static int AddStrVVal(int StrVId, String Val)
      {
         return HDocAtlasLib.AddStrVVal(StrVId, Val);
      }

      public static String GetStrVVal(int StrVId, int ValN)
      {
         IntPtr i = HDocAtlasLib.GetStrVVal(StrVId, ValN);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static int GenIntFltPrV()
      {
         return HDocAtlasLib.GenIntFltPrV();
      }

      public static void DelIntFltPrV(int IntFltPrVId)
      {
          HDocAtlasLib.DelIntFltPrV(IntFltPrVId);
      }

      public static int GetIntFltPrVLen(int IntFltPrVId)
      {
         return HDocAtlasLib.GetIntFltPrVLen(IntFltPrVId);
      }

      public static int AddIntFltPrVVal(int IntFltPrVId, int IntVal, double FltVal)
      {
         return HDocAtlasLib.AddIntFltPrVVal(IntFltPrVId, IntVal, FltVal);
      }

      public static int GetIntFltPrVIntVal(int IntFltPrVId, int ValN)
      {
         return HDocAtlasLib.GetIntFltPrVIntVal(IntFltPrVId, ValN);
      }

      public static double GetIntFltPrVFltVal(int IntFltPrVId, int ValN)
      {
         return HDocAtlasLib.GetIntFltPrVFltVal(IntFltPrVId, ValN);
      }

      public static void GksBeginPaint(long Hdc, long Handle)
      {
          HDocAtlasLib.GksBeginPaint(Hdc, Handle);
      }

      public static void GksEndPaint()
      {
          HDocAtlasLib.GksEndPaint();
      }

      public static void PaintEmpty()
      {
          HDocAtlasLib.PaintEmpty();
      }

      public static void PaintVizMap(int VizMapId, bool ShowPointNmP, int PointFontSize, int PointNmFontScale, double PointWgtThreshold, int CatId, bool ShowCatNmP, bool ShowKeyWdP, int KeyWdFontSize, bool ShowMgGlassP)
      {
          HDocAtlasLib.PaintVizMap(VizMapId, ShowPointNmP, PointFontSize, PointNmFontScale, PointWgtThreshold, CatId, ShowCatNmP, ShowKeyWdP, KeyWdFontSize, ShowMgGlassP);
      }

      public static void MouseEnter(int VizMapId)
      {
          HDocAtlasLib.MouseEnter(VizMapId);
      }

      public static void MouseLeave(int VizMapId)
      {
          HDocAtlasLib.MouseLeave(VizMapId);
      }

      public static bool MouseMove(int VizMapId, int X, int Y, int Width, int Height, int MgGlassKeyWds)
      {
         return HDocAtlasLib.MouseMove(VizMapId, X, Y, Width, Height, MgGlassKeyWds);
      }

      public static void MouseLeftDown(int VizMapId, int X, int Y, int Width, int Height)
      {
          HDocAtlasLib.MouseLeftDown(VizMapId, X, Y, Width, Height);
      }

      public static void MouseLeftUp(int VizMapId, int X, int Y, int Width, int Height)
      {
          HDocAtlasLib.MouseLeftUp(VizMapId, X, Y, Width, Height);
      }

      public static void MouseWheel(int VizMapId, int Degree, int MgGlassKeyWds)
      {
          HDocAtlasLib.MouseWheel(VizMapId, Degree, MgGlassKeyWds);
      }

      public static void SetZoomMode(int VizMapId)
      {
          HDocAtlasLib.SetZoomMode(VizMapId);
      }

      public static bool IsZoomMode(int VizMapId)
      {
         return HDocAtlasLib.IsZoomMode(VizMapId);
      }

      public static void SetSelectMode(int VizMapId)
      {
          HDocAtlasLib.SetSelectMode(VizMapId);
      }

      public static bool IsSelectMode(int VizMapId)
      {
         return HDocAtlasLib.IsSelectMode(VizMapId);
      }

      public static bool ZoomOut(int VizMapId)
      {
         return HDocAtlasLib.ZoomOut(VizMapId);
      }

      public static bool ZoomAll(int VizMapId)
      {
         return HDocAtlasLib.ZoomAll(VizMapId);
      }

      public static int GetSelectDIdV(int VizMapId)
      {
         return HDocAtlasLib.GetSelectDIdV(VizMapId);
      }

      public static void UnselectAll(int VizMapId)
      {
          HDocAtlasLib.UnselectAll(VizMapId);
      }

      public static int GetFrames(int VizMapId)
      {
         return HDocAtlasLib.GetFrames(VizMapId);
      }

      public static int GetFrameN(int VizMapId)
      {
         return HDocAtlasLib.GetFrameN(VizMapId);
      }

      public static void SetFrame(int VizMapId, int FrameN)
      {
          HDocAtlasLib.SetFrame(VizMapId, FrameN);
      }

      public static String GetFrameNm(int VizMapId, int FrameN)
      {
         IntPtr i = HDocAtlasLib.GetFrameNm(VizMapId, FrameN);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static int GetDocs(int VizMapId)
      {
         return HDocAtlasLib.GetDocs(VizMapId);
      }

      public static String GetDocTitle(int VizMapId, int DId)
      {
         IntPtr i = HDocAtlasLib.GetDocTitle(VizMapId, DId);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static double GetDocPosX(int VizMapId, int DId)
      {
          return HDocAtlasLib.GetDocPosX(VizMapId, DId);
      }

      public static double GetDocPosY(int VizMapId, int DId)
      {
          return HDocAtlasLib.GetDocPosY(VizMapId, DId);
      }

      public static int GetKeyWds(int VizMapId)
      {
          return HDocAtlasLib.GetKeyWds(VizMapId);
      }

      public static double GetKeyWdPosX(int VizMapId, int WId)
      {
          return HDocAtlasLib.GetKeyWdPosX(VizMapId, WId);
      }

      public static double GetKeyWdPosY(int VizMapId, int WId)
      {
          return HDocAtlasLib.GetKeyWdPosY(VizMapId, WId);
      }

      public static String GetKeyWdStr(int VizMapId, int WId)
      {
          IntPtr i = HDocAtlasLib.GetKeyWdStr(VizMapId, WId);
          string s = Marshal.PtrToStringAnsi(i);
          HDocAtlasLib.DelCStr(i);
          return s;
      }

      public static bool IsSelDoc(int VizMapId)
      {
         return HDocAtlasLib.IsSelDoc(VizMapId);
      }

      public static int SelDocId(int VizMapId)
      {
         return HDocAtlasLib.SelDocId(VizMapId);
      }

      public static String SelDocTitle(int VizMapId)
      {
         IntPtr i = HDocAtlasLib.SelDocTitle(VizMapId);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static String SelDocBody(int VizMapId)
      {
         IntPtr i = HDocAtlasLib.SelDocBody(VizMapId);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static String SelDocUrl(int VizMapId)
      {
         IntPtr i = HDocAtlasLib.SelDocUrl(VizMapId);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static void SelDoc(int VizMapId, int DocId)
      {
          HDocAtlasLib.SelDoc(VizMapId, DocId);
      }

      public static int GetCats(int VizMapId)
      {
         return HDocAtlasLib.GetCats(VizMapId);
      }

      public static String GetCatNm(int VizMapId, int CId)
      {
         IntPtr i = HDocAtlasLib.GetCatNm(VizMapId, CId);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static int GetSwSetTypeNmStrV()
      {
         return HDocAtlasLib.GetSwSetTypeNmStrV();
      }

      public static int GetSwSetTypeDNmStrV()
      {
         return HDocAtlasLib.GetSwSetTypeDNmStrV();
      }

      public static int GetStemmerTypeNmStrV()
      {
         return HDocAtlasLib.GetStemmerTypeNmStrV();
      }

      public static int GetStemmerTypeDNmStrV()
      {
         return HDocAtlasLib.GetStemmerTypeDNmStrV();
      }

      public static int GenSwSet(String SwSetType)
      {
         return HDocAtlasLib.GenSwSet(SwSetType);
      }

      public static void AddSwSetWord(int SwSetId, String WordCStr)
      {
          HDocAtlasLib.AddSwSetWord(SwSetId, WordCStr);
      }

      public static int GetXmlTypeStaticDoc()
      {
         return HDocAtlasLib.GetXmlTypeStaticDoc();
      }

      public static int GetXmlTypeDynamicDoc()
      {
         return HDocAtlasLib.GetXmlTypeDynamicDoc();
      }

      public static int GetXmlTypeStaticAuthor()
      {
         return HDocAtlasLib.GetXmlTypeStaticAuthor();
      }

      public static int GetXmlTypeDynamicAuthor()
      {
         return HDocAtlasLib.GetXmlTypeDynamicAuthor();
      }

      public static int NewFromBow(String BowFNm)
      {
         return HDocAtlasLib.NewFromBow(BowFNm);
      }

      public static int NewFromHtml(String HtmlPath, bool RecursiveP, int SwSetId, String StemmerType, int MxNGramLen, int MnNGramFq)
      {
         return HDocAtlasLib.NewFromHtml(HtmlPath, RecursiveP, SwSetId, StemmerType, MxNGramLen, MnNGramFq);
      }

      public static int NewFromLnDoc(String NmLineDocFNm, int SwSetId, String StemmerType, int MxNGramLen, int MnNGramFq)
      {
         return HDocAtlasLib.NewFromLnDoc(NmLineDocFNm, SwSetId, StemmerType, MxNGramLen, MnNGramFq);
      }

      public static int NewFromXml(String XmlFNm, int XmlMapType, int SwSetId, String StemmerType, int MxNGramLen, int MnNGramFq)
      {
         return HDocAtlasLib.NewFromXml(XmlFNm, XmlMapType, SwSetId, StemmerType, MxNGramLen, MnNGramFq);
      }

      public static int NewFromGoogleSearch(String WebQueryStr, int MxHits)
      {
         return HDocAtlasLib.NewFromGoogleSearch(WebQueryStr, MxHits);
      }

      public static int NewFromGoogleNewsSearch(String NewsQueryStr)
      {
         return HDocAtlasLib.NewFromGoogleNewsSearch(NewsQueryStr);
      }

      public static int NewFromGoogleScholar(String ScholarQueryStr)
      {
         return HDocAtlasLib.NewFromGoogleScholar(ScholarQueryStr);
      }

      public static int NewFromGoogleAuthorScholar(String ScholarAuthorQueryStr)
      {
         return HDocAtlasLib.NewFromGoogleAuthorScholar(ScholarAuthorQueryStr);
      }

      public static int NewFromGooglePublicationScholar(String ScholarPublicationQueryStr)
      {
         return HDocAtlasLib.NewFromGooglePublicationScholar(ScholarPublicationQueryStr);
      }

      public static int NewEmptyBow(bool StemmerP, bool SwSetP)
      {
         return HDocAtlasLib.NewEmptyBow(StemmerP, SwSetP);
      }

      public static bool AddWordStr(int BowDocBsId, String WordStr, int WId)
      {
         return HDocAtlasLib.AddWordStr(BowDocBsId, WordStr, WId);
      }

      public static int AddHtmlDoc(int BowDocBsId, String DocTitle, String DocBody)
      {
         return HDocAtlasLib.AddHtmlDoc(BowDocBsId, DocTitle, DocBody);
      }

      public static int AddIntFltPrV(int BowDocBsId, String DocTitle, int IntFltPrVId)
      {
         return HDocAtlasLib.AddIntFltPrV(BowDocBsId, DocTitle, IntFltPrVId);
      }

      public static int NewFromBowId(int BowDocBsId)
      {
         return HDocAtlasLib.NewFromBowId(BowDocBsId);
      }

      public static int NewFromKeyWdBowId(int BowDocBsId, int KeyWdBowDocBsId)
      {
         return HDocAtlasLib.NewFromKeyWdBowId(BowDocBsId, KeyWdBowDocBsId);
      }

      public static int LoadVizMap(String VizMapFNm)
      {
         return HDocAtlasLib.LoadVizMap(VizMapFNm);
      }

      public static void SaveVizMap(int VizMapId, String VizMapFNm)
      {
          HDocAtlasLib.SaveVizMap(VizMapId, VizMapFNm);
      }

      public static void SaveBmp(int VizMapId, String BmpFNm, int Width, int Height, bool ShowPointNmP, int PointFontSize, int PointNmFontScale, double PointWgtThreshold, int CatId, bool ShowCatNmP, bool ShowKeyWdP, int KeyWdFontSize, bool ShowMgGlassP, int LegendGridWidth, int LegendGridHeight)
      {
          HDocAtlasLib.SaveBmp(VizMapId, BmpFNm, Width, Height, ShowPointNmP, PointFontSize, PointNmFontScale, PointWgtThreshold, CatId, ShowCatNmP, ShowKeyWdP, KeyWdFontSize, ShowMgGlassP, LegendGridWidth, LegendGridHeight);
      }

      public static void SaveJpg(int VizMapId, String JpgFNm, int Width, int Height, bool ShowPointNmP, int PointFontSize, int PointNmFontScale, double PointWgtThreshold, int CatId, bool ShowCatNmP, bool ShowKeyWdP, int KeyWdFontSize, bool ShowMgGlassP, int LegendGridWidth, int LegendGridHeight)
      {
          HDocAtlasLib.SaveJpg(VizMapId, JpgFNm, Width, Height, ShowPointNmP, PointFontSize, PointNmFontScale, PointWgtThreshold, CatId, ShowCatNmP, ShowKeyWdP, KeyWdFontSize, ShowMgGlassP, LegendGridWidth, LegendGridHeight);
      }

      public static void SaveTiff(int VizMapId, String TiffFNm, int Width, int Height, bool ShowPointNmP, int PointFontSize, int PointNmFontScale, double PointWgtThreshold, int CatId, bool ShowCatNmP, bool ShowKeyWdP, int KeyWdFontSize, bool ShowMgGlassP, int LegendGridWidth, int LegendGridHeight)
      {
          HDocAtlasLib.SaveTiff(VizMapId, TiffFNm, Width, Height, ShowPointNmP, PointFontSize, PointNmFontScale, PointWgtThreshold, CatId, ShowCatNmP, ShowKeyWdP, KeyWdFontSize, ShowMgGlassP, LegendGridWidth, LegendGridHeight);
      }

      public static void SaveVrml(int VizMapId, String VrmlFNm, bool ShowPointNmP, bool ShowKeyWdP)
      {
          HDocAtlasLib.SaveVrml(VizMapId, VrmlFNm, ShowPointNmP, ShowKeyWdP);
      }

      public static void SaveLegend(int VizMapId, String TxtFNm, int LegendGridWidth, int LegendGridHeight)
      {
          HDocAtlasLib.SaveLegend(VizMapId, TxtFNm, LegendGridWidth, LegendGridHeight);
      }

      public static void LoadSearchBs(String InSearchBs)
      {
          HDocAtlasLib.LoadSearchBs(InSearchBs);
      }

      public static void Search(String QueryStr)
      {
          HDocAtlasLib.Search(QueryStr);
      }

      public static bool IsSearch()
      {
         return HDocAtlasLib.IsSearch();
      }

      public static int GetTotalHits()
      {
         return HDocAtlasLib.GetTotalHits();
      }

      public static int GetHits()
      {
         return HDocAtlasLib.GetHits();
      }

      public static String GetDateStr(int HitN)
      {
         IntPtr i = HDocAtlasLib.GetDateStr(HitN);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static String GetTitleStr(int HitN)
      {
         IntPtr i = HDocAtlasLib.GetTitleStr(HitN);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static String GetBodyStr(int HitN)
      {
         IntPtr i = HDocAtlasLib.GetBodyStr(HitN);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static String GetStaticDocVizMap(String OutVizMapFPath)
      {
         IntPtr i = HDocAtlasLib.GetStaticDocVizMap(OutVizMapFPath);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static String GetDynamicDocVizMap(String OutVizMapFPath)
      {
         IntPtr i = HDocAtlasLib.GetDynamicDocVizMap(OutVizMapFPath);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static String GetStaticNmEnVizMap(String OutVizMapFPath)
      {
         IntPtr i = HDocAtlasLib.GetStaticNmEnVizMap(OutVizMapFPath);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static String GetDynamicNmEnVizMap(String OutVizMapFPath)
      {
         IntPtr i = HDocAtlasLib.GetDynamicNmEnVizMap(OutVizMapFPath);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static void MakeTopics()
      {
          HDocAtlasLib.MakeTopics();
      }

      public static bool IsTopics()
      {
         return HDocAtlasLib.IsTopics();
      }

      public static int GetTopics()
      {
         return HDocAtlasLib.GetTopics();
      }

      public static String GetTopicNm(int TopicN)
      {
         IntPtr i = HDocAtlasLib.GetTopicNm(TopicN);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static int GetTopicFq(int TopicN)
      {
         return HDocAtlasLib.GetTopicFq(TopicN);
      }

      public static int GetTopicFrames()
      {
         return HDocAtlasLib.GetTopicFrames();
      }

      public static String GetTopicFrameNm(int FrameN)
      {
         IntPtr i = HDocAtlasLib.GetTopicFrameNm(FrameN);
         string s = Marshal.PtrToStringAnsi(i);
         HDocAtlasLib.DelCStr(i);
         return s;
      }

      public static double GetTopicFrameFq(int FrameN, int TopicN)
      {
         return HDocAtlasLib.GetTopicFrameFq(FrameN, TopicN);
      }

      public static double GetTopicFrameAllFq(int FrameN)
      {
         return HDocAtlasLib.GetTopicFrameAllFq(FrameN);
      }

      public static double GetMxTopicFrameAllFq()
      {
         return HDocAtlasLib.GetMxTopicFrameAllFq();
      }

   }
}
