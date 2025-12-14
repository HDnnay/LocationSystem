<script setup>
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import {
  Menu as MenuIcon,
  UserFilled,
  User,
  Calendar,
  Setting,
  Message,
  Bell,
  Search,
  ArrowDown,
  MapLocation
} from '@element-plus/icons-vue'

const router = useRouter()
const route = useRoute()
const activeMenu = ref('dentists')
const collapsed = ref(false)

// 处理菜单点击
const handleMenuClick = (index) => {
  activeMenu.value = index
  router.push(`/${index}`)
}

// 计算面包屑导航数据
const breadcrumbItems = computed(() => {
  const items = [{ path: '/', title: '首页' }]
  if (route.path !== '/') {
    const matchedRoute = router.resolve(route.path).matched[0]
    if (matchedRoute && matchedRoute.meta.title) {
      items.push({ path: route.path, title: matchedRoute.meta.title })
    }
  }
  return items
})

// 处理搜索
const searchText = ref('')
const handleSearch = () => {
  console.log('搜索:', searchText.value)
}

// 处理用户菜单点击
const handleUserMenuClick = (command) => {
  if (command === 'logout') {
    console.log('退出登录')
  }
}
</script>

<template>
  <el-container style="height: 100vh; background-color: #f0f2f5;">
    <!-- 顶部导航栏 -->
    <el-header class="header-container" style="background-color: #fff; border-bottom: 1px solid #e6e6e6; display: flex; align-items: center; justify-content: space-between; padding: 0 20px;">
      <div class="header-left" style="display: flex; align-items: center;">
        <el-button type="text" @click="collapsed = !collapsed" style="margin-right: 10px;">
          <el-icon><MenuIcon /></el-icon>
        </el-button>
        <div class="logo" style="display: flex; align-items: center;">
          <img alt="Logo" src="./assets/logo.svg" width="40" height="40" style="margin-right: 10px;" />
          <h1 style="font-size: 20px; margin: 0; color: #1890ff;">位置管理系统</h1>
        </div>
      </div>

      <div class="header-right" style="display: flex; align-items: center;">
        <!-- 搜索框 -->
        <div class="search-box" style="margin-right: 20px;">
          <el-input
            v-model="searchText"
            placeholder="搜索菜单或内容"
            :prefix-icon="Search"
            style="width: 300px;"
            @keyup.enter="handleSearch"
          />
        </div>

        <!-- 通知图标 -->
        <div style="margin-right: 20px;">
          <el-badge :value="3" class="item">
            <el-button type="text">
              <el-icon><Bell /></el-icon>
            </el-button>
          </el-badge>
        </div>

        <!-- 邮件图标 -->
        <div style="margin-right: 20px;">
          <el-badge :value="5" class="item">
            <el-button type="text">
              <el-icon><Message /></el-icon>
            </el-button>
          </el-badge>
        </div>

        <!-- 用户信息 -->
        <el-dropdown @command="handleUserMenuClick">
          <div style="display: flex; align-items: center; cursor: pointer;">
            <el-avatar size="medium" :src="'https://cube.elemecdn.com/3/7c/3ea6beec64369c2642b92c6726f1epng.png'" style="margin-right: 5px;"></el-avatar>
            <span style="margin-right: 5px;">admin</span>
            <el-icon class="el-icon--right"><ArrowDown /></el-icon>
          </div>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item command="profile">个人中心</el-dropdown-item>
              <el-dropdown-item command="settings">设置</el-dropdown-item>
              <el-dropdown-divider></el-dropdown-divider>
              <el-dropdown-item command="logout">退出登录</el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </div>
    </el-header>

    <el-container>
      <!-- 左侧侧边栏 -->
      <el-aside :width="collapsed ? '64px' : '200px'" style="background-color: #001529; overflow: hidden; transition: width 0.3s;" class="sidebar-container">
        <el-menu
          default-active="dentists"
          class="el-menu-vertical-demo"
          @select="handleMenuClick"
          :router="false"
          background-color="#001529"
          text-color="#fff"
          active-text-color="#409eff"
          :collapse="collapsed"
        >
          <!-- 基础管理 -->
          <el-sub-menu index="1">
            <template #title>
              <el-icon><MenuIcon /></el-icon>
              <span>基础管理</span>
            </template>
            <el-menu-item index="dentists">
              <el-icon><UserFilled /></el-icon>
              <span>牙医管理</span>
            </el-menu-item>
            <el-menu-item index="patients">
              <el-icon><User /></el-icon>
              <span>患者管理</span>
            </el-menu-item>
            <el-menu-item index="appointments">
              <el-icon><Calendar /></el-icon>
              <span>预约管理</span>
            </el-menu-item>
            <el-menu-item index="dental-offices">
              <el-icon><MapLocation /></el-icon>
              <span>牙科诊所管理</span>
            </el-menu-item>
          </el-sub-menu>

          <!-- 系统设置 -->
          <el-sub-menu index="2">
            <template #title>
              <el-icon><Setting /></el-icon>
              <span>系统设置</span>
            </template>
            <el-menu-item index="settings">
              <el-icon><Setting /></el-icon>
              <span>系统配置</span>
            </el-menu-item>
          </el-sub-menu>
        </el-menu>
      </el-aside>

      <!-- 主内容区域 -->
      <el-main style="padding: 20px;">
        <!-- 面包屑导航 -->
        <el-breadcrumb separator="/" style="margin-bottom: 20px;">
          <el-breadcrumb-item :to="item.path" v-for="(item, index) in breadcrumbItems" :key="index">
            {{ item.title }}
          </el-breadcrumb-item>
        </el-breadcrumb>

        <!-- 页面内容 -->
        <router-view />
      </el-main>
    </el-container>
  </el-container>
</template>

<style scoped>
.el-menu {
  border-right: none;
}

.header-container {
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.sidebar-container {
  box-shadow: 2px 0 8px rgba(0, 0, 0, 0.1);
}
</style>
