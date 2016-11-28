
$(function () {
    $("a").click(function () {
        var href = $(this).attr("href");

        if ($.param.fragment(href) == "search") {
            $.bbq.pushState({ url: href, data: $('#textfield').val() });
        }
        else if ($.param.fragment(href) == "collMapArray") {
            var array = "";
            for (var i = 0; i < researchers.length; i++) {
                array += researchers[i].id;
                if (i < (researchers.length - 1))
                    array += ",";
            }

            $.bbq.pushState({ url: href, data: array });
        }
        else if ($.param.fragment(href) == "rsrCollArray") {
            var array = "";
            for (var i = 0; i < researchers.length; i++) {
                array += researchers[i].id;
                if (i < (researchers.length - 1))
                    array += ",";
            }

            $.bbq.pushState({ url: href, data: array });
        }
        else if ($.param.fragment(href) == "collOrgArray") {
            var array = "";
            for (var i = 0; i < researchers.length; i++) {
                array += researchers[i].id;
                if (i < (researchers.length - 1))
                    array += ",";
            }

            $.bbq.pushState({ url: href, data: array });
        }
        else if ($.param.fragment(href) == "compMapArray") {
            var array = "";
            for (var i = 0; i < projects.length; i++) {
                array += projects[i].id;
                if (i < (projects.length - 1))
                    array += ",";
            }

            $.bbq.pushState({ url: href, data: array });
        }
        else if ($.param.fragment(href) == "compMapSearch") {
            var array = "";
            for (var i = 0; i < projects.length; i++) {
                array += projects[i].id;
                if (i < (projects.length - 1))
                    array += ",";
            }

            $.bbq.pushState({ url: href, data: array });
        }

        else if ($.param.fragment(href) == "collCenterArray") {
            $.bbq.pushState({ url: href, data: researchers[0].id });
        }

        else
            $.bbq.pushState({ url: href });

        return false;
    });

    $(window).bind("hashchange", function (e) {

        var url = $.bbq.getState("url");
        var hash = $.param.fragment(url);

        $("a").each(function () {
            var href = $(this).attr("href");

            if (href === url) {
                $(this).addClass("current");
            } else {
                $(this).removeClass("current");
            }
        });
        // You probably want to actually do something useful here..
        if (hash == "collCenter") {
            getData('collCenter', $.deparam.querystring(url).id);
        }
        if (hash == "collMapArray") {
            getData('collMapArray', $.bbq.getState("data"));
        }
        if (hash == "rsrCollArray") {
            getData('rsrCollArray', $.bbq.getState("data"));
        }
        if (hash == "rsrColl") {
            getData('rsrColl', $.deparam.querystring(url).id);
        }
        if (hash == "prjCollArray") {
            getData('prjCollArray', $.deparam.querystring(url).id);
        }
        if (hash == "orgCollArray") {
            getData('orgCollArray', $.deparam.querystring(url).id);
        }
        if (hash == "compMapArray") {
            getData('compMapArray', $.bbq.getState("data"));
        }
        if (hash == "rsrCompMap") {
            getData('rsrCompMap', $.deparam.querystring(url).id);
        }
        if (hash == "orgCompMap") {
            getData('orgCompMap', $.deparam.querystring(url).id);
        }
        if (hash == "compMapSearch") {
            getData('compMapSearch', $.bbq.getState("data"));
        }
        if (hash == "collOrgArray") {
            getData('collOrgArray', $.bbq.getState("data"));
        }
        if (hash == "collCenterArray") {
            getData('collCenterArray', $.bbq.getState("data"));
        }
        if (hash == "collRandom") {
            getData('collMap', $.deparam.querystring(url).id);
        }
        if (hash == "search") {
            getData('query', $.bbq.getState("data"));
        }
        if (hash == "profilePrj") {
            getData('profilePrj', $.deparam.querystring(url).id);
        }
        if (hash == "profileOrg") {
            getData('profileOrg', $.deparam.querystring(url).id);
        }
        if (hash == "profileRsr") {
            getData('profileRsr', $.deparam.querystring(url).id);
        }
        if (hash == "home") {
            goHome();
            getData('getNews', 'http://newsfeed.ijs.si/query/news-search?qjson={%22$from%22:%22Article%22,%22Title%22:%20%22science%22,%20%22Title%22:%20%22research%22}');
        }
        if (hash == "about") {
            goAbout();
        }
        if (hash == "help") {
            goHelp();
        }
    });

    $(window).trigger("hashchange");
});

//for tab
$(document).ready(function () {

    var tabs = $('#tabs');
    var tab_a_selector = 'ul.ui-tabs-nav a';
    tabs.tabs({ event: 'change' });
    tabs.find(tab_a_selector).click(function () {
        var state = {};
        var id = $(this).closest('#tabs').attr('id');
        var idx = $(this).parent().prevAll().length;
        state[id] = idx;
        $.bbq.pushState(state);
    });

    $(window).bind('hashchange', function (e) {
        tabs.each(function () {
            var idx = $.bbq.getState(this.id, true) || 0;
            $(this).find(tab_a_selector).eq(idx).triggerHandler('change');
        });
    });

    $(window).trigger('hashchange');

});