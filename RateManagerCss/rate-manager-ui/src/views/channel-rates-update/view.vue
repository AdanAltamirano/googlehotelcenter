<template>
	<div id="app">
        <b-container fluid>
            <h1 class="text-primary">{{ $t('Synchronize with Google Hotel Center') }}</h1>
            <!-- Tarifas -->
            <div>
                <h2 class="text-primary">{{$t('Prices')}}</h2>
                <details class="text-primary">{{$t('This action will only send the current prices to Google Hotel Center as of the current date')}}</details>
                <div v-if="callApi" class="mt-3 vld-parent" style="height:80px;">
                    <loading style="display:block !important;" :active="true" :is-full-page="false" color="#007bff"></loading>
                </div>
                <div else class="mt-4">
                    <b-button class="mt-1" v-if="showButton" variant="primary" @click="updateRates()">
                        {{$t('Update Rates')}}
                    </b-button>
                </div>
                <div class="mt-3">
                    <label style="color:#dc3545;">
                        {{$t("This operation make take a few minutes")}}
                    </label>
                </div>
            </div>
            <!-- Restricciones -->
            <hr class="solid">
            <div>
                <h2 class="text-primary">{{$t('Closure')}}</h2>
                <details class="text-primary">{{$t('This action will only send the current closures to Google Hotel Center from the current date')}}</details>
                <div v-if="callApiRestrictions" class="mt-3 vld-parent" style="height:80px;">
                    <loading style="display:block !important;" :active="true" :is-full-page="false" color="#007bff"></loading>
                </div>
                <div else class="mt-4">
                    <b-button class="mt-1" v-if="showButtonRestrictions" variant="primary" @click="updateRestrictions()">
                        {{$t('Update Closures')}}
                    </b-button>
                </div>
                <div class="mt-3">
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
                this.$appAlert(this.successHTML(this.$t('Closures'),html));

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
            
