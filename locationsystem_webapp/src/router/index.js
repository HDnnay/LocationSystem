import { createRouter, createWebHistory } from 'vue-router'

// 懒加载页面组件
const routes = [
    {
        path: '/',
        redirect: '/dentists',
        meta: {
            requiresAuth: true
        }
    },
    {
        path: '/roles',
        name: 'Roles',
        component: () => import('../views/Roles.vue'),
        meta: {
            title: '角色管理',
            requiresAuth: true
        }
    },
    {
        path: '/dental-offices',
        name: 'DentalOffices',
        component: () => import('../views/DentalOfficesView.vue'),
        meta: {
            title: '牙科诊所管理',
            requiresAuth: true
        }
    },
    {
        path: '/patients',
        name: 'Patients',
        component: () => import('../views/PatientsView.vue'),
        meta: {
            title: '患者管理',
            requiresAuth: true
        }
    },
    {
        path: '/dentists',
        name: 'Dentists',
        component: () => import('../views/DentistsView.vue'),
        meta: {
            title: '牙医管理',
            requiresAuth: true
        }
    },
    {
        path: '/appointments',
        name: 'Appointments',
        component: () => import('../views/AppointmentsView.vue'),
        meta: {
            title: '预约管理',
            requiresAuth: true
        }
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

// 路由前置守卫，设置页面标题和认证检查
router.beforeEach((to, from, next) => {
    document.title = to.meta.title ? `${to.meta.title} - 内容管理系统` : '内容管理系统'

    // 检查路由是否需要认证
    if (to.matched.some(record => record.meta.requiresAuth)) {
        // 检查是否已登录
        const isLoggedIn = localStorage.getItem('isLoggedIn') === 'true'
        if (!isLoggedIn) {
            // 未登录则跳转到登录页
            next({ path: '/login', query: { redirect: to.fullPath } })
        } else {
            next()
        }
    } else {
        next()
    }
})

export default router