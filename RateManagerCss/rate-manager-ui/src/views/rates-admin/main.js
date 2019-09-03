// plugins
import Vue from 'vue';
import 'bootstrap';
import moment from 'moment';
import '../../core/app.settings';
import VueMoment from 'vue-moment';
import Loading from 'vue-loading-overlay';
import VCalendar from 'v-calendar';
import VueCurrencyFilter from 'vue-currency-filter';
import VTooltip from 'v-tooltip';
import locale from '../../core/localization';
import es from './localization/es';
import esErrors from './localization/errors.es';

// app
import store from './store';
import View from './view.vue';

// styles
import './styles/app.scss';

// agregar idiomas
locale([{ language: 'es', localeFile: Object.assign(es, esErrors) }], moment, store);

// Init plugins
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

Vue.use(Loading);
Vue.use(VCalendar);
Vue.use(VTooltip);

Vue.config.productionTip = false;


// creación de app
new Vue({
    store,
    render: h => h(View),
}).$mount('#app');
