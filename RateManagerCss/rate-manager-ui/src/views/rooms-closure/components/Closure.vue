<template>
  <div class="ml-3 mr-3 mt-3">
    <!-- Plan Tarifario -->
    <div class="d-flex border dark-gray-created icon" v-b-toggle="'collapse-' + rateRooms.ratePlan">
      <div class="d-flex w-30 align-items-center">
        <div class="border-right p-1 flex-fill d-flex w-80 justify-content-between">
          <h5 class="font-weight-bold m-0 text-truncate" v-tooltip="{content : rateRooms.ratePlan}" style="font-size:12px;">
            <i class="fas fa-receipt mr-1"></i> {{rateRooms.ratePlan}}       
          </h5>                 
        </div>
      </div>
    </div>
    <!-- Habitaciones -->
    <div v-for="(room,index) in rateRooms.codeRoomModelsList" :key="index">
      <b-collapse :id="'collapse-' + rateRooms.ratePlan">
       <div class="d-flex bg-white border-5 border-primary">
         <!-- Nombre de la Habitacion -->
          <div class="d=flex w-30">
            <div class="border flex-fill d-flex w-100 align-items-center justify-content-between p-1 pl-2">
              <h5 class="m-0 pl-2 w-100 text-truncate text-muted fs-12" >
                <i class="fa fa-bed mr-1"></i>
                  <span v-tooltip="{content:room.code  + ' - ' + room.roomName}">{{room.code}} - {{room.roomName}}</span>           
              </h5>                     
            </div>
          </div>
          <!-- Status de la Habitacion -->
          <div class="d-flex w-70">
            <div v-for="(status,id) in room.status" :key="id" class="border p-1 flex-fill text-center">                 
                <h5 class="m-0 text-truncate text-muted fs-12" 
                :class="{'open-color': status === 'O','close-color' : status === 'C', 'no-arrivals-color' : status === 'N'}">                    
                  <span v-tooltip="{content : SetStatusToolTip(status)}" style="color:white;">{{SetStatus(status)}}</span>           
                </h5>                         
            </div>
          </div>
       </div>
      </b-collapse>
    </div>
  </div>
</template>
<script>
export default {
  name: 'closure',
  props:{
    rateRooms:{
      type : Object,
      required: true
    }
  },
  methods:{
    //ToolTip
    SetStatusToolTip(status){
      switch(status){
        case 'O':
          return this.$t('Open');
          break;
        
        case 'C':
          return this.$t('Close');
          break;

        case 'N':
          return this.$t('No Arrivals');
          break;
      }
    },
    //Text
    SetStatus(status){
       switch(status){
        case 'O':
          return this.$t('O');
          break;
        
        case 'C':
          return this.$t('C');
          break;

        case 'N':
          return this.$t('N');
          break;
      }
    }
  }
}
</script>