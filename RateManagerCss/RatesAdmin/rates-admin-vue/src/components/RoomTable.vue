<template>
     <div class="room-description ml-5 mr-5 mt-3">
        <div class="d-flex border dark-gray-created">
            <div class="d-flex w-30 align-items-center">
                <div class="border-right p-1 flex-fill d-flex w-80 justify-content-between">
                    <h4 class="font-weight-bold m-0"><i class="fa fa-bed mr-1"></i> {{room.name}}</h4>
                    <h4 class="text-primary m-0"><i class="fa fa-bolt"></i></h4>
                </div>
                <div class="border p-1 flex-fill w-20">
                    <span class="text-uppercase">{{ 'avail' | translate}}</span>
                </div>                    
            </div>
            <div class="d-flex w-70">
                <div class="border p-1 flex-fill text-center">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                    <span>25</span> 
                </div>
                <div class="border p-1 flex-fill text-center avail-selected">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center avail-selected">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center avail-selected">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center avail-selected">
                    <span>25</span>
                </div>
                <div class="border p-1 flex-fill text-center">
                    <span>25</span>
                </div> 
            </div>                
        </div>

        <template v-for="rate in room.rates">
            <div class="d-flex bg-white border-top border-primary border-5" :key="rate.ratePlanId">
                <div class="d-flex w-30">
                    <div class="border flex-fill d-flex w-80 align-items-center p-1 pl-4">
                        <h5 class="m-0 pl-2">{{rate.ratePlan}}</h5>
                        <div class="btn-group ml-3" v-if="rate.children.length > 0">
                            <button data-toggle="collapse" class="btn btn-primary badge badge-pill badge-primary" :data-target="'#' + rate.ratePlanId + '-'+  rate.roomId + '-lk'">
                                <span>{{rate.children.length}} {{'links' | translate}} </span><i class="fa fa-chevron-down"></i>
                            </button>
                        </div>
                    </div>
                    <div class="border p-1 flex-fill w-20">
                        <span class="text-secondary">
                            <small><i class="fa fa-user"></i>x{{rate.dailyRates[0].occupancy}}</small>
                        </span> 
                        <span class="text-uppercase text-primary ml-2">
                            <small>{{rate.currency}}</small>
                        </span>
                    </div>                    
                </div>
                <div class="d-flex w-70">
                    <div class="border p-1 flex-fill text-center" v-for="day in rate.dailyRates" :key="day.date">
                        <span>{{ day.price }}</span>
                    </div>
                </div>
            </div>
            <div :id="rate.ratePlanId + '-'+  rate.roomId + '-lk'" class="collapse show" v-if="rate.children.length > 0" :key="'c' + rate.ratePlanId">
                <div class="d-flex bg-white" v-for="child in rate.children" :key="child.ratePlanId">
                    <div class="d-flex w-30">
                        <div class="border flex-fill d-flex w-80 justify-content-between align-items-center bg-blue-created p-1 pl-4">
                            <h5 class="m-0 pl-3 text-primary">· {{child.ratePlan}}
                                <i class="fa fa-user text-secondary"></i>x{{child.dailyRates[0].occupancy}}
                            </h5>
                            <span class="text-primary m-0"><i class="fa fa-link"></i></span>
                        </div>
                        <div class="border flex-fill w-20 bg-blue-created">                      
                            <span class="text-uppercase text-primary ml-2">{{child.currency}}</span>                       
                        </div>                    
                    </div>
                    <div class="d-flex w-70">
                        <div class="border p-1 flex-fill text-center bg-blue-created" v-for="day in child.dailyRates" :key="day.date">
                            <span>{{ day.price }}</span>
                        </div>
                    </div>                
                </div>
            </div>
        </template>       
    </div>
</template>

<script>
export default {
    name: 'room-table',
    props:{
        room:{
            type: Object,
            required: true
        }
    }
}
</script>

