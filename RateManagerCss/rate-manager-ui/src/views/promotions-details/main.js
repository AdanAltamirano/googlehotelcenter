import Vue from 'vue';
import 'bootstrap';
import moment from 'moment';
import BootstrapVue from 'bootstrap-vue';
import '../../core/app.settings';
import VueMoment from 'vue-moment';
import locale from '../../core/localization';
import es from './localization/es';
import View from './view.vue';
import './styles/custom.scss';

Vue.use(BootstrapVue);

locale([{ language: 'es', localeFile: es }], moment, false);
Vue.use(VueMoment, {
    moment,
});

new Vue({
    render: h => h(View)
}).$mount('#app');