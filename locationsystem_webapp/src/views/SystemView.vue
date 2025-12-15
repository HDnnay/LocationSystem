<template>
  <div class="system-view">
    <el-card shadow="hover">
      <template #header>
        <div class="card-header">
          <span>系统设置</span>
        </div>
      </template>
      <div class="system-content">
        <p>系统设置页面内容</p>
      </div>
    </el-card>
  </div>
</template>

<script setup>
</script>

<style scoped>
.system-view {
  padding: 0;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.system-content {
  padding: 20px;
}
</style>