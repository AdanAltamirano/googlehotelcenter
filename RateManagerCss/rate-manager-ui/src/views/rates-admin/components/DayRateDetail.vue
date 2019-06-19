<template>
<!-- eslint-disable -->
    <v-popover placement="right" @show="loadDailyRate">
        <a href="javascript:;" class="tooltip-target text-decoration-none" v-if="dayRate.price > 0">{{ getPrice(dayRate, rate) | currency}}</a>
        <a href="javascript:;" v-else class="text-danger tooltip-target text-decoration-none"> N/A </a>
        <template slot="popover">
            <h2 v-if="rateDayDetails">{{dayRate.rateId}}</h2>
        </template>
    </v-popover>
<!-- eslint-enable -->
</template>
<script>
import EventBus from '../../../core/event-bus';
import RatesService from '../../../api/rates-service';

export default {
    name: 'day-rate-detail',
    props: {
        dayRate: {
            type: Object,
            required: false,
        },
        rate: {
            type: Object,
            required: false,
        },
    },
    created() {
        EventBus.$on('api.call.begin[rates.getByDay]', this.showLoader);
        EventBus.$on('api.call.begin[rates.getByDay]', this.hideLoader);
    },
    data() {
        return {
            loader: null,
            rateDayDetails: null,
        };
    },
    computed: {
        dayFormatted() {
            return this.$moment(this.dayRate.date).format('YYYY-MM-DD');
        },
    },
    methods: {
        showLoader() {
            this.loader = this.$loading.show({
                color: '#007bff', height: 64, width: 64, isFullPage: false,
            });
        },
        hideLoader() {
            this.loader.hide();
        },
        getPrice(dayRate, rate) {
            let price = dayRate.price * (1 - (dayRate.discount / 100));
            if (rate.factor !== undefined) price *= rate.factor;
            else if (rate.offset !== undefined) price += rate.offset;
            return price;
        },
        loadDailyRate() {
            RatesService.getByDay(
                this.$appConfig.session.hotelId,
                this.dayRate.rateId,
                this.dayFormatted,
            )
                .then((response) => {
                    this.rateDayDetails = response.body;
                });
        },
    },
};
</script>
