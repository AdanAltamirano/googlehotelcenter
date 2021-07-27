<template>
    <b-card no-body class="w-100">
        <template slot="header">
            {{$t('Promotion type')}}
            <b-checkbox 
            id="free-night-checkbox" 
            name="free-night-checkbox"
            v-model="isCheckedFreeNight"
            :value="true"
            :unchecked-value="false"
            class="d-inline ml-2">
                {{$t('Free night')}}
            </b-checkbox>
             <b-checkbox 
             id="discoutn-checkbox" 
             name="discount-checkbox"
             v-model="isCheckedDiscount"
             :value="true"
             :unchecked-value="false"
             class="d-inline ml-2">
                {{$t('Discount')}}
            </b-checkbox>
        </template>
        <b-row>
            <b-col>
                <b-card no-body>
                    <b-tabs pills card vertical>
                        <b-tab :title="$t('Free night')">
                            <b-row>
                                <b-col>
                                    <p>
                                        <select class="input-border-bottom" v-model="model.discountPattern">
                                            <option value="0">{{ $t('Every') }}</option>
                                            <option value="1">{{ $t('Only') }}</option>
                                        </select>
                                        &nbsp;<span v-if="model.nightsDiscounted === '1'">{{ $t('the') }}</span>
                                        
                                        <input
                                        type="number"
                                        v-model="model.nightsDiscounted"
                                        min="0"
                                        class="input-border-bottom" /> {{ getPrefix() }}
                                        &nbsp;{{ $t('will be free') }}

                                    </p>
                                    <p v-if="model.nightsDiscounted > 0">
                                        <cite class="font-weight-bold">"{{ freeNightTxt }}"</cite>
                                    </p>
                                </b-col>
                            </b-row>
                        </b-tab>
                        <b-tab :title="$t('Discount')">
                            <b-row>
                                <b-col md="5">
                                    <b-form-group :label="$t('Percentage')">
                                        <b-input-group append="%">
                                            <b-form-input @keypress="numberFormat" v-model="model.percent" />
                                        </b-input-group>
                                    </b-form-group>
                                </b-col>
                                <b-col md="7">
                                    <b-form-group :label="$t('Application mode')">
                                        <b-form-select v-model="model.applicationMode" :options="options" />
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
import help from '../helper/help.vue';
import { onlyNumber } from '../helper/util';
import Vue from 'vue';

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
            language: this.$appConfig.language,
            options: [
                { value: 0, text: this.$t('Priority to discount rate') },
                { value: 1, text: this.$t('Sum discount percentage') },
                { value: 2, text: this.$t('Additional discount') }
            ],
            isCheckedFreeNight:false,
            isCheckedDiscount:false
        }
    },
    watch:{
        isCheckedFreeNight:function(value){
            console.log("Is Checked Free Night Value: " + value);
            console.log(typeof(value));
            if(value){
                console.log(this.model);
            }
            else{
                this.model.discountPattern = null;
                this.model.nightsDiscounted = null;
                console.log(this.model);
            }
        },
        isCheckedDiscount:function(value){
            console.log("Is Checked Discount Value: " + value);
            if(value){
                console.log(this.model);
                
            }
            else{
                this.model.applicationMode = null;
                this.model.percent = null;
                console.log(this.model);
            }
        }
    },
    created(){
        if(this.model.discountPattern != null)
        {
            this.isCheckedFreeNight = true;
        }

        if(this.model.applicationMode != null)
        {
            this.isCheckedDiscount = true;
        }
    },
    computed: {
        freeNightTxt() {
            let translate = '';
            let prefix = '';

            if (this.model.discountPattern.toString() === '0')
                translate = this.$t('Every {number}{prefix} night will be free');
            else translate = this.$t('Only the {number}{prefix} night will be free');
            translate = translate.replace('{number}', this.model.nightsDiscounted);
            translate = translate.replace('{prefix}', this.getPrefix());

            return translate;
        }
    },
    methods: {
        getPrefix() {
            let prefix = 'ª';
            let number =  Number.parseInt(this.model.nightsDiscounted);
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
            // const component = Vue.extend(help);
            // const instance = new component();
            // instance.$mount();
            const html = this.helpText();
            console.log(html);
            // let self = this;
            this.$appAlert({
                title: this.$t('Discount application method'),
                icon: 'info',
                confirmButtonText: '<i class="fa fa-thumbs-up"></i>',
                showCancelButton: false,
                showCloseButton: true,
                html:html,
                // onBeforeOpen: () => {
                //     this.$swal
                //     .getContent()
                //     .querySelector('div')
                //     .append(instance.$el);
                // }
            });
        },
        numberFormat(evt) {
            onlyNumber(evt);
        },
        helpText()
        {
            let text = '<p>' + `${this.$t('The <b>application mode</b> will only affect when there is a discount at the tariff level. Otherwise, the discount percentage set here will be applied.')}` + '</p>';
            text += '<p>' + `${this.$t('Assuming that in the promotion it was configured with a 40% discount and in the rate it was configured with a 20% discount, it would be as follows:')}`+ '</p>';
            text += '<h4>' + `${this.$t('Priority to discount rate')}` + '</h4>';
            text += '<p>' + `${this.$t('Only 20% discount will be applied.')}` + '</p>';
            text += '<h4>' + `${this.$t('Sum discount percentage')}` + '</h4>';
            text += '<p>' + `${this.$t('A 60% discount will be applied.')}` + '</p>';
            text += '<h4>' +`${this.$t('Additional discount')}` + '</h4>';
            text += '<p>' + `${this.$t('First the 40% discount will be applied and the result of that will be applied the 20% discount.')}` + '</p>';
            return text;
        }
    }
}
</script>
