<template>
<!-- eslint-disable -->
    <div class="p-0" id="app">
        <div class="d-flex flex-row-reverse justify-content-between pl-3 pr-3">
            <button data-toggle="collapse" class="btn btn-link mb-2 pr-0" data-target="#bulk-update-form">
                <span>{{'bulk update' | translate}} <i class="fa fa-archive ml-2"></i></span>
            </button>
            <h2 class="text-primary mb-2">{{'Daily rates' | translate}}</h2>
        </div>
        <!--rates form-->
        <bulk-update v-if="hotel && dateRange.start != null" :hotel="hotel" :initial-date-range="dateRange"></bulk-update>
        <div class="pb-5">
            <calendar-ribbon :date-range="dateRange"></calendar-ribbon>
            <div class="pr-0 pl-0">
                <room-table v-for="room in roomRates" :key="room.id" :room="room"></room-table>
            </div>
        </div>
    </div>
<!-- eslint-enable -->
</template>

<script>
import EventBus from '../../core/event-bus';
import CalendarRibbon from './components/CalendarRibbon.vue';
import RoomTable from './components/RoomTable.vue';
import BulkUpdate from './components/BulkUpdate.vue';
import Utilities from './helpers/utilities';

export default {
    name: 'app',
    components: {
        CalendarRibbon,
        RoomTable,
        BulkUpdate,
    },
    created() {
        EventBus.$on('api.call.begin', this.showLoader);
        EventBus.$on('api.call.end', this.hideLoader);
        EventBus.$on('dayUpdate', this.reload);
    },
    beforeMount() {
        // check for last work day
        const start = Utilities.getLastWorkDay();
        const end = start.clone().add(13, 'days');
        this.$store.commit('getHotel');
        this.$store.commit('update', { start, end });
    },
    data() {
        return {
            loader: null,
        };
    },
    computed: {
        hotel() {
            return this.$store.getters.hotel;
        },
        roomsCatalog() {
            return this.$store.getters.rooms;
        },
        roomRates() {
            return this.$store.getters.roomsWithRatesAndInventory;
        },
        dateRange() {
            return this.$store.getters.dateRange;
        },
    },
    methods: {
        showLoader() {
            this.loader = this.$loading.show({ color: this.$appConfig.themeColors.info, height: 128, width: 128 });
        },
        hideLoader() {
            this.loader.hide();
        },
        reload() {
            this.$store.commit('update', this.dateRange);
        },
    },
};
</script>
