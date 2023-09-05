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

    async fixAsync(data, level, removeDuplicates) {
        data = btoa(data);
        return this.axios_instance
            .post('/config/fix', {
                'data': data,
                'level': level,
                'removeDuplicates': removeDuplicates
            })
            .then(response => {
                return response.data;
            })
            .catch(err => console.info(err));
    }

    async getVersion() {
        return this.axios_instance
            .get('/general/version')
            .then(response => {
                return response.data.Response;
            })
            .catch(err => console.info(err));
    }
}

export default new GeneralService();
