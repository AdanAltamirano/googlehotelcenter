import Vue from 'vue';

const appConfig = {
    // APP configurations
    language: window.app.language || 'es',
    session: {
        hotelId: window.app.hotelId,
        hotelName: window.app.hotelName,
        corporateId: window.app.corporateId,
        corporateName: window.app.corporateName
    },
    themeColors: {
        primary: '#10467a',
        info: '#007bff',
        warning: '#ff6c00',
    },
    confirmNumber: window.app.confirmNumber,
    basePath: process.env.VUE_APP_URL,
};

function appAlert(alertData) {
    if (!window.parent?.$swal) {
        throw new Error('Whoops! - APP ALERT HUB NOT DEFINED');
    }
    return window.parent.$swal(alertData);
}

const ConfigsPlugIn = {
    install($Vue) {
        // app config
        $Vue.prototype.$appConfig = appConfig;
        $Vue.appConfig = appConfig;
        // app alerts
        $Vue.prototype.$appAlert = appAlert;
        $Vue.appAlert = appAlert;
    },
};

Vue.use(ConfigsPlugIn);

/* eslint-disable */
// when https://github.com/vuejs/vue/pull/7765
Vue.prototype._b = (function(bind) {
    return function(data, tag, value, asProp, isSync) {
        if (value && value.$scopedSlots) {
            data.scopedSlots = value.$scopedSlots;
            delete value.$scopedSlots;
        }
        return bind.apply(this, arguments);
    };
}(Vue.prototype._b));
/* eslint-enable */