<template>
  <form @submit.prevent>
    <h4>Add book</h4>
    <my-input class="input" type="text" v-model="book.title" PLACEHOLDER="Title"/>
    <my-input class="input" type="text" v-model="book.description" PLACEHOLDER="Description"/>
    <my-button @click="createBook">Add book</my-button>
  </form>
</template>

<script>
import MyButton from "@/component/UI/MyButton.vue";
import MyInput from "@/component/UI/MyInput.vue";

export default {
  components: {MyInput, MyButton},
  data(){
    return{
      book:{
        title: '',
        description: '',
      }
    }
  },
  methods: {
    createBook(){
      this.book.id = Date.now();
      this.$emit('create', this.book)
      this.book = {
        title: '',
        description: ''
      }
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