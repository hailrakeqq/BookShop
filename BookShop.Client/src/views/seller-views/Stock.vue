<template>
  <h1>My test book stock</h1>
  <booklist :books="books"  v-if="!isBookLoading"></booklist>
  <div v-else>Loading...</div>

</template>

<script lang="ts">
import Booklist from "@/component/Booklist.vue";
export default {
  name: "Stock",
  components:{
    Booklist
  },
  data(){
    return{
      books: [],
      isBookLoading: false,
    }
  },
  methods:{
    async getBookCollectionBySellerId(id){
      try{
        this.isBookLoading = true
        await fetch(`http://localhost:5045/api/Book/GetSellerBookCollection/${id}`,{
          method: "GET",
          headers:{
            'Content-Type': 'application/json'
          }
        }).then(data => data.json()).then(data => this.books = data)
      } catch (e) {
        alert("error")
      } finally {
        this.isBookLoading = false
      }
    },
  },
  mounted() {
    this.getBookCollectionBySellerId(localStorage.getItem('id'));
  }
}
</script>

<style scoped>

</style>