<template>
<h2>{{dayRate.rateId}}</h2>
</template>
<script>
export default {
    name: 'day-rate-detail',
    props: {
        dayRate: {
            type: Object,
            required: false,
        },
    },
};
</script>
