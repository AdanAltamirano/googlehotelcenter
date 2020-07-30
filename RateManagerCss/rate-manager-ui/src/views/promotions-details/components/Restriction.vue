<template>
    <b-card :header="$t('Restrictions')">
        <b-row>
            <b-col>
                <b-row>
                    <b-col>
                        <b-form-group :label="$t('Min. nights')">
                            <b-form-input v-model="model.minNights" type="number"></b-form-input>
                        </b-form-group>
                    </b-col>
                    <b-col>
                        <b-form-group :label="$t('Max. nights')">
                            <b-form-input v-model="model.maxNights" type="number"></b-form-input>
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
                            <b-form-select v-model="model.cancellationType" :options="options"></b-form-select>
                        </b-form-group>
                    </b-col>
                </b-row>
                <b-row v-if="!notCancelable">
                    <b-col>
                        <p class="mt-2 mr-3">
                            {{ $t('Cancel') }}&nbsp;
                            <input type="number" min="1" class="input-border-bottom" v-model="byDay" v-if="model.cancellationType == 0" />
                            <input type="number" min="1" class="input-border-bottom" v-model="byHour" v-else-if="model.cancellationType == 1" />
                            <span v-else>
                                {{ $t('before')}}&nbsp;
                                <input type="number" min="1" max="23" class="input-border-bottom" v-model="bySpecificTime.hour"/>&nbsp;:&nbsp;
                                <input type="number" min="0" max="45" step="15" class="input-border-bottom" v-model="bySpecificTime.minuts"/>
                            </span>&nbsp;
                            <span v-if="cancellationType == 0">{{ perDayTxt }}</span>
                            <span v-else-if="cancellationType == 1">{{ perHourTxt }}</span>
                            <span v-else>{{ $t('from check in day') }}</span>
                        </p>
                        <cite class="mr-3 font-weight-bold">"{{ cancellationTxt }}"</cite>
                    </b-col>
                </b-row>
            </b-col>
            <b-col>
                <b-form-group :label="$t('Prior cancellation policy')">
                    <b-tabs>
                        <b-tab :title="$t('Spanish')">
                            <b-form-input v-model="model.prevCancel_es" />
                        </b-tab>
                        <b-tab :title="$t('English')">
                            <b-form-input v-model="model.prevCancel_en" />
                        </b-tab>
                    </b-tabs>
                </b-form-group>
                <b-form-group :label="$t('Detailed cancellation policy')">
                    <b-tabs>
                        <b-tab :title="$t('Spanish')">
                            <b-form-textarea v-model="model.detsCancel_es" rows="5" max-rows="5" />
                        </b-tab>
                        <b-tab :title="$t('English')">
                            <b-form-textarea v-model="model.detsCancel_en" rows="5" max-rows="5" />
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
        minNights: {
            type: Number,
            required: true
        },
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
                { value: 0, text: this.$t('For days') },
                { value: 1, text: this.$t('For hours') },
                { value: 2, text: this.$t('Per specific hour') }
            ],
            byDay: 1,
            byHour: 1,
            bySpecificTime: {
                hour: 1,
                minuts: 0
            }
        }
    },
    computed: {
        perDayTxt() {
            let translate = this.$t('day{s} before check in');
            translate = translate.replace('{s}', this.byDay > 1 ? 's' : '');
            return translate;
        },
        perHourTxt() {
            let translate = this.$t('hour{s} before check in');
            translate = translate.replace('{s}', this.byHour > 1 ? 's' : '');
            return translate;
        },
        cancellationTxt() {
            let translate = '';

            if (this.model.cancellationType === 0) {
                translate = this.$t('Cancel {number} day{s} before check in');
                translate = translate.replace('{number}', this.byDay);
                translate = translate.replace('{s}', this.byDay > 1 ? 's' : '');
            }
            else if (this.model.cancellationType == 1) {
                translate = this.$t('Cancel {number} hour{s} before check in');
                translate = translate.replace('{number}', this.byHour);
                translate = translate.replace('{s}', this.byHour > 1 ? 's' : '');
            }    
            else {
                translate = this.$t('Cancel before {number} from check in day');
                translate = translate.replace('{number}', `${this.bySpecificTime.hour}:${this.bySpecificTime.minuts}`)
            }
         
            return translate;
        }
    }
}
</script>