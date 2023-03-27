<template>
  <div class="main" v-if="books.length > 0">
    <BookItem 
        class="book" 
        v-for="book in books"
        :isWishlistPage="isWishListPage"
        :book="book"
        :key="book.id"/>
  </div>
  <h2 v-else style="color: red">This book list is empty.</h2>
</template>

<script lang="ts">
import MyButton from "@/component/UI/MyButton.vue";
import BookItem from "@/component/BookItem.vue";
export default {
  components: {BookItem, MyButton},
  props: {
    books: {
      type: Array,
      required: true
    },
    isWishListPage:{
      type: Boolean,
      required: false
    },
    images: {
      type: Map,
      required: true
    }
  },
  methods:{
    redirectToCreateBookPage(){
      this.$router.push('/bookconstructor')
    }
  }
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