import Vue from 'vue';
import 'bootstrap';
import BootstrapVue from 'bootstrap-vue';
import '../../core/app.settings';
import View from './view.vue';
import '../../styles/base.scss';
import 'bootstrap-vue/dist/bootstrap-vue.css';

Vue.use(BootstrapVue);

new Vue({
    render: h => h(View)
}).$mount('#app');