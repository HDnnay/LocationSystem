<template>
  <div class="create-rent-house-container">
    <el-card shadow="hover" class="create-rent-house-card">
      <template #header>
        <div class="card-header">
          <span>创建租房信息</span>
        </div>
      </template>

      <el-form :model="form" :rules="rules" ref="form" label-position="top">
        <!-- 第一行：标题 -->
        <el-row :gutter="20">
          <el-col :span="24">
            <el-form-item label="标题" prop="title">
              <el-input v-model="form.title" placeholder="请输入标题" />
            </el-form-item>
          </el-col>
        </el-row>

        <!-- 第二行：描述（富文本编辑器） -->
        <el-row :gutter="20">
          <el-col :span="24">
            <el-form-item label="描述" prop="description">
              <div style="border: 1px solid #e8e8e8;">
                <Toolbar
                  :editor="editorRef"
                  :defaultConfig="toolbarConfig"
                  :mode="'default'"
                />
                <Editor
                  v-model="form.description"
                  :defaultConfig="editorConfig"
                  :mode="'default'"
                  @on-created="editorCreated"
                  @on-change="editorChange"
                  style="height: 300px; overflow-y: hidden;"
                />
              </div>
            </el-form-item>
          </el-col>
        </el-row>

        <!-- 第二行：4个等宽输入框 -->
        <el-row :gutter="20">
          <el-col :span="6">
            <el-form-item label="地址" prop="address">
              <el-input v-model="form.address" placeholder="请输入地址" />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="类型" prop="type">
              <el-select v-model="form.type" placeholder="请选择类型">
                <el-option label="整租" value="1" />
                <el-option label="合租" value="2" />
                <el-option label="公寓" value="3" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="月租(元)" prop="monthlyRent">
              <el-input v-model.number="form.monthlyRent" type="number" placeholder="请输入月租" />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="押金(元)" prop="deposit">
              <el-input v-model.number="form.deposit" type="number" placeholder="请输入押金" />
            </el-form-item>
          </el-col>
        </el-row>

        <!-- 第三行：2个等宽输入框 -->
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item label="电话" prop="phone">
              <el-input v-model="form.phone" placeholder="请输入电话" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item label="图片路径" prop="imageSrc">
              <el-input v-model="form.imageSrc" placeholder="上传图片后自动填充" readonly />
            </el-form-item>
          </el-col>
        </el-row>

        <!-- 第四行：图片上传（单独一行） -->
        <el-row :gutter="20">
          <el-col :span="24">
            <el-form-item label="图片">
              <el-upload
                class="upload-demo"
                :show-file-list="true"
                :on-change="handleFileChange"
                :on-remove="handleRemove"
                :on-exceed="handleExceed"
                :file-list="fileList"
                :limit="limit"
                multiple
                list-type="picture"
                :auto-upload="false"
                ref="uploadRef"
                @click.stop>
                <template v-slot:trigger>
<el-button  size="small" type="primary">选取文件</el-button>
</template>
                <el-button style="margin-left: 10px"
                          size="small"
                          type="success"
                          @click.stop="submitUpload"
                          :loading="uploading">
                  {{ uploading ? '上传中...' : '上传到服务器' }}
                </el-button>
                <template v-slot:tip>
<div  class="el-upload__tip">
                  支持多文件上传，最多支持选择{{limit}}个文件，每个文件不超过5MB
                </div>
</template>
              </el-upload>
            </el-form-item>
          </el-col>
        </el-row>

        <!-- 底部操作按钮 -->
        <div class="bottom-actions">
          <el-button type="primary" @click="submitForm">提交</el-button>
          <el-button @click="resetForm">重置</el-button>
          <div class="icon-buttons">
            <el-button icon="el-icon-search" circle />
            <el-button icon="el-icon-edit" circle />
            <el-button icon="el-icon-delete" circle />
            <el-button icon="el-icon-right" circle />
          </div>
        </div>
      </el-form>
    </el-card>
  </div>
</template>

<script>
import api from '../../api'
import { Editor, Toolbar } from '@wangeditor/editor-for-vue'
import '@wangeditor/editor/dist/css/style.css'

