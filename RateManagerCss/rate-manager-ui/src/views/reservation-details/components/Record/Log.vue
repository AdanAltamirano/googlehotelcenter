<template>
  <div>
    <b-table   
    show-empty
    :empty-text="emptyText"
    :busy.sync="isBusy"
    striped
    bordered 
    :fields="fields"
    :items="items"
    :per-page="itemsPerPage"
    :current-page="currentPage">
     <template #table-busy>
        <div class="vld-parent" style="height:200px">
          <loading :active="true" :is-full-page="false" color="#007bff"></loading>
        </div>
      </template>
      <template v-slot:cell(empty)="scope">
        <h4>{{scope.emptyText}}</h4>
      </template>
      <template v-slot:cell(action)="data">
      <!-- RateMangager -->
        <b-badge v-if="data.item.source == 'R' && (data.item.action == 0 || data.item.action == 7)" variant="success">{{ $t('Confirmation Deposit') }}</b-badge>
        <b-badge v-if="data.item.source == 'R' && data.item.action == 2" variant="danger">{{ $t('Cancellation') }}</b-badge>
        <b-badge v-if="data.item.source == 'R' && data.item.action == 6" variant="success">{{ $t('Reactivation') }}</b-badge>
        <b-badge v-if="data.item.source == 'R' && data.item.action == 1" variant="warning">{{ $t('Modification') }}</b-badge>
        <b-badge v-if="data.item.source == 'R' && data.item.action == 2" variant="danger">{{ $t('Cancellation') }}</b-badge>
        <b-badge v-if="data.item.source == 'R' && data.item.action == 6" variant="success">{{ $t('Reactivation') }}</b-badge>
      <!-- CallCenter -->
        <b-badge v-if="data.item.source == 'CC' && data.item.action == 6" variant="warning">{{ $t('Modification') }}</b-badge>
        <b-badge v-if="data.item.source == 'CC' && data.item.action == 3" variant="danger">{{ $t('Cancellation') }}</b-badge>
        <b-badge v-if="data.item.source == 'CC' && data.item.action == 4" variant="success">{{ $t('Reactivation') }}</b-badge>
      </template>
      <template v-slot:cell(source)="data">
        <b v-if="data.item.source == 'R'">RateManager</b>
        <b v-if="data.item.source == 'CC'">CallCenter</b>
      </template>
    </b-table>
    <div class="d-flex">
      <span v-if="showResults">{{$t('Showing')}} {{$t('page')}} {{currentPage}} {{$t('of')}} {{totalPages}}</span>
      <b-pagination
        align="right"
        style="margin-left:auto !important;"
        :total-rows="totalRows"
        :per-page="itemsPerPage"
        v-model="currentPage"
        class="my-0"
      />
    </div>
  </div>
</template>

<script>
import ReservationService from '../../../../api/reservation-service';
import Loading from "vue-loading-overlay";

export default {
  components:{
    Loading
  },
  props: {
    reservationNumber:{
      require:false
    }
  },
  data(){
    return {
      fields :[
        {
          key: 'date',
          label: this.$t('Date'),
          formatter: value => this.$moment(value).format('D MMM YYYY hh:mm a'),
        },
        {
          key: 'user',
          label: this.$t('User')
        },
        {
          key: 'action',
          label: this.$t('Action')
        },
        {
          key : 'source',
          label: this.$t('Modification Source')
        },
        {
          key: 'reason',
          label: this.$t('Reason')
        }
      ],
      items: [],
      itemsPerPage: 5,
      currentPage: 1,
      totalRows: 0,
      totalPages:0,
      isBusy: false,
      emptyText: this.$t("There are no records"),
      showResults:false

    }
  },
  created(){
    this.GetRecords();
  },
  methods: {
    GetRecords(){
      this.isBusy = true;
      ReservationService.GetHistoryLog(this.reservationNumber)
      .then (response => {
        this.items = response.body;
        this.totalRows = this.items.length;
        this.totalPages = Math.ceil(this.items.length / this.itemsPerPage);
        this.isBusy = false;

        if(this.totalRows > 0) showResults = true;

      });
    }
  }
}
</script>