<template>
	<div id="app">
        <div class="center-flex">
            <b-button variant="primary" @click="updateRates()">
                Actualizar Tarifas 
            </b-button>
        </div>
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
            .then(response => {
                this.$appAlert(this.success(this.$t("Rates Updated")));
            })
            .catch(error => {
                this.$appAlert(this.error(this.$t('System Error')))
            }); 
        },       
        success(title) {
            return {
                type: "success",
                title: title,
                showCancelButton: true,
                showConfirmButton:false,
                cancelButtonText: this.$t("Exit"),
                cancelButtonColor: "#d33",
                showConfirmButton: false,
                time: 2500,               
            };
        },
        error(title) {
            return {
                type: "error",
                title: title,
                showCancelButton: true,
                showConfirmButton:false,
                cancelButtonText: this.$t("Exit"),
                cancelButtonColor: "#d33",
                showConfirmButton: false,
                time: 2500,               
            };
        },
    }
}
</script>
            
