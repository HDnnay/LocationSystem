<template>
  <div class="login-container">
    <div class="login-form">
      <h2 class="login-title">用户注册</h2>
      <el-form :model="registerForm" :rules="rules" ref="registerFormRef" label-width="80px">
        <el-form-item label="姓名" prop="name">
          <el-input v-model="registerForm.name" placeholder="请输入姓名" />
        </el-form-item>
        <el-form-item label="邮箱" prop="email">
          <el-input v-model="registerForm.email" placeholder="请输入邮箱" type="email" />
        </el-form-item>
        <el-form-item label="密码" prop="password">
          <el-input v-model="registerForm.password" placeholder="请输入密码" type="password" show-password />
        </el-form-item>
        <el-form-item label="确认密码" prop="confirmPassword">
          <el-input v-model="registerForm.confirmPassword" placeholder="请确认密码" type="password" show-password />
        </el-form-item>
        <el-form-item label="用户类型" prop="type">
          <el-radio-group v-model="registerForm.type">
            <el-radio :value="0">牙医</el-radio>
            <el-radio :value="1">患者</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="handleRegister" :loading="loading" style="width: 100%">
            注册
          </el-button>
        </el-form-item>
        <el-form-item>
          <div style="text-align: center; margin-top: 10px;">
            已有账号？<el-link type="primary" @click="goToLogin">立即登录</el-link>
          </div>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script>
export default {
  name: 'RegisterView',
  data() {
    return {
      registerForm: {
        name: '',
        email: '',
        password: '',
        confirmPassword: '',
        type: 0 // 默认牙医
      },
      rules: {
        name: [
          { required: true, message: '请输入姓名', trigger: 'blur' },
          { min: 2, message: '姓名长度至少为2位', trigger: 'blur' }
        ],
        email: [
          { required: true, message: '请输入邮箱', trigger: 'blur' },
          { type: 'email', message: '请输入正确的邮箱格式', trigger: 'blur' }
        ],
        password: [
          { required: true, message: '请输入密码', trigger: 'blur' },
          { min: 6, message: '密码长度至少为6位', trigger: 'blur' }
        ],
        confirmPassword: [
          { required: true, message: '请确认密码', trigger: 'blur' },
          {
            validator: (rule, value, callback) => {
              if (value !== this.registerForm.password) {
                callback(new Error('两次输入的密码不一致'));
              } else {
                callback();
              }
            },
            trigger: 'blur'
          }
        ],
        type: [
          { required: true, message: '请选择用户类型', trigger: 'change' }
        ]
      },
      loading: false
    }
  },
  methods: {
    async handleRegister() {
      const valid = await this.$refs.registerFormRef.validate()
      if (!valid) return

      this.loading = true
      try {
        await this.$api.auth.register(this.registerForm)

        // 注册成功后跳转到登录页面
        this.$message.success('注册成功，请登录')
        this.$router.push('/login')
      } catch (error) {
        this.$message.error('注册失败：' + (error.response?.data?.message || error.message))
      } finally {
        this.loading = false
      }
    },
    goToLogin() {
      this.$router.push('/login')
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
