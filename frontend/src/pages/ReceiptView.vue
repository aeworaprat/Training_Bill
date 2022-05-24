<template>
    <div>
        <h2>ดูบิล</h2>
        <table id="table">
            <thead>
                <tr>
                    <th width='5%'>No.</th>
                    <th width='15%'>รหัสบิล</th>
                    <th width='15%'>วันที่</th>
                    <th>ราคารามสุทธิ</th>
                    <th width='15%' style='text-align:center'>ดำเนินการ</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(receipt, index) in receipt" :key="index">
                    <td>{{index+1}}</td>
                    <td>{{receipt.receipt_code}}</td>
                    <td>{{receipt.receipt_date}}</td>
                    <td style="text-align:right">{{receipt.receipt_total_price.toFixed(2)}}</td>
                    <td><button @click="ViewReceipt(receipt.receipt_id)">ดู</button></td>
                </tr>
            </tbody>
        </table>
        <Modal :show="showModal">
            <template #header>
                <h3>ข้อมูลบิล</h3>
            </template>
            <template #body>
                <div>
                    <label>รหัสบิล</label><br>
                    <input type="text" v-model="receiptDetail.receipt_code" disabled>
                </div>
                <div>
                    <label>วันที่</label><br>
                    <input type="text" v-model="receiptDetail.receipt_date" disabled>
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
                        <tr v-for="(row, index) in receiptDetail.receipt_list" :key="index">
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
                    <input style="text-align:right" type="text" v-bind:value="receiptDetail.receipt_product_price.toFixed(2)" disabled><br><br> 
                    <label>ยอดรวมส่วนลดสินค้า</label><br>
                    <input style="text-align:right" type="text" v-bind:value="receiptDetail.receipt_product_discount.toFixed(2)" disabled><br><br>
                    <label>ส่วนลดท้ายบิล</label> <br>
                    <input style="text-align:right" type="text" v-bind:value="receiptDetail.receipt_discount" disabled><br><br>
                    <label>ราคารวมสุทธิ</label> <br>
                    <input style="text-align:right" type="text" v-bind:value="receiptDetail.receipt_total_price.toFixed(2)" disabled><br><br>
                </div>
            </template>
            <template #footer>
                <button @click="CloseModal()">ยกเลิก</button>
            </template>
        </Modal>
    </div>
</template>
<script>
import { GetAllReceipt, GetReceiptById } from '@/helpers/api.js'
import Modal from '@/components/Modal.vue'
export default {
    components : {
        Modal
    },
    data (){
        return {
            receipt : [],
            receiptDetail : {},
            showModal : false
        }
    },

    mounted : function() {
        this.GetReceipt();
    },

    methods : {
        async GetReceipt(){
            const result = await GetAllReceipt();
            this.receipt = result;
        },

        async ViewReceipt(receipt_id){
            const data_receipt = await GetReceiptById(receipt_id);
            if(data_receipt.status_code == -1){
                alert(data_receipt.message)
            }else{
                this.receiptDetail = data_receipt.data
            }
            this.showModal = true;
            
        },

        CloseModal(){
            this.showModal = false;
            this.receiptDetail = {};
        }
    }
}
</script>
<style>

</style>