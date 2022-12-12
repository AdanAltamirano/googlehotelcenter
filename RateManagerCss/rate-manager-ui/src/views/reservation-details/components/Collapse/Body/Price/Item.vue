<template>
    <v-popover trigger='hover' placement="right" :auto-hide="false">
        <a href="javascript:;">{{ GetFormat }} ${{price | currency}} {{currency}}</a>
        <template slot="popover">
            <div class="d-flex flex-row-reverse justify-content-between">
                <a ref="close" v-close-popover href="javascript:;" class="text-danger"><i class="fa fa-times"></i></a>
                <h5 class="text-primary font-weight-bold mb-2">{{$t('Rate')}} #{{(index + 1)}}</h5>
            </div>
            <hr class="divider">
            <div :id="'popover-' + index">
                <div class="container-flex-column  row-gap-1"> 
                    <div class="container-flex-column"> 
                        <div><span class="text-primary"><i class="fas fa-tag"></i> {{$t('Price')}} <i class="fas fa-long-arrow-alt-right"></i> {{price | currency}} {{currency}}</span></div>
                        <div><span class="text-muted pl-2"><i class="fas fa-tag"></i> Extra <i class="fas fa-long-arrow-alt-right"></i> {{extraPrice | currency}} {{currency}}</span></div>
                    </div>
                    <div class="container-flex-column">
                        <div><span class="text-primary"><i class="fas fa-tag"></i> {{$t('Price NR')}} <i class="fas fa-long-arrow-alt-right"></i> {{priceNR | currency}} {{currency}}</span></div>
                        <div><span class="text-muted pl-2"><i class="fas fa-tag"></i> Extra NR <i class="fas fa-long-arrow-alt-right"></i> {{extraPriceNR | currency}} {{currency}}</span></div>
                    </div>
                </div>
            </div>
        </template>                                              
    </v-popover>
</template>

<script>
export default {
    props:{
        priceDetail:{
            type:Object
        },
        index:{
            type:Number
        }
    },
    data() {
        return {
            checkIn:null,
            checkOut:null,
            price:0,
            extraPrice:0,
            priceNR:0,
            extraPriceNR:0,
            currency:null,
        }
    },
    created() {
        //Destructuring Object
        ({checkIn:this.checkIn , checkOut:this.checkOut, price: this.price,
          extraPrice:this.extraPrice, priceNR:this.priceNR, extraPriceNR:this.extraPriceNR,
          currency:this.currency} = this.priceDetail)
    },
    computed: {
        GetFormat() {
            return `${this.$moment(this.checkIn).format('DD-MM-YYYY')} - ${this.$moment(this.checkOut).format('DD-MM-YYYY')}`;
        },
    },
}
</script>
