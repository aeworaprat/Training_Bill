<template>
    <div v-if="showSlide == true">
        <slot></slot>
        <i @click="Previous()" class="btn prev arrow" style="float:left"></i>
        <i @click="Next()" class="btn next arrow" style="float:right"></i>
    </div>
</template>
<script lang="ts">
import { defineComponent } from '@vue/composition-api'

export default defineComponent({
    props : {
        showSlide : Boolean
    },
    setup(props, {emit}){

        function Next(){
            emit('Next');
        }

        function Previous(){
            emit('Previous');
        }

        return {
            Previous,
            Next
        }
    }
})
</script>
<style>
.btn {
    align-items: center;
    display: flex;
    justify-content: center;
}
.next{
    width: 0; 
    height: 0; 
    border-top: 20px solid transparent;
    border-bottom: 20px solid transparent;
    
    border-left: 20px solid green;

}

.prev {
    width: 0; 
    height: 0; 
    border-top: 20px solid transparent;
    border-bottom: 20px solid transparent; 
    
    border-right:20px solid green; 
}
</style>