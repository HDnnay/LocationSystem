<template>
    <div class="dental-offices">
        <!-- 页面头部区域 -->
        <div class="page-header gradient-bg">
            <div class="header-content">
                <div class="page-info">
                    <h2 class="page-title">牙科诊所管理</h2>
                <p class="page-description">管理牙科诊所信息</p>
                </div>
                <div class="header-actions">
                    <el-button type="primary" @click="showAddDialog">
                        <el-icon><Plus /></el-icon> 新增牙科诊所
                    </el-button>
                </div>
            </div>
        </div>

        <!-- 统计卡片区域 -->
        <div class="stats-grid">
            <!-- 诊所总数卡片 -->
            <div class="stat-card clinic-card">
                <div class="stat-icon clinic-icon">🏥</div>
                <div class="stat-content">
                    <h3 class="stat-number">{{ clinics.length }}</h3>
                    <p class="stat-label">诊所总数</p>
                </div>
                <div class="stat-status">
                    <span class="status-text">{{ clinics.length }}个牙科诊所</span>
                </div>
            </div>

            <!-- 总医生数卡片 -->
            <div class="stat-card doctor-card">
                <div class="stat-icon doctor-icon">👨‍⚕️</div>
                <div class="stat-content">
                    <h3 class="stat-number">{{ totalDoctors }}</h3>
                    <p class="stat-label">医生总数</p>
                </div>
                <div class="stat-status">
                    <span class="status-text">{{ totalDoctors }}位医生</span>
                </div>
            </div>

            <!-- 总床位卡片 -->
            <div class="stat-card bed-card">
                <div class="stat-icon bed-icon">🛏️</div>
                <div class="stat-content">
                    <h3 class="stat-number">{{ totalBeds }}</h3>
                    <p class="stat-label">床位总数</p>
                </div>
                <div class="stat-status">
                    <span class="status-text">{{ totalBeds }}张床位</span>
                </div>
            </div>
        </div>

        <!-- 搜索和筛选区域 -->
        <div class="search-filter-section">
            <div class="search-box">
                <el-input
                    v-model="searchKeyword"
                    placeholder="搜索诊所名称或地址"
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
                    v-model="filterCity"
                    placeholder="选择城市"
                    @change="handleFilter"
                    style="width: 160px; margin-right: 10px;"
                >
                    <el-option label="所有城市" value="" />
                    <el-option v-for="city in cities" :key="city" :label="city" :value="city" />
                </el-select>
                <el-select
                    v-model="filterStatus"
                    placeholder="诊所状态"
                    @change="handleFilter"
                    style="width: 120px;"
                >
                    <el-option label="所有状态" value="" />
                    <el-option label="营业中" value="open" />
                    <el-option label="已关闭" value="closed" />
                </el-select>
            </div>
        </div>

        <!-- 诊所列表表格 -->
        <div class="clinic-table-section card">
            <div class="table-header">
                <h3 class="section-title">诊所列表</h3>
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
                :data="paginatedClinics"
                style="width: 100%"
                @selection-change="handleSelectionChange"
            >
                <el-table-column type="selection" width="55" />
                <el-table-column prop="id" label="诊所ID" width="120" sortable />
                <el-table-column prop="name" label="诊所名称" width="180" sortable />
                <el-table-column prop="city" label="城市" width="100" />
                <el-table-column prop="address" label="地址" width="250" />
                <el-table-column prop="phone" label="电话" width="150" />
                <el-table-column prop="email" label="邮箱" width="200" />
                <el-table-column prop="doctors" label="医生数" width="80" sortable />
                <el-table-column prop="beds" label="床位数" width="80" sortable />
                <el-table-column prop="status" label="状态" width="100">
                    <template #default="scope">
                        <el-tag :type="scope.row.status === 'open' ? 'success' : 'warning'">
                            {{ scope.row.status === 'open' ? '营业中' : '已关闭' }}
                        </el-tag>
                    </template>
                </el-table-column>
                <el-table-column prop="openDate" label="开业日期" width="130" sortable />
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
                    :total="filteredClinics.length"
                />
            </div>
        </div>

        <!-- 新增牙科诊所对话框 -->
        <el-dialog
            v-model="dialogVisible"
            :title="isEdit ? '编辑牙科诊所' : '新增牙科诊所'"
            width="600px"
        >
            <el-form :model="formData" label-width="100px">
                <el-form-item label="诊所名称">
                    <el-input v-model="formData.name" placeholder="请输入诊所名称" />
                </el-form-item>
                <el-form-item label="城市">
                    <el-select v-model="formData.city" placeholder="请选择城市">
                        <el-option v-for="city in cities" :key="city" :label="city" :value="city" />
                    </el-select>
                </el-form-item>
                <el-form-item label="地址">
                    <el-input v-model="formData.address" type="textarea" rows="3" placeholder="请输入详细地址" />
                </el-form-item>
                <el-form-item label="电话">
                    <el-input v-model="formData.phone" placeholder="请输入电话号码" />
                </el-form-item>
                <el-form-item label="邮箱">
                    <el-input v-model="formData.email" placeholder="请输入邮箱地址" />
                </el-form-item>
                <el-form-item label="医生数量">
                    <el-input-number v-model="formData.doctors" :min="0" :step="1" placeholder="请输入医生数量" />
                </el-form-item>
                <el-form-item label="床位数量">
                    <el-input-number v-model="formData.beds" :min="0" :step="1" placeholder="请输入床位数量" />
                </el-form-item>
                <el-form-item label="诊所状态">
                    <el-select v-model="formData.status" placeholder="请选择状态">
                        <el-option label="营业中" value="open" />
                        <el-option label="已关闭" value="closed" />
                    </el-select>
                </el-form-item>
                <el-form-item label="开业日期">
                    <el-date-picker
                        v-model="formData.openDate"
                        type="date"
                        placeholder="请选择开业日期"
                        style="width: 100%"
                    />
                </el-form-item>
                <el-form-item label="诊所描述">
                    <el-input v-model="formData.description" type="textarea" rows="4" placeholder="请输入诊所描述" />
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
    name: 'DentalOffices',
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
            filterCity: '',
            filterStatus: '',
            // 城市列表
            cities: ['北京', '上海', '广州', '深圳', '杭州', '成都', '武汉', '西安', '南京', '重庆'],
            // 牙科诊所列表
            clinics: [
                { 
                    id: 'C001', 
                    name: '牙科诊所A', 
                    city: '北京', 
                    address: '北京市朝阳区建国路88号', 
                    phone: '010-88888888', 
                    email: 'clinic-a@example.com',
                    doctors: 15,
                    beds: 30,
                    status: 'open',
                    openDate: '2018-05-15',
                    description: '专业牙科诊所，提供全面的口腔医疗服务'
                },
                { 
                    id: 'C002', 
                    name: '牙科诊所B', 
                    city: '上海', 
                    address: '上海市浦东新区陆家嘴金融中心', 
                    phone: '021-99999999', 
                    email: 'clinic-b@example.com',
                    doctors: 20,
                    beds: 40,
                    status: 'open',
                    openDate: '2019-08-20',
                    description: '高端牙科诊所，配备国际先进设备'
                },
                { 
                    id: 'C003', 
                    name: '牙科诊所C', 
                    city: '广州', 
                    address: '广州市天河区天河路385号', 
                    phone: '020-77777777', 
                    email: 'clinic-c@example.com',
                    doctors: 12,
                    beds: 25,
                    status: 'closed',
                    openDate: '2020-03-10',
                    description: '社区牙科诊所，服务附近居民'
                }
            ],
            // 分页
            currentPage: 1,
            pageSize: 10,
            // 选中项
            selectedOffices: [],
            // 对话框
            dialogVisible: false,
            isEdit: false,
            formData: {
                id: '',
                name: '',
                city: '',
                address: '',
                phone: '',
                email: '',
                doctors: 0,
                beds: 0,
                status: 'open',
                openDate: '',
                description: ''
            }
        }
    },
    computed: {
        // 过滤后的诊所列表
        filteredClinics() {
            let result = [...this.clinics]
            
            // 关键词搜索
            if (this.searchKeyword) {
                const keyword = this.searchKeyword.toLowerCase()
                result = result.filter(item => 
                    item.name.toLowerCase().includes(keyword) || 
                    item.address.toLowerCase().includes(keyword) ||
                    item.id.toLowerCase().includes(keyword)
                )
            }
            
            // 城市筛选
            if (this.filterCity) {
                result = result.filter(item => item.city === this.filterCity)
            }
            
            // 状态筛选
            if (this.filterStatus) {
                result = result.filter(item => item.status === this.filterStatus)
            }
            
            return result
        },
        // 统计数据
        totalDoctors() {
            return this.clinics.reduce((total, clinic) => total + clinic.doctors, 0)
        },
        totalBeds() {
            return this.clinics.reduce((total, clinic) => total + clinic.beds, 0)
        },
        // Paginated clinics
        paginatedClinics() {
            const startIndex = (this.currentPage - 1) * this.pageSize
            const endIndex = startIndex + this.pageSize
            return this.filteredClinics.slice(startIndex, endIndex)
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
            const weekdays = ['周日', '周一', '周二', '周三', '周四', '周五', '周六']
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
            this.selectedOffices = val
        },
        
        // 对话框操作
        showAddDialog() {
            this.isEdit = false
            this.formData = {
                id: '',
                name: '',
                city: '',
                address: '',
                phone: '',
                email: '',
                doctors: 0,
                beds: 0,
                status: 'open',
                openDate: '',
                description: ''
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
                const index = this.clinics.findIndex(c => c.id === this.formData.id)
                if (index !== -1) {
                    this.clinics.splice(index, 1, {...this.formData})
                    this.$message.success('牙科诊所信息更新成功')
                }
            } else {
                // 添加模式
                const newId = 'C' + String(this.clinics.length + 1).padStart(3, '0')
                const newClinic = {
                    ...this.formData,
                    id: newId
                }
                this.clinics.unshift(newClinic)
                this.$message.success('牙科诊所添加成功')
            }
            this.dialogVisible = false
        },
        
        // 删除操作
        handleDelete(id) {
            this.$confirm('确定要删除这个牙科诊所吗？', '删除确认', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                const index = this.clinics.findIndex(c => c.id === id)
                if (index !== -1) {
                    this.clinics.splice(index, 1)
                    this.$message.success('删除成功')
                }
            }).catch(() => {
                // 取消删除
            })
        },
        
        handleBatchDelete() {
            if (this.selectedOffices.length === 0) {
                this.$message.warning('请选择要删除的牙科诊所')
                return
            }
            
            this.$confirm(`确定要删除选中的${this.selectedOffices.length}个牙科诊所吗？`, '删除确认', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'warning'
            }).then(() => {
                const idsToDelete = this.selectedOffices.map(c => c.id)
                this.clinics = this.clinics.filter(c => !idsToDelete.includes(c.id))
                this.selectedOffices = []
                this.$message.success('批量删除成功')
            }).catch(() => {
                // 取消删除
            })
        }
    }
}
</script>

<style scoped>
    .dental-offices {
        padding: 1rem 0;
    }

    /* Gradient background */
    .gradient-bg {
        background: var(--primary-color);
        color: white;
        border-radius: 8px;
    }

    /* Page header */
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

    /* Search and filter section */
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

    /* Statistics cards */
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

    /* Different type card color themes */
    .clinic-card .stat-icon {
        background: #4285f4;
    }

    .doctor-card .stat-icon {
        background: #34a853;
    }

    .bed-card .stat-icon {
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

    /* Card status information */
    .stat-status {
        display: flex;
        flex-wrap: wrap;
        gap: 1rem;
    }

    .status-text {
        color: #666;
        font-size: 0.85rem;
    }

    /* Dental office list section */
    .clinic-table-section {
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

    /* Pagination */
    .pagination {
        display: flex;
        justify-content: flex-end;
        margin-top: 1.5rem;
    }

    /* Responsive design */
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