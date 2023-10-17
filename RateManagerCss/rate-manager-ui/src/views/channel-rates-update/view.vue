<template>
	<div id="app">
        <button @click="updateRates()">
            Actualizar Tarifas 
        </button>
    </div>
</template>

<script>

import ConfluxService from '../../api/conflux-service';

export default {
    created(){

    },
    data (){
        return {
            //Hotel Id
            hotelId: this.$appConfig.session.hotelId,
        }
    },
    methods:{
        updateRates(){
            ConfluxService.UpdateRates(this.hotelId)
        }
    }
}
</script>
            
