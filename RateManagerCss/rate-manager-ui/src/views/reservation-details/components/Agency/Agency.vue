<template>
    <div>
        <h6 style="cursor:pointer" v-b-toggle.agency>
            <i class="fa fa-plus-circle"></i>
            {{$t('Agency Information')}}
        </h6>
        <b-collapse visible id="agency">
            <b-alert show variant="secondary">
                <div>
                    {{agencyDetails.name}}
                </div>
                <div>
                    {{agencyDetails.city}},{{agencyDetails.state}},{{agencyDetails.country}}
                </div>
                <div>
                    <strong>{{$t('Agent')}}:</strong> {{agencyDetails.agentName}} {{agencyDetails.agentLastName}}
                </div>
            </b-alert>
        </b-collapse>
    </div>
</template>

<script>
export default {
    props:{
        agencyDetails: {
            type: Object
        },
    }
}
</script>