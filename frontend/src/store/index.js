import { createStore } from "vuex";
import { config } from "./config.module";

const store = createStore({
    modules: {
        config
    },
});
export default store;
