<template>
  <div class="app">
    <h3>Page with books</h3>
    <div class="app_button">
      <my-button @click="showDialog">Add book</my-button>
      <my-select v-model="selectedSort" :options="sortOptions" />
    </div>
    <my-dialog v-model:show="dialogVisible" @click="hideDialog">
      <Form @create="createBook"/>
    </my-dialog>
    <Booklist :books="books"  v-if="!isBookLoading"/>
    <div v-else>Loading...</div>
  </div>
</template>

<script lang="ts">
import Form from "@/component/Form.vue";
import Booklist from "@/component/Booklist.vue";
import {Book} from "@/component/book"
import MyDialog from "@/component/UI/MyDialog.vue";
import MyButton from "@/component/UI/MyButton.vue";
import axios from "axios";
import MySelect from "@/component/UI/MySelect.vue";
import {getImagesArrayFromServer} from '../scripts/getImagesArrayFromServer'
export default {
  components: {
    MySelect,
    MyButton,
    MyDialog,
    Form, Booklist, Book
  },
  name: 'Home',
  data(){
    return{
      books: [],
      images: Map,
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
    createBook(book: Book){
      this.books.push(book)
      this.dialogVisible = false
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
    removeBook(book: Book){
      this.books = this.books.filter(b => b.id !== book.id)
    },
    hideDialog(){
      
    },
    showDialog(){
      this.dialogVisible = true;
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