/// <reference path="class.js" />


$(document).ready(function () {

    $("#textfield").keypress(function (e) {
        if (e.which == 13) $("#search").trigger('click');
        if (e.which == 13) return false;
        //or...
        if (e.which == 13) e.preventDefault();
    });

    $("#textfield").keyup(function () {
        if ($("#textfield").val().length > 1) {
            getData('query', $("#textfield").val());
        }
    });

    $('#loadingDiv')
    .hide()  // hide it initially
    .ajaxStart(function () {
        $(this).show();
    })
    .ajaxStop(function () {
        $(this).hide();
    });

    if ($(location).attr('href') == "http://scienceatlas.ijs.si/index1.aspx" || $(location).attr('href') == "http://scienceatlas.ijs.si:8888"|| $(location).attr('href') == "http://scienceatlas.ijs.si/"){
      goHome();
      getData('getNews', 'http://newsfeed.ijs.si/query/news-search?qjson={%22$from%22:%22Article%22,%22Title%22:%20%22science%22,%20%22Title%22:%20%22research%22}');
    }

    $(function () {
        // initialize scrollable with mousewheel support
        $(".scrollable").scrollable({ vertical: true, mousewheel: true, speed: 500, circular: true }).autoscroll({ autoplay: true, steps: 1, interval: 3000 });
    });

});

function printRsr(data) {
    $("#rsr").empty()
    for (var i = 0; i < data.length; i++) {
        var url = "#profileRsr";
        var paramsObj = { id: data[i].id };
        $("#rsr").append("<a href=" + $.param.querystring(url, paramsObj) + ">" + data[i].firstName + " " + data[i].lastName + "</a>; ");
    }
    if (data.length > 0) {
        $("#boxContentRsr").show('slow');
        intializePaginationRsr(data);
    }
    else {
        $("#boxContentRsr").hide();
    }
}

function printPrj(data) {
    $("#prj").empty()
    for (var i = 0; i < data.length; i++) {
        var url = "#profilePrj";
        var paramsObj = { id: data[i].id };
        $("#prj").append("<a href=" + $.param.querystring(url, paramsObj) + ">" + data[i].name + "</a>; ");
    }
    if (data.length > 0) {
        $("#boxContentPrj").show('slow');
        intializePaginationPrj(data);
    }
    else {
        $("#boxContentPrj").hide();
    }
}

function printOrg(data) {
    $("#org").empty()
    for (var i = 0; i < data.length; i++) {
        var url = "#profileOrg";
        var paramsObj = { id: data[i].id };
        $("#org").append("<a href=" + $.param.querystring(url, paramsObj) + ">" + data[i].name + "</a>; ");
    }
    if (data.length > 0) {
        $("#boxContentOrg").show('slow');
        intializePaginationOrg(data);
    }
    else {
        $("#boxContentOrg").hide();
    }
}

function printLec(data) {
    $("#lec").empty()/*
    for (var i = 0; i < data.length; i++) {
        var $a = $('<a/>');
        $a.attr('href', 'http://videolectures.net/' + data[i].url);
        $a.text(data[i].title);

        var $img = $('<img/>');
        $img.attr('src', 'http://videolectures.net/' + data[i].url + '/thumb.jpg');

        $("#lec").append($img);
    }*/
    if (data.length > 0) {
        $("#boxContentLec").show('slow');
        intializePaginationLec(data);
    }
    else {
        $("#boxContentLec").hide();
    }
}

