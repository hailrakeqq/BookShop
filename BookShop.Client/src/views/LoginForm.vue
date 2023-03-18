<script setup lang="ts">
import {RouterLink} from 'vue-router'
</script>

<template>
  <MyForm @submit.prevent>
    <h4>login form</h4>
    <p>
      Login:
        <my-input class="input" type="text" v-model="emailOrName" PLACEHOLDER="Enter Email or Username..."/>
    </p>
    <p>
      Password:
        <my-input class="input" type="password" v-model="password" PLACEHOLDER="Enter Password..."/>
    </p>
    <p class="message">Not registered? 
      <RouterLink class="link-to-register" to="/register">Create an account</RouterLink></p>
    <my-button @click="login">Login</my-button>
  </MyForm>
</template>

<script lang="ts">
import MyButton from "@/component/UI/MyButton.vue";
import MyInput from "@/component/UI/MyInput.vue";
import MyForm from "@/component/Form.vue"
import '../router'
import axios from 'axios'
export default {
  name: 'LoginForm',
  components: {MyInput, MyButton, MyForm},
  data(){
    return{
      emailOrName: '',
      password: '',
    }
  },
  methods: {
    isFormValid(): boolean{
      return (this.emailOrName != null && this.emailOrName != '') && (this.password != null && this.password != '')
    },
    saveUserDataToLocalStorage(userDataFromResponse: object): void{
      const map = new Map(Object.entries(userDataFromResponse));
      map.forEach((value, key) => localStorage.setItem(key, value));
    },
    async login(){
      if(this.isFormValid){
        const userData ={
          emailOrLogin: this.emailOrName,
          password: this.password
        }
        await axios.post('http://localhost:5045/api/Auth/Login', JSON.stringify(userData), {
          headers:{
            'content-type': 'application/json'
          }
        }).then(Response => {
              if(Response.status === 200){
                this.saveUserDataToLocalStorage(Response.data)
                setTimeout(() => this.$router.go(), 100)
                this.$router.push('/')
              }
              else{
                alert("User was not found.")
              }
            })
      }
    }
  }
}
</script>

<style scoped>
.input{
  width: 30%;
  border: 1px solid teal;
  padding: 10px 15px;
  margin-top: 15px;
}
.link-to-register{
  color: lightgreen;
  text-decoration: none;
}
</style>