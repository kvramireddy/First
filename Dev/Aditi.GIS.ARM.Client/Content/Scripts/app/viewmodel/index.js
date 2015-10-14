var ARM = ARM || {};
ARM.ViewModel = ARM.ViewModel || {};
ARM.ViewModel.Index = function viewModel(mapInstance, selectionMadeCallback) {
    var self = this;
    var mi = mapInstance;
    var vectorLayer, clusterSource, layerExtent;
    var styleCache = {};

    //Filters
    var skFilter = new ARM.Filter.SkillsetFilter({ title: "Skillset", callback: skFilterCallback });
    //Widgets
    var widgets = [new ARM.Widget.EmployeeUtilization(), new ARM.Widget.Employees(), new ARM.Widget.EmployeesUnBilled(), new ARM.Widget.Weather()]

    var interactionSelect = new ol.interaction.Select({
        condition: function (evt) {
            return evt.type == 'singleclick';
        },
        style: new ol.style.Style({
            image: new ol.style.Circle({
                radius: 20,
                stroke: new ol.style.Stroke({
                    color: 'rgba(225,225,225,0.9)',
                    width: 2
                }),
                fill: new ol.style.Fill({
                    color: 'red'
                })
            })
        })
    });

    var interactionHover = new ol.interaction.Select(
        {
            condition: ol.events.condition.pointerMove
            ,
            style: function (feature, resolution) {
                var styles = [];
                if (feature) {
                    var orgFeatures = feature.get('features');
                    if (orgFeatures) {
                        orgFeatures.forEach(function (item, idx) {
                            styles.push(new ol.style.Style({
                                geometry: item.getGeometry(),
                                image: new ol.style.RegularShape({
                                    fill: new ol.style.Fill({color:'white'}),
                                    stroke: new ol.style.Stroke({ color: 'rgba(0,200,200,0.6)', width: 2 }),
                                    points: 10,
                                    radius1: 12,
                                    radius2: 8,
                                    angle: 0
                                }),
                                text: new ol.style.Text({
                                    text: item.get('count'),
                                    fill: new ol.style.Fill({ color: 'brown' })
                                })
                            }));
                        });
                    };
                }
                return styles;
            }
        });

    var collectionSelectedFeatures = interactionSelect.getFeatures();

    if (mi) {
        mi.getView().on('change:resolution', function (a) {
            collectionSelectedFeatures.clear();
        });
        mi.addInteraction(interactionSelect);
        mi.addInteraction(interactionHover);
        collectionSelectedFeatures.on('add', function (eventData) {
            var locationNames = '';
            var featureArray = eventData.element.get('features');
            if (featureArray) {
                var locationids = [], geoDataArray = [];

                for (var i = 0, ii = featureArray.length; i < ii; i++) {
                    locationids.push(featureArray[i].get('locationId'));
                    var geomPointArray = featureArray[i].get('geometry').clone().transform(mi.getView().getProjection(), 'EPSG:4326').getCoordinates(); //has to be point
                    var gData = new ARM.Model.GeoData();
                    gData.lon = geomPointArray[0];
                    gData.lat = geomPointArray[1];
                    geoDataArray.push(gData);
                    locationNames += featureArray[i].get('name') + '</br>';
                }
                Aditi.MapHelper.Popup.show('<p>Locations</p><code>' + locationNames + '</code>', eventData.element.getGeometry().flatCoordinates);
                widgets.forEach(function (item, idx) {
                    item.updateData(geoDataArray, locationids, skFilter.selectedSkillset().id);

                });
                if (selectionMadeCallback) selectionMadeCallback();
            }
        });
    }



    function getFeatureCount(feature) {
        if (feature) {
            var innerFeatures = feature.get('features');
            if (innerFeatures) {
                var sum = 0;
                for (i = 0, ii = innerFeatures.length; i < ii; i++) {
                    var val = innerFeatures[i].get("count");
                    if (val) sum += parseInt(val);
                }
                return sum;
            }
            else
                return feature.get("count");
        }
    };

    var styleFunc = function (feature, resolution) {
        var size = getFeatureCount(feature);
        var style = styleCache[size];
        if (!style) {
            style = [new ol.style.Style({
                image: new ol.style.Circle({
                    radius: (size > 200) ? 30 : (size > 100) ? 25 : (size > 50) ? 15 : 10,
                    stroke: new ol.style.Stroke({
                        color: 'rgba(225,225,225,0.9)',
                        width: 2
                    }),
                    fill: new ol.style.Fill({
                        color: '#3399CC'
                    })
                }),
                text: new ol.style.Text({
                    text: size.toString(),
                    fill: new ol.style.Fill({
                        color: '#fffa'
                    })
                })
            })];
            styleCache[size] = style;
        }
        return style;
    }
    function loadlayer() {
        if (vectorLayer != null) {
            mi.removeLayer(vectorLayer);
            vectorLayer = null;;
        }
        vectorLayer = new ol.layer.Vector({
            source: clusterSource,
            style: styleFunc
        });
        mi.addLayer(vectorLayer);
    }
    function skFilterCallback(gData) {
        collectionSelectedFeatures.clear();
        Aditi.MapHelper.Popup.close();
        //clear widgets
        widgets.forEach(function (item, idx) {
            if (item.clear) item.clear();
        });
        if (gData) {
            var vs = new ol.source.Vector();
            $(gData).each(function (idx, val) {
                vs.addFeature(new ol.Feature({
                    geometry: new ol.geom.Point([val.lon, val.lat]).transform('EPSG:4326', 'EPSG:3857'),
                    count: val.metadata.count,
                    name: val.metadata.name,
                    locationId: val.metadata.locationId
                }));
            });
            clusterSource = new ol.source.Cluster(
                {
                    attributions: [new ol.Attribution({ html: 'Aditi ArM' })],
                    source: vs,
                    distance: 50
                });
            loadlayer();
        }
    }
    function init() {
        skFilter.init();
    }
    return { Filters: [skFilter], Widgets: widgets, init: init }
};
