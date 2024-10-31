<template>
  <div>
    <h5 class="text-info" style="cursor:pointer" v-b-toggle.itemCollapse>
      <i class="fa fa-plus-circle"></i>
      {{$t('Extras')}}
    </h5>
    <b-collapse visible id="itemCollapse">
      <div v-for="item in items" :key="item" class="card card-accent-primary mt-2">
        <div class="card-header">
          <h6>{{item.name}}</h6>       
        </div>
        <div class="card-body">
         <!-- 1st row -->
          <b-row>
            <div class="padding-l-15">
              <b-img
                style="height:100px;"
                :src="item.img"
                @error="(e) => {e.target.src=DefaultImage}"
                fluid-grow
              ></b-img>
            </div>          
          </b-row>
          <!-- 2nd row -->
          <b-row>
            <b-col>
              <div>
                <div class="color-212529">
                  {{$t('Description')}}: <strong>{{item.description}}</strong>
                </div>
                <div class="color-212529">
                  {{$t('Quantity')}}: <strong>{{item.quantity}}</strong>
                </div>
                <div class="color-212529">
                  {{$t('Price')}}: <strong>{{item.price | currency}} {{item.code}}</strong>
                </div>
                <div class="color-212529">
                  {{$t('Total')}}: <strong>{{item.total | currency}} {{item.code}}</strong>
                </div>
                <div v-if="item.allowPaymentDestination" class="alert alert-info">
                  {{$t('This item must be charged upon arrival of the guest')}}
                </div>
              </div>
            </b-col>
          </b-row>
        </div>
      </div>
    </b-collapse>
  </div>
</template>
<script>
import image from "../assets/extras.jpeg";
export default {
  name: 'hotel-items',
  props:{
    items: {
      required: false,
      type:Array
    }
  },
  data (){
    return {
      image: image,
    }
  },
  computed :{
    DefaultImage() {
      return this.image;
    },
  }
}
</script>
