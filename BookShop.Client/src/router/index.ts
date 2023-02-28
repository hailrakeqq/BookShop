import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import RegisterForm from '../views/RegisterForm.vue'
import LoginForm from '../views/LoginForm.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/login',
      name: 'login',
      component: LoginForm
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterForm
    },
  ]
})

export default router
