<template>
     <!-- eslint-disable -->
    <v-popover placement="top" :auto-hide="false">      
        <template>
            <a href="javascript:;"><i class="fas fa-pen fa-1x-c"></i></a>
        </template>
        <template slot="popover">
            <div class="d-flex col-gap-2">
                <h5 class="text-primary font-weight-bold mb-2">{{$t('Change PMS Status')}}</h5>
                <a ref="close" v-close-popover href="javascript:;" class="text-danger"><i class="fa fa-times"></i></a>
            </div>
            <hr style="margin-top: 0rem; margin-bottom: 1rem; border-top: 1px solid rgba(0,0,0,.1);">
            <div id='popover-1'>
                <form @submit.stop.prevent="save">
                    <!-- 1st Row -->
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="select-status">PMS Status</label>
                            <b-form-select id="select-status" v-model="selected" :options="options"></b-form-select>                            
                        </div>
                    </div>
                    <button type="submit" class="btn btn-success m-2"><i class="fa fa-save mr-2"></i> {{'Save' | translate}}</button>
                </form>                  
            </div>
        </template>
    </v-popover>
    <!-- eslint-enable -->
</template>

<script>
import ReservationService from '../../../../../api/reservation-service';
export default {
    props:['reservationId'],
    data() {
        return {
            selected: 1,
            options:[
                { value: 0, text: this.$t('In process') },
                { value: 1, text: this.$t('Confirmed') },
            ]
        }
    },
    methods: {
        save() {
             const request = {
                status: this.selected === 1 ? true : false
            };

            this.$swal.fire({
                type: "info",
                title: this.$t("Save ?"),
                showCancelButton: true,
                cancelButtonText: this.$t("Cancel"),
                cancelButtonColor: "#d33",
                confirmButtonColor: "#3085d6",
                confirmButtonText: this.$t("Save"),
                showLoaderOnConfirm: true,
                preConfirm: async () => {         
                    
                    return ReservationService.ReservationPmsStatusUpdate(this.reservationId, request)
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
                
                if(result.value.response.status === 200 && result.value.response.body.isSuccess) this.$swal.fire(this.success(this.$t('Pms updated')));
                else this.$swal.fire(this.error(this.$t('Pms did not update')));
                
            });            
        },
        success(title) {
            return {
                type: "success",
                title: title,
                showCancelButton: true,
                showConfirmButton:false,
                cancelButtonText: this.$t("Exit"),
                cancelButtonColor: "#d33",
                onClose:() => {
                    window.location.reload();
                }
            };
        },
        error (title) {
            return {
                type: "error",
                title: title,
                showCancelButton: true,
                showConfirmButton:false,
                cancelButtonText: this.$t("Exit"),
                cancelButtonColor: "#d33",
            }
        }
    }
}
</script>
