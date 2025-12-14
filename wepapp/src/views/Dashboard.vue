<template>
    <div class="dashboard">
        <!-- 仪表盘头部区域 -->
        <div class="dashboard-header gradient-bg">
            <div class="header-content">
                <div class="welcome-section">
                    <span class="welcome-icon">😊</span>
                    <h2 class="welcome-title">仪表盘</h2>
                    <p class="welcome-text">欢迎回来, admin! 今天是{{ currentDate }}</p>
                </div>
                <div class="header-actions">
                    <button class="refresh-btn">
                        <span class="refresh-icon">🔄</span> 刷新数据
                    </button>
                    <span class="last-update">最后更新: {{ lastUpdateTime }}</span>
                </div>
            </div>
        </div>

        <!-- 统计卡片区域 -->
        <div class="stats-grid">
            <!-- 总用户数卡片 -->
            <div class="stat-card user-card">
                <div class="stat-icon user-icon">👥</div>
                <div class="stat-content">
                    <h3 class="stat-number">16</h3>
                    <p class="stat-label">总用户数</p>
                </div>
                <div class="stat-footer">
                    <span class="footer-icon">📈</span>
                    <span class="footer-text">本周新增 16</span>
                </div>
                <div class="stat-status">
                    <span class="status-item active">
                        <span class="status-dot green"></span>
                        活跃: 16
                    </span>
                    <span class="status-item inactive">
                        <span class="status-dot gray"></span>
                        禁用: 0
                    </span>
                </div>
            </div>

            <!-- 角色数量卡片 -->
            <div class="stat-card role-card">
                <div class="stat-icon role-icon">🎭</div>
                <div class="stat-content">
                    <h3 class="stat-number">3</h3>
                    <p class="stat-label">角色数量</p>
                </div>
                <div class="stat-footer">
                    <span class="footer-icon">🎯</span>
                    <span class="footer-text">活跃角色 3</span>
                </div>
                <div class="stat-status">
                    <span class="status-item">
                        <span class="status-text">角色结构均衡，权限配置合理</span>
                    </span>
                </div>
            </div>

            <!-- 权限数量卡片 -->
            <div class="stat-card permission-card">
                <div class="stat-icon permission-icon">🔑</div>
                <div class="stat-content">
                    <h3 class="stat-number">36</h3>
                    <p class="stat-label">权限数量</p>
                </div>
                <div class="stat-footer">
                    <span class="footer-icon">📋</span>
                    <span class="footer-text">目录: 3 | 页面: 15</span>
                </div>
                <div class="stat-status">
                    <span class="status-item">
                        <span class="status-text">权限体系完整，层级清晰</span>
                    </span>
                </div>
            </div>

            <!-- 部门数量卡片 -->
            <div class="stat-card dept-card">
                <div class="stat-icon dept-icon">🏢</div>
                <div class="stat-content">
                    <h3 class="stat-number">24</h3>
                    <p class="stat-label">部门数量</p>
                </div>
                <div class="stat-footer">
                    <span class="footer-icon">👥</span>
                    <span class="footer-text">有用用户 7</span>
                </div>
                <div class="stat-status">
                    <span class="status-item">
                        <span class="status-text">组织结构明确，人员分布合理</span>
                    </span>
                </div>
            </div>
        </div>

        <!-- 用户活动趋势图表 -->
        <div class="activity-chart card">
            <div class="chart-header">
                <h3 class="section-title">用户活动趋势 (最近7天)</h3>
                <div class="chart-actions">
                    <button class="chart-action-btn active">日</button>
                    <button class="chart-action-btn">周</button>
                    <button class="chart-action-btn">月</button>
                </div>
            </div>
            <div class="chart-container">
                <!-- 简单的模拟折线图 -->
                <div class="chart-simulation">
                    <div class="chart-grid">
                        <div class="grid-line" v-for="i in 5" :key="i"></div>
                    </div>
                    <div class="chart-line-container">
                        <!-- 使用SVG路径实现的趋势线 -->
                        <svg class="chart-svg" viewBox="0 0 400 200" preserveAspectRatio="none">
                            <path class="chart-line-path" d="M50,150 Q100,100 150,120 T250,80 T350,100" />
                        </svg>
                        <div class="chart-dots">
                            <div class="chart-dot" v-for="i in 7" :key="i"></div>
                        </div>
                    </div>
                    <div class="chart-labels">
                        <span v-for="(label, index) in chartLabels" :key="index">{{ label }}</span>
                    </div>
                </div>
            </div>
        </div>

        <!-- 今日数据部分 -->
        <div class="today-data card">
            <div class="today-header">
                <h3 class="section-title">今日数据</h3>
            </div>
            <div class="today-stats">
                <div class="today-stat-item">
                    <div class="today-stat-label">今日登录</div>
                    <div class="today-stat-number">12</div>
                </div>
                <div class="today-stat-item">
                    <div class="today-stat-label">新增用户</div>
                    <div class="today-stat-number">3</div>
                </div>
                <div class="today-stat-item">
                    <div class="today-stat-label">活跃用户</div>
                    <div class="today-stat-number">8</div>
                </div>
                <div class="today-stat-item">
                    <div class="today-stat-label">系统访问</div>
                    <div class="today-stat-number">45</div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'Dashboard',
        data() {
            return {
                activities: [
                    { id: 1, content: '发布了新文章《Vue3实战指南》', user: '管理员', time: '今天 09:23' },
                    { id: 2, content: '创建了新分类《技术分享》', user: '管理员', time: '昨天 15:47' },
                    { id: 3, content: '编辑了文章《React性能优化》', user: '张三', time: '2024-01-10 11:30' },
                    { id: 4, content: '上传了5张新图片', user: '李四', time: '2024-01-09 16:15' },
                    { id: 5, content: '添加了新用户王五', user: '管理员', time: '2024-01-08 14:20' }
                ],
                currentDate: '',
                lastUpdateTime: '',
                chartLabels: ['12/03', '12/04', '12/05', '12/06', '12/07', '12/08', '12/09']
            }
        },
        mounted() {
            this.updateDateTime();
            // 每秒更新一次时间
            this.timer = setInterval(() => {
                this.updateDateTime();
            }, 1000);
        },
        beforeDestroy() {
            clearInterval(this.timer);
        },
        methods: {
            handleLogout() {
                // 清除登录状态
                localStorage.removeItem('isLoggedIn')
                // 跳转到登录页
                this.$router.push('/login')
            },
            updateDateTime() {
                // 获取当前日期和时间
                const now = new Date();
                const year = now.getFullYear();
                const month = String(now.getMonth() + 1).padStart(2, '0');
                const day = String(now.getDate()).padStart(2, '0');
                const hours = String(now.getHours()).padStart(2, '0');
                const minutes = String(now.getMinutes()).padStart(2, '0');
                const seconds = String(now.getSeconds()).padStart(2, '0');

                // 格式化星期
                const weekdays = ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'];
                const weekday = weekdays[now.getDay()];

                this.currentDate = `${year}年${month}月${day}日 ${weekday}`;
                this.lastUpdateTime = `${hours}:${minutes}:${seconds}`;
            }
        }
    }
