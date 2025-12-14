<template>
    <div class="home-page">
        <!-- 顶部导航栏 -->
        <header class="home-header">
            <div class="container">
                <div class="logo">
                    <h1>金柴内容管理系统</h1>
                </div>
                <nav class="home-nav">
                    <ul>
                        <li><router-link to="/dashboard">管理后台</router-link></li>
                        <li><a href="#articles">文章</a></li>
                        <li><a href="#categories">分类</a></li>
                        <li><a href="#about">关于我们</a></li>
                    </ul>
                </nav>
            </div>
        </header>

        <!-- 主要内容区域 -->
        <main>
            <!-- 英雄区域 -->
            <section class="hero-section">
                <div class="container">
                    <div class="hero-content">
                        <h2>高效的内容管理解决方案</h2>
                        <p>专为现代网站和应用程序设计的内容管理系统，让您轻松管理和发布内容。</p>
                        <div class="hero-actions">
                            <router-link to="/dashboard" class="btn-primary">登录管理</router-link>
                            <a href="#features" class="btn-secondary">了解更多</a>
                        </div>
                    </div>
                </div>
            </section>

            <!-- 核心功能区域 - 轮播和文章列表 -->
            <section id="features" class="features-section">
                <div class="container">
                    <h2 class="section-title">核心功能</h2>
                    <div class="content-layout">
                        <!-- 左侧图片轮播 -->
                        <div class="carousel-container">
                            <div class="carousel" ref="carousel">
                                <div class="carousel-item" v-for="(slide, index) in carouselSlides" :key="index" :class="{ active: currentSlide === index }">
                                    <img :src="slide.image" :alt="slide.title" class="carousel-image">
                                    <div class="carousel-caption">
                                        <h3>{{ slide.title }}</h3>
                                        <p>{{ slide.description }}</p>
                                    </div>
                                </div>
                            </div>
                            <button class="carousel-control prev" @click="prevSlide">&lt;</button>
                            <button class="carousel-control next" @click="nextSlide">&gt;</button>
                            <div class="carousel-indicators">
                                <span v-for="(slide, index) in carouselSlides" :key="index"
                                      class="indicator"
                                      :class="{ active: currentSlide === index }"
                                      @click="goToSlide(index)"></span>
                            </div>
                        </div>

                        <!-- 右侧最新文章列表 -->
                        <div class="latest-articles">
                            <h3 class="list-title">最新文章</h3>
                            <div class="article-list">
                                <div class="article-item" v-for="article in latestArticles" :key="article.id">
                                    <div class="article-thumb">
                                        <img :src="article.image" :alt="article.title">
                                    </div>
                                    <div class="article-info">
                                        <h4 class="article-item-title">{{ article.title }}</h4>
                                        <div class="article-meta">
                                            <span class="article-date">{{ article.createTime }}</span>
                                        </div>
                                        <router-link :to="'/article/' + article.id" class="article-link">阅读更多</router-link>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <!-- 最新文章 -->
            <section id="articles" class="articles-section">
                <div class="container">
                    <h2 class="section-title">最新文章</h2>
                    <div class="articles-grid">
                        <!-- 文章卡片示例 -->
                        <div class="article-card">
                            <div class="article-thumbnail">
                                <img src="https://via.placeholder.com/400x200" alt="文章缩略图">
                            </div>
                            <div class="article-content">
                                <div class="article-meta">
                                    <span class="article-category">技术</span>
                                    <span class="article-date">2024-01-15</span>
                                </div>
                                <h3 class="article-title">Vue 3 新特性详解</h3>
                                <p class="article-excerpt">本文详细介绍了Vue 3中的新特性，包括组合式API、性能优化和响应式系统的改进。</p>
                                <router-link to="/article/1" class="article-link">阅读更多</router-link>
                            </div>
                        </div>
                        <div class="article-card">
                            <div class="article-thumbnail">
                                <img src="https://via.placeholder.com/400x200" alt="文章缩略图">
                            </div>
                            <div class="article-content">
                                <div class="article-meta">
                                    <span class="article-category">教程</span>
                                    <span class="article-date">2024-01-10</span>
                                </div>
                                <h3 class="article-title">Vite 构建工具入门指南</h3>
                                <p class="article-excerpt">快速掌握Vite构建工具的使用方法，提高前端开发效率和项目性能。</p>
                                <router-link to="/article/2" class="article-link">阅读更多</router-link>
                            </div>
                        </div>
                        <div class="article-card">
                            <div class="article-thumbnail">
                                <img src="https://via.placeholder.com/400x200" alt="文章缩略图">
                            </div>
                            <div class="article-content">
                                <div class="article-meta">
                                    <span class="article-category">最佳实践</span>
                                    <span class="article-date">2024-01-05</span>
                                </div>
                                <h3 class="article-title">前端性能优化技巧</h3>
                                <p class="article-excerpt">10个实用的前端性能优化技巧，让您的网站加载更快，用户体验更好。</p>
                                <router-link to="/article/3" class="article-link">阅读更多</router-link>
                            </div>
                        </div>
                    </div>
                    <div class="articles-more">
                        <a href="#" class="btn-secondary">查看全部文章</a>
                    </div>
                </div>
            </section>

            <!-- 分类展示 -->
            <section id="categories" class="categories-section">
                <div class="container">
                    <h2 class="section-title">内容分类</h2>
                    <div class="categories-grid">
                        <a href="#" class="category-card">
                            <div class="category-icon">💻</div>
                            <span class="category-name">技术</span>
                            <span class="category-count">28篇</span>
                        </a>
                        <a href="#" class="category-card">
                            <div class="category-icon">📚</div>
                            <span class="category-name">教程</span>
                            <span class="category-count">15篇</span>
                        </a>
                        <a href="#" class="category-card">
                            <div class="category-icon">✅</div>
                            <span class="category-name">最佳实践</span>
                            <span class="category-count">12篇</span>
                        </a>
                        <a href="#" class="category-card">
                            <div class="category-icon">📢</div>
                            <span class="category-name">公告</span>
                            <span class="category-count">5篇</span>
                        </a>
                    </div>
                </div>
            </section>

            <!-- 关于我们 -->
            <section id="about" class="about-section">
                <div class="container">
                    <h2 class="section-title">关于我们</h2>
                    <div class="about-content">
                        <div class="about-text">
                            <p>金柴内容管理系统是一个基于现代前端技术构建的开源CMS解决方案。我们致力于为开发者和内容创作者提供简单、高效、灵活的内容管理工具。</p>
                            <p>我们的系统采用Vue 3、Vue Router和Vite等前沿技术栈，确保了出色的性能和用户体验。</p>
                        </div>
                        <div class="about-stats">
                            <div class="stat-item">
                                <div class="stat-number">100+</div>
                                <div class="stat-label">内容创作者</div>
                            </div>
                            <div class="stat-item">
                                <div class="stat-number">500+</div>
                                <div class="stat-label">发布文章</div>
                            </div>
                            <div class="stat-item">
                                <div class="stat-number">24/7</div>
                                <div class="stat-label">技术支持</div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <!-- 联系表单 -->
            <section class="contact-section">
                <div class="container">
                    <h2 class="section-title">联系我们</h2>
                    <div class="contact-form">
                        <div class="form-group">
                            <label for="name">姓名</label>
                            <input type="text" id="name" placeholder="请输入您的姓名">
                        </div>
                        <div class="form-group">
                            <label for="email">邮箱</label>
                            <input type="email" id="email" placeholder="请输入您的邮箱">
                        </div>
                        <div class="form-group">
                            <label for="message">留言</label>
                            <textarea id="message" rows="5" placeholder="请输入您的留言内容"></textarea>
                        </div>
                        <button type="submit" class="btn-primary">提交留言</button>
                    </div>
                </div>
            </section>
        </main>

        <!-- 页脚 -->
        <footer class="home-footer">
            <div class="container">
                <div class="footer-content">
                    <div class="footer-info">
                        <h3>金柴内容管理系统</h3>
                        <p>高效的内容管理解决方案</p>
                    </div>
                    <div class="footer-links">
                        <h4>快速链接</h4>
                        <ul>
                            <li><router-link to="/dashboard">管理后台</router-link></li>
                            <li><a href="#articles">文章</a></li>
                            <li><a href="#categories">分类</a></li>
                            <li><a href="#about">关于我们</a></li>
                        </ul>
                    </div>
                    <div class="footer-contact">
                        <h4>联系我们</h4>
                        <p>邮箱: contact@jinchai.com</p>
                        <p>电话: 123-456-7890</p>
                    </div>
                </div>
                <div class="footer-bottom">
                    <p>&copy; 2024 金柴内容管理系统. All rights reserved.</p>
                </div>
            </div>
        </footer>
    </div>
