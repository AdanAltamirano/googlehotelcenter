<template>
    <b-card :header="$t('Rate plans and rooms')">
        <b-row>
            <b-col>
                <!--habitaciones-->
                <h6 class="text-center">{{ $t('Rooms') }}</h6>
                <b-form-checkbox @change="selectAllRooms" v-model="allRooms">
                    {{ $t('Select all') }}
                </b-form-checkbox>
                <hr>
                <b-list-group class="mt-2 list-group-scroll">
                    <b-list-group-item v-for="room in rooms" :key="room.id">
                        <b-form-checkbox @change="allRooms = false" :value="room.id" v-model="model.rooms">
                            {{ room.name }} - [{{ room.code }}]
                        </b-form-checkbox>
                    </b-list-group-item>
                </b-list-group>
            </b-col>
            <b-col>
                <!--planes tarifarios-->
                <h6 class="text-center">{{ $t('Rate plans') }}</h6>
                <b-form-checkbox @change="selectAllRPlans" v-model="allRPlans">
                    {{ $t('Select all') }}
                </b-form-checkbox>
                <hr>
                <b-list-group class="mt-2 list-group-scroll">
                    <b-list-group-item v-for="rp in ratePlans" :key="rp.code">
                        <b-form-checkbox @change="allRPlans = false" :value="rp.code" v-model="model.ratesPlan">
                            {{rp.name}} - [{{ rp.code }}]
                        </b-form-checkbox>
                    </b-list-group-item>
                </b-list-group>
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
import roomService from '../../../api/rooms-service';
import ratePlanService from '../../../api/ratePlans-service';
import eventBus from '../../../core/event-bus';

export default {
    props: {
        dataModel: {
            type: Object,
            required: true
        }
    },
    created() {
        this.getRooms();
        this.getRatePlans();
    },
    data() {
        return {
            model: this.dataModel,
            hotelId: this.$appConfig.session.hotelId,
            rooms: [],
            allRooms: false,
            allRPlans: false,
            ratePlans: [],
        }
    },
    computed: {
        
    },
    methods: {
        getRooms() {
            roomService.getList(this.hotelId).then(response => {
                this.rooms = response.body;
                this.allRooms = this.rooms.length === this.model.rooms.length;
            });
        },
        selectAllRooms(checked) {
            this.model.rooms = [];
            if (checked) {
                this.rooms.forEach((value) => {
                    this.model.rooms.push(value.id);
                });
            }
        },

        getRatePlans() {
            ratePlanService.getList(this.hotelId).then(response => {
                this.ratePlans = response.body;
                this.allRPlans = this.ratePlans.length === this.model.ratesPlan.length;
            });
        },
        selectAllRPlans(checked) {
            this.model.ratesPlan = [];
            if (checked) {
                this.ratePlans.forEach((value) => {
                    this.model.ratesPlan.push(value.code);
                });
            }
        }
    },
    watch: {
        rooms() {
            if (this.rooms.length > 0)
                eventBus.$emit('changeOffset');
        },
        ratePlans() {
            if (this.ratePlans.length > 0)
                eventBus.$emit('changeOffset');
        }
    }
}
</script>