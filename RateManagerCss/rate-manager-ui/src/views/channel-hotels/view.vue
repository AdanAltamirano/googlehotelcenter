<template>
  <div id="app">
    <b-container fluid>
        <h3 class="text-primary">{{$t("Registered Hotels")}}</h3>
        <b-card no-body  style="border:0px" class="mt-4">
          <b-row>
            <b-col md="4" class="my-1">
              <b-form-group :description="$t('Search by hotel name')">
                  <b-form-input type="text" v-model="hotelName" :placeholder="$t('Hotel name')"></b-form-input>               
              </b-form-group>
            </b-col>
            <b-col md="4" class="my-1">
              <b-button variant="primary" @click="Search">{{$t("Search")}}</b-button>
            </b-col>
            <!-- <b-col offset-md="2" md="2" class="my-1">
              <b-button variant="outline-primary" @click="Refresh">
                <i class="fas fa-redo-alt"></i>
                {{$t("Refresh table")}}
              </b-button>
            </b-col> -->
          </b-row>
        </b-card>
        <!-- data-table -->
        <data-table
          table-id="hotels_uv"
          :columns="fields"
          :resource-function="get"
          :items-per-page="itemPerPage"
          :filter="filter_url"
          sortBy="Id"
          :small="false"
          :customClass="`mt-3`"
          :tableTypeResults="`hoteles`"
          :newProperties="properties">
          <!-- Custom User Channel -->
          <template v-slot:cell(userChannel)="{item}">       
            <b-button variant="link" @click="ToggleRowDetails(item,true,false)">{{$t('Add User Channel')}}</b-button>
          </template>
          <!-- Custom User PMS -->
          <template v-slot:cell(userPms)="{item}">       
            <b-button variant="link" @click="ToggleRowDetails(item,false,true)">{{$t('Add User Pms')}}</b-button>
          </template>

          <template v-slot:row-details="{item}">
            <template v-if="item.isChannelUser">
              <create-user
                :label="$t('User Channel')"
                :description="$t('User to do requests')" 
                :companyId="item.companyId"
                :hotelId="item.id"
                :isLoading="item.isLoading"
                :showAlert="item.showAlert"
                :success="item.success"
                :error="item.error"
                :addToHotelPms="true">
              </create-user>
            </template>
            <template v-else-if="item.isPmsUser">
              <create-user
                :label="$t('User Pms')"
                :description="$t('User to administrate')" 
                :companyId="item.companyId"
                :hotelId="item.id"
                :isLoading="item.isLoading"
                :showAlert="item.showAlert"
                :success="item.success"
                :error="item.error"
                :addToHotelPms="false">
              </create-user>
            </template>
          </template> 
        </data-table>
    </b-container>
  </div>
</template>

<script>
import ConfluxService from '../../api/conflux-service';
import DataTable from '../../components/data-table.vue';
import CreateUser from './components/CreateUser.vue'
export default {
  components: {
    DataTable,
    CreateUser
  },
    created(){
      this.filter_url = this.DefaultSearch();
    },
    data (){
      return {
        //Hotel Id
        hotelId: this.$appConfig.session.hotelId,
        hotelName: '',
        fields:[
          {
            key:'name',
            label: this.$t('Name')
          },
          {
            key:'id',
            label: 'Hotel Id'
          },
          {
            key:'companyId',
            label: this.$t('Company Id')
          },
          {
            key: 'corp',
            label: this.$t('Corporate')
          },
          {
            key:'userChannel',
            label:''
          },
          {
            key:'userPms',
            label:''
          }
        ],
        itemPerPage:10,    
        itemsPerPage: [20, 50, 100, 200],            
        filter_url: null,
        properties:{isLoading:false,showAlert:false,success:false,error:false}
        
      }
    },
    methods:{
      DefaultSearch(){
        return ``;
      },
      Search(){
        this.filter_url = this.hotelName.length > 0 ? `Name lk ${this.hotelName}` : ``;
        this.$root.$emit('bv::refresh::table', 'hotels_uv'); 
      },
      ToggleRowDetails(row,isChannelUser,isPmsUser){

         if(row._showDetails){
          
          this.$set(row, '_showDetails', false);
          this.$set(row, 'isChannelUser', false);
          this.$set(row, 'isPmsUser', false);

          if(row.lastOpened == 'channel' && !isChannelUser && isPmsUser){
            this.SetNextUserUI(row, isChannelUser, isPmsUser);
          }
          else if(row.lastOpened == 'pms' && isChannelUser && !isPmsUser){
              this.SetNextUserUI(row,isChannelUser,isPmsUser)
          }

        }else{
          this.SetNextUserUI(row, isChannelUser, isPmsUser);
        }
      },
      SetNextUserUI(row, isChannelUser, isPmsUser){
        let lastOpened = '';
        
        if(isChannelUser) lastOpened = 'channel';
        else if(isPmsUser) lastOpened = 'pms';


        this.$nextTick(() => {
          this.$set(row, '_showDetails', true);
          this.$set(row, 'isChannelUser', isChannelUser);
          this.$set(row, 'isPmsUser', isPmsUser);
          this.$set(row, 'lastOpened', lastOpened);
        })
      },
      //Api request
      get(filter, orderBy, pageSize, page){
        return ConfluxService.GetHotels(filter, orderBy, pageSize, page);
      },
    },
}
</script>
            
