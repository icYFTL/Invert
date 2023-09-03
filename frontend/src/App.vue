<template>
    <div class="current">
        <router-view></router-view>
    </div>
</template>

<script>
export default {
    name: "app"
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
</style>
