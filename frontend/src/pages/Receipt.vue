<template>
    <div>
        <h2>สร้างบิล</h2>
        <div class="col-md-3">
            <label>รหัสบิล</label><br>
            <input type="text" value="BILL-XXXX" disabled>
        </div>
        <div class="col-md-3">
            <label>วันที่</label><br>
            <input type="date" v-bind:value="date" style="width:172px;" disabled>
        </div>
        <br>
        <button @click="AddRow()">เพิ่ม</button>
        <br><br>
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
                    <th width="5%">ลบ</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(row, index) in rows" :key="index">
                    <td>{{index+1}}</td>
                    <td @click="OpenModalItem(row, index)">{{row.product.item_code}}</td>
                    <td>{{row.product.item_name}}</td>
                    <td>{{row.product.item_unit.unit_name}}</td>
                    <td><input type="number" v-model="row.qty" ></td>
                    <td>{{row.product.item_price}}</td>
                    <td><input type="number" v-model="row.discount"></td>
                    <td><input type="number" v-bind:value="getDiscountBaht(row)" disabled></td>
                    <td>{{getRowTotal(row)}}</td>
                    <td><button @click="RemoveRow(index)">ลบ</button></td>
                </tr>        
            </tbody>
        </table>
        <br>
        <div style="float:right">
            <label>ยอดรวมสินค้า</label><br>
            <input type="text" v-bind:value="productPrice.toFixed(2)" disabled><br><br> 
            <label>ยอดรวมส่วนลดสินค้า</label><br>
            <input type="text" v-bind:value="productDiscount.toFixed(2)" disabled><br><br>
            <label>ส่วนลดท้ายบิล</label> <br>
            <input type="number" v-model="receipt_discount" ><br><br>
            <label>ราคารวมสุทธิ</label> <br>
            <input type="text" v-bind:value="totalPrice.toFixed(2)" disabled><br><br>
            <button id="save" @click="Insert()">บันทึก</button>
            <button @click="ViewReceipt()">ดูตัวอย่าง</button>

        </div>
        <Modal :show="showModalItem">
            <template #header>
            <h3>Item List</h3>
            </template>
            <template #body>
                <ul v-for="(item, index) in items" :key="index">
                    <li @click="ItemModalChanged(index)" :class="modal.selectIndex === index ? 'active_row' : ''">{{item.item_name}}</li>
                </ul>
                <br>
                <hr>
                <ItemDetail :item="selectedItem">
                    <template #detail v-if="!selectedItem.item_id">
                    <p>No Item Select</p>
                    </template>
                </ItemDetail>
            </template>
            <template #footer>
                <button @click="ChooseItem()" :disabled="!selectedItem.item_id">Choose this item</button>
                <button @click="showModalItem = false">Cancel</button>
            </template>
        </Modal>
        <Modal :show="showModalView">
            <template #header>
                <h3>ข้อมูลบิล</h3>
            </template>
            <template #body>
                <ReceiptDetail :receipt="receipt" />
            </template>
            <template #footer>
                <button @click="showModalView=false">ยกเลิก</button>
            </template>
        </Modal>
    </div>
</template>
<script>
import { GetAllItem, InsertReceipt } from '@/helpers/api.js'
import Dropdown from '@/components/Dropdown.vue'
import Modal from '@/components/Modal.vue'
import ItemDetail from '@/components/ItemDetail.vue'
import ReceiptDetail from '@/components/ReceiptDetail.vue'



