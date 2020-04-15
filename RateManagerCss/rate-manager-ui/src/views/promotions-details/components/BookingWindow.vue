<template>
    <b-card header="Booking Window">
        <b-row>
            <b-col md="8">
                <h5>{{ $t('Sale period') }}</h5>
                <b-row>
                    <b-col>
                        <b-form-group :label="$t('Initial date')">
                            <v-date-picker
                            v-model="initialDate"
                            class="form-control p-0"
                            :min-date="new Date()"
                            :popover="{ placement: 'bottom', visibility: 'click' }">
                            </v-date-picker>
                        </b-form-group>
                    </b-col>
                    <b-col>
                        <b-form-group :label="$t('Final date')">
                            <v-date-picker
                            v-model="finalDate"
                            class="form-control p-0"
                            :min-date="new Date()"
                            :popover="{ placement: 'bottom', visibility: 'click' }">
                            </v-date-picker>
                        </b-form-group>
                    </b-col>
                </b-row>
            </b-col>
            <b-col md="4">
                <b-form-checkbox v-model="notCancelable">especificar la hora</b-form-checkbox>
            </b-col>
        </b-row>
        <hr class="mb-1 mt-1" />
        <b-row>
            <b-col md="6">
                <h5>{{ $t('Days in advance') }}</h5>
                <b-row>
                    <b-col>
                        <b-form-group :label="$t('Min. days')">
                            <b-form-input></b-form-input>
                        </b-form-group>
                    </b-col>
                    <b-col>
                        <b-form-group :label="$t('Max. days')">
                            <b-form-input></b-form-input>
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
            finalDate: null
        }
    }
}
</script>