function printNews(data) {
    $("#items").empty()
    for (var i = 0; i < data.length; i++) {
        var $newdiv1 = $('<div class="item" />');
        var $h = $('<h2 />');
        var $p = $('<p class="itemBody"/>');
        var $p1 = $('<p class="itemDate"/>');
        var $a = $('<a />');

        $a.attr('href', data[i].uri);
        $a.attr('target', '_blank');
        $a.text(data[i].title);

        $h.append($a);
        //alert(data[i].date);
        var d1 = new Date(data[i].date);
        $p1.text(d1.toDateString());

        if (data[i].body.length>400)
            $p.text(data[i].body.substring(0, 400) + "...");
        else
            $p.text(data[i].body);
        
        $newdiv1.append($h);
        $newdiv1.append($p1);

        if (data[i].image != "") {
            var $img = $('<img class="itemImg"/>');
            $img.attr('src', data[i].image);
            $newdiv1.append($img);
        }

        $newdiv1.append($p);

        $('#items').append($newdiv1);
    }
    var siteRegEx = /(\b(http|https?|ftp|file):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/ig;
    $(".itemBody").filter(function () {
        return $(this).html().match(siteRegEx);
    }).each(function () {
        $(this).html($(this).html().replace(siteRegEx, "<a href=\"$1\">$1</a>"));
    });
}

function fillSidebarRsr(data,data1) {
    data = data[0];

    if (data.orgName1 != "")
        $("#sRsrOrganization").html(data.orgName1);
    if (data.orgName2 != "")
        $("#sRsrOrganization").append("<br />" + data.orgName2);
    if (data.orgName3 != "")
        $("#sRsrOrganization").append("<br />" + data.orgName3);

    $("#sRsrName").html(data.firstName + " " + data.lastName);
    $("#sRsrScience").html(data.science);
    $("#sRsrField").html(data.field);
    $("#sRsrSubfield").html(data.subfield);
    $("#sRsrKeywords").html(data.keyws);

    if (data1.length > 1) {
        $("#sRsrColl").show();
        $("#sRsrColl").attr("href", "#rsrCollArray");
    }
    else{
        $("#sRsrColl").hide();
    }

    if (projects.length > 1) {
        $("#sRsrComp").show();
        var url = "#rsrCompMap";
        var paramsObj = { id: data.id };
        $("#sRsrComp").attr('href', $.param.querystring(url, paramsObj));
    }
    else{
        $("#sRsrComp").hide();
    }
    
    var $img = $('<img src="http://scienceatlas.ijs.si/css/images/sicris.png"/>');
    var $a = $('<a href=\"http://sicris.izum.si/search/rsr.aspx?lang=slv&id='+data.id+'\" targe=\"_blank\"/>');
    $a.append($img);
    $("#imgsRsr").empty();
    $("#imgsRsr").append($a);
}

function fillSidebarPrj(data) {
    data = data[0];
    $("#sPrjName").html(data.name);
    $("#sPrjDate").html(data.start + " - " + data.end);

    if (data.keyws != "") {
        $("#sPrjKeywordsTag").show();
        $("#sPrjKeywords").html(data.keyws);
    }
    else {
        $("#sPrjKeywordsTag").hide();
    }

    if (data.abstract != "") {
        $("#sPrjAbstractTag").show();
        if (data.abstract.length > 200) {
        var abst = data.abstract.replace(/[\[\]"]+/g, '');
            $("#sPrjAbstract").html(data.abstract.substring(0, 200) + "... <a href='javascript:moreAbstract(\""+abst+"\")'>(more)</a>");
        }
        else{
            $("#sPrjAbstract").html(data.abstract);
        }
    }
    else {
        $("#sPrjAbstractTag").hide();
    }

    if (researchers.length > 1) {
        $("#sPrjColl").show();
        var url = "#prjCollArray";
        var paramsObj = { id: data.id };
        $("#sPrjColl").attr('href', $.param.querystring(url, paramsObj));
    }
    else {
        $("#sPrjColl").hide();
    }

    if (projects.length > 1) {
        $("#sPrjComp").show();
        $("#sPrjComp").attr("href", "#compMapArray");
    }
    else {
        $("#sPrjComp").hide();
    }

    var $img = $('<img src="http://scienceatlas.ijs.si/css/images/sicris.png"/>');
    var $a = $('<a href=\"http://sicris.izum.si/search/prj.aspx?lang=slv&id='+data.id+'\" targe=\"_blank\"/>');
    $a.append($img);
    $("#imgsPrj").empty();
    $("#imgsPrj").append($a);
}

function moreAbstract(data){
  $("#sPrjAbstract").empty;
  $("#sPrjAbstract").html(data+ " <a href='javascript:collapseAbstract(\""+data+"\")'>(collapse)</a>");
}
function collapseAbstract(data){
  $("#sPrjAbstract").empty;
  $("#sPrjAbstract").html(data.substring(0, 200) + "... <a href='javascript:moreAbstract(\""+data+"\")'>(more)</a>");
}

function fillSidebarOrg(data) {
    data = data[0];
    $("#sOrgName").html(data.Name);
    $("#sOrgCity").html(data.City);

    if (researchers.length > 1) {
        $("#sOrgColl").show();
        var url = "#orgCollArray";
        var paramsObj = { id: data.Id };
        $("#sOrgColl").attr('href', $.param.querystring(url, paramsObj));
    }
    else {
        $("#sOrgColl").hide();
    }

    if (projects.length > 1) {
        $("#sOrgComp").show();
        $("#sOrgComp").attr("href", "#compMapArray");
        var url = "#orgCompMap";
        var paramsObj = { id: data.Id };
        $("#sOrgComp").attr('href', $.param.querystring(url, paramsObj));
    }
    else {
        $("#sOrgComp").hide();
    }

    var $img = $('<img src="http://scienceatlas.ijs.si/css/images/sicris.png"/>');
    var $a = $('<a href=\"http://sicris.izum.si/search/org.aspx?lang=slv&id='+data.Id+'\" targe=\"_blank\"/>');
    $a.append($img);
    $("#imgsOrg").empty();
    $("#imgsOrg").append($a);
}

function fillTabRsrColl() {
    $('#t1').empty();
    $('#t2').empty();
    $('#t3').empty();
    $('#t4').empty();
    var $newdiv1 = $('<div id="sigCollCenter" class="sig" />');
    var $newdiv2 = $('<div id="sigCollMap" class="sig" />');
    var $newdiv3 = $('<div id="sigCollOrg" class="sig" />');
    $('#main').append($newdiv1);
    $('#main').append($newdiv2);
    $('#main').append($newdiv3);
}

function fillTabRsrCollMap() {
    $('#t1').empty();
    $('#t2').empty();
    $('#t3').empty();
    $('#t4').empty();
    var $newdiv2 = $('<div id="sigCollMap" class="sig" />');
    var $newdiv3 = $('<div id="sigCollOrg" class="sig" />');
    $('#main').append($newdiv2);
    $('#main').append($newdiv3);
}

function fillTabRsrCompMap() {
    $('#t1').empty();
    $('#t2').empty();
    $('#t3').empty();
    $('#t4').empty();
    var $newdiv2 = $('<div id="sigCompMap" class="sig" />');
    var $compMapLayout = $('<input type=\"button\" id="compMapLayout" value=\"Stop Layout\" />');
    $('#t4').append($compMapLayout);

    $('#main').append($newdiv2);
}

function fillSidebarSearch(data) {
    if (researchers.length > 1) {
        $("#sSearchColl").attr("href", "#collMapArray");
        $("#sSearchColl").show('slow');
    }
    else {
        $("#sSearchColl").attr("href", "#");
        $("#sSearchColl").hide('hide');
    }
    if (projects.length > 1) {
        $("#sSearchComp").attr("href", "#compMapSearch");
        $("#sSearchComp").show('slow');
    }
    else {
        $("#sSearchComp").attr("href", "#");
        $("#sSearchComp").hide('slow');
    }
}

function goHome() {
    $('#sidebar1Rsr').hide();
    $('#sidebar1Prj').hide();
    $('#sidebar1Org').hide();
    $('#sidebar1Searching').hide();

    $('#boxContentRsr').hide();
    $('#boxContentPrj').hide();
    $('#boxContentOrg').hide();

    $('#tabs').hide();
    $('#boxContentPrj').hide();
    $('#boxContentOrg').hide();
    $('#boxContentLec').hide();

    $('#sidebar2News').show();
    $('#sidebar1About').hide();
    $('#sidebar1Help').hide();

    $('#homeContent').show();
    $('#aboutContent').hide();
    $('#helpContent').hide();
}
function goAbout() {
    $('#sidebar1Rsr').hide();
    $('#sidebar1Prj').hide();
    $('#sidebar1Org').hide();
    $('#sidebar1Searching').hide();

    $('#boxContentRsr').hide();
    $('#boxContentPrj').hide();
    $('#boxContentOrg').hide();

    $('#tabs').hide();
    $('#boxContentPrj').hide();
    $('#boxContentOrg').hide();
    $('#boxContentLec').hide();

    $('#sidebar2News').hide();
    $('#sidebar1About').show();
    $('#sidebar1Help').hide();

    $('#homeContent').hide();
    $('#aboutContent').show();
    $('#helpContent').hide();
}
function goHelp() {
    $('#sidebar1Rsr').hide();
    $('#sidebar1Prj').hide();
    $('#sidebar1Org').hide();
    $('#sidebar1Searching').hide();

    $('#boxContentRsr').hide();
    $('#boxContentPrj').hide();
    $('#boxContentOrg').hide();

    $('#tabs').hide();
    $('#boxContentPrj').hide();
    $('#boxContentOrg').hide();
    $('#boxContentLec').hide();

    $('#sidebar2News').hide();
    $('#sidebar1About').hide();
    $('#sidebar1Help').show();

    $('#homeContent').hide();
    $('#aboutContent').hide();
    $('#helpContent').show();
}
function closeAllSidebars() {
    $('#sidebar1Rsr').hide();
    $('#sidebar1Prj').hide();
    $('#sidebar1Org').hide();
    $('#sidebar1Searching').hide();
    $('#sidebar2News').hide();
    $('#sidebar1About').hide();
    $('#sidebar1Help').hide();
}
function closeAllDivs() {
    $('#boxContentRsr').hide();
    $('#boxContentPrj').hide();
    $('#boxContentOrg').hide();
    $('#boxContentLec').hide();

    $('#homeContent').hide();
    $('#aboutContent').hide();
    $('#helpContent').hide();

    $('#tabs').hide();

    $('#sig1').empty();
    $('#sigCollMap').hide();
    $('#sigCollCenter').hide();
    $('#sigCollOrg').hide();
    $('#sigCompMap').hide();
    
}

// PAGINATIONS
function intializePaginationRsr(data,div) {
    var opt = { callback: pageselectCallbackRsr,num_display_entries:5,items_per_page:5 };
    $("#paginatorRsr").pagination(data.length,opt);
}
function pageselectCallbackRsr(page_index, jq) {
    var items_per_page = 5;
    var data = researchers;
    var max_elem = Math.min((page_index + 1) * items_per_page, data.length);
    var newcontent = '';
    for (var i = page_index * items_per_page; i < max_elem; i++) {
        var url = "#profileRsr";
        var paramsObj = { id: data[i].id };
        newcontent += "<p><a href=" + $.param.querystring(url, paramsObj) + ">" + data[i].firstName + " " + data[i].lastName + "</a></p>";
    }
    $('#searchresultsRsr').html(newcontent);
    return false;
}

function intializePaginationPrj(data, div) {
    var opt = { callback: pageselectCallbackPrj, num_display_entries: 5, items_per_page: 5 };
    $("#paginatorPrj").pagination(data.length, opt);
}
function pageselectCallbackPrj(page_index, jq) {
    var items_per_page = 5;
    var data = projects;
    var max_elem = Math.min((page_index + 1) * items_per_page, data.length);
    var newcontent = '';

    for (var i = page_index * items_per_page; i < max_elem; i++) {
        var url = "#profilePrj";
        var paramsObj = { id: data[i].id };
        newcontent += "<p><a href=" + $.param.querystring(url, paramsObj) + ">" + data[i].name + "</a></p>";
    }
    $('#searchresultsPrj').html(newcontent);
    return false;
}

function intializePaginationOrg(data, div) {
    var opt = { callback: pageselectCallbackOrg, num_display_entries: 5, items_per_page: 5 };
    $("#paginatorOrg").pagination(data.length, opt);
}
function pageselectCallbackOrg(page_index, jq) {
    var items_per_page = 5;
    var data = organizations;
    var max_elem = Math.min((page_index + 1) * items_per_page, data.length);
    var newcontent = '';

    for (var i = page_index * items_per_page; i < max_elem; i++) {
        var url = "#profileOrg";
        var paramsObj = { id: data[i].id };
        newcontent += "<p><a href=" + $.param.querystring(url, paramsObj) + ">" + data[i].name + "</a></p>";
    }
    $('#searchresultsOrg').html(newcontent);
    return false;
}

function intializePaginationLec(data, div) {
    var opt = { callback: pageselectCallbackLec, num_display_entries: 5, items_per_page: 5 };
    $("#paginatorLec").pagination(data.length, opt);
}
function pageselectCallbackLec(page_index, jq) {
    var items_per_page = 4;
    var data = lectures;
    var max_elem = Math.min((page_index + 1) * items_per_page, data.length);
    var newcontent = '';

    for (var i = page_index * items_per_page; i < max_elem; i++) {
        newcontent += "<div><img class='pagingImg' src='http://videolectures.net/" + data[i].url + "/thumb.jpg' target='_blank' onerror='javascript:ImgError(this)'/><p><a class='pagingText' href='http://videolectures.net/" + data[i].url + "'>" + data[i].title + "</a></p></div>";
    }
    $('#searchresultsLec').html(newcontent);
    return false;
}

function ImgError(source){
    source.src = "/img/film.png";
    source.onerror = "";
    return true;
}