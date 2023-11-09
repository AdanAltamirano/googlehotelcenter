<template>
	<div id="app">
        <b-container fluid>
            <h2 class="text-primary">{{ $t('Synchronize Rates in Channel Manager') }}</h2>
            <div v-if="callApi" class="center-flex mt-3 vld-parent" style="height:200px;">
                <loading :active="true" :is-full-page="false" color="#007bff"></loading>
            </div>
            <div else class="center-flex mt-3">
                <b-button v-if="showButton" variant="primary" @click="updateRates()">
                    {{$t('Update Rates')}}
                </b-button>
            </div>
            <div class="center-flex mt-3">
                <label style="color:#dc3545;">
                    {{$t("This operation make take a few minutes")}}
                </label>
            </div>
        </b-container>
    </div>
</template>

<script>
import Loading from "vue-loading-overlay";
import ConfluxService from '../../api/conflux-service';

export default {
    components: {
        Loading
    },
    created(){

    },
    data (){
        return {
            //Hotel Id
            hotelId: this.$appConfig.session.hotelId,
            callApi: false,
            showButton: true
        }
    },
    methods:{
        updateRates(){
            this.showButton = false;
            this.callApi = true;
            ConfluxService.UpdateRates(this.hotelId)
            .then(response => {
                this.callApi = false;
                this.showButton = true;
                this.$appAlert(this.success(this.$t("Rates Updated")));
            })
            .catch(error => {
                this.callApi = false;
                this.showButton = true;
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
            
