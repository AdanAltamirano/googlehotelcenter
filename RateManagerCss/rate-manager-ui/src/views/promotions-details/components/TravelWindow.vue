<template>
    <b-card header="Travel Window">
        <b-row>
            <b-col md="5">
                <b-row>
                    <b-col md="12">
                        <b-form-group :label="$t('Start date of the trip')">
                            <b-input-group>
                                <v-date-picker
                                v-model="model.initialDate"
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
                                v-model="model.finalDate"
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
            </b-col>
            <b-col md="auto" class="mr-auto ml-auto pr-1 pl-1">
                <b-form-group :label="$t('Promotion valid for specific day')">
                    <div class="btn-group-toggle btn-group">
                        <label
                        v-for="d in days"
                        :key="d.day"
                        class="btn btn-secondary"
                        :class="{'active-blue': model.validDays.includes(d.day)}">
                            <input type="checkbox" :value="d.day" v-model="model.validDays" autocomplete="off" /> {{ getDayPrefix(d.day) }}
                        </label>
                    </div>
                </b-form-group>
                <b-form-group :label="$t('No arrivals')" class="mt-3">
                    <div class="btn-group-toggle btn-group">
                        <label
                        v-for="d in days"
                        :key="d.day"
                        class="btn btn-secondary"
                        :class="{active: model.noArrivalDays.includes(d.day)}">
                            <input type="checkbox" :value="d.day" v-model="model.noArrivalDays" autocomplete="off" /> {{ getDayPrefix(d.day) }}
                        </label>
                    </div>
                </b-form-group>
            </b-col>
        </b-row>
        <b-row>
            <b-col md="9">
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
                    <b-input-group-append>
                        <b-button :disabled="excludeDates == null" @click="addClosure()" variant="primary">
                            {{ $t('Add closure') }}
                        </b-button>
                    </b-input-group-append>
                </b-input-group>

                <b-list-group>
                    <b-list-group-item class="cite-date" v-for="(c, index) in model.closures" :key="index">
                        {{ getDateFormat(c) }}
                        <span @click="removeClosure(index)" class="ml-4 mt-2 text-danger">
                            ( <i class="fa fa-times"></i> ) {{ $t('Remove') }}
                        </span>
                    </b-list-group-item>
                </b-list-group>
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
            days: [
                { day: 0}, //domingo
                { day: 1}, //..
                { day: 2},
                { day: 3},
                { day: 4},
                { day: 5},
                { day: 6}, //sabado
            ],
            excludeDates: null
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
        },
        getDateFormat(date) {
            return `${this.$moment(date.start).format('DD-MMMM-YYYY')} - ${this.$moment(date.end).format('DD-MMMM-YYYY')}`;
        },
        addClosure() {
            let add = false;
            if (this.model.closures.length > 0)
                add =
                (this.model.closures.findIndex(x => 
                    x.start.toString() === this.excludeDates.start.toString() 
                    && x.end.toString() === this.excludeDates.end.toString()
                ) === -1)
            else add = true;

            if (add) {
                this.model.closures.push(this.excludeDates);
                this.excludeDates = null;
            }
        },
        removeClosure(index) {
            this.model.closures.splice(index, 1);
        }
    }
}
</script>