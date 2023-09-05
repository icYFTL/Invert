<template>
    <div class="workflow">
        <Dialog v-model:visible="fix_dialog_visible" modal header="Fixing settings" :style="{ width: '50vw' }">
            <div class="dialog-body">
                <div class="slider-c">
                    <InputText id="input-slider" v-model.lazy="level_rendered" readonly v-tooltip="level_description"/>
                    <Slider v-model="level_value" id="level-slider" :step="50"/>
                    <p style="">Level</p>
                </div>
                <div class="dialog-cbs">
                    <div>
                        <Checkbox v-model="remove_duplicates_condition" inputId="remove-duplicates_cb" :binary="true" />
                        <label for="remove-duplicates_cb" class="ml-2"> Remove duplicates </label>
                    </div>
                    <div>
                        <Checkbox v-model="optimize_condition" inputId="optimize_cb" :binary="true" disabled v-tooltip="'Soon'"/>
<!--                        v-tooltip="'cl_updaterate etc.'"-->
                        <label for="optimize_cb" class="ml-2"> Optimize </label>
                    </div>
                    <div>
                        <Checkbox inputId="cs2Format_cb" :binary="true" disabled v-tooltip="'Soon'"/>
                        <label for="cs2Format_cb" class="ml-2" > Prepare CS2 format config files </label>
                    </div>
                </div>
                <div style="position: fixed; bottom: 15px; width: 100%; left: 0;">
                    <div class="buttons-c">
                        <Button class="bc-btn" style="margin-left: 0; margin-top: 0;" label="Do it" @click="onFixClick"/>
                    </div>
                </div>
            </div>
        </Dialog>
        <div class="tools-zone">
            <div class="buttons-c">
                <Button class="bc-btn" label="Save" @click="onSaveClickAsync"/>
                <Button class="bc-btn" label="Fix" @click="onShowDialogClick"/>
                <Button class="bc-btn" label="Select other config" @click="onResetClickAsync"/>
            </div>
        </div>
        <hr>
        <div class="grid-zone">
            <Vue3EasyDataTable
                    :headers="headers"
                    :items="items"
                    theme-color="#1d90ff"
                    table-class-name="customize-table"
                    header-text-direction="center"
                    body-text-direction="center"
                    ref="c-table"
            >
                <template #loading>
                    <img
                            src="https://media.tenor.com/RVvnVPK-6dcAAAAC/reload-cat.gif"
                            style="width: 60px; height: 100px"
                            alt="Loading..."

                    />
                </template>
                <template #item-Add="{Add}">
                    <div class="added-wrapper">
                        <input type="checkbox" :checked="Add"/>
                    </div>
                </template>
                <!--                <template #item-Deprecated="{Deprecated}">-->
                <!--                    <div class="deprecated-wrapper">-->
                <!--                        <input type="checkbox" :checked="Deprecated" onclick="return false;"/>-->
                <!--                    </div>-->
                <!--                </template>-->
            </Vue3EasyDataTable>
        </div>
    </div>
</template>

<script lang="ts">
import store from '@/store/index.js'
import GeneralService from '@/api/services/general.service.js'
import type {Header, Item} from "vue3-easy-data-table";
import Vue3EasyDataTable from 'vue3-easy-data-table';
import {defineComponent, ref} from "vue";
import Slider from 'primevue/slider';
import InputText from 'primevue/inputtext';
import Card from 'primevue/card';
import Button from 'primevue/button';
import Dialog from "primevue/dialog";
import Checkbox from 'primevue/checkbox';

