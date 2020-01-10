<template>
  <div>
    <b-table
      class="my-2"
      show-empty
      striped
      bordered
      hover
      responsive
      :fields="fields"
      :items="rates_hotels_list"
      :empty-text="empty"
    >
      <template slot="empty" slot-scope="scope">
        <h5 class="text-center">{{scope.emptyText}}</h5>
      </template>
    </b-table>
  </div>
</template>
<script>
export default {
  props: ["fields", "rates_hotels_list", "empty"]
};
</script>
