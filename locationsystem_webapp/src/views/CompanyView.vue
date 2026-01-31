<template>
  <div>
    <h1>公司管理</h1>
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
    beforeDestroy() {
    },
    destroyed() {
    },
    methods: {
      async getData() {
        try {
          const result = await api.company.getCompanies({
            page: 1,
            pageSize: 100
          });
          console.log(result)
          // result 现在有完整的类型提示
          this.companies = result.data;
          console.log(`共 ${result.total} 条数据`);
        } catch {

        }
      }
    },
  });
</script>

<style>
</style>
