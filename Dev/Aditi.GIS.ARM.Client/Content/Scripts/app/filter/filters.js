var ARM = ARM || {};
ARM.Filter = ARM.Filter || {};
ARM.Filter.SkillsetFilter = function (options) {
    var self = this;
    var callback = options && options.callback;
    var title = (options && options.title) || 'Untitled';
    var skillsets = ko.observableArray();
    var selectedSkillset = ko.observable();
    var skillset = new ARM.Model.skillset();
    selectedSkillset.subscribe(function (sk) {
        if (sk) {
            skillset.locations(sk.id, function (rs) {
                var gDataArray = [];
                $(rs).each(function (idx, val) {
                    var gd = new ARM.Model.GeoData();
                    gd.lat = val.pos.Lat;
                    gd.lon = val.pos.Lon;
                    gd.metadata.count = val.extprops['emp-count'];
                    gd.metadata.name = val.ln;
                    gd.metadata.locationId = val.id;
                    gDataArray.push(gd);
                });
                if (callback) callback(gDataArray);
            });
        }
    }, 'change');

    var init = function () {
        skillset.pull(function (rs) {
            skillsets.push.apply(skillsets, rs);
        });
    };
    return { init: init, skillsets: skillsets, selectedSkillset: selectedSkillset, title: title }
};
