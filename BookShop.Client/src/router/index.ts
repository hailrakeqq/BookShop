import { createRouter, createWebHistory } from 'vue-router'

import HomeView from '../views/HomeView.vue'
import RegisterForm from '../views/RegisterForm.vue'
import LoginForm from '../views/LoginForm.vue'
import BookPage from '../views/BookPage.vue'
import AccountPage from '../views/AccountPage.vue'
import SettingsPage from '../views/SettingsPage.vue'

//user rout
import Library from '../views/user-views/Library.vue'
import Wishlist from '../views/user-views/Wishlist.vue'

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
      path: '/book/:id',
      name: 'book',
      component: BookPage
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
    },
    {
      path: '/wishlist/:userid',
      name: 'wishlist',
      component: Wishlist
    },
    {
      path: '/library/:userid',
      name: 'library',
      component: Library
    },
    {
      path: '/user/:id',
      name: 'user',
      component: AccountPage
    },
    {
      path: '/settings',
      name: 'settings',
      component: SettingsPage
    }
  ]
})

export default router