export default {
    components : {
        Dropdown,
        Modal,
        ItemDetail,
        ReceiptDetail
    },
    data() {
        return {
            items : [],
            rows : [],
            date : new Date().toISOString().slice(0,10),
            receipt_discount : '',
            showModalItem : false,
            showModalView : false,
            modal : {
                index : null,
                selectIndex : null,
                item_id : null,
                product : {
                    item_unit : {}
                },
            },
            receipt : {},
        }
    },
    computed : {
        itemOptions(){
            return this.items.map(x => {
                return {
                    value : x.item_id,
                    text : x.item_code
                }
            })
        },
        productPrice(){
            let sum = 0
            for(let i=0;i<this.rows.length;i++){
                sum += +this.getRowTotal(this.rows[i])
            }
            return sum

        },
        productDiscount(){
            let sum = 0
            for(let i=0;i<this.rows.length;i++){
                sum += +this.getDiscountBaht(this.rows[i])
            }
            return sum
        },
        totalPrice(){
            let total = this.productPrice;
            let last_discount = this.receipt_discount;
            let total_price = total - last_discount;
            if(total_price < 0){
                return '0.00';
            }else{
                return total_price
            }
        },
        selectedItem(){
            return this.items[this.modal.selectIndex] ?? {item_unit:{}}
        }
    },
    mounted: function () {
        this.GetItem()
    },
    methods : {
        getRowTotal(row){
            let discount = this.getDiscountBaht(row)
            if(!isNaN(row.product.item_price )){
                let total = (+row.qty * +row.product.item_price) - +discount;
                if (!isNaN(total)) {
                    if(total < 0){
                        return '0.00';
                    }else{
                        return +total.toFixed(2);
                    }
                }else{
                    return 0;
                }
            }else{
                return 0;
            }
        },

        getDiscountBaht(row){
            if(!isNaN(row.product.item_price)){
                let total = (+row.qty * +row.product.item_price)
                let discount_baht = (total * +row.discount)/100
                if(discount_baht > total){
                    discount_baht = total;
                }
                return +discount_baht.toFixed(2)
            }else{
                return 0
            }
        },

        itemChanged(row, item_id){
            row.item_id = item_id
            row.product = this.items.find(x => x.item_id === item_id)
        },
        
        async GetItem(){
            const result = await GetAllItem();
            this.items = result;
        },

        async Insert(){
            const list = [];
            this.rows.forEach((row, index) => {
                if(row.item_id != null){
                    list.push({
                        list_item_id : this.rows[index].item_id,
                        list_quantity : +this.rows[index].qty,
                        list_price : +this.rows[index].product.item_price,
                        list_discount : +this.rows[index].discount,
                        list_discount_bath : +this.getDiscountBaht(this.rows[index]),
                        list_total_price : +this.getRowTotal(this.rows[index])
                    })
                }
            });
            let receipt_product_price = this.productPrice
            let receipt_product_discount = this.productDiscount
            let receipt_discount = this.receipt_discount
            let receipt_total_price = this.totalPrice
            if(receipt_discount == ''){
                receipt_discount = 0;
            }
            if(list.length == 0){
                alert('no select')
            }else{
                const result = await InsertReceipt(+receipt_product_price, +receipt_product_discount, +receipt_discount, +receipt_total_price, list)
                if(result.status_code == -1){
                    alert(result.message);
                }else{
                    alert(result.message);
                    location.reload();
                }
            }
        },

        AddRow(){
            const obj = {
                item_id : null,
                product : {
                    item_unit : {}
                },
                qty : '',
                discount : '',
                discount_baht : 0,
                total_price : 0,
            }
            this.rows.push(obj)
        },

        RemoveRow(index){
            this.rows.splice(index, 1);
        },

        OpenModalItem(row, index){
            this.modal.index = index
            this.modal.selectIndex = null,
            this.modal.item_id = null,
            this.modal.product = {
                item_unit : {}
            }
            if(row.item_id != null){
                this.modal.item_id = row.item_id
                this.modal.product = row.product
            }
            this.showModalItem = true
        },
        
        ItemModalChanged(index){
            this.modal.item_id = this.items[index].item_id
            this.modal.product = this.items[index]
            this.modal.selectIndex = index
        },
        
        ChooseItem(){
            this.rows[this.modal.index].item_id = this.modal.item_id
            this.rows[this.modal.index].product = this.modal.product
            this.showModalItem = false
        },

        ViewReceipt(){
            this.receipt = {
                receipt_list : []
            }
            this.receipt.receipt_code = "BILL-XXXX"
            this.receipt.receipt_date = this.date
            for(let i=0;i< this.rows.length;i++){
                if(this.rows[i].item_id != null){
                    const obj = {
                        list_item : this.rows[i].product,
                        list_price : this.rows[i].product.item_price,
                        list_quantity : this.rows[i].qty,
                        list_discount : this.rows[i].discount,
                        list_discount_bath : this.getDiscountBaht(this.rows[i]),
                        list_total_price : this.getRowTotal(this.rows[i])
                    }
                    this.receipt.receipt_list.push(obj)
                }
            }
            this.receipt.receipt_product_price = this.productPrice
            this.receipt.receipt_product_discount =this. productDiscount
            this.receipt.receipt_discount = this.receipt_discount
            this.receipt.receipt_total_price = this.totalPrice
            this.showModalView = true
        }
    }
}
</script>