<template>
  <div class="main">
    <div class="profile-info">
      <h2 class="info-label">Name:</h2>
      <h2 class="info-value">{{currentUser.username}}</h2>
    </div>
    <div class="profile-info">
      <h2 class="info-label">Role:</h2>
      <h2 class="info-value">{{currentUser.role}}</h2>
    </div>
    <div v-if="currentUser.role === 'seller'">
<!--      seller section-->
      <div class="profile-info">
        <h2 class="info-label">Products:</h2>
        <h2 class="info-value">{{currentUser.countOfProduct}}</h2>
      </div>
      <div class="profile-info">
        <h2 class="info-label">Number of sold production:</h2>
        <h2 class="info-value">{{currentUser.countOfSoldProduct}}</h2>
      </div>
    </div>
    <div v-else>
<!--      user section-->
      <div class="profile-info">
        <h3><a class="info-label a-link" @click="goToLibrary">Library ({{this.currentUser.boughtBook}})</a></h3>
      </div>
      <div class="profile-info">
        <h3><a class="info-label a-link" @click="goToWishList">Wishlist ({{this.currentUser.wishlistCount}})</a></h3>
      </div>
    </div>
<!--    seller top books section-->
    <div class="seller__container" v-if="currentUser.role === 'seller'">
      <h3 style="text-align: center">Top seller product</h3>
      <booklist :books="books" :images="images"></booklist>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import Booklist from '@/component/Booklist.vue'
import {getBookCollectionFromAPIEndpointAndId} from '@/scripts/BookService'
import {getImagesHashMapFromServer} from '@/scripts/getImagesHashMapFromServer'
import MyButton from '@/component/UI/MyButton.vue'
export default {
name: "AccountPage",
  components: {
    Booklist, MyButton
  },
  data(){
    return{
      currentUser: {},
      books: [],
      images: Map,
    }
  },
  methods: {
    async getUserData(){
      const response = await axios.get(`http://localhost:5045/api/User/GetUserPublicData/${this.$route.params.id}`)
      this.currentUser = await response.data
    },
    async getBookCollection(){
      const response = await getBookCollectionFromAPIEndpointAndId(
          "http://localhost:5045/api/Book/GetSellerBookCollection",
          this.$route.params.id
      )
      
      this.books = response.sort((book1, book2) => book2.rating - book1.rating)
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
      this.getBookCollection();
      getImagesHashMapFromServer()
          .then(images => {this.images = images})
          .then(() => this.findCoverForBookByBookTitle());
    },
    goToLibrary(){
      this.$router.push({name: 'library', params: {userid: this.currentUser.id}})
    },
    goToWishList(){
      this.$router.push({name: 'wishlist', params: {userid: this.currentUser.id}})
    }
  },
  mounted() {
    this.getUserData().then(() => {
      if(this.currentUser.role === 'seller')
        this.loadBookAndImages();
    })
  }
}
</script>

<style scoped>
.main{
  padding: 100px;
}

.profile-info {
  margin-bottom: 10px;
  display: flex;
  align-items: center;
}

.info-label {
  width: 120px;
  font-weight: bold;
}

.info-value {
  flex: 1;
}
.seller__container{
  margin-top: 50px;
}
.a-link{
  text-decoration: underline;
}
</style>