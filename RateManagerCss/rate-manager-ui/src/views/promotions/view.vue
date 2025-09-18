<template>
    <div id="app">
        <b-container fluid>
            <h2 class="text-primary">{{ $t('Promotions') }}</h2>
            <div class="d-flex justify-content-end"><b-button variant="primary" :href="$appConfig.basePath + '/rate-manager-ui/dist/promotions-details.aspx'">{{$t('New')}}</b-button></div>
            <b-card no-body  style="border:0px">
                <b-row>
                    <!-- -->
                    <b-col md="3" class="my-1">
                        <b-form-checkbox
                        id="idCheckboxOldPromotions"
                        name="checkboxOldPromotions"
                        v-model="checkboxOldPromotions"
                        value="1"
                        unchecked-value="0"
                        style="margin-bottom:0.5rem;">
                        {{$t('Include Old Promotions')}}
                        </b-form-checkbox>
                        <b-form-select class="mb-3" v-model="selected">
                            <b-form-select-option value="1">{{$t('Active')}}</b-form-select-option>
                            <b-form-select-option value="0">{{$t('InActive')}}</b-form-select-option>
                            <b-form-select-option value="-1">{{$t('Active and InActive')}}</b-form-select-option>
                        </b-form-select>
                    </b-col>
                    <!--  -->
                    <b-col offset="5" md="4" class="" style="margin-top:2.2rem;">
                        <b-form-input v-model="inputValue"></b-form-input>                             
                        <b-form-radio-group id="idRadioTypeSearch" name="radioTypeSearch" v-model="selectedRadio" style="float:right; margin-top:0.5rem;">
                            <label class="mr-3">{{$t('Search By')}} :</label>
                            <b-form-radio value="2">{{$t('Name')}}</b-form-radio>
                            <b-form-radio value="3">{{$t('Code')}}</b-form-radio>
                            <b-button variant="primary" @click="search">{{$t('Search')}}</b-button>
                        </b-form-radio-group>                                          
                    </b-col>
                </b-row>
            </b-card>
            <!-- -->
          <data-table 
          table-id="promo_table"
          :columns="fields"
          :resource-function="get"
          :items-per-page="itemPerPage"
          :filter="filter_url"
          sortBy="EndDate"
          :small="true"
          :customClass="`mt-3`"
          :rowClass="setColor"
          :tableTypeResults="`promociones`">
            <!--Custom Description -->
            <template v-slot:cell(description)="data">
                {{setDescription(data.item)}}
            </template>
            <!--Custom Edit -->
            <template v-slot:cell(edit)="data">
                 <b-link
                    :href="$appConfig.basePath + '/rate-manager-ui/dist/promotions-details.aspx?qs=' + data.item.promotionCode + '&edit=1'">
                    {{$t('Edit')}}
                </b-link>
            </template>
            <!--Custom Active -->
            <template v-slot:cell(active)="data">
                <b-button v-if="data.item.active == 1" variant="link" @click="disable(data.item.hotelId,data.item.promotionCode)">{{$t('Disable')}}</b-button>
                <b-button v-else-if="data.item.active == 0" variant="link" @click="enable(data.item.hotelId,data.item.promotionCode)">{{$t('Enable')}}</b-button>
            </template>
            <template v-slot:cell(clone)="data">
                <b-link
                    :href="$appConfig.basePath + '/rate-manager-ui/dist/promotions-details.aspx?qs=' + data.item.promotionCode + '&clone=1'">
                    {{$t('Clone')}}
                </b-link>
            </template>
          </data-table>
        </b-container>
    </div>
</template>