</script>

<style scoped>
    .dashboard {
        padding: 1rem 0;
    }

    /* 渐变背景 */
    .gradient-bg {
        background: var(--primary-color);
        color: white;
        border-radius: 8px;
    }

    /* 仪表盘头部 */
    .dashboard-header {
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

    .welcome-section {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
    }

    .welcome-icon {
        font-size: 1.5rem;
        margin-right: 0.5rem;
    }

    .welcome-title {
        font-size: 1.8rem;
        margin: 0;
        display: flex;
        align-items: center;
        gap: 0.5rem;
    }

    .welcome-text {
        font-size: 1rem;
        margin: 0;
        opacity: 0.9;
    }

    .header-actions {
        display: flex;
        align-items: center;
        gap: 1rem;
        flex-wrap: wrap;
    }

    .refresh-btn {
        background: rgba(255, 255, 255, 0.2);
        color: white;
        border: none;
        padding: 0.5rem 1rem;
        border-radius: 4px;
        cursor: pointer;
        display: flex;
        align-items: center;
        gap: 0.5rem;
        font-weight: 500;
        transition: background 0.3s;
    }

        .refresh-btn:hover {
            background: rgba(255, 255, 255, 0.3);
        }

    .last-update {
        font-size: 0.9rem;
        opacity: 0.9;
    }

    /* 响应式设计 */
    @media (max-width: 768px) {
        .header-content {
            flex-direction: column;
            align-items: flex-start;
        }

        .welcome-title {
            font-size: 1.5rem;
        }
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
    .user-card .stat-icon {
        background: #4285f4;
    }

    .role-card .stat-icon {
        background: #ea4335;
    }

    .permission-card .stat-icon {
        background: #fbbc05;
    }

    .dept-card .stat-icon {
        background: #34a853;
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

    /* 卡片底部信息 */
    .stat-footer {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        margin-bottom: 1rem;
        padding-bottom: 1rem;
        border-bottom: 1px dashed #eee;
    }

    .footer-icon {
        font-size: 1rem;
        color: var(--primary-color);
    }

    .footer-text {
        font-size: 0.9rem;
        color: #666;
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

    /* 用户活动趋势图表 */
    .activity-chart {
        background: white;
        border-radius: 8px;
        padding: 1.5rem;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        margin-bottom: 1.5rem;
    }

    .chart-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1.5rem;
    }

    .chart-actions {
        display: flex;
        gap: 0.5rem;
    }

    .chart-action-btn {
        background: #f5f5f5;
        border: none;
        padding: 0.3rem 0.8rem;
        border-radius: 4px;
        cursor: pointer;
        font-size: 0.9rem;
        color: #666;
        transition: all 0.3s;
    }

        .chart-action-btn.active {
            background: var(--primary-color);
            color: white;
        }

    .chart-container {
        height: 250px;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    /* 模拟图表样式 */
    .chart-simulation {
        width: 100%;
        height: 100%;
        position: relative;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .chart-grid {
        position: absolute;
        width: 100%;
        height: 100%;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
    }

    .grid-line {
        height: 1px;
        background: #eee;
    }

    .chart-line-container {
        position: relative;
        width: 80%;
        height: 80%;
        overflow: visible;
    }

    /* 使用SVG路径实现更真实的趋势线 */
    .chart-svg {
        width: 100%;
        height: 100%;
        position: absolute;
        top: 0;
        left: 0;
    }

    .chart-line-path {
        fill: none;
        stroke: var(--primary-color);
        stroke-width: 2px;
        stroke-linecap: round;
    }

    .chart-dots {
        position: absolute;
        width: 100%;
        height: 100%;
        display: flex;
        justify-content: space-between;
        align-items: center;
        transform: translateY(0);
        padding: 0 5px;
    }

    .chart-dot {
        width: 8px;
        height: 8px;
        background: var(--primary-color);
        border-radius: 50%;
        border: 2px solid white;
        box-shadow: 0 0 0 1px rgba(0, 0, 0, 0.1);
        margin-top: 30px; /* 调整点的垂直位置 */
    }

    .chart-labels {
        position: absolute;
        bottom: -25px;
        width: 100%;
        display: flex;
        justify-content: space-between;
        color: #666;
        font-size: 0.85rem;
    }

    /* 今日数据部分 */
    .today-data {
        background: white;
        border-radius: 8px;
        padding: 1.5rem;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        margin-bottom: 1.5rem;
    }

    .today-header {
        margin-bottom: 1.5rem;
    }

    .today-stats {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
        gap: 1.5rem;
    }

    .today-stat-item {
        text-align: center;
    }

    .today-stat-label {
        color: #666;
        font-size: 0.9rem;
        margin-bottom: 0.5rem;
    }

    .today-stat-number {
        font-size: 2rem;
        font-weight: 600;
        color: var(--primary-color);
        margin: 0;
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
        .stats-grid {
            grid-template-columns: 1fr;
        }

        .today-stats {
            grid-template-columns: repeat(2, 1fr);
        }

        .chart-header {
            flex-direction: column;
            align-items: flex-start;
            gap: 1rem;
        }
    }
</style>