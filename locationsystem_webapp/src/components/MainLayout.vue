<template>
  <div class="layout-container">
    <!-- 顶部导航栏 -->
    <el-header class="layout-header">
      <div class="header-title">
        内容管理系统 - 后台管理
      </div>
      <div class="header-right">
        <el-dropdown>
          <span class="header-dropdown">
            <el-icon><User /></el-icon>
            管理员
            <el-icon class="el-icon--right"><ArrowDown /></el-icon>
          </span>
          <template #dropdown>
            <el-dropdown-menu>
              <el-dropdown-item>个人中心</el-dropdown-item>
              <el-dropdown-item>退出登录</el-dropdown-item>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </div>
    </el-header>

    <div class="layout-body">
      <!-- 左侧导航栏 -->
      <el-aside width="200px" class="layout-aside">
        <el-menu
          :default-active="activeMenu"
          class="layout-menu"
          router
          default-openeds="['menu-1']"
        >
          <el-sub-menu index="menu-1">
            <template #title>
              <el-icon><Document /></el-icon>
              <span>数据管理</span>
            </template>
            <el-menu-item index="/dentists">
              <el-icon><UserFilled /></el-icon>
              <span>牙医管理</span>
            </el-menu-item>
            <el-menu-item index="/patients">
              <el-icon><User /></el-icon>
              <span>患者管理</span>
            </el-menu-item>
            <el-menu-item index="/appointments">
              <el-icon><Calendar /></el-icon>
              <span>预约管理</span>
            </el-menu-item>
            <el-menu-item index="/dental-offices">
              <el-icon><OfficeBuilding /></el-icon>
              <span>牙科诊所管理</span>
            </el-menu-item>
          </el-sub-menu>
        </el-menu>
      </el-aside>

      <!-- 右侧内容区域 -->
      <el-main class="layout-main">
        <router-view />
      </el-main>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import { UserFilled, User, Calendar, OfficeBuilding, Document, ArrowDown } from '@element-plus/icons-vue'

const route = useRoute()
const activeMenu = computed(() => route.path)
</script>

<style scoped>
.layout-container {
  display: flex;
  flex-direction: column;
  height: 100vh;
}

.layout-header {
  background-color: #1890ff;
  color: white;
  height: 60px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0 20px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.header-title {
  font-size: 18px;
  font-weight: bold;
}

.header-right {
  display: flex;
  align-items: center;
}

.header-dropdown {
  display: flex;
  align-items: center;
  cursor: pointer;
  padding: 4px 12px;
  border-radius: 4px;
  transition: background-color 0.3s;
}

.header-dropdown:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

.layout-body {
  display: flex;
  flex: 1;
  overflow: hidden;
}

.layout-aside {
  background-color: #001529;
  overflow-y: auto;
}

.layout-menu {
  height: 100%;
  border-right: none;
}

.layout-main {
  background-color: #f0f2f5;
  padding: 20px;
  overflow-y: auto;
}
</style>
