<template>
<!-- eslint-disable -->
    <div>
        <v-popover v-if="dayRate.price > 0"  placement="right" @show="getDailyRates" :auto-hide="false">
            <a href="javascript:;" class="tooltip-target text-decoration-none">{{ getPrice(dayRate, rate) | currency}}</a>
            <template slot="popover">
                <div class="d-flex flex-row-reverse justify-content-between">
                    <a ref="close" v-close-popover href="javascript:;" class="text-primary"><i class="fa fa-times"></i></a>
                    <h4 class="text-primary mb-2"><small>{{this.room.name}} / {{this.rate.ratePlan}} / {{this.dayRate.date | moment('MMM D, YYYY')}}</small></h4>
                </div>
                <div v-if="rateDayDetails" :id="'popover-' + idString">
                    <ul class="nav nav-tabs nav-justified" id="priceTabs">
                        <li class="nav-item">
                            <a class="nav-link active border text-dark">{{'prices' | translate}}</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark invisible" data-toggle="tab" href="#menu2"></a>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane active">
                            <div class="form-check d-flex pl-0">
                                <div class="price-rates">
                                    <div class="p-2 text-center text-primary">
                                        <label class="m-0">{{'adults' | translate}}</label>
                                    </div>
                                    <div v-show="!occupancyPrices">
                                        <div class="d-flex justify-content-between bg-blue-created border p-1">
                                            <label class="w-15">{{'base' | translate}}</label>
                                            <input v-model.number="prices.byRoom.adult" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                            <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                        </div>
                                    </div>
                                    <div v-show="occupancyPrices">
                                            <div v-for="(p, idx) in prices.byOccupancy.adult" :key="'ra' + idx" class="d-flex justify-content-between bg-blue-created border p-1">
                                            <label class="w-15 text-center">{{p.occupation}}</label>
                                            <input v-model.number="p.price" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                            <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                        </div>
                                    </div>
                                    <div v-show="room.extraOccupancyAllowed > 0">
                                        <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                <label class="w-15">{{'extra' | translate}}</label>
                                                <input v-model.number="prices.extra.adult" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                            </div>
                                    </div>
                                </div>
                                <div class="price-rates" v-if="room.maxChildrenOccupancy > 0">
                                    <div class="p-2 text-center text-primary">
                                        <label class="m-0">{{'children' | translate}}</label>
                                    </div>
                                    <div v-show="!occupancyPrices">
                                        <div class="d-flex justify-content-between bg-blue-created border p-1">
                                            <label class="w-15">{{'base' | translate}}</label>
                                            <input v-model.number="prices.byRoom.child" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                            <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                        </div>
                                    </div>
                                    <div v-show="occupancyPrices">
                                        <div v-for="(p, idx) in prices.byOccupancy.child" :key="'rc' + idx" class="d-flex justify-content-between bg-blue-created border p-1">
                                            <label class="w-15 text-center">{{p.occupation}}</label>
                                            <input v-model.number="p.price" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                            <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                        </div>
                                    </div>
                                    <div v-show="room.extraOccupancyAllowed > 0">
                                        <div class="d-flex justify-content-between dark-gray-created border p-1">
                                            <label class="w-15">{{'extra' | translate}}</label>
                                            <input v-model.number="prices.extra.child" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                            <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="junior-rates" v-if="room.maxChildrenOccupancy > 0 && room.juniorsAllowed">
                                    <div class="p-2 text-center text-primary">
                                        <label class="m-0">{{'juniors' | translate}}</label>
                                    </div>
                                    <div v-show="!occupancyPrices">
                                        <div class="d-flex justify-content-between bg-blue-created border p-1">
                                            <label class="w-15">{{'base' | translate}}</label>
                                            <input v-model.number="prices.byRoom.junior" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                            <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                        </div>
                                    </div>
                                    <div v-show="occupancyPrices">
                                        <div v-for="(p, idx) in prices.byOccupancy.junior" :key="'rj' + idx"  class="d-flex justify-content-between bg-blue-created border p-1">
                                            <label class="w-15">{{p.occupation}}</label>
                                            <input v-model.number="p.price" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                            <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                        </div>
                                    </div>
                                    <div v-show="room.extraOccupancyAllowed > 0">
                                        <div class="d-flex justify-content-between dark-gray-created border p-1">
                                            <label class="w-15">{{'extra' | translate}}</label>
                                            <input v-model.number="prices.extra.junior" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                            <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="menu2" class="container tab-pane fade"><br></div>
                    </div>
                    <div class="gds-container border-top">
                        <div class="pt-3">
                            <button type="button" @click="verifyRequest" class="btn btn-success">{{'save' | translate}}</button>
                        </div>
                    </div>
                </div>
            </template>
        </v-popover>
        <a href="javascript:;" v-else class="text-danger tooltip-target text-decoration-none"> N/A </a>
    </div>


