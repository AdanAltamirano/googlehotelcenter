import Vue from 'vue';
import 'bootstrap';
import '../../core/app.settings';
import VueMoment from 'vue-moment';
// cargar el store al final para que los settings ya hayan sido cargados
import Loading from 'vue-loading-overlay';
import VCalendar from 'v-calendar';
import VueCurrencyFilter from 'vue-currency-filter';
import VTooltip from 'v-tooltip';
import { localeMoment } from '../../core/localization/locale';
import store from './store';
import App from './view.vue';

// styles
import '../../assets/app.scss';

// Init plugins
Vue.use(VueMoment, {
    moment: localeMoment,
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

new Vue({
    store,
    render: h => h(App),
}).$mount('#app');
