<template>
<!-- eslint-disable -->
    <div id="app">
        <input v-model="filter"/>
        <button type="button" @click="update" >actualizar</button>
        <data-table table-id="hotels-table" :columns="tableFields" :resource-function="provider" :filter="filter" :items-per-page="5">
            <template slot="status" slot-scope="data">
                <b-badge v-if="data.value == 1" variant="success">{{ 'Active' | translate }}</b-badge>
                <b-badge v-if="data.value == 2" variant="success">{{ 'published' | translate }}</b-badge>
                <b-badge v-if="data.value == 3" variant="danger">{{ 'Cancelled' | translate }}</b-badge>
            </template>
            <template slot="nameid" slot-scope="data">
                {{ data.item.name }} is {{ data.item.id }}
            </template>
        </data-table>
    </div>
<!-- eslint-enable -->
</template>

<script>
import dataTable from '../../components/data-table.vue';
import hotelsService from '../../api/hotels-service';

export default {
    name: 'app',
    components: {
        dataTable,
    },
    methods: {
        provider(filter, order, pageSize, page) {
            return hotelsService.getList(filter, order, page, pageSize);
        },
        update() {
            this.$root.$emit('bv::refresh::table', 'hotels-table');
        },
    },
    data() {
        return {
            filter: '',
            tableFields: [
                {
                    key: 'id',
                    label: '#',
                    sortable: true,
                },
                {
                    key: 'nameid',
                    label: this.$t('name'),
                    sortable: true,
                },
                {
                    key: 'corp',
                    label: this.$t('corporative'),
                    sortable: true,
                },
                {
                    key: 'status',
                    label: this.$t('status'),
                    sortable: true,
                },
            ],
        };
    },
};
</script>
