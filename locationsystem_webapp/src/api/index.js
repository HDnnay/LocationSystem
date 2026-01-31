// 统一导出所有API接口
import * as dentists from './dentists'
import * as patients from './patients'
import * as appointments from './appointments'
import * as dentalOffices from './dentalOffices'
import * as company from './company'

const api={
  dentists,
  patients,
  appointments,
  dentalOffices,
  company
};

export default api;