</template>

<script>
    import request from "../utils/request.js"
    export default {
        name: 'Home',
        data() {
            return {
                // 轮播数据
                carouselSlides: [
                    //{
                    //  image: 'https://via.placeholder.com/600x400?text=文章管理系统',
                    //  title: '文章管理系统',
                    //  description: '轻松创建、编辑和发布文章，支持富文本编辑和媒体管理'
                    //},
                    //{
                    //  image: 'https://via.placeholder.com/600x400?text=分类管理',
                    //  title: '灵活的分类系统',
                    //  description: '帮助您组织和分类各类内容，提高内容管理效率'
                    //},
                    //{
                    //  image: 'https://via.placeholder.com/600x400?text=用户管理',
                    //  title: '完善的用户权限控制',
                    //  description: '支持多角色管理，确保系统安全和用户数据保护'
                    //}
                ],
                currentSlide: 0,

                // 最新文章数据
                latestArticles: [
                    //{
                    //  id: 1,
                    //  image: 'https://via.placeholder.com/80x80?text=Vue3',
                    //  title: 'Vue 3 新特性详解',
                    //  category: '技术',
                    //  date: '2024-01-15'
                    //},
                    //{
                    //  id: 2,
                    //  image: 'https://via.placeholder.com/80x80?text=Vite',
                    //  title: 'Vite 构建工具入门指南',
                    //  category: '教程',
                    //  date: '2024-01-10'
                    //},
                    //{
                    //  id: 3,
                    //  image: 'https://via.placeholder.com/80x80?text=性能优化',
                    //  title: '前端性能优化技巧',
                    //  category: '最佳实践',
                    //  date: '2024-01-05'
                    //},
                    //{
                    //  id: 4,
                    //  image: 'https://via.placeholder.com/80x80?text=React',
                    //  title: 'React Hooks 实战应用',
                    //  category: '技术',
                    //  date: '2024-01-01'
                    //}
                ],

                // 轮播定时器
                carouselInterval: null
            }
        },
        mounted() {
            // 页面加载后的逻辑
            this.setupSmoothScroll()
            this.startCarousel()
            this.loadArtcle()
        },
        beforeUnmount() {
            // 组件销毁前清除定时器
            if (this.carouselInterval) {
                clearInterval(this.carouselInterval)
            }
        },
        methods: {
            async loadArtcle() {
                const self = this;
                await request.get("api/Home/articles").then(res => {
                    if (res.status === 200) {
                        console.log(res.data)
                        self.latestArticles = res.data;
                    }
                });
            },
            setupSmoothScroll() {
                // 平滑滚动到锚点
                document.querySelectorAll('a[href^="#"]').forEach(anchor => {
                    anchor.addEventListener('click', function (e) {
                        e.preventDefault()
                        const targetId = this.getAttribute('href')
                        if (targetId === '#') return

                        const targetElement = document.querySelector(targetId)
                        if (targetElement) {
                            targetElement.scrollIntoView({
                                behavior: 'smooth'
                            })
                        }
                    })
                })
            },

            // 轮播相关方法
            startCarousel() {
                this.carouselInterval = setInterval(() => {
                    this.nextSlide()
                }, 5000)
            },

            nextSlide() {
                this.currentSlide = (this.currentSlide + 1) % this.carouselSlides.length
            },

            prevSlide() {
                this.currentSlide = (this.currentSlide - 1 + this.carouselSlides.length) % this.carouselSlides.length
            },

            goToSlide(index) {
                this.currentSlide = index
            }

        }
    }