<script>
import DataTable from '../../components/data-table.vue';
import PromotionService from '../../api/offers-service';
export default {
    name:'promotions',
    components:{
        DataTable
    },
    data() {
        return {
            //Hotel Id
            hotelId: this.$appConfig.session.hotelId,
            //Checkbox old promotions
            checkboxOldPromotions:'0',
            //Selected Option Default Active = 1
            selected: 1,
            //Select Option Type Search Default Name = 2
            selectedRadio: '2',
            //Input Search Value
            inputValue:'',
            //Table Array Result
            result:[],
            //Fields Table
            fields:[
                {
                    key:'promotionCode',
                    label:this.$t('Code')
                },
                {
                    key:'description',
                    label:this.$t('Name')
                },
                {
                    key:'startDate',
                    label:this.$t('Start'),
                    formatter: value => this.$moment(value).format('D MMM YYYY'),
                },
                {
                    key:'endDate',
                    label:this.$t('End'),
                    formatter: value => this.$moment(value).format('D MMM YYYY'),
                },
                {
                    key:'edit',
                    label:''
                },
                {
                    key:'active',
                    label:''
                },
                {
                    key:'clone',
                    label:''
                }
            ],
            //Table Per Page
            itemPerPage: 10,
            //Options Table Item Per Page
            itemsPerPage: [20, 50, 100, 200],
            //
            filter_url: null
        }
    },
    created(){
        this.filter_url = this.defaultSearch();
        console.log(this.$appConfig);
    },
    watch:{
        //Works as OnChange
        selected:function(val){
            //Base Filter 
            this.filter_url = `HotelId eq ${this.hotelId}`;
            //Filter By Active and InActive When Condition Is True OtherWise Filter By Active Or InActive
            this.filter_url += (val === -1 || val === '-1')? '' : ` and Active eq ${val}`;
            //Include Old Promotions When Condition Is True Otherwise Doesn't Include Old Promotions
            this.filter_url += (this.checkboxOldPromotions === 1 || this.checkboxOldPromotions === '1') ? '' : ` and IsOldPromotion eq ${this.checkboxOldPromotions}`;
            //Include Promotion Code or Name If Input Is Not Empty
            this.filter_url += (!this.inputValue) ? '': (this.selectedRadio === 2 || this.selectedRadio === '2')? ` and Description lk ${this.inputValue}`: ` and PromotionCode lk ${this.inputValue}`;
            //Update Table
            this.$root.$emit('bv::refresh::table', 'promo_table'); 
        },
        checkboxOldPromotions:function(val){
            // Base Filter
            this.filter_url =  `HotelId eq ${this.hotelId}`;
            //Include Old Promotions When Condition Is Not True Otherwise Doesn't Include Old Promotions
            this.filter_url += (val === 0 || val === '0')? ` and IsOldPromotion eq ${val}` : '';
            //Filter By Active and InActive When Condition Is True OtherWise Filter By Active Or InActive
            this.filter_url += (this.selected === -1 || this.selected === '-1')? '' : ` and Active eq ${this.selected}`;
            //Include Promotion Code or Name If Input Is Not Empty
            this.filter_url += (!this.inputValue) ? '': (this.selectedRadio === 2 || this.selectedRadio === '2')? ` and Description lk ${this.inputValue}`: ` and PromotionCode lk ${this.inputValue}`;
            //Update Table
            this.$root.$emit('bv::refresh::table', 'promo_table');
        }
    },
    methods:{
        //Set Description
        setDescription(data){
            console.log(data)
            if(this.$appConfig.language === 'es')
                return data.descriptionEs;
            
            return data.descriptionEn;
        },
        // Api Request
        get(filter, orderBy, pageSize, page){
            return PromotionService.getByHotelId(filter, orderBy, pageSize, page);
        },
        // Filter Default For Query String
        defaultSearch(){
            const def = `HotelId eq ${this.hotelId} and Active eq ${this.selected} and IsOldPromotion eq ${this.checkboxOldPromotions}`
            return def;
        },
        //Search Event Button
        search(){
            //Base Filter 
            this.filter_url = `HotelId eq ${this.hotelId}`;
            //Filter By Active and InActive When Condition Is True OtherWise Filter By Active Or InActive
            this.filter_url += (this.selected === -1 || this.selected === '-1')? '' : ` and Active eq ${this.selected}`;
            //Include Old Promotions When Condition Is True Otherwise Doesn't Include Old Promotions
            this.filter_url += (this.checkboxOldPromotions === 1 || this.checkboxOldPromotions === '1') ? '' : ` and IsOldPromotion eq ${this.checkboxOldPromotions}`;
            //Include Promotion Code or Name If Input Is Not Empty
            this.filter_url += (!this.inputValue) ? '': (this.selectedRadio === 2 || this.selectedRadio === '2')? ` and Description lk ${this.inputValue}`: ` and PromotionCode lk ${this.inputValue}`;
            //Update Table
            this.$root.$emit('bv::refresh::table', 'promo_table'); 
        },
        //Set Color To Row If This One Is Not Active
        setColor(item,type){
            if(item && item.active == 0) return 'table-danger';
        },
        //Enable Promotion
        //TODO: Finish Alerts Enable and Disable and Refresh Table after Update
        enable(hotelId,code){
           this.$appAlert({
                type: "question",
                title: this.$t('Enable Promotion ?'),
                showCancelButton: true,
                cancelButtonColor: "#d33",
                showLoaderOnConfirm: true,
                confirmButtonText: this.$t('Enable'),
                confirmButtonColor: "#3085d6",
                cancelButtonText:this.$t('Cancel'),
                //Request Api
                preConfirm:() =>{
                    return PromotionService.enablePromotion(hotelId,code)
                    .then(response => {
                        //Callback Response Api
                        return response;
                    })
                    .catch(error => {
                        return error;
                    }) 
                },
                allowOutsideClick: () => !this.$swal.isLoading()
           })
           //Result of Callback
           .then(response => {
               if(response.value.ok)
               {
                   this.$appAlert(
                        {
                            type: "success",
                            title: this.$t('Promotion Enabled'),
                            showCloseButton: true,
                            onClose : () => {
                                //Update Table
                                this.$root.$emit('bv::refresh::table', 'promo_table'); 
                            }
                        }
                    );
               }
               else if(!response.value.ok){
                    this.$appAlert(
                        {
                            type: "error",
                            title: this.$t('Cannot Enable Promotion'),
                            showCloseButton: true,
                            
                        }
                    );
               }
           })
        },
        //Disable Promotion
        disable(hotelId,code){
            
            this.$appAlert({
                type: "question",
                title: this.$t('Disable Promotion ?'),
                showCancelButton: true,
                cancelButtonColor: "#d33",
                showLoaderOnConfirm: true,
                confirmButtonText: this.$t('Disable'),
                confirmButtonColor: "#3085d6",
                cancelButtonText: this.$t('Cancel'),
                // Request Api
                preConfirm:() =>{
                    return PromotionService.disablePromotion(hotelId,code)
                    .then(response => {
                        //Callback Response Api
                        return response;
                    })
                    .catch(error => {
                        return error;
                    }) 
                },
                allowOutsideClick: () => !this.$swal.isLoading()
            })
            //Result of Callback
            .then(response => {
                if(response.value.ok)
                {
                    this.$appAlert(
                        {
                            type: "success",
                            title: this.$t('Promotion Disabled'),
                            showCloseButton: true,
                            onClose : () => {
                                //Update Table
                                this.$root.$emit('bv::refresh::table', 'promo_table'); 
                            }
                        }
                    );
                }
                else if(!response.value.ok){
                    this.$appAlert(
                        {
                            type: "error",
                            title: this.$t('Cannot Disable Promotion'),
                            showCloseButton: true,
                            
                        }
                    );
                }
            });
         }
    }, 
    mounted(){
        this.$root.$on('table-result', val => {
            this.result = val;
            console.log(this.result)
        });
        console.log(this.result);
    }
}
</script>

<style>

</style>