export default defineComponent({
    setup() {
        const headers: Header[] = [
            {text: "Command", value: "Name", sortable: true},
            {text: "Parameters", value: "ShowParsedArgs", sortable: true},
            {text: "Add", value: "Add", sortable: true}
            // {text: "Deprecated", value: "Deprecated", sortable: true}
        ];

        const items: Item[] = ref(store.state.config.config_data);

        return {
            headers,
            items
        };
    },
    methods: {
        onShowDialogClick() {
            this.fix_dialog_visible = true;
        },
        onFixClick() {
            GeneralService.fixAsync(this.objectsToStringWithNewlineSeparator(this.items), this.getLevel, this.remove_duplicates_condition).then(async (x) => {
                // await store.dispatch("config/setConfig", x.Response);
                this.items = x.Response;
            });
            this.fix_dialog_visible = false;
        },
        onRemoveDuplicatesClick() {
            GeneralService.removeDuplicatesAsync(this.objectsToStringWithNewlineSeparator(this.items)).then(async (x) => {
                // await store.dispatch("config/setConfig", x.Response);
                this.items = x.Response;
            });
        },
        async onResetClickAsync() {
            await store.dispatch("config/clearConfig");
            await this.$router.push('/');
        },
        objectsToStringWithNewlineSeparator(objects, isEnd = false) {
            var result = "";
            for (var i = 0; i < objects.length; i++) {
                if (objects[i].Add === true) {
                    result += objects[i].FullCommand + '\n';
                }
            }
            if (isEnd) {
                result += "\n";
                result += "echo \"Converted from CS:GO by Invert\"\n";
                result += "echo \"In case of problems write issue at https://github.com/icYFTL/Invert\"\n";
                result += "echo \"bw, icY\"\n";
            }
            return result;
        },
        async onSaveClickAsync() {
            const blob = new Blob([this.objectsToStringWithNewlineSeparator(this.items, true)], {type: 'text/plain'});

            const url = URL.createObjectURL(blob);

            const a = document.createElement('a');
            a.href = url;
            a.download = 'invert.cfg';
            a.style.display = 'none';

            document.body.appendChild(a);
            a.click();

            URL.revokeObjectURL(url);
            document.body.removeChild(a);
        }
    },
    components: {
        Vue3EasyDataTable,
        Slider,
        InputText,
        Card,
        Button,
        Dialog,
        Checkbox
    },
    computed: {
        getLevel() {
            if (this.level_value === 0)
                return 1;
            else if (this.level_value == 50)
                return 2;
            else if (this.level_value == 100)
                return 3;

            return 1;
        }
    },
    data() {
        return {
            level_value: 0,
            level_rendered: 'Basic',
            level_description: 'Only necessary commands (+moveleft -> +left etc.)',
            fix_dialog_visible: false,
            remove_duplicates_condition: false,
            optimize_condition: false
        }
    },
    watch: {
        level_value(old) {
            if (old === 0){
                this.level_rendered = 'Basic'
                this.level_description = 'Only necessary commands (+moveleft -> +left etc.)'
            }
            else if (old === 50){
                this.level_rendered = 'Default'
                this.level_description = 'Basic + removing old obsolete commands'
            }
            else if (old === 100){
                this.level_rendered = 'Extended'
                this.level_description = 'Default + replace/removing aliases and links to them'
            }
        }
    }
});
</script>

<style>
.customize-table {
    --easy-table-border: 1px solid #445269;
    --easy-table-row-border: 1px solid #445269;

    --easy-table-header-font-size: 14px;
    --easy-table-header-height: auto;
    --easy-table-header-font-color: #c1cad4;
    --easy-table-header-background-color: #2d3a4f;

    --easy-table-header-item-padding: 10px 15px;

    --easy-table-body-even-row-font-color: #fff;
    --easy-table-body-even-row-background-color: #4c5d7a;

    --easy-table-body-row-font-color: #c0c7d2;
    --easy-table-body-row-background-color: #2d3a4f;
    --easy-table-body-row-height: 50px;
    --easy-table-body-row-font-size: 14px;

    --easy-table-body-row-hover-font-color: #2d3a4f;
    --easy-table-body-row-hover-background-color: #eee;

    --easy-table-body-item-padding: 10px 15px;

    --easy-table-footer-background-color: #2d3a4f;
    --easy-table-footer-font-color: #c0c7d2;
    --easy-table-footer-font-size: 14px;
    --easy-table-footer-padding: 0px 10px;
    --easy-table-footer-height: 50px;

    --easy-table-rows-per-page-selector-width: 70px;
    --easy-table-rows-per-page-selector-option-padding: 10px;
    --easy-table-rows-per-page-selector-z-index: 1;


    --easy-table-scrollbar-track-color: #2d3a4f;
    --easy-table-scrollbar-color: #2d3a4f;
    --easy-table-scrollbar-thumb-color: #4c5d7a;;
    --easy-table-scrollbar-corner-color: #2d3a4f;

    --easy-table-loading-mask-background-color: #2d3a4f;
}

.workflow {
    height: 100%;
    width: 100%;
}

.grid-zone {
    width: 100%;
}

.tools-zone {
    width: 100%;
    padding: 25px;
    display: flex;
    flex-flow: row nowrap;
}

.tools-zone div {
    margin-left: 20px;
}

.slider-c #level-slider {
    min-width: 150px;
    max-width: 300px;
}

.slider-c #input-slider {
    min-width: 150px;
    max-width: 300px;

    max-height: 30px;
}

#input-slider {
    margin-bottom: 10px;
    text-align: center;
}

.slider-c {
    display: flex;
    flex-flow: column nowrap;
    align-items: center;
    padding-bottom: 10px;
}

.slider-c * {
    margin-top: 5px;
}

.buttons-c {
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
}

.bc-btn {
    max-height: 35px;
    width: 200px;
    margin-top: 2px;
    margin-left: 15px;
}

.p-dialog .p-dialog-content {
    background: #181818 !important;
    color: #fff !important;
}

.p-dialog .p-dialog-header {
    background: #181818 !important;
    color: #fff !important;
}

.p-dialog {
    min-height: 200px;
}

.p-dialog-content {
    min-height: 200px;
}

.dialog-cbs {
    width: 100%;
    display: flex;
    flex-flow: row nowrap;
    justify-content: center;
    gap: 10%;
}
</style>