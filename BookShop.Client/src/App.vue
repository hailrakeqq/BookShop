<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <div class="wrapper">
      <nav>
        <RouterLink class="item" to="/">Home</RouterLink>
        <RouterLink class="item" to="/topSellerList">Top Seller List</RouterLink>
        <RouterLink class="selectedList" v-if="!jwtToken" to="/login">Login</RouterLink>
        <div class="selectedList" v-else-if="role === 'user'">
          <my-select v-model="selectedSort" :disabledValue="username" :options="sortOptionsForUser" ></my-select>
        </div>
        <div class="selectedList" v-else-if="role === 'seller'">
          <my-select v-model="selectedSort" :disabledValue="username" :options="sortOptionsForSeller" ></my-select>
        </div>
      </nav>
    </div>
  </header>
  <RouterView />
</template>
<script lang="ts">
import MySelect from "@/component/UI/MySelect.vue";
import './router'
import axios from 'axios'
  export default {
  components: {
    MySelect
  },
    data(){
      return {
        email: localStorage.getItem("email"),
        id: localStorage.getItem("id"),
        jwtToken: localStorage.getItem("accessToken"),
        role: localStorage.getItem("role"),
        username: localStorage.getItem("username"),
        selectedSort: '',
        sortOptionsForUser: [
          {value: 'account', name: 'Account'},
          {value: 'library', name: 'Library'},
          {value: 'wishlist', name: 'Wishlist'},
          {value: 'settings', name: 'Settings'},
          {value: 'logout', name: 'Logout'},
        ],
        sortOptionsForSeller: [
          {value: 'account', name: 'Account'},
          {value: 'addbook', name: 'Add book'},
          {value: 'stock', name: 'Stock'},
          {value: 'settings', name: 'Settings'},
          {value: 'logout', name: 'Logout'},
        ]
      }
    },
    methods: {
      logout(){
        localStorage.clear()
        this.$router.go()
        this.$router.push('/')
      },
      redirectToAccount(){
        this.$router.push(`/user/${this.id}`)
      },
      redirectToSettings(){
        this.$router.push('/settings')
      },
      redirectToBookConstructor(){
        if(this.role === 'seller')
          this.$router.push('/bookconstructor')
        else 
          alert('You haven\'t access to this page') //in future create forbidden page
      },
      redirectToStock(){
        if(this.role === 'seller')
          this.$router.push('/stock')
        else
          alert('You haven\'t access to this page') //in future create forbidden page
      },
      redirectToWishist(){
        if(this.role === 'user')
          this.$router.push({name: 'wishlist', params: {userid: this.id}})
        else
          alert('You haven\'t access to this page') //in future create forbidden page
      },
      redirectToLibrary(){
        if(this.role === 'user')
          this.$router.push({name: 'library', params: {userid: this.id}})
        else
          alert('You haven\'t access to this page') //in future create forbidden page
      }
    },
    watch: {
      selectedSort(newValue){
        const action = newValue
        
        switch (action) {
          case ('logout'):{
            this.logout();
            break;
          }
          case ('account'):{
            this.redirectToAccount();
            break;
          }
          case ('settings'):{
            this.redirectToSettings();
            break;
          }
          case ('addbook'):{
            this.redirectToBookConstructor();
            break;
          }
          case ('stock'):{
            this.redirectToStock();
            break;
          }
          case ('wishlist'):{
            this.redirectToWishist();
            break;
          }
          case ('library'):{
            this.redirectToLibrary();
            break;
          }
        }
      }
    }
  }
</script>
<style scoped>
.wrapper{
  background-color: #f7f7f7;
  border: 1px solid #ddd;
  border-radius: 5px;
  box-shadow: 0 2px 2px rgba(0, 0, 0, 0.3);
  padding: 20px;
  margin: 20px 0;
  display: flex;
  flex-direction: column;
}
.item{
  padding-left: 30px;
}
.selectedList{
  margin-right: 25px;
  display: inline-block;
  float: right;
}
</style>
