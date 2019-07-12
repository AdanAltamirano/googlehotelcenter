<template>
    <div id="app">
        <b-container fluid>
            <h2 class="text-primary">Detalle de la reserva</h2>
            <general-info></general-info>
        </b-container>
    </div>
</template>

<script>
import GeneralInfo from './components/GeneralInfo.vue';
import ReservationService from '../../api/reservation-service';

export default {
    name: 'app',
    components: {
        GeneralInfo
    },
    mounted() {
        ReservationService.GetDetails(324849).then((response) => {
            console.log(response);
        })
        
    }
}
</script>
