import { createRouter, createWebHistory } from 'vue-router'

// 懒加载页面组件
const routes = [
    {
        path: '/',
        redirect: '/dentists',
        meta: {

        }
    },

    {
        path: '/dental-offices',
        name: 'DentalOffices',
        component: () => import('../views/DentalOfficesView.vue'),
        meta: {
            title: '牙科诊所管理',

        }
    },
    {
        path: '/patients',
        name: 'Patients',
        component: () => import('../views/PatientsView.vue'),
        meta: {
            title: '患者管理',

        }
    },
    {
        path: '/dentists',
        name: 'Dentists',
        component: () => import('../views/DentistsView.vue'),
        meta: {
            title: '牙医管理',
        }
    },
    {
        path: '/appointments',
        name: 'Appointments',
        component: () => import('../views/AppointmentsView.vue'),
        meta: {
            title: '预约管理',
        }
    },

]

const router = createRouter({
    history: createWebHistory(),
    routes
})

// 路由前置守卫，仅设置页面标题
router.beforeEach((to, from, next) => {
    document.title = to.meta.title ? `${to.meta.title} - 内容管理系统` : '内容管理系统'
    next()
})

export default router
