<template>
	<div>
		<div class="font-size-2">{{totalDetailsLog.total | currency}} {{totalDetailsLog.currency}}</div>
	</div>
</template>

<script>
export default {
	props:{
		totalDetailsLog :{
			require:false
		}
	},
	created(){
		console.log(this.totalDetailsLog);
	}
}
</script>

