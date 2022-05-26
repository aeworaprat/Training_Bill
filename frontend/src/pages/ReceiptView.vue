<template>
    <div>
        <br><br>
        <div class="container">
            <h3>ค้นหาบิล</h3>
            <label>วันที่เริ่มต้น : </label> 
            <input type="date" v-model="startDate">&nbsp;
            <label>วันที่สิ้นสุด : </label>
            <input type="date" v-model="endDate">
            <br><br>
            <button @click="Searh()">ค้นหา</button>
            <br><br>
        </div>
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
        <Modal :show="showModal" >
            <template #header>
                <h3>ข้อมูลบิล</h3>
            </template>
            <template #body>
                <ReceiptDetail :receipt="receiptDetail" />
            </template>
            <template #footer>
                <button @click="showModal=false">ยกเลิก</button>
            </template>
        </Modal>
    </div>
</template>
<script>
import { GetAllReceipt, GetReceiptById, GetReceiptFilterDate } from '@/helpers/api.js'
import ReceiptDetail from '@/components/ReceiptDetail.vue'
import Modal from '@/components/Modal.vue'


export default {
    components : {
        ReceiptDetail,
        Modal
    },
    data (){
        return {
            receipt : [],
            receiptDetail : {},
            startDate : "",
            endDate : "",
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
            this.receiptDetail = {}
            const data_receipt = await GetReceiptById(receipt_id);
            console.log(data_receipt)
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
        },

        async Searh(){
            let end = ""
            let start = ""
            if(this.startDate != ""){
                start = this.startDate;
                let startYear = start.substring(0,4);
                startYear = +startYear + 543;
                start = startYear+start.substring(4)

            }
            if(this.endDate != ""){
                end = this.endDate;
                let endYear = end.substring(0,4);
                endYear = +endYear + 543;
                end = endYear+end.substring(4)
            }
            const data_receipt = await GetReceiptFilterDate(start, end)
            if(data_receipt.status_code == -1){
                alert(data_receipt.message)
            }else{
                this.receipt = data_receipt.data
            }


        }
    }
}
</script>
<style>

</style>