<template>
  <div class="form">
    
  <h1>Test Book Constructor</h1>
    <MyForm class="form-content">
      <div class="form-item">
        <div>
          <p>Upload a book cover</p>
          <button @click="showFileSelect = !showFileSelect">Select a file</button>
        </div>
        <div v-show="showFileSelect">
          <FileUpload :maxSize="1" accept="png" @file-uploaded="getUploadedData" />
        </div>

        <div v-if="fileSelected">
          Successfully Selected file: {{ file.name }}.{{ file.fileExtention }}
        </div>
      </div>

      <div class="form-item">
        <label for="title">Title: <span v-if="!isTitleValid">Enter Title</span></label>
        <my-input type="text" class="title" v-model="book.title"/>
      </div>
      <div class="form-item">
        <label for="desc">Description: <span v-if="!isDescriptionValid">Enter Description</span></label>
        <br/>
        <textarea class="desc" v-model="book.description"></textarea>
      </div>
      <div class="form-item">
      <label for="book-list">Genre</label>
      <book-genre-list class="book-list" :items="items" v-model="book.genre" />
      </div>
      <div class="form-item">
        <label for="author">Author:  <span v-if="!isAuthorValid">Enter Author</span></label>
        <my-input type="text" class="author" v-model="book.author"/>
      </div>
      <div class="form-item">
        <label for="price">Price:  <span v-if="!isPriceValid">Enter Correct Price.</span></label>
        <my-input type="number" class="price" v-model="book.price"/>
      </div>
      <div class="form-item">
        <label for="year">Year of Publication: <span v-if="!isYearOfPublicationValid">Enter Correct year</span></label>
        <my-input type="number" class="year" v-model="book.yearOfPublication"/>
      </div>
      <div class="form-item">
        <label for="pages">Pages: <span v-if="!isPagesValid">Enter Correct Number of Pages</span></label>
        <my-input type="number" class="pages" v-model="book.pages"/>
      </div>
      <div class="form-item">
        <label for="count">Count in stock: <span v-if="!isCountInStockValid">Enter Correct Item Of Book</span></label>
        <my-input type="number" class="count" v-model="book.countInStock"/>
      </div>
      
      <my-button>Add Book!</my-button>
      <my-button @click="sendBookCoverFileToServer">Test save cover!</my-button>
    </MyForm>
  </div>
</template>

<script lang="ts">
import MyButton from "@/component/UI/MyButton.vue";
import MyInput from "@/component/UI/MyInput.vue";
import MyForm from "@/component/Form.vue"
import BookGenreList from "@/component/BookGenreList.vue"
import FileUpload from "@/component/FileUpload.vue"
export default {
name: "BookConstructor",
  components: {MyInput, MyButton, MyForm, BookGenreList, FileUpload},
  data(){
    return{
      sellerData: {
        jwtToken: localStorage.getItem('jwtToken'),
        id: localStorage.getItem('id')
      },
      items: [
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
      file: {},
      fileSelected: false,
      showFileSelect: false,
      book: {
        title: '',
        description: '',
        author: '',
        genre: [],
        price: 0,
        yearOfPublication: 0,
        pages: 0,
        countInStock: 0,
      },
    }
  },
  computed: {
    isTitleValid(): boolean{
      return this.book.title != null && this.book.title != ''
    },
    isDescriptionValid(): boolean{
      return this.book.description != null && this.book.description != ""
    },
    isAuthorValid(): boolean{
      return this.book.author != null && this.book.author != ''
    },
    isPriceValid(): boolean{
      return this.book.price > 0
    },
    isYearOfPublicationValid(): boolean{
      return this.book.yearOfPublication <= new Date().getFullYear()
    },
    isPagesValid(): boolean{
      return this.book.pages > 0
    },
    isCountInStockValid(): boolean{
      return this.book.countInStock > 0
    }
  }, 
  methods: {
    getUploadedData(file) {
      this.fileSelected = true;
      this.showFileSelect = false;
      this.file = file;
    },
    async sendBookCoverFileToServer() {
      const formData = new FormData()
      formData.append(`${this.title}`, this.file)
      console.log(formData)
      //TODO: create controller to receive image c#
      await fetch('http://localhost:5045/api/File/SaveBookCover', {
        method: 'POST',
        headers: {
          "Authorization": `bearer ${this.sellerData.jwtToken}`,
          "seller-id": `${this.sellerData.id}`,
          "content-type": "multipart/form-data",
        },
        body: formData
      })
    },
    async sendBookTextDataToServer() {
      await fetch('adderss', {
        method: 'POST',
        headers: {
          "Authorization": `bearer ${this.sellerData.jwtToken}`,
          "seller-id": `${this.sellerData.id}`,
          "content-type": "application/json",
        },
        body: JSON.stringify(this.book)
      })
    }
  }
}
</script>

<style scoped>
.form{
  padding: 20px;
}
.form-content{
  width: 40%;
}
.form-item{
  margin-top: 20px;
}
.desc{
  resize: none;
  width: 103%;
  height: 250px;
}
.file-form{
  display: flex;
}
</style>