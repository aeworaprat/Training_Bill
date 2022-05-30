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
        <ReceiptDetail :receipt="receipt" :show="showModalView" @Cancel="showModalView=false"/>
    </div>
</template>
<script lang='ts'>

import { defineComponent, computed, ref, onMounted, reactive } from '@vue/composition-api'
import { GetAllItem, InsertReceipt } from '@/helpers/api'
import Dropdown from '@/components/Dropdown.vue'
import Modal from '@/components/Modal.vue'
import ItemDetail from '@/components/ItemDetail.vue'
import ReceiptDetail from '@/components/ReceiptDetail.vue'

interface IREceipt {
    receipt_code : string,
    receipt_date : string,
    receipt_product_price : number,
    receipt_product_discount : number,
    receipt_discount : number,
    receipt_total_price : number,
    receipt_list : IList[]
}

interface IList {
    list_item_id : number
    list_quantity : number
    list_price : number,
    list_discount : number,
    list_discount_bath : number,
    list_total_price : number
    list_item : IItem
}

interface IItem { 
    item_id : number
    item_code : string
    item_name : string
    item_price : number,
    item_unit : IUnit
}

interface IUnit {
    unit_id : number
    unit_name : string
}

interface IRows {
    item_id : number,
    product : IItem,
    qty : number,
    discount : number,
    discount_baht : number,
    total_price : number,
}

interface IModal {
    index : number
    selectIndex : number
    item_id : number
    product : IItem
}

