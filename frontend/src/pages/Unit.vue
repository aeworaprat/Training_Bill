<template id="test">
<div>
  <h2>จัดการหน่วย</h2>
  <button @click="OpenModal()">เพิ่มหน่วย</button>
  <br><br>
  <div class="row">
    <div class="column">
      <table class="table">
        <thead>
          <tr>
            <th width='5%'>No.</th>
            <th width='15%'>ชื่อ</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(unit, index) in units" :key="index" > 
            <td>{{index+1}}</td>
            <td @click="SlideOn(index)" :class="selectIndex === index ? 'active_row' : ''">{{unit.unit_name}}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="column">
    <Slide  :showSlide="showSlide" @Next="Next()" @Previous="Previous()">
      <div class="container">
        <h3>Unit detail</h3>
        <b>ชื่อหน่วย</b><br>
        {{selectedUnit.unit_name}}
        <br>
        <br>
        <div style="text-align:center;">
          <button @click="OpenModal(selectedUnit)">Update</button>
          <button @click="Delete(selectedUnit.unit_id)">Delete</button>
        </div>
      </div>
    </Slide>
    </div>
  </div>
  <Modal :show="showModal" @Cancel="showModal = false" @Save="Save()">
    <template #header>
      <h3>{{modal.title}}</h3>
    </template>
    <template #body>
      <label>ชื่อหน่วย</label><br>
      <input type="text" v-model="modal.val" >
    </template>
  </Modal>
</div>
</template>

<script lang="ts">

import { defineComponent, computed, ref, onMounted, reactive } from '@vue/composition-api'
import { GetAllUnit, InsertUnit, DeleteUnit, UpdateUnit } from '@/helpers/api'
import Modal from '@/components/Modal.vue'
import Slide from '@/components/Slide.vue'

interface IUnit {
  unit_id : number
  unit_name : string
}

interface IModal {
  title : string
  val : string
  id : number
}


export default defineComponent({
	name: 'Unit',
  components : {
    Modal,
    Slide,
  },
  setup(){
    const units = ref<IUnit[]>();
    const showSlide = ref<boolean>(false);
    const showModal= ref<boolean>(false);
    const selectIndex = ref<number>();
    const modal: IModal = reactive({
      title: "",
      val : "",
      id : undefined,
    })

    onMounted(() => {
      GetUnit()
    })
    

    const selectedUnit = computed(() => {
      if(selectIndex.value != undefined){
        return units.value[selectIndex.value] ?? {}
      }else{
        return {};
      }
    })
    

    async function GetUnit(){
      const result = await GetAllUnit();
      units.value =  result;
    }

    function SlideOn(index : number){
      selectIndex.value = index,
      showSlide.value = true;
    }

    function Next(){
      if(selectIndex.value < units.value.length -1){
        selectIndex.value++;
      }
    }
    function Previous(){
      if(selectIndex.value > 0){
        selectIndex.value--;
      }
    }

    function  OpenModal(unit : IUnit){
      modal.title = ""
      modal.val = ''
      modal.id = undefined
      if(unit === undefined || unit === null){
        modal.title = "เพิ่มหน่วย"
        
      }else{
        modal.title = "แก้ไขหน่วย"
        modal.val = unit.unit_name
        modal.id = unit.unit_id
      }
      showModal.value = true
    }

    async function Insert(name : string){
      if(name == ''){
        alert('no value');
      }else{
        const result = await InsertUnit(name)
        if(result.status_code == -1){
        alert(result.message)
        }else{
          GetUnit()
          alert(result.message)
        }
      }	
    }

    async function Update(id : number, name : string){
      if(name == ''){
        alert('no value');
      }else{
        const result = await UpdateUnit(id, name)
        if(result.status_code == -1){
          alert(result.message);
        }else{
          GetUnit();
          alert(result.message);
        }
      }
    }

    async function Delete(id : number){
      const result = await DeleteUnit(id)
      if(result.status_code == -1){
        alert(result.message)
      }else{
        GetUnit()
        alert(result.message)
        showSlide.value = false
      }
    }

    function Save(){
      if(modal.id === undefined){
        Insert(modal.val)
      }else{
        Update(modal.id, modal.val)
      }
      showModal.value = false
    }


    return { 
      units, 
      showSlide,
      modal,
      selectIndex,
      selectedUnit,
      showModal,
      SlideOn,
      OpenModal,
      Delete,
      Save,
      Next,
      Previous,
    }
  },
});
</script>
<style >
table {
    border-collapse: collapse;
    width: 100%;
}

td, th {
    border: 1px solid #262626;
    text-align: left;
    padding: 8px;
}

.add{
  float: right;
}
.column {
  float: left;
  width: 45%;
  padding: 10px;
  height: 300px; /* Should be removed. Only for demonstration */
}

/* Clear floats after the columns */
.row:after {
  content: "";
  display: table;
  clear: both;
}
.container {
  padding: 2px 16px;
  border-style: solid;
  border-color: black;
}
.active_row {
  color:red
}
.container {
    padding: 2px 16px;
    border-style: solid;
    border: 2px solid black
}
</style>