export default {
  components: {
    Editor,
    Toolbar
  },

  data() {
    return {
      form: {
        title: '',
        description: '',
        address: '',
        type: '',
        phone: '',
        monthlyRent: 0,
        deposit: 0,
        imageSrc: ''
      },
      editorRef: null,
      toolbarConfig: {
        excludeKeys: ['fullScreen', 'code', 'todo']
      },
      editorConfig: {
        placeholder: '请输入描述',
        height: 300,
        MENU_CONF: {}
      },

      rules: {
        title: [
          { required: true, message: '请输入标题', trigger: 'blur' }
        ],
        description: [
          { required: true, message: '请输入描述', trigger: 'blur' }
        ],
        address: [
          { required: true, message: '请输入地址', trigger: 'blur' }
        ],
        type: [
          { required: true, message: '请选择类型', trigger: 'change' }
        ],
        monthlyRent: [
          { required: true, message: '请输入月租', trigger: 'blur' },
          { type: 'number', message: '请输入有效的数字', trigger: 'blur' }
        ],
        deposit: [
          { required: true, message: '请输入押金', trigger: 'blur' },
          { type: 'number', message: '请输入有效的数字', trigger: 'blur' }
        ]
      },
      limit: 5,
      fileList: [],
      uploading: false,
      selectedFiles: []
    }
  },
  // 组件销毁时，销毁编辑器
  beforeUnmount() {
    if (this.editorRef) {
      this.editorRef.destroy()
    }
  },
  methods: {
    // 编辑器实例创建成功
    editorCreated(editor) {
      this.editorRef = editor
    },

    // 编辑器内容变化
    editorChange(editor) {
      this.form.description = editor.getHtml()
    },

    handleExceed(files, fileList) {
      this.$message.warning(`最多只能选择 ${this.limit} 个文件，当前已选择 ${fileList.length + files.length} 个文件`);
    },


    // 文件选择变化
    handleFileChange(file, fileList) {
      this.fileList = fileList
      this.selectedFiles = fileList
    },

    // 文件移除
    handleRemove(file, fileList) {
      this.fileList = fileList
      this.selectedFiles = fileList
    },

    // 手动提交上传
    async submitUpload() {
      if (this.selectedFiles.length === 0) {
        this.$message.warning('请先选择文件')
        return
      }

      this.uploading = true

      try {
        // 提取文件数组（使用file.raw获取原始File对象）
        const files = this.selectedFiles.map(file => file.raw)

        // 调用api.uploadRoomImage函数
        const response = await api.rent.uploadRoomImage(files, progressEvent => {
          const percentCompleted = Math.round(
            (progressEvent.loaded * 100) / progressEvent.total
          )
          console.log(`上传进度: ${percentCompleted}%`)
        })

        console.log('上传结果:', response)

        // 如果后端返回的是Ok()，没有数据
        if (response.status === 200) {
          this.$message.success(`成功上传 ${this.selectedFiles.length} 个文件`)
          // 假设后端返回的是图片路径数组，这里简化处理
          // 实际情况需要根据后端返回的数据结构来处理
          this.form.imageSrc = 'uploaded-images/' + this.selectedFiles[0].name
        }

        // 延迟清空文件列表，避免触发组件状态异常
        setTimeout(() => {
          this.selectedFiles = []
          this.fileList = []
        }, 300)

      } catch (error) {
        console.error('上传失败:', error)
        this.$message.error('文件上传失败: ' + (error.response?.data || error.message))
      } finally {
        this.uploading = false
      }
    },

    // 提交表单
    async submitForm() {
      this.$refs.form.validate(async (valid) => {
        if (valid) {
          try {
            // 调用API创建租房信息
            const response = await api.rent.create(this.form)
            if (response.status === 200) {
              this.$message.success('创建成功')
              // 重置表单
              this.resetForm()
              // 跳转到列表页面
              this.$router.push('/renthouse')
            }
          } catch (error) {
            console.error('创建失败:', error)
            this.$message.error('创建失败: ' + (error.response?.data || error.message))
          }
        } else {
          this.$message.warning('请填写必填字段')
          return false
        }
      })
    },

    // 重置表单
    resetForm() {
      this.$refs.form.resetFields()
      this.fileList = []
      this.selectedFiles = []
    }
  }
}
</script>

<style scoped>
.create-rent-house-container {
  padding: 20px;
}

.create-rent-house-card {
  max-width: 100%;
  margin: 0 auto;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.upload-demo {
  margin-top: 0;
}

.bottom-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 30px;
  padding-top: 20px;
  border-top: 1px solid #ebeef5;
}

.icon-buttons {
  display: flex;
  gap: 10px;
}

/* 调整输入框高度，使布局更紧凑 */
.el-input__inner {
  height: 36px;
  line-height: 36px;
}

/* 调整表单标签字体大小 */
.el-form-item__label {
  font-size: 14px;
  font-weight: 500;
}

/* 调整上传组件按钮大小 */
.el-upload__trigger {
  margin-right: 10px;
}

/* 确保编辑器高度为300px，优先级高于默认样式 */
:deep(.w-e-text-container) {
  min-height: 300px !important;
  height: 300px !important;
}

:deep(.w-e-scroll) {
  min-height: 300px !important;
  height: 300px !important;
}

/* 上传文件列表横向排列 */
:deep(.el-upload-list) {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

:deep(.el-upload-list__item) {
  width: calc(20% - 8px); /* 5个文件一行，减去gap */
  margin-bottom: 0 !important;
}

/* 确保图片预览也横向排列 */
:deep(.el-upload-list--picture) {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

:deep(.el-upload-list--picture .el-upload-list__item) {
  width: calc(20% - 8px); /* 5个文件一行，减去gap */
  margin-bottom: 0 !important;
}
</style>
