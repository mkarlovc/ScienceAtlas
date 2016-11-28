<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index2.aspx.cs" Inherits="basicWeb.Index2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

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
    
    <script type='text/javascript'>
        $('.sig').width($(document).width() + 'px');
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container"><div id="loadingDiv"><img class="loadingAnimation" src="img/loading.gif" alt="loading"/></div>

          <!-- end .header --></div>
          <div id="main">





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

          </div>

    </form>
</body>
</html>
