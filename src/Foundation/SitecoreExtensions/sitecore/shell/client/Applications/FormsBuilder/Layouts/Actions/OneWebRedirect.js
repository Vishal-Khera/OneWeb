(function (speak) {
    var parentApp = window.parent.Sitecore.Speak.app.findApplication('EditActionSubAppRenderer');

    speak.pageCode([], function () {
        return {
            initialized: function () {
                this.on({
                    "loaded": this.loadDone
                }, this);

                this.ItemTreeView.on("change:SelectedItem", this.updateSelectability, this);
                this.ExternalLink.on("change", this.updateSelectability, this);
                if (parentApp) {
                    parentApp.loadDone(this, this.HeaderTitle.Text, this.HeaderSubtitle.Text);
                }
            },

            updateSelectability: function () {
                var isSelectable = !!this.ItemTreeView.SelectedItem;
                if (isSelectable) {
                    parentApp.setSelectability(this, true, this.ItemTreeView.SelectedItemId);
                } else if (this.ExternalLink.Value != undefined && this.ExternalLink.Value.length > 0) {
                    parentApp.setSelectability(this, true, this.ExternalLink.Value);
                }
            },

            loadDone: function (parameters) {
                this.Parameters = parameters || {};
                this.ItemTreeView.SelectedItemId = this.Parameters.internalItemId;
                this.ExternalLink.Value = this.Parameters.externalLink;
                this.Anchor.Value = this.Parameters.anchor;
                this.QueryString.Value = this.Parameters.queryString;
            },

            getData: function () {
                this.Parameters.internalItemId = this.ItemTreeView.SelectedItemId;
                this.Parameters.externalLink = this.ExternalLink.Value;
                this.Parameters.anchor = this.Anchor.Value;
                this.Parameters.queryString = this.QueryString.Value;
                return this.Parameters;
            },

            getDescription: function () {
                if (this.ExternalLink.Value != undefined && this.ExternalLink.Value.length > 0) {
                    var externalLink = this.ExternalLink.Value;
                    if (this.QueryString.Value != undefined && this.QueryString.Value.length > 0) {
                        externalLink += "?" + this.QueryString.Value;
                    }
                    if (this.Anchor.Value != undefined && this.Anchor.Value.length > 0) {
                        externalLink += "#" + this.Anchor.Value;
                    }

                    return externalLink;
                } else {
                    return this.ItemTreeView.SelectedItem.$displayName;
                }
                
            },
        };
    });
})(Sitecore.Speak);