<template>
    <b-card :header="$t('Restrictions')">
        <b-row>
            <b-col md="2">
                <b-form-group :label="$t('Minimum nights')">
                    <b-form-input></b-form-input>
                </b-form-group>
            </b-col>
            <b-col md="2">
                <b-form-group :label="$t('Maximum nights')">
                    <b-form-input></b-form-input>
                </b-form-group>
            </b-col>
            <b-col md="8">
                
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
export default {
    name: 'restrictions'
}
</script>