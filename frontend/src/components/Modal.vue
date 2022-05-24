<template>
    <div v-if="show" class="modal-mask">
        <div class="modal-wrapper">
        <div class="modal-container">
            <div class="modal-header">
                <slot name="header">
                    
                </slot>
            </div>
            <div class="modal-body">
                <slot name="body">


                </slot>
            </div>
            <div class="modal-footer">
                <slot name="footer">
                
                </slot>
            </div>
        </div>
        </div>
    </div>
</template>
<script>
export default {
    props: {
      show: Boolean
    }
}
</script>
<style>
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
    display: flex;
    align-items:center;
    justify-content:center;
    height:100%;
}

.modal-container {
  width: 800px;
  margin: 0px auto;
  padding: 50px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.33);
  overflow-y: auto;
  transition: all 0.3s ease;
  max-height: 500px; 
  display:inline-block;
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