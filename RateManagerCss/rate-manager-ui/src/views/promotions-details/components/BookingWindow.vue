<template>
    <b-card header="Booking Window">
        <h5>{{ $t('Sale period') }}</h5>
        <b-row align-h="between">
            <b-col md="4">
                <b-form-group :label="$t('Initial date')">
                    <b-input-group>
                        <v-date-picker
                        v-model="initialDate"
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
                        v-model="finalDate"
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
            <b-col md="auto" class="mr-auto ml-auto">
                <b-form-checkbox class="mb-2" v-model="specifyTime">{{ $t('Specify time') }}</b-form-checkbox>
                <b-time locale="en" :disabled="!specifyTime" v-model="time_from"></b-time>
                &nbsp;:&nbsp;
                <b-time locale="en" :disabled="!specifyTime" v-model="time_to"></b-time>
            </b-col>
        </b-row>
        <hr class="mb-2 mt-2" />
        <b-row>
            <b-col md="6">
                <h5>{{ $t('Days in advance') }}</h5>
                <b-row>
                    <b-col>
                        <b-form-group :label="$t('Min. days')">
                            <b-form-input v-model="minDays" min="1" type="number"></b-form-input>
                        </b-form-group>
                    </b-col>
                    <b-col>
                        <b-form-group :label="$t('Max. days')">
                            <b-form-input v-model="maxDays" min="1" type="number"></b-form-input>
                        </b-form-group>
                    </b-col>
                </b-row>
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
export default {
    name: 'booking-window',
    data() {
        return {
            initialDate: null,
            finalDate: null,
            minDays: 1,
            maxDays: 1,
            specifyTime: false,
            time_from: '',
            time_to: ''
        }
    }
}
</script>