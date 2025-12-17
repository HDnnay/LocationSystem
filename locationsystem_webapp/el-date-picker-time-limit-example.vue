<template>
  <div class="date-time-example">
    <h2>el-date-picker 时间限制示例</h2>
    
    <el-form label-width="120px">
      <el-form-item label="开始时间">
        <el-date-picker
          v-model="formData.startDate"
          type="datetime"
          placeholder="选择开始时间"
          style="width: 100%"
          format="YYYY-MM-DD HH:mm:ss"
          value-format="YYYY-MM-DD HH:mm:ss"
          :disabled-date="disabledStartDate"
          :disabled-time="disabledStartTime"
          @change="handleStartTimeChange" />
      </el-form-item>
      
      <el-form-item label="结束时间">
        <el-date-picker
          v-model="formData.endDate"
          type="datetime"
          placeholder="选择结束时间"
          style="width: 100%"
          format="YYYY-MM-DD HH:mm:ss"
          value-format="YYYY-MM-DD HH:mm:ss"
          :disabled-date="disabledEndDate"
          :disabled-time="disabledEndTime"
          :disabled="!formData.startDate"
          @change="handleEndTimeChange" />
      </el-form-item>
    </el-form>
  </div>
</template>

<script setup>
import { ref, reactive } from 'vue'

// 表单数据
const formData = reactive({
  startDate: '',
  endDate: ''
})

// 禁用开始日期（只能选择今天及以后）
const disabledStartDate = (time) => {
  const today = new Date()
  today.setHours(0, 0, 0, 0)
  return time.getTime() < today.getTime()
}

// 禁用开始时间（如果是今天，只能选择当前时间及以后）
const disabledStartTime = (date) => {
  const today = new Date()
  const selectedDate = new Date(date)

  // 如果选择的不是今天，不限制时间
  if (selectedDate.toDateString() !== today.toDateString()) {
    return { disabledHours: () => [], disabledMinutes: () => [], disabledSeconds: () => [] }
  }

  // 如果是今天，限制小时、分钟、秒
  const currentHour = today.getHours()
  const currentMinute = today.getMinutes()
  const currentSecond = today.getSeconds()

  return {
    disabledHours: () => {
      // 禁用当前小时之前的所有小时
      return Array.from({ length: currentHour }, (_, i) => i)
    },
    disabledMinutes: (selectedHour) => {
      if (selectedHour < currentHour) {
        // 如果选择的小时已经过去，禁用所有分钟
        return Array.from({ length: 60 }, (_, i) => i)
      }
      if (selectedHour > currentHour) {
        // 如果选择的小时在未来，不禁用分钟
        return []
      }
      // 如果选择的是当前小时，禁用当前分钟之前的所有分钟
      return Array.from({ length: currentMinute }, (_, i) => i)
    },
    disabledSeconds: (selectedHour, selectedMinute) => {
      if (selectedHour < currentHour || selectedMinute < currentMinute) {
        // 如果选择的小时或分钟已经过去，禁用所有秒
        return Array.from({ length: 60 }, (_, i) => i)
      }
      if (selectedHour > currentHour || selectedMinute > currentMinute) {
        // 如果选择的小时或分钟在未来，不禁用秒
        return []
      }
      // 如果选择的是当前小时和当前分钟，禁用当前秒之前的所有秒
      return Array.from({ length: currentSecond }, (_, i) => i)
    }
  }
}

// 禁用结束日期（只能选择开始日期及以后）
const disabledEndDate = (time) => {
  if (!formData.startDate) return false
  const startDate = new Date(formData.startDate)
  startDate.setHours(0, 0, 0, 0)
  return time.getTime() < startDate.getTime()
}

