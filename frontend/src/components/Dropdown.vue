<template>
    <select v-model="selected">
        <option></option>
        <option v-for="(option, index) in options" :key="index" v-bind:value="option.value">{{option.text}}</option>
    </select>
</template>

<script lang="ts">
import { defineComponent, computed, PropType } from '@vue/composition-api'
export default defineComponent({
    props : {
        value : {
            // type : String,
            // required : true,
        },
        options : {
            type : Array as PropType<{ value : any, text : string}[]>,
            required : true,
        }
    },
    setup(props, {emit}){
        const selected = computed({
            get : ()=>{
                return props.value
            },
            set : (value)=>{
                emit('input', value)
            }
        })
        return {
            selected
        }
    }
});
</script>
