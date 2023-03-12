<template>
  <div class="main" v-if="books.length > 0">
    <BookItem 
        class="book" 
        v-for="book in books"
        :book="book"
        :key="book.id"/>
  </div>
  <h2 v-else style="color: red">Book list is empty</h2>
</template>

<script lang="ts">
import BookItem from "@/component/BookItem.vue";
export default {
  components: {BookItem},
  props: {
    books: {
      type: Array,
      required: true
    },
    images: {
      type: Map,
      required: true
    }
  },
}
</script>

<style scoped>
.main{
  /*padding-left: 300px;*/
  padding: 50px;
  flex: 1;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: center;
}
</style>