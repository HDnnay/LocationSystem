<template>
  <div class="settings">
    <h2 class="page-title">系统设置</h2>

    <div class="settings-container">
      <div class="setting-section">
        <h3 class="section-title">基本设置</h3>
        <form @submit.prevent="saveSettings">
          <div class="form-group">
            <label for="siteName">网站名称</label>
            <input type="text" id="siteName" v-model="settings.siteName" required maxlength="100">
          </div>
          <div class="form-group">
            <label for="siteDescription">网站描述</label>
            <textarea id="siteDescription" v-model="settings.siteDescription" rows="3" maxlength="200"></textarea>
          </div>
          <div class="form-group">
            <label for="defaultPage">默认首页</label>
            <select id="defaultPage" v-model="settings.defaultPage">
              <option value="dashboard">仪表盘</option>
              <option value="articles">文章列表</option>
            </select>
          </div>
          <div class="form-group">
            <label for="itemsPerPage">每页显示数量</label>
            <input type="number" id="itemsPerPage" v-model.number="settings.itemsPerPage" min="5" max="50" required>
          </div>
          <button type="submit" class="save-btn">保存设置</button>
        </form>
      </div>
      <div class="setting-section">
        <h3 class="section-title">安全设置</h3>
        <form @submit.prevent="changePassword">
          <div class="form-group">
            <label for="currentPassword">当前密码</label>
            <input type="password" id="currentPassword" v-model="passwordForm.currentPassword" required>
          </div>
          <div class="form-group">
            <label for="newPassword">新密码</label>
            <input type="password" id="newPassword" v-model="passwordForm.newPassword" required minlength="6">
          </div>
          <div class="form-group">
            <label for="confirmPassword">确认新密码</label>
            <input type="password" id="confirmPassword" v-model="passwordForm.confirmPassword" required minlength="6">
          </div>
          <button type="submit" class="save-btn">修改密码</button>
        </form>
      </div>
      <div class="setting-section">
        <h3 class="section-title">系统信息</h3>
        <div class="system-info">
          <p><strong>系统版本：</strong>{{ systemInfo.version }}</p>
          <p><strong>构建时间：</strong>{{ systemInfo.buildTime }}</p>
          <p><strong>技术栈：</strong>{{ systemInfo.techStack }}</p>
          <p><strong>最后登录时间：</strong>{{ systemInfo.lastLogin }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Settings',
  data() {
    return {
      settings: {
        siteName: '内容管理系统',
        siteDescription: '一个基于Vue3的现代化内容管理系统',
        defaultPage: 'dashboard',
        itemsPerPage: 10
      },
      passwordForm: {
        currentPassword: '',
        newPassword: '',
        confirmPassword: ''
      },
      systemInfo: {
        version: '1.0.0',
        buildTime: '2024-01-12',
        techStack: 'Vue 3 + Vue Router + Vite',
        lastLogin: '2024-01-12 09:30:00'
      }
    }
  },
  methods: {
    saveSettings() {
      // 在实际应用中，这里会发送API请求保存设置
      alert('设置保存成功！')
      // 可以将设置保存到localStorage中作为演示
      localStorage.setItem('cmsSettings', JSON.stringify(this.settings))
    },
    changePassword() {
      // 表单验证
      if (this.passwordForm.newPassword !== this.passwordForm.confirmPassword) {
        alert('新密码和确认密码不一致！')
        return
      }
      
      if (this.passwordForm.newPassword.length < 6) {
        alert('新密码长度至少为6位！')
        return
      }
      
      // 在实际应用中，这里会发送API请求修改密码
      alert('密码修改成功！')
      
      // 重置表单
      this.passwordForm = {
        currentPassword: '',
        newPassword: '',
        confirmPassword: ''
      }
    },
    loadSettings() {
      // 从localStorage加载设置（演示用）
      const savedSettings = localStorage.getItem('cmsSettings')
      if (savedSettings) {
        try {
          this.settings = JSON.parse(savedSettings)
        } catch (e) {
          console.error('加载设置失败', e)
        }
      }
    }
  },
  mounted() {
    this.loadSettings()
  }
}
</script>

<style scoped>
.settings {
  padding: 1rem 0;
}

.page-title {
  font-size: 1.5rem;
  margin-bottom: 1.5rem;
  color: var(--text-color);
}

.settings-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(400px, 1fr));
  gap: 2rem;
}

.setting-section {
  background: white;
  border-radius: 8px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.section-title {
  font-size: 1.25rem;
  margin-bottom: 1.5rem;
  color: var(--text-color);
  border-bottom: 2px solid var(--primary-color);
  padding-bottom: 0.5rem;
}

.form-group {
  margin-bottom: 1.5rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: var(--text-color);
}

.form-group input,
.form-group select,
.form-group textarea {
  width: 100%;
  padding: 0.5rem;
  border: 1px solid var(--border-color);
  border-radius: 4px;
  font-size: 1rem;
}

.form-group textarea {
  resize: vertical;
}

.save-btn {
  padding: 0.5rem 1.5rem;
  background-color: var(--primary-color);
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
}

.system-info {
  color: #666;
}

.system-info p {
  margin: 0.75rem 0;
  padding: 0.5rem 0;
  border-bottom: 1px solid #f0f0f0;
}

.system-info p:last-child {
  border-bottom: none;
}

/* 移动端响应式 */
@media (max-width: 768px) {
  .settings-container {
    grid-template-columns: 1fr;
  }
  
  .setting-section {
    padding: 1rem;
  }
}
</style>