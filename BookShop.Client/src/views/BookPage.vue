<template>
  <div v-if="isPageLoading">
    Loading...
  </div>
  <div v-else>
    <div class="main">
      <div class="img">
        <img class="img" :src="image"/>
        <div class="buy-section">
          <h3>Price: {{book.price}} грн.</h3>
          <my-button @click="showDialog">Buy Book</my-button>
          <my-button @click="addBookToMyWishList">Add Book to Wishlist</my-button>
        </div>
      </div>
      <div class="book-data-block">
        <div class="book-data-block-main">
          <h1>{{book.title}}</h1>
          <star-rating id="rating" class="rating-book"
                       v-model:rating="this.book.rating"
                       v-bind:read-only="true"
                       v-bind:increment="0.1"
                       v-bind:show-rating="true"
                       v-bind:star-size="10"
                       v-bind:border-width="2"></star-rating>
          <h3>Author: <b style="color: blue">{{book.author}}</b></h3>
          <h4>Genre: {{genrelist}}</h4>
          <h5>Year of publication: {{book.yearOfPublication}}</h5>
          <h5>Pages: {{book.countOfPages}}</h5>
        </div>
        <div class="desc-section">
          <h3>Description: </h3>
          <p>{{book.description}}</p>
        </div>
      </div>
    </div>

    <div class="comment-section">
      <div class="comment-form">
      <p v-if="isUserHaveBook === false">You cannot add comment because you didn't have this book.</p>
        <my-input v-model="comment.text" id="comment-input" type="text" placeholder="Your comment"/>
        <star-rating class="rating-book"
                     v-model:rating="comment.rating"
                     v-bind:increment="0.5"
                     v-bind:show-rating="false"
                     v-bind:star-size="18"
                     v-bind:border-width="2"></star-rating>
        <MyButton v-if="isUserHaveBook === true" @click="addComment">Add a comment</MyButton>
      </div>
      <div class="render-comment">
        <comment-list :comments="bookComment"></comment-list>
      </div>
    </div>
    <my-dialog v-model:show="isDialogVisible">
      <div class="dialog__content">
        <h1>{{ book.title }}</h1>
        <p>{{ book.author }}</p>
        <my-form @submit.prevent="submitForm">
          <div>
            <label for="quantity">Quantity: </label>
            <input type="number" id="quantity" v-model.number="bookCount" min="1" max="15" required>
          </div>
          <div>
            <my-button style="margin-left: 100px" @click="buyBook">Buy Now</my-button>
          </div>
        </my-form>
      </div>
    </my-dialog>
  </div>
</template>