export default defineComponent({
    components : {
        Dropdown,
        Modal,
        ItemDetail,
        ReceiptDetail
    },
    setup(){
        const items = ref<IItem[]>();
        const rows = ref<IRows[]>([]);
        const date = ref<string>(new Date().toISOString().slice(0,10));
        const receipt_discount = ref<number>(null);
        const showModalItem = ref<boolean>();
        const showModalView = ref<boolean>();
        const modal: IModal = reactive({
            index : undefined,
            selectIndex : undefined,
            item_id : undefined,
            product : undefined
        });
        const receiptInitialState: IREceipt = reactive({
            receipt_code : "",
            receipt_date : "",
            receipt_product_price : 0,
            receipt_product_discount : 0,
            receipt_discount : 0,
            receipt_total_price : 0,
            receipt_list : []
        });
        const receipt = receiptInitialState

        onMounted(() => {
            GetItem()
        })

        const itemOptions = computed(() => {
            return items.value.map(x => {
                return {
                    value : x.item_id,
                    text : x.item_code
                }
            })
        })

        const productPrice = computed(() => {
            let sum = 0
            for(let i=0;i<rows.value.length;i++){
                sum += +getRowTotal(rows.value[i])
            }
            return +sum
        })

        const productDiscount = computed(() => {
            let sum = 0
            for(let i=0;i<rows.value.length;i++){
                sum += +getDiscountBaht(rows.value[i])
            }
            return sum
        })

        const totalPrice = computed(() => {
            let total = productPrice.value;
            let last_discount = receipt_discount.value ?? 0;
            let total_price = +total - last_discount;
            if(total_price < 0){
                return 0;
            }else{
                return total_price
            }
        })

        const selectedItem = computed(() => {
            return items.value[modal.selectIndex] ?? {item_unit:{}}
        })

        function getRowTotal(row : IRows){
            let discount = getDiscountBaht(row)
            if(row.product.item_price != undefined){
                let total = (+row.qty * +row.product.item_price) - +discount;
                if (!isNaN(total)) {
                    if(total < 0){
                        return 0;
                    }else{
                        return +total;
                    }
                }else{
                    return 0;
                }
            }else{
                return 0;
            }
        }

        function getDiscountBaht(row : IRows){
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
        }

        async function GetItem(){
            const result = await GetAllItem();
            items.value = result;
        }

        function AddRow(){
            const obj : IRows = {
                item_id : undefined,
                product : {
                    item_id: undefined,
                    item_code: "",
                    item_name: "",
                    item_price: null,
                    item_unit: {
                        unit_id: undefined,
                        unit_name: "",
                    }
                },
                qty : null,
                discount : null,
                discount_baht : null,
                total_price : null,
            }
            rows.value.push(obj)
        }

        function OpenModalItem(row : IRows, index : number){
            modal.index = index
            modal.selectIndex = items.value.findIndex(x => x.item_id === row.item_id),
            modal.item_id = undefined,
            modal.product = undefined
            if(row.item_id != undefined){
                modal.item_id = row.item_id
                modal.product = row.product
            }
            showModalItem.value = true
        }

        function RemoveRow(index : number){
            rows.value.splice(index, 1);
        }

        function ItemModalChanged(index : number){
            modal.item_id = items.value[index].item_id
            modal.product = items.value[index]
            modal.selectIndex = index
        }

        function ChooseItem(){
            rows.value[modal.index].item_id = modal.item_id
            rows.value[modal.index].product = modal.product
            showModalItem.value = false
        }

        async function Insert(){
            receipt.receipt_list = [];
            receipt.receipt_code = "BILL-XXXX";
            receipt.receipt_date = date.value
            for(let i=0;i< rows.value.length;i++){
                if(rows.value[i].item_id != null){
                    const obj : IList = reactive({
                        list_item_id : +rows.value[i].item_id,
                        list_item : rows.value[i].product,
                        list_price : +rows.value[i].product.item_price,
                        list_quantity : +rows.value[i].qty,
                        list_discount : +rows.value[i].discount,
                        list_discount_bath : +getDiscountBaht(rows.value[i]),
                        list_total_price : +getRowTotal(rows.value[i])
                    })
                    receipt.receipt_list.push(obj)
                }
            }
            receipt.receipt_product_price = +productPrice.value
            receipt.receipt_product_discount = +productDiscount.value
            receipt.receipt_discount = +receipt_discount.value
            receipt.receipt_total_price = +totalPrice.value
            if(receipt.receipt_list.length == 0){
                alert('no select')
            }else{
                const result = await InsertReceipt(+receipt.receipt_product_price, +receipt.receipt_product_discount, +receipt.receipt_discount, +receipt.receipt_total_price, receipt.receipt_list)
                if(result.status_code == -1){
                    alert(result.message);
                }else{
                    alert(result.message);
                    location.reload();
                }
            }
        }

        function ViewReceipt(){
            receipt.receipt_list = [];
            receipt.receipt_code = "BILL-XXXX";
            receipt.receipt_date = date.value
            for(let i=0;i< rows.value.length;i++){
                if(rows.value[i].item_id != null){
                    const obj : IList = reactive({
                        list_item_id : rows.value[i].item_id,
                        list_item : rows.value[i].product,
                        list_price : rows.value[i].product.item_price,
                        list_quantity : rows.value[i].qty,
                        list_discount : rows.value[i].discount,
                        list_discount_bath : getDiscountBaht(rows.value[i]),
                        list_total_price : getRowTotal(rows.value[i])
                    })
                    receipt.receipt_list.push(obj)
                }
            }
            receipt.receipt_product_price = +productPrice.value
            receipt.receipt_product_discount = +productDiscount.value
            receipt.receipt_discount = +receipt_discount.value
            receipt.receipt_total_price = +totalPrice.value
            showModalView.value = true
        }

        return {
            items,
            date,
            rows,
            receipt_discount,
            showModalItem,
            showModalView,
            modal,
            productPrice,
            productDiscount,
            totalPrice,
            selectedItem,
            itemOptions,
            receipt,
            AddRow,
            OpenModalItem,
            getDiscountBaht,
            getRowTotal,
            RemoveRow,
            ItemModalChanged,
            ChooseItem,
            ViewReceipt,
            Insert
        }
    }
})
</script>