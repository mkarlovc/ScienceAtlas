/// <reference path="class.js" />
function getData(_method, d) {
    $.ajax({
        type: 'POST',
        url: 'AjaxServer1.aspx',
        data: { data: d, method: _method, language: dataLanguage },
        success: function (data) {

            if (_method == "getNews") {
                saveNews(data);
                printNews(news_list);
            }
            if (_method == "rsrCollArray") {
                closeAllSidebars();
                closeAllDivs();

                fillTabRsrColl();

                $('#sidebar1Rsr').show();

                saveRsr(data[2]);
                fillSidebarRsr(researchers, data[0]);
                g1 = saveGraph(data[3]);
                drawDiagramCollMap();

                saveRsr(data[0]);
                g = saveGraph(data[1]);
                printRsr(researchers);
                drawDiagramCollCenter();

                saveOrg(data[4]);
                g3 = saveGraph(data[5]);
                printOrg(organizations);
                drawDiagramCollOrg();

                printPrj(projects);

                $('#t1').append($('#sigCollCenter'));
                $('#t2').append($('#sigCollMap'));
                $('#t3').append($('#sigCollOrg'));

                $('#tabs').show();
                $('#liT1').show();
                $('#liT2').show();
                $('#liT3').show();
                $('#liT4').hide();
                $("#tabs").tabs({ selected: 0 });
            }
            if (_method == "rsrColl") {
                closeAllSidebars();
                closeAllDivs();

                fillTabRsrColl();

                $('#sidebar1Rsr').show();

                saveRsr(data[2]);
                fillSidebarRsr(researchers, data[0]);
                g1 = saveGraph(data[3]);
                drawDiagramCollMap();

                saveRsr(data[0]);
                g = saveGraph(data[1]);
                printRsr(researchers);
                drawDiagramCollCenter();

                saveOrg(data[4]);
                g3 = saveGraph(data[5]);
                printOrg(organizations);
                drawDiagramCollOrg();

                printPrj(projects);

                $('#t1').append($('#sigCollCenter'));
                $('#t2').append($('#sigCollMap'));
                $('#t3').append($('#sigCollOrg'));

                $('#tabs').show();
                $('#liT1').show();
                $('#liT2').show();
                $('#liT3').show();
                $('#liT4').hide();
                $("#tabs").tabs({ selected: 0 });
            }
            if (_method == "rsrCompMap") {
                closeAllSidebars();
                closeAllDivs();

                fillTabRsrCompMap();
                $('#sigCompMap').show();

                $('#sidebar1Rsr').show();

                saveRsr(data[3]);

                savePrj(data[0]);
                saveKeyws(data[1]);
                g2 = saveGraph(data[2]);
                fillSidebarRsr(researchers, g2);

                printPrj(projects);
                drawDiagramCompMap();

                $('#t4').append($('#sigCompMap'));

                $('#tabs').show();
                $('#liT1').hide();
                $('#liT2').hide();
                $('#liT3').hide();
                $('#liT4').show();
                $("#tabs").tabs({ selected: 3 });
            }
            if (_method == "orgCompMap") {
                closeAllSidebars();
                closeAllDivs();

                fillTabRsrCompMap();
                $('#sigCompMap').show();

                $('#sidebar1Org').show();
                fillSidebarOrg(data[3]);

                savePrj(data[0]);
                saveKeyws(data[1]);
                g2 = saveGraph(data[2]);
                printPrj(projects);
                drawDiagramCompMap();

                $('#t4').append($('#sigCompMap'));

                $('#tabs').show();
                $('#liT1').hide();
                $('#liT2').hide();
                $('#liT3').hide();
                $('#liT4').show();
                $("#tabs").tabs({ selected: 3 });
            }
            if (_method == "collMapArray") {
                closeAllSidebars();
                closeAllDivs();

                closeAllDivs();

                fillTabRsrCollMap();
                $('#sigCollMap').show();
                $('#sigCollOrg').show();

                $('#sidebar1Searching').show();
                fillSidebarSearch();

                saveRsr(data[0]);
                g1 = saveGraph(data[1]);
                drawDiagramCollMap();

                g3 = saveGraph(data[3]);
                saveOrg(data[2]);
                drawDiagramCollOrg();
                printOrg(organizations);

                printRsr(researchers);

                printPrj(projects);

                $('#t2').append($('#sigCollMap'));
                $('#t3').append($('#sigCollOrg'));

                $('#tabs').show();
                $('#liT1').hide();
                $('#liT2').show();
                $('#liT3').show();
                $('#liT4').hide();
                $("#tabs").tabs({ selected: 1 });
            }

            if (_method == "compMapSearch") {
                closeAllSidebars();
                closeAllDivs();

                fillTabRsrCompMap();
                $('#sigCompMap').show();

                $('#sidebar1Searching').show();
                fillSidebarSearch();

                savePrj(data[0]);
                saveKeyws(data[1]);
                g2 = saveGraph(data[2]);
                printPrj(projects);
                drawDiagramCompMap();

                printRsr(researchers);

                $('#t4').append($('#sigCompMap'));

                $('#tabs').show();
                $('#liT1').hide();
                $('#liT2').hide();
                $('#liT3').hide();
                $('#liT4').show();
                $("#tabs").tabs({ selected: 3 });
            }

            if (_method == "prjCollArray") {
                closeAllSidebars();
                closeAllDivs();

                fillTabRsrCollMap();
                $('#sigCollMap').show();
                $('#sigCollOrg').show();

                $('#sidebar1Prj').show();
                savePrj(data[4]);
                fillSidebarPrj(projects);

                saveRsr(data[0]);
                g1 = saveGraph(data[1]);
                drawDiagramCollMap();

                g3 = saveGraph(data[3]);
                saveOrg(data[2]);
                drawDiagramCollOrg();
                printOrg(organizations);

                printRsr(researchers);

                printPrj(projects);

                $('#t2').append($('#sigCollMap'));
                $('#t3').append($('#sigCollOrg'));

                $('#tabs').show();
                $('#liT1').hide();
                $('#liT2').show();
                $('#liT3').show();
                $('#liT4').hide();
                $("#tabs").tabs({ selected: 1 });
            }

            if (_method == "orgCollArray") {
                closeAllSidebars();
                closeAllDivs();

                fillTabRsrCollMap();
                $('#sigCollMap').show();
                $('#sigCollOrg').show();

                $('#sidebar1Org').show();
                fillSidebarOrg(data[4]);

                saveRsr(data[0]);
                g1 = saveGraph(data[1]);
                drawDiagramCollMap();

                g3 = saveGraph(data[3]);
                saveOrg(data[2]);
                drawDiagramCollOrg();
                printOrg(organizations);

                printRsr(researchers);

                printPrj(projects);

                $('#t2').append($('#sigCollMap'));
                $('#t3').append($('#sigCollOrg'));

                $('#tabs').show();
                $('#liT1').hide();
                $('#liT2').show();
                $('#liT3').show();
                $('#liT4').hide();
                $("#tabs").tabs({ selected: 1 });
            }

            if (_method == "query") {
                closeAllSidebars();
                closeAllDivs();
                $('#sidebar1Searching').show('slow');
                saveRsr(data[0]);
                savePrj(data[1]);
                saveOrg(data[2]);
                fillSidebarSearch();
                printRsr(researchers);
                printPrj(projects);
                printOrg(organizations);
            }
            if (_method == "profileOrg") {
                closeAllSidebars();
                closeAllDivs();
                $('#sidebar1Org').show('slow');
                saveRsr(data[0]);
                savePrj(data[1]);
                fillSidebarOrg(data[2]);
                printRsr(researchers);
                printPrj(projects);
            }
            if (_method == "profilePrj") {
                closeAllSidebars();
                closeAllDivs();
                $('#sidebar1Prj').show('slow');
                savePrj(data[1]);
                fillSidebarPrj(projects);
                saveRsr(data[0]);
                printRsr(researchers);
            }
            if (_method == "profileRsr") {
                closeAllSidebars();
                closeAllDivs();
                $('#sidebar1Rsr').show('slow');
                saveRsr(data[3]);
                savePrj(data[1]);
                saveLec(data[4]);
                fillSidebarRsr(researchers, data[0]);
                saveRsr(data[0]);
                g = saveGraph(data[2]);
                printRsr(researchers);
                printPrj(projects);
                printLec(lectures);
            }
        },
        dataType: 'json'
    });
}

