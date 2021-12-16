
const routes = [
  { path: '/', component: () => import('pages/Dispatcher') },
  { path: '/login', component: () => import('pages/Login') },
  {
    path: '/layout',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '', component: () => import('pages/Index.vue') }
    ]
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/Error404.vue')
  }
]

export default routes
