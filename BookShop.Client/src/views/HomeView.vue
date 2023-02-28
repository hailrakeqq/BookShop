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
    <Booklist :books="books" @remove="removeBook" v-if="!isBookLoading"/>
    <div v-else>Loading...</div>
  </div>
</template>

<script lang="ts">
import Form from "@/component/Form.vue";
import Booklist from "@/component/Booklist.vue";
import {Book} from "@/component/book"
import { defineComponent } from 'vue'
import MyDialog from "@/component/UI/MyDialog.vue";
import MyButton from "@/component/UI/MyButton.vue";
import axios from "axios";
import MySelect from "@/component/UI/MySelect.vue";

export default defineComponent({
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
        const responce = await axios.get('https://jsonplaceholder.typicode.com/posts?_limit=10')
        this.books = responce.data
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
    }
  },
  mounted() {
    this.getBookList();
  }
})
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