<template>
  <div class="book">
    <div>
      <h4>{{book.title}}</h4>
      <p>{{book.body}}</p>
    </div>
    <div>
      <my-button @click="$emit('remove', book)">Delete</my-button>
    </div>
  </div>
</template>

<script>
import MyButton from "@/component/UI/MyButton.vue";
import {Book} from "@/component/book";

export default {
name: "bookcomponent",
  components: {MyButton},
  props: {
    book:{
      type: Book,
      required: true
    }
  }
}
</script>

<style scoped>
.book{
  padding: 15px;
  border: 2px solid teal;
  margin-top: 15px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}
</style>