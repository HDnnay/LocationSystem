import { createRouter, createWebHistory } from 'vue-router'
import { getUserMenus, getUserPermissions as apiGetUserPermissions } from '../api/permissions'

// 懒加载页面组件
const routes = [
    {
        path: '/',
        redirect: '/dashboard',
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
        path: '/dashboard',
        name: 'Dashboard',
        component: () => import('../views/DashboardView.vue'),
        meta: {
            title: '仪表盘',
            requiresAuth: true
        }
    },
  {
    path: '/role-permissions',
    name: 'RolePermissions',
    component: () => import('../views/SystemView.vue'),
    meta: {
      title: '角色权限管理',
      requiresAuth: true
    }
  },
  {
    path: '/company',
    name: 'company',
    component: () => import('../views/company/CompanyView.vue'),
    meta: {
      title: '公司管理',
      requiresAuth: true
    }
  },
  {
    path: '/company/list',
    name: 'company_list',
    component: () => import('../views/company/CompanyView.vue'),
    meta: {
      title: '公司列表',
      requiresAuth: true
    }
  },
  {
    path: '/company/statistics',
    name: 'company_statistics',
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
    path: '/rent/list',
    name: 'rent_list',
    component: () => import('../views/renthouse/RentHouseView.vue'),
    meta: {
      title: '租房列表',
      requiresAuth: true
    }
  },
  {
    path: '/rent/create',
    name: 'create-rent',
    component: () => import('../views/renthouse/CreateRentHouseView.vue'),
    meta: {
      title: '租房创建',
      requiresAuth: true
    }
  },
  {
    path: '/roles',
    name: 'Roles',
    component: () => import('../views/RolesView.vue'),
    meta: {
      title: '角色管理',
      requiresAuth: true,
      permission: 'role:view'
    }
  },
  {
    path: '/permissions',
    name: 'Permissions',
    component: () => import('../views/users/PermissionsView.vue'),
    meta: {
      title: '权限管理',
      requiresAuth: true,
      permission: 'permission:view'
    }
  },
  {
    path: '/users',
    name: 'Users',
    component: () => import('../views/users/UsersView.vue'),
    meta: {
      title: '用户管理',
      requiresAuth: true
    }
  },
  {
    path: '/menus',
    name: 'Menus',
    component: () => import('../views/users/MenusView.vue'),
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

// 路由前置守卫
router.beforeEach(async (to, from, next) => {
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
    } else {
        // 不需要认证的页面
        next()
    }
})

// 获取用户权限
async function getUserPermissions() {
    try {
        // 尝试从localStorage获取权限
        const cachedPermissions = localStorage.getItem('userPermissions')

        // 检查cachedPermissions是否存在且不是空数组的字符串表示
        if (cachedPermissions && cachedPermissions !== "[]") {
            try {
                const parsedPermissions = JSON.parse(cachedPermissions)
                // 检查解析后的权限是否非空
                if (parsedPermissions && parsedPermissions.length > 0) {
                    return parsedPermissions
                }
            } catch (parseError) {
                console.error('解析权限数据失败:', parseError)
                // 解析失败，继续从后端获取
            }
        }

        // 从后端获取权限
        const permissions = await apiGetUserPermissions()

        // 存储到localStorage
        localStorage.setItem('userPermissions', JSON.stringify(permissions))

        return permissions
    } catch (error) {
        console.error('获取用户权限失败:', error)
        // 错误时返回空数组，不使用默认权限
        localStorage.setItem('userPermissions', JSON.stringify([]))
        return []
    }
}

export default router
