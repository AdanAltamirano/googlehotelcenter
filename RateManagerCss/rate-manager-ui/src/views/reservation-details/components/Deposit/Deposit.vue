<template>
    <!-- eslint-disable -->
    <v-popover placement="right" :auto-hide="false">      
        <template>
            <a href="javascript:;"><i class="fas fa-pen"></i></a>
        </template>
        <template slot="popover">
            <div class="d-flex justify-content-between">
                <h5 class="text-primary font-weight-bold mb-2">{{$t('Confirmation Deposit')}}</h5>
                <a ref="close" v-close-popover href="javascript:;" class="text-danger"><i class="fa fa-times"></i></a>
            </div>
            <hr style="margin-top: 0rem; margin-bottom: 1rem; border-top: 1px solid rgba(0,0,0,.1);">
            <div id='popover-1'>
                <form @submit.stop.prevent="deposit">
                    <!-- 1st Row -->
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="input-account">{{$t('Bank Account')}}</label>
                            <b-form-input id="input-account" pattern="^[0-9]+" v-model="account" required></b-form-input>
                            <small class="form-text text-muted">{{$t('Only numbers')}}</small>
                        </div>
                    </div>
                    <!-- 2nd Row -->
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="input-date">{{$t('Date')}}</label>
                            <v-date-picker
                                v-model="date"
                                id="input-date"
                                :popover="{ placement: 'bottom', visibility: 'click' }"
                                class="form-control p-0">
                            </v-date-picker>
                        </div>
                    </div>
                    <!-- 3rd Row -->
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="input-bank">{{$t('Bank')}}</label>
                            <b-form-input id="input-bank" pattern="^[a-zA-Z\s]+" v-model="bank" required></b-form-input>
                            <small class="form-text text-muted">{{$t('Only letters')}}</small>
                        </div>
                    </div>
                    <!-- 4th Row -->
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="input-amount">{{$t('Amount to Deposit')}}</label>
                            <div class="d-flex col-gap-2">
                                <div>
                                    <b-form-input id="input-amount" class="text-right" type="number" step="0.01" v-model="amount" required></b-form-input>
                                    <small class="form-text text-muted">{{$t('Only two decimals')}}</small>
                                </div>
                                <b-form-select class="font-size-1 text-capitalize" v-model="selected" :options="options"></b-form-select>
                            </div>
                        </div>
                    </div>
                    <!-- 5th Row -->
                     <div class="form-row">
                        <div class="form-group col-md-12">
                            <label for="input-amount">{{$t('Details')}}</label>
                            <b-textarea v-model="details" :trim="true"></b-textarea>
                        </div>
                    </div>
                    <button type="submit" class="btn btn-success m-2"><i class="fa fa-save mr-2"></i> {{'Save' | translate}}</button>
                </form>                  
            </div>
        </template>
    </v-popover>
    <!-- eslint-enable -->
</template>

<script>
import ConfigService from '../../../../api/config-service';
import reservationService from '../../../../api/reservation-service';
import ReservationService from '../../../../api/reservation-service';

export default {
    props:['reservationId','reservationNumber','reference'],
    data () {
        return {
            account: '',
            date: new Date(),
            bank: '',
            amount: '',
            selected:'MXN',
            details: '',
            options: []
        }
    },
    created() {
       this.currencies();
    },
    methods: {
        async currencies() {
            const response = await ConfigService.GetCurrencies();
            const currenciesData = await response.json();

            this.options = currenciesData.map(currency => {

                const { name , code} = currency;
                const text =  [name,'-',code].join(' ');

                return {
                    value: currency.code, 
                    text: text
                }

            });

        },
        deposit() {
            const request = {
                userId : 0,
                reservationId: this.reservationId,
                reference: this.reservationNumber,
                authorizationNumber: this.reference,
                depositDate: this.$moment(this.date),
                numberAccount: this.account,
                bank: this.bank,
                amount: this.amount,
                currency: this.selected,
                details: this.details
            };

            this.$swal.fire({
                type: "info",
                title: this.$t("Save Deposit?"),
                showCancelButton: true,
                cancelButtonText: this.$t("Cancel"),
                cancelButtonColor: "#d33",
                confirmButtonColor: "#3085d6",
                confirmButtonText: this.$t("Save"),
                showLoaderOnConfirm: true,
                preConfirm: async () => {                   
                    const response = await ReservationService.ReservationDeposit(this.reservationId, request);
                    return await response.json();
                },
                allowOutsideClick: () => !this.$swal.isLoading(),

            }).then(result => {
                if(result.value.isSuccess) this.$swal.fire(this.success());
                else this.$swal.fire(this.error());
                
            });

        },
        success() {
            return {
                type: "success",
                title: this.$t("Deposit has been confirmed"),
                showCancelButton: true,
                showConfirmButton:false,
                cancelButtonText: this.$t("Exit"),
                cancelButtonColor: "#d33",
                onClose:() => {
                    window.location.reload();
                }
            };
        },
        error () {
            return {
                type: "error",
                title: this.$t('Deposit has not been confirmed'),
                showCancelButton: true,
                showConfirmButton:false,
                cancelButtonText: this.$t("Exit"),
                cancelButtonColor: "#d33",
            }
        }
    },
}
</script>
