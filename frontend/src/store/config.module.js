export const config = {
    namespaced: true,
    state: {
        config_data: []
    },
    getters: {
        config: state => state.config_data
    },
    actions: {
        setConfig({commit}, config) {
            commit('setConfig', config);
        },
        clearConfig({commit}){
            commit('clearConfig');
        }
    },
    mutations: {
        setConfig(state, config) {
            state.config_data = config;
        },
        clearConfig(state){
            state.config_data = []
        }
    }
}
