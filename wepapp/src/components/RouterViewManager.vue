<template>
    <div class="router-view-manager">
        <!-- 前台模板 - 首页 -->
        <template v-if="isFrontendRoute">
            <router-view />
        </template>

        <!-- 后台管理模板 -->
        <template v-else>
            <el-container style="min-height: 100vh; background-color: #f5f7fa;">
                <!-- 后台导航栏 -->
                <el-header height="60px" style="background-color: #2c3e50; color: white; padding: 0 20px; display: flex; align-items: center; justify-content: space-between; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);">
                    <div class="logo">
                        <h1 style="margin: 0; font-size: 1.5rem; font-weight: 500;">内容管理系统 - 后台管理</h1>
                    </div>
                    <div class="header-actions">
                        <el-button type="text"
                                   icon="el-icon-Menu"
                                   @click="toggleSidebar"
                                   v-if="isMobile"
                                   style="color: white; margin-right: 10px;">
                        </el-button>
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
                            <el-menu-item index="/dashboard">
                                <el-icon><Monitor /></el-icon>
                                <template #title>
                                    仪表盘
                                </template>
                            </el-menu-item>
                            <el-menu-item index="/settings">
                                <el-icon><Setting /></el-icon>
                                <template #title>
                                    系统设置
                                </template>
                            </el-menu-item>
                            <el-menu-item index="/">
                                <el-icon><House /></el-icon>
                                <template #title>
                                    返回前台
                                </template>
                            </el-menu-item>
                        </el-menu>
                    </el-aside>

                    <!-- 后台内容区域 -->
                    <el-main style="padding: 20px;">
                        <transition name="fade" mode="out-in">
                            <div class="content-view-container">
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
    export default {
        name: 'RouterViewManager',
        components: {
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
            openDropdown: {
                type: String,
                default: null
            }
        },
        computed: {
            // 判断当前路由是否为前台路由
            isFrontendRoute() {
                return this.$route.path === '/'
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