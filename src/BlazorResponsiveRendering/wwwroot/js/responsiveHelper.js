window.ResponsiveFragment = {
    dotNetRefs: new Set(),

    registerResizeCallback: function (dotNetHelper) {
        this.dotNetRefs.add(dotNetHelper);

        if (!this._resizeListenerAttached) {
            window.addEventListener("resize", this.reportResize.bind(this));
            this._resizeListenerAttached = true;
        }

        this.reportResize();
    },

    reportResize: function () {
        const width = window.innerWidth;
        this.dotNetRefs.forEach(dotNetRef => {
            dotNetRef.invokeMethodAsync("UpdateFragment", width)
                .catch(err => console.error("Error invoking UpdateFragment:", err));
        });
    },

    getWindowWidth: function () {
        return window.innerWidth;
    },

    dispose: function (dotNetHelper) {
        if (this.dotNetRefs.has(dotNetHelper)) {
            this.dotNetRefs.delete(dotNetHelper);
        }

        if (this.dotNetRefs.size === 0 && this._resizeListenerAttached) {
            window.removeEventListener("resize", this.reportResize.bind(this));
            this._resizeListenerAttached = false;
        }
    }
};