</script>

<style scoped>
    /* 全局样式 */
    .home-page {
        font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
        line-height: 1.6;
        color: #333;
    }

    .container {
        max-width: 1200px;
        margin: 0 auto;
        padding: 0 20px;
    }

    .section-title {
        text-align: center;
        font-size: 2rem;
        margin-bottom: 2rem;
        color: #2c3e50;
    }

    /* 按钮样式 */
    .btn-primary {
        display: inline-block;
        padding: 10px 20px;
        background-color: #3498db;
        color: white;
        text-decoration: none;
        border-radius: 4px;
        transition: background-color 0.3s;
        border: none;
        cursor: pointer;
        font-size: 1rem;
    }

        .btn-primary:hover {
            background-color: #2980b9;
        }

    .btn-secondary {
        display: inline-block;
        padding: 10px 20px;
        background-color: transparent;
        color: #3498db;
        text-decoration: none;
        border-radius: 4px;
        transition: all 0.3s;
        border: 1px solid #3498db;
        cursor: pointer;
        font-size: 1rem;
    }

        .btn-secondary:hover {
            background-color: #3498db;
            color: white;
        }

    /* 头部样式 */
    .home-header {
        background-color: white;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        position: sticky;
        top: 0;
        z-index: 100;
    }

        .home-header .container {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 1rem 20px;
        }

    .logo h1 {
        margin: 0;
        font-size: 1.5rem;
        color: #2c3e50;
    }

    .home-nav ul {
        list-style: none;
        display: flex;
        margin: 0;
        padding: 0;
    }

    .home-nav li {
        margin-left: 2rem;
    }

    .home-nav a {
        text-decoration: none;
        color: #333;
        transition: color 0.3s;
    }

        .home-nav a:hover {
            color: #3498db;
        }

    /* 英雄区域 */
    .hero-section {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        padding: 6rem 0;
        text-align: center;
    }

    .hero-content h2 {
        font-size: 3rem;
        margin-bottom: 1rem;
        line-height: 1.2;
    }

    .hero-content p {
        font-size: 1.2rem;
        margin-bottom: 2rem;
        max-width: 800px;
        margin-left: auto;
        margin-right: auto;
    }

    .hero-actions {
        display: flex;
        justify-content: center;
        gap: 1rem;
    }

    /* 核心功能区域 - 轮播和文章列表 */
    .features-section {
        padding: 4rem 0;
        background-color: #f9f9f9;
    }

    .content-layout {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 2rem;
        align-items: stretch;
    }

    /* 轮播样式 */
    .carousel-container {
        position: relative;
        background-color: white;
        border-radius: 8px;
        overflow: hidden;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        height: 400px;
    }

    .carousel {
        height: 100%;
        position: relative;
    }

    .carousel-item {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        opacity: 0;
        transition: opacity 0.5s ease;
    }

        .carousel-item.active {
            opacity: 1;
            z-index: 1;
        }

    .carousel-image {
        width: 100%;
        height: 100%;
        object-fit: cover;
    }

    .carousel-caption {
        position: absolute;
        bottom: 0;
        left: 0;
        right: 0;
        background: linear-gradient(transparent, rgba(0, 0, 0, 0.8));
        color: white;
        padding: 1.5rem;
    }

        .carousel-caption h3 {
            margin: 0 0 0.5rem 0;
            font-size: 1.5rem;
        }

        .carousel-caption p {
            margin: 0;
            font-size: 1rem;
            opacity: 0.9;
        }

    .carousel-control {
        position: absolute;
        top: 50%;
        transform: translateY(-50%);
        background-color: rgba(255, 255, 255, 0.8);
        border: none;
        border-radius: 50%;
        width: 40px;
        height: 40px;
        font-size: 1.5rem;
        cursor: pointer;
        transition: background-color 0.3s;
        z-index: 10;
    }

        .carousel-control:hover {
            background-color: white;
        }

        .carousel-control.prev {
            left: 1rem;
        }

        .carousel-control.next {
            right: 1rem;
        }

    .carousel-indicators {
        position: absolute;
        bottom: 1rem;
        left: 50%;
        transform: translateX(-50%);
        display: flex;
        gap: 0.5rem;
        z-index: 10;
    }

    .indicator {
        width: 10px;
        height: 10px;
        border-radius: 50%;
        background-color: rgba(255, 255, 255, 0.5);
        cursor: pointer;
        transition: background-color 0.3s;
    }

        .indicator.active {
            background-color: white;
        }

    /* 最新文章列表样式 */
    .latest-articles {
        background-color: white;
        border-radius: 8px;
        padding: 1.5rem;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        height: 400px;
        overflow-y: auto;
        display: flex;
        flex-direction: column;
    }
  
    .list-title {
        font-size: 1.5rem;
        margin-top: 0;
        margin-bottom: 1.5rem;
        color: #2c3e50;
        padding-bottom: 0.5rem;
        border-bottom: 2px solid #3498db;
    }

    .article-list {
        display: flex;
        flex-direction: column;
        gap: 1.5rem;
        overflow-y: auto;
        max-height: 400px;
        max-height: 400px !important;
        box-sizing: border-box;
    }

    .article-item {
        display: flex;
        gap: 1rem;
        transition: transform 0.3s;
    }

        .article-item:hover {
            transform: translateX(5px);
        }

    .article-thumb {
        flex-shrink: 0;
        width: 80px;
        height: 80px;
        border-radius: 4px;
        overflow: hidden;
    }

        .article-thumb img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

    .article-info {
        flex: 1;
        display: flex;
        flex-direction: column;
    }

    .article-item-title {
        margin: 0 0 0.5rem 0;
        font-size: 1.1rem;
        color: #2c3e50;
        line-height: 1.4;
    }

    .article-item .article-meta {
        margin-bottom: auto;
    }

    .article-item .article-link {
        margin-top: 0.5rem;
    }

    /* 响应式设计 */
    @media (max-width: 992px) {
        .content-layout {
            grid-template-columns: 1fr;
        }

        .carousel-container {
            height: 350px;
        }
    }

    /* 文章列表 */
    .articles-section {
        padding: 4rem 0;
    }

    .articles-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
        gap: 2rem;
        margin-bottom: 2rem;
    }

    .article-card {
        background-color: white;
        border-radius: 8px;
        overflow: hidden;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        transition: transform 0.3s, box-shadow 0.3s;
    }

        .article-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.15);
        }

    .article-thumbnail {
        width: 100%;
        height: 200px;
        overflow: hidden;
    }

        .article-thumbnail img {
            width: 100%;
            height: 100%;
            object-fit: cover;
            transition: transform 0.3s;
        }

    .article-card:hover .article-thumbnail img {
        transform: scale(1.05);
    }

    .article-content {
        padding: 1.5rem;
    }

    .article-meta {
        display: flex;
        gap: 1rem;
        margin-bottom: 1rem;
        font-size: 0.9rem;
        color: #666;
    }

    .article-category {
        background-color: #3498db;
        color: white;
        padding: 3px 8px;
        border-radius: 4px;
        font-size: 0.8rem;
    }

    .article-title {
        font-size: 1.3rem;
        margin-bottom: 1rem;
        color: #2c3e50;
    }

    .article-excerpt {
        color: #666;
        margin-bottom: 1rem;
    }

    .article-link {
        color: #3498db;
        text-decoration: none;
        font-weight: 500;
        transition: color 0.3s;
    }

        .article-link:hover {
            color: #2980b9;
        }

    .articles-more {
        text-align: center;
    }

    /* 分类展示 */
    .categories-section {
        padding: 4rem 0;
        background-color: #f9f9f9;
    }

    .categories-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 1.5rem;
    }

    .category-card {
        background-color: white;
        padding: 2rem;
        border-radius: 8px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        text-align: center;
        text-decoration: none;
        color: #333;
        transition: transform 0.3s, box-shadow 0.3s;
    }

        .category-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 5px 20px rgba(0, 0, 0, 0.15);
        }

    .category-icon {
        font-size: 3rem;
        margin-bottom: 1rem;
    }

    .category-name {
        display: block;
        font-size: 1.2rem;
        font-weight: 500;
        margin-bottom: 0.5rem;
    }

    .category-count {
        color: #666;
        font-size: 0.9rem;
    }

    /* 关于我们 */
    .about-section {
        padding: 4rem 0;
    }

    .about-content {
        display: flex;
        flex-direction: column;
        gap: 2rem;
    }

    .about-text {
        max-width: 800px;
        margin: 0 auto;
        text-align: center;
        line-height: 1.8;
        color: #666;
    }

    .about-stats {
        display: flex;
        justify-content: space-around;
        flex-wrap: wrap;
        gap: 2rem;
    }

    .stat-item {
        text-align: center;
    }

    .stat-number {
        font-size: 2.5rem;
        font-weight: bold;
        color: #3498db;
    }

    .stat-label {
        color: #666;
    }

    /* 联系表单 */
    .contact-section {
        padding: 4rem 0;
        background-color: #f9f9f9;
    }

    .contact-form {
        max-width: 600px;
        margin: 0 auto;
        background-color: white;
        padding: 2rem;
        border-radius: 8px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    }

    .form-group {
        margin-bottom: 1.5rem;
    }

        .form-group label {
            display: block;
            margin-bottom: 0.5rem;
            font-weight: 500;
            color: #2c3e50;
        }

        .form-group input,
        .form-group textarea {
            width: 100%;
            padding: 0.75rem;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 1rem;
        }

        .form-group textarea {
            resize: vertical;
        }

    /* 页脚 */
    .home-footer {
        background-color: #2c3e50;
        color: white;
        padding: 3rem 0 1.5rem;
    }

    .footer-content {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 2rem;
        margin-bottom: 2rem;
    }

    .footer-info h3 {
        margin-top: 0;
        font-size: 1.5rem;
    }

    .footer-links h4,
    .footer-contact h4 {
        margin-top: 0;
        font-size: 1.2rem;
        margin-bottom: 1rem;
    }

    .footer-links ul {
        list-style: none;
        padding: 0;
        margin: 0;
    }

    .footer-links li {
        margin-bottom: 0.5rem;
    }

    .footer-links a {
        color: #ddd;
        text-decoration: none;
        transition: color 0.3s;
    }

        .footer-links a:hover {
            color: white;
        }

    .footer-bottom {
        text-align: center;
        padding-top: 1.5rem;
        border-top: 1px solid #34495e;
        font-size: 0.9rem;
        color: #ddd;
    }

    /* 响应式设计 */
    @media (max-width: 768px) {
        .home-header .container {
            flex-direction: column;
            gap: 1rem;
        }

        .home-nav li {
            margin-left: 1rem;
            margin-right: 1rem;
        }

        .hero-content h2 {
            font-size: 2.5rem;
        }

        .hero-actions {
            flex-direction: column;
            align-items: center;
        }

        .articles-grid {
            grid-template-columns: 1fr;
        }

        .about-stats {
            flex-direction: column;
            align-items: center;
        }
    }

    @media (max-width: 480px) {
        .hero-content h2 {
            font-size: 2rem;
        }

        .section-title {
            font-size: 1.8rem;
        }

        .features-grid,
        .categories-grid {
            grid-template-columns: 1fr;
        }
    }
</style>