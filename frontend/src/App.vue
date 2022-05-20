<template>
  <div id="app">
    <nav class="mainNav">
      <router-link :to="{ name: 'unit'}">หน่วย</router-link>
      <router-link :to="{ name: 'item'}">สินค้า</router-link>
      <router-link :to="{ name: 'receipt-view'}">ดูบิล</router-link>
      <router-link :to="{ name: 'receipt'}">สร้างบิล</router-link>
    </nav>
    <router-view/>
  </div>
</template>
<style>
#app {
  max-width: 1200px;
  margin: auto;
}
nav.mainNav {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
 nav.mainNav > * {
  flex: 1;
  display: flex;
  justify-content: center;
  text-decoration: none;
}
nav.mainNav > *:nth-last-child(n+2) {
	border-right: 1px solid #aaa;
}
</style>
