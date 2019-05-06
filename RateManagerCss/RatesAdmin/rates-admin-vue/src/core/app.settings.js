import Vue from 'vue'

const AppConfig = {
    //APP configurations
    languaje: window.Language|| 'en',
    domain: window.Domain,
    user: window.User,
    session:{
        hotelId: 1978 // cambiar para obtener de la sesion
    }    
}


const ConfigsPlugIn = {
    install(Vue) {
        Vue.prototype.$appConfig = AppConfig;
        Vue.appConfig = AppConfig;
    }
}
Vue.use(ConfigsPlugIn);