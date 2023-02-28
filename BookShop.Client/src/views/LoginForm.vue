<template>
  <form @submit.prevent>
    <h4>login form</h4>
    <my-input class="input" type="text" v-model="user.email" PLACEHOLDER="Enter Email..."/>
    <my-input class="input" type="text" v-model="user.password" PLACEHOLDER="Enter Password..."/>
    <my-button @click="login">Login</my-button>
  </form>
</template>

<script>
import MyButton from "@/component/UI/MyButton.vue";
import MyInput from "@/component/UI/MyInput.vue";

export default {
  components: {MyInput, MyButton},
  data(){
    return{
      user:{
        email: '',
        password: '',
      }
    }
  },
  methods: {
    login(){
      alert("test")
    }
  }
}
</script>

<style scoped>
.input{
  width: 100%;
  border: 1px solid teal;
  padding: 10px 15px;
  margin-top: 15px;
}
</style>