<template>
<div>
    <h2>จัดการสินค้า</h2>
    <button @click="OpenModal()">เพิ่มสินค้า</button>
    <br><br>
    <table class="table table-striped table-bordered">
        <thead>
            <tr>
                <th width='5%'>No.</th>
                <th width='15%'>รหัสสินค้า</th>
                <th>ชื่อสินค้า</th>
                <th width='10%'>หน่วย</th>
                <th style='text-align:right'>ราคา</th>
                <th width='20%' style='text-align:center'>ดำเนินการ</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="(item, index) in item" :key="item.item_id" > 
                <td>{{index+1}}</td>
                <td>{{item.item_code}}</td>
                <td>{{item.item_name}}</td>
                <td>{{item.item_unit.unit_name}}</td>
                <td>{{item.item_price}}</td>
                <td>
                    <button @click="OpenModal(item)">แก้ไข</button>&nbsp; 
                    <button @click="Delete(item.item_id)">ลบ</button>
                </td>
            </tr>
        </tbody>
    </table>
    <div v-if="show" class="modal-mask" ref="newModal">
		<div class="modal-wrapper">
			<div class="modal-container">
				<div class="modal-header">
					<slot name="header"><h3>{{modal.title}}</h3></slot>
				</div>
				<div class="modal-body">
					<slot name="body">
                        <label>รหัสสินค้า</label><br>
                        <input type="text" value="ITEM-XXXX"><br>
                        <label>ชื่อสินค้า</label><br>
                        <input type="text" v-model="modal.name"  autocomplete="off"><br>
                        <label>ราคา</label><br>
                        <input type="number" v-model="modal.price" autocomplete="off"><br>
                        <label>หน่วย</label><br>
                        <Dropdown v-model="modal.select" :options="unitOptions" />
					</slot>
				</div>
				<div class="modal-footer">
					<slot name="footer">
						<button class="modal-default-button" @click="show = false">ยกเลิก</button>
						<button class="modal-default-button" @click="Save()">บันทึก</button>
					</slot>
				</div>
			</div>
		</div>
    </div>
</div>
</template>
<script>
import { GetAllUnit, GetAllItem, InsertItem, UpdateItem, DeleteItem } from '@/helpers/api.js'
import Dropdown from '@/components/Dropdown.vue'

export default {
    components : {
        Dropdown,
    },
    data (){
        return {
            item : [],
            unit : [],
            show : false,
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
            this.show = true;
        },


        Save(){
            if(this.modal.id === ''){
                this.Insert(this.modal.name, this.modal.price, this.modal.select)
            }else{
                this.Update(this.modal.name, this.modal.price, this.modal.select, this.modal.id)
            }
            this.show = false;
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
                    this.show = false;
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
                    this.show = false;
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
            }
        }



    }
}
</script>