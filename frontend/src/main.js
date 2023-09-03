import {createApp} from 'vue'
import App from './App.vue'
import router from './router'
import store from "./store";

import './assets/main.css'
import "vue-toastification/dist/index.css";
import 'vue3-easy-data-table/dist/style.css';
import "primevue/resources/themes/lara-light-indigo/theme.css";

createApp(App)
    .use(router)
    .use(store)
    .mount('#app')
