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
                        <td @click="SlideOn(index)" :class="visibel === index ? 'active_row' : ''">{{item.item_code}}</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="column">
            <Slide :showSlide="showSlide" @Next="Next()" @Previous="Previous()">
                <SlideItem v-for="(item, index) in item" :key="index" :index="index" :visible="visibel">
                    <div class="card">
                        <div class="container">
                        <h3>Item detail</h3>
                        <b>รหัสสินค้า</b><br>
                        {{item.item_code}}<br><br>
                        <b>ชื่อสินค้า</b><br>
                        {{item.item_name}}<br><br>
                        <b>ราคา</b><br>
                        {{item.item_price}}<br><br>
                        <b>หน่วย</b><br>
                        {{item.item_unit.unit_name}}
                        <br>
                        <br>
                        <div style="text-align:center;">
                        <button @click="OpenModal(item)">Update</button>
                        <button @click="Delete(item.item_id)">Delete</button>
                        </div>
                        </div>
                    </div>
                </SlideItem>
            </Slide>
        </div>
    </div>
    <Modal :show="showModal">
        <template #header>
            <h3>{{modal.title}}</h3>
        </template>
        <template #body>
            <label>รหัสสินค้า</label><br>
            <input type="text" value="ITEM-XXXX"><br>
            <label>ชื่อสินค้า</label><br>
            <input type="text" v-model="modal.name"  autocomplete="off"><br>
            <label>ราคา</label><br>
            <input type="number" v-model="modal.price" autocomplete="off"><br>
            <label>หน่วย</label><br>
            <Dropdown v-model="modal.select" :options="unitOptions" />
            <br><br>
        </template>
        <template #footer>
            <button class="modal-default-button" @click="showModal = false">ยกเลิก</button>
			<button class="modal-default-button" @click="Save()">บันทึก</button>
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


export default {
    components : {
        Dropdown,
        Modal,
        Slide,
        SlideItem
    },
    data (){
        return {
            item : [],
            unit : [],
            showModal : false,
            showSlide : false,
            visibel : '',
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
            if(this.visibel < this.item.length -1){
                this.visibel++;
            }
            console.log('next')
        },
        Previous(){
            if(this.visibel > 0){
                this.visibel--;
            }
            console.log('pre')
        },
        SlideOn(index){
            this.visibel = index,
            this.showSlide = true;
        },



    }
}
</script>