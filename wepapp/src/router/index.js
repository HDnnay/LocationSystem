import { createRouter, createWebHistory } from 'vue-router'

// 懒加载页面组件
const Home = () => import('../views/Home.vue')
const Dashboard = () => import('../views/Dashboard.vue')
const Settings = () => import('../views/Settings.vue')

const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home,
        meta: {
            title: '首页'
        }
    },
    {
        path: '/dashboard',
        name: 'Dashboard',
        component: Dashboard,
        meta: {
            title: '仪表盘'
        }
    },
    {
        path: '/settings',
        name: 'Settings',
        component: Settings,
        meta: {
            title: '系统设置'
        }
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

// 路由前置守卫，设置页面标题
router.beforeEach((to, from, next) => {
    document.title = to.meta.title ? `${to.meta.title} - 内容管理系统` : '内容管理系统'
    next()
})

export default router