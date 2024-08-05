<template>
    <!-- eslint-disable -->
    <b-card no-body style="border:0px">
        <!--
        <b-modal id="modal-comissions" title="$t('Channel Comissions')">
            <template>
                <div>
                    <b-table striped hover :items="hotelChannels" :fields="hotelChannelfields"></b-table>
                </div>
            </template>
</b-modal>
-->
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
                        <b-button class="float-right" v-b-toggle.filter_content variant="link">{{ $t("Advanced Search")
                            }}</b-button>
                    </b-col>
                </b-row>
            </b-col>
            <b-col md="3" class="my-1">
                <b-row>
                    <b-col md="9">
                        <b-button class="float-right" v-b-toggle.config_channel variant="link">{{ $t("Channel
                            Comissions")
                            }}</b-button>
                    </b-col>
                </b-row>
            </b-col>
            <b-col md="3" class="my-1">
                <b-row>
                    <b-col md="8">
                        <b-button v-if="result.length > 0" @click="exportToExcel"
                            style="background-color:green;float:right" variant="primary">
                            <i class="fas fa-file-excel"></i> &nbsp;{{ $t("Export page") }}
                        </b-button>
                    </b-col>
                    <b-col md="4" style="margin-top:-1.9rem;">
                        <b-form-group :label="$t('items per page')">
                            <b-form-select @change="changeItemsPerPage" class="float-right" v-model.number="itemPerPage"
                                :options="itemsPerPage">
                            </b-form-select>
                        </b-form-group>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
        <b-row>
            <b-col md="12">
                <b-collapse id="config_channel">
                    <b-card class="mt-3 bg-light">
                        <b-row>
                            <b-col md="3">
                                <b-form-group :label="$t('Channel')">
                                    <b-form-select v-model="hotelChannel"
                                        :options="hotelChannelOptions"></b-form-select>
                                </b-form-group>
                            </b-col>
                            <b-col md="3">
                                <b-form-group :label="$t('Comission')">
                                    <b-form-input v-model="channelComission" v-validate="required | numeric"
                                        :placeholder="$t('Comission')" />
                                </b-form-group>
                            </b-col>
                            <b-col md="3">
                                <b-form-group>
                                    <b-button v-if="channelComission > 0" variant="primary"
                                        @click="SaveChannelComission">{{
                                            $t("Save")
                                        }}</b-button>
                                </b-form-group>
                            </b-col>
                            <b-col md="3">
                                <b-form-group>
                                    <b-button id="btn_modal_comissions" v-if="hotelChannelOptions.length > 0"
                                        v-on:click="showComissionsModal" variant="primary">{{ $t("Show Configured
                                        Comissions")
                                        }}</b-button>
                                </b-form-group>
                            </b-col>
                        </b-row>
                    </b-card>
                </b-collapse>
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

                                        <v-date-picker v-model="dates" class="form-control p-0" mode="range"
                                            :min-date="minDate" :popover="{ placement: 'bottom', visibility: 'click' }"
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
                                        <b-form-checkbox value="4" v-if="isSupervisor">{{ $t('In process')
                                            }}</b-form-checkbox>
                                        <b-form-checkbox value="3">{{ $t('Cancelled') }}</b-form-checkbox>
                                    </b-form-checkbox-group>
                                </b-form-group>
                            </b-col>
                        </b-row>
                        <b-row>
                            <!--
                            <b-col md="4">
                                <b-form-group :label="$t('Customer Name')">
                                    <b-form-input v-model="clientName"></b-form-input>
                                </b-form-group>
                            </b-col>-->
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
                            <b-col md="4" v-if="isSupervisor">
                                <b-form-group v-if="hotels.length > 0">
                                    <template slot="label">
                                        <div class="d-flex">
                                            <span class="mr-2">{{ $t('Hotels') }}</span>
                                            <b-form-checkbox v-if="corporates.length > 0" v-model="isCheckCorporate"
                                                switch>
                                                <span>{{ $t('Corporate') }}</span>
                                            </b-form-checkbox>
                                        </div>
                                    </template>

                                    <!--hoteles-->
                                    <multiselect v-if="!isCheckCorporate" v-model="hotel"
                                        :custom-label="nameWithCorporate" :options="hotels" track-by="id"
                                        :multiple="true" :selectLabel="$t('select')" :selectedLabel="''"
                                        :deselectLabel="''" :placeholder="$t('Search Hotel')">
                                    </multiselect>

                                    <!--corporativos-->
                                    <multiselect v-if="isCheckCorporate" label="nombreCorp" v-model="corporate"
                                        :options="corporates" track-by="idCorporativo" :multiple="true"
                                        :selectLabel="$t('select')" :selectedLabel="''" :deselectLabel="''"
                                        :placeholder="$t('Search Corporate')">
                                    </multiselect>
                                </b-form-group>
                            </b-col>
                            <b-col md="4">
                                <b-form-group>
                                    <template slot="label">
                                        <div class="d-flex">
                                            <span class="mr-2">{{ $t('Channels') }}</span>
                                        </div>
                                    </template>
                                    <multiselect track-by="text" label="text" v-model="channels" :options="channelOpts"
                                        :multiple="true" :selectLabel="$t('select')" :selectedLabel="''"
                                        :deselectLabel="''" :placeholder="$t('')">
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
                                    <multiselect track-by="paymentMethodId" label="paymentMethod"
                                        v-model="paymentMethod" :options="paymentMethods" :multiple="true"
                                        :selectLabel="$t('select')" :selectedLabel="''" :deselectLabel="''"
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
    name: 'advance-report-filter',
    components: {
        Multiselect
    },
    mounted() {
        this.getHotels();
        this.getAgencies();
        this.getChannels();
        this.getHotelChannels();
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
            hotelChannel: '',
            hotelChannels: [],
            hotelChannelOptions: [],
            channelComission: 0,
            channels: [],
            hotelChannelfields: [
                {
                    key: 'idCanal',
                    label: "#",
                    sortable: false
                },
                {
                    key: 'nombre',
                    label: this.$t('Name'),
                    sortable: false
                },
                {
                    key: 'comision',
                    label: this.$t('Comission'),
                    sortable: true
                }
            ],
            typeDate: 'ReservationDate',
            clientName: '',
            source: 'ALL',
            hotel: [],
            hotels: [],
            paymentMethod: [],
            paymentMethods: [
                { paymentMethodId: 'Deposito', paymentMethod: this.$t('Deposit') },
                { paymentMethodId: 'Hotel', paymentMethod: 'Hotel' },
                { paymentMethodId: 'Banamex', paymentMethod: 'Banamex' },
                { paymentMethodId: 'Santander', paymentMethod: 'Santander' },
                { paymentMethodId: 'DineroMail', paymentMethod: 'DineroMail' },
                { paymentMethodId: 'Bancomer', paymentMethod: 'Bancomer' },
                { paymentMethodId: 'Banorte', paymentMethod: 'Banorte' },
                { paymentMethodId: 'AzubaPay', paymentMethod: 'AzubaPay' },
                { paymentMethodId: 'Bidaiondo', paymentMethod: 'Bidaiondo' },
                { paymentMethodId: 'American Express', paymentMethod: 'American Express' },
                { paymentMethodId: 'Paypal', paymentMethod: 'Paypal' },
                { paymentMethodId: 'PayU', paymentMethod: 'PayU' },
                { paymentMethodId: 'Conekta / OXXO', paymentMethod: 'Conekta / OXXO' },
                { paymentMethodId: 'Amex', paymentMethod: 'Amex' },
                { paymentMethodId: 'Banregio', paymentMethod: 'Banregio' }
            ],
            ota: 'ALL',
            agency: -1,
            agencies: [],
            agent: -1,
            agents: [],
            agentsToFilter: [],
            isAgencyCompany: (this.$appConfig.session.isAgencyCompany === 'True') ? true : false,
            isSupervisor: (this.$appConfig.session.isSupervisor === 'True') ? true : false,
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
                { text: this.$t('Agency'), value: 'AGENCY' }
            ],
            channelOpts: [],
            hotelChannelOpts: [],
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
    watch: {
        agency: function (value) {
            this.agents = [];
            this.agent = -1;
            // Si es != -1 Filtra los agentes por agencia
            if (value != -1) {
                this.agents.push({
                    text: this.$t('All'),
                    value: -1
                });

                this.agentsToFilter.forEach(agentFilter => {
                    if (agentFilter.agencyId === value) {
                        this.agents.push({
                            text: `${agentFilter.name} ${agentFilter.lastName} - ${agentFilter.email}`,
                            value: agentFilter.userId
                        })
                    }
                });

                this.agent = this.agents[0].value;
            }
        },
        hotelChannel: function () {
            //channelComission
            console.log('Canal Seleccionado: ' + this.hotelChannel);

            for (var x = 0; x < this.hotelChannels.length; x++) {
                console.log('Comparando Canal: ' + this.hotelChannels[x].nombre + ' - Id: ' + this.hotelChannels[x].idCanal);
                if (this.hotelChannels[x].idCanal == this.hotelChannel) {
                    this.channelComission = this.hotelChannels[x].comision;

                    console.log(this.hotelChannels[x].nombre + ' ' + this.hotelChannel);
                    break;
                    //return this.channelComission;
                } else {
                    this.channelComission = 0;
                }
            }
        }
    },
    methods: {
        getHotels() {
            HotelService.getList().then(response => {
                this.hotels = response.body;
                /*console.log(this.hotels);
                console.log(response.body);*/
            });
        },
        getHotelChannels() {
            ReservationService.GetHotelChannels(this.$appConfig.session.hotelId).then(response => {
                this.hotelChannels = response.body;

                console.log('Canales configurados: ');
                console.log(this.hotelChannels);
            });
        },
        SaveChannelComission() {

            if (this.hotelChannel > 0 && this.channelComission > 0 && this.channelComission <= 100) {
                let newHotelChannel = {
                    id: 0,
                    idHotel: this.$appConfig.session.hotelId,
                    idCanal: this.hotelChannel,
                    Comision: this.channelComission
                };

                ReservationService.SaveChannelComission(newHotelChannel).then(response => {
                    //console.log(response);

                    switch (response.body) {
                        case "SUCCESS_UPDATED":
                            this.$appAlert({
                                type: 'success',
                                title: this.$t('Register updated successfully'),
                                showCancelButton: true,
                                showConfirmButton: false,
                                cancelButtonText: this.$t("Close"),
                                cancelButtonColor: "#d33",
                                showConfirmButton: false,
                                onClose: () => {
                                    window.location.reload();
                                }
                            });
                            break;
                        case "SUCCESS_REGISTERED":
                            this.$appAlert({
                                type: 'success',
                                title: this.$t('Register saved successfully'),
                                showCancelButton: true,
                                showConfirmButton: false,
                                cancelButtonText: this.$t("Close"),
                                cancelButtonColor: "#d33",
                                showConfirmButton: false,
                                onClose: () => {
                                    window.location.reload();
                                }
                            });
                            break;
                        default:
                            this.$appAlert({
                                type: 'error',
                                title: this.$t('Request error'),
                                confirmButtonText: this.$t('Close'),
                                confirmButtonColor: '#d33'
                            });
                            break;
                    }
                });

                /*.catch(error => {

                        let errorCode = error.body.errors[0].details[0].key;
                        //this.errorResponse = this.ErrorMessage(errorCode);
                        console.log(errorCode)

                 });*/
            }
        },
        getChannels() {
            ReservationService.GetChannels().then(response => {

                console.log('Channels response');
                console.log(response.body);

                response.body.forEach(x => {
                    this.channelOpts.push({
                        text: x.nombre,
                        value: x.nombre
                    })
                });

                response.body.forEach(x => {
                    this.hotelChannelOptions.push({
                        text: x.nombre,
                        value: x.idCanal
                    })
                });
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
        getAgents() {
            ReservationService.GetAgents().then(response => {
                this.agentsToFilter = response.body;
            });

        },
        search() {
            this.$emit('search', this.getFilter());
        },
        exportToExcel() {
            ReservationService.GetNamedExcel(
                this.filterQueryString,
                this.formatQueryString,
                this.perPageQueryString,
                this.currentPageQueryString,
                "Comissions-Report"
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

            if (this.dates != null) {
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
            // Canales
            if (this.channels.length > 0) {
                filter += this.and(filter);
                this.channels.forEach((value, index) => {
                    if (index > 0) filter += ' or ';
                    filter += `portal lk ${value.text}`;
                });
            }

            if (this.clientName !== '')
                filter += `${this.and(filter)}Client lk ${this.clientName}`;
            if (this.source !== 'ALL') {
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

            if (this.paymentMethod.length > 0) {
                filter += this.and(filter);
                this.paymentMethod.forEach((value, index) => {
                    if (index > 0) filter += ' or ';
                    filter += `PaymentMethod lk ${value.paymentMethodId}`;
                });
            }

            if (this.source === 'AGENCY') {
                // Si se escogio una agencia
                if (this.agency !== -1) {

                    //Si se escogio un agente
                    if (this.agent !== -1) {
                        filter += `${this.and(filter)}AgencyId eq ${this.agency} and AgencyUserId eq ${this.agent}`;
                    }
                    // No se escogio agente
                    else {

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
            this.channn = [];
            this.paymentMethod = [];
            this.channels = [];
        },
        changeItemsPerPage() {
            this.$emit('changeItems', this.itemPerPage);
        },
        nameWithCorporate({ name, corp }) {

            let noDots = (corp.includes(":")) ? corp.split(":")[1] : corp;

            return name + (corp !== '' ? ` - [${noDots}]` : '');
        },
        showComissionsModal() {

            if (hotelChannels.length > 0) {
                var tcN = 0;
                var tableBody = "";

                this.hotelChannels.forEach(x => {
                    tcN++;
                    tableBody += "<tr><td>" + tcN + "</td>" + "<td>" + x.nombre + "</td><td>" + x.comision + "</td></tr>";
                });

                tableBody += "</table>"

                var tableChannels = "<table role='table' aria-busy='false' aria-colcount='15' class='table b-table table-striped table-hover table-bordered table-sm'><tr><th>#</th><th>Nombre</th><th>Comision</th>";
                tableChannels += tableBody;

                this.$appAlert({
                    type: '',
                    title: this.$t('Channel Comissions'),
                    html: tableChannels,
                    showCancelButton: true,
                    showConfirmButton: false,
                    cancelButtonText: this.$t("Close"),
                    cancelButtonColor: "#d33",
                    showConfirmButton: false
                });

            } else {
                this.$appAlert({
                    type: 'warning',
                    title: this.$t('Channel Comissions'),
                    text: this.$t('There are no commissions set up yet.'),
                    showCancelButton: true,
                    showConfirmButton: false,
                    cancelButtonText: this.$t("Close"),
                    cancelButtonColor: "#d33",
                    showConfirmButton: false
                });
            }

        }
        /*
        GetConfiguredComission() {
            //channelComission
            console.log('Canal Seleccionado: ' + this.hotelChannel);

            for (var x = 0; x < this.hotelChannels.length; x++) {
                if (this.hotelChannels[x].idcanal == this.hotelChannel) {
                    this.channelComission = this.hotelChannels[x].comision;
                    console.log(this.hotelChannels[x].nombre + ' ' + this.hotelChannel);
                    break;
                    //return this.channelComission;
                }else{
                    this.channelComission = 0;
                }
            }
        }
        */
    }
};
</script>
