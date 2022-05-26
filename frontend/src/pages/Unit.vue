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

<script>
import { GetAllUnit, InsertUnit, DeleteUnit, UpdateUnit } from '@/helpers/api.js'
import Modal from '@/components/Modal.vue'
import Slide from '@/components/Slide.vue'
import SlideItem from '@/components/SlideItem.vue'

export default {
	name: 'Unit',
  components : {
    Modal,
    Slide,
    SlideItem
  },
	data(){
		return { 
			units : [], 
			showModal : false,
      showSlide : false,
      selectIndex : null,
			modal : { 
        title : null,
        val : null,
        id : null,
      }
		}
	},
	mounted: function () {
		this.GetUnit()
	},
  computed : {
    selectedUnit(){
      return this.units[this.selectIndex] ?? {}
      
    }
  },
  methods : {
    async GetUnit(){
      const result = await GetAllUnit();
      this.units = result;
    },

    async Insert(name){
      if(name == ''){
        alert('no value');
      }else{
        const result = await InsertUnit(name)
        if(result.status_code == -1){
        alert(result.message)
        }else{
          this.GetUnit()
          alert(result.message)
        }
      }	
    },

    async Update(id, name){
      if(name == ''){
        alert('no value');
      }else{
        const result = await UpdateUnit(id, name)
        if(result.status_code == -1){
          alert(result.message);
        }else{
          this.GetUnit();
          alert(result.message);
        }
      }
    },

    async Delete(id){
      const result = await DeleteUnit(id)
      if(result.status_code == -1){
        alert(result.message)
      }else{
        this.GetUnit()
        alert(result.message)
        this.showSlide = false
      }
    },

    OpenModal(unit){
      this.modal.title = ''
      this.modal.val = ''
      this.modal.id = ''
      if(unit === undefined || unit === null){
        this.modal.title = 'เพิ่มหน่วย'
        
      }else{
        this.modal.title = 'แก้ไขหน่วย'
        this.modal.val = unit.unit_name
        this.modal.id = unit.unit_id
      }
      this.showModal = true
    },

    Save(){
      if(this.modal.id === ''){
        this.Insert(this.modal.val)
      }else{
        this.Update(this.modal.id, this.modal.val)
      }
      this.showModal = false
    },

    Next(){
      if(this.selectIndex < this.units.length -1){
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
};
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
