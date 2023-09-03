import {axios_getInstance} from "@/api/generic_axios";

class GeneralService {
    constructor() {
        this.axios_instance = axios_getInstance();
    }

    async parseAsync(raw) {
        raw = btoa(raw);
        return this.axios_instance
            .post('/config/parse', {
                'data': raw
            })
            .then(response => {
                return response.data;
            })
            .catch(err => console.info(err));
    }

    async fixAsync(data, level) {
        data = btoa(data);
        return this.axios_instance
            .post('/config/fix', {
                'data': data,
                'level': level
            })
            .then(response => {
                return response.data;
            })
            .catch(err => console.info(err));
    }

    async removeDuplicatesAsync(data) {
        data = btoa(data);
        return this.axios_instance
            .post('/config/remove_duplicates', {
                'data': data
            })
            .then(response => {
                return response.data;
            })
            .catch(err => console.info(err));
    }
}

export default new GeneralService();
