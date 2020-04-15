<template>
    <b-card header="Planes Tarifarios y habitaciones">
        <b-row>
            <b-col>
                <!--habitaciones-->
                <h5 class="text-center">{{ $t('Rooms') }}</h5>
                <b-form-checkbox @change="selectAllRooms" v-model="allRooms">
                    {{ $t('Select all') }}
                </b-form-checkbox>
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
                <b-list-group class="mt-2 list-group-scroll">

                </b-list-group>
            </b-col>
        </b-row>
    </b-card>
</template>

<script>
import RoomService from '../../../api/rooms-service';
import EventBus from '../../../core/event-bus';

export default {
    name: 'rate-plan-rooms',
    created() {
        this.getRooms();
    },
    data() {
        return {
            rooms: [],
            selectedRooms: [],
            allRooms: false,
            allRPlans: false
        }
    },
    methods: {
        getRooms() {
            RoomService.getList(this.$appConfig.session.hotelId).then(response => {
                this.rooms = response.body;
            });
        },
        selectAllRooms(checked) {
            this.selectedRooms = [];
            if (checked) {
                this.rooms.forEach((value) => {
                    this.selectedRooms.push(value.id);
                });
            }
        },
        selectRoom() {
            this.allRooms = false;
        },
        selectAllRPlans(checked) {

        }
    },
    watch: {
        rooms() {
            if (this.rooms.length > 0) {
                EventBus.$emit('changeOffset');
            }
        }
    }
}
</script>