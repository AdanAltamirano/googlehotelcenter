import Vue from 'vue';
import VueSweetalert2 from 'vue-sweetalert2';
import 'bootstrap';
import moment from 'moment';
import BootstrapVue from 'bootstrap-vue';
import '../../core/app.settings';
import VueMoment from 'vue-moment';
import VCalendar from 'v-calendar';
import VueCurrencyFilter from 'vue-currency-filter';
import locale from '../../core/localization';
import View from './view.vue';

import es from './localization/es';

import './styles/custom.scss';
import 'bootstrap-vue/dist/bootstrap-vue.css';

Vue.use(BootstrapVue);
Vue.use(VueSweetalert2);
locale([{ language: 'es', localeFile: es }], moment, false);
Vue.use(VueMoment, {
    moment,
});
Vue.use(VueCurrencyFilter, {
    symbol: '',
    thousandsSeparator: ',',
    fractionCount: 2,
    fractionSeparator: '.',
    symbolPosition: 'front',
    symbolSpacing: false,
});
Vue.use(VCalendar);

new Vue({
    render: h => h(View)
}).$mount('#app');