<template>
    <div v-if="showSlide == true">
        <slot></slot>
        <button @click="Previous()" class="prev">Previous</button>
        <button @click="Next()" class="next">Next</button>
    </div>
</template>
<script>
export default {
    props : {
        showSlide : Boolean
    },
    methods : {
        Next(){
            this.$emit('Next');
        },
        Previous(){
            this.$emit('Previous');
        }
    }
}
</script>
<style>
</style>