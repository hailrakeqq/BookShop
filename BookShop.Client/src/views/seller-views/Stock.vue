<template>
  <h1>My test book stock</h1>
  <booklist :books="books" :images="images" v-if="!isBookLoading"></booklist>
  <div v-else>Loading...</div>

</template>

<script lang="ts">
import Booklist from "@/component/Booklist.vue";
import {getImagesArrayFromServer} from '../../scripts/getImagesArrayFromServer'

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
    async getBookCollectionBySellerId(id){
      try{
        this.isBookLoading = true
        await fetch(`http://localhost:5045/api/Book/GetSellerBookCollection/${id}`,{
          method: "GET",
          headers:{
            'Content-Type': 'application/json'
          }
        }).then(data => data.json()).then(data => this.books = data)
      } catch (e) {
        if(e.message == "JSON.parse: unexpected end of data at line 1 column 1 of the JSON data")
          return
        alert("error")
      } finally {
        this.isBookLoading = false
      }
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
      this.getBookCollectionBySellerId(localStorage.getItem('id'));
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