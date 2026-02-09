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
        path: '/login',
        name: 'Login',
        component: () => import('../views/auth/LoginView.vue'),
        meta: {
            title: '用户登录',
            requiresAuth: false
        }
    },
    {
        path: '/register',
        name: 'Register',
        component: () => import('../views/auth/RegisterView.vue'),
        meta: {
            title: '用户注册',
            requiresAuth: false
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
  },
  {
    path: '/company',
    name: 'company',
    component: () => import('../views/company/CompanyView.vue'),
    meta: {
      title: '信息管理',
      requiresAuth: true
    }
  },
  {
    path: '/company/provice',
    name: 'company_provice',
    component: () => import('../views/company/CompanyProviceView.vue'),
    meta: {
      title: '统计管理',
      requiresAuth: true
    }
  },
  {
    path: '/rent',
    name: 'rent',
    component: () => import('../views/renthouse/RentHouseView.vue'),
    meta: {
      title: '租房管理',
      requiresAuth: true
    }
  },
  {
    path: '/rent/create',
    name: 'create-rent',
    component: () => import('../views/renthouse/CreateRentHouseView.vue'),
    meta: {
      title: '租房管理',
      requiresAuth: true
    }
  }

]

const router = createRouter({
    history: createWebHistory(),
    routes
})

// 路由前置守卫
router.beforeEach((to, from, next) => {
    document.title = to.meta.title ? `${to.meta.title} - 内容管理系统` : '内容管理系统'

    // 检查是否需要认证
    if (to.meta.requiresAuth !== false) {
        // 获取token
        const token = localStorage.getItem('access_token')
        if (!token) {
            // 没有token，跳转到登录页
            next({ path: '/login' })
            return
        }
    }

    // 已经登录，或者不需要认证的页面
    next()
})

export default router
