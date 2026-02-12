<template>
    <div class="permission-tree">
        <!-- 加载状态 -->
        <div v-if="loading" class="loading-state">
            <p>权限数据加载中...</p>
        </div>

        <!-- 错误状态 -->
        <div v-else-if="error" class="error-state">
            <p>数据加载失败: {{ error }}</p>
        </div>

        <!-- 权限树渲染 -->
        <div v-else-if="treeData && treeData.length > 0" class="permission-root">
            <el-tree ref="permissionTreeRef"
                     :data="treeData"
                     :props="treeProps"
                     :default-expanded-keys="allExpandedKeys.length > 0 ? allExpandedKeys : expandedPermissions"
                     :default-checked-keys="selectedPermissions"
                     node-key="id"
                     show-checkbox
                     @check-change="handleCheckChange"
                     @expand-change="handleExpandChange" />
        </div>

        <!-- 空状态 -->
        <div v-else class="empty-state">
            <p>暂无权限数据</p>
        </div>
    </div>
</template>

<script>
    // Element Plus的ElTree组件已经在全局注册，不需要单独导入

    export default {
        name: 'PermissionTree',
        components: {
            // Element Plus的ElTree组件不需要在这里注册
        },
        props: {
            permissionTree: {
                type: Array,
                default: () => []
            },
            selectedPermissions: {
                type: Array,
                default: () => []
            },
            expandedPermissions: {
                type: Array,
                default: () => []
            },
            loading: {
                type: Boolean,
                default: false
            },
            error: {
                type: String,
                default: null
            }
        },
        computed: {
            // 转换权限数据为ElTree所需格式
            treeData() {
                return this.transformToTreeData(this.permissionTree)
            },
            // 树配置项
            treeProps() {
                return {
                    children: 'children',
                    label: 'label',
                    value: 'id'
                }
            },
            // 获取所有需要展开的节点ID
            allExpandedKeys() {
                const keys = []
                const collectKeys = (data) => {
                    data.forEach(item => {
                        // 只收集有子节点的节点ID
                        if (item.children && item.children.length > 0) {
                            keys.push(item.id)
                            collectKeys(item.children)
                        }
                    })
                }
                collectKeys(this.treeData)
                return keys
            }
        },
        methods: {
            // 转换数据格式为ElTree兼容格式
            transformToTreeData(data) {
                if (!Array.isArray(data)) return []

                return data.map(item => {
                    // 深拷贝item对象
                    const node = JSON.parse(JSON.stringify(item))

                    // 设置label字段
                    node.label = item.displayName || item.name || '未知权限'

                    // 转换childPermissions为children并递归处理
                    if ((item.childPermissions || item.ChildPermissions) &&
                        ((Array.isArray(item.childPermissions) && item.childPermissions.length > 0) ||
                         (Array.isArray(item.ChildPermissions) && item.ChildPermissions.length > 0))) {
                        const childPermissions = item.childPermissions || item.ChildPermissions;
                        node.children = this.transformToTreeData(childPermissions)
                        // 删除原始childPermissions字段
                        if (item.childPermissions) {
                            delete node.childPermissions
                        }
                        if (item.ChildPermissions) {
                            delete node.ChildPermissions
                        }
                    } else {
                        node.children = []
                    }

                    return node
                })
            },

            // 自动展开所有节点
            autoExpandAll() {
                // 检查ref是否存在且有setExpandedKeys方法
                if (this.$refs.permissionTreeRef && typeof this.$refs.permissionTreeRef.setExpandedKeys === 'function') {
                    // 获取所有节点的ID
                    const getAllKeys = (data, keys = []) => {
                        data.forEach(item => {
                            keys.push(item.id)
                            if (item.children && item.children.length > 0) {
                                getAllKeys(item.children, keys)
                            }
                        })
                        return keys
                    }

                    const allKeys = getAllKeys(this.treeData)
                    // 调用Element Plus ElTree的setExpandedKeys方法
                    this.$refs.permissionTreeRef.setExpandedKeys(allKeys)
                    // 触发展开事件，更新父组件的展开状态
                    this.$emit('expand-change', allKeys)
                } else {
                    // 如果ref不存在或方法不可用，回退到使用default-expanded-keys
                    console.warn('PermissionTree ref or setExpandedKeys method not available')
                }
            },

            // 处理权限节点点击
            handleNodeClick(node) {
                // 点击节点时的处理
            },

            // 处理权限选择变化
            handleCheckChange(node, checked, indeterminate) {
                // 获取当前所有选中的节点
                const checkedNodes = this.$refs.permissionTreeRef.getCheckedNodes(false, true);
                // 提取选中节点的ID
                const checkedIds = checkedNodes.map(node => node.id);
                // 触发权限选择变化事件，通知父组件
                this.$emit('permission-change', checkedIds);
            },

            // 处理节点展开/收起变化
            handleExpandChange(expandedKeys) {
                this.$emit('expand-change', expandedKeys)
            }
        },
        watch: {
            // 当treeData变化时，自动展开所有节点
            treeData: {
                handler() {
                    // 使用多个nextTick确保组件完全渲染
                    this.$nextTick(() => {
                        this.$nextTick(() => {
                            this.autoExpandAll()
                        })
                    })
                },
                deep: true
            }
        }
    }
</script>

<style scoped>
    .permission-tree {
        padding: 10px 0;
        font-size: 14px;
    }

    .permission-root {
        /* 根节点容器样式 */
        height: 400px;
        overflow-y: auto;
    }

    .permission-node {
        margin: 2px 0;
    }

    .permission-header {
        display: flex;
        align-items: center;
        padding: 8px 10px;
        cursor: pointer;
        border-radius: 4px;
        transition: background-color 0.2s;
        min-height: 36px;
    }

        .permission-header:hover {
            background-color: #f5f5f5;
        }

    .expand-icon {
        width: 16px;
        height: 16px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 10px;
        transition: transform 0.2s ease;
        color: #666;
    }

        .expand-icon.placeholder {
            color: transparent;
        }

    .permission-checkbox {
        margin: 0 8px 0 0;
        width: 16px;
        height: 16px;
        cursor: pointer;
    }

    .permission-name {
        flex: 1;
        user-select: none;
    }

    .permission-children {
        margin-left: 16px;
        padding-left: 16px;
        border-left: 1px dashed #e0e0e0;
    }

    /* 展开/收起动画 */
    .expand-enter-active,
    .expand-leave-active {
        transition: all 0.3s ease;
        overflow: hidden;
    }

    .expand-enter,
    .expand-leave-to {
        max-height: 0;
        opacity: 0;
        transform: translateY(-10px);
    }

    .expand-enter-to,
    .expand-leave {
        max-height: 1000px;
        opacity: 1;
        transform: translateY(0);
    }

    /* 状态样式 */
    .loading-state,
    .error-state,
    .empty-state {
        text-align: center;
        padding: 40px 20px;
        color: #999;
        border-radius: 4px;
        background-color: #fafafa;
    }

    .error-state {
        color: #f56c6c;
        background-color: #fef0f0;
    }
</style>
