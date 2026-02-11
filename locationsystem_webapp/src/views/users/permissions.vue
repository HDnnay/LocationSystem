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

            <!-- 选项卡 -->
            <el-tabs v-model="activeTab" class="mb-4">
                <el-tab-pane label="权限列表" name="list">
                    <!-- 权限列表表格 -->
                    <el-table v-loading="loading"
                              :data="permissions"
                              style="width: 100%"
                              stripe
                              border>
                        <el-table-column type="index" :index="(index) => index + 1" label="序号" width="80" />
                        <el-table-column prop="name" label="权限名称" width="180" />
                        <el-table-column prop="code" label="权限代码" width="180" />
                        <el-table-column prop="parentName" label="父级权限" width="180" />
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
                </el-tab-pane>
                <el-tab-pane label="权限树形结构" name="tree">
                    <!-- 权限树形结构 -->
                    <el-card>
                        <template #header>
                            <div class="card-header">
                                <span>权限层级关系</span>
                                <el-button size="small" type="primary" @click="refreshPermissionTree">
                                    刷新
                                </el-button>
                            </div>
                        </template>
                        <PermissionTree :permissionTree="permissionTree"
                                      :loading="treeLoading"
                                      :error="treeError"
                                      @permission-change="handlePermissionChange"
                                      @expand-change="handleExpandChange" />
                    </el-card>
                </el-tab-pane>
            </el-tabs>
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
                <el-form-item label="父权限" prop="parentId">
                    <el-cascader
                        v-model="cascaderValue"
                        :options="permissionTreeOptions"
                        :props="cascaderProps"
                        placeholder="选择父权限（可选）"
                        clearable
                        style="width: 100%"
                        @change="handleCascaderChange"
                    />
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
    import PermissionTree from '../../components/PermissionTree.vue'

    export default {
        name: 'Permissions',
        components: {
            Plus, Edit, Delete, Search,
            PermissionTree
        },
        data() {
            return {
                activeTab: 'list',
                permissions: [],
                allPermissions: [],
                permissionTree: [],
                permissionTreeOptions: [],
                loading: false,
                treeLoading: false,
                treeError: null,
                searchQuery: '',
                showPermissionModal: false,
                editingPermission: null,
                formData: {
                    name: '',
                    code: '',
                    description: '',
                    parentId: null
                },
                cascaderValue: [],
                showDeleteConfirm: false,
                deletePermission: null,
                cascaderProps: {
                    children: 'children',
                    label: 'label',
                    value: 'id',
                    checkStrictly: true
                }
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
                        const permissions = response.data;
                        // 构建权限ID到名称的映射
                        const permissionMap = new Map();
                        permissions.forEach(permission => {
                            permissionMap.set(permission.id, permission.name);
                        });

                        // 为每个权限添加父级权限名称
                        this.permissions = permissions.map(permission => ({
                            ...permission,
                            parentName: permission.parentId ? permissionMap.get(permission.parentId) || '未知' : '无'
                        }));

                        this.allPermissions = permissions;
                        // 构建树形结构的权限选项
                        this.permissionTreeOptions = this.buildPermissionTreeOptions(permissions);
                    }
                } catch (error) {
                    ElMessage.error('获取权限列表失败');
                } finally {
                    this.loading = false;
                }
            },
            // 构建树形结构的权限选项
            buildPermissionTreeOptions(permissions) {
                const permissionMap = new Map();
                const rootPermissions = [];

                // 首先创建所有权限节点的映射
                permissions.forEach(permission => {
                    permissionMap.set(permission.id, {
                        id: permission.id,
                        label: permission.name,
                        code: permission.code,
                        disabled: false,
                        children: []
                    });
                });

                // 然后构建树形结构
                permissions.forEach(permission => {
                    const permissionNode = permissionMap.get(permission.id);
                    if (permission.parentId) {
                        // 如果有父权限，添加到父权限的子节点中
                        const parentNode = permissionMap.get(permission.parentId);
                        if (parentNode) {
                            parentNode.children.push(permissionNode);
                        } else {
                            // 如果父权限不存在，作为根节点处理
                            rootPermissions.push(permissionNode);
                        }
                    } else {
                        // 没有父权限的作为根节点
                        rootPermissions.push(permissionNode);
                    }
                });

                return rootPermissions;
            },
            async getPermissionTree() {
                this.treeLoading = true;
                this.treeError = null;
                try {
                    const response = await request.get('/api/permissions/tree');
                    if (response.status === 200) {
                        this.permissionTree = response.data;
                    }
                } catch (error) {
                    this.treeError = '获取权限树形结构失败';
                    ElMessage.error('获取权限树形结构失败');
                } finally {
                    this.treeLoading = false;
                }
            },
            showAddPermissionModal() {
                this.editingPermission = null;
                this.formData = {
                    name: "",
                    code: "",
                    description: "",
                    parentId: null
                }
                this.cascaderValue = [];
                this.showPermissionModal = true
            },
            editPermission(permission) {
                this.editingPermission = permission
                this.formData = {
                    name: permission.name,
                    code: permission.code,
                    description: permission.description,
                    parentId: permission.parentId || null
                }
                // 初始化级联选择器的值
                this.cascaderValue = permission.parentId ? [permission.parentId] : [];
                this.showPermissionModal = true
            },
            // 处理级联选择器变化
            handleCascaderChange(value) {
                if (value && value.length > 0) {
                    this.formData.parentId = value[value.length - 1];
                } else {
                    this.formData.parentId = null;
                }
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
                            description: this.formData.description,
                            parentId: this.formData.parentId
                        }
                        await request.put(`/api/permissions/${this.editingPermission.id}`, updatePermissionDto);
                        ElMessage.success('权限更新成功');
                    } else {
                        // 创建新权限
                        const createPermissionDto = {
                            name: this.formData.name,
                            code: this.formData.code,
                            description: this.formData.description,
                            parentId: this.formData.parentId
                        }
                        await request.post('/api/permissions', createPermissionDto);
                        ElMessage.success('权限创建成功');
                    }
                    this.closePermissionModal();
                    this.getPermissions();
                    if (this.activeTab === 'tree') {
                        this.getPermissionTree();
                    }
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
                        if (this.activeTab === 'tree') {
                            this.getPermissionTree();
                        }
                    } catch (error) {
                        ElMessage.error('删除失败，请重试');
                    }
                }
            },
            refreshPermissionTree() {
                this.getPermissionTree();
            },
            handlePermissionChange(checkedKeys) {
                // 处理权限选择变化
                console.log('Selected permissions:', checkedKeys);
            },
            handleExpandChange(expandedKeys) {
                // 处理节点展开/收起变化
                console.log('Expanded keys:', expandedKeys);
            }
        },
        watch: {
            activeTab(newTab) {
                if (newTab === 'tree') {
                    this.getPermissionTree();
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

    .card-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
</style>
