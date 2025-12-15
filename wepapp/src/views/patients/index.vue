<template>
    <div class="patients">
        <!-- 页面头部区域 -->
        <div class="page-header gradient-bg">
            <div class="header-content">
                <div class="page-info">
                    <h2 class="page-title">患者管理</h2>
                <p class="page-description">管理诊所患者信息</p>
                </div>
                <div class="header-actions">
                    <el-button type="primary" @click="showAddDialog">
                        <el-icon><Plus /></el-icon> 新增患者
                    </el-button>
                </div>
            </div>
        </div>

        <!-- 统计卡片区域 -->
        <div class="stats-grid">
            <!-- 患者总数卡片 -->
            <div class="stat-card patient-card">
                <div class="stat-icon patient-icon">👥</div>
                <div class="stat-content">
                    <h3 class="stat-number">{{ patients.length }}</h3>
                    <p class="stat-label">患者总数</p>
                </div>
                <div class="stat-status">
                    <span class="status-text">共管理 {{ patients.length }} 位患者</span>
                </div>
            </div>

            <!-- 今日预约卡片 -->
            <div class="stat-card appointment-card">
                <div class="stat-icon appointment-icon">📅</div>
                <div class="stat-content">
                    <h3 class="stat-number">{{ todayAppointments }}</h3>
                    <p class="stat-label">今日预约</p>
                </div>
                <div class="stat-status">
                    <span class="status-text">今日 {{ todayAppointments }} 个预约</span>
                </div>
            </div>

            <!-- 未就诊患者卡片 -->
            <div class="stat-card pending-card">
                <div class="stat-icon pending-icon">⏳</div>
                <div class="stat-content">
                    <h3 class="stat-number">{{ pendingPatients }}</h3>
                    <p class="stat-label">未就诊患者</p>
                </div>
                <div class="stat-status">
                    <span class="status-text">{{ pendingPatients }} 位患者待就诊</span>
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
                    v-model="filterGender"
                    placeholder="选择性别"
                    @change="handleFilter"
                    style="width: 120px; margin-right: 10px;"
                >
                    <el-option label="所有性别" value="" />
                    <el-option label="男" value="male" />
                    <el-option label="女" value="female" />
                </el-select>
                <el-select
                    v-model="filterStatus"
                    placeholder="就诊状态"
                    @change="handleFilter"
                    style="width: 160px;"
                >
                    <el-option label="所有状态" value="" />
                    <el-option label="已完成" value="completed" />
                    <el-option label="待就诊" value="pending" />
                    <el-option label="已取消" value="cancelled" />
                </el-select>
            </div>
        </div>

        <!-- 患者列表表格 -->
        <div class="patient-table-section card">
            <div class="table-header">
                <h3 class="section-title">患者列表</h3>
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
                :data="paginatedPatients"
                style="width: 100%"
                @selection-change="handleSelectionChange"
            >
                <el-table-column type="selection" width="55" />
                <el-table-column prop="id" label="患者ID" width="120" sortable />
                <el-table-column prop="name" label="姓名" width="120" sortable />
                <el-table-column prop="gender" label="性别" width="80">
                    <template #default="scope">
                        <el-tag>{{ scope.row.gender === 'male' ? '男' : '女' }}</el-tag>
                    </template>
                </el-table-column>
                <el-table-column prop="age" label="年龄" width="80" sortable />
                <el-table-column prop="phone" label="电话" width="150" />
                <el-table-column prop="address" label="地址" width="200" />
                <el-table-column prop="lastVisit" label="上次就诊" width="130" sortable />
                <el-table-column prop="status" label="状态" width="120">
                    <template #default="scope">
                        <el-tag :type="getStatusColor(scope.row.status)">
                            {{ getStatusText(scope.row.status) }}
                        </el-tag>
                    </template>
                </el-table-column>
                <el-table-column prop="doctor" label="主治医师" width="120" />
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
                    :total="filteredPatients.length"
                />
            </div>
        </div>

        <!-- 新增/编辑患者对话框 -->
        <el-dialog
            v-model="dialogVisible"
            :title="isEdit ? '编辑患者' : '新增患者'"
            width="500px"
        >
            <el-form :model="formData" label-width="80px">
                <el-form-item label="姓名">
                    <el-input v-model="formData.name" placeholder="请输入患者姓名" />
                </el-form-item>
                <el-form-item label="性别">
                    <el-select v-model="formData.gender" placeholder="选择性别">
                        <el-option label="男" value="male" />
                        <el-option label="女" value="female" />
                    </el-select>
                </el-form-item>
                <el-form-item label="年龄">
                    <el-input-number v-model="formData.age" :min="1" :max="120" placeholder="请输入年龄" />
                </el-form-item>
                <el-form-item label="电话">
                    <el-input v-model="formData.phone" placeholder="请输入电话号码" />
                </el-form-item>
                <el-form-item label="地址">
                    <el-input v-model="formData.address" type="textarea" rows="2" placeholder="请输入地址" />
                </el-form-item>
                <el-form-item label="主治医师">
                    <el-select v-model="formData.doctor" placeholder="选择主治医师">
                        <el-option v-for="doc in doctors" :key="doc" :label="doc" :value="doc" />
                    </el-select>
                </el-form-item>
                <el-form-item label="就诊状态">
                    <el-select v-model="formData.status" placeholder="选择就诊状态">
                        <el-option label="已完成" value="completed" />
                        <el-option label="待就诊" value="pending" />
                        <el-option label="已取消" value="cancelled" />
                    </el-select>
                </el-form-item>
                <el-form-item label="上次就诊">
                    <el-date-picker
                        v-model="formData.lastVisit"
                        type="date"
                        placeholder="选择上次就诊日期"
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
    name: 'Patients',
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
            filterGender: '',
            filterStatus: '',
            // 医生列表（用于下拉选择）
            doctors: ['张医生', '李医生', '王医生', '赵医生', '孙医生'],
            // 患者列表
            patients: [
                { id: 'P001', name: '张三', gender: 'male', age: 25, phone: '13900139001', address: '北京市朝阳区', lastVisit: '2025-01-15', status: 'completed', doctor: '张医生' },
                { id: 'P002', name: '李四', gender: 'female', age: 32, phone: '13900139002', address: '上海市浦东新区', lastVisit: '2025-01-20', status: 'pending', doctor: '李医生' },
                { id: 'P003', name: '王五', gender: 'male', age: 45, phone: '13900139003', address: '广州市天河区', lastVisit: '2025-01-18', status: 'completed', doctor: '王医生' },
                { id: 'P004', name: '赵六', gender: 'female', age: 28, phone: '13900139004', address: '深圳市南山区', lastVisit: '2025-01-22', status: 'cancelled', doctor: '赵医生' },
                { id: 'P005', name: '孙七', gender: 'male', age: 55, phone: '13900139005', address: '杭州市西湖区', lastVisit: '2025-01-19', status: 'pending', doctor: '孙医生' }
            ],
            // 分页
            currentPage: 1,
            pageSize: 10,
            // 选中项
            selectedPatients: [],
            // 对话框
            dialogVisible: false,
            isEdit: false,
            formData: {
                id: '',
                name: '',
                gender: 'male',
                age: null,
                phone: '',
                address: '',
                doctor: '',
                status: 'pending',
                lastVisit: ''
            }
        }
    },
    computed: {
        // 过滤后的患者列表
        filteredPatients() {
            let result = [...this.patients]
            
            // 关键词搜索
            if (this.searchKeyword) {
                const keyword = this.searchKeyword.toLowerCase()
                result = result.filter(item => 
                    item.name.toLowerCase().includes(keyword) || 
                    item.id.toLowerCase().includes(keyword) ||
                    item.phone.includes(keyword)
                )
            }
            
            // 性别筛选
            if (this.filterGender) {
                result = result.filter(item => item.gender === this.filterGender)
            }
            
            // 状态筛选
            if (this.filterStatus) {
                result = result.filter(item => item.status === this.filterStatus)
            }
            
            return result
        },
        // 分页后的患者列表
        paginatedPatients() {
            const startIndex = (this.currentPage - 1) * this.pageSize
            const endIndex = startIndex + this.pageSize
            return this.filteredPatients.slice(startIndex, endIndex)
        },
        // 统计数据
        todayAppointments() {
            // 模拟今日预约数
            return Math.floor(Math.random() * 15) + 5
        },
        pendingPatients() {
            return this.patients.filter(patient => patient.status === 'pending').length
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
            this.selectedPatients = val
        },
        
        // 对话框操作
        showAddDialog() {
            this.isEdit = false
            this.formData = {
                id: '',
                name: '',
                gender: 'male',
                age: null,
                phone: '',
                address: '',
                doctor: '',
                status: 'pending',
                lastVisit: ''
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
                const index = this.patients.findIndex(p => p.id === this.formData.id)
                if (index !== -1) {
                    this.patients.splice(index, 1, {...this.formData})
                    this.$message.success('患者信息更新成功')
                }
            } else {
                // 添加模式
                const newId = 'P' + String(this.patients.length + 1).padStart(3, '0')
                const newPatient = {
                    ...this.formData,
                    id: newId
                }
                this.patients.unshift(newPatient)
                this.$message.success('患者添加成功')
            }
            this.dialogVisible = false
        },
        
        // 删除操作
        handleDelete(id) {
            this.$confirm('确定要删除这位患者吗？', '警告', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                const index = this.patients.findIndex(p => p.id === id)
                if (index !== -1) {
                    this.patients.splice(index, 1)
                    this.$message.success('删除成功')
                }
            }).catch(() => {
                // 取消删除
            })
        },
        
        handleBatchDelete() {
            if (this.selectedPatients.length === 0) {
                this.$message.warning('请选择要删除的患者')
                return
            }
            
            this.$confirm(`确定要删除选中的 ${this.selectedPatients.length} 位患者吗？`, '警告', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                const idsToDelete = this.selectedPatients.map(p => p.id)
                this.patients = this.patients.filter(p => !idsToDelete.includes(p.id))
                this.selectedPatients = []
                this.$message.success('批量删除成功')
            }).catch(() => {
                // 取消删除
            })
        },
        
        // 辅助方法
        getStatusText(status) {
            const statusMap = {
                'completed': '已完成',
                'pending': '待就诊',
                'cancelled': '已取消'
            }
            return statusMap[status] || status
        },
        
        getStatusColor(status) {
            const colorMap = {
                'completed': 'success',
                'pending': 'warning',
                'cancelled': 'info'
            }
            return colorMap[status] || 'info'
        }
    }
}
</script>

<style scoped>
    .patients {
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
    .patient-card .stat-icon {
        background: #4285f4;
    }

    .appointment-card .stat-icon {
        background: #34a853;
    }

    .pending-card .stat-icon {
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

    .status-text {
        color: #666;
        font-size: 0.85rem;
    }

    /* 患者列表区域 */
    .patient-table-section {
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

    .section-title {
        font-size: 1.2rem;
        margin: 0;
        color: var(--text-color);
    }

    .table-actions {
        display: flex;
        gap: 0.5rem;
    }

    /* 分页 */
    .pagination {
        display: flex;
        justify-content: flex-end;
        margin-top: 1.5rem;
    }

    /* 响应式设计 */
    @media (max-width: 768px) {
        .header-content {
            flex-direction: column;
            align-items: flex-start;
        }

        .search-filter-section {
            flex-direction: column;
            align-items: stretch;
        }

        .stats-grid {
            grid-template-columns: 1fr;
        }

        .table-header {
            flex-direction: column;
            align-items: stretch;
        }

        .table-actions {
            justify-content: flex-end;
        }

        .pagination {
            justify-content: center;
        }
    }
</style>