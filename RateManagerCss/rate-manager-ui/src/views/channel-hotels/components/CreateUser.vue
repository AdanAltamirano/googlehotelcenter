<template>
  <b-card>
    <div v-if="isLoading" style="height:100px; padding:2rem;">
      <loading :active="isLoading" :is-full-page="false" color="#007bff"></loading>
    </div>
    <b-row v-else-if="!isLoading">
      <b-form-group
        class="w-20-p"
        label-class="font-weight-bold"
        :label="label"
        :description="description">
        <b-form-input v-model="username" :placeholder="$t('Enter username')"></b-form-input>
      </b-form-group>
      <b-form-group 
        class="w-20-p ml-1">        
        <b-form-input class="mt-31-px" v-model="password" :placeholder="$t('Enter password')"></b-form-input>
      </b-form-group>
      <b-form-group class="ml-1">
        <b-button class="mt-31-px" variant="primary" @click="SaveUser()">{{$t('Save')}}</b-button>  
      </b-form-group>
    </b-row>
    <b-row class="mt-1" v-show="showAlert">
      <b-col md="12">
        <b-alert :show="success" variant="success" dismissible @dismissed="success=false">{{$t('User created')}}</b-alert>
        <b-alert :show="error" variant="danger" dismissible @dismissed="error=false">{{errorResponse}}</b-alert>
      </b-col>
    </b-row>   
  </b-card>
</template>

<script>
import Loading from "vue-loading-overlay";
import ConfluxService from '../../../api/conflux-service';

export default {
  components:{
    Loading
  },
  props:["isLoading","showAlert","success","error",
  "addToHotelPms","hotelId","companyId",
  "label","description"],
  data(){
    return {
      username:'',
      password:'',
      errorResponse:null
    }
  },
  methods: {
    SaveUser() {
      let payload = {
        companyId : this.companyId,
        hotelId : this.hotelId,
        userName: this.username,
        password: this.password,
        addToHotelPms: this.addToHotelPms
      }

      this.isLoading = true;
      this.showAlert = false;
      this.success = false;
      this.error = false;

      this.saveUser(payload);
    },
    ErrorMessage(errorCode){
      let errorMessage = null;

      switch(errorCode){
        case '500':
          errorMessage = this.$t('System Error');
          break;
        case '501':
          errorMessage = this.$t('User already exists')
          break;
      }

      return errorMessage;
    },
    //Api request
    saveUser(payload){
      ConfluxService.CreateUser(payload).then(response =>{
        console.log(response);
        this.isLoading = false;
        this.showAlert = true;
        this.success = true;
        this.error = false;
      })
      .catch(error => {
        
        let errorCode = error.body.errors[0].details[0].key;
        this.errorResponse = this.ErrorMessage(errorCode);
        console.log(errorCode)

        this.isLoading = false;
        this.showAlert = true;
        this.success = false;
        this.error = true;

      });
    }
  },
}
</script>
