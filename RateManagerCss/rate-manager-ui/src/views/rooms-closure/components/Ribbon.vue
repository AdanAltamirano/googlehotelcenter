<template>
<div class="d-flex w-70 two-weeks">
  <div v-for="(day, idx) in dates" :key="idx"  class="border p-1 flex-fill text-center">
    <span>{{day.format('ddd')}}</span> <!-- Dia de la Semana -->
    <h3 class="font-weight-bold mt-0 mb-0 fs-17">{{day.format('DD')}}</h3> <!-- Dia del Mes -->
    <span class="text-uppercase fs-15">{{day.format('MMM')}}</span> <!-- Mes -->
  </div>
</div>
</template>

<script>
export default {
  name:'ribbon',
  props:{
    startDate:{
      type:Object,
      required:true
    },
    endDate:{
      type:Object,
      required:true
    }
  },
  computed:{
    dates() {
      if (!this.startDate || !this.endDate) return [];

      // let diffTime = this.endDate.getTime() - this.startDate.getTime();
      
      // let diffDays = diffTime / (1000 * 3600 * 24);
      
      console.log(this.startDate);
      console.log(this.endDate);

      return Array(7).fill(0).map((v,i) =>this.$moment(this.startDate).add(i,'days'));

      // return Array(1 + this.$moment(this.endDate).diff(this.$moment(this.startDate),'days')).fill(0)
      //   .map((v, i) => this.$moment(this.startDate).clone().add(i, 'days'));
    },
  }
}
</script>

<style>

</style>