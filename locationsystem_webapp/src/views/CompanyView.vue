<template>
  <div>
    <el-page-header class="page-container">
      <template #title>
        <h1>公司信息管理</h1>
      </template>
    </el-page-header>
    <el-card class="toolbar-card" shadow="hover">
      <div class="toolbar">
        <el-input v-model="searchQuery" placeholder="搜索公司名称，地址，电话"
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
      <el-table v-loading="loading" :data="companies"
                height="400"
                style="width: 100%"
                stripe
                border>
        <el-table-column type="index" class="company-table-column"
                         :index="(index)=>(currentPage - 1) * pageSize + index + 1"
                         label="序号"

                         width="58" />
        <el-table-column prop="name" label="公司名" show-overflow-tooltip min-width="78" />
        <el-table-column prop="address" label="地址" show-overflow-tooltip min-width="100%" />
        <el-table-column prop="phoneNumber" label="电话" show-overflow-tooltip min-with="78" />
      </el-table>
      <div class="pagination-container">
        <el-pagination v-model:current-page="currentPage"
                       v-model:page-size="pageSize"
                       :page-sizes="[10, 20, 50, 100]"
                       layout="total, sizes, prev, pager, next, jumper"
                       :total="total"
                       @size-change="handleSizeChange"
                       @current-change="handleCurrentChange"
                       :disabled="loading" />
      </div>
    </el-card>
  </div>

</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import api from "../api"
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
        companies:[],
        searchQuery: "",
        loading: false,
        currentPage: 1,
        pageSize: 100,
        total:0
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
      handleSizeChange() {
        alert("handleSizeChange");
      },
      handleCurrentChange(newPage) {
        this.page = newPage;
        this.getData();
      },
      onSearch() { console.log('defined later') },
      async getData() {
        try {
          const result = await api.company.getCompanies({
            page: this.page,
            pageSize: this.pageSize
          });
          console.log(result)
          // result 现在有完整的类型提示
          this.companies = result.data;
          this.total = result.total;
          this.currentPage = result.currentPage;
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
