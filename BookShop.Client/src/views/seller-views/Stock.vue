<template>
  <h1>My test book stock</h1>
  <booklist :books="books" :images="images" v-if="!isBookLoading"></booklist>
  <div v-else>Loading...</div>

</template>

<script lang="ts">
import Booklist from "@/component/Booklist.vue";
import {getImagesArrayFromServer} from '../../scripts/getImagesArrayFromServer'
import {getBookCollectionBySellerId} from '../../scripts/BookService'
export default {
  name: "Stock",
  components:{
    Booklist
  },
  data(){
    return{
      books: [],
      images: Map,
      isBookLoading: false,
    }
  },
  methods:{
    async getBookCollection(){
      const response = await getBookCollectionBySellerId(localStorage.getItem('id'))
      this.books = response
    },
    findCoverForBookByBookTitle(){
      this.books.forEach(book => {
        for (const [key, value] of this.images.entries()) {
          if (key === book.title) 
            book.imageLink = value;
        }
      })
    },
    loadBookAndImages(){
      this.getBookCollection();
      getImagesArrayFromServer()
          .then(images => {this.images = images})
          .then(() => this.findCoverForBookByBookTitle());
    }
  },
  mounted() {
    this.loadBookAndImages();
  }
}
</script>

<style scoped>

</style>