function saveRsr(data) {
    researchers = [];
    if (dataLanguage == "slv") {
        for (var i = 0; i < data.length; i++) {
            researchers.push(new researcher(data[i].Id, data[i].First_name, data[i].Last_name, data[i].Keyws, data[i].OrgName1, data[i].OrgId1, data[i].OrgName2, data[i].OrgId2, data[i].OrgName3, data[i].OrgName3, data[i].Science, data[i].ScienceCode, data[i].Field, data[i].FieldCode, data[i].Subfield, data[i].SubfieldCode, data[i].Tel, data[i].Fax, data[i].Email, data[i].Url, data[i].X, data[i].Y, data[i].Vd));
        }
    }
    else if (dataLanguage == "eng") {
        for (var i = 0; i < data.length; i++) {
            researchers.push(new researcher(data[i].Id, data[i].First_name, data[i].Last_name, data[i].Keyws_en, data[i].OrgName1, data[i].OrgId1, data[i].OrgName2, data[i].OrgId2, data[i].OrgName3, data[i].OrgName3, data[i].Science_en, data[i].ScienceCode, data[i].Field_en, data[i].FieldCode, data[i].Subfield_en, data[i].SubfieldCode, data[i].Tel, data[i].Fax, data[i].Email, data[i].Url, data[i].X, data[i].Y, data[i].Vd));
        }
    }
    else { }
}

