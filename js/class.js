var dataLanguage;
dataLanguage = "eng";

var researchers;
researchers = new Array();

function researcher(id, firstName, lastName, keyws, orgName1, orgId1, orgName2, orgId2, orgName3, orgId3, science, scienceId, field, fieldId, subfield, subfieldId, tel, fax, email, url, x, y, vd) {
    this.id = id;
    this.firstName = firstName;
    this.lastName = lastName;
    this.keyws = keyws;
    this.orgName1 = orgName1;
    this.orgId1 = orgId1;
    this.orgName2 = orgName2;
    this.orgId2 = orgId2;
    this.orgName3 = orgName3;
    this.orgId3 = orgId3;
    this.science = science;
    this.field = field;
    this.subfield = subfield;
    this.scienceId = scienceId;
    this.fieldId = fieldId;
    this.subfieldId = subfieldId;
    this.tel = tel;
    this.fax = fax;
    this.email = email;
    this.url = url;
    this.x = x;
    this.y = y;
    this.vd = vd;
}

var projects;
projects = new Array();

function project(id, name, abstract, keyws, worldSign, domSign, start, end,scienceCode, fieldCode, subfieldCode, science,field,subfield, x, y, vd) {
    this.id = id;
    this.name = name;
    this.abstract = abstract;
    this.keyws = keyws;
    this.worldSign = worldSign;
    this.domSign = domSign;
    this.start = start;
    this.end = end;
    this.scienceCode = scienceCode;
    this.fieldCode = fieldCode;
    this.subfieldCode = subfieldCode;
    this.science = science;
    this.field = field;
    this.subfield = subfield;
    this.x = x;
    this.y = y;
    this.vd = vd;
}

var organizations;
organizations = new Array();

function organization(id, name, type, city, n, x, y, vd) {
    this.id = id;
    this.name = name;
    this.type = type;
    this.city = city;
    this.n = n;
    this.x = x;
    this.y = y;
    this.vd = vd;
}

var keywords;
keywords = new Array();

function keyword(id, word, n, x, y, vd) {
    this.id = id;
    this.word = word;
    this.n = n;
    this.x = x;
    this.y = y;
    this.vd = vd;
}

var news_list;
news_list = new Array();

function news(id, title, date, body, uri, image, language) {
    this.id = id;
    this.title = title;
    this.date = date;
    this.body = body;
    this.uri = uri;
    this.image = image;
    this.language = language;
}

var lectures;
lectures = new Array();

function lecture(url, lang, enabled, ispublic, title, description, type, recorded, published, views, role) {
    this.url = url;
    this.lang = lang;
    this.enabled = enabled;
    this.ispublic = ispublic;
    this.title = title;
    this.description = description;
    this.type = type;
    this.recorded = recorded;
    this.published = published;
    this.views = views;
    this.role = role;
}

var g; //Rsr graph Cebter
g = new Array();

var g1; //Rsr graph Map
g = new Array();

var g2; //Prj-keyws graph
g1 = new Array();

var g3; //Org graph
g2 = new Array();

function graph(n1, n2, weight) {
    this.n1 = n1;
    this.n2 = n2;
    this.weight = weight;
}

// default color of node names for all diagrams
var _defaultLabelColor = "#000";
// colors for nodes - each color represents one science field; first and second color are for business and research organizations respectively
var science_color = new Array("#9B9B9B", "#0E53A7", "#0ACF00", "#A64300", "#FF6700", "#A64300", "#0ACF00", "#BF381A");
//0-unknown; 1-; 2-; 3-; 4-; 5-; 6-;
var science_color1 = new Array("#FF6700", "#0099ff", "#BF381A", "#A0D8F1", "#E9AF32", "#E07628", "#00BE2C", "#A64300");
