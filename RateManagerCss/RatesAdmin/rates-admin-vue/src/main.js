import Vue from 'vue';
import App from './App.vue';
import 'bootstrap';
import Settings from './core/app.settings'
import { localeMoment } from './core/localization/locale'
import VueMoment from 'vue-moment' 
import VueCurrencyFilter from 'vue-currency-filter'
import Loading from 'vue-loading-overlay';


// Import stylesheet

// Init plugin
Vue.use(Loading);

//styles
import './assets/app.scss';

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

Vue.config.productionTip = false;

new Vue({
  render: h => h(App),
}).$mount('#app');
