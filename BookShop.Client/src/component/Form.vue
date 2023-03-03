<template>
  <form @submit.prevent>
    <slot></slot>
  </form>
</template>

<script>
import MyButton from "@/component/UI/MyButton.vue";
import MyInput from "@/component/UI/MyInput.vue";

export default {
  components: {MyInput, MyButton},
  data(){
    return{
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
  width: 50%;
  border: 1px solid teal;
  padding: 10px 15px;
  margin-top: 15px;
}
</style>