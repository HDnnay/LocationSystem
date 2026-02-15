<template>
    <div class="router-view-manager">
        <!-- 前台模板 - 首页和文章详情页 -->
        <template v-if="isFrontendRoute">
            <router-view />
        </template>

        <!-- 登录和注册页面模板 -->
        <template v-else-if="isAuthRoute">
            <transition name="fade" mode="out-in">
                <div class="auth-view-container" :key="$route.path">
                    <router-view />
                </div>
            </transition>
        </template>

        <!-- 后台管理模板 -->
        <template v-else-if="isLoggedIn">
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
                                <span style="margin-left: 5px;">{{ getUserInfo().name || '管理员' }}</span>
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
                          <template v-for="(menuItem, index) in menuItems" :key="index">
                            <!-- 确保menuItem是有效的 -->
                            <template v-if="menuItem">
                              <!-- 子菜单 -->
                              <el-sub-menu v-if="menuItem.children && menuItem.children.length > 0" :index="menuItem.index || `menu-${index}`">
                                <template #title>
                                  <el-icon><component :is="menuItem.icon || 'List'" /></el-icon>
                                  <span>{{ menuItem.title || '未知菜单' }}</span>
                                </template>
                                <!-- 子菜单项 -->
                                <el-menu-item
                                  v-for="(childItem, childIndex) in menuItem.children"
                                  :key="childIndex"
                                  :index="childItem.index || `child-${index}-${childIndex}`"
                                >
                                  <el-icon><component :is="childItem.icon || 'List'" /></el-icon>
                                  <template #title>
                                    {{ childItem.title || '未知菜单' }}
                                  </template>
                                </el-menu-item>
                              </el-sub-menu>
                              <!-- 菜单项 -->
                              <el-menu-item
                                v-else
                                :index="menuItem.index || `menu-${index}`"
                                v-if="hasPermission(menuItem.permission)"
                              >
                                <el-icon><component :is="menuItem.icon || 'List'" /></el-icon>
                                <template #title>
                                  {{ menuItem.title || '未知菜单' }}
                                </template>
                              </el-menu-item>
                            </template>
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

        <!-- 未登录状态模板 -->
        <template v-else>
            <transition name="fade" mode="out-in">
                <div class="auth-view-container" :key="$route.path">
                    <router-view />
                </div>
            </transition>
        </template>
    </div>
</template>

<script>
import { ref, onMounted, onUnmounted } from 'vue'
import { User, UserFilled, OfficeBuilding, Calendar, List, Document, Lock, SetUp, Key, Menu, House } from '@element-plus/icons-vue'
import menuManager from '@/utils/menuManager'
import authStorage from '@/utils/authStorage'
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
                // 初始化 SignalR 连接
                menuManager.initSignalR()
                // 监听菜单更新事件
                window.addEventListener('menu:updated', this.handleMenuUpdated)
            }
        },
        beforeUnmount() {
            // 移除事件监听器
            window.removeEventListener('menu:updated', this.handleMenuUpdated)
            // 断开 SignalR 连接
            menuManager.disconnectSignalR()
        },
        watch: {
            isLoggedIn: {
                handler(newVal) {
                    if (newVal) {
                        this.loadMenus()
                        // 初始化 SignalR 连接
                        menuManager.initSignalR()
                        // 监听菜单更新事件
                        window.addEventListener('menu:updated', this.handleMenuUpdated)
                    } else {
                        // 移除事件监听器
                        window.removeEventListener('menu:updated', this.handleMenuUpdated)
                        // 断开 SignalR 连接
                        menuManager.disconnectSignalR()
                    }
                },
                immediate: true
            }
        },
        methods: {
            // 处理菜单更新事件
            handleMenuUpdated() {
                console.log('收到菜单更新通知，重新加载菜单');
                this.loadMenus();
            },

            // 加载菜单
            async loadMenus() {
                try {
                    // 使用菜单管理器获取菜单数据（直接从API加载）
                    const response = await menuManager.getMenus()

                    // 直接使用后端返回的数据作为菜单数据
                    this.menuItems = this.buildMenuTree(response)

                    // 提取所有权限并保存到localStorage
                    const permissions = []
                    const extractPermissions = (menu) => {
                        // 处理大小写不同的字段名
                        const code = menu.code || menu.Code
                        if (code) {
                            permissions.push(code)
                        }
                        // 处理大小写不同的字段名
                        const children = menu.children || menu.Children
                        if (children) {
                            children.forEach(child => extractPermissions(child))
                        }
                    }
                    // 确保response是一个数组
                    if (Array.isArray(response)) {
                        response.forEach(menu => extractPermissions(menu))
                    } else {
                        console.error('菜单数据不是一个数组，无法提取权限:', response)
                    }
                    localStorage.setItem('userPermissions', JSON.stringify(permissions))
                } catch (error) {
                    console.error('加载菜单失败:', error)
                    // 加载失败时，设置空菜单
                    this.menuItems = []
                    // 加载失败时，设置空权限
                    localStorage.setItem('userPermissions', JSON.stringify([]))
                }
            },

            // 构建菜单树
            buildMenuTree(menuList) {
                // 确保menuList是一个数组
                if (!Array.isArray(menuList)) {
                    console.error('菜单数据不是一个数组:', menuList)
                    return []
                }

                // 简化的菜单处理逻辑，直接使用后端返回的树形结构
                const processedMenus = menuList.map(menu => {
                    // 处理父菜单
                    const parentMenu = {
                        index: menu.menuPath || menu.MenuPath || '',
                        title: menu.name || menu.Name || '',
                        icon: this.getMenuIcon(menu.menuIcon || menu.MenuIcon || ''),
                        permission: menu.code || menu.Code || null,
                        children: []
                    }

                    // 处理子菜单
                    if (menu.children && Array.isArray(menu.children)) {
                        parentMenu.children = menu.children.map(child => ({
                            index: child.menuPath || child.MenuPath || '',
                            title: child.name || child.Name || '',
                            icon: this.getMenuIcon(child.menuIcon || child.MenuIcon || ''),
                            permission: child.code || child.Code || null,
                            children: []
                        }))
                    } else if (menu.Children && Array.isArray(menu.Children)) {
                        parentMenu.children = menu.Children.map(child => ({
                            index: child.menuPath || child.MenuPath || '',
                            title: child.name || child.Name || '',
                            icon: this.getMenuIcon(child.menuIcon || child.MenuIcon || ''),
                            permission: child.code || child.Code || null,
                            children: []
                        }))
                    }

                    return parentMenu
                })

                return processedMenus
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
                    'House': 'House'
                }
                return iconMap[iconName] || 'List'
            },



            // 检查是否有权限
            hasPermission(permission) {
                // 如果permission为null或undefined，直接返回true（暂时跳过权限检查）
                if (!permission) {
                    return true
                }
                // 暂时跳过权限检查，确保所有菜单都能显示
                return true
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
            },

            // 获取用户信息
            getUserInfo() {
                try {
                    return authStorage.getUserInfo() || {}
                } catch (error) {
                    console.error('获取用户信息失败:', error)
                    return {}
                }
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
