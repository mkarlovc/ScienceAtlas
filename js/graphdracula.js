/// <reference path="class.js" />

function drawDiagramCollMap() {
    $('#sigCollMap').empty();
    var sigRoot = document.getElementById('sigCollMap');
    var sigInst = sigma.init(sigRoot).drawingProperties({
        defaultLabelColor: _defaultLabelColor,
        font: 'Arial',
        edgeColor: 'source',
        defaultEdgeType: 'curve'
    }).graphProperties({
        minNodeSize: 2,
        maxNodeSize: 10
    });

    // Bind events :
    var greyColor = '#666';

    var popUp;

    function showNodeInfo(event) {
        popUp && popUp.remove();

        var node;
        sigInst.iterNodes(function (n) { node = n; }, [event.content[0]]);

        if (node.attr['science'] != "" || node.attr['keywords'] != "") {

            var ul = $('<ul>').css('margin', '0 0 0 5px');

            if (node.attr['science'] != "")
                ul.append("<li>Science: " + node.attr['science'] + "</li>");

            if (node.attr['keywords'] != "")
                ul.append("<li>Keywords: " + node.attr['keywords'] + "</li>");

            popUp = $('<div/>').append(ul).css({
                'display': 'inline-block',
                'border-radius': 3,
                'background': '#fff',
                'color': '#000',
                'padding': 0,
                'margin': 0,
                'box-shadow': '0 0 4px #666',
                'position': 'absolute',
                'font-size': 'x-small',
                'font-family': 'Arial',
                'left': node.displayX,
                'top': node.displayY + 60,
                'max-width': '400px'
            });

            $('#sigCollMap').append(popUp);
        }
    }

    function hideNodeInfo(event) {
        popUp && popUp.remove();
        popUp = false;
    }

    sigInst.bind('overnodes', showNodeInfo).bind('outnodes', hideNodeInfo);

    sigInst.bind('upnodes', function (event) {
        var node;
        sigInst.iterNodes(function (n) { node = n; }, [event.content[0]]);
        window.open($.param.querystring("#profileRsr", { id: node['id'] }), "_self");
    });

    sigInst.bind('overnodes', function (event) {
        var nodes = event.content;
        var neighbors = {};
        sigInst.iterEdges(function (e) {
            if (nodes.indexOf(e.source) < 0 && nodes.indexOf(e.target) < 0) {
                if (!e.attr['grey']) {
                    e.attr['true_color'] = e.color;
                    e.color = greyColor;
                    e.attr['grey'] = 1;
                }
            } else {
                e.color = e.attr['grey'] ? e.attr['true_color'] : e.color;
                e.attr['grey'] = 0;

                neighbors[e.source] = 1;
                neighbors[e.target] = 1;
            }
        }).iterNodes(function (n) {
            if (n.id == event.content)
                n.label += " " + n.attr['first_name'];
            if (!neighbors[n.id]) {
                if (!n.attr['grey']) {
                    n.attr['true_color'] = n.color;
                    n.color = greyColor;
                    n.attr['grey'] = 1;
                }
            } else {
                n.color = n.attr['grey'] ? n.attr['true_color'] : n.color;
                n.attr['grey'] = 0;
            }
        }).draw(2, 2, 2);
    }).bind('outnodes', function () {
        sigInst.iterEdges(function (e) {
            e.color = e.attr['grey'] ? e.attr['true_color'] : e.color;
            e.attr['grey'] = 0;
        }).iterNodes(function (n) {
            n.color = n.attr['grey'] ? n.attr['true_color'] : n.color;
            n.attr['grey'] = 0;
            n.label = n.attr['last_name'];
        }).draw(2, 2, 2);
    });

    for (var i = 0; i < researchers.length; i++) {
        var color;
        if (researchers[i].scienceId != "") color = parseInt(researchers[i].scienceId);
        else color = 0;
        sigInst.addNode(researchers[i].id, { label: researchers[i].lastName, first_name: researchers[i].firstName, last_name: researchers[i].lastName, 'x': researchers[i].x, 'y': researchers[i].y, color: science_color[color], size: researchers[i].vd, science: researchers[i].science, keywords: researchers[i].keyws })
    }

    for (var i = 0; i < g1.length; i++) {
        sigInst.addEdge(researchers[g1[i].n1].id + "_" + researchers[g1[i].n2].id, researchers[g1[i].n1].id, researchers[g1[i].n2].id);
    }

    sigInst.draw();
};

