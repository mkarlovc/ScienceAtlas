<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index1.aspx.cs" Inherits="basicWeb.index1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Science Atlas</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>
    <script src="http://cdn.jquerytools.org/1.2.7/all/jquery.tools.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.15/jquery-ui.min.js" type="text/javascript"></script>
    
    <script src="js/jquery.ba-bbq.js" type="text/javascript"></script>
    <script src="js/jquery.pagination.js" type="text/javascript"></script>
    <script src="js/date.js" type="text/javascript"></script>
    <script src="js/sigma.min.js" type="text/javascript"></script>
    <script src="js/sigma.forceatlas2.js" type="text/javascript"></script>

    <script src="js/graphdracula.js" type="text/javascript"></script>
    <script src="js/class.js" type="text/javascript"></script>
    <script src="js/data.js" type="text/javascript"></script>
    <script src="js/interface.js" type="text/javascript"></script>
    <script src="js/history.js" type="text/javascript"></script>

    <link href="css/main.css" rel="stylesheet" type="text/css" />
    <link href="css/divs.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery-ui-1.8.23.custom.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container"><div id="loadingDiv"><img class="loadingAnimation" src="img/loading.gif" alt="loading"/></div>
          <div class="header"><div id="logo"><a href="#home"><img src="img/logo.jpg" alt="Insert Logo Here" name="Insert_logo" width="357" height="157" id="Insert_logo" style="background-color: #FFFFFF; display: block;" /></a> </div>
            <div id="search">
     
                <label for="textfield"></label>
                <input type="text" name="textfield" id="textfield" />
                <br />
                <div id="btns">
                    <a class="btn_left" href="#home">Home</a>
                    <a class="btn_left" href="#about">About</a>
                    <a class="btn_left" href="#help">Help</a>
                </div> 
                <div id="btn_submit"><a class="btn_isci" href="#search">search</a></div>

            </div>
          <!-- end .header --></div>
          <div id="main">

          <div id="sidebar2News" class="sidebar2">
            <h1>News</h1>
            <div class="scrollable vertical sidebar2">
                <div id="items" class="items">
                    <div class="item"></div>
                </div>
            </div>
          </div>

          <div id="sidebar1Rsr" class="sidebar1">
            <div id="imgsRsr"></div>
            <h1 id="sRsrName"></h1>
            <p id="sRsrOrganization"></p>
            <p><a id="sRsrColl" href="#"><img src="img/circle.png" alt="" name="" width="42" height="40" /></a><a id="sRsrComp" href="#"><img name="" src="img/competence.png" width="42" height="40" alt="" /></a></p>
            <p>
              <span class="tag">Science:</span><br />
              <span id="sRsrScience"></span><br />
              <span class="tag">Field:</span><br />
              <span id="sRsrField"></span><br />
              <span class="tag">Subfield:</span><br />
              <span id="sRsrSubfield"></span><br />
              <span class="tag">Keywords:</span><br />
              <span id="sRsrKeywords"></span>
             </p>
          </div>

          <div id="sidebar1Prj" class="sidebar1">
            <div id="imgsPrj"></div>
            <h1 id="sPrjName"></h1>
            <p id="sPrjDate"></p>
            <p><a id="sPrjColl" href=""><img src="img/circle.png" alt="" name="" width="42" height="40" /></a><a id="sPrjComp" href="#"><img name="" src="img/competence.png" width="42" height="40" alt="" /></a></p>
            <p>
              <span id="sPrjKeywordsTag" class="tag">Keywords:</span><br />
              <span id="sPrjKeywords"></span><br />
              <span id="sPrjAbstractTag" class="tag">Abstract:</span><br />
              <span id="sPrjAbstract"></span><br />
             </p>
          </div>

          <div id="sidebar1Org" class="sidebar1">
            <div id="imgsOrg"></div>
            <h1 id="sOrgName"></h1>
            <p id="sOrgCity"></p>
            <p><a id="sOrgColl" href="#"><img src="img/circle.png" alt="" name="" width="42" height="40" /></a><a id="sOrgComp" href="#"><img name="" src="img/competence.png" width="42" height="40" alt="" /></a></p>
          </div>

          <div id="sidebar1Searching" class="sidebar1">
          <br />
            <h1>SEARCHING</h1>
            <p><a id="sSearchColl" href="#"><img src="img/circle.png" alt="" name="" width="42" height="40" /></a><a id="sSearchComp" href="#"><img name="" src="img/competence.png" width="42" height="40" alt="" /></a></p>
          </div>

          <div id="sidebar1About" class="sidebar1">
            <br />
            <h1>ABOUT</h1>
          </div>

          <div id="sidebar1Help" class="sidebar1">
            <br />
            <h1>HELP</h1>
          </div>

        <div id= "content" class="content">  

        <div id="tabs">
            <ul id="ul1">
                <li id="liT1"><a href="#t1"><img src="img/circle_small.png" width="25" height="24" /></a></li>
                <li id="liT2"><a href="#t2"><img src="img/circle_small2.png" width="25" height="24" /></a></li>
                <li id="liT3"><a href="#t3"><img src="img/dia.png" width="25" height="24" /></a></li>
                <li id="liT4"><a href="#t4"><img src="img/circle_small3.png" width="25" height="24" /></a></li>
            </ul>
            <div id="t1"></div>
            <div id="t2"></div>
            <div id="t3"></div>
            <div id="t4"></div>
        </div>

        <div id="boxContentRsr" class="boxContent">
          <h2>Researchers</h2>
          <br/>
          <div id="searchresultsRsr"> </div>
          <div id="paginatorRsr" class="paginator"> </div>
        </div>
        <div id="boxContentPrj" class="boxContent">
          <h2>Projects</h2>
          <br/>
          <div id="searchresultsPrj"> </div>
          <div id="paginatorPrj" class="paginator"> </div>
        </div>
        <div id="boxContentOrg" class="boxContent">
          <h2>Organizations</h2>
          <br/>
          <div id="searchresultsOrg"> </div>
          <div id="paginatorOrg" class="paginator"> </div>
        </div>
        <div id="boxContentLec" class="boxContent">
          <h2>Video lectures</h2>
          <br/>
          <div id="searchresultsLec"> </div>
          <div id="paginatorLec" class="paginator"> </div>
        </div>

        <div id="homeContent" class="text"><p>Welcome to Science Atlas!</p><p>Science Atlas is a web portal for analyzing the scientific community. It is applied on Slovenian, but it could be easily applied to any other scientific community. Atlas integrates data about researchers, projects and organizations from different sources. It provides analytic tools and visualizations which help the user to better understand the scientific community.</p></div>
        <div id="aboutContent" class="text"><p>About page is under construction!</p><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum egestas arcu mi. Aliquam et enim dolor. Morbi sapien erat, rutrum et viverra quis, volutpat vitae dui. Fusce eu tortor sit amet mauris pulvinar fringilla. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nulla gravida velit quis justo interdum pharetra. Sed semper est vel erat sollicitudin et rhoncus tellus facilisis. Praesent id purus eros, ut cursus augue. Nunc nec neque nisi. Donec aliquam suscipit venenatis. Suspendisse a ante nisi, at porta tortor. Fusce hendrerit auctor mauris blandit facilisis. </p></div>
        <div id="helpContent" class="text"><p>Help page is under construction!</p><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum egestas arcu mi. Aliquam et enim dolor. Morbi sapien erat, rutrum et viverra quis, volutpat vitae dui. Fusce eu tortor sit amet mauris pulvinar fringilla. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Nulla gravida velit quis justo interdum pharetra. Sed semper est vel erat sollicitudin et rhoncus tellus facilisis. Praesent id purus eros, ut cursus augue. Nunc nec neque nisi. Donec aliquam suscipit venenatis. Suspendisse a ante nisi, at porta tortor. Fusce hendrerit auctor mauris blandit facilisis. </p></div>

          </div></div>
          <div class="footer">
            <p>Atlas of Slovenian Science is being developed by the Artificial Intelligence Laboratory of the Jozef Stefan Institute and it is funded by the Slovenian Research Agency.</p>
          <!-- end .footer --></div>
          <!-- end .container --></div>
    </form>
</body>
</html>
