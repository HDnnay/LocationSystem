<template>
    <div class="roles-container">
        <el-card class="page-card">
            <template #header>
                <div class="page-header">
                    <h1>角色管理</h1>
                    <el-button type="primary" @click="showAddRoleModal">
                        <el-icon><Plus /></el-icon> 添加角色
                    </el-button>
                </div>
            </template>

            <!-- 搜索区域 -->
            <el-row :gutter="20" class="mb-4">
                <el-col :span="12">
                    <el-input v-model="searchQuery"
                              placeholder="搜索角色名称或标识..."
                              clearable
                              @keyup.enter.native="getRoles(1)">
                        <template #append>
                            <el-button icon="Search"
                                       @click="getRoles(1)">
                                搜索
                            </el-button>
                        </template>
                    </el-input>
                </el-col>
            </el-row>

            <!-- 角色列表表格 -->
            <el-table v-loading="loading"
                      :data="roles"
                      style="width: 100%"
                      stripe
                      border>
                <el-table-column type="index" :index="(index) => (currentPage - 1) * pageSize + index + 1" label="序号" width="80" />
                <el-table-column prop="roleName" label="角色名称" width="180" />
                <el-table-column prop="roleDescription" label="角色描述" min-width="200" />
                <el-table-column prop="createDate" label="创建时间" width="180">
                    <template #default="scope">
                        {{ formatDate(scope.row.createDate) }}
                    </template>
                </el-table-column>
                <el-table-column prop="status" label="状态" width="100">
                    <template #default="scope">
                        <el-tag :type="scope.row.status ? 'success' : 'info'">
                            {{ scope.row.status ? '启用' : '禁用' }}
                        </el-tag>
                    </template>
                </el-table-column>
                <el-table-column label="操作" min-width="200" fixed="right">
                    <template #default="scope">
                        <el-button size="small" type="info" @click="showPermissionModal(scope.row)">
                            权限管理
                        </el-button>
                        <el-button size="small" type="primary" @click="editRole(scope.row)">
                            编辑
                        </el-button>
                        <el-button size="small" :type="!scope.row.isDisabled ? 'warning' : 'success'" @click="toggleStatus(scope.row)">
                            {{ !scope.row.isDisabled ? '禁用' : '启用' }}
                        </el-button>
                        <el-button size="small" type="danger" @click="confirmDelete(scope.row)">
                            删除
                        </el-button>
                    </template>
                </el-table-column>
            </el-table>

            <!-- 分页组件 -->
            <div class="pagination-container">
                <el-pagination v-model:current-page="currentPage"
                               v-model:page-size="pageSize"
                               :page-sizes="[10, 20, 50, 100]"
                               layout="total, sizes, prev, pager, next, jumper"
                               :total="total"
                               @size-change="handleSizeChange"
                               @current-change="handleCurrentChange"
                               :disabled="loading"></el-pagination>
            </div>
        </el-card>

        <!-- 添加/编辑角色模态框 -->
        <el-dialog v-model="showRoleModal"
                   :title="editingRole ? '编辑角色' : '添加角色'"
                   width="500px">
            <el-form :model="formData" ref="roleFormRef" label-width="80px" @submit.prevent="saveRole">
                <el-form-item label="角色名称" prop="roleName" required>
                    <el-input v-model="formData.roleName" placeholder="请输入角色名称" />
                </el-form-item>
                <el-form-item label="角色描述" prop="roleDescription" required>
                    <el-input v-model="formData.roleDescription" placeholder="请输入角色描述" />
                </el-form-item>
                <el-form-item label="角色编码" prop="roleCode" required>
                    <el-input v-model="formData.roleCode" placeholder="请输入角色编码" />
                </el-form-item>
                <el-form-item v-if="!isAddRole">
                    <el-checkbox v-model="formData.status">启用角色</el-checkbox>
                </el-form-item>
            </el-form>
            <template #footer>
                <div class="dialog-footer">
                    <el-button @click="closeRoleModal">取消</el-button>
                    <el-button type="primary" @click="saveRole">{{ editingRole ? '更新' : '创建' }}</el-button>
                </div>
            </template>
        </el-dialog>

        <!-- 权限管理模态框 -->
        <el-dialog v-model="showPermission"
                   :title="selectedRole && selectedRole.roleName ? `设置 '${selectedRole.roleName}' 的权限` : '权限管理'"
                   width="700px">
            <div class="permission-tree-container">
                <!-- 使用PermissionTree组件显示权限数据 -->
                <permission-tree :permission-tree="permissionTree"
                                 :selected-permissions="selectedPermissions"
                                 :expanded-permissions="expandedPermissions"
                                 :loading="permissionLoading"
                                 :error="permissionError"
                                 @permission-change="selectedPermissions = $event"
                                 @expand-change="expandedPermissions = $event" />

                <!-- 已选权限统计 -->
                <div class="selected-count">
                    已选择 {{ selectedPermissions.length }} 项权限
                </div>
            </div>
            <template #footer>
                <div class="dialog-footer">
                    <el-button @click="closePermissionModal">取消</el-button>
                    <el-button type="primary" @click="savePermissions">保存权限</el-button>
                </div>
            </template>
        </el-dialog>

        <!-- 删除确认模态框 -->
        <el-dialog v-model="showDeleteConfirm"
                   title="确认删除"
                   width="400px">
            <span>您确定要删除角色 "{{ deleteRole?.roleName }}" 吗？此操作不可撤销。</span>
            <template #footer>
                <div class="dialog-footer">
                    <el-button @click="showDeleteConfirm = false">取消</el-button>
                    <el-button type="danger" @click="deleteRoleConfirmed">删除</el-button>
                </div>
            </template>
        </el-dialog>
    </div>
