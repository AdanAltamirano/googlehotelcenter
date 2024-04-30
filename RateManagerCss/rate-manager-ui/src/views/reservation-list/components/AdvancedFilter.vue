<template>
    <!-- eslint-disable -->
    <b-card no-body style="border:0px">
        <b-row>
            <b-col md="3" class="my-1">
                <b-form-group :description="$t('It has priority over advanced search')">
                    <b-form-input v-model="noReservation" :placeholder="$t('Reservation number')" />
                </b-form-group>
            </b-col>
            <b-col md="3" class="my-1">
                <b-row>
                    <b-col md="3">
                        <b-button variant="primary" @click="search">{{ $t("Search") }}</b-button>
                    </b-col>
                    <b-col md="9">
                        <b-button class="float-right" v-b-toggle.filter_content variant="link">{{ $t("Advanced Search") }}</b-button>
                    </b-col>
                </b-row>
            </b-col>
            <b-col md="6" class="my-1">
                <b-row>
                    <b-col md="8">
                        <b-button
                        v-if="result.length > 0"
                        @click="exportToExcel"
                        style="background-color:green;float:right"
                        variant="primary">
                            <i class="fas fa-file-excel"></i> &nbsp;{{ $t("Export page") }}
                        </b-button>
                    </b-col>
                    <b-col md="4" style="margin-top:-1.9rem;">
                        <b-form-group :label="$t('items per page')">
                            <b-form-select
                            @change="changeItemsPerPage"
                            class="float-right"
                            v-model.number="itemPerPage"
                            :options="itemsPerPage">
                            </b-form-select>
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
                                        :min-date="minDate"
                                        :popover="{ placement: 'bottom', visibility: 'click' }"
                                        :columns="2">
                                        </v-date-picker>

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
                                    <b-form-checkbox-group v-model="checkStatus">
                                        <b-form-checkbox value="1">{{ $t('Reserved') }}</b-form-checkbox>
                                        <b-form-checkbox value="4" v-if="isSupervisor">{{ $t('In process') }}</b-form-checkbox>
                                        <b-form-checkbox value="3">{{ $t('Cancelled') }}</b-form-checkbox>
                                    </b-form-checkbox-group>
                                </b-form-group>
                            </b-col>
                        </b-row>
                        <b-row>
                            <b-col md="4">
                                <b-form-group :label="$t('Customer Name')">
                                    <b-form-input v-model="clientName"></b-form-input>
                                </b-form-group>
                            </b-col>
                            <b-col :md="source === 'IDS' || source === 'AGENCY' ? '2' : '4'">
                                <b-form-group :label="$t('Origin')">
                                    <b-form-select v-model="source" :options="sources"></b-form-select>
                                </b-form-group>
                            </b-col>
                            <b-col md="2" v-if="source === 'IDS'">
                                <b-form-group label="OTAS">
                                    <b-form-select v-model="ota" :options="otas"></b-form-select>
                                </b-form-group>
                            </b-col>
                            <b-col md="2" v-if="source === 'AGENCY'">
                                <b-form-group :label="$t('Agencies')">
                                    <b-form-select v-model="agency" :options="agencies"></b-form-select>
                                </b-form-group>
                                 <b-form-group v-show="agency != -1" :label="$t('Agents')">
                                    <b-form-select v-model="agent" :options="agents"></b-form-select>
                                </b-form-group>
                            </b-col>
                            <b-col md="4">
                                <b-form-group v-if="hotels.length > 0">
                                    <template slot="label">
                                        <div class="d-flex">
                                            <span class="mr-2">{{ $t('Hotels') }}</span>
                                            <b-form-checkbox v-if="corporates.length > 0" v-model="isCheckCorporate" switch>
                                                <span>{{ $t('Corporate') }}</span>
                                            </b-form-checkbox>
                                        </div>
                                    </template>

                                    <!--hoteles-->
                                    <multiselect
                                    v-if="!isCheckCorporate"
                                    v-model="hotel"
                                    :custom-label="nameWithCorporate"
                                    :options="hotels"
                                    track-by="id"
                                    :multiple="true"
                                    :selectLabel="$t('select')"
                                    :selectedLabel="''"
                                    :deselectLabel="''"
                                    :placeholder="$t('Search Hotel')">
                                    </multiselect>

                                    <!--corporativos-->
                                    <multiselect v-if="isCheckCorporate"
                                    label="nombreCorp"
                                    v-model="corporate"
                                    :options="corporates"
                                    track-by="idCorporativo"
                                    :multiple="true"
                                    :selectLabel="$t('select')"
                                    :selectedLabel="''"
                                    :deselectLabel="''"
                                    :placeholder="$t('Search Corporate')">
                                    </multiselect>
                                </b-form-group>
                            </b-col>
                        </b-row>
                        <b-row>
                            <b-col md="4">
                                <b-form-group>
                                    <template slot="label">
                                        <div class="d-flex">
                                            <span class="mr-2">{{ $t('Payment Way') }}</span>                                           
                                        </div>
                                    </template>
                                    <multiselect
                                    track-by="paymentMethodId"
                                    label="paymentMethod"
                                    v-model="paymentMethod"
                                    :options="paymentMethods"
                                    :multiple="true"
                                    :selectLabel="$t('select')"
                                    :selectedLabel="''"
                                    :deselectLabel="''"
                                    :placeholder="$t('')">
                                    </multiselect>
                                </b-form-group>
                            </b-col>
                        </b-row>
                        <b-row>
                            <b-col>
                                <b-button variant="primary" @click="search">{{ $t('Search') }}</b-button>
                            </b-col>
                        </b-row>
                    </b-card>
                </b-collapse>
            </b-col>
        </b-row>
    </b-card>
    <!-- eslint-enable -->
