<template>
  <div v-if="books.length > 0">
    <h3>Book list</h3>
    <Bookcomponent v-for="book in books" :book="book" :key="book.id" @remove="$emit('remove', book)"/>
  </div>
  <h2 v-else style="color: red">Book list is empty</h2>
</template>

<script lang="ts">
import Bookcomponent from "@/component/Bookcomponent.vue";
export default {
  components: {Bookcomponent},
  props: {
    books: {
      type: Array,
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
}
</style>