<!-- eslint-enable -->
</template>
<script>
import EventBus from '../../../core/event-bus';
import ratesService from '../../../api/rates-service';
import RQHelper from '../helpers/rateUpdateHelper';

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
        room: {
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
            error: null,
            loader: null,
            rateDayDetails: null,
            occupancyPrices: false,
            prices: {
                byRoom: {
                    adult: 0,
                    child: 0,
                    junior: 0,
                },
                byOccupancy: {
                    adult: [],
                    child: [],
                    junior: [],
                },
                extra: {
                    adult: 0,
                    child: 0,
                    junior: 0,
                },
            },
        };
    },
    computed: {
        dayFormatted() {
            return this.$moment(this.dayRate.date).format('YYYY-MM-DD');
        },
        idString() {
            return this.rateDayDetails ? `${this.rateDayDetails.rateId}-${this.rateDayDetails.date.split('T')[0]}` : '';
        },
    },
    methods: {
        verifyRequest() {
            const rqHelper = new RQHelper(
                this.room,
                { code: this.rate.ratePlanId },
                { start: this.dayFormatted, end: this.dayFormatted },
                null,
                this.occupancyPrices,
                this.prices,
                false,
                null,
            );
            // validación;
            rqHelper.validate();

            let html = '';
            if (rqHelper.errors.length > 0) {
                for (let i = 0; i < rqHelper.errors.length; i += 1) {
                    html += `<div class="alert alert-danger mt-1 mb-1" role="alert">
                                <i class="fa fa-times-circle"></i> <small>${this.$t(rqHelper.errors[i])}</small>
                            </div>`;
                }

                // mostrar alerta con errores
                this.$swal({
                    type: 'error',
                    html,
                    position: 'top',
                    target: `#popover-${this.idString}`,
                });

                return;
            }

            if (rqHelper.warnings.length > 0) {
                for (let i = 0; i < rqHelper.warnings.length; i += 1) {
                    html += `<div class="alert alert-info mt-1 mb-1" role="alert">
                                <i class="fa fa-exclamation-triangle"></i> <small>${this.$t(rqHelper.warnings[i])}</small>
                            </div>`;
                }
            }


            // mostrar alerca con advertencias y si lo quiere continuar
            this.$swal({
                type: 'info',
                title: this.$t('are you sure?'),
                position: 'top',
                html,
                showCancelButton: true,
                confirmButtonText: this.$t('yes, save it!'),
                cancelButtonText: this.$t('cancel'),
                target: `#popover-${this.idString}`,
            }).then((result) => {
                // si acepa enviar request
                if (result.value) this.sendRequest(rqHelper.createRQ());
            });
        },
        sendRequest(RQ) {
            ratesService.dayUpdate(
                this.$appConfig.session.hotelId,
                this.rateDayDetails.rateId,
                this.dayFormatted,
                RQ
                )
                .then(() => {
                    this.$swal({
                        type: 'success',
                        position: 'top',
                        title: this.$t('successful update'),
                        target: `#popover-${this.idString}`,
                    }).then(() => {
                        this.$refs.close.click();
                        EventBus.$emit('dayUpdate');
                    });
                }).catch(() => {
                    this.$swal({
                        type: 'error',
                        position: 'top',
                        title: this.$t('invalid request, please contact support'),
                        target: `#popover-${this.idString}`,
                    });
                });
        },
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
        getDailyRates() {
            if (this.dayRate.rateId) {
                ratesService.getByDay(
                    this.$appConfig.session.hotelId,
                    this.dayRate.rateId,
                    this.dayFormatted,
                ).then((response) => {
                    if (response.body.rateId) {
                        this.parseDailyRates(response.body);
                    } else {
                        this.error = this.$t('invalid request, please contact support');
                    }
                }).catch(() => {
                    this.error = this.$t('invalid request, please contact support');
                });
            }
        },
        parseDailyRates(rateDetails) {
            const adultPrices = rateDetails.prices.filter(x => x.type === 1);
            const childPrices = rateDetails.prices.filter(x => x.type === 2);
            const juniorPrices = rateDetails.prices.filter(x => x.type === 3);

            this.occupancyPrices = adultPrices.length > 0 && !adultPrices.every(r => r.price === adultPrices[0].price);

            if (!this.occupancyPrices && this.room.maxChildrenOccupancy > 0) {
                this.occupancyPrices = childPrices.length > 0
                    && !childPrices.every(r => r.price === childPrices[0].price);
            }

            if (!this.occupancyPrices && this.room.maxChildrenOccupancy > 0 && this.room.juniorsAllowed) {
                this.occupancyPrices = juniorPrices.length > 0
                    && !juniorPrices.every(r => r.price === juniorPrices[0].price);
            }

            const sortOccupation = (a, b) => a.occupation - b.occupation;

            this.prices.byOccupancy.adult = adultPrices.sort(sortOccupation);
            this.prices.byOccupancy.child = childPrices.sort(sortOccupation);
            this.prices.byOccupancy.junior = juniorPrices.sort(sortOccupation);

            this.prices.byRoom.adult = adultPrices.length > 0 ? adultPrices[0].price : 0;
            this.prices.byRoom.child = childPrices.length > 0 ? childPrices[0].price : 0;
            this.prices.byRoom.junior = juniorPrices.length > 0 ? juniorPrices[0].price : 0;

            if (this.room.extraOccupancyAllowed > 0) {
                this.prices.extra.adult = rateDetails.extras.filter(x => x.type === 1)[0].price;

                if (this.room.maxChildrenOccupancy > 0) {
                    this.prices.extra.child = rateDetails.extras.filter(x => x.type === 2)[0].price;
                }

                if (this.room.maxChildrenOccupancy > 0 && this.room.juniorsAllowed) {
                    this.prices.extra.junior = rateDetails.extras.filter(x => x.type === 3)[0].price;
                }
            }

            this.rateDayDetails = rateDetails;
        },
    },
};
</script>
