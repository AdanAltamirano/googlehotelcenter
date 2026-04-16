<template>
	<div>
		<!-- Item 1 -->
		<table style="border-collapse:separate !important; border-spacing:15px !important;"> 
			<thead>
				<tr>
					<th>Hotel Internet Power</th>
					<th>Hotel Arpon</th>
					<th>{{$t('Agency')}}</th>
					<th>Url</th>
				</tr>
			</thead>
			<tbody>
				<tr>
					<td>{{hotelId}}</td>
					<td>
						<b-form-input class="w-300" v-model="arponId" required></b-form-input>
					</td>
					<td>
						<b-form-input class="w-300" v-model="agencyArpon"></b-form-input>
					</td>
					<td>
						<b-form-input class="w-400" v-model="urlArpon" required></b-form-input>
					</td>
				</tr>
				<tr>
					<td></td>
					<td><strong>{{$t('Emails To Notif')}}</strong></td>
					<td></td>
					<td></td>
				</tr>
				<tr>
					<td></td>
					<td>
						<b-form-input class="w-300" v-model="emails" required></b-form-input>
					</td>
					<td>
						<b-form-checkbox v-model="active">
							<strong>{{$t('Send Reservations')}}</strong>
						</b-form-checkbox>
					</td>
					<td>
						<div v-if="callApi" class="vld-parent" style="width:70px;height:70px;">
							<loading style="display:block !important;" :active="true" :is-full-page="false" color="#007bff"></loading>
						</div>
						<div v-else>
							<b-button v-if="showButton" :disabled="!arponId.length > 0 || !urlArpon.length > 0" variant="primary" @click="SaveArponHotel">{{$t('Save')}}</b-button>
						</div>
					</td>
				</tr>
			</tbody>
		</table>		
	</div>
</template>

<script>
import Loading from "vue-loading-overlay";
import ArponService from '../../../api/arpon-service';

export default {
	components:{
		Loading
	},
	created(){
		this.GetArponHotel();
	},
	data(){
		return {
			hotelId: this.$appConfig.session.hotelId,
			arponId: '',
			urlArpon: '',
			agencyArpon: '',
			emails: '',
			active:false,
			callApi:false,
			showButton:true
		}
	},
	mounted(){

	},
	methods:{
		//Alert
		success(title) {
			return {
					type: "success",
					title: title,
					showCancelButton: true,
					showConfirmButton:false,
					cancelButtonText: this.$t("Exit"),
					cancelButtonColor: "#d33",
					showConfirmButton: false,
					time: 2500,               
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
					showConfirmButton: false,
					time: 2500,               
			};
		},
		//API
		GetArponHotel(){
			this.callApi = true;
			ArponService.GetHotelArponByIdIP(this.hotelId)
			.then(response =>{
				console.log(response);
				this.arponId = response.body.idHotelArpon;
				this.urlArpon = response.body.urlArpon;
				this.agencyArpon = response.body.agencyArpon;
				this.emails = response.body.eMailsToNotif;
				this.active = response.body.active;
				this.callApi = false;
			})
			.catch(error => {
				console.log(error);
			});	
		},
		SaveArponHotel() {
			this.showButton = false;
			this.callApi = true;

			const payload  = {
				IdHotelIp : this.hotelId,
				IdHotelArpon : this.arponId,
				UrlArpon : this.urlArpon,
				AgencyArpon: this.agencyArpon,
				Active : this.active,
				eMailsToNotif : this.emails
			}

			ArponService.SaveHotelArpon(this.hotelId, payload)
			.then(response => {
				console.log(response);
				this.callApi = false;
				this.showButton = true;
				this.$appAlert(this.success(this.$t("Hotel Saved")));
			})
			.catch(error =>{
				console.log(error);
				this.callApi = false;
				this.showButton = true;
				this.$appAlert(this.error(this.$t('System Error')));
			});

		}
	}
}
</script>