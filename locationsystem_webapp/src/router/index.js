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
  {
    path: '/company',
    name: 'company',
    component: () => import('../views/company/CompanyView.vue'),
    meta: {
      title: '信息管理',
    }
  },
  {
    path: '/company/provice',
    name: 'company_provice',
    component: () => import('../views/company/CompanyProviceView.vue'),
    meta: {
      title: '统计管理',
    }
  },
  {
    path: '/rent',
    name: 'rent',
    component: () => import('../views/renthouse/RentHouseView.vue'),
    meta: {
      title: '租房管理',
    }
  },
  {
    path: '/rent/create',
    name: 'create-rent',
    component: () => import('../views/renthouse/CreateRentHouseView.vue'),
    meta: {
      title: '租房管理',
    }
  }

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
