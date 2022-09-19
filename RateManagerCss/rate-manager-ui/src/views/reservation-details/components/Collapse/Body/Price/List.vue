<template>
    <div>
        <b-list-group-item class="cite-date d-flex justify-content-center p-2" v-for="(price, priceIndex) in priceDetails" :key="priceIndex">               
                <price-item :priceDetail="price" :index="priceIndex"/>
                <span class="ml-4 text-danger remove-item" @click="remove(priceIndex)">( <i class="fa fa-times"></i> ) {{$t('Remove Rate')}}</span>
        </b-list-group-item>
    </div>
</template>

<script>
import PriceItem from './Item.vue';
export default {
    props: {
        priceDetails:{
            type: Array
        }
    },
    components: {
        PriceItem
    },
    methods:{
        remove(index){
            this.$emit('removePriceDetail',index);
        }
    }
}
</script>
