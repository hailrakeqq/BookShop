<template>
  <div class="app">
    <h3>Page with books</h3>
    <div class="app_button" v-if="userRole === 'seller'">
      <my-button @click="redirectToBookConstructor">Add book</my-button>
    </div>
    <Booklist :books="books"  v-if="!isBookLoading"/>
    <div v-else>Loading...</div>
  </div>
</template>

<script lang="ts">
import Booklist from "@/component/Booklist.vue";
import {Book} from "@/component/book"
import MyDialog from "@/component/UI/MyDialog.vue";
import MyButton from "@/component/UI/MyButton.vue";
import axios from "axios";
import MySelect from "@/component/UI/MySelect.vue";
import {getImagesHashMapFromServer} from '../scripts/getImagesHashMapFromServer'
export default {
  components: {
    MySelect,
    MyButton,
    MyDialog,
    Booklist, Book
  },
  name: 'Home',
  data(){
    return{
      books: [],
      images: Map,
      userRole: localStorage.getItem('role'),
      dialogVisible: false,
      isBookLoading: false,
      selectedSort: '',
      sortOptions: [
        {value: 'title', name: 'By name'},
        {value: 'body', name: 'By body'},
      ]
    }
  },
  methods:{
    redirectToBookConstructor(){
      this.$router.push('/bookconstructor')
    },
    async getBookList(){
      try{
        this.isBookLoading = true
        const response = await axios.get('http://localhost:5045/api/Book')
        this.books = response.data
      } catch (e) {
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
      this.getBookList()
      getImagesHashMapFromServer()
          .then(images => {this.images = images})
          .then(() => this.findCoverForBookByBookTitle());
    }
  },
  mounted() {
    this.loadBookAndImages();
  }
}
</script>

<style>
.app_button{
  margin-top: 15px;
  display: flex;
  justify-content: space-between;
}
.app{
  padding: 20px;
}
</style>