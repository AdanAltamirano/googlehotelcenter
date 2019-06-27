import Vue from 'vue';

const AppConfig = {
    // APP configurations
    language: window.app.language || 'es',
    session: {
        hotelId: window.app.hotelId,
    },
};


const ConfigsPlugIn = {
    install($Vue) {
        $Vue.prototype.$appConfig = AppConfig;
        $Vue.appConfig = AppConfig;
    },
};

Vue.use(ConfigsPlugIn);


//when https://github.com/vuejs/vue/pull/7765
Vue.prototype._b = (function (bind) {
    return function (data, tag, value, asProp, isSync) {
        if (value && value.$scopedSlots) {
            data.scopedSlots = value.$scopedSlots;
            delete value.$scopedSlots;
        }
        return bind.apply(this, arguments);
    };
})(Vue.prototype._b);
