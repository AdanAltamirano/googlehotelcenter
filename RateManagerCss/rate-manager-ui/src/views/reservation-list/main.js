import Vue from 'vue';
import 'bootstrap';
import moment from 'moment';
import BootstrapVue from 'bootstrap-vue';
import '../../core/app.settings';
import VueMoment from 'vue-moment';
import VCalendar from 'v-calendar';
import locale from '../../core/localization';
import es from './localization/es';
import store from './store';
import View from './view.vue';

import '../../styles/base.scss';
import 'bootstrap-vue/dist/bootstrap-vue.css';

Vue.use(BootstrapVue);

locale([{ language: 'es', localeFile: es }], moment, store);

Vue.use(VueMoment, {
    moment,
});

Vue.use(VCalendar);



new Vue({
    store,
    render: h => h(View),
}).$mount('#app');
