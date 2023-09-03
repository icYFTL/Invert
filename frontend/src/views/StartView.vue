<template>
    <div class="start-dropzone">
        <p v-if="isDragActive">Drop it...</p>
        <p v-else @click="open">Drop config here<br>(or click to select)</p>
        <div v-bind="getRootProps()" style="width: 100%; height: 100%; position: absolute">
            <input v-bind="getInputProps()">
<!--            <div v-if="isFocused" id="focus">-->
<!--                focused-->
<!--            </div>-->
<!--            <div v-if="isDragReject" id="isDragReject">-->
<!--                isDragReject: {{ isDragReject }}-->
<!--            </div>-->
        </div>
    </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue'
import { useDropzone } from 'vue3-dropzone'
import type { FileRejectReason } from 'vue3-dropzone'
import {POSITION, useToast} from "vue-toastification";
import {useRouter} from "vue-router";
import GeneralService from '@/api/services/general.service.js'
import store from '@/store/index.js'

const toasts = useToast();
const router = useRouter();
async function onDrop(acceptedFiles: File[], rejectReasons: FileRejectReason[]) {
    if (rejectReasons.length > 0){
        toasts.error(rejectReasons[0].errors[0].message, { position: POSITION.BOTTOM_LEFT }) // TODO: fix
        return;
    }
    const selectedFile = acceptedFiles[0];

    if (selectedFile) {
        const reader = new FileReader();

        reader.onload = async (e) => {
            GeneralService.parseAsync(e.target.result).then(async (x) => {
                await store.dispatch("config/setConfig", x.Response).then(() => {
                    router.push('/workflow')
                });
            });

        };
        reader.readAsText(selectedFile);
    }

    // console.warn('wtf')
    // console.log('acceptedFiles', acceptedFiles)
    // console.log('rejectReasons', rejectReasons)
}

const options = reactive({
    multiple: false,
    onDrop,
    accept: '.cfg',
    maxSize: 1048576
})

const {
    getRootProps,
    getInputProps,
    isDragActive,
    isFocused,
    isDragReject,
    open
} = useDropzone(options)


</script>
<style>
.start-dropzone {
    width: 100vw;
    height: 100vh;
    margin: auto; /* Устанавливаем автоматические отступы по вертикали и горизонтали */
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    display: flex; /* Добавляем стиль flex для выравнивания по центру */
    flex-direction: column; /* Располагаем дочерние элементы вертикально */
    justify-content: center; /* Выравниваем по вертикали по центру */
    align-items: center; /* Выравниваем по горизонтали по центру */
    font-size: 3em;
}
</style>