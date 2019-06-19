<template>
<!-- eslint-disable -->
    <div id="rates" class="collapse pl-3 pr-3 pt-3">
        <div class="formulario-page overflow-auto pb-5">
            <div class="bg-light">
                <div class="rate-plan d-flex justify-content-between border-top mt-2 pl-3 pt-3 pr-3">
                    <div class="w-25 pr-3">
                        <label>{{'room' | translate}}:</label>
                        <div class="form-group">
                            <select v-model="room" class="form-control text-dark" id="room" name="room">
                                <option v-for="room in hotel.rooms" :value="room" :key="room.id">{{room.code}} - {{room.name}}</option>
                            </select>
                        </div>
                    </div>
                    <div class="w-25 pr-3">
                        <label>{{'rate plan' | translate}}:</label>
                        <div class="form-group">
                            <select v-model="ratePlan" class="form-control text-dark" id="ratePlan" name="rateplan">
                                <option v-for="plan in hotel.ratePlans" :value="plan" :key="plan.code">{{plan.code}} - {{plan.name}}</option>
                            </select>
                        </div>
                    </div>
                    <div class="w-25 pr-3">
                        <label>{{ 'dates' | translate }}:</label>
                        <v-date-picker
                        mode="range"
                        class="w-100"
                        title-position="left"
                        v-model="dateRange"
                        :popover="{ placement: 'bottom', visibility: 'click' }"
                        :min-date="new Date()"
                        :is-required="true"
                        :locale="$appConfig.language"
                        :columns="2"
                        :masks="{input: 'DD/MMM/YYYY'}"
                        :input-props='{
                            class: "border rounded-left p-1 form-control",
                            readonly: true
                        }'>
                        </v-date-picker>
                    </div>
                    <div class="w-25 pr-3">
                        <label>{{'show rates by' | translate}}:</label>
                        <div class="custom-control custom-switch">
                            <label class="mr-5"> {{'room' | translate}}</label>
                            <input type="checkbox" v-model="occupancyPrices" class="custom-control-input" id="showOccupancy" name="showOccupancy">
                            <label class="custom-control-label label-style" for="showOccupancy">{{'occupancy' | translate}}</label>
                        </div>
                    </div>
                </div>
                <div class="border heading-divider dark-gray-created pl-3 pr-3 pt-2 pb-2">
                    <p class="font-weight-bold mb-0">{{'room prices - tax not included' | translate}}</p>
                </div>
                <div class="d-flex pt-3 pb-3">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-sm-7">
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
                                                        <input v-model.number="prices.byRoom.adult" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="occupancyPrices">
                                                     <div v-for="(p, idx) in prices.byOccupancy.adult" :key="'ra' + idx" class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-15 text-center">{{p.occupation}}</label>
                                                        <input v-model.number="p.price" type="number"  step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                            <label class="w-15">{{'extra' | translate}}</label>
                                                            <input v-model.number="prices.extra.adult" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                            <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
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
                                                        <input v-model.number="prices.byRoom.child" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="occupancyPrices">
                                                    <div v-for="(p, idx) in prices.byOccupancy.child" :key="'rc' + idx" class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-15 text-center">{{p.occupation}}</label>
                                                        <input v-model.number="p.price" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                        <label class="w-15">{{'extra' | translate}}</label>
                                                        <input v-model.number="prices.extra.child" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
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
                                                        <input v-model.number="prices.byRoom.junior" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="occupancyPrices">
                                                    <div v-for="(p, idx) in prices.byOccupancy.junior" :key="'rj' + idx"  class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-15">{{p.occupation}}</label>
                                                        <input v-model.number="p.price" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                        <label class="w-15">{{'extra' | translate}}</label>
                                                        <input v-model.number="prices.extra.junior" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
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
                                        <div class="form-check d-flex justify-content-between  pl-0">
                                            <div class="price-rates">
                                                <div class="p-2 text-center text-primary">
                                                    <label class="m-0">{{'adults' | translate}}</label>
                                                </div>
                                                <div v-show="occupancyPrices">
                                                     <div v-for="(p, idx) in prices.exceptions.adult" :key="'rax' + idx" class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-15 text-center">{{ p.occupation }}</label>
                                                        <input v-model.number="p.price" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2" >
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                            <label class="w-15">{{'extra' | translate}}</label>
                                                            <input v-model.number="prices.extra.adult" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2">
                                                            <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
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
                                                        <input v-model.number="p.price" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                        <label class="w-15">{{'extra' | translate}}</label>
                                                        <input v-model.number="prices.extra.child" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
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
                                                        <input v-model.number="p.price" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                        <label class="w-15">{{'extra' | translate}}</label>
                                                        <input v-model.number="prices.extra.junior" type="number" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="menu2" class="container tab-pane fade"><br></div>
                                </div>
                            </div>
                            <div class="col-sm">
                                <div class="d-flex justify-content-start">
                                    <p>{{'promotion' | translate}}:</p>
                                    <input v-model.number="promotion.discount" type="number" step="any" class="form-control w-25 ml-3 mr-3">
                                    <p>{{'% off' | translate}}</p>
                                </div>
                                <div class="mt-1">
                                    <p>{{'promotion description' | translate}}:</p>
                                    <div class="btn-group btn-group-toggle btn-group-primary d-flex w-100">
                                        <label class="btn btn-secondary shadow-none w-50" :class="{active: promotion.selectionLanguage == 'es'}">
                                            <input type="radio" v-model="promotion.selectionLanguage" name="options" id="option1" autocomplete="off" value="es" checked >
                                            {{'spanish' | translate}}
                                        </label>
                                        <label class="btn btn-secondary shadow-none w-50" :class="{active: promotion.selectionLanguage == 'en'}">
                                            <input type="radio" v-model="promotion.selectionLanguage" name="options" id="option2" autocomplete="off" value="en">
                                            {{'english' | translate}}
                                        </label>
                                    </div>
                                    <input v-show="promotion.selectionLanguage == 'es'" v-model="promotion.spanishDescription" type="text" class="form-control w-100 mt-2">
                                    <input v-show="promotion.selectionLanguage == 'en'" v-model="promotion.englishDescription" type="text" class="form-control w-100 mt-2">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="border heading-divider dark-gray-created pl-3 pr-3 pt-2 pb-2">
                    <p class="font-weight-bold mb-0">{{'rules' | translate}}</p>
                </div>
                <div class="d-flex pt-3 pb-3">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-sm-3">
                                 <label>{{'rules' | translate}}:</label>
                                <div class="custom-control custom-switch d-flex pl-2">
                                    <label class="mr-5">{{'default' | translate }}</label>
                                    <input v-model="overrideRules" type="checkbox" class="custom-control-input" id="rules-check" name="rules-check">
                                    <label class="custom-control-label" for="rules-check">{{'override' | translate }}</label>
                                </div>
                            </div>
                            <div class="col-sm-3" v-show="overrideRules">
                                <label>{{ 'booking window' | translate }}:</label>

                                <div class="input-group">
                                    <v-date-picker
                                    class="form-control p-0"
                                    mode="range"
                                    title-position="left"
                                    v-model="rules.bookingWindow"
                                    :popover="{ placement: 'bottom', visibility: 'click' }"
                                    :min-date="new Date()"
                                    :locale="$appConfig.language"
                                    :columns="2"
                                    :masks="{input: 'DD/MMM/YYYY'}"
                                    :input-props='{
                                        class: "form-control-deep",
                                        readonly: true
                                    }'>
                                    </v-date-picker>
                                    <div class="input-group-append">
                                        <button @click="rules.bookingWindow = null" class="btn btn-danger" type="button"><i class="fa fa-times"></i></button>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3" v-show="overrideRules">
                                <label>{{ 'no arrivals' | translate }}:</label>
                                <div class="btn-group btn-group-toggle btn-group-danger d-flex w-100">
                                    <label class="btn btn-secondary shadow-none" :class="{active: rules.noArrival.sun}">
                                        <input type="checkbox"
                                        v-model="rules.noArrival.sun"
                                        id="na-sun" autocomplete="off"> {{'U' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: rules.noArrival.mon}">
                                        <input type="checkbox"
                                        v-model="rules.noArrival.mon"
                                        id="na-mon" autocomplete="off"> {{'M' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: rules.noArrival.tue}">
                                        <input type="checkbox"
                                        v-model="rules.noArrival.tue"
                                        id="na-tue" autocomplete="off"> {{'T' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: rules.noArrival.wed}">
                                        <input type="checkbox"
                                        v-model="rules.noArrival.wed"
                                        id="na-wed" autocomplete="off"> {{'W' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: rules.noArrival.thu}">
                                        <input type="checkbox"
                                        v-model="rules.noArrival.thu"
                                        id="na-thu" autocomplete="off"> {{'R' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: rules.noArrival.fri}">
                                        <input type="checkbox"
                                        v-model="rules.noArrival.fri"
                                        id="na-fri" autocomplete="off"> {{'F' | translate}}
                                    </label>
                                    <label class="btn btn-secondary shadow-none" :class="{active: rules.noArrival.sat}">
                                        <input type="checkbox"
                                        v-model="rules.noArrival.sat"
                                        id="na-sat" autocomplete="off"> {{'S' | translate}}
                                    </label>
                                </div>
                            </div>
                            <div class="col-sm-3" v-show="overrideRules">
                                <label>{{'advance reservation days' | translate}}:</label>
                                <div class="d-flex justify-content-between">
                                    <div class="d-flex justify-content-start">
                                        <p class="fs-12">{{'min' | translate}}:</p>
                                        <input v-model.number="rules.minAdvBooking" type="number" class="form-control w-50 ml-3 mr-3">
                                    </div>
                                    <div class="d-flex justify-content-start">
                                        <p class="fs-12">{{'max' | translate}}:</p>
                                        <input v-model.number="rules.maxAdvBooking" type="number" class="form-control w-50 ml-3 mr-3">
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="d-flex pt-3">
                    <div class="container-fluid" v-show="overrideRules">
                        <div class="row">
                            <div class="col-sm-3">
                                <label>{{'nights' | translate}}:</label>
                                <div class="d-flex justify-content-between">
                                    <div class="d-flex justify-content-start">
                                        <p class="fs-12">{{'min' | translate}}:</p>
                                        <input v-model.number="rules.minLOS" type="number" class="form-control w-50 ml-3 mr-3">
                                    </div>
                                    <div class="d-flex justify-content-start">
                                        <p class="fs-12">{{'max' | translate}}:</p>
                                        <input v-model.number="rules.maxLOS" type="number" class="form-control w-50 ml-3 mr-3">
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <label>{{'occupancy' | translate}}:</label>
                                <div class="d-flex justify-content-between">
                                    <div class="d-flex justify-content-start w-50">
                                        <p class="fs-12">{{'max people' | translate}}:</p>
                                        <select v-model.number="rules.maxGuests" class="form-control text-dark w-50 ml-1 mr-3" id="maxGuests" name="maxGuests">
                                            <option :value="null">...</option>
                                            <option v-for="n in room.maxOccupancy" :value="n" :key="'mxg' + n">{{n}}</option>
                                        </select>
                                    </div>
                                    <div class="d-flex justify-content-start w-50">
                                        <p class="fs-12">{{'children' | translate}}:</p>
                                        <select v-model.number="rules.children" class="form-control text-dark w-50 ml-1 mr-3" id="maxChildren" name="maxChildren">
                                            <option :value="null">...</option>
                                            <option :value="0">0</option>
                                            <option v-for="n in room.maxChildrenOccupancy" :value="n" :key="'mxc' + n">{{n}}</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <label>&nbsp;</label>
                                <div class="d-flex justify-content-between">
                                    <div class="d-flex justify-content-start w-50">
                                        <p class="fs-12">{{'min adults' | translate}}:</p>
                                        <select v-model.number="rules.minAdults" class="form-control text-dark w-50 ml-1 mr-3" id="minAdults" name="minAdults">
                                            <option :value="null">...</option>
                                            <option v-for="n in room.maxChildrenOccupancy" :value="n" :key="'mna' + n">{{n}}</option>
                                        </select>
                                    </div>
                                    <div class="d-flex justify-content-start w-50">
                                        <p class="fs-12">{{'max adults' | translate}}:</p>
                                        <select v-model.number="rules.maxAdults" class="form-control text-dark w-50 ml-1 mr-3" id="maxAdults" name="maxAdults">
                                            <option :value="null">...</option>
                                            <option v-for="n in room.maxAdultsOccupancy" :value="n" :key="'mxa' + n">{{n}}</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <label>&nbsp;</label>
                                <div class="d-flex justify-content-between">
                                    <div class="d-flex justify-content-start">
                                        <p class="fs-12">{{'extra people' | translate}}:</p>
                                        <select v-model.number="rules.extraGuests" class="form-control text-dark w-50 ml-1 mr-3" id="extraGuests" name="extraGuests">
                                            <option :value="null">...</option>
                                            <option :value="0">0</option>
                                            <option v-for="n in room.extraOccupancyAllowed" :value="n" :key="'mxex' + n">{{n}}</option>
                                        </select>
                                    </div>
                                    <div class="d-flex justify-content-start w-50" >
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="gds-container border-top">
                    <div class="p-3">
                        <button type="button" class="btn text-primary m-2"><i class="fa fa-undo mr-3"></i>{{'reset' | translate}}</button>
                        <button type="button" @click="sendRequest" class="btn btn-success m-2">{{'save' | translate}}</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
<!-- eslint-enable -->
</template>

<script>

import RQHelper from '../helpers/rateUpdateHelper';

export default {
    name: 'bulk-update',
    props: {
        hotel: {
            type: Object,
            required: true,
        },
    },
    mounted() {
        this.updateOccupancyPrices();
    },
    data() {
        return {
            room: this.hotel.rooms[0],
            ratePlan: this.hotel.ratePlans[0],
            dateRange: {
                start: new Date(),
                end: new Date(),
            },
            promotion: {
                selectionLanguage: 'es',
                discount: null,
                englishDescription: null,
                spanishDescription: null,
            },
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
            occupancyPrices: false,
            overrideRules: false,
            rules: {
                bookingWindow: null,
                noArrival: {
                    mon: false,
                    tue: false,
                    wed: false,
                    thu: false,
                    fri: false,
                    sat: false,
                    sun: false,
                },
                maxGuests: null,
                maxAdults: null,
                minAdults: null,
                children: null,
                extraGuests: null,
                maxAdvBooking: null,
                minAdvBooking: null,
                minLOS: null,
                maxLOS: null,

            },
        };
    },
    methods: {
        updateOccupancyPrices() {
            this.prices.byOccupancy.adult = [];
            this.prices.exceptions.adult = [];
            for (let i = 0; i < this.room.maxAdultsOccupancy; i += 1) {
                this.prices.byOccupancy.adult.push({ occupation: i + 1, price: 0, type: 1 });
                this.prices.exceptions.adult.push({ occupation: i + 1, price: 0, type: 1 });
            }

            this.prices.byOccupancy.child = [];
            this.prices.exceptions.child = [];
            for (let i = 0; i < this.room.maxChildrenOccupancy; i += 1) {
                this.prices.byOccupancy.child.push({ occupation: i + 1, price: 0, type: 2 });
                this.prices.exceptions.child.push({ occupation: i + 1, price: 0, type: 2 });
            }

            this.prices.byOccupancy.junior = [];
            this.prices.exceptions.junior = [];
            for (let i = 0; i < this.room.maxChildrenOccupancy; i += 1) {
                this.prices.byOccupancy.junior.push({ occupation: i + 1, price: 0, type: 3 });
                this.prices.exceptions.junior.push({ occupation: i + 1, price: 0, type: 3 });
            }
        },
        sendRequest() {
            const rqHelper = new RQHelper(
                this.room,
                this.ratePlan,
                this.dateRange,
                this.promotion,
                this.occupancyPrices,
                this.prices,
                this.overrideRules,
                this.rules,
            );

            rqHelper.validate();
            let html = '';
            if (rqHelper.errors.length > 0) {
                for (let i = 0; i < rqHelper.errors.length; i += 1) {
                    html += `<div class="alert alert-warning mt-1 mb-1" role="alert">
                                <i class="fa fa-times-circle"></i> ${rqHelper.errors[i]}
                            </div>`;
                }

                this.$swal({
                    type: 'warning',
                    html,
                });

                return;
            }

            if (rqHelper.warnings.length > 0) {
                for (let i = 0; i < rqHelper.warnings.length; i += 1) {
                    html += `<div class="alert alert-warning mt-1 mb-1" role="alert">
                                <i class="fa fa-exclamation-triangle"></i> ${rqHelper.warnings[i]}
                            </div>`;
                }
            }
        },
    },
    watch: {
        occupancyPrices(newVal) {
            if (!newVal) {
                $('#priceRatesTab').tab('show');
            }
        },
        room() {
            this.updateOccupancyPrices();
        },
    },
};
</script>
