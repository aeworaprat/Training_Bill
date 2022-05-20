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
    </div>
</template>
<script>
import { GetAllReceipt } from '@/helpers/api.js'
export default {

    data (){
        return {
            receipt : [],
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

        ViewReceipt(receipt_id){
            // console.log('test')
            this.$router.push({ name : 'receipt-info', params : { id : receipt_id }});
        }
    }
}
</script>
<style>
table {
    border-collapse: collapse;
    width: 100%;
}

td, th {
    border: 1px solid #262626;
    text-align: left;
    padding: 8px;
}
</style>