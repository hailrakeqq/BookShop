<template>
  <form @submit.prevent>
    <h4>Registration form</h4>
    <my-input class="input" type="text" v-model="book.title" PLACEHOLDER="Title"/>
    <my-input class="input" type="text" v-model="book.description" PLACEHOLDER="Description"/>
    <my-button @click="createUser">Register</my-button>
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
        username: '',
        password: '',
        confirmPassword: '',
      }
    }
  },
  methods: {
    createUser(){
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