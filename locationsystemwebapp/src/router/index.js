import { createRouter, createWebHistory } from 'vue-router'
import { permissionService } from '@/api/services'

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
    meta: { title: '牙医管理', permission: 'dentist:view' }
  },
  {
    path: '/patients',
    name: 'Patients',
    component: () => import('@/views/patients/PatientsView.vue'),
    meta: { title: '患者管理', permission: 'patient:view' }
  },
  {
    path: '/appointments',
    name: 'Appointments',
    component: () => import('@/views/appointments/AppointmentsView.vue'),
    meta: { title: '预约管理', permission: 'appointment:view' }
  },
  {
    path: '/dentaloffices',
    name: 'DentalOffices',
    component: () => import('@/views/dentalOffices/DentalOfficesView.vue'),
    meta: { title: '牙科诊所管理', permission: 'dental-office:view' }
  },
  {
    path: '/403',
    name: '403',
    component: () => import('@/views/403.vue'),
    meta: { title: '无权限' }
  }
]

// 创建路由实例
const router = createRouter({
  history: createWebHistory(),
  routes
})

// 路由守卫
router.beforeEach(async (to, from, next) => {
  // 检查是否需要权限
  if (to.meta.permission) {
    try {
      // 获取用户权限
      const userPermissions = await getUserPermissions()
      
      // 检查权限
      if (userPermissions.includes(to.meta.permission)) {
        next()
      } else {
        next({ name: '403' })
      }
    } catch (error) {
      console.error('获取权限失败:', error)
      next({ name: '403' })
    }
  } else {
    next()
  }
})

// 获取用户权限
async function getUserPermissions() {
  try {
    // 尝试从localStorage获取权限
    const cachedPermissions = localStorage.getItem('userPermissions')
    if (cachedPermissions) {
      return JSON.parse(cachedPermissions)
    }
    
    // 从后端获取权限
    const response = await permissionService.getUserMenus()
    
    // 提取权限代码
    const permissions = []
    const extractPermissions = (menus) => {
      menus.forEach(menu => {
        if (menu.code) {
          permissions.push(menu.code)
        }
        if (menu.children && menu.children.length > 0) {
          extractPermissions(menu.children)
        }
      })
    }
    
    extractPermissions(response)
    
    // 存储到localStorage
    localStorage.setItem('userPermissions', JSON.stringify(permissions))
    
    return permissions
  } catch (error) {
    console.error('获取用户权限失败:', error)
    return []
  }
}

export default router
