<template>
  <div id="mychart" style="height: 400px;" :loadding="loading"></div>
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
        provice_count: [],
        loading:false
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
      this.getData().then(() => {
        const myChart = echarts.init(document.getElementById('mychart'));
        const option = {
          title: {
            text: '各省份美容院数据统计',
            left: 'center'
          },
          tooltip: {
            trigger: 'axis',
            axisPointer: {
              type: 'shadow'
            }
          },
          xAxis: {
            type: 'category',
            data: this.provices,
            axisLabel: {
              rotate: 45, // 如果省份名称太长，可以旋转45度显示
              interval: 0 // 显示所有标签
            }
          },
          yAxis: {
            type: 'value',
            name: '美容院数量'
          },
          series: [{
            name: '数量',
            type: 'bar',
            data: this.provice_count,
            itemStyle: {
              color: 'green' // 自定义柱状图颜色
            },
            // 添加数据标签显示
            label: {
              show: true,
              position: 'top',
              formatter: '{c}'
            }
          }],
          // 添加滚动条，适合数据较多的情况
          dataZoom: [
            {
              type: 'inside',
              start: 0,
              end: 100
            },
            {
              show: true,
              type: 'slider',
              top: '90%',
              start: 0,
              end: 100
            }
          ]
        };

        myChart.setOption(option);
        window.addEventListener('resize', () => {
          myChart.resize();
        });
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
        var respone = await api.company.getProviceCompany();
        const provinces = respone.proviceConpany.map(item => Object.keys(item)[0]);;
        this.provices = provinces;
        const values = respone.proviceConpany.map(item => Object.values(item)[0]);
        this.provice_count = values;
      }
    },
  });
</script>

<style>
</style>
