<template>
    <div>
        <h6 style="cursor:pointer" v-b-toggle.pms>
            <i class="fa fa-plus-circle"></i>
            {{$t('PMS Status')}}
        </h6>
        <b-collapse visible id="pms">
            <b-alert show variant="secondary">
                <address>               
                    <div v-if="supervisor">
                        <template v-if="!pms.status && pms.failedAttempts === 3">
                            <strong>{{ $t('Maximun attempts reached') }}</strong>
                            <b-button class="ml-1 p-1 align-baseline" @click="reactivate" variant="primary">{{$t('Send Again')}}</b-button>
                        </template>
                        <template v-else-if="!pms.status && pms.failedAttempts < 3" >
                            <strong>{{ $t('Waiting to be collected') }}</strong>
                        </template>
                        <template v-else-if="pms.status">
                            <strong>{{ $t('Reservation Collected') }}</strong>
                        </template>
                    </div>
                    <div v-else>
                        <strong>{{(!pms.status ? $t('Waiting to be collected') : $t('Reservation Collected'))}}</strong>
                    </div>
                    <div>
                        <strong>{{$t('Status')}}:</strong>
                        {{PmsStatus}}
                        <div class="d-inline-block">                   
                                                <pms-status v-if="supervisor" :reservationId="id"></pms-status>
                        </div>
                        <!-- <div v-if="supervisor">
                            <strong>{{$t('Change only status')}}</strong>
                            <div class=" d-inline-block ml-1">
                                <pms-status-only v-if="supervisor" :reservationId="id"></pms-status-only>
                            </div>
                        </div>-->
                        <div>
                            <span v-if="pms.status">
                                <strong>{{$t('Reservation number')}}:</strong>
                                {{pms.reservationNumber}}
                            </span>
                        </div>
                        <div v-if="supervisor">
                            <b-button  v-tooltip="$t('With this action the reservation will be available to be downloaded for the pms')" 
                                v-if="pms.status" class="font-weight-bold mb-2" 
                                variant="primary" 
                                @click="saveNotVerified">{{$t('Check as not verified')}}
                            </b-button>
                            <pms-verify v-else-if="!pms.status" :reservationId="id"></pms-verify>
                        </div>
                    </div>                               
                </address>
            </b-alert>
        </b-collapse>
    </div>
</template>

<script>
import ReservationService from '../../../../api/reservation-service';
import PmsStatus from './Status/Status.vue';
import PmsStatusOnly from './Status/StatusOnly.vue';
import PmsVerify from './Status/Verify.vue';
export default {
    props: {
        id: {
            require: true,
            type: Number
        },
        pms: {
            type: Object
        },
        supervisor: {

        }
    },
    components: {
        PmsStatus,
        PmsStatusOnly,
				PmsVerify
    },
    created() {
    },
    computed: {
        PmsStatus() {
            let r;
            switch (this.pms.action) {
                case "SS":
                r = this.$t("New");
                break;
                case "CC":
                r = this.$t("Modified");
                break;
                case "XX":
                r = this.$t("Canceled");
                break;
            }
            return r;
        }
    },
    methods: {
        saveNotVerified() {
            const request = {
                status: false,
                verifyAction: true
            }

            this.$swal.fire({
                type: "info",
                title: this.$t("Save ?"),
                showCancelButton: true,
                cancelButtonText: this.$t("Cancel"),
                cancelButtonColor: "#d33",
                confirmButtonColor: "#3085d6",
                confirmButtonText: this.$t("Save"),
                showLoaderOnConfirm: true,
                preConfirm:async()=> {                           
                    return ReservationService.ReservationPmsVerifyUpdate(this.id, request)
                    .then(response => {
                            return {
                                    response : response
                            }
                    })
                    .catch(error => {
                            return {
                                    response: error
                            }
                    });
                },
                allowOutsideClick: () => !this.$swal.isLoading(),
            }).then(result => {             
                if(result.value.response.status === 200 && result.value.response.body.isSuccess) this.$swal.fire(this.success(this.$t('Saved')));
                else this.$swal.fire(this.error(this.$t('Error')));               
            });  	
        },
        reactivate() {
            this.$swal.fire({
                title:this.$t('Reactivate PMS ?'),
                text:'',
                icon: 'warning',
                confirmButtonText:this.$t('Reactivate'),
                cancelButtonText: this.$t("Cancel"),
                showLoaderOnConfirm: true,
                showCancelButton: true,
                showConfirmButton: true,
                cancelButtonColor: "#d33",
                confirmButtonColor: "#3085d6",
                preConfirm: () => {

                    return ReservationService.ReservationPmsReactivate(this.id.toString())
                    .then(response => {
                        return {
                            response : response
                        }
                    })
                    .catch(response => {
                        return {
                            response: response
                        }
                    });

                },
                allowOutsideClick: () => !this.$swal.isLoading(),
            })
            .then(res => {

                if(res.value.response.status === 200) this.$swal.fire(this.success(this.$t("Pms has been reactivated"))); 
                else this.$swal.fire(this.error(this.$t("Pms has not been reactivated")));
            });
        },
        success(title) {
            return {
                type: "success",
                title: title,
                showConfirmButton: false,
                showCancelButton: true,
                cancelButtonText: this.$t("Exit"),
                cancelButtonColor: "#d33",
                time: 2500,
                onClose: () => {
                window.location.reload();
                }
            };
        },
        error(title) {
            return {
                type: "error",
                title: title,
                showConfirmButton: false,
                showCancelButton: true,
                cancelButtonText: this.$t("Exit"),
                cancelButtonColor: "#d33",
            };
        },
    },
}
</script>

