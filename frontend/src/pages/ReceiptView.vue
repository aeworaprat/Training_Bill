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
        <ReceiptDetail :receipt="receiptDetail" :show="showModal" @Cancel="showModal=false"/>
    </div>
</template>
<script lang="ts">
import { GetAllReceipt, GetReceiptById, GetReceiptFilterDate } from '@/helpers/api.js'
import { defineComponent, ref, onMounted, } from '@vue/composition-api'
import ReceiptDetail from '@/components/ReceiptDetail.vue'
import Modal from '@/components/Modal.vue'

interface IReceipt { 
    receipt_id : number
    receipt_code : string
    receipt_date : string
    receipt_product_price : number
    receipt_product_discount : number 
    receipt_discount : number
    receipt_total_price : number
    receipt_list : object
}

export default defineComponent({
    components : {
        ReceiptDetail,
        Modal
    },
    setup(){
        const receipt = ref<IReceipt[]>();
        const receiptDetail = ref<IReceipt>();
        const startDate = ref<string>();
        const endDate = ref<string>();
        const showModal = ref<boolean>(false);

        onMounted(() => {
            GetReceipt();
        })

        async function GetReceipt(){
            const result = await GetAllReceipt();
            receipt.value = result;
        }

        async function ViewReceipt(receipt_id : number){
            receiptDetail.value = undefined
            const data_receipt = await GetReceiptById(receipt_id);
            if(data_receipt.status_code == -1){
                alert(data_receipt.message)
            }else{
                receiptDetail.value = data_receipt.data
            }
            showModal.value = true;
        }

        return {
            receipt,
            receiptDetail,
            startDate,
            endDate,
            showModal,
            ViewReceipt
        }
    }
})
</script>
<style>

</style>