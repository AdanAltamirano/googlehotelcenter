<template>
  <div id="app">
    <b-container fluid>
      <h3 class="text-primary">{{$t("Users")}}</h3>
       <data-table
          table-id="users_connectivities"
          :columns="fields"
          :resource-function="get"
          :items-per-page="itemPerPage"
          :filter="filter_url"
          sortBy="UserId"
          :small="false"
          :customClass="`mt-3`"
          :tableTypeResults="`usuarios`">
          <template v-slot:cell(typePms)="{item}">
            <span>{{ GetTypePms(item.typePms) }}</span>
          </template>
        </data-table>
      <b-link :href=" $appConfig.basePath + '/rate-manager-ui/dist/channel-hotels.aspx'">
       {{$t('Return')}}                       
      </b-link>  
    </b-container>
  </div>
</template>
<script>
import ConfluxService from '../../api/conflux-service';
import DataTable from '../../components/data-table.vue';
export default {
    components: {
    DataTable
  },
  created(){
    this.filter_url = this.DefaultSearch();
    console.log(this.filter_url);
  },
  data() {
    return {
      //Hotel Id
      hotelId: this.$appConfig.session.hotelId,
      filter_url: null,
      fields: [
        {
          key: 'user',
          label: this.$t('User')
        },
        {
          key: 'password',
          label: this.$t('Password')
        },
        {
          key: 'typePms',
          label: this.$t('User Type')
        }
      ],
      itemPerPage:10,    
      itemsPerPage: [20, 50, 100, 200],
    }
  },
  methods : {
    DefaultSearch (){
      return `HotelId eq ${this.hotelId}`;
    },
    GetTypePms(typePms) {
      let typeUser = ''
      switch(typePms){
        case 0:
          typeUser = ''
          break;
        case 1:
          typeUser = 'Pms'
          break;
          case 2:
            typeUser = this.$t('User Channel')
            break;
      }

      return typeUser;
    },
    //Api request
    get(filter, orderBy, pageSize, page){
      return ConfluxService.GetUsersConnectivities(filter, orderBy, pageSize, page);
    },
  }
}
</script>
