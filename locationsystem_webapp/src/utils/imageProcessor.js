/**
 * 压缩图片
 * @param {File} file - 原始图片文件
 * @param {Object} options - 压缩选项
 * @param {number} options.maxWidth - 最大宽度
 * @param {number} options.quality - 压缩质量 (0-1)
 * @returns {Promise<Object>} 包含压缩后文件和大小信息的对象
 */
export function compressImage(file, options = {}) {
  return new Promise((resolve, reject) => {
    const { maxWidth = 1920, quality = 0.8 } = options;
    const originalSize = file.size;
    
    const canvas = document.createElement('canvas');
    const ctx = canvas.getContext('2d');
    const img = new Image();
    
    img.onload = () => {
      // 计算压缩后的尺寸
      let { width, height } = img;
      if (width > maxWidth) {
        height = (height * maxWidth) / width;
        width = maxWidth;
      }
      
      canvas.width = width;
      canvas.height = height;
      
      // 绘制压缩后的图片
      ctx.drawImage(img, 0, 0, width, height);
      
      // 转换为Blob
      canvas.toBlob((blob) => {
        if (blob) {
          // 保持原始文件的名称和类型
          const compressedFile = new File([blob], file.name, {
            type: file.type,
            lastModified: Date.now()
          });
          resolve({
            file: compressedFile,
            originalSize: originalSize,
            compressedSize: blob.size,
            compressionRatio: (blob.size / originalSize * 100).toFixed(2)
          });
        } else {
          reject(new Error('图片压缩失败'));
        }
      }, file.type, quality);
    };
    
    img.onerror = () => {
      reject(new Error('图片加载失败'));
    };
    
    img.src = URL.createObjectURL(file);
  });
}

/**
 * 批量压缩图片
 * @param {File[]} files - 原始图片文件数组
 * @param {Object} options - 压缩选项
 * @returns {Promise<Object>} 包含压缩后文件和统计信息的对象
 */
export async function compressImages(files, options = {}) {
  const compressedFiles = [];
  const compressionStats = [];
  
  for (const file of files) {
    try {
      // 只压缩图片文件
      if (file.type.startsWith('image/')) {
        const result = await compressImage(file, options);
        compressedFiles.push(result.file);
        compressionStats.push({
          name: file.name,
          originalSize: result.originalSize,
          compressedSize: result.compressedSize,
          compressionRatio: result.compressionRatio
        });
      } else {
        // 非图片文件直接添加
        compressedFiles.push(file);
        compressionStats.push({
          name: file.name,
          originalSize: file.size,
          compressedSize: file.size,
          compressionRatio: '100.00' // 未压缩
        });
      }
    } catch (error) {
      console.error('压缩图片失败:', error);
      // 压缩失败时使用原始文件
      compressedFiles.push(file);
      compressionStats.push({
        name: file.name,
        originalSize: file.size,
        compressedSize: file.size,
        compressionRatio: '100.00', // 压缩失败
        error: error.message
      });
    }
  }
  
  return {
    files: compressedFiles,
    stats: compressionStats,
    totalOriginalSize: compressionStats.reduce((sum, stat) => sum + stat.originalSize, 0),
    totalCompressedSize: compressionStats.reduce((sum, stat) => sum + stat.compressedSize, 0),
    overallCompressionRatio: compressionStats.length > 0 
      ? ((compressionStats.reduce((sum, stat) => sum + stat.compressedSize, 0) / 
         compressionStats.reduce((sum, stat) => sum + stat.originalSize, 0) * 100).toFixed(2))
      : '0.00'
  };
}

/**
 * 格式化文件大小
 * @param {number} bytes - 字节数
 * @returns {string} 格式化后的大小字符串
 */
export function formatFileSize(bytes) {
  if (bytes === 0) return '0 B';
  const k = 1024;
  const sizes = ['B', 'KB', 'MB', 'GB'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));
  return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}