<template>
  <div class="form" v-if="!this.isPageLoading">
    
  <h1>Test Book Constructor</h1>
    <MyForm class="form-content">
      <div class="form-item">
        <div>
          <div v-if="isChangePage">
            <p>You can change image if you want.</p> 
          </div>
          <p>Upload a book cover</p>
          <button @click="showFileSelect = !showFileSelect">Select a file</button>
        </div>
        <div v-show="showFileSelect">
          <FileUpload :maxSize="3" accept="jpg" @file-uploaded="getUploadedData" />
        </div>

        <div v-if="fileSelected">
          Successfully Selected file: {{ fileInfo.name }}.{{ fileInfo.fileExtention }}
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
        <my-input type="number" class="pages" v-model="book.countOfPages"/>
      </div>
      <div class="form-item">
        <label for="count">Count in stock: <span v-if="!isCountInStockValid">Enter Correct Item Of Book</span></label>
        <my-input type="number" class="count" v-model="book.countInStock"/>
      </div>
      <my-button v-if="isChangePage" @click="updateBookDataRequest(book.id)">Update Book!</my-button>
      <my-button v-else @click="insertBookToDBRequest">Add Book!</my-button>
    </MyForm>
  </div>
  <div v-else>
    <h3>Loading...</h3>
  </div>
</template>

<script lang="ts">
import MyButton from "@/component/UI/MyButton.vue";
import MyInput from "@/component/UI/MyInput.vue";
import MyForm from "@/component/Form.vue"
import BookGenreList from "@/component/BookGenreList.vue"
import FileUpload from "@/component/FileUpload.vue"
import axios from 'axios'
import '../../router'
export default {
name: "BookConstructor",
  components: {MyInput, MyButton, MyForm, BookGenreList, FileUpload},
  data(){
    return{
      book: {},
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
      file: null,
      fileInfo: null,
      fileSelected: false,
      showFileSelect: false,
      isRequestStatus200: [],
      isPageLoading: false,
      isChangePage: false
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
      return this.book.countOfPages > 0
    },
    isCountInStockValid(): boolean{
      return this.book.countInStock > 0
    },
    isImageHasBeenAdded(): boolean{
      return this.file != null
    },
  }, 
  methods: {
    getUploadedData(file, fileInfo) {
      this.fileSelected = true;
      this.showFileSelect = false;
      this.file = file;
      this.fileInfo = fileInfo
    },
    isDataValid(){
      return this.isTitleValid && this.isDescriptionValid && this.isAuthorValid && this.isPriceValid &&
          this.isPagesValid && this.isYearOfPublicationValid && this.isCountInStockValid && this.isImageHasBeenAdded
    },
    async sendBookCoverFileToServer() {
      const formData = new FormData()
      formData.append(`${this.book.title}`, this.file, this.book.title)
      try {
        await axios.post('http://localhost:5045/api/File/SaveBookCover', formData, {
          headers: {
            'Authorization': `bearer ${this.sellerData.jwtToken}`,
            'seller-id': `${this.sellerData.id}`,
            "Content-Type": "multipart/form-data"
          }}).then(Response => {
            if(Response.status == 200)
              this.isRequestStatus200[0] = true
          })
      } catch (e) {
        console.log(e)
      }
    },
    async sendBookTextDataToServer() {
      try {
          const book = {
            sellerId: this.sellerData.id,
            title: this.book.title,
            description: this.book.description,
            genre: this.book.genre,
            author: this.book.author,
            price: this.book.price,
            yearOfPublication: this.book.yearOfPublication,
            countOfPages: this.book.countOfPages,
            countInStock: this.book.countInStock
          }
          console.log(book)
        await axios.post('http://localhost:5045/api/Seller/AddBook', JSON.stringify(book),{
          headers: {
            "Authorization": `bearer ${this.sellerData.jwtToken}`,
            "seller-id": `${this.sellerData.id}`,
            "Content-Type": "application/json",
          },
        }).then(Response => {
          if(Response.status == 200)
            this.isRequestStatus200[1] = true
        })
      } catch (e){
        console.log(e)
      }
    },
    isRequestSuccessfully(): boolean{
      return ((this.isRequestStatus200[0] && this.isRequestStatus200[1]) == true)
    },
    insertBookToDBRequest(){
      if(this.isDataValid()) {
        Promise.all([this.sendBookCoverFileToServer(), this.sendBookTextDataToServer()])
            .then(() => {
              if(this.isRequestSuccessfully())
                alert("Book has been successfully add. Redirecting to stock")
                this.$router.push('/stock')
            })
      } else {
        alert("data is not valid")
      }
    },
    updateBookDataRequest(id){
      if(this.isDataValid()) {
        Promise.all([this.sendBookCoverFileToServer(), this.updateBookTextData(id)])
            .then(() => {
              if(this.isRequestSuccessfully())
                alert("Book has been successfully updated. Redirecting to stock")
              this.$router.push('/stock')
            })
      } else {
        alert("data is not valid")
      }
    },
    loadPage(){
      try {
        this.isPageLoading = true
        this.getBookById(this.$route.params.bookId)
      } catch (e) {
        console.log(e)
      } finally {
        this.isPageLoading = false 
      }
    },
    async getBookById(id){
        const request = await axios.get(`http://localhost:5045/api/Book/GetBookById/${id}`)
        this.book = request.data
    },
    async updateBookTextData(id) {
      try {
        const book = {
          Id: id,
          sellerId: this.sellerData.id,
          title: this.book.title,
          description: this.book.description,
          genre: this.book.genre,
          author: this.book.author,
          price: this.book.price,
          yearOfPublication: this.book.yearOfPublication,
          countOfPages: this.book.countOfPages,
          countInStock: this.book.countInStock
        }
        await axios.put(`http://localhost:5045/api/Seller/ChangeBookData/${id}`, JSON.stringify(book),{
          headers: {
            "Authorization": `bearer ${this.sellerData.jwtToken}`,
            "seller-id": `${this.sellerData.id}`,
            "Content-Type": "application/json",
          },
        }).then(Response => Response.status === 200 ?  this.isRequestStatus200[1] = true : alert('error'))
      } catch (e){
        console.log(e)
      }
    },
  },
  mounted() {
    if(window.location.href.endsWith('/bookconstructor')){
      this.isChangePage = false
      this.book = {
        title: '',
        description: '',
        genre: [],
        author: '',
        price: '',
        yearOfPublication: '',
        countOfPages: '',
        countInStock: ''
      }
    } else {
      this.isChangePage = true
      this.loadPage()
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

//TODO: test save with image(and replace image)