<template>
    <div class="current">
        <div class="version">
            <code>Invert version: {{invert_version}}</code>
            <br>
            <code>Invert API version: {{invert_api_version}}</code>
        </div>
        <router-view></router-view>

    </div>
</template>

<script>
import GeneralService from '@/api/services/general.service.js'
export default {
    name: "app",
    data(){
        return {
            'invert_version': '',
            'invert_api_version': '',
        }
    },
    async mounted() {
        this.invert_version = import.meta.env.VITE_VERSION
        await GeneralService.getVersion().then(x => {
            this.invert_api_version = x
        });
    }
}

</script>

<style scoped>
.current {
    width: 100vw;
    height: 100vh;
    top: 0;
    left: 0;
    position: absolute;
}
.version {
    position: fixed;
    right: 0;
}
</style>
