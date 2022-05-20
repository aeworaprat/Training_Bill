<template id="test">
<div>
    <h2>จัดการหน่วย</h2>
    <button @click="OpenModal()">เพิ่มหน่วย</button>
    <br><br>
	<table class="table">
		<thead>
			<tr>
				<th width='10%'>No.</th>
				<th>ชื่อ</th>
				<th width='15%'>ดำเนินการ</th>
			</tr>
		</thead>
		<tbody>
			<tr v-for="(unit, index) in unit" :key="index" > 
				<td>{{index+1}}</td>
				<td>{{unit.unit_name}}</td>
				<td>
				<button @click="OpenModal(unit)">แก้ไข</button>&nbsp; 
				<button @click="Delete(unit.unit_id)">ลบ</button>
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
						<label>ชื่อหน่วย</label><br>
						<input type="text" v-model="modal.val">
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
import { GetAllUnit, InsertUnit, DeleteUnit, UpdateUnit } from '@/helpers/api.js'
import Modal from '@/components/Modal.vue'

export default {
	name: 'Unit',
  components : {
    Modal
  },
	data(){
		return { 
			unit : [], 
			show : false,
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

methods : {
  async GetUnit(){
    const result = await GetAllUnit();
    this.unit = result;
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
    this.show = true
  },

  Save(){
    if(this.modal.id === ''){
      this.Insert(this.modal.val)
    }else{
      this.Update(this.modal.id, this.modal.val)
    }
    this.show = false
  }
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

.modal-fader {
	display: none;
	position: fixed;
	top: 0;
	left: 0;
	right: 0;
	bottom: 0;
	width: 100%;
	z-index: 99998;
	background: rgba(0,0,0,0.8);
}
.modal-fader.active {
	display: block;
}

.modal-window {
	display: none;
	position: absolute;
	left: 50%;
	transform: translateX(-40%);
	z-index: 99999;
	background: #fff;
  width: 300px;
	padding: 50px;
	border-radius: 5px;
	font-family: sans-serif;
	top: 50px;
}
.modal-window.active {
	display: block;
}

.modal-mask {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
  transition: opacity 0.3s ease;
}

.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.modal-container {
  width: 500px;
  margin: 0px auto;
  padding: 50px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  transition: all 0.3s ease;
}

.modal-header h3 {
  margin-top: 0;
}

.modal-body {
  margin: 20px 0;
}

.modal-default-button {
  float: right;
}


.modal-enter-from {
  opacity: 0;
}

.modal-leave-to {
  opacity: 0;
}

.modal-enter-from .modal-container,
.modal-leave-to .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}
</style>
