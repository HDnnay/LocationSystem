<template>
    <div class="role-checkbox-group">
        <div class="role-checkbox-title" v-if="title">{{ title }}</div>
        <el-checkbox-group v-model="selectedRoleValues" @change="handleChange">
            <el-checkbox v-for="role in roles"
                         :key="role.id || role.roleName"
                         :label="role.id || role.roleName"
                         :disabled="disabled || role.disabled">
                {{ role.roleName || role.name }}
            </el-checkbox>
        </el-checkbox-group>
        <!-- 错误提示 -->
        <div class="role-checkbox-error" v-if="error">{{ error }}</div>
    </div>
</template>

<script>
    export default {
        name: 'RoleCheckboxGroup',
        mounted() {
            console.log('RoleCheckboxGroup组件接收到的角色数据:', this.roles)
            console.log('RoleCheckboxGroup组件接收到的选中值:', this.modelValue)
        },
        props: {
            // 角色列表数据
            roles: {
                type: Array,
                default: () => []
            },
            // 默认选中的角色ID数组
            modelValue: {
                type: Array,
                default: () => []
            },
            // 组件标题
            title: {
                type: String,
                default: ''
            },
            // 是否禁用
            disabled: {
                type: Boolean,
                default: false
            },
            // 错误信息
            error: {
                type: String,
                default: ''
            }
        },
        data() {
            return {
                // 内部维护的选中值
                selectedRoleValues: this.normalizeRoleValues(this.modelValue)
            }
        },
        watch: {
            // 监听外部传入的modelValue变化，更新内部选中值
            modelValue: {
                handler(newVal) {
                    // 规范化新值为字符串数组
                    const normalizedValue = this.normalizeRoleValues(newVal)
                    // 只有当新值与内部值不同时才更新，避免递归更新
                    if (JSON.stringify(normalizedValue) !== JSON.stringify(this.selectedRoleValues)) {
                        this.selectedRoleValues = normalizedValue
                        // 当外部通过v-model更新时，也触发selected事件
                        this.handleChange()
                    }
                },
                deep: true,
                immediate: true
            }
        },
        methods: {
            // 处理选择变化
            handleChange() {
                const selectedRoles = this.getSelectedRoles()
                this.$emit('update:modelValue', [...this.selectedRoleValues])
                this.$emit('change', [...this.selectedRoleValues])
                this.$emit('selected', selectedRoles) // 新增选中事件，传递选中的角色对象数组
            },
            // 规范化角色值为字符串数组
            normalizeRoleValues(roles) {
                let normalized = []
                if (roles) {
                    if (Array.isArray(roles)) {
                        if (roles.length > 0 && typeof roles[0] === 'object') {
                            // 如果是对象数组，提取id或roleName
                            normalized = roles.map(role => role.id || role.roleName || role)
                        } else {
                            // 如果是字符串数组，直接使用
                            normalized = roles
                        }
                    } else if (typeof roles === 'string') {
                        // 如果是逗号分隔的字符串，转换为数组
                        normalized = roles.split(',').map(role => role.trim())
                    }
                }
                return normalized
            },
            // 获取当前选中的角色对象列表
            getSelectedRoles() {
                return this.roles.filter(role =>
                    this.selectedRoleValues.includes(role.id || role.roleName)
                )
            },
            // 清空选择
            clearSelection() {
                this.selectedRoleValues = []
            },
            // 全选/取消全选
            toggleSelectAll() {
                if (this.selectedRoleValues.length === this.roles.length) {
                    this.clearSelection()
                } else {
                    this.selectedRoleValues = this.roles.map(role => role.id || role.roleName)
                }
            }
        }
    }
</script>

<style scoped>
    .role-checkbox-group {
        width: 100%;
    }

    .role-checkbox-title {
        font-size: 14px;
        font-weight: 600;
        margin-bottom: 12px;
        color: #333;
    }

    .role-checkbox-error {
        font-size: 12px;
        color: #f56c6c;
        margin-top: 8px;
    }

    /* 自定义 Element Plus 复选框组的布局 */
    :deep(.el-checkbox-group) {
        display: flex;
        flex-direction: row;
        gap: 20px;
        flex-wrap: wrap;
    }

    :deep(.el-checkbox) {
        padding: 8px 12px;
        border-radius: 6px;
        transition: all 0.2s ease;
    }

    :deep(.el-checkbox:hover) {
        background-color: #f5f7fa;
    }
</style>