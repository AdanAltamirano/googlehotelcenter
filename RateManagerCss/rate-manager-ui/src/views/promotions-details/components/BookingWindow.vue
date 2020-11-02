<template>
    <b-card header="Booking Window">
        <h6>{{ $t('Sale period') }}</h6>
        <b-row align-h="between">
            <b-col md="4">
                <b-form-group :label="$t('Initial date')">
                    <b-input-group>
                        <v-date-picker
                        v-model="model.startDate"
                        class="form-control p-0"
                        :min-date="new Date()"
                        :popover="{ placement: 'bottom', visibility: 'click' }">
                        </v-date-picker>

                        <template v-slot:prepend>
                            <b-input-group-text >
                                <i class="fa fa-calendar"></i>
                            </b-input-group-text>
                        </template>
                    </b-input-group>
                </b-form-group>
                <b-form-group class="mt-2" :label="$t('Final date')">
                    <b-input-group>
                        <v-date-picker
                        v-model="model.endDate"
                        class="form-control p-0"
                        :min-date="new Date()"
                        :popover="{ placement: 'bottom', visibility: 'click' }">
                        </v-date-picker>

                        <template v-slot:prepend>
                            <b-input-group-text >
                                <i class="fa fa-calendar"></i>
                            </b-input-group-text>
                        </template>
                    </b-input-group>                 
                </b-form-group>
            </b-col>
            <b-col md="auto" class="mr-auto ml-auto pr-1 pl-1">
                <b-form-checkbox class="mb-2" v-model="specifyTime">{{ $t('Specify time') }}</b-form-checkbox>
                <b-time locale="en" :disabled="!specifyTime" v-model="model.startHour"></b-time>
                &nbsp;:&nbsp;
                <b-time locale="en" :disabled="!specifyTime" v-model="model.endHour"></b-time>
            </b-col>
        </b-row>
        <hr class="mb-2 mt-2" />
        <b-row>
            <b-col md="6">
                <h6>{{ $t('Days in advance') }}</h6>
                <b-row>
                    <b-col>
                        <b-form-group class="mb-0" :label="$t('Min. days')">
                            <b-form-input v-model="model.minDays" min="1" type="number" />
                        </b-form-group>
                    </b-col>
                    <b-col>
                        <b-form-group class="mb-0" :label="$t('Max. days')">
                            <b-form-input v-model="model.maxDays" min="1" type="number" />
                        </b-form-group>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
export default {
    props: {
        dataModel: {
            type: Object,
            required: true
        }
    },
    created(){
        if (this.model.startDate)
            this.model.startDate = new Date(this.model.startDate);
        if (this.model.endDate)
            this.model.endDate = new Date(this.model.endDate);
        
        console.log(this.model);
    },
    data() {
        return {
            model: this.dataModel,
            specifyTime: false,
        }
    }
}
</script>