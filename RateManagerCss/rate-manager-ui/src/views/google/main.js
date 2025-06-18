// plugins
import Vue from 'vue';
import 'bootstrap';
import BootstrapVue from 'bootstrap-vue';
import moment from 'moment';
import '../../core/app.settings';
import VueMoment from 'vue-moment';
import VCalendar from 'v-calendar';
import VTooltip from 'v-tooltip';
import Loading from 'vue-loading-overlay';
import locale from '../../core/localization';
import es from './localization/es';

// app
import View from './view.vue';

// styles
import './styles/app.scss';

// agregar idiomas
locale([{ language: 'es', localeFile: Object.assign(es) }], moment,false);

// Init plugins
Vue.use(VueMoment, {
    moment,
});

Vue.use(Loading);
Vue.use(VCalendar);
Vue.use(VTooltip);
Vue.use(BootstrapVue);

Vue.config.productionTip = false;

// creación de app
new Vue({
    render: h => h(View),
}).$mount('#app');