function drawDiagramCollCenter() {
    /**
    * This is the code to write the FishEye plugin :
    */
    (function () {

        // First, let's write a FishEye class.
        // There is no need to make this class global, since it is made to be used through
        // the SigmaPublic object, that's why a local scope is used for the declaration.
        // The parameter 'sig' represents a Sigma instance.
        var FishEye = function (sig) {
            sigma.classes.Cascade.call(this); // The Cascade class manages the chainable property
            // edit/get function.

            var self = this; // Used to avoid any scope confusion.
            var isActivated = false; // Describes is the FishEye is activated.

            this.p = { // The object containing the properties accessible with
                radius: 200, // the Cascade.config() method.
                power: 2
            };

            function applyFishEye(mouseX, mouseY) { // This method will apply a formula relatively to
                // the mouse position.
                var newDist, newSize, xDist, yDist, dist,radius = self.p.radius,power = self.p.power,powerExp = Math.exp(power);

                sig.graph.nodes.forEach(function (node) {
                    xDist = node.displayX - mouseX;
                    yDist = node.displayY - mouseY;
                    dist = Math.sqrt(xDist * xDist + yDist * yDist);

                    if (dist < radius) {
                        newDist = powerExp / (powerExp - 1) * radius * (1 - Math.exp(-dist / radius * power));
                        newSize = powerExp / (powerExp - 1) * radius * (1 - Math.exp(-dist / radius * power));

                        if (!node.isFixed) {
                            node.displayX = mouseX + xDist * (newDist / dist * 3 / 4 + 1 / 4);
                            node.displayY = mouseY + yDist * (newDist / dist * 3 / 4 + 1 / 4);
                        }

                        node.displaySize = Math.min(node.displaySize * newSize / dist, 10 * node.displaySize);
                    }
                });
            };

            // The method that will be triggered when Sigma's 'graphscaled' is dispatched.
            function handler() {
                applyFishEye(sig.mousecaptor.mouseX,sig.mousecaptor.mouseY);
            }

            this.handler = handler;

            // A public method to set/get the isActivated parameter.
            this.activated = function (v) {
                if (v == undefined) {
                    return isActivated;
                } else {
                    isActivated = v;
                    return this;
                }
            };

            // this.refresh() is just a helper to draw the graph.
            this.refresh = function () {
                sig.draw(2, 2, 2);
            };
        };

        // Then, let's add some public method to sigma.js instances :
        sigma.publicPrototype.activateFishEye = function () {
            if (!this.fisheye) {
                var sigmaInstance = this;
                var fe = new FishEye(sigmaInstance._core);
                sigmaInstance.fisheye = fe;
            }

            if (!this.fisheye.activated()) {
                this.fisheye.activated(true);
                this._core.bind('graphscaled', this.fisheye.handler);
                document.getElementById('sigma_mouse_' + this.getID()).addEventListener('mousemove', this.fisheye.refresh, true);
            }

            return this;
        };

        sigma.publicPrototype.desactivateFishEye = function () {
            if (this.fisheye && this.fisheye.activated()) {
                this.fisheye.activated(false);
                this._core.unbind('graphscaled', this.fisheye.handler);
                document.getElementById('sigma_mouse_' + this.getID()).removeEventListener('mousemove', this.fisheye.refresh, true);
            }

            return this;
        };

        sigma.publicPrototype.fishEyeProperties = function (a1, a2) {
            var res = this.fisheye.config(a1, a2);
            return res == s ? this.fisheye : res;
        };
    })();

    $('#sigCollCenter').empty();
    var sigRoot = document.getElementById('sigCollCenter');
    var sigInst = sigma.init(sigRoot).drawingProperties({
        defaultLabelColor: _defaultLabelColor,
        font: 'Arial',
        edgeColor: 'source',
        defaultEdgeType: 'curve'
    }).graphProperties({
        minNodeSize: 1,
        maxNodeSize: 5
    });

    // Bind events :
    var greyColor = '#666';

    var popUp;

    function showNodeInfo(event) {
        popUp && popUp.remove();

        var node;
        sigInst.iterNodes(function (n) { node = n; }, [event.content[0]]);

        if (node.attr['science'] != "" || node.attr['keywords'] != "") {

            var ul = $('<ul>').css('margin', '0 0 0 5px');

            if (node.attr['science'] != "")
                ul.append("<li>Science: " + node.attr['science'] + "</li>");

            if (node.attr['keywords'] != "")
                ul.append("<li>Keywords: " + node.attr['keywords'] + "</li>");

            popUp = $('<div/>').append(ul).css({
                'display': 'inline-block',
                'border-radius': 3,
                'background': '#fff',
                'color': '#000',
                'padding': 0,
                'margin': 0,
                'box-shadow': '0 0 4px #666',
                'position': 'absolute',
                'font-size': 'x-small',
                'font-family': 'Arial',
                'left': node.displayX,
                'top': node.displayY + 60,
                'max-width': '400px'
            });

            $('#sigCollCenter').append(popUp);
        }
    }

    function hideNodeInfo(event) {
        popUp && popUp.remove();
        popUp = false;
    }

    sigInst.bind('overnodes', showNodeInfo).bind('outnodes', hideNodeInfo);

    sigInst.bind('upnodes', function (event) {
        var node;
        sigInst.iterNodes(function (n) { node = n; }, [event.content[0]]);
        window.open($.param.querystring("#profileRsr", { id: node['id'] }), "_self");
    });

    sigInst.bind('overnodes', function (event) {
        var nodes = event.content;
        var neighbors = {};
        sigInst.iterEdges(function (e) {
            if (nodes.indexOf(e.source) < 0 && nodes.indexOf(e.target) < 0) {
                if (!e.attr['grey']) {
                    e.attr['true_color'] = e.color;
                    e.color = greyColor;
                    e.attr['grey'] = 1;
                }
            } else {
                e.color = e.attr['grey'] ? e.attr['true_color'] : e.color;
                e.attr['grey'] = 0;

                neighbors[e.source] = 1;
                neighbors[e.target] = 1;
            }
        }).iterNodes(function (n) {
            if (n.id == event.content) 
                n.label += " "+ n.attr['first_name'];

            if (!neighbors[n.id]) {
                if (!n.attr['grey']) {
                    n.attr['true_color'] = n.color;
                    n.color = greyColor;
                    n.attr['grey'] = 1;
                }
            } else {
                n.color = n.attr['grey'] ? n.attr['true_color'] : n.color;
                n.attr['grey'] = 0;
            }
        }).draw(2, 2, 2);
    }).bind('outnodes', function () {
        sigInst.iterEdges(function (e) {
            e.color = e.attr['grey'] ? e.attr['true_color'] : e.color;
            e.attr['grey'] = 0;
        }).iterNodes(function (n) {
            n.color = n.attr['grey'] ? n.attr['true_color'] : n.color;
            n.attr['grey'] = 0;
            n.label = n.attr['last_name'];
        }).draw(2, 2, 2);
    });

    /*
    for (var i = 0; i < researchers.length; i++) {
    sigInst.addNode(researchers[i].id, { label: researchers[i].lastName, color: '#00ff00', 'x': Math.random(), 'y': Math.random() })
    }*/

    for (var i = 0; i < researchers.length; i++) {
      var color;
        if (researchers[i].scienceId != "") color = parseInt(researchers[i].scienceId);
        else color = 0;
        sigInst.addNode(researchers[i].id, { label: researchers[i].lastName, first_name: researchers[i].firstName, last_name: researchers[i].lastName, color: science_color[color], 'x': researchers[i].x, 'y': researchers[i].y, science: researchers[i].science, keywords: researchers[i].keyws })
    }

    for (var i = 0; i < g.length; i++) {
        sigInst.addEdge(researchers[g[i].n1].id + "_" + researchers[g[i].n2].id, researchers[g[i].n1].id, researchers[g[i].n2].id);
    }

    sigInst.activateFishEye().draw();
    //sigInst.draw();
};

