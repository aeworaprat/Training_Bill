import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import Unit from '@/pages/Unit.vue';

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'home',
    component: Unit
  },
  {
    path: '/unit',
    name: 'unit',
    component: () => import('@/pages/Unit.vue')
  },
  {
    path: '/item',
    name: 'item',
    component: () => import('@/pages/Item.vue')
  },
  {
    path: '/receipt-view',
    name: 'receipt-view',
    component: () => import('@/pages/ReceiptView.vue')
  },
  {
    path: '/receipt',
    name: 'receipt',
    component: () => import('@/pages/Receipt.vue')
  },
]

const router = new VueRouter({
  routes
})

export default router
