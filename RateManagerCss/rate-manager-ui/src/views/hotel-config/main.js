import Vue from 'vue';
import '../../core/app.settings';
import 'bootstrap';
import moment from 'moment';
import BootstrapVue from 'bootstrap-vue';
import locale from '../../core/localization';
import View from './view.vue';

import es from './localization/es';

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css';
import './styles/app.scss';

Vue.use(BootstrapVue);

locale([{
    language: 'es',
    localeFile: es
}], moment, false);

new Vue({
    render: h => h(View)
}).$mount('#app');