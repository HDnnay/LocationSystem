<template>
  <div>
    <el-page-header class="page-container">
      <template #title>
        <h3>租房信息管理</h3>
      </template>
    </el-page-header>

    <el-card class="toolbar-card" shadow="hover">
      <div class="toolbar">
        <el-input v-model="searchQuery" placeholder="搜索租房"
                  clearable
                  style="width: 400px; margin-right: 10px;"
                  @keyup.enter="onSearch">
          <template #append>
            <el-button type="primary" @click="onSearch" :loading="loading">搜索</el-button>
          </template>
        </el-input>
      </div>
    </el-card>

    <el-card class="table-card" shadow="hover">
      <div class="table-container">
        <el-table v-loading="loading" :data="rent_houses"
                  style="width: 100%"
                  stripe
                  border
                  fit>
          <!-- 序号列 -->
          <el-table-column type="index"
                           :index="(index)=>(currentPage - 1) * pageSize + index + 1"
                           label="序号" width="60" align="center" />

          <!-- ID列（可根据需要隐藏） -->
          <el-table-column prop="id" label="ID" show-overflow-tooltip min-width="200" v-if="showIdColumn" />

          <!-- 标题 -->
          <el-table-column prop="title" label="标题" min-width="80" show-overflow-tooltip />

          <!-- 地址 -->
          <el-table-column prop="address" label="地址" min-width="80" show-overflow-tooltip />

          <!-- 描述 -->
          <el-table-column prop="description" label="描述" min-width="80" show-overflow-tooltip>
            <template #default="scope">
              <span v-if="scope.row.description">{{ escapeHtml(scope.row.description) }}</span>
              <span v-else style="color: #999;">无描述</span>
            </template>
          </el-table-column>

          <!-- 类型 -->
          <el-table-column prop="type" label="类型" min-width="70" align="center">
            <template #default="scope">
              <el-tag :type="getTypeTagType(scope.row.type)" size="small">
                {{ getTypeLabel(scope.row.type) }}
              </el-tag>
            </template>
          </el-table-column>

          <!-- 电话 -->
          <el-table-column prop="phone" label="电话" min-width="100" align="center" show-overflow-tooltip />

          <!-- 月租 -->
          <el-table-column prop="monthlyRent" label="月租(元)" min-width="80" align="center">
            <template #default="scope">
              <span>{{ formatMoney(scope.row.monthlyRent) }}</span>
            </template>
          </el-table-column>

          <!-- 押金 -->
          <el-table-column prop="deposit" label="押金(元)" min-width="80" align="center">
            <template #default="scope">
              <span>{{ formatMoney(scope.row.deposit) }}</span>
            </template>
          </el-table-column>

          <!-- 创建时间 -->
          <el-table-column prop="createTime" label="创建时间" min-width="140" align="center">
            <template #default="scope">
              {{ formatDateTime(scope.row.createTime) }}
            </template>
          </el-table-column>

          <!-- 创建用户ID -->
          <el-table-column prop="createUserId" label="创建人ID" show-overflow-tooltip min-width="180" v-if="showCreateUserId" />

          <!-- 操作列 -->
          <el-table-column label="操作" min-width="100" align="center">
            <template #default="scope">
              <div style="display: flex; justify-content: center; align-items: center; padding: 4px 0; width: 100%;">
                <el-button type="primary" size="small" @click="CopyData(scope.row)" style="margin-right: 5px;">复制</el-button>
                <el-button type="success" size="small" @click="viewDetails(scope.row)">查看</el-button>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </div>

      <!-- 分页 -->
      <div class="pagination-container">
        <el-pagination :current-page="currentPage"
                       :page-size="pageSize"
                       :page-sizes="[10, 20, 50, 100]"
                       :total="total"
                       layout="total, sizes, prev, pager, next, jumper"
                       @update:current-page="handleCurrentChange"
                       @update:page-size="handleSizeChange" />
      </div>
    </el-card>

    <!-- 查看详情对话框 -->
    <el-dialog v-model="detailDialogVisible" title="租房详情" width="800px">
      <div class="detail-container">
        <!-- 左侧信息区域 -->
        <div class="detail-info-section">
          <div class="detail-item">
            <span class="detail-label">标题:</span>
            <span class="detail-value">{{ currentDetail.title }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">地址:</span>
            <span class="detail-value">{{ currentDetail.address }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">描述:</span>
            <span class="detail-value">{{ currentDetail.description || '无描述' }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">类型:</span>
            <span class="detail-value">
              <el-tag :type="getTypeTagType(currentDetail.type)" size="small">
                {{ getTypeLabel(currentDetail.type) }}
              </el-tag>
            </span>
          </div>
          <div class="detail-item">
            <span class="detail-label">电话:</span>
            <span class="detail-value">{{ currentDetail.phone }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">月租:</span>
            <span class="detail-value">{{ formatMoney(currentDetail.monthlyRent) }}元</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">押金:</span>
            <span class="detail-value">{{ formatMoney(currentDetail.deposit) }}元</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">创建时间:</span>
            <span class="detail-value">{{ formatDateTime(currentDetail.createTime) }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">创建人ID:</span>
            <span class="detail-value">{{ currentDetail.createUserId }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">记录ID:</span>
            <span class="detail-value">{{ currentDetail.id }}</span>
          </div>
        </div>

        <!-- 右侧图片区域 -->
        <div class="detail-image-section">
          <div v-if="currentDetail.images && currentDetail.images.length > 0" class="image-preview-container">
            <div class="image-preview-main">
              <img :src="getImageUrl(currentDetail.images[currentImageIndex])" :alt="currentDetail.title" class="main-image">
              <div class="image-navigation">
                <el-button type="primary" circle @click="prevImage" :disabled="currentImageIndex === 0">
                  <el-icon><ArrowLeft /></el-icon>
                </el-button>
                <span class="image-counter">{{ currentImageIndex + 1 }} / {{ currentDetail.images.length }}</span>
                <el-button type="primary" circle @click="nextImage" :disabled="currentImageIndex === currentDetail.images.length - 1">
                  <el-icon><ArrowRight /></el-icon>
                </el-button>
              </div>
            </div>
            <div class="image-thumbnails">
              <div
                v-for="(image, index) in currentDetail.images"
                :key="index"
                class="thumbnail-item"
                :class="{ active: index === currentImageIndex }"
                @click="currentImageIndex = index"
              >
                <img :src="getImageUrl(image)" :alt="`缩略图 ${index + 1}`" class="thumbnail-image">
              </div>
            </div>
          </div>
          <div v-else class="no-image">
            <span style="color: #999;">无图片</span>
          </div>
        </div>
      </div>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="detailDialogVisible = false">关闭</el-button>
          <el-button type="primary" @click="CopyData(currentDetail)">复制信息</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import api from "../../api"
  import { ArrowLeft, ArrowRight } from '@element-plus/icons-vue'

  export default defineComponent({
    components: {
      ArrowLeft,
      ArrowRight
    },
    directives: {
    },
    filters: {
    },
    props: {
    },
    data() {
      return {
        rent_houses: [],
        searchQuery: "",
        loading: false,
        currentPage: 1,
        pageSize: 10,
        total: 0,
        // 是否显示ID列
        showIdColumn: false,
        // 是否显示创建人ID列
        showCreateUserId: false,
        // 详情对话框
        detailDialogVisible: false,
        currentDetail: {},
        // 当前图片索引
        currentImageIndex: 0,
        // 表格宽度
        tableWidth: 0
      }
    },
    computed: {
    },
    watch: {
    },
    beforeCreate() {
    },
    created() {
      this.getData()
    },
    beforeMount() {
    },
    mounted() {
      this.calculateTableWidth()
      window.addEventListener('resize', this.calculateTableWidth)
    },
    updated() {
    },
    activated() {
    },
    deactivated() {
    },
    beforeUnmount() {
      window.removeEventListener('resize', this.calculateTableWidth)
    },
    unmounted() {
    },
    methods: {
      // 转义HTML标签和特殊字符，用于显示富文本内容为纯文本
      escapeHtml(html) {
        if (!html) return ''
        // 移除所有HTML标签
        let plainText = html.replace(/<[^>]*>/g, '')
        // 处理HTML实体
        plainText = plainText
          .replace(/&lt;/g, '<')
          .replace(/&gt;/g, '>')
          .replace(/&amp;/g, '&')
          .replace(/&quot;/g, '"')
          .replace(/&#39;/g, "'")
          .replace(/&nbsp;/g, ' ')
          .replace(/&copy;/g, '©')
          .replace(/&reg;/g, '®')
        // 处理多余的空白字符
        plainText = plainText.replace(/\s+/g, ' ').trim()
        return plainText
      },

      // 计算表格宽度
      calculateTableWidth() {
        this.$nextTick(() => {
          const tableContainer = this.$el.querySelector('.table-container')
          if (tableContainer) {
            this.tableWidth = tableContainer.offsetWidth
            console.log('Table width calculated:', this.tableWidth)
            // 计算操作列宽度
            this.calculateActionColumnWidth()
          }
        })
      },

      // 计算操作列宽度
      calculateActionColumnWidth() {
        this.$nextTick(() => {
          const tableContainer = this.$el.querySelector('.table-container')
          const table = this.$el.querySelector('.el-table')
          const headerRow = this.$el.querySelector('.el-table__header-row')
          const bodyRow = this.$el.querySelector('.el-table__body-row')

          if (tableContainer && table && headerRow && bodyRow) {
            // 获取表格容器宽度
            const containerWidth = tableContainer.offsetWidth
            console.log('Container width:', containerWidth)

            // 获取所有列的单元格
            const headerCells = headerRow.querySelectorAll('.el-table__cell')
            const bodyCells = bodyRow.querySelectorAll('.el-table__cell')

            // 计算除操作列外的所有列的宽度总和
            let totalWidth = 0
            const cells = headerCells.length > 0 ? headerCells : bodyCells

            cells.forEach((cell, index) => {
              if (index < cells.length - 1) { // 排除操作列
                totalWidth += cell.offsetWidth
              }
            })

            console.log('Total width of other columns:', totalWidth)

            // 计算操作列宽度
            const actionColumnWidth = containerWidth - totalWidth
            console.log('Action column width calculated:', actionColumnWidth)

            // 确保操作列宽度至少能够容纳两个按钮
            const minActionColumnWidth = 120
            const finalActionColumnWidth = Math.max(actionColumnWidth, minActionColumnWidth)
            console.log('Final action column width:', finalActionColumnWidth)

            // 设置操作列宽度
            if (this.$refs.actionColumn) {
              // 尝试直接设置宽度
              this.$refs.actionColumn.width = finalActionColumnWidth
              console.log('Action column width set via ref:', finalActionColumnWidth)
            }

            // 同时更新表格宽度
            table.style.width = containerWidth + 'px'

            // 更新操作列的单元格宽度
            if (cells.length > 0) {
              const actionHeaderCell = headerCells[headerCells.length - 1]
              const actionBodyCell = bodyCells[bodyCells.length - 1]

              if (actionHeaderCell) {
                actionHeaderCell.style.width = finalActionColumnWidth + 'px'
              }
              if (actionBodyCell) {
                actionBodyCell.style.width = finalActionColumnWidth + 'px'
              }
            }
          }
        })
      },

      // 监听侧边栏变化（如果需要）
      watchSidebarChange() {
        // 这里可以添加对侧边栏状态的监听
        // 例如，通过props或事件总线获取侧边栏状态
        this.calculateTableWidth()
      },

      // 复制到剪贴板
      async copyToClipboard(text) {
        console.log(text)
      },

      // 复制数据
      async CopyData(rowData) {
        console.log('复制行数据:', rowData)

      },

      // 查看详情
      viewDetails(rowData) {
        console.log('查看详情:')
        console.log(rowData)
        this.currentDetail = { ...rowData }
        // 处理ImageSrc字段，转换为数组
        if (rowData.imageSrc) {
          this.currentDetail.images = rowData.imageSrc.split(',').filter(img => img.trim())
        } else {
          this.currentDetail.images = []
        }
        this.currentImageIndex = 0 // 重置图片索引
        this.detailDialogVisible = true
      },

      // 获取图片URL
      getImageUrl(image) {
        if (!image) return ''
        // 如果是完整URL，直接返回
        if (image.startsWith('http://') || image.startsWith('https://')) {
          return image
        }
        // 否则使用相对路径，自动适配当前端口
        // 确保路径格式正确，去除首尾空格
        const cleanImageName = image.trim()
        return `/api/renthouse/preview/${cleanImageName}`
      },

      // 上一张图片
      prevImage() {
        if (this.currentImageIndex > 0) {
          this.currentImageIndex--
        }
      },

      // 下一张图片
      nextImage() {
        if (this.currentDetail.images && this.currentImageIndex < this.currentDetail.images.length - 1) {
          this.currentImageIndex++
        }
      },

      // 格式化金额
      formatMoney(amount) {
        if (amount === null || amount === undefined) return '0'
        return Number(amount).toLocaleString('zh-CN')
      },

      // 格式化日期时间
      formatDateTime(dateTimeStr) {
        if (!dateTimeStr) return ''

        try {
          const date = new Date(dateTimeStr)
          return date.toLocaleString('zh-CN', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
          }).replace(/\//g, '-')
        } catch (error) {
          return dateTimeStr
        }
      },

      // 获取类型标签样式
      getTypeTagType(type) {
        const typeMap = {
          1: 'success', // 成功/可用
          2: 'warning', // 警告/待定
          3: 'danger',  // 危险/不可用
          4: 'info'     // 信息/其他
        }
        return typeMap[type] || 'info'
      },

      // 获取类型标签文本
      getTypeLabel(type) {
        const typeLabels = {
          1: '一房一厅',
          2: '两房一厅',
          3: '三房一厅',
        }
        return typeLabels[type] || `类型${type}`
      },

      handleSizeChange(newPageSize) {
        if (newPageSize == this.pageSize) return

        this.pageSize = newPageSize
        this.currentPage = 1 // 重置到第一页
        this.getData()
      },

      handleCurrentChange(newPage) {
        // 后端返回当前也设置了,会导致触发该函数，要判断newPage == this.currentPage，
        // 要不然连续请求两次api -> this.getData();
        if (newPage == this.currentPage) return

        this.currentPage = newPage
        this.getData()
      },

      onSearch() {
        this.currentPage = 1 // 搜索时重置到第一页
        this.getData()
      },

      async getData() {
        try {
          this.loading = true
          const result = await api.rent.fetchRentHouses({
            page: this.currentPage,
            pageSize: this.pageSize,
            search: this.searchQuery.trim() || undefined
          })

          console.log('API响应结果:', result)

          // 根据你的响应结构，result.data是数组，result.total和result.currentPage是直接属性
          this.rent_houses = result.data.data || []
          this.total = result.data.total || 0
          this.currentPage = result.data.currentPage || 1

        } catch (error) {
          console.error('获取数据失败:', error)
          this.$message.error('获取数据失败: ' + (error.message || '未知错误'))
        } finally {
          this.loading = false
        }
      }
    },
  })
</script>

<style scoped>
  .page-container {
    padding: 5px;
    background-color: white;
    margin-bottom: 10px;
  }

  .toolbar-card {
    margin-bottom: 10px;
  }

  .table-card {
    margin-bottom: 10px;
  }

  .table-container {
    width: 100%;
    margin-bottom: 16px;
    overflow: hidden;
  }

  .table-container > .el-table {
    width: 100%;
    border-right: none;
  }

  /* 确保操作列与表格右侧贴合 */
  .table-container > .el-table .el-table__column--last {
    padding-right: 0;
  }

  .table-container > .el-table .el-table__cell {
    padding-right: 0;
  }

  /* 确保操作列内容充满单元格 */
  .table-container > .el-table .el-table__column--last .cell {
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
  }

  /* 详情容器布局 */
  .detail-container {
    display: flex !important;
    gap: 20px;
    width: 100%;
    flex-direction: row !important;
  }

  /* 左侧信息区域 */
  .detail-info-section {
    flex: 1;
    min-width: 0;
  }

  /* 右侧图片区域 */
  .detail-image-section {
    width: 300px;
    min-width: 300px;
  }

  /* 无图片状态 */
  .no-image {
    width: 100%;
    height: 250px;
    display: flex;
    align-items: center;
    justify-content: center;
    border: 2px solid #e0e0e0;
    border-radius: 8px;
    background-color: #f5f5f5;
  }

  /* 图片预览主区域 */
  .image-preview-main {
    height: 250px;
  }

  /* 移动端响应式 */
  @media (max-width: 768px) {
    .table-container {
      margin-bottom: 12px;
    }

    .table-card {
      margin-bottom: 8px;
    }

    .toolbar-card {
      margin-bottom: 8px;
    }

    .pagination-container {
      flex-direction: column;
      align-items: center;
      gap: 10px;
    }
  }

  .table-container::-webkit-scrollbar {
    height: 8px;
  }

  .table-container::-webkit-scrollbar-track {
    background: #f1f1f1;
    border-radius: 4px;
  }

  .table-container::-webkit-scrollbar-thumb {
    background: #c1c1c1;
    border-radius: 4px;
  }

  .table-container::-webkit-scrollbar-thumb:hover {
    background: #a8a8a8;
  }

  .pagination-container {
    display: flex;
    justify-content: center;
    margin-top: 20px;
  }

  .detail-item {
    display: flex;
    line-height: 28px;
  }

  .detail-label {
    width: 100px;
    font-weight: bold;
    color: #606266;
    text-align: right;
    margin-right: 12px;
  }

  .detail-value {
    flex: 1;
    color: #303133;
    word-break: break-all;
  }

  /* 图片预览样式 */
  .image-preview-container {
    width: 100%;
  }

  .image-preview-main {
    position: relative;
    width: 100%;
    height: 400px;
    border: 2px solid #e0e0e0;
    border-radius: 8px;
    overflow: hidden;
    margin-bottom: 16px;
    background-color: #f5f5f5;
  }

  .main-image {
    width: 100%;
    height: 100%;
    object-fit: contain;
  }

  .image-navigation {
    position: absolute;
    bottom: 15px;
    left: 50%;
    transform: translateX(-50%);
    display: flex;
    align-items: center;
    gap: 10px;
    background-color: rgba(0, 0, 0, 0.6);
    padding: 6px 12px;
    border-radius: 20px;
    width: 90%;
    max-width: 260px;
    justify-content: center;
  }

  .image-counter {
    color: white;
    font-size: 12px;
    min-width: 60px;
    text-align: center;
  }

  .image-thumbnails {
    display: flex;
    gap: 12px;
    overflow-x: auto;
    padding: 8px 0;
  }

  .thumbnail-item {
    width: 80px;
    height: 80px;
    border: 2px solid #e0e0e0;
    border-radius: 4px;
    overflow: hidden;
    cursor: pointer;
    transition: all 0.3s ease;
  }

  .thumbnail-item:hover {
    border-color: #409eff;
    transform: scale(1.05);
  }

  .thumbnail-item.active {
    border-color: #409eff;
    box-shadow: 0 0 0 2px rgba(64, 158, 255, 0.2);
  }

  .thumbnail-image {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }

  /* 滚动条样式 */
  .image-thumbnails::-webkit-scrollbar {
    height: 6px;
  }

  .image-thumbnails::-webkit-scrollbar-track {
    background: #f1f1f1;
    border-radius: 3px;
  }

  .image-thumbnails::-webkit-scrollbar-thumb {
    background: #c1c1c1;
    border-radius: 3px;
  }

  .image-thumbnails::-webkit-scrollbar-thumb:hover {
    background: #a8a8a8;
  }
</style>
