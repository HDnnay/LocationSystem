<template>
    <div class="app">
        <!-- 使用RouterViewManager组件管理不同路由视图 -->
        <RouterViewManager :sidebar-open="sidebarOpen"
                           :is-mobile="isMobile"
                           :is-logged-in="isLoggedIn"
                           :open-dropdown="openDropdown"
                           @update:sidebar-open="sidebarOpen = $event"
                           @update:open-dropdown="openDropdown = $event"
                           @logout="handleLogout" />
    </div>
</template>

<script>
    import RouterViewManager from './components/RouterViewManager.vue'

    export default {
        name: 'App',
        components: {
            RouterViewManager
        },
        data() {
        return {
            sidebarOpen: false,
            isMobile: false,
            isLoggedIn: false, // 默认设置为未登录状态
            openDropdown: null
        }
    },
    mounted() {
        this.checkScreenSize()
        // 从localStorage读取登录状态
        this.isLoggedIn = localStorage.getItem('access_token') !== null
        window.addEventListener('resize', this.checkScreenSize)

        // 全局路由守卫已移除，不再需要登录检查
    },
        beforeUnmount() {
            window.removeEventListener('resize', this.checkScreenSize)
        },
        methods: {
            checkScreenSize() {
                this.isMobile = window.innerWidth < 768
                if (!this.isMobile) {
                    this.sidebarOpen = true
                } else {
                    this.sidebarOpen = false
                }
            },
            handleLogout() {
                // 清除localStorage中的token和用户信息
                localStorage.removeItem('access_token')
                localStorage.removeItem('refresh_token')
                localStorage.removeItem('user_type')
                localStorage.removeItem('user_info')

                // 更新登录状态
                this.isLoggedIn = false

                // 跳转到登录页
                this.$router.push('/login')
            }
        }
    }
</script>

