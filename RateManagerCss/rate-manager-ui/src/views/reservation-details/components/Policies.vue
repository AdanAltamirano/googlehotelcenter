<template>
    <div>
        <h5 class="text-info">{{$t('Policies')}}</h5>
        <h6 style="cursor:pointer" v-b-toggle.hpol>
            <i class="fa fa-plus-circle"></i> {{$t('Hotel policies')}}
        </h6>
        <b-collapse id="hpol">
            <b-alert show variant="secondary">
                <address>
                    <strong>{{$t('Cancellation policies')}}: </strong>{{result.policyDetails.hotelCancellation}}
                    <br>
                    <strong>{{$t('Warranty policies')}}: </strong>{{result.policyDetails.hotelGuarantee}}
                    <br>
                    <strong>{{$t('Credit card policies')}}: </strong>{{result.policyDetails.hotelCreditCard}}
                </address>
            </b-alert>
        </b-collapse>
        <h6 class="mt-3" style="cursor:pointer" v-b-toggle.rppol>
            <i class="fa fa-plus-circle"></i> {{$t('Rate plan policies')}}
        </h6>
        <b-collapse id="rppol">
            <b-alert show variant="secondary">
                <address>
                    <strong>{{$t('Cancellation policies')}}: </strong>{{result.policyDetails.ratePlanCancellation}}
                    <br>
                    <strong>{{$t('Warranty policies')}}: </strong>{{result.policyDetails.ratePlanGuarantee}}
                    <br>
                    <strong>{{$t('Credit card policies')}}: </strong>{{result.policyDetails.ratePlanCreditCard}}
                </address>
            </b-alert>
        </b-collapse>
    </div>
</template>
<script>
export default {
    props: {
        result: {
            required: true,
            type: Object
        },
    },
}
</script>

