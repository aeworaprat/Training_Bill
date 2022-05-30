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
                    <tr v-for="(item, index) in items" :key="index" > 
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
<script lang="ts">
// import { GetAllUnit } from '@/helpers/foo';
import { GetAllUnit,  GetAllItem, InsertItem, UpdateItem, DeleteItem } from '@/helpers/api'
import { defineComponent, computed, ref, onMounted, reactive } from '@vue/composition-api'
import Dropdown from '@/components/Dropdown.vue'
import Modal from '@/components/Modal.vue'
import Slide from '@/components/Slide.vue'
import ItemDetail from '@/components/ItemDetail.vue'

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

interface IModal {
    title : string,
    name : string,
    select : number,
    price : number,
    id : number,
}

export default defineComponent({
    components : {
        Dropdown,
        Modal,
        Slide,
        ItemDetail
    },
    setup(){
        const items = ref<IItem[]>();
        const unit = ref<IUnit[]>();
        const showModal = ref<boolean>(false)
        const showSlide = ref<boolean>(false)
        const selectIndex = ref<number>(undefined)
        const modal: IModal = reactive({
            title : '',
            name : '',
            select : undefined,
            price : undefined,
            id : undefined,
        });
        
        onMounted(() => {
            GetItem();
            GetUnit();
        })

        const selectedItem = computed(() => {
            if(selectIndex.value != undefined){
                return items.value[selectIndex.value] ?? {item_unit : {}}
            }else{
                return {}
            }
        })

        const unitOptions = computed(() =>{
            return unit.value.map(x => {
                return {
                    value : x.unit_id,
                    text : x.unit_name
                }
            })
        })

        async function GetUnit() {
            const result = await GetAllUnit();
            unit.value = result;
        }

        async function GetItem() {
            const result = await GetAllItem();
            items.value = result;         
        }

        function SlideOn(index : number){
                selectIndex.value = index,
                showSlide.value = true;
        }

        function Next(){
            if(selectIndex.value < items.value.length -1){
                selectIndex.value++;
            }
        }

        function Previous(){
            if(selectIndex.value > 0){
            selectIndex.value--;
            }
        }

        function OpenModal(item : IItem){
            modal.title = ''
            modal.name = ''
            modal.select = undefined
            modal.price = undefined
            modal.id = undefined
            if(item === undefined || item === null){
                modal.title = 'เพิ่มสินค้า'
            }else{
                modal.title = 'แก้ไขสินค้า'
                modal.name = item.item_name
                modal.select = item.item_unit.unit_id
                modal.price = item.item_price
                modal.id = item.item_id
            }
            showModal.value = true;
        }

        function Save(){
            if(modal.id === undefined){
                Insert(modal.name, modal.price, modal.select)
            }else{
                Update(modal.name, modal.price, modal.select, modal.id)
            }
            showModal.value = false;
        }

        async function Insert(name : string, price : number, unit_id : number){
            let check = 0;

            if(name == ''){
                check = 1;
            }else if(price == undefined){
                check = 1;
            }else if(unit_id == undefined){
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
                    showModal.value = false;
                }
                GetItem();
            }
        }

        async function Update(name : string, price : number, unit_id : number, item_id : number){
            let check = 0;

            if(name == ''){
                check = 1;
            }else if(price == undefined){
                check = 1;
            }
            
            if(check == 1){
                alert('no value');
            }else{
                const result = await UpdateItem(name, price, unit_id, item_id)
                if(result.status_code == -1){
                    alert(result.message);
                }else{
                    GetItem();
                    alert(result.message);
                    showModal.value = false;
                }
            }
        }

        async function Delete(id : number)
        {
            const result = await DeleteItem(id)
            if(result.status_code == -1){
                alert(result.message);
            }else{
                GetItem();
                alert(result.message);
                showSlide.value = false;
            }
        }

        return { 
            items,
            unit,
            showModal,
            showSlide,
            selectIndex,
            modal,
            selectedItem,
            unitOptions,
            SlideOn,
            Next,
            Previous,
            OpenModal,
            Save,
            Delete
        }
    }
});
</script>