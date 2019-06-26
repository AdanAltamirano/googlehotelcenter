// plugins
import Vue from 'vue';
import 'bootstrap';
import BootstrapVue from 'bootstrap-vue';
import moment from 'moment';
import '../../core/app.settings';
import VueMoment from 'vue-moment';
import locale from '../../core/localization';
import es from './localization/es';

// app
import View from './view.vue';

// styles
import './styles/app.scss';

// agregar idiomas
locale([{ language: 'es', localeFile: es }], moment);

// Init plugins
Vue.use(VueMoment, {
    moment,
});
Vue.use(BootstrapVue);

Vue.config.productionTip = false;

// creación de app
new Vue({
    render: h => h(View),
}).$mount('#app');
