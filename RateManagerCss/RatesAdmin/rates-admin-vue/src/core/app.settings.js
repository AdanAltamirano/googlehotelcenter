import Vue from 'vue';

const AppConfig = {
    // APP configurations
    language: window.Language || 'en',
    domain: window.Domain,
    user: window.User,
    session: {
        hotelId: 3167, //1978//// cambiar para obtener de la sesion
    },
};


const ConfigsPlugIn = {
    install($Vue) {
        $Vue.prototype.$appConfig = AppConfig;
        $Vue.appConfig = AppConfig;
    },
};
Vue.use(ConfigsPlugIn);