<script lang="ts">
import axios from 'axios'
import MyButton from '@/component/UI/MyButton.vue'
import MyInput from '@/component/UI/MyInput.vue'
import MyDialog from '@/component/UI/MyDialog.vue'
import MyForm from '@/component/Form.vue'
import CommentList from '@/component/CommentList.vue'
import StarRating from 'vue-star-rating'
export default {
  components: {MyButton, MyInput, StarRating , MyDialog, MyForm, CommentList},
  name: "BookPage",
  data(){
    return{
      userid: localStorage.getItem('id'),
      userRole: localStorage.getItem('role'),
      jwtToken: localStorage.getItem('accessToken'),
      book: {},
      bookComment: [],
      bookCount: 1, 
      genrelist: '',
      isUserHaveBook: false,
      isDialogVisible: false,
      image: null,
      isPageLoading: false,
      comment:{
        rating: 0,
        text: '',
      }
    }
  },
  methods: {
    loadBookData(){
      try {
        this.isPageLoading = true
        this.getBookTextData(this.$route.params.id)
            .then(() => this.getBookCoverImage(this.book.title))
        this.getBookComment()
        this.CheckIfUserHaveThisBook()
      } catch (e) {
        alert(e.message)
      } finally {
        this.isPageLoading = false
      }
    },
    changeCommentCreatedDate(){
      /* change comment created date:
       * example: 
       * before: 2023-03-27T11:51:31.257+00:00
       * after: 2023.03.27
      */
      this.bookComment.forEach(comment =>
          comment.timeWhenCommentWasCreated = comment.timeWhenCommentWasCreated.replace(/-/g, '.').split('T')[0])
    },
    async getBookComment(){
      try{
        axios.get(`http://localhost:5045/api/Comment/GetCommentForBook/${this.$route.params.id}`)
            .then(response => {
              this.bookComment = response.data
              this.changeCommentCreatedDate()
            })
              
      }catch (e) {
        alert(e.message)
      }
    },
    async getBookTextData(id){
      try{
        await axios.get(`http://localhost:5045/api/Book/GetBookById/${id}`,{headers:{"Content-Type": "application/json"}})
            .then(response => {
                this.book = response.data
                this.genrelist = this.book.genre.join(', ')
              })
      } catch (e) {
        if(e.message.includes("404"))
          this.$router.push({name: 'notFoundPage'})
      }
    },
    async getBookCoverImage(name){
      await axios.get(`http://localhost:5045/api/File/GetBookCoverByName/${name}`,{
        responseType: 'blob'
      }).then(response =>this.image = URL.createObjectURL(response.data))
    },
    CheckIfUserHaveThisBook(){
      axios.get(`http://localhost:5045/api/Comment/isUserHaveBook/${this.$route.params.id}`, {}, {
        headers: {
          "Content-Type": "application/json",
          "Authorization": `bearer ${this.jwtToken}`
        }}).then(response => {
          if(response.status === 200)
            this.isUserHaveBook = true
          else
            this.isUserHaveBook = false 
      })
    },
    async addComment(){
      const comment = {
        text: this.comment.text,
        rating: this.comment.rating
      }
      try {
        await axios.post(`http://localhost:5045/api/Comment/CreateBookComment/${this.book.id}`, JSON.stringify(comment), {
          headers:{
            "Content-Type": "application/json",
            "Authorization": `bearer ${this.jwtToken}`},
        }).then(Response => {
          if(Response.status === 200)
            document.location.reload(false)
        })
      } catch (e) {
        alert("You Can Add Only 1 comment for 1 book.")
        document.location.reload(false)
      }
    },
    showDialog(){
      this.isDialogVisible = true
    },
    buyBook(){
      if(this.userRole !== 'seller'){
        axios.post(`http://localhost:5045/api/Book/BuyBook/${this.book.id}`, this.bookCount, {
          headers:{
            "Content-Type": "application/json",
            "Authorization": `bearer ${this.jwtToken}`,
          }
        }).then(response => {
          if(response.status === 200){
            this.isDialogVisible = false
            this.$router.push({name: 'library', params:{userid: localStorage.getItem('id')}})
          }
        }).catch(error => {
          if(error.message.includes('401') || error.message.includes('404')){
            this.$router.push('/login')
          }
          else if(error.message.includes('409')){
            alert("You already have this book")
            this.isDialogVisible = false
          }
        })
      } else{
        alert("Seller Cannot buy a book.")
      }
    },
    addBookToMyWishList(){
      if(this.userRole !== 'seller') {
        /*
        * here i send null data because when we use post request and it's haven't any data we get 
        * error 401 unauthorized 
        */
        axios.post(`http://localhost:5045/api/Book/AddBookToUserWishList/${this.book.id}`, null, {
          headers: {
            "Authorization": `bearer ${this.jwtToken}`,
          }
        })
            .then(response => {
              if (response.status === 200)
                this.$router.push({name: 'wishlist', params: {userid: localStorage.getItem('id')}})
            }).catch(error => {
          if (error.message.includes('401') || error.message.includes('404')) {
            this.$router.push('/login')
          }
          else if(error.message.includes('409'))
            alert("You have already this book in your wishlist")
        })
      }
      else {
        alert("Seller Cannot add book to wishlist.")
      }
    },
  },
  mounted(){
    this.loadBookData()
  }
}
</script>

<style scoped>
.main{
  margin-left: 180px;
  padding: 50px;
}
.img{
  height: 300px;
  width: 300px;
  display: inline-block;
  margin-bottom: 10px;
}
.book-data-block{
  display: inline-block;
  margin-left: 50px;
  position: absolute;
}
.buy-section{
  display: inline-block;
  align-content: center;
  margin-left: 75px;
  text-align: center;
  margin-right: 20px;
}
.desc-section{
  width: 300%;
  max-width: 300%;
  height: auto;
  border: 1px solid teal;
  padding-left: 10px;
}
.comment-section{
  margin-left: 150px;
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
}
.comment-form{
  padding: 15px;
  border: 1px solid teal;
  width: 40%;
}
#comment-input{
  width: 95%;
}
.rating-book{
  margin-top: 10px;
}
.dialog__content{
  padding: 50px;
}
</style>