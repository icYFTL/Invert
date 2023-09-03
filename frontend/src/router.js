import { createRouter, createWebHistory } from 'vue-router'
import StartView from "@/views/StartView.vue";
import WorkflowView from "@/views/WorkflowView.vue";

const routes = [
    { path: '/', component: StartView },
    { path: '/workflow', component: WorkflowView , meta: { transition: 'fade' }}
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router