function savePrj(data) {
    projects = [];
    
    if (dataLanguage == "slv") {
        for (var i = 0; i < data.length; i++) {
            projects.push(new project(data[i].Id, data[i].Name, data[i].Abstract, data[i].Keywords, data[i].SignificanceDomestic, data[i].SignificanceWorld, data[i].Startdate, data[i].Enddate, data[i].ScienceCode, data[i].FieldCode, data[i].SubfieldCode, data[i].Science, data[i].Field, data[i].Subfield, data[i].X, data[i].Y, data[i].Vd));
        }
    }
    else if (dataLanguage == "eng") {
        for (var i = 0; i < data.length; i++) {
            projects.push(new project(data[i].Id, data[i].Name_en, data[i].Abstract_en, data[i].Keywords_en, data[i].SignificanceDomestic_en, data[i].SignificanceWorld_en, data[i].Startdate, data[i].Enddate, data[i].ScienceCode, data[i].FieldCode, data[i].SubfieldCode, data[i].Science_en, data[i].Field_en, data[i].Subfield_en, data[i].X, data[i].Y, data[i].Vd));
        }
    }
    else { }
}

function saveOrg(data) {
    organizations = [];
    for (var i = 0; i < data.length; i++) {
        organizations.push(new organization(data[i].Id, data[i].Name, data[i].Orgtype, data[i].City, data[i].N, data[i].X, data[i].Y, data[i].Vd));
    }
}

function saveLec(data) {
    lectures = [];
    for (var i = 0; i < data.length; i++) {
        lectures.push(new lecture(data[i].Uri, data[i].Url, data[i].Enabled, data[i].Ispublic, data[i].Title, data[i].Description, data[i].Type, data[i].Recorded, data[i].Published, data[i].Views, data[i].Role));
    }
}

function saveKeyws(data) {
    keywords = [];
    for (var i = 0; i < data.length; i++) {
        keywords.push(new keyword(data[i].Id, data[i].Word, data[i].N, data[i].X, data[i].Y, data[i].Vd));
    }
}

function saveNews(data) {
    news_list = [];
    for (var i = 0; i < data.length; i++) {
        news_list.push(new news(data[i].Id, data[i].Title, data[i].Date, data[i].Body, data[i].Uri, data[i].Image, data[i].Language));
    }
}

function saveGraph(data) {
    var g_ = new Array();
    g_ = [];
    for (var i = 0; i < data.length; i++) {
        g_.push(new graph(data[i].N1, data[i].N2, data[i].Weight));
    }
    return g_;
}