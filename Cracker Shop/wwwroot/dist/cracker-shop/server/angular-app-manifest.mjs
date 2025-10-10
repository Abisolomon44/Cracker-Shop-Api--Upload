
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 2,
    "redirectTo": "/login",
    "route": "/"
  },
  {
    "renderMode": 2,
    "route": "/login"
  },
  {
    "renderMode": 2,
    "redirectTo": "/default/master/company",
    "route": "/default"
  },
  {
    "renderMode": 2,
    "route": "/default/master/company"
  },
  {
    "renderMode": 2,
    "route": "/default/master/branch"
  },
  {
    "renderMode": 2,
    "route": "/default/master/department"
  },
  {
    "renderMode": 2,
    "route": "/default/master/role"
  },
  {
    "renderMode": 2,
    "route": "/default/master/user"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 11606, hash: '010c3f86f9cd445ebfae1f4c2356c03d9ba0f59cff73fc0b5ef3c9c673afdd31', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 6662, hash: 'b809136fc562b6b7065a18044405b41233a7c8ba12118f1f4a200d9f0e869a8b', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'login/index.html': {size: 14888, hash: 'fe2eb05140b3fca9327ec4ad9b474746541276f218c14cce9cfcda7e4f88ab65', text: () => import('./assets-chunks/login_index_html.mjs').then(m => m.default)},
    'default/master/company/index.html': {size: 28397, hash: '607f7f4727b58bb99b52a472ae95678fa4a87c83ff748b33e5ae7e2e892fc95e', text: () => import('./assets-chunks/default_master_company_index_html.mjs').then(m => m.default)},
    'default/master/role/index.html': {size: 22110, hash: '9466f26acd0fe05e1d6cb8ddaef1529ad00048c5a4d77e440a1d19b70a03d05d', text: () => import('./assets-chunks/default_master_role_index_html.mjs').then(m => m.default)},
    'default/master/branch/index.html': {size: 23078, hash: '12e723fc2ee3a2b66520fa40fdd88f980656413e2ed0a211ff82dd071f79e23b', text: () => import('./assets-chunks/default_master_branch_index_html.mjs').then(m => m.default)},
    'default/master/department/index.html': {size: 23066, hash: 'd1ce73d52fb9231b166227f67ecc9232b5ad164720e4facd4dcf7d5556a286ca', text: () => import('./assets-chunks/default_master_department_index_html.mjs').then(m => m.default)},
    'default/master/user/index.html': {size: 22975, hash: 'a88306dea0cdd433e06154bcef30a7dd87611d13d267d2cdb88b49289e820e5b', text: () => import('./assets-chunks/default_master_user_index_html.mjs').then(m => m.default)},
    'styles-RDD5KNLM.css': {size: 6999, hash: 'pRTij/H3nC8', text: () => import('./assets-chunks/styles-RDD5KNLM_css.mjs').then(m => m.default)}
  },
};
