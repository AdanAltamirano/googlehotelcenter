<template>
    <div>
        <p v-html="fontWeight()"></p>
        <p>{{ $t('Assuming that in the promotion it was configured with a 40% discount and in the rate it was configured with a 20% discount, it would be as follows:') }}</p>
        <h4>{{ $t('Priority to discount rate') }}</h4>
        <p>{{ $t('Only 20% discount will be applied.') }}</p>
        <h4>{{ $t('Sum discount percentage') }}</h4>
        <p>{{ $t('A 60% discount will be applied.') }}</p>
        <h4>{{ $t('Additional discount') }}</h4>
        <p>{{ $t('First the 40% discount will be applied and the result of that will be applied the 20% discount.') }}</p>
    </div>
</template>

<script>
export default {
    name: 'help',
    methods: {
        fontWeight() {
            let translate = this.$t('The {appmode} will only affect when there is a discount at the tariff level. Otherwise, the discount percentage set here will be applied.');
            translate = translate.replace('{appmode}', `<b>${this.$t('Application mode')}</b>`);
            return translate;
        }
    }
}
</script>