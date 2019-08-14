<template>
    <form>
        <div class="form-row">
            <div class="form-group col-md-6">
                <label>{{$t('Name')}}</label>
                <b-form-input id="name" v-model="name"></b-form-input>
            </div>
            <div class="form-group col-md-6">
                <label>{{$t('Last names')}}</label>
                <b-form-input id="lastname" v-model="lastName"></b-form-input>
            </div>
        </div>
        <div class="form-row">
            <div :class="col" class="form-group">
                <label>{{$t('Check in')}}</label>
                <v-date-picker
                v-model="_checkIn"
                id="checkin"
                :popover="{ placement: 'bottom', visibility: 'click' }"
                class="form-control p-0">
                </v-date-picker>
            </div>
            <div :class="col" class="form-group">
                <label>{{$t('Check out')}}</label>
                <v-date-picker
                v-model="_checkOut"
                id="checkout"
                :popover="{ placement: 'bottom', visibility: 'click' }"
                class="form-control p-0">
                </v-date-picker>
            </div>
            <div v-if="showTotalNR" class="form-group col-md-3">
                <label>TotalNR</label>
                <b-form-input id="totalnr" v-model="totalNR"></b-form-input>
            </div>
            <div :class="col" class="form-group">
                <label>Total</label>
                <b-form-input id="total" v-model="total"></b-form-input>
            </div>
        </div>
    </form>
</template>
<script>
export default {
    props: ['name',
    'lastName',
    'total',
    'totalNR',
    'checkIn',
    'checkOut',
    'showTotalNR'],

    computed: {
        _checkIn() {
            return new Date(this.checkIn);
        },
        _checkOut() {
            return new Date(this.checkOut);
        },
        col() {
            return {
                'col-md-4': !this.showTotalNR,
                'col-md-3': this.showTotalNR,
            }
        },
    },
}
</script>
