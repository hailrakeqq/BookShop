<template>
  <h1>Test Book Constructor</h1>
  <MyForm>
    <label for="title">Title: <span v-if="!isTitleValid">Enter Title</span></label>
    <my-input type="text" class="title" v-model="title"/>
    <br/>
    <label for="desc">Description: <span v-if="!isDescriptionValid">Enter Description</span></label>
    <br/>
    <textarea class="desc" v-model="description"></textarea>
    <br/>
<!--    genre 
    TODO: add list from book genre list and add bind with genre + begin implement upload image to backend
     розбити всі інпути реюзабельними
-->
    <book-genre-list v-bind="genre"></book-genre-list>
    
    <br/>
    <label for="author">Author:  <span v-if="!isAuthorValid">Enter Author</span></label>
    <my-input type="text" class="author" v-model="author"/>
    <br/>
    <label for="price">Price:  <span v-if="!isPriceValid">Enter Correct Price.</span></label>
    <my-input type="number" class="price" v-model="price"/>
    <br/>
    <label for="year">Year of Publication: <span v-if="!isYearOfPublicationValid">Enter Correct year</span></label>
    <my-input type="number" class="year" v-model="yearOfPublication"/>
    <br/>
    <label for="pages">Pages: <span v-if="!isPagesValid">Enter Correct Number of Pages</span></label>
    <my-input type="number" class="pages" v-model="pages"/>
    <br/>
    <label for="count">Count in stock: <span v-if="!isCountInStockValid">Enter Correct Item Of Book</span></label>
    <my-input type="number" class="count" v-model="countInStock"/>
    <my-button>Add Book!</my-button>
  </MyForm>
</template>

<script lang="ts">
import MyButton from "@/component/UI/MyButton.vue";
import MyInput from "@/component/UI/MyInput.vue";
import MyForm from "@/component/Form.vue"
import BookGenreList from "@/component/BookGenreList.vue"
export default {
name: "BookConstructor",
  components: {MyInput, MyButton, MyForm, BookGenreList},
  data(){
    return{
      title: '',
      description: '',
      author: '',
      genre: '',
      price: 0,
      yearOfPublication: 0,
      pages: 0,
      countInStock: 0,
    }
  },
  computed: {
    isTitleValid(): boolean{
      return this.title != null && this.title != ''
    },
    isDescriptionValid(): boolean{
      return this.description != null && this.description != ""
    },
    isAuthorValid(): boolean{
      return this.author != null && this.author != ''
    },
    isPriceValid(): boolean{
      return this.price > 0
    },
    isYearOfPublicationValid(): boolean{
      return this.yearOfPublication <= new Date().getFullYear()
    },
    isPagesValid(): boolean{
      return this.pages > 0
    },
    isCountInStockValid(): boolean{
      return this.countInStock > 0
    }
  }, 
  methods:{
    
  }
}
</script>

<style scoped>

</style>