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
