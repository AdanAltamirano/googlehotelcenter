<template>
<!-- eslint-disable -->
    <div id="bulk-update-form" class="collapse pl-3 pr-3 pt-3">
        <div class="formulario-page overflow-auto pb-5">
            <div class="bg-light">
                <div class="rate-plan d-flex justify-content-between border-top mt-2 pl-3 pt-3 pr-3">
                    <div class="w-20 pr-3">
                        <label>{{'room' | translate}}:</label>
                        <div class="form-group">
                            <select v-model="room" class="form-control text-dark" id="room" name="room">
                                <option v-for="room in hotelRooms" :value="room" :key="room.id">{{room.code}} - {{room.name}}</option>
                            </select>
                        </div>
                    </div>
                    <div class="w-20 pr-3">
                        <label>{{'rate plan' | translate}}:</label>
                        <div class="form-group">
                            <select v-model="ratePlan" class="form-control text-dark" id="ratePlan" name="rateplan">
                                <option v-for="plan in hotel.ratePlans" :value="plan" :key="plan.code">{{plan.code}} - {{plan.name}}</option>
                            </select>
                        </div>
                    </div>
                    <div class="w-25 pr-3">
                        <label class="d-block">{{ 'dates' | translate }}:</label>
                        <div class="d-flex">
                            <v-date-picker
                            mode="range"
                            class="w-70"
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
                            <div class="d-flex ml-2">                          
                                <button type="button" class="btn" @click="addDateToList(dateRange)"><i class="fas fa-calendar-plus fa-lg" style="color:#15cc3f;"></i></button>
                                <button type="button" class="btn" @click="removeDateFromList()"><i class="fas fa-calendar-minus fa-lg" style="color:#f55050;"></i></button>
                            </div>
                        </div>
                    </div>
                    <div class="w-20 pr-3">
                        <label class="d-block">{{'dates for the rate' | translate}}:</label>
                        <div v-if="datesList.length > 0" class="list-group w-100 h-90-px scroll-y">                           
                            <span v-for="(date,index) in datesList" :value="date" :key="index" class="list-group-item">{{getDateFormat(date)}}</span>                        
                        </div>
                        <div v-else class="alert alert-warning" role="alert">
                            {{'Add Dates' | translate}} 
                        </div>
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
                <div class="border heading-divider dark-gray-created pl-3 pr-3 pt-2 pb-2 mt-2">
                    <p class="font-weight-bold mb-0">{{'room prices' | translate}} - <b>{{ hotel.taxIncluded ? 'tax included': 'tax not included' | translate}}</b></p>
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
                                            <div class="rates-col col-4 p-0">
                                                <div class="p-2 text-center text-primary">
                                                    <label class="m-0">{{'adults' | translate}}</label>
                                                </div>
                                                <div v-show="!occupancyPrices">
                                                    <div class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-15">{{'base' | translate}}</label>
                                                        <input v-model.number="prices.byRoom.adult" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="occupancyPrices">
                                                     <div v-for="(p, idx) in prices.byOccupancy.adult" :key="'ra' + idx" class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-15 text-center">{{p.occupation}}</label>
                                                        <input v-model.number="p.price" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                            <label class="w-15">{{'extra' | translate}}</label>
                                                            <input v-model.number="prices.extra.adult" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                            <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                        </div>
                                                </div>
                                            </div>
                                            <div class="rates-col col-4 p-0" v-if="room.maxChildrenOccupancy > 0">
                                                <div class="p-2 text-center text-primary">
                                                    <label class="m-0">{{'children' | translate}}</label>
                                                </div>
                                                <div v-show="!occupancyPrices">
                                                    <div class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-15">{{'base' | translate}}</label>
                                                        <input v-model.number="prices.byRoom.child" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="occupancyPrices">
                                                    <div v-for="(p, idx) in prices.byOccupancy.child" :key="'rc' + idx" class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-15 text-center">{{p.occupation}}</label>
                                                        <input v-model.number="p.price" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                        <label class="w-15">{{'extra' | translate}}</label>
                                                        <input v-model.number="prices.extra.child" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="rates-col col-4 p-0" v-if="room.maxChildrenOccupancy > 0 && room.juniorsAllowed">
                                                <div class="p-2 text-center text-primary">
                                                    <label class="m-0">{{'juniors' | translate}}</label>
                                                </div>
                                                <div v-show="!occupancyPrices">
                                                    <div class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-15">{{'base' | translate}}</label>
                                                        <input v-model.number="prices.byRoom.junior" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="occupancyPrices">
                                                    <div v-for="(p, idx) in prices.byOccupancy.junior" :key="'rj' + idx"  class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-15">{{p.occupation}}</label>
                                                        <input v-model.number="p.price" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                        <label class="w-15">{{'extra' | translate}}</label>
                                                        <input v-model.number="prices.extra.junior" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
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
                                        <div class="form-check d-flex pl-0">
                                            <div class="price-rates">
                                                <div class="p-2 text-center text-primary">
                                                    <label class="m-0">{{'adults' | translate}}</label>
                                                </div>
                                                <div v-show="occupancyPrices">
                                                     <div v-for="(p, idx) in prices.exceptions.adult" :key="'rax' + idx" class="d-flex justify-content-between bg-blue-created border p-1">
                                                        <label class="w-15 text-center">{{ p.occupation }}</label>
                                                        <input v-model.number="p.price" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" >
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                            <label class="w-15">{{'extra' | translate}}</label>
                                                            <input v-model.number="prices.extra.adult" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2">
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
                                                        <input v-model.number="p.price" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                        <label class="w-15">{{'extra' | translate}}</label>
                                                        <input v-model.number="prices.extra.child" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2">
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
                                                        <input v-model.number="p.price" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
                                                        <label class="font-weight-bold text-primary w-15"><span>{{ratePlan.currency}}</span></label>
                                                    </div>
                                                </div>
                                                <div v-show="room.extraOccupancyAllowed > 0">
                                                    <div class="d-flex justify-content-between dark-gray-created border p-1">
                                                        <label class="w-15">{{'extra' | translate}}</label>
                                                        <input v-model.number="prices.extra.junior" type="number" min="0" step="any" class="form-control text-right w-60 ml-2 mr-2" value="1">
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
                                    <input v-model.number="promotion.discount" type="number" min="0" max="100" step="any" class="form-control w-25 ml-3 mr-3">
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
                                        <input v-model.number="rules.minAdvanceBooking" type="number" min="0" class="form-control w-50 ml-3 mr-3">
                                    </div>
                                    <div class="d-flex justify-content-start">
                                        <p class="fs-12">{{'max' | translate}}:</p>
                                        <input v-model.number="rules.maxAdvanceBooking" type="number" min="0" class="form-control w-50 ml-3 mr-3">
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
                                        <input v-model.number="rules.minLOS" type="number" min="0" class="form-control w-50 ml-3 mr-3">
                                    </div>
                                    <div class="d-flex justify-content-start">
                                        <p class="fs-12">{{'max' | translate}}:</p>
                                        <input v-model.number="rules.maxLOS" type="number" min="0" class="form-control w-50 ml-3 mr-3">
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
                        <button type="button" @click="reset" class="btn text-primary m-2"><i class="fa fa-undo mr-2"></i>{{'reset' | translate}}</button>
                        <button type="button" :disabled="datesList.length == 0" @click="verifyRequest" class="btn btn-success m-2"><i class="fa fa-save mr-2"></i> {{'save' | translate}}</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
