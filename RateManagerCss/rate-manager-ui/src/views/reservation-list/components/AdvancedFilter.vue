<template>
    <b-card style="border:0px">
        <b-row>
            <b-col md="3" class="my-1">
                <b-form-group :description="$t('It has priority over advanced search')">
                    <b-form-input v-model="noReservation" :placeholder="$t('Reservation number')" />
                </b-form-group>
            </b-col>
            <b-col md="3" class="my-1">
                <b-row>
                    <b-col md="3">
                        <b-button variant="primary" @click="search">{{$t('Search')}}</b-button>
                    </b-col>
                    <b-col md="9">
                        <b-button class="float-right" v-b-toggle.filter_content variant="link">{{$t('Advanced Search')}}</b-button>
                    </b-col>
                </b-row>
            </b-col>
            <b-col md="6" class="my-1">
                <b-row>
                    <b-col md="8">
                        <b-button v-if="result.length > 0" @click="exportToExcel" variant="primary" style="background-color:green;float:right">
                            <i class="fas fa-file-excel"></i>&nbsp;{{$t('Download')}}
                        </b-button>
                    </b-col>
                    <b-col md="4" style="margin-top:-1.9rem;">
                        <b-form-group label="Items por pagina">
                            <b-form-select @change="changeItemsPerPage" class="float-right" v-model.number="itemPerPage" :options="itemsPerPage"></b-form-select>
                        </b-form-group>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
        <b-row>
            <b-col md="12">
                <b-collapse id="filter_content">
                    <b-card class="mt-3 bg-light">
                        <b-row>
                            <b-col md="4">
                                <b-form-group :label="$t('Search by')">
                                    <b-form-select v-model="typeDate" :options="typeDates"></b-form-select>
                                </b-form-group>
                            </b-col>
                            <b-col md="4">
                                <b-form-group :label="$t('Date Range')">
                                    <b-input-group>
                                        <v-date-picker 
                                        v-model="dates"
                                        class="form-control p-0"
                                        mode="range"
                                        :max-date="new Date()"
                                        :popover="{ placement: 'bottom', visibility: 'click' }"
                                        :columns="2"></v-date-picker>
                                        <b-input-group-append>
                                            <b-button :disabled="dates == null" variant="danger" @click="dates = null">
                                                <i class="fa fa-times"></i>
                                            </b-button>
                                        </b-input-group-append>
                                    </b-input-group>
                                </b-form-group>
                            </b-col>
                            <b-col md="4">
                                <b-form-group :label="$t('Status')">
                                    <b-form-select v-model="status" :options="allStatus"></b-form-select>
                                </b-form-group>
                            </b-col>
                        </b-row>
                        <b-row>
                            <b-col md="4">
                                <b-form-group :label="$t('Customer Name')">
                                    <b-form-input v-model="clientName"></b-form-input>
                                </b-form-group>
                            </b-col>
                            <b-col :md="source === 'IDS' ? '2' : '4'">
                                <b-form-group :label="$t('Origin')">
                                    <b-form-select v-model="source" :options="sources"></b-form-select>
                                </b-form-group>
                            </b-col>
                            <b-col md="2" v-if="source === 'IDS'">
                                <b-form-group label="OTAS">
                                    <b-form-select v-model="ota" :options="otas"></b-form-select>
                                </b-form-group>
                            </b-col>
                            <b-col md="4">
                                <b-form-group v-if="hotels.length > 0" :label="$t('Hotels')">
                                    <multiselect label="name" 
                                    v-model="hotel"
                                    :options="hotels"
                                    track-by="id"
                                    :multiple="true"
                                    :selectLabel="$t('select')"
                                    :selectedLabel="''"
                                    :deselectLabel="''"
                                    :placeholder="$t('Find Hotel')"></multiselect>
                                </b-form-group>
                            </b-col>
                        </b-row>
                    </b-card>
                </b-collapse>
            </b-col>
        </b-row>
    </b-card>
    
</template>

<script>
import HotelService from '../../../api/hotels-service';
import Multiselect from 'vue-multiselect';

