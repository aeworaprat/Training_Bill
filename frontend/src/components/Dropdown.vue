<template>
    <select v-model="Selected">
        <option></option>
        <option v-for="(option, index) in options" :key="index" v-bind:value="option.value">{{option.text}}</option>
    </select>
</template>

<script>

export default {
    props : {
        value : {
            // type : String,
            // required : true,
        },
        options : {
            type : Array,
            required : true,
        }
    },
    computed : {

        Selected : {
            get() {
                return this.value
            },
            set(value) {
                this.$emit('input', value)
            }
        }

    },
};
</script>
