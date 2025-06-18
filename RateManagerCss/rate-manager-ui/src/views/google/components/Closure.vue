<template>
    <div class="">        
        <details class="text-primary">{{$t('This action will only send the current closures to Google Hotel Center from the current date')}}</details>
        <div v-if="callApi" class="mt-3 vld-parent" style="height:80px;">
            <loading style="display:block !important;" :active="true" :is-full-page="false" color="#007bff"></loading>
        </div>
        <div else class="mt-4">
            <b-button :disabled="!isEnabledGoogle" class="mt-1" v-if="showButton" variant="primary" @click="updateRestrictions()">
                {{$t('Update Closures')}}
            </b-button>
        </div>
        <div class="mt-3">
            <label v-if="isEnabledGoogle" style="color:#dc3545;">
                {{$t("This operation make take a few minutes")}}
            </label>
                <label v-else-if="!isEnabledGoogle" style="color:#dc3545;">
                {{$t("Enable Google Prices in Content / General Information")}}
            </label>
        </div>
        <hr class="solid">
    </div>
</template>

<script>
import Vue from "vue";
import Loading from "vue-loading-overlay";
import RestrictionAlert from "./RestrictionsAlert.vue";
import ConfluxService from '../../../api/conflux-service';

export default {
    props:{
        hotelId:{

        },
        isEnabledGoogle:{
            type:Boolean
        }
    },
    components:{
        Loading
    },
    data(){
        return{
            callApi: false,
            showButton: true
        }
    },
    methods:{
        updateRestrictions(){
            this.showButton = false;
            this.callApi = true;
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
                this.callApi = false;
                this.showButton = true;
                this.$appAlert(this.successHTML(this.$t('Closures'),html));

            })
            .catch(error => {
                this.callApi = false;
                this.showButton = true;
                this.$appAlert(this.error(this.$t('System Error')))
            });
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

