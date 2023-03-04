<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <div class="wrapper">
      <nav>
        <RouterLink to="/">Home</RouterLink>
        <RouterLink v-if="!jwtToken" to="/login">Login</RouterLink>
        <a @click="logout" v-else>Logout</a>
        <div id="user-header" v-if="role == 'user'">
          <RouterLink to="/wishlist">My Wishlist</RouterLink>
          <RouterLink to="/library">My Library</RouterLink>
        </div>
        <div id="seller-header" v-else-if="role == 'seller'">
          <RouterLink to="/stock">My Stock</RouterLink>
          <RouterLink to="/bookconstructor">Add Book</RouterLink>
        </div>
      </nav>
    </div>
  </header>
  <RouterView />
</template>
<script lang="ts">
import './router'
  export default {
    data(){
      return {
        email: localStorage.getItem("email"),
        id: localStorage.getItem("id"),
        jwtToken: localStorage.getItem("jwtToken"),
        role: localStorage.getItem("role"),
        username: localStorage.getItem("username")
      }
    },
    computed:{
    },
    methods: {
      logout(){
        localStorage.clear()
        this.$router.go()
        this.$router.push('/')
      }
    }
  }
</script>
<style scoped>

</style>
