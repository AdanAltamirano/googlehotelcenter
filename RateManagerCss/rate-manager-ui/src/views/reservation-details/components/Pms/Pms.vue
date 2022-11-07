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
                        <b-button variant="link" class="ml-1 p-0 align-baseline" @click="reactivate"><i class="fas fa-pen fa-1x-c"></i></b-button>
                    </template>
                    <template v-else-if="!pms.status && pms.failedAttempts < 3" >
                        <strong>{{ $t('Waiting to be confirmed') }}</strong>
                    </template>
                    <template v-else-if="pms.status">
                        <strong>{{ $t('Reservation confirmed') }}</strong>
                    </template>
                </div>
                <div v-else>
                    <strong>{{(!pms.status ? $t('Waiting to be confirmed') : $t('Reservation confirmed'))}}</strong>
                </div>
                <div>
                    <strong>{{$t('Status')}}:</strong>
                    {{PmsStatus}}
                    <div style="display:inline-block !important;">                   
                        <pms-status v-if="supervisor" :reservationId="id"></pms-status>
                    </div>                   
                </div>                
                <span v-if="pms.status">
                    <strong>{{$t('Reservation number')}}:</strong>
                    {{pms.reservationNumber}}
                </span>
            </address>
        </b-alert>
    </b-collapse>
    </div>
</template>

<script>
import ReservationService from '../../../../api/reservation-service';
import PmsStatus from './Status/Status.vue';
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
        PmsStatus
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

