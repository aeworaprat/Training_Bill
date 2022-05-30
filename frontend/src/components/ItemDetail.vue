<template>
    <div>
        <h3>Item detail</h3>
        <slot name="detail">
            <b>รหัสสินค้า</b><br>
            {{item.item_code}}<br><br>
            <b>ชื่อสินค้า</b><br>
            {{item.item_name}}<br><br>
            <b>ราคา</b><br>
            {{item.item_price}}<br><br>
            <b>หน่วย</b><br>
            {{item.item_unit.unit_name}}
        </slot>
    </div>
</template>
<script lang="ts">
import { defineComponent } from '@vue/composition-api'

export default defineComponent({
    props : {
        item : Object
    }

})
</script>
<style>
</style>