// 禁用结束时间（如果与开始日期相同，只能选择开始时间及以后）
const disabledEndTime = (date) => {
  if (!formData.startDate) return { disabledHours: () => [], disabledMinutes: () => [], disabledSeconds: () => [] }

  const selectedDate = new Date(date)
  const startDateTime = new Date(formData.startDate)
  const startDate = new Date(startDateTime)
  startDate.setHours(0, 0, 0, 0)

  // 如果选择的日期与开始日期不同，不限制时间
  if (selectedDate.toDateString() !== startDate.toDateString()) {
    return { disabledHours: () => [], disabledMinutes: () => [], disabledSeconds: () => [] }
  }

  // 如果选择的日期与开始日期相同，限制时间
  const startHour = startDateTime.getHours()
  const startMinute = startDateTime.getMinutes()
  const startSecond = startDateTime.getSeconds()

  return {
    disabledHours: () => {
      // 禁用开始时间小时之前的所有小时
      return Array.from({ length: startHour }, (_, i) => i)
    },
    disabledMinutes: (selectedHour) => {
      if (selectedHour < startHour) {
        // 如果选择的小时早于开始时间的小时，禁用所有分钟
        return Array.from({ length: 60 }, (_, i) => i)
      }
      if (selectedHour > startHour) {
        // 如果选择的小时晚于开始时间的小时，不禁用分钟
        return []
      }
      // 如果选择的是开始时间的小时，禁用开始分钟之前的所有分钟
      return Array.from({ length: startMinute }, (_, i) => i)
    },
    disabledSeconds: (selectedHour, selectedMinute) => {
      if (selectedHour < startHour || selectedMinute < startMinute) {
        // 如果选择的小时或分钟早于开始时间的小时或分钟，禁用所有秒
        return Array.from({ length: 60 }, (_, i) => i)
      }
      if (selectedHour > startHour || selectedMinute > startMinute) {
        // 如果选择的小时或分钟晚于开始时间的小时或分钟，不禁用秒
        return []
      }
      // 如果选择的是开始时间的小时和分钟，禁用开始秒之前的所有秒
      return Array.from({ length: startSecond }, (_, i) => i)
    }
  }
}

// 处理开始时间变化
const handleStartTimeChange = (value) => {
  if (!value) return

  let selectedDate = new Date(value)
  const currentTime = new Date()

  // 检查时间部分是否为默认的00:00:00
  if (selectedDate.getHours() === 0 && selectedDate.getMinutes() === 0 && selectedDate.getSeconds() === 0) {
    // 将时间部分设置为当前时间的时分秒
    selectedDate.setHours(currentTime.getHours(), currentTime.getMinutes(), currentTime.getSeconds())
    formData.startDate = selectedDate.toLocaleString('en-CA', { 
      year: 'numeric', 
      month: '2-digit', 
      day: '2-digit', 
      hour: '2-digit', 
      minute: '2-digit', 
      second: '2-digit', 
      hour12: false 
    })
  }

  // 如果选择的是今天且时间小于当前时间，自动调整为当前时间
  if (selectedDate.toDateString() === currentTime.toDateString() && selectedDate <= currentTime) {
    selectedDate.setHours(currentTime.getHours(), currentTime.getMinutes(), currentTime.getSeconds())
    formData.startDate = selectedDate.toLocaleString('en-CA', { 
      year: 'numeric', 
      month: '2-digit', 
      day: '2-digit', 
      hour: '2-digit', 
      minute: '2-digit', 
      second: '2-digit', 
      hour12: false 
    })
  }

  // 将结束时间设置为开始时间后30分钟
  let updatedStartTime = new Date(formData.startDate)
  let newEndTime = new Date(updatedStartTime)
  newEndTime.setMinutes(newEndTime.getMinutes() + 30)
  formData.endDate = newEndTime.toLocaleString('en-CA', { 
    year: 'numeric', 
    month: '2-digit', 
    day: '2-digit', 
    hour: '2-digit', 
    minute: '2-digit', 
    second: '2-digit', 
    hour12: false 
  })
}

// 处理结束时间变化
const handleEndTimeChange = (value) => {
  if (!value || !formData.startDate) return

  const selectedDate = new Date(value)
  const startTime = new Date(formData.startDate)

  // 检查时间部分是否为默认的00:00:00
  if (selectedDate.getHours() === 0 && selectedDate.getMinutes() === 0 && selectedDate.getSeconds() === 0) {
    // 将结束时间设置为开始时间后30分钟
    const endTime = new Date(startTime)
    endTime.setMinutes(endTime.getMinutes() + 30)
    formData.endDate = endTime.toLocaleString('en-CA', { 
      year: 'numeric', 
      month: '2-digit', 
      day: '2-digit', 
      hour: '2-digit', 
      minute: '2-digit', 
      second: '2-digit', 
      hour12: false 
    })
  } else {
    // 确保结束时间不早于开始时间
    const endTime = new Date(value)
    if (endTime <= startTime) {
      const adjustedEndTime = new Date(startTime)
      adjustedEndTime.setMinutes(adjustedEndTime.getMinutes() + 30)
      formData.endDate = adjustedEndTime.toLocaleString('en-CA', { 
        year: 'numeric', 
        month: '2-digit', 
        day: '2-digit', 
        hour: '2-digit', 
        minute: '2-digit', 
        second: '2-digit', 
        hour12: false 
      })
    }
  }
}
</script>

<style scoped>
.date-time-example {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
}

.el-form-item {
  margin-bottom: 20px;
}
</style>