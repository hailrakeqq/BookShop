<template>
  <div class="book">
    <div class="img" @click="goToBookPage(this.book.id)">
      <img :src="book.imageLink"/>
    </div>
    <div class="text_data">
      <h2>{{book.title}}</h2>
      <h3>{{book.price}} грн.</h3>
      <star-rating id="rating" class="rating-book"
                   v-model:rating="book.rating"
                   v-bind:read-only="true"
                   v-bind:show-rating="false"
                   v-bind:star-size="10"
                   v-bind:border-width="2"></star-rating>
      <div v-if="userRole === 'seller'">
        <my-button @click="redirectToChangeBookDataPage(book.id)">Update</my-button>
        <my-button @click="deleteBookRequest(book.id)">Delete</my-button>
      </div>
      <div v-else>
        <my-button @click="goToBookPage(this.book.id)">Buy book</my-button>
      </div>
      <div v-if="isWishlistPage">
        <my-button @click="deleteBookFromWishlist">Delete</my-button>
      </div>
    </div>
  </div>
</template>

<script>
import MyButton from "@/component/UI/MyButton.vue";
import '../router'
import StarRating from 'vue-star-rating'
import axios from 'axios'

export default {
  components: {
    MyButton, StarRating
  },
  name: "bookItem",
  data(){
    return{
      userRole: localStorage.getItem('role')
    }
  },
  props:{
    book: {
      type: Object,
      required: true
    },
    isWishlistPage:{
      type: Boolean,
      required: false
    }
  },
  methods:{
    deleteBookFromWishlist(){
      axios.delete(`http://localhost:5045/api/Book/DeleteBookFromUserWishList/${this.book.id}`,{
        headers:{
          'Authorization': `bearer ${localStorage.getItem("accessToken")}`,
        }
      }).then(response => {
            if(response.status === 200){
              alert("book has been successfully deleted from your wishlist")
              document.location.reload(false)
            }
          })
    },
    goToBookPage(id){
      this.$router.push({name: 'book', params: {id: id}})
    },
    redirectToChangeBookDataPage(id){
      this.$router.push({name: 'changebookdata', params: {bookId: id}})
    },
    deleteBookRequest(id){
      axios.delete(`http://localhost:5045/api/Seller/DeleteBook/${id}`,{
        headers:{'Authorization': `bearer ${localStorage.getItem("accessToken")}`}
      }).then(response => response.status == 200 ? document.location.reload(false) : alert("error when you try to delete"))
    }
  }
}
</script>

<style scoped>
.book {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 300px;
  margin: 10px;
  padding: 10px;
  border: 1px solid #ccc;
}

img {
  max-width: 100%;
  height: 300px;
}

h3 {
  font-size: 1.2rem;
  margin: 10px 0;
}
p {
  margin: 10px 0;
}
</style>