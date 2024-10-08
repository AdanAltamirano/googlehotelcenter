<template>
	<div>
		<h6 style="cursor:pointer; color:#10467a;" v-b-toggle.roomsLog>
			<i class="fa fa-plus-circle"></i> {{$t('Rooms')}}
		</h6>
		<b-collapse id="roomsLog">
			<div v-for="(roomDetailLog,index) in roomDetailsLog" :key="index">
				<div class="custom-border-card-primary mt-1">
					<div class="d-flex flex-column">
						<div class="font-size-2">{{$t('Room')+ ' '+ (index + 1)}}</div>
						<div v-if="roomDetailLog.adults > 0" :id="'adults-' +index" class="font-size-3">{{$t('Adult(s)')}}: {{roomDetailLog.adults}}</div>
						<div v-if="roomDetailLog.childrens > 0" :id="'children-' +index" class="font-size-3">{{$t('Children')}}: {{roomDetailLog.childrens}}</div>
						<div v-if="roomDetailLog.extraAdults > 0" :id="'extraAdults-' +index" class="font-size-3">{{$t('Extra Adult(s)')}}: {{roomDetailLog.extraAdults}}</div>
						<div v-if="roomDetailLog.extraChildrens > 0" :id="'extraChildren-' +index" class="font-size-3">{{$t('Extra Children')}}: {{roomDetailLog.extraChildrens}}</div>
						<div v-if="roomDetailLog.agesChildren.length > 0" :id="'agesChildren-' +index" class="font-size-3">{{$t('Children Age')}}: {{roomDetailLog.ageChildren}}</div>
						<div v-if="roomDetailLog.total > 0" :id="'total-' +index" class="font-size-3">{{$t('Total')}}: {{roomDetailLog.total | currency}} {{roomDetailLog.currency}}</div>						
						<div v-if="roomDetailLog.priceDetails !== undefined && roomDetailLog.priceDetails.length > 0" style="cursor:pointer" v-b-toggle="'rates-' + index" class="mt-1 font-size-3 text-muted">
							<i class="fa fa-plus-circle"></i> {{$t('Prices')}}
						</div>
						<b-collapse :id="'rates-' + index">
							<div v-for="(rate,indexRate) in roomDetailLog.priceDetails" :key="indexRate">
								<div class="custom-border-card-primary">
									<div class="font-size-3">
										{{$t('Date')}}: {{$moment(rate.checkIn).format('D MMM')}}<span v-if="rate.checkIn != rate.checkOut"> - {{$moment(rate.checkOut).format('D MMM')}}</span> 								
									</div> 
									<div class="font-size-3">{{$t('Price')}}: {{rate.price | currency}} {{rate.currency}}</div>
									<div v-if="rate.extraPrice > 0" class="font-size-3">{{$t('Extra Price')}}: {{rate.extraPrice | currency}} {{rate.currency}}</div>
								</div>
							</div>
						</b-collapse>
					</div>
				</div>
			</div>					
		</b-collapse>
	</div>
</template>

<script>
export default {
	props:{
		roomDetailsLog:{
			require:false
		}
	},
	created(){
		console.log(this.roomDetailsLog);
	}
}
</script>