import { createRouter, createWebHistory } from 'vue-router'

// 懒加载页面组件
const Home = () => import('../views/Home.vue')
const Dashboard = () => import('../views/Dashboard.vue')
const Settings = () => import('../views/Settings.vue')
const Dentists = () => import('../views/dentists/index.vue')
const Patients = () => import('../views/patients/index.vue')
const DentalOffices = () => import('../views/dental-offices/index.vue')
const Appointments = () => import('../views/appointments/index.vue')

const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home,
        meta: {
            title: 'Home'
        }
    },
    {
        path: '/dashboard',
        name: 'Dashboard',
        component: Dashboard,
        meta: {
            title: 'Dashboard'
        }
    },
    {
        path: '/settings',
        name: 'Settings',
        component: Settings,
        meta: {
            title: 'Settings'
        }
    },
    {
        path: '/dentists',
        name: 'Dentists',
        component: Dentists,
        meta: {
            title: 'Dentist Management'
        }
    },
    {
        path: '/patients',
        name: 'Patients',
        component: Patients,
        meta: {
            title: 'Patient Management'
        }
    },
    {
        path: '/dental-offices',
        name: 'DentalOffices',
        component: DentalOffices,
        meta: {
            title: 'Dental Office Management'
        }
    },
    {
        path: '/appointments',
        name: 'Appointments',
        component: Appointments,
        meta: {
            title: 'Appointment Management'
        }
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

// 路由前置守卫，设置页面标题
router.beforeEach((to, from, next) => {
    document.title = to.meta.title ? `${to.meta.title} - Admin System` : 'Admin System'
    next()
})

export default router