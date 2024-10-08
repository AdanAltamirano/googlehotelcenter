<template>
	<b-table-simple caption-top class="mt-2" >
		<caption style="color:#dc3545 !important"><b>{{$t('Only data that have been updated are displayed')}}</b></caption>
		 <b-thead>
			<b-tr>
				<b-th colspan="2"></b-th>
        <b-th colspan="2">Datos Antes</b-th>
        <b-th colspan="3">Datos Despues</b-th>
      </b-tr>
		 </b-thead>
		 <b-tbody>
			<!-- Customer -->
			 <b-tr v-if="logDetail.customerBefore !== undefined && logDetail.customerAfter !== undefined">
        <b-th colspan="2" class="font-size-2">{{$t('Name')}}</b-th>
        <b-th colspan="2" class="font-size-2">{{logDetail.customerBefore.fullName}}</b-th>
        <b-th colspan="3" class="font-size-2">{{logDetail.customerAfter.fullName}}</b-th>
      </b-tr>
			<!-- Booking Date -->
			<b-tr v-if="logDetail.checkDatesBefore !== undefined && logDetail.checkDatesAfter !== undefined">
				<b-th colspan="2">
					<div class="d-flex flex-column font-size-2">
						<div>{{$t('Check In')}}</div>
						<div>{{$t('Check Out')}}</div>
					</div>
				</b-th>
        <b-th colspan="2">
					<div class="d-flex flex-column font-size-2">
						<div>{{$moment(logDetail.checkDatesBefore.checkIn).format('D MMMM YYYY')}} </div>
						<div>{{$moment(logDetail.checkDatesBefore.checkOut).format('D MMMM YYYY')}}</div>
					</div>					
				</b-th>
        <b-th colspan="3">
					<div class="d-flex flex-column font-size-2">
						<div>{{$moment(logDetail.checkDatesAfter.checkIn).format('D MMMM YYYY')}} </div>
						<div>{{$moment(logDetail.checkDatesAfter.checkOut).format('D MMMM YYYY')}}</div>
					</div> 
				</b-th>
			</b-tr>
			<!-- Rooms -->
			<b-tr v-if="logDetail.roomDetailsBefore.length> 0 && logDetail.roomDetailsAfter.length > 0">
				<b-th colspan="2" class="font-size-2">{{$t('Rooms')}}</b-th>
				<b-th colspan="2" class="font-size-2"><rooms-log :roomDetailsLog="logDetail.roomDetailsBefore"></rooms-log></b-th>
				<b-th colspan="3" class="font-size-2"><rooms-log :roomDetailsLog="logDetail.roomDetailsAfter"></rooms-log></b-th>
			</b-tr>
			<!-- Total -->
			<b-tr v-if="logDetail.totalDetailsBefore !== undefined && logDetail.totalDetailsAfter !== undefined">
				<b-th colspan="2" class="font-size-2">{{$t('Total Booking')}}</b-th>
				<b-th colspan="2" class="font-size-2"><total-log :totalDetailsLog="logDetail.totalDetailsBefore"></total-log></b-th>
				<b-th colspan="3" class="font-size-2"><total-log :totalDetailsLog="logDetail.totalDetailsAfter"></total-log></b-th>
			</b-tr>
		 </b-tbody>
	</b-table-simple>
</template>

<script>
import RoomsLog from './Rooms.vue';
import TotalLog from './Total.vue';
export default {
	components: {
		RoomsLog,
		TotalLog
	},
	props: {
		logDetail:{
			require:true
		}
	},
	data(){
		return {

		}
	},
	created(){
		console.log(this.logDetail);
	},
	methods :{

	}
};
</script>