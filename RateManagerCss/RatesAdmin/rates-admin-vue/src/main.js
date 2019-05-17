import Vue from 'vue';
import App from './App.vue';
import 'bootstrap';
import './core/app.settings';
import { localeMoment } from './core/localization/locale';
import VueMoment from 'vue-moment';
import VueCurrencyFilter from 'vue-currency-filter';
import Loading from 'vue-loading-overlay';
// cargar el store al final para que los settings ya hayan sido cargados
import store from './core/store'; 

//styles
import './assets/app.scss';

// Init plugins
Vue.use(VueMoment, {
  moment: localeMoment,
});

Vue.use(VueCurrencyFilter, {
  symbol: '$',
  thousandsSeparator: ',',
  fractionCount: 2,
  fractionSeparator: '.',
  symbolPosition: 'front',
  symbolSpacing: true
});

Vue.use(Loading);

Vue.config.productionTip = false;

new Vue({
  store,
  render: h => h(App),
}).$mount('#app');
