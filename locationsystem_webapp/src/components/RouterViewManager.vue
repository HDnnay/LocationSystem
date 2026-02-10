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
                        <el-menu-item index="/dentists">
                          <el-icon><User /></el-icon>
                          <template #title>
                            牙医管理
                          </template>
                        </el-menu-item>
                        <el-menu-item index="/patients">
                          <el-icon><UserFilled /></el-icon>
                          <template #title>
                            患者管理
                          </template>
                        </el-menu-item>
                        <el-menu-item index="/dental-offices">
                          <el-icon><OfficeBuilding /></el-icon>
                          <template #title>
                            牙科诊所管理
                          </template>
                        </el-menu-item>
                        <el-menu-item index="/appointments">
                          <el-icon><Calendar /></el-icon>
                          <template #title>
                            预约管理
                          </template>
                        </el-menu-item>
                        <el-sub-menu index="/roles">
                          <template #title>
                            <el-icon><Lock /></el-icon>
                            <span>角色权限管理</span>
                          </template>
                          <el-menu-item index="/roles">
                            <el-icon><SetUp /></el-icon>
                            <template #title>
                              角色管理
                            </template>
                          </el-menu-item>
                          <el-menu-item index="/permissions">
                            <el-icon><Key /></el-icon>
                            <template #title>
                              权限管理
                            </template>
                          </el-menu-item>
                        </el-sub-menu>
                        <el-sub-menu index="/company">
                          <template #title>
                            <el-icon><Calendar /></el-icon>
                            <span>公司管理</span>
                          </template>
                          <el-menu-item index="/company">
                            <el-icon><List /></el-icon>
                            <template #title>
                              公司列表
                            </template>
                          </el-menu-item>
                          <el-menu-item index="/company/provice">
                            <el-icon><List /></el-icon>
                            <template #title>
                              统计管理
                            </template>
                          </el-menu-item>
                        </el-sub-menu>
                        <el-sub-menu index="/rent">
                          <template #title>
                            <el-icon><Calendar /></el-icon>
                            <span>租房管理</span>
                          </template>

                          <el-menu-item index="/rent">
                            <el-icon><List /></el-icon>
                            <template #title>
                              租房列表
                            </template>
                          </el-menu-item>

                          <el-menu-item index="/rent/create">
                            <el-icon><Document /></el-icon>
                            <template #title>
                              租房创建
                            </template>
                          </el-menu-item>
                          <!-- 可以继续添加更多子菜单 -->
                        </el-sub-menu>
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
import { User, UserFilled, OfficeBuilding, Calendar, List, Document, Lock, SetUp, Key } from '@element-plus/icons-vue'
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
            Key
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
        methods: {
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
