// 测试日期时间选择器功能

// 模拟当前时间
const currentTime = new Date();
console.log('当前时间:', currentTime);

// 模拟选择日期时的默认时间（00:00:00）
const selectedDate = new Date('2025-12-20T00:00:00.000Z');
console.log('选择日期时的默认时间:', selectedDate);

// 测试开始时间处理逻辑
if (selectedDate.getHours() === 0 && selectedDate.getMinutes() === 0 && selectedDate.getSeconds() === 0) {
    // 将时间部分设置为当前时间的时分秒
    selectedDate.setHours(currentTime.getHours(), currentTime.getMinutes(), currentTime.getSeconds());
    console.log('处理后的开始时间:', selectedDate);
    console.log('时分秒是否为当前时间:', 
        selectedDate.getHours() === currentTime.getHours() &&
        selectedDate.getMinutes() === currentTime.getMinutes() &&
        selectedDate.getSeconds() === currentTime.getSeconds()
    );
}

// 测试结束时间处理逻辑
const startTime = new Date(selectedDate);
const endDate = new Date('2025-12-20T00:00:00.000Z');
console.log('结束时间选择时的默认时间:', endDate);

if (endDate.getHours() === 0 && endDate.getMinutes() === 0 && endDate.getSeconds() === 0) {
    // 将结束时间设置为开始时间后30分钟
    endDate.setTime(startTime.getTime() + 30 * 60 * 1000);
    console.log('处理后的结束时间:', endDate);
    console.log('结束时间是否为开始时间后30分钟:', 
        endDate.getTime() === startTime.getTime() + 30 * 60 * 1000
    );
}

// 测试边界情况：选择今天且时间小于当前时间
const todayDate = new Date();
todayDate.setHours(10, 0, 0); // 设置为今天上午10点
console.log('\n边界测试 - 选择今天上午10点:', todayDate);

if (todayDate.toDateString() === currentTime.toDateString() && todayDate <= currentTime) {
    todayDate.setHours(currentTime.getHours(), currentTime.getMinutes(), currentTime.getSeconds());
    console.log('处理后的时间（今天且小于当前时间）:', todayDate);
    console.log('时间是否已调整为当前时间:', 
        todayDate.getHours() === currentTime.getHours() &&
        todayDate.getMinutes() === currentTime.getMinutes() &&
        todayDate.getSeconds() === currentTime.getSeconds()
    );
}
