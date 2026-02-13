// 统一导出所有API接口
import * as company from './company'
import * as rent from './renthouse'
import * as auth from './auth'
import * as permissions from './permissions'
import * as users from './users'
import * as menus from './menus'
const api={
  company,
  rent,
  auth,
  permissions,
  users,
  menus
};

export default api;
