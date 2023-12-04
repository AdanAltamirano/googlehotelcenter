<template>
  <div class="">
    <div v-for="restriction in restrictions" :key="restriction.type">
        <!-- 1 LockGral -->
        <template v-if="restriction.type == 1">
            <b-alert class="mt-3" :variant="Variant(restriction.isSuccess)" show>
                {{$t('Restriction General')}} {{IsSuccess(restriction.isSuccess)}}
            </b-alert>
            <b-alert v-if="restriction.xml.length == 2" class="mt-3" :variant="Variant(restriction.isSuccessPromo)" show>
                {{$t('Restriction Promos')}} {{IsSuccess(restriction.isSuccessPromo)}}
            </b-alert>
        </template>
        <!-- 2 LockRatePlan -->
        <template v-if="restriction.type == 2">
            <b-alert class="mt-3" :variant="Variant(restriction.isSuccess)" show>
                {{$t('Restriction RatePlan')}} {{IsSuccess(restriction.isSuccess)}}
            </b-alert>
        </template>
        <!-- 3 LockRoomType -->
        <template v-if="restriction.type == 3">
            <b-alert class="mt-3" :variant="Variant(restriction.isSuccess)" show>
                {{$t('Restriction by Room and RatePlan')}} {{IsSuccess(restriction.isSuccess)}}
            </b-alert>
        </template>
    </div>
  </div>
</template>

<script>
export default {
    props:{
        restrictions: {
            type:Object
        }
    },
    methods:{
        IsSuccess(isSuccess){
            let res = isSuccess? this.$t("Updated") : this.$t("Fail")
            return res;
        },
        Variant(isSuccess){
            return isSuccess ? 'success': 'danger';
        }
    }
}
</script>

