import { createRouter, createWebHistory } from 'vue-router'
import { getUserPermissions as apiGetUserPermissions } from '../api/permissions'
import authStorage from '../utils/authStorage'

// 懒加载页面组件
const routes = [
    {
        path: '/',
        redirect: '/admin/dashboard',
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
        path: '/admin/dashboard',
        name: 'Dashboard',
        component: () => import('../views/DashboardView.vue'),
        meta: {
            title: '仪表盘',
            requiresAuth: true
        }
    },
  {
    path: '/admin/role-permissions',
    name: 'RolePermissions',
    component: () => import('../views/SystemView.vue'),
    meta: {
      title: '角色权限管理',
      requiresAuth: true
    }
  },
  {
    path: '/admin/company',
    name: 'company',
    component: () => import('../views/company/CompanyView.vue'),
    meta: {
      title: '公司管理',
      requiresAuth: true
    }
  },
  {
    path: '/admin/company/list',
    name: 'company_list',
    component: () => import('../views/company/CompanyView.vue'),
    meta: {
      title: '公司列表',
      requiresAuth: true
    }
  },
  {
    path: '/admin/company/statistics',
    name: 'company_statistics',
    component: () => import('../views/company/CompanyProviceView.vue'),
    meta: {
      title: '统计管理',
      requiresAuth: true
    }
  },
  {
    path: '/admin/rent',
    name: 'rent',
    component: () => import('../views/renthouse/RentHouseView.vue'),
    meta: {
      title: '租房管理',
      requiresAuth: true
    }
  },
  {
    path: '/admin/rent/list',
    name: 'rent_list',
    component: () => import('../views/renthouse/RentHouseView.vue'),
    meta: {
      title: '租房列表',
      requiresAuth: true
    }
  },
  {
    path: '/admin/rent/create',
    name: 'create-rent',
    component: () => import('../views/renthouse/CreateRentHouseView.vue'),
    meta: {
      title: '租房创建',
      requiresAuth: true
    }
  },
  {
    path: '/admin/roles',
    name: 'Roles',
    component: () => import('../views/roles/RolesView.vue'),
    meta: {
      title: '角色管理',
      requiresAuth: true,
      permission: 'role:view'
    }
  },
  {
    path: '/admin/permissions',
    name: 'Permissions',
    component: () => import('../views/permissions/PermissionsView.vue'),
    meta: {
      title: '权限管理',
      requiresAuth: true,
      permission: 'permission:view'
    }
  },
  {
    path: '/admin/users',
    name: 'Users',
    component: () => import('../views/users/UsersView.vue'),
    meta: {
      title: '用户管理',
      requiresAuth: true
    }
  },
  {
    path: '/admin/menus',
    name: 'Menus',
    component: () => import('../views/menus/MenusView.vue'),
    meta: {
      title: '菜单管理',
      requiresAuth: true
    }
  },
  {
    path: '/403',
    name: '403',
    component: () => import('../views/403View.vue'),
    meta: {
      title: '无权限',
      requiresAuth: false
    }
  }

]

const router = createRouter({
    history: createWebHistory(),
    routes
})

// 获取用户权限
async function getUserPermissions() {
    try {
        // 从后端获取权限
        const response = await apiGetUserPermissions();
        console.log("用户权限响应", response)

        // 检查响应状态码
        if (response.status === 200) {
            const permissions = response.data
            // 存储到localStorage
            localStorage.setItem('userPermissions', JSON.stringify(permissions))
            return permissions || []
        } else {
            console.error(`获取用户权限失败，状态码: ${response.status}`)
            localStorage.setItem('userPermissions', JSON.stringify([]))
            return []
        }
    } catch (error) {
        console.error('获取用户权限失败:', error)
        // 错误时返回空数组，不使用默认权限
        localStorage.setItem('userPermissions', JSON.stringify([]))
        return []
    }
}

// 路由前置守卫
router.beforeEach(async (to, from, next) => {
    document.title = to.meta.title ? `${to.meta.title} - 内容管理系统` : '内容管理系统'

    // 检查是否需要认证
    if (to.meta.requiresAuth !== false) {
        // 获取token
        const token = authStorage.getAccessToken()
        if (!token) {
            // 没有token，跳转到登录页
            next({ path: '/login' })
            return
        }

        // 检查是否需要权限
        if (to.meta.permission) {
            try {
                // 先从本地存储获取权限，避免频繁调用API
                let userPermissions = JSON.parse(localStorage.getItem('userPermissions') || '[]')

                // 如果本地没有权限数据，才从API获取
                if (!userPermissions || userPermissions.length === 0) {
                    console.log('本地无权限数据，从API获取...')
                    userPermissions = await getUserPermissions()
                }

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
    } else {
        // 不需要认证的页面
        next()
    }
})

export default router