</template>

<script>
    import { ElMessage } from 'element-plus'
    import {
        Plus, Edit, Delete, Lock, Unlock, Setting, Search, Check, Close, Refresh, User
    } from '@element-plus/icons-vue'
    import PermissionTree from '../../components/PermissionTree.vue'
    import * as api from '../../api/roles.js'
import * as permissionApi from '../../api/permissions.js'
    export default {
        name: 'Roles',
        components: {
            Plus, Edit, Delete, Lock, Unlock, Setting, Search, Check, Close, Refresh, User,
            PermissionTree
        },
        data() {
            return {
                roles: [],
                currentPage: 1,
                pageSize: 10,
                total: 0,
                loading: false,
                searchQuery: '',
                showRoleModal: false,
                editingRole: null,
                isAddRole: false,
                formData: {
                    roleName: '',
                    roleDescription: '',
                    roleCode: '',
                    status: true
                },
                showPermission: false,
                selectedRole: null,
                permissionData: {},
                permissionTree: [],
                permissionLoading: false,
                permissionError: null,
                selectedPermissions: [],
                expandedPermissions: [],
                openPermissionGroups: [],
                showDeleteConfirm: false,
                deleteRole: null
            }
        },
        mounted() {
            this.getRoles(this.currentPage)
            this.permissionTree = this.getMockPermissionTree()
        },
        computed: {
        },
        methods: {
            formatDate(dateString) {
                if (!dateString) return '';
                const date = new Date(dateString);
                return date.toLocaleString();
            },
            async getRoles() {
                this.loading = true;
                try {
                    const response = await api.getAllRoles();
                  if (response.status === 200) {
                    const rolesData = response.data.items || response.data;
                        this.roles = (rolesData || []).map(role => ({
                            id: role.id,
                            roleName: role.name,
                            roleDescription: role.description,
                            roleCode: role.code,
                            status: !role.isDisabled,
                            isDisabled: role.isDisabled,
                            createDate: role.createdAt
                        }));
                        this.total = this.roles.length;
                    } else {
                        console.error(`获取角色列表失败，状态码: ${response.status}`);
                        ElMessage.error('获取角色列表失败');
                        this.roles = [];
                        this.total = 0;
                    }
                } finally {
                    this.loading = false;
                }
            },
            handleSizeChange(val) {
                this.pageSize = val;
                this.currentPage = 1;
                this.getRoles();
            },
            handleCurrentChange(val) {
                this.currentPage = val;
                this.getRoles();
            },
            formatDate2(dateString) {
                const date = new Date(dateString)
                return date.toLocaleDateString('zh-CN', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                    hour: '2-digit',
                    minute: '2-digit'
                })
            },
            showAddRoleModal() {
                this.editingRole = null;
                this.isAddRole = true
                this.formData = {
                    roleName: "",
                    roleDescription: "",
                    roleCode: "",
                    status: false

                }
                this.showRoleModal = true
            },
            editRole(role) {
                this.editingRole = role
                this.isAddRole = false
                this.formData = {
                    roleName: role.roleName,
                    roleDescription: role.roleDescription,
                    roleCode: role.roleCode,
                    status: role.status
                }
                this.showRoleModal = true
            },
            closeRoleModal() {
                this.showRoleModal = false
                this.editingRole = null
                if (this.$refs.roleFormRef) {
                    this.$refs.roleFormRef.resetFields()
                }
            },
            async saveRole() {
                try {
                    if (this.editingRole) {
                        var updateRoleDto = {
                            name: this.formData.roleName,
                            code: this.formData.roleCode || this.editingRole.roleCode,
                            description: this.formData.roleDescription,
                            permissionIds: []
                        }
                        const updateResponse = await api.updateRole(this.editingRole.id, updateRoleDto);
                        if (updateResponse.status === 200) {
                            ElMessage.success('角色更新成功');
                        } else {
                            console.error(`角色更新失败，状态码: ${updateResponse.status}`);
                            ElMessage.error('角色更新失败');
                        }
                    } else {
                        const createRoleDto = {
                            name: this.formData.roleName,
                            code: this.formData.roleCode,
                            description: this.formData.roleDescription,
                            permissionIds: []
                        }
                        const createResponse = await api.createRole(createRoleDto);
                        if (createResponse.status === 200) {
                            ElMessage.success('角色创建成功');
                        } else {
                            console.error(`角色创建失败，状态码: ${createResponse.status}`);
                            ElMessage.error('角色创建失败');
                        }
                    }
                    this.closeRoleModal();
                    this.getRoles();
                } catch (error) {
                    ElMessage.error('保存失败，请重试');
                }
            },
            async showPermissionModal(role) {
                this.selectedRole = role
                this.showPermission = true
                this.permissionLoading = true
                this.selectedPermissions = []
                this.expandedPermissions = []
                this.permissionError = null

                try {
                    const permissionsResponse = await permissionApi.getPermissionTreeWithCheckStatus(role.id);

                    if (permissionsResponse && Array.isArray(permissionsResponse)) {
                        this.permissionTree = permissionsResponse;
                        this.selectedPermissions = this.extractSelectedPermissionsFromTree(permissionsResponse);
                    } else if (permissionsResponse && Array.isArray(permissionsResponse.data)) {
                        this.permissionTree = permissionsResponse.data;
                        this.selectedPermissions = this.extractSelectedPermissionsFromTree(permissionsResponse.data);
                    } else if (permissionsResponse && Array.isArray(permissionsResponse.items)) {
                        this.permissionTree = permissionsResponse.items;
                        this.selectedPermissions = this.extractSelectedPermissionsFromTree(permissionsResponse.items);
                    } else {
                        this.permissionTree = [];
                        this.permissionError = '权限数据格式错误';
                    }

                    if (Array.isArray(this.permissionTree)) {
                        this.expandedPermissions = this.permissionTree.map(p => p.id);
                    } else {
                        this.expandedPermissions = [];
                    }

                } catch (error) {
                    if (error.response) {
                        this.permissionError = `服务器错误: ${error.response.status} - ${error.response.data.message || '未知错误'}`;
                    } else if (error.request) {
                        this.permissionError = '网络错误，请检查网络连接';
                    } else {
                        this.permissionError = `请求错误: ${error.message}`;
                    }

                    if (this.permissionTree.length > 0) {
                        this.expandedPermissions = this.permissionTree.map(p => p.id);
                    }
                } finally {
                    this.permissionLoading = false;
                }
            },

            extractSelectedPermissionsFromTree(permissions) {
                const selected = [];

                function traverse(tree) {
                    tree.forEach(permission => {
                        if (permission.isCheck) {
                            selected.push(permission.id);
                        }
                        if (permission.childPermissions && permission.childPermissions.length > 0) {
                            traverse(permission.childPermissions);
                        }
                    });
                }

                traverse(permissions);
                return selected;
            },
            closePermissionModal() {
                this.showPermission = false
                this.selectedRole = null
                this.permissionTree = []
                this.selectedPermissions = []
                this.expandedPermissions = []
            },
            toggleExpand(permissionId) {
                const index = this.expandedPermissions.indexOf(permissionId)
                if (index !== -1) {
                    this.expandedPermissions.splice(index, 1)
                } else {
                    this.expandedPermissions.push(permissionId)
                }
            },
            hasAllChildrenSelected(permission) {
                if (!permission.childPermissions || permission.childPermissions.length === 0) {
                    return this.selectedPermissions.includes(permission.id)
                }

                return permission.childPermissions.every(child =>
                    this.selectedPermissions.includes(child.id)
                )
            },

            toggleAllChildren(permission) {
                const allSelected = this.hasAllChildrenSelected(permission)

                permission.childPermissions.forEach(child => {
                    if (allSelected) {
                        const index = this.selectedPermissions.indexOf(child.id)
                        if (index !== -1) {
                            this.selectedPermissions.splice(index, 1)
                        }
                    } else {
                        if (!this.selectedPermissions.includes(child.id)) {
                            this.selectedPermissions.push(child.id)
                        }
                    }
                })
            },

            togglePermission(permission) {
                const isSelected = this.selectedPermissions.includes(permission.id)

                if (isSelected) {
                    this.selectedPermissions = this.selectedPermissions.filter(id => id !== permission.id)
                    if (permission.childPermissions) {
                        this.deselectChildren(permission)
                    }
                } else {
                    this.selectedPermissions.push(permission.id)
                    if (permission.childPermissions) {
                        this.selectChildren(permission)
                    }
                }
            },

            selectChildren(permission) {
                if (permission.childPermissions) {
                    permission.childPermissions.forEach(child => {
                        if (!this.selectedPermissions.includes(child.id)) {
                            this.selectedPermissions.push(child.id)
                        }
                        if (child.childPermissions) {
                            this.selectChildren(child)
                        }
                    })
                }
            },

            deselectChildren(permission) {
                if (permission.childPermissions) {
                    permission.childPermissions.forEach(child => {
                        const index = this.selectedPermissions.indexOf(child.id)
                        if (index !== -1) {
                            this.selectedPermissions.splice(index, 1)
                        }
                        if (child.childPermissions) {
                            this.deselectChildren(child)
                        }
                    })
                }
            },

            getMockPermissionTree() {
                return [
                    {
                        id: 1,
                        name: '系统管理',
                        code: 'System',
                        childPermissions: [
                            { id: 11, name: '用户管理', code: 'UserManagement', childPermissions: [] },
                            { id: 12, name: '角色管理', code: 'RoleManagement', childPermissions: [] },
                            { id: 13, name: '菜单管理', code: 'MenuManagement', childPermissions: [] },
                            { id: 14, name: '权限管理', code: 'PermissionManagement', childPermissions: [] }
                        ]
                    },
                    {
                        id: 2,
                        name: '业务管理',
                        code: 'Business',
                        childPermissions: [
                            { id: 21, name: '订单管理', code: 'OrderManagement', childPermissions: [] },
                            { id: 22, name: '客户管理', code: 'CustomerManagement', childPermissions: [] }
                        ]
                    }
                ]
            },
            async savePermissions() {
                try {
                    const response = await api.assignPermissionsToRole(this.selectedRole.id, this.selectedPermissions);
                    if (response.status === 200) {
                        ElMessage.success('权限分配成功');
                        console.log("分配权限返回数据：",response.data)
                        this.closePermissionModal();
                    } else {
                        ElMessage.error('权限分配失败');
                    }
                } catch (error) {
                    ElMessage.error('权限分配失败');
                }
            },
            async toggleStatus(role) {
                try {
                    const newStatus = !role.isDisabled
                    const response = await api.updateRole(role.id, {
                        name: role.roleName,
                        code: role.roleCode,
                        description: role.roleDescription,
                        isDisabled: newStatus,
                        permissionIds: []
                    })

                    if (response.status === 200) {
                        ElMessage.success(`角色${newStatus ? '启用' : '禁用'}成功`);
                        this.getRoles();
                    } else {
                        ElMessage.error('操作失败');
                    }
                } catch (error) {
                    ElMessage.error('操作失败');
                }
            },
            confirmDelete(role) {
                this.deleteRole = role
                this.showDeleteConfirm = true
            },
            async deleteRoleConfirmed() {
                try {
                    const response = await api.deleteRole(this.deleteRole.id);
                    if (response.status === 200) {
                        ElMessage.success('角色删除成功');
                        this.showDeleteConfirm = false;
                        this.deleteRole = null;
                        this.getRoles();
                    } else {
                        ElMessage.error('角色删除失败');
                    }
                } catch (error) {
                    ElMessage.error('角色删除失败');
                }
            },
            handleToggleExpand(permissionId) {
                console.log('🔄 切换权限展开状态:', permissionId);
                const index = this.expandedPermissions.indexOf(permissionId);

                if (index > -1) {
                    this.expandedPermissions.splice(index, 1);
                } else {
                    this.expandedPermissions.push(permissionId);
                }

                console.log('📋 当前展开的权限:', this.expandedPermissions);
            }
        },
        watch: {
            searchQuery() {
                this.currentPage = 1
            }
        }
    }
</script>

<style scoped>
    .roles-container {
        padding: 1rem;
    }

    .page-card {
        margin-bottom: 1rem;
    }

    .mb-4 {
        margin-bottom: 1.5rem;
    }

    .text-right {
        text-align: right;
    }

    .permission-tree-container {
        max-height: 400px;
        overflow-y: auto;
    }
</style>