function drawDiagramCollOrg() {
    $('#sigCollOrg').empty();
    var sigRoot = document.getElementById('sigCollOrg');
    var sigInst = sigma.init(sigRoot).drawingProperties({
        defaultLabelColor: _defaultLabelColor,
        font: 'Arial',
        edgeColor: 'source',
        defaultEdgeType: 'curve'
    }).graphProperties({
        minNodeSize: 2,
        maxNodeSize: 10
    });

    // Bind events :
    var greyColor = '#666';

    var popUp;

    function showNodeInfo(event) {
        popUp && popUp.remove();

        var node;
        sigInst.iterNodes(function (n) { node = n; }, [event.content[0]]);

        if (node.attr['type'] != "") {

            var ul = $('<ul>').css('margin', '0 0 0 5px');

            if(node.attr['science'] != "")
                ul.append("<li>Type: " + node.attr['type'] + "</li>");

            popUp = $('<div/>').append(ul).css({
                'display': 'inline-block',
                'border-radius': 3,
                'background': '#fff',
                'color': '#000',
                'padding': 0,
                'margin': 0,
                'box-shadow': '0 0 4px #666',
                'position': 'absolute',
                'font-size': 'x-small',
                'font-family': 'Arial',
                'left': node.displayX,
                'top': node.displayY + 60,
                'max-width': '400px'
            });

            $('#sigCollOrg').append(popUp);
        }
    }

    function hideNodeInfo(event) {
        popUp && popUp.remove();
        popUp = false;
    }

    sigInst.bind('overnodes', showNodeInfo).bind('outnodes', hideNodeInfo);

    sigInst.bind('overnodes', function (event) {
        var nodes = event.content;
        var neighbors = {};
        sigInst.iterEdges(function (e) {
            if (nodes.indexOf(e.source) < 0 && nodes.indexOf(e.target) < 0) {
                if (!e.attr['grey']) {
                    e.attr['true_color'] = e.color;
                    e.color = greyColor;
                    e.attr['grey'] = 1;
                }
            } else {
                e.color = e.attr['grey'] ? e.attr['true_color'] : e.color;
                e.attr['grey'] = 0;

                neighbors[e.source] = 1;
                neighbors[e.target] = 1;
            }
        }).iterNodes(function (n) {
            if (!neighbors[n.id]) {
                if (!n.attr['grey']) {
                    n.attr['true_color'] = n.color;
                    n.color = greyColor;
                    n.attr['grey'] = 1;
                    n.size = "1";
                }
            } else {
                n.color = n.attr['grey'] ? n.attr['true_color'] : n.color;
                n.attr['grey'] = 0;
                n.size = "100";
            }
        }).draw(2, 2, 2);
    }).bind('outnodes', function () {
        sigInst.iterEdges(function (e) {
            e.color = e.attr['grey'] ? e.attr['true_color'] : e.color;
            e.attr['grey'] = 0;
        }).iterNodes(function (n) {
            n.color = n.attr['grey'] ? n.attr['true_color'] : n.color;
            n.attr['grey'] = 0;
            n.size = n.attr['vd'];
        }).draw(2, 2, 2);
    });

    for (var i = 0; i < organizations.length; i++) {
        var color, x;
        var type="";
        if (organizations[i].type == "r") { color = 0; x = randomXToY(0, 0.5, 5);  type = "research"; }
        else { color = 1; x = randomXToY(0.5, 1, 5); type = "business";}

        sigInst.addNode(organizations[i].id, { label: organizations[i].name, 'x': x, 'y': Math.random(), color: science_color1[color], vd:organizations[i].vd, size: organizations[i].vd, type: type });
    }
    for (var i = 0; i < g3.length; i++) {
        sigInst.addEdge(organizations[g3[i].n1].id + "_" + organizations[g3[i].n2].id, organizations[g3[i].n1].id, organizations[g3[i].n2].id);
    }

    sigInst.draw();
};