<!-- eslint-enable -->
</template>

<script>

import RQHelper from '../helpers/rateUpdateHelper';
import ratesService from '../../../api/rates-service';
import utilities from '../helpers/utilities';

const initalState = (room, ratePlan, start, end) => ({
    room,
    ratePlan,
    dateRange: {
        start,
        end,
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
        maxAdvanceBooking: null,
        minAdvanceBooking: null,
        minLOS: null,
        maxLOS: null,
    },
    datesList:[]
});


export default {
    name: 'bulk-update',
    props: {
        hotel: {
            type: Object,
            required: true,
        },
        initialDateRange: {
            type: Object,
            required: true,
        },
    },
    mounted() {
        this.updateOccupancyPrices();
    },
    data() {
        return initalState(
            this.hotel.rooms[0],
            this.hotel.ratePlans[0],
            this.initialDateRange.start.toDate(),
            this.initialDateRange.end.toDate(),
        );
    },
    computed: {
        stateDateRangeStart() {
            return this.$store.getters.dateRange.start;
        },
        hotelRooms() {
            // solo habitaciones no linkeadas
            return this.hotel.rooms.filter(x => !x.isLinked);
        },
    },
    methods: {
        addDateToList(date){
            //Pendiente que no traslapen las fechas
            //Ver que las fechas no traslapen asi solo se puede agregar a la lista
            this.datesList.push(this.dateRange);

            const overlap = this.overlapDates(this.datesList);
            
            if(overlap.overlap)
            {
                this.$appAlert({
                     type: 'error',
                    title: this.$t('Dates Overlap'),
                    showCloseButton: true,
                    showConfirmButton:false,
                    showCancelButton:false
                });

                this.datesList = [];
            }
        },
        overlapDates(dates){
            var sortedRanges = dates.sort((previous, current) => {  
                // get the start date from previous and current
                var previousTime = previous.start.getTime();
                var currentTime = current.start.getTime();

                // if the previous is earlier than the current
                if (previousTime < currentTime) {
                return -1;
                }

                // if the previous time is the same as the current time
                if (previousTime === currentTime) {
                return 0;
                }

                // if the previous time is later than the current time
                return 1;
            });

            var result = sortedRanges.reduce((result, current, idx, arr) => {
                // get the previous range
                if (idx === 0) { return result; }
                var previous = arr[idx-1];
            
                // check for any overlap
                var previousEnd = previous.end.getTime();
                var currentStart = current.start.getTime();
                var overlap = (previousEnd >= currentStart);
            
                // store the result
                if (overlap) {
                    // yes, there is overlap
                    result.overlap = true;
                    // store the specific ranges that overlap
                    result.ranges.push({
                        previous: previous,
                        current: current
                    })
                }
            
                return result;
            
                // seed the reduce  
            }, {overlap: false, ranges: []});
            return result;
        },
        getDateFormat(date) {
            return `${this.$moment(date.start).format('DD/MMM/YYYY')} - ${this.$moment(date.end).format('DD/MMM/YYYY')}`;
        },
        removeDateFromList(){
            this.datesList.pop();
        },
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
        verifyRequest() {
            const rqHelper = new RQHelper(
                this.room,
                this.ratePlan,
                this.dateRange,
                this.promotion,
                this.occupancyPrices,
                this.prices,
                this.overrideRules,
                this.rules,
                this.datesList
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
                this.$appAlert({
                    type: 'error',
                    html,
                });

                return;
            }

            if (rqHelper.warnings.length > 0) {
                for (let i = 0; i < rqHelper.warnings.length; i += 1) {
                    /* eslint-disable max-len */
                    html += `<div class="alert alert-info mt-1 mb-1" role="alert">
                                <i class="fa fa-exclamation-triangle"></i> <small>${this.$t(rqHelper.warnings[i])}</small>
                            </div>`;
                    /* eslint-enable max-len */
                }
            }


            // mostrar alerca con advertencias y si lo quiere continuar
            this.$appAlert({
                type: 'info',
                title: this.$t('are you sure?'),
                html,
                showCancelButton: true,
                confirmButtonText: this.$t('yes, save it!'),
                cancelButtonText: this.$t('cancel'),
            }).then((result) => {
                // si acepa enviar request
                if (result.value) this.sendRequest(rqHelper.createRQ());
            });
        },
        sendRequest(RQ) {
            console.log("Send Request");
            console.log(RQ);
            ratesService.bulkUpdate(this.$appConfig.session.hotelId, RQ)
                .then(() => {
                    this.$appAlert({
                        type: 'success',
                        title: this.$t('successful update'),
                    }).then(() => {
                        $('#bulk-update-form').collapse('hide');
                        const start = this.$moment(this.$data.dateRange.start);
                        const end = start.clone().add(13, 'days');
                        utilities.setLastWorkDay(start);
                        this.$store.commit('update', { start, end });
                        this.resetData();
                    });
                }).catch(() => {
                    this.$appAlert({
                        type: 'error',
                        title: this.$t('invalid request, please contact support'),
                    });
                });
        },
        reset() {
            this.$appAlert({
                type: 'question',
                title: this.$t('are you sure?'),
                text: this.$t('the form will be set to its initial state'),
                showCancelButton: true,
                cancelButtonText: this.$t('cancel'),
                confirmButtonText: this.$t('yes'),
            }).then((result) => {
                // si acepa enviar request
                if (result.value) {
                    this.resetData();
                }
            });
        },
        resetData() {
            const initialData = initalState(
                this.hotel.rooms[0],
                this.hotel.ratePlans[0],
                this.$store.getters.dateRange.start.toDate(),
                this.$store.getters.dateRange.end.toDate(),          
            );
            console.log(this.$data);
            Object.assign(this.$data, initialData);
            this.updateOccupancyPrices();
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
        stateDateRangeStart(newStart) {
            if (!newStart.isSame(this.dateRange.start)) {
                this.dateRange = {
                    start: newStart.toDate(),
                    end: newStart.clone().add(13, 'days').toDate(),
                };
            }
        },
        datesList(value){
            console.log(this.datesList);
            console.log(value);
        }
    },
};
</script>
