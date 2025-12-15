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
                        <el-button size="small" :type="scope.row.status ? 'warning' : 'success'" @click="toggleStatus(scope.row)">
                            {{ scope.row.status ? '禁用' : '启用' }}
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
    import request from '../utils/request.js'
    import { ElMessage } from 'element-plus'
    import {
        Plus, Edit, Delete, Lock, Unlock, Setting, Search, Check, Close, Refresh, User
    } from '@element-plus/icons-vue'
    import PermissionTree from '../components/PermissionTree.vue'
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
                selectedPermissions: [], // 默认选中的权限ID列表
                expandedPermissions: [], // 默认展开的权限ID列表
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
            // 使用Element Plus的el-table和el-pagination，无需手动过滤和分页
        },
        methods: {
            formatDate(dateString) {
                if (!dateString) return '';
                const date = new Date(dateString);
                return date.toLocaleString();
            },
            async getRoles() {
                this.loading = true;
                const pageRequest = {
                    pageIndex: this.currentPage,
                    pageSize: this.pageSize
                };
                try {
                    const response = await request.get('/api/role/GetRoles', { params: pageRequest })
                    if (response.status === 200) {
                        this.roles = response.data.result;
                        this.total = response.data.total;
                    }
                } catch (error) {
                    ElMessage.error('获取角色列表失败');
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
            formatDate(dateString) {
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
                    status: role.status
                }
                this.showRoleModal = true
            },
            closeRoleModal() {
                this.showRoleModal = false
                this.editingRole = null
                // 重置表单验证状态
                if (this.$refs.roleFormRef) {
                    this.$refs.roleFormRef.resetFields()
                }
            },
            async saveRole() {
                try {
                    if (this.editingRole) {
                        // 更新角色
                        var newRole = {
                            roleName: this.formData.roleName,
                            roleDescription: this.formData.roleDescription,
                            status: this.formData.status
                        }
                        await request.put("/api/role/" + this.editingRole.id, newRole);
                        ElMessage.success('角色更新成功');
                    } else {
                        // 创建新角色
                        const newRole = {
                            roleName: this.formData.roleName,
                            roleDescription: this.formData.roleDescription,
                            roleCode: this.formData.roleCode
                        }
                        await request.post("/api/role/create", newRole);
                        ElMessage.success('角色创建成功');
                    }
                    this.closeRoleModal();
                    this.getRoles();
                } catch (error) {
                    ElMessage.error('保存失败，请重试');
                }
            },
            async showPermissionModal(role) {
                console.log('🚀 打开权限模态框');
                this.selectedRole = role
                this.showPermission = true
                this.permissionLoading = true
                this.selectedPermissions = []
                this.expandedPermissions = []
                this.permissionError = null

                try {
                    // 从API接口获取实际权限数据
                    const response = await request.get('/api/Role/Permissions');
                    console.log('✅ API请求成功，响应数据:', response.data);

                    // 确保数据格式正确，并添加必要的displayName属性
                    this.permissionTree = response.data.map(permission => ({
                        ...permission,
                        displayName: permission.displayName || permission.name
                    }));
                    console.log(this.permissionTree);
                    this.expandedPermissions = this.permissionTree.map(p => p.id);

                    console.log(this.expandedPermissions)
                    // 初始化已选权限
                    if (role.permissions) {
                        this.selectedPermissions = this.extractSelectedPermissions(role.permissions)
                        console.log('✅ 已选权限初始化完成:', this.selectedPermissions);
                    }

                } catch (error) {
                    // 详细的错误处理
                    console.error('❌ API请求失败:', error);
                    if (error.response) {
                        // 服务器返回了错误状态码

                        this.permissionError = `服务器错误: ${error.response.status} - ${error.response.data.message || '未知错误'}`;
                    } else if (error.request) {
                        // 请求已发出，但没有收到响应
                        console.error('❌ 网络错误，无响应:', error.request);
                        this.permissionError = '网络错误，请检查网络连接';
                    } else {
                        // 其他错误
                        console.error('❌ 请求配置错误:', error.message);
                        this.permissionError = `请求错误: ${error.message}`;
                    }

                    this.expandedPermissions = this.permissionTree.map(p => p.id);
                } finally {
                    this.permissionLoading = false;
                    console.log('🔄 权限加载完成，加载状态:', this.permissionLoading);
                }
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
            // 判断某个权限组的所有子权限是否都被选中
            hasAllChildrenSelected(permission) {
                if (!permission.childPermissions || permission.childPermissions.length === 0) {
                    return this.selectedPermissions.includes(permission.id)
                }

                return permission.childPermissions.every(child =>
                    this.selectedPermissions.includes(child.id)
                )
            },

            // 切换权限组的所有子权限
            toggleAllChildren(permission) {
                const allSelected = this.hasAllChildrenSelected(permission)

                permission.childPermissions.forEach(child => {
                    if (allSelected) {
                        // 如果全部已选中，则全部取消选中
                        const index = this.selectedPermissions.indexOf(child.id)
                        if (index !== -1) {
                            this.selectedPermissions.splice(index, 1)
                        }
                    } else {
                        // 如果未全部选中，则全部选中
                        if (!this.selectedPermissions.includes(child.id)) {
                            this.selectedPermissions.push(child.id)
                        }
                    }
                })
            },

            togglePermission(permission) {
                const isSelected = this.selectedPermissions.includes(permission.id)

                if (isSelected) {
                    // 取消选中
                    this.selectedPermissions = this.selectedPermissions.filter(id => id !== permission.id)
                    // 递归取消选中所有子权限
                    if (permission.childPermissions) {
                        this.deselectChildren(permission)
                    }
                } else {
                    // 选中当前权限
                    this.selectedPermissions.push(permission.id)
                    // 如果有父级，检查是否需要递归选中父级
                    this.checkAndSelectParent(permission)
                }
            },
            deselectChildren(permission) {
                if (permission.childPermissions && permission.childPermissions.length > 0) {
                    permission.childPermissions.forEach(child => {
                        this.selectedPermissions = this.selectedPermissions.filter(id => id !== child.id)
                        this.deselectChildren(child)
                    })
                }
            },
            checkAndSelectParent(permission) {
                // 这里简化处理，实际可能需要根据后端数据结构调整
                // 主要是处理父子级联关系
            },
            extractSelectedPermissions(permissions) {
                // 将后端返回的权限对象转换为ID数组
                // 根据实际后端返回的数据结构调整
                if (Array.isArray(permissions)) {
                    return permissions.map(p => p.id)
                } else if (typeof permissions === 'object') {
                    // 兼容旧格式
                    return Object.keys(permissions).reduce((selected, key) => {
                        const perm = permissions[key]
                        if (typeof perm === 'object') {
                            Object.values(perm).forEach(value => {
                                if (value === true) {
                                    // 这里简化处理，实际需要根据旧格式与新ID的映射关系调整
                                    // 暂时返回空数组
                                }
                            })
                        }
                        return selected
                    }, [])
                }
                return []
            },
            async savePermissions() {
                if (this.selectedRole) {
                    try {
                        var role = {
                            roleId: this.selectedRole.id,
                            permissions: this.selectedPermissions
                        }
                        const self = this;
                        await request.post("/api/role/RolePsermission", role).then(res => {
                            if (res.status == 200) {
                                ElMessage.success('权限保存成功');
                                self.getRoles(self.currentPage);
                            }
                        })
                    } catch (error) {
                        ElMessage.error('权限保存失败');
                    }
                }
                this.closePermissionModal();
            },
            // 提供模拟的权限树数据
            getMockPermissionTree() {

            },
            async toggleStatus(role) {
                if (role == null)
                    return;
                var roleStatus = {
                    id: role.id,
                    status: !role.status
                };
                const self = this;
                await request.put("/api/role/Status", roleStatus).then(res => {
                    if (res.status === 200)
                        self.getRoles(self.currentPage)
                });

            },
            confirmDelete(role) {
                this.deleteRole = role
                this.showDeleteConfirm = true
            },
            async deleteRoleConfirmed() {
                if (this.deleteRole) {
                    try {
                        await request.delete("/api/role/" + this.deleteRole.id);
                        ElMessage.success('角色删除成功');
                        this.deleteRole = null;
                        this.showDeleteConfirm = false;
                        this.getRoles();
                    } catch (error) {
                        ElMessage.error('删除失败，请重试');
                    }
                }
            },
            handleTogglePermission(permission) {
                // 兼容旧事件的处理函数（已不再使用）
                console.log('🔄 切换权限选择:', permission.id, permission.displayName || permission.name);
            },
            handleToggleExpand(permissionId) {
                // 兼容旧事件的处理函数（已不再使用）
                console.log('🔄 切换权限展开状态:', permissionId);
                const index = this.expandedPermissions.indexOf(permissionId);

                if (index > -1) {
                    // 如果已展开，则收起
                    this.expandedPermissions.splice(index, 1);
                } else {
                    // 如果未展开，则展开
                    this.expandedPermissions.push(permissionId);
                }

                console.log('📋 当前展开的权限:', this.expandedPermissions);
            }
        },
        watch: {
            // 当搜索条件改变时，重置到第一页
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