<template>
	<div id="app">
        <b-container fluid>
            <h2 class="text-primary">{{ $t('Synchronize Rates in Channel Manager') }}</h2>
            <!-- Tarifas -->
            <div>
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
            </div>
            <!-- Restricciones -->
            <div>
                <div v-if="callApiRestrictions" class="center-flex mt-3 vld-parent" style="height:200px;">
                    <loading :active="true" :is-full-page="false" color="#007bff"></loading>
                </div>
                <div else class="center-flex mt-3">
                    <b-button v-if="showButtonRestrictions" variant="primary" @click="updateRestrictions()">
                        {{$t('Update Restrictions')}}
                    </b-button>
                </div>
                <div class="center-flex mt-3">
                    <label style="color:#dc3545;">
                        {{$t("This operation make take a few minutes")}}
                    </label>
                </div>
            </div>
        </b-container>
    </div>
</template>

<script>
import Vue from "vue";
import Loading from "vue-loading-overlay";
import RestrictionAlert from "./components/RestrictionsAlert.vue";
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
            callApiRestrictions: false,
            showButton: true,
            showButtonRestrictions: true
        }
    },
    methods:{
        updateRates(){
            this.showButton = false;
            this.callApi = true;
            ConfluxService.UpdateRates(this.hotelId)
            .then(response => {
                console.log(response);
                this.callApi = false;
                this.showButton = true;
                this.$appAlert(this.success(this.$t("Rates Updated")));
            })
            .catch(error => {
                console.log(error);
                this.callApi = false;
                this.showButton = true;
                this.$appAlert(this.error(this.$t('System Error')))
            }); 
        },
        updateRestrictions(){
            this.showButtonRestrictions = false;
            this.callApiRestrictions = true;
            ConfluxService.UpdateRestrictions(this.hotelId)
            .then(response =>{

                let component = Vue.extend(RestrictionAlert);
                let instance = new component({
                    propsData:{
                        restrictions: response.body.restrictions
                    }
                });

                instance.$mount();
                let html = $("<div>").append(instance.$el);
                console.log(html);
                this.callApiRestrictions = false;
                this.showButtonRestrictions = true;
                this.$appAlert(this.successHTML(this.$t('Restrictions'),html));

            })
            .catch(error => {
                this.callApiRestrictions = false;
                this.showButtonRestrictions = true;
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
        successHTML(title, html) {
            return {
                title: title,
                type: "success",
                html: html,
                showCancelButton: true,
                showConfirmButton:false,
                cancelButtonText: this.$t("Exit"),
                cancelButtonColor: "#d33",
            }
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
            
