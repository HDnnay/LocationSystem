<template>
    <div class="permissions-container">
        <el-card class="page-card">
            <template #header>
                <div class="page-header">
                    <h1>权限管理</h1>
                    <el-button type="primary" @click="showAddPermissionModal">
                        <el-icon><Plus /></el-icon> 添加权限
                    </el-button>
                </div>
            </template>

            <!-- 搜索区域 -->
            <el-row :gutter="20" class="mb-4">
                <el-col :span="12">
                    <el-input v-model="searchQuery"
                              placeholder="搜索权限名称或代码..."
                              clearable
                              @keyup.enter.native="getPermissions">
                        <template #append>
                            <el-button icon="Search"
                                       @click="getPermissions">
                                搜索
                            </el-button>
                        </template>
                    </el-input>
                </el-col>
            </el-row>

            <!-- 权限列表表格 -->
            <el-table v-loading="loading"
                      :data="permissions"
                      style="width: 100%"
                      stripe
                      border>
                <el-table-column type="index" :index="(index) => index + 1" label="序号" width="80" />
                <el-table-column prop="name" label="权限名称" width="180" />
                <el-table-column prop="code" label="权限代码" width="180" />
                <el-table-column prop="description" label="权限描述" min-width="200" />
                <el-table-column prop="createdAt" label="创建时间" width="180">
                    <template #default="scope">
                        {{ formatDate(scope.row.createdAt) }}
                    </template>
                </el-table-column>
                <el-table-column prop="updatedAt" label="更新时间" width="180">
                    <template #default="scope">
                        {{ formatDate(scope.row.updatedAt) }}
                    </template>
                </el-table-column>
                <el-table-column label="操作" min-width="150" fixed="right">
                    <template #default="scope">
                        <el-button size="small" type="primary" @click="editPermission(scope.row)">
                            编辑
                        </el-button>
                        <el-button size="small" type="danger" @click="confirmDelete(scope.row)">
                            删除
                        </el-button>
                    </template>
                </el-table-column>
            </el-table>
        </el-card>

        <!-- 添加/编辑权限模态框 -->
        <el-dialog v-model="showPermissionModal"
                   :title="editingPermission ? '编辑权限' : '添加权限'"
                   width="500px">
            <el-form :model="formData" ref="permissionFormRef" label-width="80px">
                <el-form-item label="权限名称" prop="name" required>
                    <el-input v-model="formData.name" placeholder="请输入权限名称" />
                </el-form-item>
                <el-form-item label="权限代码" prop="code" required>
                    <el-input v-model="formData.code" placeholder="请输入权限代码" />
                </el-form-item>
                <el-form-item label="权限描述" prop="description">
                    <el-input v-model="formData.description" placeholder="请输入权限描述" type="textarea" />
                </el-form-item>
            </el-form>
            <template #footer>
                <div class="dialog-footer">
                    <el-button @click="closePermissionModal">取消</el-button>
                    <el-button type="primary" @click="savePermission">{{ editingPermission ? '更新' : '创建' }}</el-button>
                </div>
            </template>
        </el-dialog>

        <!-- 删除确认模态框 -->
        <el-dialog v-model="showDeleteConfirm"
                   title="确认删除"
                   width="400px">
            <span>您确定要删除权限 "{{ deletePermission?.name }}" 吗？此操作不可撤销。</span>
            <template #footer>
                <div class="dialog-footer">
                    <el-button @click="showDeleteConfirm = false">取消</el-button>
                    <el-button type="danger" @click="deletePermissionConfirmed">删除</el-button>
                </div>
            </template>
        </el-dialog>
    </div>
</template>

<script>
    import request from '../../utils/request.js'
    import { ElMessage } from 'element-plus'
    import { Plus, Edit, Delete, Search } from '@element-plus/icons-vue'

    export default {
        name: 'Permissions',
        components: {
            Plus, Edit, Delete, Search
        },
        data() {
            return {
                permissions: [],
                loading: false,
                searchQuery: '',
                showPermissionModal: false,
                editingPermission: null,
                formData: {
                    name: '',
                    code: '',
                    description: ''
                },
                showDeleteConfirm: false,
                deletePermission: null
            }
        },
        mounted() {
            this.getPermissions()
        },
        methods: {
            formatDate(dateString) {
                if (!dateString) return '';
                const date = new Date(dateString);
                return date.toLocaleString('zh-CN', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                    hour: '2-digit',
                    minute: '2-digit'
                });
            },
            async getPermissions() {
                this.loading = true;
                try {
                    const response = await request.get('/api/permissions');
                    if (response.status === 200) {
                        this.permissions = response.data;
                    }
                } catch (error) {
                    ElMessage.error('获取权限列表失败');
                } finally {
                    this.loading = false;
                }
            },
            showAddPermissionModal() {
                this.editingPermission = null;
                this.formData = {
                    name: "",
                    code: "",
                    description: ""
                }
                this.showPermissionModal = true
            },
            editPermission(permission) {
                this.editingPermission = permission
                this.formData = {
                    name: permission.name,
                    code: permission.code,
                    description: permission.description
                }
                this.showPermissionModal = true
            },
            closePermissionModal() {
                this.showPermissionModal = false
                this.editingPermission = null
                // 重置表单验证状态
                if (this.$refs.permissionFormRef) {
                    this.$refs.permissionFormRef.resetFields()
                }
            },
            async savePermission() {
                try {
                    if (this.editingPermission) {
                        // 更新权限
                        var updatePermissionDto = {
                            name: this.formData.name,
                            code: this.formData.code,
                            description: this.formData.description
                        }
                        await request.put(`/api/permissions/${this.editingPermission.id}`, updatePermissionDto);
                        ElMessage.success('权限更新成功');
                    } else {
                        // 创建新权限
                        const createPermissionDto = {
                            name: this.formData.name,
                            code: this.formData.code,
                            description: this.formData.description
                        }
                        await request.post('/api/permissions', createPermissionDto);
                        ElMessage.success('权限创建成功');
                    }
                    this.closePermissionModal();
                    this.getPermissions();
                } catch (error) {
                    ElMessage.error('保存失败，请重试');
                }
            },
            confirmDelete(permission) {
                this.deletePermission = permission
                this.showDeleteConfirm = true
            },
            async deletePermissionConfirmed() {
                if (this.deletePermission) {
                    try {
                        await request.delete(`/api/permissions/${this.deletePermission.id}`);
                        ElMessage.success('权限删除成功');
                        this.deletePermission = null;
                        this.showDeleteConfirm = false;
                        this.getPermissions();
                    } catch (error) {
                        ElMessage.error('删除失败，请重试');
                    }
                }
            }
        }
    }
</script>

<style scoped>
    .permissions-container {
        padding: 1rem;
    }

    .page-card {
        margin-bottom: 1rem;
    }

    .mb-4 {
        margin-bottom: 1.5rem;
    }

    .page-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .page-header h1 {
        margin: 0;
        font-size: 1.5rem;
    }

    .dialog-footer {
        display: flex;
        justify-content: flex-end;
    }
</style>