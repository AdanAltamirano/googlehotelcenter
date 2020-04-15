<template>
    <b-card header="Travel Window">
        <b-row>
            <b-col md="7">
                <b-row>
                    <b-col>
                        <b-form-group :label="$t('Start date of the trip')">
                            <v-date-picker
                            v-model="initialDate"
                            class="form-control p-0"
                            :min-date="new Date()"
                            :popover="{ placement: 'bottom', visibility: 'click' }">
                            </v-date-picker>
                        </b-form-group>
                    </b-col>
                    <b-col>
                        <b-form-group :label="$t('End date of the trip')">
                            <v-date-picker
                            v-model="finalDate"
                            class="form-control p-0"
                            :min-date="new Date()"
                            :popover="{ placement: 'bottom', visibility: 'click' }">
                            </v-date-picker>
                        </b-form-group>
                    </b-col>
                </b-row>
                <hr>
                <b-row>
                    <b-col>
                        <h6>{{ $t('Exclude promotion in the following days') }}</h6>
                    </b-col>
                </b-row>
            </b-col>
            <b-col>
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
    data() {
        return {
            days: [
                { visible: false, day: 0},
                { visible: false, day: 1},
                { visible: false, day: 2},
                { visible: false, day: 3},
                { visible: false, day: 4},
                { visible: false, day: 5},
                { visible: false, day: 6},
            ],
            validDays: [],
            noArrivalDays: [],
            initialDate: null,
            finalDate: null
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