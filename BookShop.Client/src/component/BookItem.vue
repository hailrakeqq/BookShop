<template>
  <div class="book">
    <div class="img">
      <img :src="book.imageLink"/>
    </div>
    <div class="text_data">
      <h2>{{book.title}}</h2>
      <h3>{{book.price}} грн.</h3>
      <div v-if="userRole === 'seller'">
        <my-button @click="redirectToChangeBookDataPage(book.id)">Update</my-button>
        <my-button>Delete</my-button>
      </div>
      <div v-else>
        <my-button>Buy book</my-button>
      </div>
    </div>
  </div>
</template>

<script>
import MyButton from "@/component/UI/MyButton.vue";
import '../router'
export default {
  components: {
    MyButton
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
  },
  methods:{
    redirectToChangeBookDataPage(id){
      this.$router.push({name: 'changebookdata', params: {bookId: id}})
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