<template>
  <div class="permission-denied">
    <div class="container">
      <h1>403</h1>
      <h2>权限不足</h2>
      <p>您没有权限访问此页面</p>
      <router-link to="/" class="back-home">返回首页</router-link>
    </div>
  </div>
</template>

<script>
export default {
  name: '403Page'
}
</script>

<style scoped>
.permission-denied {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #f5f7fa;
}

.container {
  text-align: center;
  padding: 40px;
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
}

h1 {
  font-size: 120px;
  font-weight: bold;
  color: #409eff;
  margin: 0;
}

h2 {
  font-size: 36px;
  color: #303133;
  margin: 20px 0;
}

p {
  font-size: 18px;
  color: #606266;
  margin: 20px 0 40px;
}

.back-home {
  display: inline-block;
  padding: 10px 20px;
  background-color: #409eff;
  color: white;
  text-decoration: none;
  border-radius: 4px;
  transition: background-color 0.3s;
}

.back-home:hover {
  background-color: #66b1ff;
}
</style>