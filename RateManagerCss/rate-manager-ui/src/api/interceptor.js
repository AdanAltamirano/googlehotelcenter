import Vue from 'vue';
import EventBus from '../core/event-bus';

const proccessing = { count: 0 };

export default (request) => {
    request.headers.set('Accept-Language', Vue.appConfig.language);
    console.log(request);
    let evt = '';
    if(request.params.customTracker){
        evt = request.params.customTracker ? '[' + request.params.customTracker +']' : '';
        delete request.params.customTracker;
    }

    if (proccessing.count === 0) EventBus.$emit('api.call.begin' + evt);
    proccessing.count += 1;
    return () => {
        proccessing.count -= 1;
        if (proccessing.count === 0) EventBus.$emit('api.call.end' + evt);
    };
};
