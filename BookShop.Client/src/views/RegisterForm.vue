<script setup lang="ts">
import {RouterLink} from 'vue-router'
</script>

<template>
  <my-form @submit.prevent>
    <h4>Registration form</h4>
    <p>
      Email:
        <my-input class="input" type="email" v-model="email"/>
        <br/>
        <span v-if="!isEmailValid">Please enter email.</span>
    </p>
    <p>
      Username:
        <my-input class="input" type="text" v-model="username"/>
        <br/>
        <span v-if="!isUsernameValid">Please enter username.</span>
    </p>
    <p>
      Password:
        <my-input class="input" type="password" v-model="password"/>
        <br/>
        <span v-if="!isPasswordValid">Please enter password which have `lenght > 5, one upper character, number`.</span>
    </p>
    <p>
      Confirm Password:
        <my-input class="input" type="password" v-model="confirmPassword"/>
        <br/>
        <span v-if="!isConfirmPasswordEqualToPassword">Passwords do not match</span>
    </p>
    
    <input type="radio" value="user" v-model="role">
    <label>User</label>
    <br/>
    <input type="radio" value="seller" v-model="role">
    <label>Seller</label>
    <br/>
    <span v-if="!isRoleValid">You must choose role</span>
    <br/>
    
    <p class="message">Already registered?
      <RouterLink class="link-to-login" to="/login" >Sign In</RouterLink>
    </p> 
       
    <my-button @click="createUser">Register</my-button>
  </my-form>
</template>

<script lang="ts">
import MyButton from "@/component/UI/MyButton.vue";
import MyInput from "@/component/UI/MyInput.vue";
import MyForm from "@/component/Form.vue"
import '../router'
import axios from 'axios'
export default {
  components: {MyInput, MyButton, MyForm},
  data(){
    return{
        email: '',
        username: '',
        password: '',
        confirmPassword: '',
        role: ''
    }
  },
  computed:{
    isEmailValid(): boolean {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      return emailRegex.test(this.email);
    },
    isUsernameValid(): boolean {
      return !!this.username.trim();
    },
    isPasswordValid(): boolean {
      const containsUppercase = /[A-Z]/.test(this.password)
      const containsLowercase = /[a-z]/.test(this.password)
      const containsNumber = /[0-9]/.test(this.password)
      const passwordLength = this.password.length > 5
      return containsUppercase && containsLowercase && containsNumber && passwordLength
    },
    isConfirmPasswordEqualToPassword(): boolean {
      return this.password == this.confirmPassword;
    },
    isRoleValid(): boolean{
      return this.role != '' && this.role != null;
    }
  },
  methods: {
    checkIfFormValid(): boolean{
      if(this.isEmailValid && this.isUsernameValid && this.isConfirmPasswordEqualToPassword && this.isRoleValid)
        return true
      return false
    },
    async createUser(){
      if(this.checkIfFormValid())
      {
        const user = {
          Username: this.username,
          Email: this.email,
          Password: this.password,
          Role: this.role
        }
        await axios.post('http://localhost:5045/api/Auth/Registration', JSON.stringify(user), {
          headers: {
            'Content-Type': 'application/json'
          }
        }).then(response => {
              if(response.status === 200){
                alert("You successfully create account. Redirect to login.")
                this.$router.push({path: '/login'})
              } else {
                  alert("Error. Account with these name or email already exist")
              }
            })
      }
      else alert("Error")
    }
  }
}
</script>

<style scoped>
.link-to-login{
  color: #53ff72;
  text-decoration: none;
}
</style>
