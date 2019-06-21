<template>
<!-- eslint-disable -->
    <div class="bg-white border-top border-bottom border-5 pl-0 pr-0">
        <div class="ml-3 mr-3">
            <div class="d-flex">
                <div class="d-flex justify-content-end align-items-center border-right w-30 pr-5">
                    <button @click="addMonths(-1)" class="btn btn-link btn-sm"><i class="fa fa-angle-double-left"></i></button>
                    <button @click="addDays(-1)" class="btn btn-link btn-sm"><i class="fa fa-angle-left"></i></button>
                    <v-date-picker
                    v-model="currentDay"
                    :popover="{ placement: 'bottom', visibility: 'click' }"
                    :min-date="new Date()"
                    :is-required="true"
                    title-position="left"
                    :locale="$appConfig.language">
                        <a href="javascript:;" class="text-decoration-none h3">{{ currentDay | moment('MMM D, YYYY')}}</a>
                    </v-date-picker>
                    <button @click="addDays(1)" class="btn btn-link btn-sm"><i class="fa fa-angle-right"></i></button>
                    <button @click="addMonths(1)" class="btn btn-link btn-sm"><i class="fa fa-angle-double-right"></i></button>
                </div>
                <div class="d-flex w-70 two-weeks">
                    <div v-for="(day, idx) in dates" :key="idx" class="border p-1 flex-fill text-center">
                        <span>{{day.format('ddd')}}</span>
                        <h3 class="font-weight-bold mt-0 mb-0">{{day.format('DD')}}</h3>
                        <span class="text-uppercase">{{day.format('MMM')}}</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
<!-- eslint-enable -->
</template>

<script>
import Utilities from '../../../core/utilities';

export default {
    name: 'calendar-ribbon',
    props: {
        dateRange: {
            type: Object,
            required: true,
        },
    },
    data() {
        return {
            currentDay: Utilities.getLastWorkDay().toDate(),
            today: this.$moment(),
        };
    },
    computed: {
        dates() {
            if (!this.dateRange.start || !this.dateRange.end) return [];
            return Array(1 + this.dateRange.end.diff(this.dateRange.start, 'days')).fill(0)
                .map((v, i) => this.dateRange.start.clone().add(i, 'days'));
        },
    },
    methods: {
        addDays(days) {
            const newVal = this.$moment(this.currentDay).add(days, 'days');
            if (!this.today.isAfter(newVal, 'day')) {
                this.currentDay = newVal.toDate();
            }
        },
        addMonths(months) {
            const newVal = this.$moment(this.currentDay).add(months, 'months');
            if (!this.today.isAfter(newVal, 'day')) {
                this.currentDay = newVal.toDate();
            }
        },
    },
    watch: {
        currentDay(newDay) {
            if (!this.$moment(newDay).isSame(this.dateRange.start, 'day')) {
                const start = this.$moment(newDay);
                const end = start.clone().add(13, 'days');
                Utilities.setLastWorkDay(start);
                this.$store.commit('update', { start, end });
            }
        },
    },
};
</script>
