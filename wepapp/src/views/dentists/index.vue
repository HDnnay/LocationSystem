<template>
    <div class="dentists">
        <!-- 页面头部区域 -->
        <div class="page-header gradient-bg">
            <div class="header-content">
                <div class="page-info">
                    <h2 class="page-title">医师管理</h2>
                <p class="page-description">管理诊所医师信息</p>
                </div>
                <div class="header-actions">
                    <el-button type="primary" @click="showAddDialog">
                        <el-icon><Plus /></el-icon> 新增医师
                    </el-button>
                </div>
            </div>
        </div>

        <!-- 统计卡片区域 -->
        <div class="stats-grid">
            <!-- 牙医总数卡片 -->
            <div class="stat-card dentist-card">
                <div class="stat-icon dentist-icon">👨‍⚕️</div>
                <div class="stat-content">
                    <h3 class="stat-number">{{ dentists.length }}</h3>
                    <p class="stat-label">医师总数</p>
                </div>
                <div class="stat-status">
                    <span class="status-item active">
                        <span class="status-dot green"></span>
                        在岗: {{ activeDentists }}
                    </span>
                    <span class="status-item inactive">
                        <span class="status-dot gray"></span>
                        休假: {{ dentists.length - activeDentists }}
                    </span>
                </div>
            </div>

            <!-- 今日排班卡片 -->
            <div class="stat-card schedule-card">
                <div class="stat-icon schedule-icon">📅</div>
                <div class="stat-content">
                    <h3 class="stat-number">{{ todayAppointments }}</h3>
                    <p class="stat-label">今日排班</p>
                </div>
                <div class="stat-status">
                    <span class="status-text">今日{{ todayAppointments }}个预约</span>
                </div>
            </div>

            <!-- 科室分布卡片 -->
            <div class="stat-card department-card">
                <div class="stat-icon department-icon">🏥</div>
                <div class="stat-content">
                    <h3 class="stat-number">{{ departmentCount }}</h3>
                    <p class="stat-label">科室分布</p>
                </div>
                <div class="stat-status">
                    <span class="status-text">覆盖{{ departmentCount }}个科室</span>
                </div>
            </div>
        </div>

        <!-- 搜索和筛选区域 -->
        <div class="search-filter-section">
            <div class="search-box">
                <el-input
                        v-model="searchKeyword"
                        placeholder="按姓名或ID搜索"
                        clearable
                        @input="handleSearch"
                    >
                    <template #prefix>
                        <el-icon><Search /></el-icon>
                    </template>
                </el-input>
            </div>
            <div class="filter-box">
                <el-select
                        v-model="filterDepartment"
                        placeholder="选择科室"
                        @change="handleFilter"
                        style="width: 160px; margin-right: 10px;"
                    >
                    <el-option label="所有科室" value="" />
                    <el-option v-for="dept in departments" :key="dept" :label="dept" :value="dept" />
                </el-select>
                <el-select
                        v-model="filterStatus"
                        placeholder="状态"
                        @change="handleFilter"
                        style="width: 120px;"
                    >
                    <el-option label="所有状态" value="" />
                    <el-option label="在岗" value="active" />
                    <el-option label="休假" value="vacation" />
                </el-select>
            </div>
        </div>

        <!-- 牙医列表表格 -->
        <div class="dentist-table-section card">
            <div class="table-header">
                <h3 class="section-title">医师列表</h3>
                <div class="table-actions">
                    <el-button type="info" size="small">
                        <el-icon><Download /></el-icon> 导出
                    </el-button>
                    <el-button type="danger" size="small" @click="handleBatchDelete">
                        <el-icon><Delete /></el-icon> 批量删除
                    </el-button>
                </div>
            </div>

            <el-table
                :data="paginatedDentists"
                style="width: 100%"
                @selection-change="handleSelectionChange"
            >
                <el-table-column type="selection" width="55" />
                <el-table-column prop="id" label="医师ID" width="100" sortable />
                <el-table-column prop="name" label="姓名" width="120" sortable />
                <el-table-column prop="gender" label="性别" width="80">
                    <template #default="scope">
                        <el-tag>{{ scope.row.gender === 'male' ? '男' : '女' }}</el-tag>
                    </template>
                </el-table-column>
                <el-table-column prop="department" label="科室" width="120" />
                <el-table-column prop="position" label="职称" width="120" />
                <el-table-column prop="phone" label="电话" width="150" />
                <el-table-column prop="status" label="状态" width="100">
                    <template #default="scope">
                        <el-tag :type="scope.row.status === 'active' ? 'success' : 'warning'">
                            {{ scope.row.status === 'active' ? '在岗' : '休假' }}
                        </el-tag>
                    </template>
                </el-table-column>
                <el-table-column prop="joinDate" label="入职日期" width="130" sortable />
                <el-table-column label="操作" width="180" fixed="right">
                    <template #default="scope">
                        <el-button type="primary" size="small" @click="showEditDialog(scope.row)">
                        <el-icon><Edit /></el-icon> 编辑
                    </el-button>
                        <el-button type="danger" size="small" @click="handleDelete(scope.row.id)">
                        <el-icon><Delete /></el-icon> 删除
                    </el-button>
                    </template>
                </el-table-column>
            </el-table>

            <!-- 分页 -->
            <div class="pagination">
                <el-pagination
                    @size-change="handleSizeChange"
                    @current-change="handleCurrentChange"
                    :current-page="currentPage"
                    :page-sizes="[10, 20, 50, 100]"
                    :page-size="pageSize"
                    layout="total, sizes, prev, pager, next, jumper"
                    :total="filteredDentists.length"
                />
            </div>
        </div>

        <!-- 新增/编辑医师对话框 -->
        <el-dialog
            v-model="dialogVisible"
            :title="isEdit ? '编辑医师' : '新增医师'"
            width="500px"
        >
            <el-form :model="formData" label-width="80px">
                <el-form-item label="姓名">
                    <el-input v-model="formData.name" placeholder="请输入医师姓名" />
                </el-form-item>
                <el-form-item label="性别">
                    <el-select v-model="formData.gender" placeholder="选择性别">
                        <el-option label="男" value="male" />
                        <el-option label="女" value="female" />
                    </el-select>
                </el-form-item>
                <el-form-item label="科室">
                    <el-select v-model="formData.department" placeholder="选择科室">
                        <el-option v-for="dept in departments" :key="dept" :label="dept" :value="dept" />
                    </el-select>
                </el-form-item>
                <el-form-item label="职称">
                    <el-input v-model="formData.position" placeholder="请输入职称" />
                </el-form-item>
                <el-form-item label="电话">
                    <el-input v-model="formData.phone" placeholder="请输入电话号码" />
                </el-form-item>
                <el-form-item label="状态">
                    <el-select v-model="formData.status" placeholder="选择状态">
                        <el-option label="在岗" value="active" />
                        <el-option label="休假" value="vacation" />
                    </el-select>
                </el-form-item>
                <el-form-item label="入职日期">
                    <el-date-picker
                        v-model="formData.joinDate"
                        type="date"
                        placeholder="选择入职日期"
                        style="width: 100%"
                    />
                </el-form-item>
            </el-form>
            <template #footer>
                <div class="dialog-footer">
                    <el-button @click="dialogVisible = false">取消</el-button>
                    <el-button type="primary" @click="handleSave">保存</el-button>
                </div>
            </template>
        </el-dialog>
    </div>
