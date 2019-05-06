import Vue from 'vue'
import Vuex from 'vuex'
import vuexI18n from 'vuex-i18n'
import moment from 'moment'
import 'moment/locale/es'
import es from './es'



Vue.use(Vuex); 
Vue.use(vuexI18n.plugin, new Vuex.Store());

Vue.i18n.add('es', es);

// verificacion del lenguaje
// el prlugin de verificacion ya debío ser cargado
let currentLanguaje = Vue.appConfig.languaje;

// establecer idioma
Vue.i18n.set(currentLanguaje);
moment.locale(currentLanguaje);

export { moment as localeMoment };