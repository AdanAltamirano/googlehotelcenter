<template>
    <b-card header="Travel Window">
        <b-row>
            <b-col md="6">
                <b-row>
                    <b-col>
                        <b-form-group :label="$t('Start date of the trip')">
                            <b-input-group>
                                <v-date-picker
                                v-model="initialDate"
                                class="form-control p-0"
                                :min-date="new Date()"
                                :popover="{ placement: 'bottom', visibility: 'click' }">
                                </v-date-picker>

                                <template v-slot:prepend>
                                    <b-input-group-text >
                                        <i class="fa fa-calendar"></i>
                                    </b-input-group-text>
                                </template>
                            </b-input-group>
                        </b-form-group>
                    </b-col>
                    <b-col>
                        <b-form-group :label="$t('End date of the trip')">
                            <b-input-group>
                                <v-date-picker
                                v-model="finalDate"
                                class="form-control p-0"
                                :min-date="new Date()"
                                :popover="{ placement: 'bottom', visibility: 'click' }">
                                </v-date-picker>

                                <template v-slot:prepend>
                                    <b-input-group-text>
                                        <i class="fa fa-calendar"></i>
                                    </b-input-group-text>
                                </template>
                            </b-input-group>
                        </b-form-group>
                    </b-col>
                </b-row>
                <hr>
                <b-row>
                    <b-col>
                        <h6>{{ $t('Exclude promotion in the following days') }}</h6>
                        <b-input-group class="mb-3">
                            <v-date-picker
                            v-model="excludeDates"
                            class="form-control p-0"
                            :min-date="new Date()"
                            mode="range"
                            :popover="{ placement: 'bottom', visibility: 'click'}"
                            :columns="2">
                            </v-date-picker>

                            <template v-slot:prepend>
                                <b-input-group-text>
                                    <i class="fa fa-calendar"></i>
                                </b-input-group-text>
                            </template>
                        </b-input-group>
                        <cite class="cite-date" v-show="excludeDates != null">
                            {{ excludeDatesTxt }}
                            <span @click="excludeDates = null" class="ml-4 mt-2 text-danger">
                                ( <i class="fa fa-times"></i> ) {{ $t('Remove') }}
                            </span>
                        </cite>
                        <cite v-show="excludeDates == null">&nbsp;</cite>
                    </b-col>
                </b-row>
            </b-col>
            <b-col md="auto" class="mr-auto ml-auto">
                <b-form-group :label="$t('Promotion valid for specific day')">
                    <div class="btn-group-toggle btn-group">
                        <label
                        v-for="d in days"
                        :key="d.day"
                        class="btn btn-secondary"
                        :class="{active: validDays.includes(d.day)}">
                            <input type="checkbox" :value="d.day" v-model="validDays" autocomplete="off" /> {{ getDayPrefix(d.day) }}
                        </label>
                    </div>
                </b-form-group>
                <b-form-group :label="$t('No arrivals')" class="mt-3">
                    <div class="btn-group-toggle btn-group">
                        <label
                        v-for="d in days"
                        :key="d.day"
                        class="btn btn-secondary"
                        :class="{active: noArrivalDays.includes(d.day)}">
                            <input type="checkbox" :value="d.day" v-model="noArrivalDays" autocomplete="off" /> {{ getDayPrefix(d.day) }}
                        </label>
                    </div>
                </b-form-group>
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
export default {
    name: 'travel-window',
    created() {
        this.days.forEach(x => {
            this.validDays.push(x.day);
        });
    },
    data() {
        return {
            days: [
                { day: 0}, //domingo
                { day: 1}, //..
                { day: 2},
                { day: 3},
                { day: 4},
                { day: 5},
                { day: 6}, //sabado
            ],
            validDays: [],
            noArrivalDays: [],
            initialDate: null,
            finalDate: null,
            excludeDates: null
        }
    },
    computed: {
        excludeDatesTxt() {
            if (this.excludeDates != null) {
                return `${this.$moment(this.excludeDates.start).format('DD-MMMM-YYYY')} - 
                ${this.$moment(this.excludeDates.end).format('DD-MMMM-YYYY')}`;
            }
            return '';
        }
    },
    methods: {
        getDayPrefix(day) {
            let d = '';
            switch(day) {
                case 0: d = 'SU'; break;
                case 1: d = 'M'; break;
                case 2: d = 'TU'; break;
                case 3: d = 'W'; break;
                case 4: d = 'T'; break;
                case 5: d = 'F'; break;
                case 6: d = 'S'; break;
            };

            let translate = this.$t(d);
            if (translate.length > 1) {
                translate = translate.substring(0, 1);
            }
            return translate;
        }
    }
}
</script>