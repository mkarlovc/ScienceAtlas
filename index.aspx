<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="basicWeb.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>
    <script src="js/jquery.ba-bbq.js" type="text/javascript"></script>

    <script src="js/sigma.min.js" type="text/javascript"></script>
    <script src="js/graphdracula.js" type="text/javascript"></script>
    <script src="js/class.js" type="text/javascript"></script>
    <script src="js/data.js" type="text/javascript"></script>
    <script src="js/interface.js" type="text/javascript"></script>
    <script src="js/history.js" type="text/javascript"></script>
    <link href="css/Style1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="textfield" type="text" /><a id="search" href="#search">search</a>  <br />
        <a href="#collMapArray">for all</a> <a href="#compMapArray">all prj</a><br /><br />
        <div id="rsr" class="dat"></div><br /> <div id="prj" class="dat"></div> <br /><div id="org" class="dat"></div> <br />
        <div id="sig1" class="sig"></div> <br />
        <div id="sig" class="sig"></div> <br />
        <div id="sig2" class="sig"></div><br />
        <div id="sig3" class="sig"></div>
    </div>
    </form>
</body>
</html>
