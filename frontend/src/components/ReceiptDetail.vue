<template>
    <div>
        <div class="col-md-3">
            <label>รหัสบิล</label><br>
            <input type="text" v-model="receipt.receipt_code" disabled>
        </div>
        <div class="col-md-3">
            <label>วันที่</label><br>
            <input type="text" v-model="receipt.receipt_date" disabled>
        </div>
        <br>
        <table>
            <thead>
                <tr>
                    <th width="5%">No.</th>
                    <th width="15%">รหัสสินค้า</th>
                    <th width="15%">ชื่อสินค้า</th>
                    <th width="5%">หน่วย</th>
                    <th width="5%">จำนวน</th>
                    <th width="15%">ราคา</th>
                    <th width="10%">ส่วนลด (%)</th>
                    <th width="10%">ส่วนลด (บาท)</th>
                    <th width="20%">รวมเงิน</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(row, index) in receipt.receipt_list" :key="index">
                    <td>{{index+1}}</td>
                    <td>{{row.list_item.item_code}}</td>
                    <td>{{row.list_item.item_name}}</td>
                    <td>{{row.list_item.item_unit.unit_name}}</td>
                    <td style="text-align:right">{{row.list_quantity}}</td>
                    <td style="text-align:right">{{row.list_price.toFixed(2)}}</td>
                    <td>{{row.list_discount}}</td>
                    <td style="text-align:right">{{row.list_discount_bath.toFixed(2)}}</td>
                    <td style="text-align:right">{{row.list_total_price.toFixed(2)}}</td>
                </tr>        
            </tbody>
        </table>
        <br>
        <div style="float:right">
            <label>ยอดรวมสินค้า</label><br>
            <input style="text-align:right" type="text" v-bind:value="receipt.receipt_product_price.toFixed(2)" disabled><br><br> 
            <label>ยอดรวมส่วนลดสินค้า</label><br>
            <input style="text-align:right" type="text" v-bind:value="receipt.receipt_product_discount.toFixed(2)" disabled><br><br>
            <label>ส่วนลดท้ายบิล</label> <br>
            <input style="text-align:right" type="text" v-bind:value="receipt.receipt_discount" disabled><br><br>
            <label>ราคารวมสุทธิ</label> <br>
            <input style="text-align:right" type="text" v-bind:value="receipt.receipt_total_price.toFixed(2)" disabled><br><br>
        </div>
    </div>
</template>
<script>
export default {
    props : {
        receipt : Object,
        show : Boolean
    },
    methods : {
        Cancel(){
            this.$emit('Cancel')
        }
    }
}
</script>