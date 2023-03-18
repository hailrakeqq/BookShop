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
        <my-button @click="buyBook">Buy Book</my-button>
        <my-button @click="addBookToMyWishList">Add Book to Wishlist</my-button>
      </div>
      </div>
      <div class="book-data-block">
        <div class="book-data-block-main">
          <h1>{{book.title}}</h1>
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
        <my-input id="comment-input" type="text" placeholder="Your comment"/>
        <star-rating class="rating-book" 
                     v-model:rating="rating" 
                     v-bind:increment="0.5"
                     v-bind:show-rating="false"
                     v-bind:star-size="18"
                     v-bind:border-width="2"></star-rating>
        <MyButton @click="addComment">Add a comment</MyButton>
      </div>
      <div class="render-comment">
      <!--here can be comment loaded from db-->
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import MyButton from '@/component/UI/MyButton.vue'
import MyInput from '@/component/UI/MyInput.vue'
import StarRating from 'vue-star-rating'
export default {
  components: {MyButton, MyInput, StarRating},
name: "BookPage",
  data(){
    return{
      book: {},
      bookComment: [],
      genrelist: '',
      image: null,
      isPageLoading: false,
      comment:{
        rating: 0,
        comment: '',
      }
    }
  },
  computed:{
    generateGenreListFromArray(){
      this.genrelist = this.book.genre.join(', ')
    }
  },
  methods: {
    loadBookData(){
      try {
        this.isPageLoading = true
        console.log(this.$route.params.id)
        this.getBookTextData(this.$route.params.id)
            .then(() =>{
          this.getBookCoverImage(this.book.title)
        })
      } catch (e) {
        alert(e.message)
      } finally {
        this.isPageLoading = false
      }
    },
    async getBookTextData(id){
      await axios.get(`http://localhost:5045/api/Book/GetBookById/${id}`,{headers:{"Content-Type": "image/jpeg"}})
          .then(response => this.book = response.data)
    },
    async getBookCoverImage(name){
      await axios.get(`http://localhost:5045/api/File/GetBookCoverByName/${name}`,{
        responseType: 'blob'
      }).then(response =>this.image = URL.createObjectURL(response.data))
    },
    addComment(){
      
    },
    buyBook(){
      
    },
    addBookToMyWishList(){
    }
  },
  mounted(){
    this.loadBookData()
  }
}
</script>

<style scoped>
.main{
  margin-left: 100px;
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

</style>