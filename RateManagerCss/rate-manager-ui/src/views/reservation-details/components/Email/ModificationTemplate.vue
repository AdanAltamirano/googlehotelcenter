<template>
  <div class="card card-accent-primary" style="text-align:left !important; width:inherit !important;">
    <div class="card-header">
      <h6>{{$t('An Email has been sent to the following users')}}</h6>
    </div>
    <div class="card-body">
      <address>
        {{$t('Client')}}:
        <strong v-if="clientEmail">{{clientEmail}}</strong>
        <strong v-else>{{$t('There was an error sending e-mail')}}</strong>
        <br>
        {{$t('Hotel')}}:
        <strong v-if="hotelEmail">{{hotelEmail}}</strong>
        <strong v-else>{{$t('There was an error sending e-mail')}}</strong>
      </address>
    </div>
  </div>
</template>
<script>
export default {
  name: "ModificationMessage",
  props:[
    "clientEmail",
    "hotelEmail"
  ]
};
</script>