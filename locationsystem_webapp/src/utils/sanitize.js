// utils/sanitize.js
import DOMPurify from 'dompurify'

/**
 * 净化 HTML 内容
 * @param {string} html - 原始 HTML 字符串
 * @param {Object} options - DOMPurify 配置选项
 * @returns {string} 净化后的安全 HTML
 */
export function sanitizeHtml(html, options = {}) {
  if (!html) return ''

  const defaultOptions = {
    ALLOWED_TAGS: [
      'p', 'br', 'strong', 'em', 'u', 's',
      'h1', 'h2', 'h3', 'h4', 'h5', 'h6',
      'ul', 'ol', 'li', 'a', 'img',
      'div', 'span', 'table', 'tr', 'td', 'th',
      'blockquote', 'pre', 'code'
    ],
    ALLOWED_ATTR: [
      'href', 'target', 'src', 'alt',
      'class', 'id', 'title', 'width', 'height'
    ]
  }

  return DOMPurify.sanitize(html, { ...defaultOptions, ...options })
}
