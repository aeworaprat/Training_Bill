<template>
<div>
    <h2>จัดการสินค้า</h2>
    <button @click="OpenModal()">เพิ่มสินค้า</button>
    <br><br>
    <div class="row">
        <div class="column">
            <table class="table table-striped table-bordered">
                <thead>
                    <tr>
                        <th width='5%'>No.</th>
                        <th width='15%'>รหัสสินค้า</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(item, index) in item" :key="index" > 
                        <td>{{index+1}}</td>
                        <td @click="SlideOn(index)" :class="selectIndex === index ? 'active_row' : ''">{{item.item_code}}</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="column">
            <Slide :showSlide="showSlide" @Next="Next()" @Previous="Previous()">
                <div class="container">
                    <ItemDetail :item="selectedItem" ></ItemDetail>
                    <br>
                    <div style="text-align:center;">
                        <button @click="OpenModal(selectedItem)">Update</button>
                        <button @click="Delete(selectedItem.item_id)">Delete</button>
                    </div>
                </div>
            </Slide>
        </div>
    </div>
    <Modal :show="showModal"  @Cancel="showModal = false" @Save="Save()">
        <template #header>
            <h3>{{modal.title}}</h3>
        </template>
        <template #body>
            <label>รหัสสินค้า</label><br>
            <input type="text" value="ITEM-XXXX" disabled><br>
            <label>ชื่อสินค้า</label><br>
            <input type="text" v-model="modal.name"  autocomplete="off"><br>
            <label>ราคา</label><br>
            <input type="number" v-model="modal.price" autocomplete="off"><br>
            <label>หน่วย</label><br>
            <Dropdown v-model="modal.select" :options="unitOptions" />
            <br><br>
        </template>
    </Modal>
</div>
</template>
<script>
import { GetAllUnit, GetAllItem, InsertItem, UpdateItem, DeleteItem } from '@/helpers/api.js'
import Dropdown from '@/components/Dropdown.vue'
import Modal from '@/components/Modal.vue'
import Slide from '@/components/Slide.vue'
import SlideItem from '@/components/SlideItem.vue'
import ItemDetail from '@/components/ItemDetail.vue'



export default {
    components : {
        Dropdown,
        Modal,
        Slide,
        SlideItem,
        ItemDetail
    },
    data (){
        return {
            item : [],
            unit : [],
            showModal : false,
            showSlide : false,
            selectIndex : null,
            modal : {
                title : '',
                name : '',
                select : '',
                price : '',
                id : '',
            },
        }
    },
    computed : {
        unitOptions(){
            return this.unit.map(x => {
                return {
                    value : x.unit_id,
                    text : x.unit_name
                }
            })
        },
        selectedItem(){
            return this.item[this.selectIndex] ?? {item_unit : {}}
        }
    },
    mounted : function() {
        this.GetItem();
        this.GetUnit();
    },
    methods : {
        async GetItem(){
            const result = await GetAllItem();
            this.item = result;           
        },

        async GetUnit(){
            const result = await GetAllUnit();
            this.unit = result;
        },

        OpenModal(item){
            this.modal.title = ''
            this.modal.name = ''
            this.modal.select = ''
            this.modal.price = ''
            this.modal.id = ''
            if(item === undefined || item === null){
                this.modal.title = 'เพิ่มสินค้า'
            }else{
                this.title = 'แก้ไขสินค้า'
                this.modal.name = item.item_name
                this.modal.select = item.item_unit.unit_id
                this.modal.price = item.item_price
                this.modal.id = item.item_id
            }
            this.showModal = true;
        },


        Save(){
            if(this.modal.id === ''){
                this.Insert(this.modal.name, this.modal.price, this.modal.select)
            }else{
                this.Update(this.modal.name, this.modal.price, this.modal.select, this.modal.id)
            }
            this.showModal = false;
        },

        async Insert(name, price, unit_id){
            let check = 0;

            if(name == ''){
                check = 1;
            }else if(price == ''){
                check = 1;
            }else if(unit_id == ''){
                check = 1;
            }
            if(check == 1){
                alert('no value');
            }else{
                const result = await InsertItem(name, price, unit_id)
                if(result.status_code == -1){
                    alert(result.message);
                }else{
                    alert(result.message);
                    this.showModal = false;
                }
                this.GetItem();
            }
        },

        async Update(name, price, unit_id, item_id){
            const check = 0;

            if(name == ''){
                check = 1;
            }else if(price == ''){
                check = 1;
            }
            
            if(check == 1){
                alert('no value');
            }else{
                const result = await UpdateItem(name, price, unit_id, item_id)
                if(result.status_code == -1){
                    alert(result.message);
                }else{
                    this.GetItem();
                    alert(result.message);
                    this.showModal = false;
                }
            }
        },

        async Delete(id)
        {
            const result = await DeleteItem(id)
            if(result.status_code == -1){
                alert(result.message);
            }else{
                this.GetItem();
                alert(result.message);
                this.showSlide = false;
            }
        },
        Next(){
            if(this.selectIndex < this.item.length -1){
                this.selectIndex++;
            }
        },
        Previous(){
            if(this.selectIndex > 0){
                this.selectIndex--;
            }
        },
        SlideOn(index){
            this.selectIndex = index,
            this.showSlide = true;
        },



    }
}
</script>