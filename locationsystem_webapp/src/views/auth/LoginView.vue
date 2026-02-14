<template>
  <div class="login-container">
    <div class="login-form">
      <h2 class="login-title">用户登录</h2>
      <el-form :model="loginForm" :rules="rules" ref="loginFormRef" label-width="80px">
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="loginForm.email" placeholder="请输入邮箱" type="email" />
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input v-model="loginForm.password" placeholder="请输入密码" type="password" show-password />
        </el-form-item>
        <el-form-item label="用户类型" prop="type">
          <el-radio-group v-model="loginForm.type">
            <el-radio v-for="type in userTypes" :key="type.value" :value="type.value">{{ type.name }}</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleLogin" :loading="loading" style="width: 100%">
            登录
          </el-button>
        </el-form-item>
        <el-form-item>
          <div style="text-align: center; margin-top: 10px;">
            没有账号？<el-link type="primary" @click="goToRegister">立即注册</el-link>
          </div>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script>
import { getUserTypes } from '../../api/users'

export default {
  name: 'LoginView',
  data() {
    return {
      loginForm: {
        email: '',
        password: '',
        type: 0 // 默认用户
      },
      rules: {
        email: [
          { required: true, message: '请输入邮箱', trigger: 'blur' },
          { type: 'email', message: '请输入正确的邮箱格式', trigger: 'blur' }
        ],
        password: [
          { required: true, message: '请输入密码', trigger: 'blur' },
          { min: 6, message: '密码长度至少为6位', trigger: 'blur' }
        ],
        type: [
          { required: true, message: '请选择用户类型', trigger: 'change' }
        ]
      },
      loading: false,
      userTypes: []
    }
  },
  mounted() {
    // 强制清空表单值，确保没有默认值
    this.loginForm.email = '';
    this.loginForm.password = '';
    // 获取用户类型
    this.fetchUserTypes();
  },
  methods: {
            async fetchUserTypes() {
                try {
                    const response = await getUserTypes();
                    this.userTypes = response.data;
                } catch (error) {
                    console.error('获取用户类型失败:', error);
                    // 如果获取失败，使用默认值
                    this.userTypes = [
                        { value: 0, name: '默认用户' },
                        { value: 1, name: '管理员' },
                        { value: 2, name: '普通用户' }
                    ];
                }
            },
            async handleLogin() {
                const valid = await this.$refs.loginFormRef.validate()
                if (!valid) return

                this.loading = true
                try {
                    const data = await this.$api.auth.login(this.loginForm)
                    console.log('登录响应数据:', data)

                    // 保存token和用户信息
                    localStorage.setItem('access_token', data.accessToken)
                    localStorage.setItem('refresh_token', data.refreshToken)
                    localStorage.setItem('user_type', this.loginForm.type)
                    localStorage.setItem('user_info', JSON.stringify(data.userInfo))

                    // 更新App.vue中的登录状态
                    this.$root.$data.isLoggedIn = true

                    // 跳转到首页
                    this.$router.push('/')
                } catch (error) {
                    this.$message.error('登录失败：' + (error.response?.data?.message || error.message))
                } finally {
                    this.loading = false
                }
            },
            goToRegister() {
                this.$router.push('/register')
            }
        }
}
</script>

<style scoped>
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background-color: #f5f7fa;
}

.login-form {
  width: 400px;
  padding: 30px;
  background-color: white;
  border-radius: 8px;
  box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
}

.login-title {
  text-align: center;
  margin-bottom: 20px;
  color: #303133;
}
</style>
