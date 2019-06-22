<template>
<!-- eslint-disable -->
    <v-popover placement="right" @show="getDailyRates">
        <a href="javascript:;" class="tooltip-target text-decoration-none" v-if="dayRate.price > 0">{{ getPrice(dayRate, rate) | currency}}</a>
        <a href="javascript:;" v-else class="text-danger tooltip-target text-decoration-none"> N/A </a>
        <template slot="popover">
            <h2 v-if="rateDayDetails">{{dayRate.rateId}}</h2>
            <div v-if="rateDayDetails" class="container-fluid">
                <div class="row">
                    <div class="col">
                        <ul class="nav nav-tabs nav-justified" id="priceTabs">
                            <li class="nav-item">
                                <a class="nav-link active border text-dark" data-toggle="tab"
                                    href="#priceRates" id="priceRatesTab">{{'prices' | translate}}</a>
                            </li>
                            <li class="nav-item" v-show="occupancyPrices">
                                <a class="nav-link border text-dark" data-toggle="tab"
                                    href="#priceExceptions">{{'price exceptions' | translate}}</a>
                            </li>
                            <li class="nav-item" v-show="!occupancyPrices">
                                <a class="nav-link text-dark invisible" data-toggle="tab" href="#menu2"></a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link text-dark invisible" data-toggle="tab" href="#menu2"></a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div id="priceRates" class="tab-pane active">
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
                            <div id="priceExceptions" class="container tab-pane fade"><br>
                                <div class="btn-group btn-group-toggle btn-group-primary d-flex w-100">
                                    <label class="btn btn-secondary shadow-none" :class="{active: prices.exceptions.apply.sun}">
                                        <input type="checkbox" v-model="prices.exceptions.apply.sun"
                                        id="ex-sun" autocomplete="off"> {{'U' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: prices.exceptions.apply.mon}">
                                        <input type="checkbox" v-model="prices.exceptions.apply.mon"
                                        id="ex-mon" autocomplete="off"> {{'M' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: prices.exceptions.apply.tue}">
                                        <input type="checkbox" v-model="prices.exceptions.apply.tue"
                                        id="ex-tue" autocomplete="off"> {{'T' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: prices.exceptions.apply.wed}">
                                        <input type="checkbox" v-model="prices.exceptions.apply.wed"
                                        id="ex-wed" autocomplete="off"> {{'W' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: prices.exceptions.apply.thu}">
                                        <input type="checkbox" v-model="prices.exceptions.apply.thu"
                                        id="ex-thu" autocomplete="off"> {{'R' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: prices.exceptions.apply.fri}">
                                        <input type="checkbox" v-model="prices.exceptions.apply.fri"
                                        id="ex-fri" autocomplete="off"> {{'F' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: prices.exceptions.apply.sat}">
                                        <input type="checkbox" v-model="prices.exceptions.apply.sat"
                                        id="ex-sat" autocomplete="off"> {{'S' | translate}}
                                    </label>
                                </div>
                                <div class="form-check d-flex pl-0">
                                    <div class="price-rates">
                                        <div class="p-2 text-center text-primary">
                                            <label class="m-0">{{'adults' | translate}}</label>
                                        </div>
                                        <div v-show="occupancyPrices">
                                                <div v-for="(p, idx) in prices.exceptions.adult" :key="'rax' + idx" class="d-flex justify-content-between bg-blue-created border p-1">
                                                <label class="w-15 text-center">{{ p.occupation }}</label>
                                                <input v-model.number="p.price" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" >
                                                <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                            </div>
                                        </div>
                                        <div v-show="room.extraOccupancyAllowed > 0">
                                            <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                    <label class="w-15">{{'extra' | translate}}</label>
                                                    <input v-model.number="prices.extra.adult" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2">
                                                    <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                                </div>
                                        </div>
                                    </div>
                                    <div class="price-rates" v-if="room.maxChildrenOccupancy > 0">
                                        <div class="p-2 text-center text-primary">
                                            <label class="m-0">{{'children' | translate}}</label>
                                        </div>
                                        <div v-show="occupancyPrices">
                                            <div v-for="(p, idx) in prices.exceptions.child" :key="'rcx' + idx" class="d-flex justify-content-between bg-blue-created border p-1">
                                                <label class="w-15 text-center">{{ p.occupation }}</label>
                                                <input v-model.number="p.price" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2">
                                                <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                            </div>
                                        </div>
                                        <div v-show="room.extraOccupancyAllowed > 0">
                                            <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                <label class="w-15">{{'extra' | translate}}</label>
                                                <input v-model.number="prices.extra.child" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2">
                                                <label class="font-weight-bold text-primary w-15"><span>{{rateDayDetails.currency}}</span></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="junior-rates" v-if="room.maxChildrenOccupancy > 0 && room.juniorsAllowed">
                                        <div class="p-2 text-center text-primary">
                                            <label class="m-0">{{'juniors' | translate}}</label>
                                        </div>
                                        <div v-show="occupancyPrices">
                                            <div v-for="(p, idx) in prices.exceptions.junior" :key="'rjx' + idx"  class="d-flex justify-content-between bg-blue-created border p-1">
                                                <label class="w-15 text-center">{{ p.occupation }}</label>
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
                    </div>
                </div>
            </div>
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
                exceptions: {
                    apply: {
                        mon: false,
                        tue: false,
                        wed: false,
                        thu: false,
                        fri: false,
                        sat: false,
                        sun: false,
                    },
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
        getDailyRates() {
            RatesService.getByDay(
                this.$appConfig.session.hotelId,
                this.dayRate.rateId,
                this.dayFormatted,
            ).then((response) => {
                if(response.body.rateId){
                    parseDailyRates(response.body);
                }
                else {
                    this.error = this.$t('invalid request, please contact support');
                }
            }).catch(() => {
                this.error = this.$t('invalid request, please contact support');
            });
        },
        parseDailyRates(rateDetails) {
            const adultPrices = rateDetails.prices.filter((x) => x.type === 1);
            const childPrices = rateDetails.prices.filter((x) => x.type === 2);
            const juniorPrices = rateDetails.prices.filter((x) => x.type === 3);

            this.occupancyPrices = adultPrices.lenght > 0  && !adultPrices.every((r) => r.price === adultPrices[0].price);

            if(this.room.maxChildrenOccupancy > 0) {
                this.occupancyPrices = childPrices.lenght > 0  && !childPrices.every((r) => r.price === childPrices[0].price);
            }

            if(this.room.maxChildrenOccupancy > 0 && this.room.juniosAllowed) {
                this.occupancyPrices = juniorPrices.lenght > 0  && !juniorPrices.every((r) => r.price === juniorPrices[0].price);
            }

            let sortOccupation = (a, b) => a.occupation - b.occupation;

            this.prices.byOccupancy.adult = adultPrices.sort(sortOccupation);
            this.prices.byOccupancy.child = childPrices.sort(sortOccupation);
            this.prices.byOccupancy.junior = juniorPrices.sort(sortOccupation);

            this.prices.byRoom.adult = adultPrices.lenght > 0 ? adultPrices[0].price : 0;
            this.prices.byRoom.child = childPrices.lenght > 0 ? childPrices[0].price : 0;
            this.prices.byRoom.junior = juniorPrices.lenght > 0 ? juniorPrices[0].price : 0;
        },
    },
};
</script>
