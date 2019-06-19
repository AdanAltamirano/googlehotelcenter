import Vue from 'vue';
import 'bootstrap';
import BootstrapVue from 'bootstrap-vue';
import '../../core/app.settings';
import store from './store'
import View from './view.vue';

Vue.use(BootstrapVue);
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';

new Vue({
    store,
    render: h => h(View),
}).$mount('#app');
