<template>
  <div class="main">
    <h1>My Wishlist</h1>
    <booklist :books="books" :isWishListPage="isWishlistPage"></booklist>
  </div>
</template>

<script>
import {getBookCollectionFromAPIEndpointAndId} from '../../scripts/BookService'
import booklist from '@/component/Booklist.vue'
import {getImagesHashMapFromServer} from '../../scripts/getImagesHashMapFromServer'
export default {
  name: "Wishlist",
  components:{
    booklist
  },
  data(){
    return{
      books: [],
      isWishlistPage: true
    }
  },
  methods:{
    loadBookAndImages(){
      this.getUserBookCollection();
      getImagesHashMapFromServer()
          .then(images => {this.images = images})
          .then(() => this.findCoverForBookByBookTitle());
    },
    async getUserBookCollection(){
      const response = await getBookCollectionFromAPIEndpointAndId(
          "http://localhost:5045/api/User/GetUserWishlist",
          localStorage.getItem('id')
      )
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
  },
  mounted(){
    this.loadBookAndImages()
  }
}
</script>

<style scoped>
.main{
  padding: 50px;
}
</style>