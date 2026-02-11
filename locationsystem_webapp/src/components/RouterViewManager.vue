<template>
    <div class="router-view-manager">
        <!-- 前台模板 - 首页和文章详情页 -->
        <template v-if="isFrontendRoute">
            <router-view />
        </template>

        <!-- 登录和注册页面模板 -->
        <template v-else-if="isAuthRoute || !isLoggedIn">
            <transition name="fade" mode="out-in">
                <div class="auth-view-container" :key="$route.path">
                    <router-view />
                </div>
            </transition>
        </template>

        <!-- 后台管理模板 -->
        <template v-else>
            <el-container style="min-height: 100vh; background-color: #f5f7fa;">
                <!-- 后台导航栏 -->
                <el-header height="60px" style="background-color: #2c3e50; color: white; padding: 0 20px; display: flex; align-items: center; justify-content: space-between; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);">
                    <div class="logo">
                        <h1 style="margin: 0; font-size: 1.5rem; font-weight: 500;">内容管理系统 - 后台管理</h1>
                    </div>
                    <div class="header-actions" style="display: flex; align-items: center;">
                        <el-button link
                                   icon="Menu"
                                   @click="toggleSidebar"
                                   style="color: white; margin-right: 10px;">
                            {{ sidebarOpen ? '收起菜单' : '展开菜单' }}
                        </el-button>
                        <el-dropdown>
                            <span class="el-dropdown-link" style="color: white; cursor: pointer; display: flex; align-items: center;">
                                <el-icon><User /></el-icon>
                                <span style="margin-left: 5px;">管理员</span>
                                <el-icon class="el-icon--right"><ArrowDown /></el-icon>
                            </span>
                            <template #dropdown>
                                <el-dropdown-menu>
                                    <el-dropdown-item divided @click="logout">
                                        <el-icon><TurnOff /></el-icon>
                                        退出登录
                                    </el-dropdown-item>
                                </el-dropdown-menu>
                            </template>
                        </el-dropdown>
                    </div>
                </el-header>

                <el-container>
                    <!-- 后台侧边栏 -->
                    <el-aside :width="sidebarOpen ? '240px' : '64px'"
                              style="background-color: #2c3e50; color: white; transition: width 0.3s ease;">

                      <el-menu :default-active="$route.path"
                               class="el-menu-vertical-demo"
                               :collapse="!sidebarOpen"
                               background-color="#2c3e50"
                               text-color="#ecf0f1"
                               active-text-color="#409EFF"
                               router>
                          <!-- 动态生成菜单 -->
                          <template v-for="menuItem in menuItems" :key="menuItem.index">
                            <!-- 子菜单 -->
                            <el-sub-menu v-if="menuItem.children && menuItem.children.length > 0" :index="menuItem.index">
                              <template #title>
                                <el-icon><component :is="menuItem.icon" /></el-icon>
                                <span>{{ menuItem.title }}</span>
                              </template>
                              <!-- 子菜单项 -->
                              <el-menu-item
                                v-for="childItem in menuItem.children"
                                :key="childItem.index"
                                :index="childItem.index"
                                v-if="hasPermission(childItem.permission)"
                              >
                                <el-icon><component :is="childItem.icon" /></el-icon>
                                <template #title>
                                  {{ childItem.title }}
                                </template>
                              </el-menu-item>
                            </el-sub-menu>
                            <!-- 菜单项 -->
                            <el-menu-item
                              v-else
                              :index="menuItem.index"
                              v-if="hasPermission(menuItem.permission)"
                            >
                              <el-icon><component :is="menuItem.icon" /></el-icon>
                              <template #title>
                                {{ menuItem.title }}
                              </template>
                            </el-menu-item>
                          </template>
                        </el-menu>

                    </el-aside>

                    <!-- 后台内容区域 -->
                    <el-main :style="{
                        padding: sidebarOpen ? '20px' : '20px 20px 20px 10px',
                        transition: 'all 0.3s ease',
                        minWidth: 0
                    }">
                        <transition name="fade" mode="out-in">
                            <div class="content-view-container"
                                 :key="$route.path"
                                 style="width: 100%; minWidth: 0;">
                                <router-view />
                            </div>
                        </transition>
                    </el-main>
                </el-container>
            </el-container>
        </template>
    </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import { User, UserFilled, OfficeBuilding, Calendar, List, Document, Lock, SetUp, Key, Menu, Home, Company, House } from '@element-plus/icons-vue'
    export default {
        name: 'RouterViewManager',
        components: {
            User,
            UserFilled,
            OfficeBuilding,
            Calendar,
            List,
            Document,
            Lock,
            SetUp,
            Key,
            Menu,
            Home,
            Company,
            House
        },
        props: {
            sidebarOpen: {
                type: Boolean,
                default: false
            },
            isMobile: {
                type: Boolean,
                default: false
            },
            isLoggedIn: {
                type: Boolean,
                default: true
            },
            openDropdown: {
                type: String,
                default: null
            }
        },
        data() {
            return {
                menuItems: []
            }
        },
        computed: {
            // 判断当前路由是否为前台路由
            isFrontendRoute() {
                return false // 当前没有前台路由
            },

            // 判断当前路由是否为认证相关路由
            isAuthRoute() {
                const authRoutes = ['/login', '/register']
                return authRoutes.includes(this.$route.path)
            }
        },
        mounted() {
            if (this.isLoggedIn) {
                this.loadMenus()
            }
        },
        methods: {
            // 加载菜单
            async loadMenus() {
                try {
                    const response = await this.$api.permissions.getUserMenus()
                    if (response.status === 200) {
                        this.menuItems = this.buildMenuTree(response.data)
                    }
                } catch (error) {
                    console.error('加载菜单失败:', error)
                    // 使用默认菜单
                    this.menuItems = this.getDefaultMenus()
                }
            },

            // 构建菜单树
            buildMenuTree(menuList) {
                const menuMap = new Map()
                const rootMenus = []

                // 构建菜单映射
                menuList.forEach(menu => {
                    menuMap.set(menu.id, {
                        index: menu.menuPath,
                        title: menu.name,
                        icon: this.getMenuIcon(menu.menuIcon),
                        permission: menu.code,
                        children: []
                    })
                })

                // 构建菜单树
                menuList.forEach(menu => {
                    const menuItem = menuMap.get(menu.id)
                    if (menu.parentId === null) {
                        rootMenus.push(menuItem)
                    } else if (menuMap.has(menu.parentId)) {
                        menuMap.get(menu.parentId).children.push(menuItem)
                    }
                })

                return rootMenus
            },

            // 获取菜单图标
            getMenuIcon(iconName) {
                const iconMap = {
                    'User': 'User',
                    'UserFilled': 'UserFilled',
                    'OfficeBuilding': 'OfficeBuilding',
                    'Calendar': 'Calendar',
                    'List': 'List',
                    'Document': 'Document',
                    'Lock': 'Lock',
                    'SetUp': 'SetUp',
                    'Key': 'Key',
                    'Menu': 'Menu',
                    'Home': 'Home',
                    'Company': 'Company',
                    'House': 'House'
                }
                return iconMap[iconName] || 'Menu'
            },

            // 获取默认菜单
            getDefaultMenus() {
                return [
                    {
                        index: '/dentists',
                        title: '牙医管理',
                        icon: 'UserFilled',
                        permission: 'dentist:view',
                        children: []
                    },
                    {
                        index: '/patients',
                        title: '患者管理',
                        icon: 'User',
                        permission: 'patient:view',
                        children: []
                    },
                    {
                        index: '/dental-offices',
                        title: '牙科诊所管理',
                        icon: 'OfficeBuilding',
                        permission: 'dental-office:view',
                        children: []
                    },
                    {
                        index: '/appointments',
                        title: '预约管理',
                        icon: 'Calendar',
                        permission: 'appointment:view',
                        children: []
                    },
                    {
                        index: '/roles',
                        title: '角色权限管理',
                        icon: 'Lock',
                        permission: 'role:view',
                        children: [
                            {
                                index: '/roles',
                                title: '角色管理',
                                icon: 'SetUp',
                                permission: 'role:view',
                                children: []
                            },
                            {
                                index: '/permissions',
                                title: '权限管理',
                                icon: 'Key',
                                permission: 'permission:view',
                                children: []
                            }
                        ]
                    },
                    {
                        index: '/company',
                        title: '公司管理',
                        icon: 'Company',
                        permission: 'company:view',
                        children: [
                            {
                                index: '/company',
                                title: '公司列表',
                                icon: 'List',
                                permission: 'company:view',
                                children: []
                            },
                            {
                                index: '/company/provice',
                                title: '统计管理',
                                icon: 'List',
                                permission: 'company:statistics:view',
                                children: []
                            }
                        ]
                    },
                    {
                        index: '/rent',
                        title: '租房管理',
                        icon: 'House',
                        permission: 'rent:view',
                        children: [
                            {
                                index: '/rent',
                                title: '租房列表',
                                icon: 'List',
                                permission: 'rent:view',
                                children: []
                            },
                            {
                                index: '/rent/create',
                                title: '租房创建',
                                icon: 'Document',
                                permission: 'rent:create',
                                children: []
                            }
                        ]
                    }
                ]
            },

            // 检查是否有权限
            hasPermission(permission) {
                if (!permission) {
                    return true
                }
                const userPermissions = JSON.parse(localStorage.getItem('userPermissions') || '[]')
                return userPermissions.includes(permission)
            },

            // 切换侧边栏
            toggleSidebar() {
                this.$emit('update:sidebarOpen', !this.sidebarOpen)
            },

            // 关闭侧边栏
            closeSidebar() {
                this.$emit('update:sidebarOpen', false)
            },

            // 切换下拉菜单
            toggleDropdown(dropdownName) {
                this.$emit('update:openDropdown', this.openDropdown === dropdownName ? null : dropdownName)
            },

            // 退出登录
            logout() {
                this.$emit('logout')
            }
        }
    }
</script>

<style scoped>
    .router-view-manager {
        display: flex;
        flex-direction: column;
        min-height: 100vh;
    }
</style>
