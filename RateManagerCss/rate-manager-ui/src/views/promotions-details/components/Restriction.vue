<template>
    <b-card :header="$t('Restrictions')">
        <!-- {{model}} -->
        <b-row>
            <b-col>
                <b-row>
                    <b-col>
                        <b-form-group :label="$t('Min. nights')">
                            <b-form-input v-model="model.minNights" min="0" type="number"></b-form-input>
                        </b-form-group>
                    </b-col>
                    <b-col>
                        <b-form-group :label="$t('Max. nights')">
                            <b-form-input v-model="model.maxNights" min="0" type="number"></b-form-input>
                        </b-form-group>
                    </b-col>
                </b-row>
                <hr class="mt-0 mb-2" />
                <b-row>
                    <b-col md="12" class="text-right">
                        <b-form-checkbox v-model="notCancelable">{{ $t('Not cancelable') }}</b-form-checkbox>
                    </b-col>
                    <b-col md="12" class="mt-3" v-if="!notCancelable">
                        <b-form-group :label="$t('Cancellation policies')">
                            <b-form-select v-model="model.offsetTimeUnit" :options="options"></b-form-select>
                        </b-form-group>
                    </b-col>
                </b-row>
                <b-row v-if="!notCancelable">
                    <b-col>
                        <p class="mt-2 mr-3">
                            {{ $t('Cancel') }}&nbsp;
                            <input type="number" min="1" class="input-border-bottom" v-model="model.offsetTimeUnitMiltiplier" v-if="model.offsetTimeUnit == 0" />
                            <input type="number" min="1" class="input-border-bottom" v-model="model.offsetTimeUnitMiltiplier" v-else-if="model.offsetTimeUnit == 1" />
                            <!-- <span v-else>
                                {{ $t('before')}}&nbsp;
                                <input type="number" min="1" max="23" class="input-border-bottom" v-model="model.bySpecificTime.byHour"/>&nbsp;:&nbsp;
                                <input type="number" min="0" max="45" step="15" class="input-border-bottom" v-model="model.bySpecificTime.minuts"/>
                            </span>&nbsp; -->
                            <span v-if="model.offsetTimeUnit== 0">{{ perDayTxt }}</span>
                            <span v-else-if="model.offsetTimeUnit == 1">{{ perHourTxt }}</span>
                            <!-- <span v-else>{{ $t('from check in day') }}</span> -->
                        </p>
                        <cite class="mr-3 font-weight-bold">"{{ cancellationTxt }}"</cite>
                    </b-col>
                </b-row>
            </b-col>
            <b-col>
                <b-form-group :label="$t('Prior cancellation policy')">
                    <b-tabs>
                        <b-tab :title="$t('Spanish')">
                            <b-form-input v-model="model.shortDescription.esp" trim/>
                        </b-tab>
                        <b-tab :title="$t('English')">
                            <b-form-input v-model="model.shortDescription.eng" trim/>
                        </b-tab>
                    </b-tabs>
                </b-form-group>
                <b-form-group :label="$t('Detailed cancellation policy')">
                    <b-tabs>
                        <b-tab :title="$t('Spanish')">
                            <b-form-textarea v-model="model.detailedDescription.esp" rows="5" max-rows="5" trim/>
                        </b-tab>
                        <b-tab :title="$t('English')">
                            <b-form-textarea v-model="model.detailedDescription.eng" rows="5" max-rows="5" trim/>
                        </b-tab>
                    </b-tabs>
                </b-form-group>
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
export default {
    props: {
        dataModel: {
            type: Object,
            required: true
        }
    },
    data() {
        return {
            model: this.dataModel,
            notCancelable: false,
            options: [
                { value: -1, text: this.$t('Select') },
                { value: 0, text: this.$t('For days') },
                { value: 1, text: this.$t('For hours') },
                // { value: 2, text: this.$t('Per specific hour') }
            ]
        }
    },
    watch:{
        notCancelable:function(val){
            if(val){
                // this.model.byDay = null;
                // this.model.byHour = null;
                // this.model.cancellationType = -1;
                //this.model.offsetTimeUnitMiltiplier = 1;
            }
        },
        'model.offsetTimeUnit':function(val){
            if(val === -1){
                // this.model.byDay = null;
                // this.model.byHour = null;
                this.model.offsetTimeUnitMiltiplier = 1;
            }
        }
        
    },
    computed: {
        perDayTxt() {
            let translate = this.$t('day{s} before check in');
            translate = translate.replace('{s}', this.model.offsetTimeUnitMiltiplier > 1 ? 's' : '');
            return translate;
        },
        perHourTxt() {
            let translate = this.$t('hour{s} before check in');
            translate = translate.replace('{s}', this.model.offsetTimeUnitMiltiplier > 1 ? 's' : '');
            return translate;
        },
        cancellationTxt() {
            let translate = '';

            if (this.model.offsetTimeUnit === 0) {
                translate = this.$t('Cancel {number} day{s} before check in');
                translate = translate.replace('{number}', this.model.offsetTimeUnitMiltiplier);
                translate = translate.replace('{s}', this.model.offsetTimeUnitMiltiplier > 1 ? 's' : '');
            }
            else if (this.model.offsetTimeUnit == 1) {
                translate = this.$t('Cancel {number} hour{s} before check in');
                translate = translate.replace('{number}', this.model.offsetTimeUnitMiltiplier);
                translate = translate.replace('{s}', this.model.offsetTimeUnitMiltiplier > 1 ? 's' : '');
            }    
            else {
                //translate = this.$t('Cancel before {number} from check in day');
                //translate = translate.replace('{number}', `${this.model.bySpecificTime.hour}:${this.model.bySpecificTime.minuts}`)
            }
         
            return translate;
        }
    }
}
</script>