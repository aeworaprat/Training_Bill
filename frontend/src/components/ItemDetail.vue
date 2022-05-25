<template>
    <div>
        <slot name="header">
            <h3>Item detail</h3>
        </slot>
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
<script>
export default {
    props : {
        item : Object
    }

}
</script>
<style>
</style>