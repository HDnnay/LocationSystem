<template>
  <div>
    <el-upload
      class="upload-demo"
      :show-file-list="true"
      :on-change="handleFileChange"
      :on-remove="handleRemove"
      :file-list="fileList"
      :limit="5"
      list-type="picture"
      :auto-upload="false"
      ref="uploadRef"
      @click.stop>
      <el-button slot="trigger" size="small" type="primary">选取文件</el-button>
      <el-button style="margin-left: 10px"
                 size="small"
                 type="success"
                 @click.stop="submitUpload"
                 :loading="uploading">
        {{ uploading ? '上传中...' : '上传到服务器' }}
      </el-button>
      <div slot="tip" class="el-upload__tip">
        支持多文件上传，每个文件不超过5MB
      </div>
    </el-upload>
  </div>
</template>

<script>
  import api from '../api'

  export default {
    data() {
      return {
        fileList: [],
        uploading: false,
        selectedFiles: []
      }
    },
    methods: {
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

      handleRemove(file, fileList) {
        console.log('移除文件:', file)
      }
    }
  }
</script>
