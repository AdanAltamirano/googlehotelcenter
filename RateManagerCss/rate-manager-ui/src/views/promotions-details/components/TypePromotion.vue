<template>
    <b-card :header="$t('Promotion type')">
        <b-row>
            <b-col>
                <b-card no-body>
                    <b-tabs pills card vertical>
                        <b-tab>
                            <template v-slot:title>
                                <b-form-checkbox v-model="freeNight">{{ $t('Free night') }}</b-form-checkbox>
                            </template>
                            <b-card-text>
                                <p>
                                    <select class="input-border-bottom" v-model="typeFreeNight">
                                        <option value="0">{{ $t('Every') }}</option>
                                        <option value="1">{{ $t('Only') }}</option>
                                    </select>&nbsp;
                                    <span v-if="typeFreeNight == '1'">{{ $t('the') }}</span>
                                    <input type="number" v-model="freeNightNumber" min="1" class="input-border-bottom" />
                                    {{ getPrefix(freeNightNumber) }}&nbsp;{{ $t('will be free') }}
                                </p>
                                <cite class="font-weight-bold">"{{ freeNightTxt }}"</cite>
                            </b-card-text>
                        </b-tab>
                        <b-tab>
                            <template v-slot:title>
                                <b-form-checkbox v-model="discount">{{ $t('Discount') }}</b-form-checkbox>
                            </template>
                            <b-card-text>

                            </b-card-text>
                        </b-tab>
                    </b-tabs>
                </b-card>
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
export default {
    name: 'type-promotion',
    data() {
        return {
            language: this.$appConfig.language,
            freeNight: false,
            discount: false,
            typeFreeNight: '0',
            freeNightNumber: 1
        }
    },
    computed: {
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
        tabChanged() {

        }
    }
}
</script>