<style>
    :root {
        --primary-color: #4285f4;
        --primary-hover: #3367d6;
        --sidebar-bg: #2c3e50;
        --text-color: #333;
        --light-text: #ecf0f1;
        --border-color: #ddd;
        --hover-bg: #34495e;
        --success-color: #27ae60;
        --success-hover: #219a52;
        --warning-color: #ffc107;
        --danger-color: #dc3545;
        --danger-hover: #c82333;
        --info-color: #17a2b8;
        --info-hover: #138496;
        --gray-color: #6c757d;
        --gray-hover: #545b62;
        --box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        --box-shadow-hover: 0 4px 12px rgba(0, 0, 0, 0.15);
        --admin-bg: #f5f7fa;
    }

    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
    }

    body {
        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
        background-color: #f5f7fa;
        color: var(--text-color);
        line-height: 1.6;
    }

    /* 全局按钮样式 */
    button {
        transition: all 0.3s ease;
        font-family: inherit;
    }

        button:active {
            transform: scale(0.98);
        }

    /* 特定按钮样式 */
    .search-btn {
        padding: 0.5rem 1rem;
        background-color: var(--primary-color);
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-weight: 500;
    }

        .search-btn:hover {
            background-color: var(--primary-hover);
        }

    .add-btn {
        padding: 0.5rem 1rem;
        background-color: var(--success-color);
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-weight: 500;
    }

        .add-btn:hover {
            background-color: var(--success-hover);
        }

    .edit-btn {
        background-color: var(--info-color);
        color: white;
    }

        .edit-btn:hover {
            background-color: var(--info-hover);
        }

    .delete-btn {
        background-color: var(--danger-color);
        color: white;
    }

        .delete-btn:hover {
            background-color: var(--danger-hover);
        }

        .delete-btn:disabled:hover {
            background-color: var(--gray-color);
            cursor: not-allowed;
        }

    .cancel-btn {
        background-color: var(--gray-color);
        color: white;
    }

        .cancel-btn:hover {
            background-color: var(--gray-hover);
        }

    .save-btn {
        background-color: var(--primary-color);
        color: white;
    }

        .save-btn:hover {
            background-color: var(--primary-hover);
        }

    /* 卡片样式 */
    .card {
        background-color: white;
        border-radius: 8px;
        box-shadow: var(--box-shadow);
        padding: 1.5rem;
        transition: box-shadow 0.3s ease, transform 0.2s ease;
    }

        .card:hover {
            box-shadow: var(--box-shadow-hover);
            transform: translateY(-2px);
        }

    /* 表格样式 */
    table {
        width: 100%;
        border-collapse: collapse;
        background-color: white;
        border-radius: 8px;
        overflow: hidden;
        box-shadow: var(--box-shadow);
    }

    th, td {
        padding: 0.75rem 1rem;
        text-align: left;
        border-bottom: 1px solid var(--border-color);
    }

    th {
        background-color: #f8f9fa;
        font-weight: 600;
        color: #495057;
    }

    tr:hover {
        background-color: #f8f9fa;
    }

    /* 模态框样式 */
    .modal-overlay {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000;
        animation: fadeIn 0.3s ease;
    }

    .modal {
        background-color: white;
        border-radius: 8px;
        width: 90%;
        max-width: 600px;
        max-height: 90vh;
        overflow-y: auto;
        box-shadow: 0 10px 25px rgba(0, 0, 0, 0.2);
        animation: slideIn 0.3s ease;
    }

    @keyframes fadeIn {
        from {
            opacity: 0;
        }

        to {
            opacity: 1;
        }
    }

    @keyframes slideIn {
        from {
            transform: translateY(-20px);
            opacity: 0;
        }

        to {
            transform: translateY(0);
            opacity: 1;
        }
    }

    /* 全局输入框样式 - 仅应用于非Element Plus组件 */
    input:not(.el-input__inner), select:not(.el-select__input), textarea:not(.el-textarea__inner) {
        font-family: inherit;
        padding: 0.5rem 0.75rem;
        border: 1px solid var(--border-color);
        border-radius: 4px;
        background-color: white;
        color: var(--text-color);
        font-size: 1rem;
        transition: border-color 0.3s ease, box-shadow 0.3s ease;
    }

        input:not(.el-input__inner):focus, select:not(.el-select__input):focus, textarea:not(.el-textarea__inner):focus {
            outline: none;
            border-color: var(--primary-color);
            box-shadow: 0 0 0 2px rgba(66, 133, 244, 0.2);
        }

    .app {
        display: flex;
        flex-direction: column;
        min-height: 100vh;
    }

    /* 后台导航栏样式 */
    .admin-navbar {
        background-color: var(--primary-color);
        color: white;
        padding: 0 1rem;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        position: sticky;
        top: 0;
        z-index: 100;
    }

    /* 后台容器样式 */
    .admin-container {
        display: flex;
        flex: 1;
    }

    /* 后台侧边栏样式 */
    .admin-sidebar {
        width: 240px;
        background-color: var(--sidebar-bg);
        color: var(--light-text);
        transition: transform 0.3s ease, width 0.3s ease;
        position: fixed;
        height: calc(100vh - 60px);
        overflow-y: auto;
        z-index: 99;
    }

    /* 后台内容区域样式 */
    .admin-content {
        flex: 1;
        padding: 1.5rem;
        margin-left: 240px;
        max-width: calc(100vw - 240px);
        min-height: calc(100vh - 60px);
        background-color: var(--admin-bg);
    }

    .navbar {
        background-color: var(--primary-color);
        color: white;
        padding: 0 1rem;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

    .navbar-container {
        display: flex;
        justify-content: space-between;
        align-items: center;
        height: 60px;
        max-width: 1200px;
        margin: 0 auto;
    }

    .logo h1 {
        font-size: 1.5rem;
        font-weight: 500;
    }

    .menu-toggle {
        background: none;
        border: none;
        color: white;
        font-size: 1.5rem;
        cursor: pointer;
        padding: 0.5rem;
        border-radius: 4px;
        transition: background-color 0.3s;
    }

        .menu-toggle:hover,
        .menu-toggle.active {
            background-color: rgba(255, 255, 255, 0.1);
        }

    .user-info {
        display: flex;
        align-items: center;
        gap: 1rem;
    }

    .main-container {
        display: flex;
        flex: 1;
    }

    .sidebar {
        width: 240px;
        background-color: var(--sidebar-bg);
        color: var(--light-text);
        transition: transform 0.3s ease, width 0.3s ease;
        position: fixed;
        height: calc(100vh - 60px);
        overflow-y: auto;
        z-index: 99;
    }

    .logout-btn {
        background-color: var(--danger-color);
        color: white;
        border: none;
        padding: 0.5rem 1rem;
        border-radius: 4px;
        cursor: pointer;
        font-size: 0.9rem;
    }

        .logout-btn:hover {
            background-color: var(--danger-hover);
        }

    .sidebar-nav ul {
        list-style: none;
        padding: 1rem 0;
    }

    .nav-item {
        margin: 0.5rem 0;
    }

    .nav-dropdown {
        margin: 0.5rem 0;
    }

    .dropdown-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 0.75rem 1.5rem;
        color: var(--light-text);
        cursor: pointer;
        border-radius: 4px;
        transition: all 0.3s ease;
    }

        .dropdown-header:hover,
        .dropdown-header.active {
            background-color: var(--hover-bg);
            color: var(--light-text);
        }

    .dropdown-icon {
        font-size: 0.75rem;
        transition: transform 0.3s ease;
    }

        .dropdown-icon.rotated {
            transform: rotate(180deg);
        }

    .dropdown-menu {
        margin: 0.25rem 0 0.25rem 1rem;
        padding-left: 1rem;
        animation: slideDown 0.3s ease;
    }

    @keyframes slideDown {
        from {
            opacity: 0;
            transform: translateY(-5px);
        }

        to {
            opacity: 1;
            transform: translateY(0);
        }
    }

    .dropdown-menu li {
        margin: 0.25rem 0;
    }

    .nav-item a {
        display: block;
        padding: 0.75rem 1.5rem;
        color: var(--light-text);
        text-decoration: none;
        transition: background-color 0.3s;
    }

        .nav-item a:hover {
            background-color: var(--hover-bg);
        }

    .nav-item.active a {
        background-color: var(--primary-color);
        font-weight: 500;
    }

    .content {
        flex: 1;
        padding: 1.5rem;
        margin-left: 240px;
        max-width: calc(100vw - 240px);
    }

    /* 移动端响应式 */
    @media (max-width: 768px) {
        .admin-sidebar {
            transform: translateX(-100%);
            z-index: 100;
            box-shadow: 2px 0 8px rgba(0, 0, 0, 0.15);
        }

            .admin-sidebar.sidebar-open {
                transform: translateX(0);
            }

        .admin-content {
            margin-left: 0;
            max-width: 100vw;
            padding: 1rem;
        }

        .navbar-container {
            padding: 0 0.5rem;
        }

        .logo h1 {
            font-size: 1.2rem;
        }

        /* 添加移动端遮罩层 */
        .sidebar-overlay {
            position: fixed;
            top: 70px;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 98;
            display: none;
        }

            .sidebar-overlay.active {
                display: block;
            }
    }

    /* 平板设备响应式 */
    @media (min-width: 769px) and (max-width: 1024px) {
        .admin-sidebar {
            width: 200px;
        }

        .admin-content {
            margin-left: 200px;
            max-width: calc(100vw - 200px);
        }
    }

    /* 重定向提示样式 */
    .redirecting {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100vh;
        flex-direction: column;
        font-size: 1.2rem;
        color: var(--text-color);
        background-color: var(--admin-bg);
        text-align: center;
    }

    .login-link {
        display: inline-block;
        margin-top: 1rem;
        padding: 0.75rem 1.5rem;
        background-color: var(--primary-color);
        color: white;
        text-decoration: none;
        border-radius: 4px;
        transition: background-color 0.3s;
    }

        .login-link:hover {
            background-color: var(--primary-hover);
        }

    /* 过渡效果 */
    .fade-enter-active, .fade-leave-active {
        transition: opacity 0.3s;
    }

    .fade-enter-from, .fade-leave-to {
        opacity: 0;
    }
</style>
