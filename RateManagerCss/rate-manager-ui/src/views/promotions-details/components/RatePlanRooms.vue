<template>
    <b-card header="Planes Tarifarios y habitaciones">
        <b-row>
            <b-col>
                <!--habitaciones-->
                <h5 class="text-center">{{ $t('Rooms') }}</h5>
                <b-form-checkbox @change="selectAllRooms" v-model="allRooms">
                    {{ $t('Select all') }}
                </b-form-checkbox>
                <hr>
                <b-list-group class="mt-2 list-group-scroll">
                    <b-list-group-item v-for="room in rooms" :key="room.id">
                        <b-form-checkbox @change="selectRoom" :value="room.id" v-model="selectedRooms">
                            {{ room.name }} - [{{ room.code }}]
                        </b-form-checkbox>
                    </b-list-group-item>
                </b-list-group>
            </b-col>
            <b-col>
                <!--planes tarifarios-->
                <h5 class="text-center">{{ $t('Rate plans') }}</h5>
                <b-form-checkbox @change="selectAllRPlans" v-model="allRPlans">
                    {{ $t('Select all') }}
                </b-form-checkbox>
                <hr>
                <b-list-group class="mt-2 list-group-scroll">
                    <b-list-group-item v-for="rp in ratePlans" :key="rp.code">
                        <b-form-checkbox @change="selectRatePlan" :value="rp.code" v-model="selectedRatePlan">
                            {{rp.name}} - [{{ rp.code }}]
                        </b-form-checkbox>
                    </b-list-group-item>
                </b-list-group>
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
import RoomService from '../../../api/rooms-service';
import RatePlanService from '../../../api/ratePlans-service';
import EventBus from '../../../core/event-bus';

export default {
    name: 'rate-plan-rooms',
    props: {
        /*--> request */
        selectedRooms: {
            type: Array,
            required: true
        },
        selectRatePlan: {
            type: Array,
            required: true
        }
        /*<-- request */
    },
    created() {
        this.getRooms();
        this.getRatePlans();
    },
    data() {
        return {
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
        //->rooms
        getRooms() {
            RoomService.getList(this.hotelId).then(response => {
                this.rooms = response.body;
            });
        },
        selectRoom() {
            this.allRooms = false;
        },
        selectAllRooms(checked) {
            this.selectedRooms = [];
            if (checked) {
                this.rooms.forEach((value) => {
                    this.selectedRooms.push(value.id);
                });
            }
        },
        //<-room

        //->rateplan
        getRatePlans() {
            RatePlanService.getList(this.hotelId).then(response => {
                this.ratePlans = response.body;
            });
        },
        selectRatePlan() {
            this.allRPlans = false;
        },
        selectAllRPlans(checked) {
            this.selectedRatePlan = [];
            if (checked) {
                this.ratePlans.forEach((value) => {
                    this.selectedRatePlan.push(value.code);
                });
            }
        }
        //<-rateplan
    },
    watch: {
        rooms() {
            if (this.rooms.length > 0) EventBus.$emit('changeOffset');
        },
        ratePlans() {
            if (this.ratePlans.length > 0) EventBus.$emit('changeOffset');
        }
    }
}
</script>