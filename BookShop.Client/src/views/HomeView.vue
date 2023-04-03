<template>
  <div class="app">
    <h3>Page with books</h3>
    <div class="app_button" v-if="userRole === 'seller'">
      <my-button @click="redirectToBookConstructor">Add book</my-button>
    </div>
    <my-input id="searchItem" v-model="searchByName" placeholder="Search by name..."></my-input>
    <div class="main-container" v-if="!isBookLoading">
      <div class="left-block">
        <my-select id="select" v-model="selectedSort" :disabledValue="disabledValue" :options="sortOptions"></my-select>
        <book-genre-list class="genre-list" :items="genreList" v-model="sortByGenre"></book-genre-list>
        <my-button @click="getBookListBySelectedGenre">Find by Genre</my-button>
      </div>
      <Booklist class="book-list" :books="sortedListByGenre"/>
    </div>
    <div v-else>Loading...</div>
  </div>
</template>

<script lang="ts">
import Booklist from "@/component/Booklist.vue";
import {Book} from "@/component/book"
import MyDialog from "@/component/UI/MyDialog.vue";
import MyButton from "@/component/UI/MyButton.vue";
import MyInput from "@/component/UI/MyInput.vue"
import BookGenreList from "@/component/BookGenreList.vue"
import axios from "axios";
import MySelect from "@/component/UI/MySelect.vue";
import {getImagesHashMapFromServer} from '../scripts/getImagesHashMapFromServer'
export default {
  components: {
    MySelect,
    MyButton,
    MyDialog,
    MyInput,
    BookGenreList,
    Booklist, 
    Book
  },
  name: 'Home',
  data(){
    return{
      books: [],
      images: Map,
      userRole: localStorage.getItem('role'),
      dialogVisible: false,
      isBookLoading: false,
      searchByName: '',
      disabledValue: "Sort by...",
      selectedSort: '',
      sortOptions: [
        {value: 'title', name: 'By name'},
        {value: 'rating', name: 'By rating'},
        {value: 'growing', name: 'Price: Low to High'},
        {value: 'descending', name: 'Price: High to Low'},
        {value: 'countInStock', name: 'By count in stock'}
      ],
      sortByGenre: [],
      genreList: [
        {label: 'Action', value: 'action'},
        {label: 'Detective', value: 'detective'},
        {label: 'Historical Novel', value: 'historical-novel'},
        {label: 'Romance', value: 'romance'},
        {label: 'Mystic', value: 'mystic'},
        {label: 'Adventures', value: 'adventures'},
        {label: 'Thriller', value: 'thriller'},
        {label: 'Horror', value: 'horror'},
        {label: 'Fantastic', value: 'fantastic'},
        {label: 'Manga', value: 'manga'},
        {label: 'Fantasy', value: 'fantasy'},
        {label: 'Science fiction', value: 'science-fiction'}
      ],
    }
  },
  computed:{
    sortedListByGenre(){
      if(this.sortByGenre.length === 0)
        return this.filteredItems
      
      return this.filteredItems.filter(item => item.genre.some(genre => this.sortByGenre.includes(genre)))
    },
    filteredItems(){
      if(!this.searchByName)
        return this.sortedBooks

      return this.sortedBooks.filter(item => item.title.toLowerCase().includes(this.searchByName.toLowerCase()))
    },
    sortedBooks() {
      if (!this.selectedSort) {
        return this.books;
      }
      switch (this.selectedSort){
        case "title":
          return this.books.slice().sort((book1, book2) =>
              book1[this.selectedSort] > book2[this.selectedSort] ? 1 : -1);
        case "rating":
          return this.books.slice().sort((book1, book2) =>
              book2[this.selectedSort].rating - book1[this.selectedSort].rating)
        case "countInStock":
          return this.books.slice().sort((book1, book2) =>
              book2.countInStock - book1.countInStock);
        case "growing":
          return this.books.slice().sort((book1, book2) =>
              book1.price - book2.price);
        case "descending":
          return this.books.slice().sort((book1, book2) =>
              book2.price - book1.price);
      }
    },
  },
  methods:{
    redirectToBookConstructor(){
      this.$router.push('/bookconstructor')
    },
    async getBookList(){
      try{
        this.isBookLoading = true
        const response = await axios.get('http://localhost:5045/api/Book')
        this.books = response.data.reverse()
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
.genre-list{
  display: flex;
}
.main-container{
  display: flex;
  align-content: flex-start;
}
#searchItem{
  width:59%;
  margin-left: 350px;
}
.left-block{
  position: fixed;
}
.book-list{
  margin-left:150px;
  max-width: 80%;
}
</style>