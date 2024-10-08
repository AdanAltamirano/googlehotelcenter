<template>
	<div>
		<h5 class="text-info" style="cursor:pointer" v-b-toggle.ratesplanCollapse>
			<i class="fa fa-plus-circle"></i>
			{{$t('Ratesplan')}}
		</h5>
		<div v-if="loading">
			<div class="vld-parent" style="width:100px;height:100px;">
				<loading style="display:block !important;" :active="true" :is-full-page="false" color="#007bff"></loading>
			</div>
		</div>
		<div v-else>
			<b-collapse visible id="ratesplanCollapse">
					<table class="fixed_header" style="border-collapse:separate !important; border-spacing:1px !important;">
						<thead>
							<tr>
								<th>{{$t('Code')}} Internet Power</th>
								<th>{{$t('Code')}} Arpon</th>
							</tr>
						</thead>
						<tbody>
							<tr class="tr-config" v-for="rateplan in ratePlans" :key="rateplan">
								<td>{{rateplan.idRatePlanIp}}</td>
								<td>
									<b-form-input v-model="rateplan.idRatePlanArpon" ></b-form-input>
								</td>
							</tr>
						</tbody>
					</table>
					<div v-if="callApi" class="vld-parent" style="width:70px;height:70px;">
						<loading style="display:block !important;" :active="true" :is-full-page="false" color="#007bff"></loading>
					</div>
					<div v-else>
						<b-button v-if="showButton" variant="primary" @click="SaveRatePlansArpon">{{$t('Save')}}</b-button>
					</div> 
			</b-collapse>
		</div>
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
		this.GetRatePlansArpon();
	},
	data(){
		return {
			hotelId: this.$appConfig.session.hotelId,
			ratePlans: null,
			callApi:false,
			showButton:true,
			loading:false
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
		GetRatePlansArpon(){
			this.loading = true;
			ArponService.GetRatePlansByHotelId(this.hotelId)
			.then(response =>{
				console.log(response);
				this.ratePlans = response.body;
				this.loading = false;
			})
			.catch(error =>{
				console.log(error);
			});
		},
		SaveRatePlansArpon(){
			this.showButton = false;
			this.callApi = true;

			ArponService.SaveRatePlansArpon(this.hotelId,this.ratePlans)
			.then(response => {
				console.log(response);
				this.callApi = false;
				this.showButton = true;
				this.$appAlert(this.success(this.$t("Rateplans Saved")));
			})
			.catch(error => {
				console.log(error);
				this.callApi = false;
				this.showButton = true;
				this.$appAlert(this.error(this.$t('System Error')));
			});
		}
	}
}
</script>