</template>

<script>
import { Plus, Search, Download, Edit, Delete } from '@element-plus/icons-vue'

export default {
    name: 'Dentists',
    components: {
        Plus,
        Search,
        Download,
        Edit,
        Delete
    },
    data() {
        return {
            // 页面数据
            currentDate: '',
            // 搜索筛选
            searchKeyword: '',
            filterDepartment: '',
            filterStatus: '',
            // 牙医列表
            dentists: [
                { id: 'D001', name: '张三', gender: 'male', department: '口腔科', position: '主治医师', phone: '13800138001', status: 'active', joinDate: '2020-05-15' },
                { id: 'D002', name: '李四', gender: 'female', department: '正畸科', position: '副主任医师', phone: '13800138002', status: 'active', joinDate: '2019-08-20' },
                { id: 'D003', name: '王五', gender: 'male', department: '牙周科', position: '主治医师', phone: '13800138003', status: 'vacation', joinDate: '2021-03-10' },
                { id: 'D004', name: '赵六', gender: 'female', department: '儿童牙科', position: '医师', phone: '13800138004', status: 'active', joinDate: '2022-01-15' },
                { id: 'D005', name: '孙七', gender: 'male', department: '口腔修复科', position: '副主任医师', phone: '13800138005', status: 'active', joinDate: '2018-10-05' }
            ],
            // 科室列表
            departments: ['口腔科', '正畸科', '牙周科', '儿童牙科', '口腔修复科', '口腔外科'],
            // 分页
            currentPage: 1,
            pageSize: 10,
            // 选中项
            selectedDentists: [],
            // 对话框
            dialogVisible: false,
            isEdit: false,
            formData: {
                id: '',
                name: '',
                gender: 'male',
                department: '',
                position: '',
                phone: '',
                status: 'active',
                joinDate: ''
            }
        }
    },
    computed: {
        // 过滤后的牙医列表
        filteredDentists() {
            let result = [...this.dentists]
            
            // 关键词搜索
            if (this.searchKeyword) {
                const keyword = this.searchKeyword.toLowerCase()
                result = result.filter(item => 
                    item.name.toLowerCase().includes(keyword) || 
                    item.id.toLowerCase().includes(keyword)
                )
            }
            
            // 科室筛选
            if (this.filterDepartment) {
                result = result.filter(item => item.department === this.filterDepartment)
            }
            
            // 状态筛选
            if (this.filterStatus) {
                result = result.filter(item => item.status === this.filterStatus)
            }
            
            return result
        },
        // 分页后的医师列表
        paginatedDentists() {
            const startIndex = (this.currentPage - 1) * this.pageSize
            const endIndex = startIndex + this.pageSize
            return this.filteredDentists.slice(startIndex, endIndex)
        },
        // 统计数据
        activeDentists() {
            return this.dentists.filter(dentist => dentist.status === 'active').length
        },
        departmentCount() {
            const deptSet = new Set(this.dentists.map(dentist => dentist.department))
            return deptSet.size
        },
        todayAppointments() {
            // 模拟今日预约数
            return Math.floor(Math.random() * 20) + 5
        }
    },
    mounted() {
        this.updateDateTime()
    },
    methods: {
        // 更新日期时间
        updateDateTime() {
            const now = new Date()
            const year = now.getFullYear()
            const month = String(now.getMonth() + 1).padStart(2, '0')
            const day = String(now.getDate()).padStart(2, '0')
            const weekdays = ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六']
            const weekday = weekdays[now.getDay()]
            
            this.currentDate = `${year}-${month}-${day} ${weekday}`
        },
        
        // 搜索和筛选
        handleSearch() {
            this.currentPage = 1
        },
        
        handleFilter() {
            this.currentPage = 1
        },
        
        // 分页处理
        handleSizeChange(val) {
            this.pageSize = val
            this.currentPage = 1
        },
        
        handleCurrentChange(val) {
            this.currentPage = val
        },
        
        // 表格选择
        handleSelectionChange(val) {
            this.selectedDentists = val
        },
        
        // 对话框操作
        showAddDialog() {
            this.isEdit = false
            this.formData = {
                id: '',
                name: '',
                gender: 'male',
                department: '',
                position: '',
                phone: '',
                status: 'active',
                joinDate: ''
            }
            this.dialogVisible = true
        },
        
        showEditDialog(row) {
            this.isEdit = true
            this.formData = {...row}
            this.dialogVisible = true
        },
        
        // 保存
        handleSave() {
            if (this.isEdit) {
                // 编辑模式
                const index = this.dentists.findIndex(d => d.id === this.formData.id)
                if (index !== -1) {
                    this.dentists.splice(index, 1, {...this.formData})
                    this.$message.success('医师信息更新成功')
                }
            } else {
                // 添加模式
                const newId = 'D' + String(this.dentists.length + 1).padStart(3, '0')
                const newDentist = {
                    ...this.formData,
                    id: newId
                }
                this.dentists.unshift(newDentist)
                this.$message.success('医师添加成功')
            }
            this.dialogVisible = false
        },
        
        // 删除操作
        handleDelete(id) {
            this.$confirm('确定要删除这位医师吗？', '警告', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                const index = this.dentists.findIndex(d => d.id === id)
                if (index !== -1) {
                    this.dentists.splice(index, 1)
                    this.$message.success('删除成功')
                }
            }).catch(() => {
                // 取消删除
            })
        },
        
        handleBatchDelete() {
            if (this.selectedDentists.length === 0) {
                this.$message.warning('请选择要删除的医师')
                return
            }
            
            this.$confirm(`确定要删除选中的${this.selectedDentists.length}位医师吗？`, '警告', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                const idsToDelete = this.selectedDentists.map(d => d.id)
                this.dentists = this.dentists.filter(d => !idsToDelete.includes(d.id))
                this.selectedDentists = []
                this.$message.success('批量删除成功')
            }).catch(() => {
                // Cancel deletion
            })
        }
    }
}
</script>

