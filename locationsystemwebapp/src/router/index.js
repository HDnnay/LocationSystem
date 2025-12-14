import { createRouter, createWebHistory } from 'vue-router'

// 定义路由
const routes = [
  {
    path: '/',
    redirect: '/dentists',
    meta: { title: '首页' }
  },
  {
    path: '/dentists',
    name: 'Dentists',
    component: () => import('@/views/dentists/DentistsView.vue'),
    meta: { title: '牙医管理' }
  },
  {
    path: '/patients',
    name: 'Patients',
    component: () => import('@/views/patients/PatientsView.vue'),
    meta: { title: '患者管理' }
  },
  {
    path: '/appointments',
    name: 'Appointments',
    component: () => import('@/views/appointments/AppointmentsView.vue'),
    meta: { title: '预约管理' }
  },
  {
    path: '/dentaloffices',
    name: 'DentalOffices',
    component: () => import('@/views/dentalOffices/DentalOfficesView.vue'),
    meta: { title: '牙科诊所管理' }
  }
]

// 创建路由实例
const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
