<template>
  <div>
    <el-page-header class="page-container">
      <template #title>
        <h3>租房信息管理</h3>
      </template>
    </el-page-header>
    <el-card class="toolbar-card" shadow="hover">
      <div class="toolbar">
        <el-input v-model="searchQuery" placeholder="搜索租房"
                  clearable
                  style="width: 400px; margin-right: 10px;"
                  @keyup.enter="onSearch">
          <template #append>
            <el-button type="primary" @click="onSearch" :loading="loading">搜索</el-button>
          </template>
        </el-input>
      </div>
    </el-card>
    <el-card class="table-card " shadow="hover">
      <el-table v-loading="loading" :data="rent_houses"
                height="400"
                style="width: 100%"
                stripe
                border>
        <el-table-column type="index" class="company-table-column"
                         :index="(index)=>(currentPage - 1) * pageSize + index + 1"
                         label="序号" />
        <el-table-column prop="title" label="标题" show-overflow-tooltip min-width="98" />
        <el-table-column prop="address" label="地址" show-overflow-tooltip min-width="100" />
        <el-table-column prop="phone" label="电话" show-overflow-tooltip min-with="*" />
        <el-table-column label="操作" fixed="right">
          <template #default="scope">
            <el-button type="primary" @click="CopyData(scope.row)">复制</el-button>
            <!-- 可以添加更多按钮 -->
          </template>
        </el-table-column>
      </el-table>
      <div class="pagination-container">
        <el-pagination :current-page="currentPage"
                       :page-size="pageSize"
                       :page-sizes="[10, 20, 50, 100]"
                       :total="total"
                       layout="total, sizes, prev, pager, next, jumper"
                       @update:current-page="handleCurrentChange"
                       @update:page-size="handleSizeChange" />
      </div>
    </el-card>
  </div>

</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import api from "../../api"
import { el } from 'element-plus/es/locale';
  export default defineComponent({
    components: {
    },
    directives: {
    },
    filters: {
    },
    props: {
    },
    data() {
      return {
        rent_houses: [],
        searchQuery: "",
        loading: false,
        currentPage: 1,
        pageSize: 10,
        total: 0
      }
    },
    computed: {
    },
    watch: {
    },
    beforeCreate() {
    },
    created() {
      this.getData()
    },
    beforeMount() {
    },
    mounted() {

    },
    updated() {
    },
    activated() {
    },
    deactivated() {
    },
    beforeUnmount() {
    },
    unmounted() {
    },
    methods: {
      async copyToClipboard(text) {
        try {
          // 现代浏览器的 Clipboard API
          if (navigator.clipboard && window.isSecureContext) {
            await navigator.clipboard.writeText(text)
          } else {
            // 兼容旧版浏览器的写法
            const textArea = document.createElement('textarea')
            textArea.value = text
            textArea.style.position = 'fixed'
            textArea.style.left = '-999999px'
            textArea.style.top = '-999999px'
            document.body.appendChild(textArea)
            textArea.focus()
            textArea.select()
            document.execCommand('copy')
            textArea.remove()
          }
          console.log('复制成功:')
        } catch (error) {
          console.error('复制失败:', error)
        }
      },
      async CopyData(rowData) {
        console.log(rowData)
        var str = `${rowData.name},${rowData.address},${rowData.phoneNumber}`
        await this.copyToClipboard(str)
      },

      handleSizeChange(newPageSize) {
        if (newPageSize == this.pageSize)
          return
        if (Number.isInteger(newPageSize)) {
          this.pageSize = newPageSize;
          alert(newPageSize);
        }
        
      },
      handleCurrentChange(newPage) {
        //后端返回当前也设置了,会导致触发该函数，要判断newPage == this.currentPage，
        //要不然连续请求两次api -> this.getData();
        if (newPage == this.currentPage)
          return;
        else {
          this.currentPage = newPage;
          this.getData();
        }
        
      },
      onSearch() { console.log('defined later') },
      async getData() {
        try {
          const result = await api.rent.fetchRentHouses({
            page: this.currentPage,
            pageSize: this.pageSize
          });
          console.log(result)
          // result 现在有完整的类型提示
          this.rent_houses = result.data.data;
          console.log("赋值后总记录："+this.rent_houses)
          this.total = result.data.total;
          console.log("赋值后总页数："+this.total);
          this.currentPage = result.data.currentPage;
          console.log("赋值后当前页："+this.currentPage);
        } catch (error) {
          console.log(error)
        }
      }
    },
  });
</script>

<style>
  .page-container {
    padding: 5;
    background-color: white;
  }
</style>