function randomXToY(minVal, maxVal, floatVal) {
    var randVal = minVal + (Math.random() * (maxVal - minVal));
    return typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal);
}

function drawDiagramCompMap() {
    $('#sigCompMap').empty();
    var sigRoot = document.getElementById('sigCompMap');
    var sigInst = sigma.init(sigRoot).drawingProperties({
        defaultLabelColor: _defaultLabelColor,
        font: 'Arial',
        edgeColor: 'source',
        defaultEdgeType: 'curve'
    }).graphProperties({
        minNodeSize: 2,
        maxNodeSize: 10
    });

    // Bind events :
    var greyColor = '#666';

    var popUp;

    function showNodeInfo(event) {
        popUp && popUp.remove();

        var node;
        sigInst.iterNodes(function (n) { node = n; }, [event.content[0]]);

        if (node.attr['science'] != "" || node.attr['keywords'] != "") {

            var ul = $('<ul>').css('margin', '0 0 0 5px');

            if(node.attr['science'] != "")
                ul.append("<li>Science: " + node.attr['science'] + "</li>");
            if (node.attr['keywords'] != "")
                ul.append("<li>Keywords: " + node.attr['keywords'] + "</li>");

            popUp = $('<div/>').append(ul).css({
                'display': 'inline-block',
                'border-radius': 3,
                'background': '#fff',
                'color': '#000',
                'padding': 0,
                'margin': 0,
                'box-shadow': '0 0 4px #666',
                'position': 'absolute',
                'font-size': 'x-small',
                'font-family': 'Arial',
                'left': node.displayX,
                'top': node.displayY + 60,
                'max-width': '400px'
            });

            $('#sigCompMap').append(popUp);
        }
    }

    function hideNodeInfo(event) {
        popUp && popUp.remove();
        popUp = false;
    }

    sigInst.bind('overnodes', showNodeInfo).bind('outnodes', hideNodeInfo);


    sigInst.bind('upnodes', function (event) {
        var node;
        sigInst.iterNodes(function (n) { node = n; }, [event.content[0]]);
        window.open($.param.querystring("#profilePrj", { id: node['id'] }), "_self");
    });
    sigInst.bind('overnodes', function (event) {
        var nodes = event.content;
        var neighbors = {};
        sigInst.iterEdges(function (e) {
            if (nodes.indexOf(e.source) < 0 && nodes.indexOf(e.target) < 0) {
                if (!e.attr['grey']) {
                    e.attr['true_color'] = e.color;
                    e.color = greyColor;
                    e.attr['grey'] = 1;
                }
            } else {
                e.color = e.attr['grey'] ? e.attr['true_color'] : e.color;
                e.attr['grey'] = 0;
                neighbors[e.source] = 1;
                neighbors[e.target] = 1;
            }
        }).iterNodes(function (n) {
            if (!neighbors[n.id]) {
                if (!n.attr['grey']) {
                    n.attr['true_color'] = n.color;
                    n.color = greyColor;
                    n.attr['grey'] = 1;
                }
            } else {
                n.color = n.attr['grey'] ? n.attr['true_color'] : n.color;
                n.color = n.attr['grey'] ? n.attr['true_color'] : n.color;
                n.attr['grey'] = 0;
            }
        }).draw(2, 2, 2);
    }).bind('outnodes', function () {
        sigInst.iterEdges(function (e) {
            e.color = e.attr['grey'] ? e.attr['true_color'] : e.color;
            e.attr['grey'] = 0;
        }).iterNodes(function (n) {
            n.color = n.attr['grey'] ? n.attr['true_color'] : n.color;
            n.attr['grey'] = 0;
        }).draw(2, 2, 2);
    });

    for (var i = 0; i < projects.length; i++) {
        var color;
        if (projects[i].scienceCode != "") color = parseInt(projects[i].scienceCode);
        else color = 0;
        sigInst.addNode(projects[i].id, { label: projects[i].name, 'x': projects[i].x, 'y': projects[i].y, color: science_color[color], size: projects[i].vd, science: projects[i].science, keywords: projects[i].keyws });
    }

    for (var i = 0; i < keywords.length; i++) {
        sigInst.addNode(keywords[i].id, { label: keywords[i].word, 'x': keywords[i].x, 'y': keywords[i].y, color: science_color[1], size: 0.3, science: "", keywords: "" });
    }

    for (var i = 0; i < g2.length; i++) {
        sigInst.addEdge(projects[g2[i].n1].id + "_" + keywords[g2[i].n2].id, projects[g2[i].n1].id, keywords[g2[i].n2].id);
    }

    //sigInst.draw();
    sigInst.startForceAtlas2();
    isRunning = true;
    setTimeout(function () {
        if (isRunning) {
            isRunning = false;
            sigInst.stopForceAtlas2();
            sigInst.refresh();
            $('#compMapLayout').attr('value', 'Start Layout');
        }
    }, 5000);

    document.getElementById('compMapLayout').addEventListener('click', function () {
        if (isRunning) {
            isRunning = false;
            sigInst.stopForceAtlas2();
            $('#compMapLayout').attr('value', 'Start Layout');
        } else {
            isRunning = true;
            sigInst.startForceAtlas2();
            $('#compMapLayout').attr('value','Stop Layout');
        }
    }, true);
};