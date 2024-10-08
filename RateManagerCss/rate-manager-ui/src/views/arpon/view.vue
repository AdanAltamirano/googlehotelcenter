<template>
	<div id="app">
		<b-container fluid>
			<h1 class="text-primary">{{ $t('Configuration mapping with Arpon service') }}</h1>
			<!-- Hotel -->
			<hotel></hotel>
			<!-- Rateplans -->
			<rateplans></rateplans>
			<!-- Rooms -->
			<rooms></rooms>
		</b-container>
	</div>
</template>

<script>
import Hotel from "./components/Hotel.vue";
import Rateplans from "./components/Rateplans.vue";
import Rooms from "./components/Rooms.vue";

export default {
	components:{
		Hotel,
		Rateplans,
		Rooms
	},
	created(){

	},
	data(){
		return {

		}
	},
	mounted(){

	},
	methods:{

	}
}
</script>