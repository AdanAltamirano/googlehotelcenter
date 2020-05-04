<template>
    <b-card no-body :header="$t('Promotion type')">
        <b-row>
            <b-col>
                <b-card no-body>
                    <b-tabs pills card vertical>
                        <b-tab :title="$t('Free night')">
                            <b-form-checkbox v-model="async_freeNight" class="mb-3 text-right" switch>
                                {{ $t('Add free night') }}
                            </b-form-checkbox>
                            <b-row>
                                <b-col>
                                    <p>
                                        <select class="input-border-bottom" v-model="typeFreeNight">
                                            <option value="0">{{ $t('Every') }}</option>
                                            <option value="1">{{ $t('Only') }}</option>
                                        </select>&nbsp;
                                        <span v-if="typeFreeNight == '1'">{{ $t('the') }}</span>
                                        <input type="number" v-model="freeNightNumber" min="1" class="input-border-bottom" />
                                        {{ getPrefix(freeNightNumber) }}&nbsp;{{ $t('will be free') }}
                                    </p>
                                    <p>
                                        <cite class="font-weight-bold">"{{ freeNightTxt }}"</cite>
                                    </p>
                                </b-col>
                            </b-row>
                        </b-tab>
                        <b-tab :title="$t('Discount')">
                            <b-form-checkbox v-model="async_discount" class="mb-3 text-right" switch>
                                {{ $t('Add discount') }}
                            </b-form-checkbox>
                            <b-row>
                                <b-col md="4">
                                    <b-form-group :label="$t('Percentage')">
                                        <b-input-group append="%">
                                            <b-form-input v-model="percentage"></b-form-input>
                                        </b-input-group>
                                    </b-form-group>
                                </b-col>
                                <b-col md="8">
                                    <b-form-group :label="$t('Application mode')">
                                        <b-form-select v-model="applicationMode" :options="options"></b-form-select>
                                    </b-form-group>
                                </b-col>
                                <b-col class="text-right">
                                    <b-link @click="help" href="#">{{ $t('Help') }}</b-link>
                                </b-col>
                            </b-row>
                        </b-tab>
                    </b-tabs>
                </b-card>
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
import Help from '../helper/help.vue';
import Vue from 'vue';

export default {
    name: 'type-promotion',
    data() {
        return {
            language: this.$appConfig.language,
            typeFreeNight: '0',
            freeNightNumber: 1,
            percentage: 0,
            options: [
                { value: 0, text: this.$t('Priority to discount rate') },
                { value: 1, text: this.$t('Sum discount percentage') },
                { value: 2, text: this.$t('Additional discount') }
            ],
            applicationMode: 0,
            discountChecked: false,
            freeNightChecked: false
        }
    },
    computed: {
        /**->state */
        checkFreeNight: {
            get() { return this.$store.state.req.typePromotion.freeNight.check },
            set(val) { this.$store.commit('checkFreeNight', val) }
        },
        modeFreeNight: {
            get() { return this.$store.state.req.typePromotion.freeNight },
            set(val) { this.$store.commit('modeFreeNight', val) }
        },
        quantityFreeNight: {
            get() { return this.$store.state.req.typePromotion.freeNight },
            set(val) { this.$store.commit('quantityFreeNight', val) }
        },
        /**<- */
        freeNightTxt() {
            let translate = '';
            let prefix = '';

            if (this.typeFreeNight === '0')
                translate = this.$t('Every {number}{prefix} night will be free');
            else translate = this.$t('Only the {number}{prefix} night will be free');
            translate = translate.replace('{number}', this.freeNightNumber);
            translate = translate.replace('{prefix}', this.getPrefix(this.freeNightNumber));

            return translate;
        }
    },
    methods: {
        getPrefix(number) {
            let prefix = 'ª';
            number =  Number.parseInt(number);
            if (this.language !== 'es') {
                if (number > 20)
                    number =  Number.parseInt(number.toString().slice(-1));
                switch(number) {
                    case 1: prefix = 'st'; break;
                    case 2: prefix = 'nd'; break;
                    case 3: prefix = 'rd'; break;
                    default: prefix = 'th'; break;
                }
            }
            return prefix;
        },
        help() {
            const component = Vue.extend(Help);
            const instance = new component({});
            instance.$mount();

            let self = this;
            this.$swal
            .fire({
                title: self.$t('Discount application method'),
                icon: 'info',
                confirmButtonText: '<i class="fa fa-thumbs-up"></i>',
                showCancelButton: false,
                showCloseButton: true,
                html: '<div></div>',
                onBeforeOpen: () => {
                    this.$swal
                    .getContent()
                    .querySelector('div')
                    .append(instance.$el);
                }
            });
        }
    }
}
</script>