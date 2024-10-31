<template>
	<!-- eslint-disable -->
	<v-popover placement="top" :auto-hide="false">      
		<template>
			<b-button class="font-weight-bold mb-2" variant="primary">{{$t('Verified booking')}}</b-button>
		</template>
		<template slot="popover">
			<diV class="d-flex col-gap-2">
				<h5 class="text-primary font-weight-bold mb-2">{{$t('Verification Code')}}</h5>
				<a ref="close" v-close-popover href="javascript:;" class="text-danger"><i class="fa fa-times"></i></a>
			</div>
			<hr style="margin-top: 0rem; margin-bottom: 1rem; border-top: 1px solid rgba(0,0,0,.1);">
			<div id='popover-1'>
				<form @submit.stop.prevent="save">
					<!-- 1st Row -->
					<div class="form-row">
							<div class="form-group col-md-12">
								<label for="select-status">{{$t('Code')}}</label>
								<b-form-input v-model="pmsCode"></b-form-input>                         
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
			pmsCode:''
		}
	},
	methods: {
		save(){
			const request = {
				status: true,
				pmsCode: this.pmsCode,
				verifyAction: false
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
					return ReservationService.ReservationPmsVerifyUpdate(this.reservationId, request)
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
		error(title) {
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
