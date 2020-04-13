<template>
    <div>
        <b-container fluid>
            <h2 class="text-primary">{{ $t('Promotions') }}</h2>
            <b-row>
                <b-col md="6">
                    <b-card>
                        <b-row>
                            <b-col md="5">
                                <b-form-group :label="$t('Promotion code')">
                                    <b-form-input></b-form-input>
                                </b-form-group>
                                <b-form-group class="pt-2" :label="$t('Promotion name')">
                                    <b-tabs>
                                        <b-tab :title="$t('Spanish')">
                                            <b-form-input></b-form-input>
                                        </b-tab>
                                        <b-tab :title="$t('English')">
                                            <b-form-input></b-form-input>
                                        </b-tab>
                                    </b-tabs>
                                </b-form-group>
                            </b-col>
                            <b-col md="7">
                                <b-form-group :label="$t('Promotion description')">
                                    <b-tabs>
                                        <b-tab :title="$t('Spanish')">
                                            <b-form-textarea rows="5" max-rows="5"></b-form-textarea>
                                        </b-tab>
                                        <b-tab :title="$t('English')">
                                            <b-form-textarea rows="5" max-rows="5"></b-form-textarea>
                                        </b-tab>
                                    </b-tabs>
                                </b-form-group>
                            </b-col>
                        </b-row>
                    </b-card>
                </b-col>
                <b-col md="6">
                    <!--rate plans y habitaciones-->
                    <rate-plan-rooms></rate-plan-rooms>
                </b-col>
            </b-row>
            <b-row>
                <b-col md="6">
                    <!--travel window-->

                </b-col>
                <b-col md="6">
                    <!--booking window-->
                </b-col>
            </b-row>
            <b-row>
                <b-col md="6">
                    <!--tipo de promocion-->
                </b-col>
                <b-col md="6">
                    <!--restricciones-->
                    <restrictions></restrictions>
                </b-col>
            </b-row>
        </b-container>
    </div>
</template>

<script>
import RatePlanRooms from './components/rateplan-rooms.vue';
import Restrictions from './components/restrictions.vue';

export default {
    name: 'app',
    components: {
        RatePlanRooms,
        Restrictions
    }
}
</script>