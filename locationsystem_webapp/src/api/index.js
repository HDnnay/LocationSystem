// 统一导出所有API接口
import * as dentists from './dentists'
import * as patients from './patients'
import * as appointments from './appointments'
import * as dentalOffices from './dentalOffices'
import * as company from './company'
import * as rent from './renthouse'
import * as auth from './auth'
const api={
  dentists,
  patients,
  appointments,
  dentalOffices,
  company,
  rent,
  auth
};

export default api;