<style scoped>
    .dentists {
        padding: 1rem 0;
    }

    /* 渐变背景 */
    .gradient-bg {
        background: var(--primary-color);
        color: white;
        border-radius: 8px;
    }

    /* 页面头部 */
    .page-header {
        margin-bottom: 1.5rem;
        padding: 1.5rem;
    }

    .header-content {
        display: flex;
        justify-content: space-between;
        align-items: center;
        flex-wrap: wrap;
        gap: 1rem;
    }

    .page-info {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
    }

    .page-title {
        font-size: 1.8rem;
        margin: 0;
    }

    .page-description {
        font-size: 1rem;
        margin: 0;
        opacity: 0.9;
    }

    /* 搜索筛选区域 */
    .search-filter-section {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1.5rem;
        flex-wrap: wrap;
        gap: 1rem;
    }

    .search-box {
        flex: 1;
        min-width: 250px;
    }

    .filter-box {
        display: flex;
        gap: 0.5rem;
        flex-wrap: wrap;
    }

    /* 统计卡片 */
    .stats-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
        gap: 1.5rem;
        margin-bottom: 2rem;
    }

    .stat-card {
        background: white;
        border-radius: 8px;
        padding: 1.5rem;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        transition: transform 0.2s, box-shadow 0.2s;
        border: 1px solid #f0f0f0;
    }

    .stat-card:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    }

    /* 不同类型卡片的颜色主题 */
    .dentist-card .stat-icon {
        background: #4285f4;
    }

    .schedule-card .stat-icon {
        background: #ea4335;
    }

    .department-card .stat-icon {
        background: #fbbc05;
    }

    .stat-icon {
        font-size: 2.5rem;
        width: 60px;
        height: 60px;
        display: flex;
        align-items: center;
        justify-content: center;
        color: white;
        border-radius: 8px;
        margin-bottom: 1rem;
    }

    .stat-content {
        margin-bottom: 1rem;
    }

    .stat-number {
        font-size: 2.2rem;
        font-weight: 600;
        color: var(--text-color);
        margin: 0;
        line-height: 1;
    }

    .stat-label {
        color: #666;
        margin: 0.5rem 0 0;
        font-size: 1rem;
    }

    /* 卡片状态信息 */
    .stat-status {
        display: flex;
        flex-wrap: wrap;
        gap: 1rem;
    }

    .status-item {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        font-size: 0.85rem;
    }

    .status-dot {
        width: 8px;
        height: 8px;
        border-radius: 50%;
    }

    .status-dot.green {
        background-color: #43e97b;
    }

    .status-dot.gray {
        background-color: #d1d1d1;
    }

    .status-text {
        color: #666;
        font-size: 0.85rem;
    }

    /* 牙医列表区域 */
    .dentist-table-section {
        background: white;
        border-radius: 8px;
        padding: 1.5rem;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    }

    .table-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1.5rem;
        flex-wrap: wrap;
        gap: 1rem;
    }

    .table-actions {
        display: flex;
        gap: 0.5rem;
    }

    /* 分页 */
    .pagination {
        margin-top: 1.5rem;
        display: flex;
        justify-content: flex-end;
    }

    /* 通用标题样式 */
    .section-title {
        font-size: 1.25rem;
        margin-bottom: 1rem;
        color: var(--text-color);
        font-weight: 600;
    }

    /* 移动端响应式 */
    @media (max-width: 768px) {
        .search-filter-section {
            flex-direction: column;
            align-items: stretch;
        }

        .search-box {
            width: 100%;
        }

        .filter-box {
            justify-content: flex-start;
        }

        .stats-grid {
            grid-template-columns: 1fr;
        }

        .table-header {
            flex-direction: column;
            align-items: flex-start;
        }

        .pagination {
            justify-content: center;
        }
    }
</style>