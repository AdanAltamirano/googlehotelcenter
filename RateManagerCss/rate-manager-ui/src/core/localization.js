import Vue from 'vue';
import Vuex from 'vuex';
import vuexI18n from 'vuex-i18n';
import 'moment/locale/es';

export default (resources, moment, store) => {
    // si no hay store agregamos el uso aqui.
    if (!store) {
        Vue.use(Vuex);
    }

    Vue.use(vuexI18n.plugin, store || new Vuex.Store());

    for (let i = 0; i < resources.length; i += 1) {
        Vue.i18n.add(resources[i].language, resources[i].localeFile);
    }
    // establecer idioma
    // el plugin de appsettings ya debío ser cargado
    Vue.i18n.set(Vue.appConfig.language);
    moment.locale(Vue.appConfig.language);
};
