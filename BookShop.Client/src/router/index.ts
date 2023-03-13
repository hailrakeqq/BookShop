import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import RegisterForm from '../views/RegisterForm.vue'
import LoginForm from '../views/LoginForm.vue'

//seller rout
import BookConstuctor from '../views/seller-views/BookConstructor.vue'
import Stock from '../views/seller-views/Stock.vue'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
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
    {
      path: '/changeBookData/:bookId',
      name: 'changebookdata',
      component: BookConstuctor
    },
    {
      path: '/bookconstructor',
      name: 'bookconstructor',
      component: BookConstuctor
    },
    {
      path: '/stock',
      name: 'stock',
      component: Stock
    }
  ]
})

export default router
