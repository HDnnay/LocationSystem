<template>
  <div id="mychart" style="height: 400px;"></div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import api from "../../api";
  import * as echarts from 'echarts';

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
        provices: [],

      }
    },
    computed: {
    },
    watch: {
    },
    beforeCreate() {
    },
    created() {

    },
    beforeMount() {
    },
    mounted() {
      this.getData().then(res=>{
        console.log("请求数据：");
        console.log(res);
        const myChart = echarts.init(document.getElementById('mychart'));
        const option = {
              xAxis: { type: 'category', data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'] },
              yAxis: { type: 'value' },
              series: [{ data: [120, 200, 150, 80, 70, 110, 130], type: 'bar' }]
        };
        myChart.setOption(option);
      })

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
        var respone =await api.company.getProviceCompany();
        console.log(respone)
      }
    },
  });
</script>

<style>
</style>
