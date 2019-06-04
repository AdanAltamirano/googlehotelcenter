import Vue from 'vue';
import Vuex from 'vuex';
import vuexI18n from 'vuex-i18n';
import moment from 'moment';
import 'moment/locale/es';
import es from './es';


Vue.use(Vuex);
Vue.use(vuexI18n.plugin, new Vuex.Store());

Vue.i18n.add('es', es);

// verificacion del lenguaje
// el prlugin de verificacion ya debío ser cargado
const currentLanguage = Vue.appConfig.language;

// establecer idioma
Vue.i18n.set(currentLanguage);
moment.locale(currentLanguage);

export { moment as localeMoment };
