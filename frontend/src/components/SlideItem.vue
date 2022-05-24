<template>
    <div v-show="visible === index"> 
        <slot></slot>
    </div>
</template>
<script>
export default {
    props: {
        visible : Number,
        index : Number
    }
}
</script>