</template>

<script>
import Multiselect from 'vue-multiselect';
import HotelService from '../../../api/hotels-service';
import ReservationService from '../../../api/reservation-service';

export default {
    name: 'advance-filter',
    components: {
        Multiselect
    },
    mounted() {
        this.getHotels();
        this.getAgencies();
        this.getAgents();
        this.$root.$on('queryString', array => {
            this.filterQueryString = array[0];
            this.formatQueryString = array[1];
            this.perPageQueryString = array[2];
            this.currentPageQueryString = array[3];
        });
    },
    created() {
        this.dates = this.$parent.defaultDates();
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
        },
        corporates: {
            required: true,
            type: Array
        }
    },
    computed: {
        minDate() {
            const date = new Date();
            date.setFullYear(date.getFullYear() - 1);
            return date;
        }
    },
    data() {
        return {
            filterQueryString: null,
            formatQueryString: null,
            perPageQueryString: null,
            currentPageQueryString: null,
            includeDates: false,
            dates: null,
            noReservation: '',
            checkStatus: ['1'],
            typeDate: 'ReservationDate',
            clientName: '',
            source: 'ALL',
            hotel: [],
            hotels: [],
            paymentMethod:[],
            paymentMethods:[
                {paymentMethodId : 'Deposito', paymentMethod: this.$t('Deposit')}, 
                {paymentMethodId : 'Hotel', paymentMethod: 'Hotel'},
                {paymentMethodId : 'Banamex', paymentMethod: 'Banamex'},
                {paymentMethodId : 'Santander', paymentMethod: 'Santander'},
                {paymentMethodId : 'DineroMail', paymentMethod: 'DineroMail'},
                {paymentMethodId : 'Bancomer', paymentMethod: 'Bancomer'},
                {paymentMethodId : 'Banorte', paymentMethod: 'Banorte'},
                {paymentMethodId : 'AzubaPay', paymentMethod: 'AzubaPay'},
                {paymentMethodId : 'Bidaiondo', paymentMethod: 'Bidaiondo'},
                {paymentMethodId : 'American Express', paymentMethod: 'American Express'},
                {paymentMethodId : 'Paypal', paymentMethod: 'Paypal'},
                {paymentMethodId : 'PayU', paymentMethod: 'PayU'},
                {paymentMethodId : 'Conekta / OXXO', paymentMethod: 'Conekta / OXXO'},
                {paymentMethodId : 'Amex', paymentMethod: 'Amex'},
                {paymentMethodId : 'Banregio', paymentMethod: 'Banregio'}
            ],
            ota: 'ALL',
            agency: -1,
            agencies: [],
            agent: -1,
            agents:[],
            agentsToFilter:[],
            isAgencyCompany:(this.$appConfig.session.isAgencyCompany === 'True')? true : false,
            isSupervisor:(this.$appConfig.session.isSupervisor === 'True')? true : false,
            typeDates: [
                { text: this.$t('Reservation date'), value: 'ReservationDate' },
                { text: this.$t('Arrival date'), value: 'CheckIn' },
                { text: this.$t('Departure date'), value: 'CheckOut' }
            ],
            sources: [
                { text: `-- ${this.$t('All')} --`, value: 'ALL' },
                { text: 'Portal', value: 'POR' },
                { text: 'Call Center', value: 'CCT' },
                { text: this.$t('One Page'), value: 'UNI' },
                { text: this.$t('Front Desk'), value: 'HTL' },
                { text: 'GDS', value: 'WIZ' },
                { text: 'ADS', value: 'ADS' },
                { text: 'OTAS', value: 'IDS' },
                { text: this.$t('Agency'), value: 'AGENCY'}
            ],
            otas: [
                { text: `-- ${this.$t('All')} --`, value: 'ALL' },
                { text: 'BestDay', value: 'BestDay' },
                { text: 'Booking.com', value: 'Booking' },
                { text: 'Expedia', value: 'Expedia' },
                { text: 'Hotel Beds', value: 'Hotel Beds' },
                { text: 'PriceTravel', value: 'PriceTravel' }
            ],
            corporate: [],
            //muestra hoteles o corporativos en la busqueda avazanda
            isCheckCorporate: false
        };
    },
    watch:{
        agency:function(value){
            this.agents = [];
            this.agent = -1;
            // Si es != -1 Filtra los agentes por agencia
            if(value != -1)
            {
                this.agents.push({
                    text: this.$t('All'),
                    value:-1
                });

                this.agentsToFilter.forEach(agentFilter => {
                    if(agentFilter.agencyId === value){
                        this.agents.push({
                            text: `${agentFilter.name} ${agentFilter.lastName} - ${agentFilter.email}`,
                            value: agentFilter.userId
                        })
                    }  
                });

                this.agent = this.agents[0].value;
            }
        },
    },
    methods: {
        getHotels() {
            HotelService.getList().then(response => {
                this.hotels = response.body;
                console.log(this.hotels);
                console.log(response.body);
            });
        },
        getAgencies() {
            ReservationService.GetAgencies().then(response => {
                this.agencies.push({
                    text: this.$t('All'),
                    value: -1
                });

                response.body.forEach(x => {
                    this.agencies.push({
                        text: x.nombre,
                        value: x.idAgencia
                    })
                });
                if (this.agencies.length > 0) {
                    this.agency = this.agencies[0].value;
                }
            });
        },
        getAgents()
        {
            ReservationService.GetAgents().then(response =>{
                this.agentsToFilter = response.body;
            });

        },
        search() {
            this.$emit('search', this.getFilter());
        },
        exportToExcel() {
            ReservationService.GetExcel(
                this.filterQueryString,
                this.formatQueryString,
                this.perPageQueryString,
                this.currentPageQueryString
            )
            .then(res => {
                console.log(res);
                document.location.href = res.url;
            })
            .catch(err => {
                this.$appAlert({
                    type: 'error',
                    title: this.$t('Can not export page'),
                    confirmButtonText: this.$t('Exit'),
                    confirmButtonColor: '#d33'
                });
            });
        },
        getFilter() {
            let filter = '';

            /* filtrar #reservacion (tiene prioridad) */
            if (this.noReservation !== '') {
                filter = `confirmNumber eq ${this.noReservation}`;
                this.cleanFilters();
                return filter;
            }

            /* filtrar demas campos si estan disponibles */
            /* eslint-disable max-len */

            if (this.dates != null){
                let dateEnd = new Date(this.dates.end);
                dateEnd.setDate(dateEnd.getDate() + 1);
                filter = `${this.typeDate} ge ${this.dateFormat(this.dates.start)} and ${this.typeDate} lt ${this.dateFormat(dateEnd)}`;
                }
            if (this.checkStatus.length > 0) {
                filter += this.and(filter);
                this.checkStatus.forEach((value, index) => {
                    if (index > 0) filter += ' or ';
                    filter += `Status eq ${value}`;
                });
            }
            if (this.clientName !== '')
                filter += `${this.and(filter)}Client lk ${this.clientName}`;
            if (this.source !== 'ALL')
            {
                let _source = this.source;
                if (this.source === 'AGENCY') _source = 'POR';
                filter += `${this.and(filter)}Source eq ${_source}`;
            }
            if (this.source === 'IDS' && this.ota !== 'ALL')
                filter += `${this.and(filter)}Portal eq ${this.ota}`;
            if (this.hotel.length > 0 && !this.isCheckCorporate) {
                filter += this.and(filter);
                this.hotel.forEach((value, index) => {
                    if (index > 0) filter += ' or ';
                    filter += `hotelId eq ${value.id}`;
                });
            }
            if (this.corporate.length > 0 && this.isCheckCorporate) {
                filter += this.and(filter);
                this.corporate.forEach((value, index) => {
                    if (index > 0) filter += ' or ';
                    filter += `CorporateId eq ${value.idCorporativo}`;
                });
            }

            if(this.paymentMethod.length > 0) {
                filter += this.and(filter);
                this.paymentMethod.forEach((value,index) => {
                    if (index > 0) filter += ' or ';
                    filter += `PaymentMethod lk ${value.paymentMethodId}`;
                });
            }


            if(this.source === 'AGENCY')
            {
                // Si se escogio una agencia
                if (this.agency !== -1) {
                   
                    //Si se escogio un agente
                    if(this.agent !== -1)
                    {
                        filter += `${this.and(filter)}AgencyId eq ${this.agency} and AgencyUserId eq ${this.agent}`;
                    }
                    // No se escogio agente
                    else{

                        filter += `${this.and(filter)}AgencyId eq ${this.agency}`;
                    }
                }// No se escogio agencia 
                else {
                    filter += `${this.and(filter)}AgencyId gt 0`;
                }
            }

            // if (this.agency !== -1) {
            //     filter += `${this.and(filter)}AgencyId eq ${this.agency}`;
            // }

            //filter += `${this.and(filter)}AgencyId gt 0`;

            /* eslint-enable max-len */
            return filter === '' ? null : filter;
        },
        and(filter) {
            return filter !== '' ? ' and ' : '';
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
            this.corporate = [];
            this.paymentMethod = [];
        },
        changeItemsPerPage() {
            this.$emit('changeItems', this.itemPerPage);
        },
        nameWithCorporate({name, corp}) {
            
            let noDots = (corp.includes(":"))? corp.split(":")[1]: corp;

            return name + (corp !== '' ? ` - [${noDots}]` : '');
        }
    }
};
</script>
