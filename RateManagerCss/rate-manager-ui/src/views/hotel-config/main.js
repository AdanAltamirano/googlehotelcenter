import Vue from 'vue';
import 'bootstrap';
import '../../core/app.settings';
import moment from 'moment';
import BootstrapVue from 'bootstrap-vue';
import locale from '../../core/localization';
import View from './view.vue';

import es from './localization/es';

import './styles/app.scss';

Vue.use(BootstrapVue);


locale([{
    language: 'es',
    localeFile: es
}], moment, false);

new Vue({
    render: h => h(View)
}).$mount('#app');