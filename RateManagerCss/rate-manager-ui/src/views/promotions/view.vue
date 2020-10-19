<template>
    <div id="app">
        <b-container fluid>
            <h2 class="text-primary">{{ $t('Promotions') }}</h2>
            <div class="d-flex justify-content-end"><b-button variant="primary">{{$t('New')}}</b-button></div>
            <b-card no-body  style="border:0px">
                <b-row>
                    <!-- -->
                    <b-col md="3" class="my-1">
                        <b-form-checkbox
                        id="idCheckboxOldPromotions"
                        name="checkboxOldPromotions"
                        v-model="checkboxOldPromotions"
                        >
                        {{$t('Include Old Promotions')}}
                        </b-form-checkbox>
                        <b-form-select class="mb-3" v-model="selected" :options="options"></b-form-select>
                        {{selected}}
                    </b-col>
                    <!--  -->
                    <b-col md="3" class="my-1">
                        <b-row>
                            <b-col md="3">
                               
                            </b-col>
                            <b-col md="9">
                            
                            </b-col>
                        </b-row>
                    </b-col>
                    <!-- -->
                    <b-col md="6" class="" style="margin-top:1.1rem;">
                        <b-form-input placeholder="Enter your name"></b-form-input>
                        <b-form-group>
                            <b-form-radio-group id="radio-group-2" name="radio-sub-component">
                                <b-form-radio value="first">Toggle this custom radio</b-form-radio>
                                <b-form-radio value="second">Or toggle this other custom radio</b-form-radio>
                            </b-form-radio-group>
                        </b-form-group>
                    </b-col>
                </b-row>
            </b-card>
            <b-table striped hover :items="items"></b-table>
        </b-container>
    </div>
</template>

<script>
export default {
    data() {
        return {
            //Checkbox old promotions
            checkboxOldPromotions:false,
            //Options Select
            options:[
                {value : 1, text: $t('Active') },
                {value : 0, text: $t('InActive')},
                {value : -1, text: $t('Active and InActive')},
            ],
            //Selected Option Default Active = 1
            selected: 1,
            items: [
                { age: 40, first_name: 'Dickerson', last_name: 'Macdonald' },
                { age: 21, first_name: 'Larsen', last_name: 'Shaw' },
                { age: 89, first_name: 'Geneva', last_name: 'Wilson' },
                { age: 38, first_name: 'Jami', last_name: 'Carney' }
            ]
        }
    }
}
</script>

<style>

</style>