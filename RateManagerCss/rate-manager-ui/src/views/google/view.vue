<template>
	<div id="app">
        <b-container fluid>
            <h1 class="text-primary">{{ $t('Synchronize with Google Hotel Center') }}</h1>
            <!-- Fechas -->
            <b-row class="mt-4">
                <b-col class="col-custom-3">
                    <b-form-group 
                    :label="$t('Choose Dates')"
                    :description="$t('Date Range')">
                        <b-input-group>
                            <v-date-picker
                            v-model="dates"
                            class="form-control p-0"
                            mode="range"                                                     
                            :popover="{ placement: 'bottom', visibility: 'click' }"
                            :columns="2">
                            </v-date-picker>
                            <b-input-group-append>
                                <b-button :disabled="dates == null" variant="danger" @click="dates = null">
                                    <i class="fa fa-times"></i>
                                </b-button>
                            </b-input-group-append>
                        </b-input-group>
                    </b-form-group>
                </b-col>
            </b-row>            
            <!-- Tabs -->
            <div class="mt-4">
                <b-tabs
                active-nav-item-class="nav-custom-tab"
                active-tab-class="mt-3"
                nav-class="nav-custom-tab"> 
                
                    <b-tab :title="$t('Prices')" active>
                        <prices :hotelId="hotelId" :isEnabledGoogle="isEnabledGoogleRequest" :isEnabledAPICache="isEnabledAPICacheRequest" :dates="dates"></prices>
                    </b-tab>

                    <b-tab :title="$t('Closure')">
                        <closure :hotelId="hotelId" :isEnabledGoogle="isEnabledGoogleRequest" :isEnabledAPICache="isEnabledAPICacheRequest" :dates="dates"></closure>
                    </b-tab>

                    <b-tab :title="$t('Inventory')">
                        <inventory :hotelId="hotelId" :isEnabledGoogle="isEnabledGoogleRequest" :isEnabledAPICache="isEnabledAPICacheRequest"  :dates="dates"></inventory>
                    </b-tab>

                    <b-tab :title="$t('Delete Rates')" v-if="hasPermission && (isEnabledGoogleRequest || isEnabledAPICacheRequest)">
                        <delete-rates :hotelId="hotelId" :isEnabledGoogle="isEnabledGoogleRequest" :isEnabledAPICache="isEnabledAPICacheRequest" :dates="dates"></delete-rates>
                    </b-tab>
                </b-tabs>
            </div>
            <div></div>
        </b-container>
    </div>
</template>

<script>
import Prices from "./components/Prices.vue";
import Closure from "./components/Closure.vue";
import Inventory from "./components/Inventory.vue";
import DeleteRates from "./components/DeleteRates.vue";
import { GetUserPermission } from '../../api/conflux-service';

export default {
    components: {
        Prices,
        Closure,
        Inventory,
        DeleteRates
    },
    created(){

        const start = new Date();
       
        let dateEnd = new Date();
        dateEnd.setDate(dateEnd.getDate() + 1);

        this.dates = {
            start: start,
            end: dateEnd
        }

        this.userHasPermission();

    },
    data (){
        return {
            //Hotel Id
            hotelId: this.$appConfig.session.hotelId,
            isEnabledGoogleRequest: this.$appConfig.google.isEnabledGoogleRequest === 0 ? false : true,
            isEnabledAPICacheRequest: this.$appConfig.google.isEnabledAPICacheRequest === 0 ? false : true,
            hasPermission : false,
            dates: null,
        }
    },
    mounted(){
        console.log(this.isEnabledGoogleRequest);
    },
    methods:{
        userHasPermission(){
            GetUserPermission()
            .then(response =>{
                console.log(response);
                this.hasPermission = response.body.hasPermissions;
            })
            .catch(error => {
               console.log(error);
            });
        }
    }
}
</script>
            