export default {
    name: 'advance-filter',
    components: {
        Multiselect
    },
    mounted() {
        this.getHotels();
    },
    props: {
        result: {
            required: false,
            type: Array
        },
        itemPerPage: {
            required: true,
            type: Number
        },
        itemsPerPage: {
            required: true,
            type: Array
        }
    },
    data() {
        return {
            includeDates: false,
            dates: null,
            noReservation: '',
            status: 0,
            typeDate: 'ReservationDate',
            clientName: '',
            source: 'ALL',
            hotel: [],
            hotels: [],
            ota: 'ALL',


            typeDates: [
                { text: this.$t('Reservation date'), value: 'ReservationDate' },
                { text: this.$t('Arrival date'), value: 'CheckOut' },
                { text: this.$t('Departure date'), value: 'CheckIn' }
            ],
            allStatus: [
                { text: '-- ' + this.$t('All') + ' --', value: 0 },
                { text: this.$t('Reserved'), value: 1 },
                { text: this.$t('Cancelled'), value: 3 },
                { text: this.$t('In process'), value: 4 }
            ],
            sources: [
                { text: '-- ' + this.$t('All') + ' --', value: 'ALL' },
                { text: 'Portal', value: 'POR' },
                { text: 'Call Center', value: 'CCT' },
                { text: this.$t('One Page'), value: 'UNI' },
                { text: this.$t('Front Desk'), value:'HTL' },
                { text: 'GDS', value: 'WIZ' },
                { text: 'ADS', value: 'ADS' },
                { text: 'OTAS', value: 'IDS' }
            ],
            otas: [
                { text: '-- ' + this.$t('All') + ' --', value: 'ALL'},
                { text: 'BestDay', value: 'BestDay'},
                { text: 'Booking', value: 'Booking'},
                { text: 'Bookit.com', value: 'Bookit.com'},
                { text: 'Expedia', value: 'Expedia'},
                { text: 'Hotel Beds', value: 'Hotel Beds'},
                { text: 'PriceTravel', value: 'PriceTravel'}
            ]
        }
    },
    methods: {
        search() {
            this.$emit('search', this.getFilter());
        },
        exportToExcel() {
            this.$emit('exportToExcel');
        },
        getFilter() {
            let filter = '';

            //filtran solo el #reservacion
            if (this.noReservation !== '') {
                filter = 'confirmNumber eq ' + this.noReservation;
                this.cleanFilters();
                return filter;
            }

            //si #reservacion es vacio, filtrar por lo demas
            if (this.dates != null) {
                filter += this.typeDate + ' gt ' + this.dateFormat(this.dates.start) +
                ' and ' + this.typeDate + ' lt ' + this.dateFormat(this.dates.end);
            }

            if (this.status != 0)
                filter += (filter !== '' ? ' and ' : '') + 'Status eq ' + this.status;

            if (this.clientName !== '')
                filter += (filter !== '' ? ' and ' : '') + 'Client lk ' + this.clientName;
            
            if (this.source != 'ALL')
                filter += (filter !== '' ? ' and ' : '') + 'Source eq ' + this.source;

            if (this.source === 'IDS' && this.ota != 'ALL')
                filter += (filter !== '' ? ' and ' : '') + 'Portal eq ' + this.ota;

            if (this.hotel.length > 0) {
                filter += (filter !== '' ? ' and ' : '');
                this.hotel.forEach((value, index) => {
                    if (index > 0)
                        filter += ' or ';
                    filter += 'hotelId eq ' + value.id;
                });
            }

            return (filter === '' ? null : filter);
        },


        getHotels() {
            HotelService.getList().then((response) => {
                this.hotels = response.body;
            });
        },
        dateFormat(date) {
            return this.$moment(date).format('YYYY-MM-DD');
        },
        cleanFilters() {
            this.status = 0;
            this.dates = null;
            this.clientName = '';
            this.source = 'ALL';
            this.hotel = [];
        },
        changeItemsPerPage() {
            this.$emit('changeItems', this.itemPerPage);
        }
    }
};
</script>