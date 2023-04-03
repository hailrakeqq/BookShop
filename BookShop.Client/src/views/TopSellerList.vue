<template>
  <table class="seller-list">
    <thead>
      <tr>
        <th>Seller Name</th>
        <th>Product Count</th>
        <th>Sold Count</th>
        <th><my-sellect v-model="selectedSort" :disabledValue="disabledValue" :options="sortOptions"></my-sellect></th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="seller in sortedSellers" :key="seller.id" >
        <td>{{ seller.username }}</td>
        <td>{{ seller.countOfProduct }}</td>
        <td>{{ seller.countOfSoldProduct }}</td>
        <td><my-button @click="redirectToSellerProfile(seller.id)">Visit profile</my-button></td>
      </tr>
    </tbody>
  </table>
</template>

<script>
import axois from 'axios'
import MyButton from '@/component/UI/MyButton.vue'
import MySellect from '@/component/UI/MySelect.vue'
export default {
name: "TopSellerList",
  components: { MyButton, MySellect},
  data() {
    return{
      sellers: [],
      selectedSort: '',
      disabledValue: 'Sort by...',
      sortOptions: [
        {value: 'username', name: 'By name'},
        {value: 'countOfProduct', name: 'By Count Product'},
        {value: 'countOfSoldProduct', name: 'By Count Sold Product'},
      ],
    }
  },
  computed: {
    sortedSellers() {
      if (!this.selectedSort) {
        return this.sellers;
      }
      switch (this.selectedSort){
        case "username":
          return this.sellers.slice().sort((seller1, seller2) =>
              seller1[this.selectedSort] > seller2[this.selectedSort] ? 1 : -1);
        case "countOfProduct":
          return this.sellers.slice().sort((seller1, seller2) =>
              seller2[this.selectedSort].countOfProduct - seller1[this.selectedSort].countOfProduct)
        case "countOfSoldProduct":
          return this.sellers.slice().sort((seller1, seller2) =>
              seller2.countOfSoldProduct - seller1.countOfSoldProduct);
      }
    },
  },
  methods:{
    loadPage(){
      this.getSellersCollection() 
    },
    getSellersCollection(){
      axois.get(`http://localhost:5045/api/User/GetSellerList`)
          .then(response => this.sellers = response.data)
    },
    redirectToSellerProfile(id){
      this.$router.push(`/user/${id}`)
    },
  },
  mounted(){
    this.loadPage()
  },
}
</script>

<style scoped>
.seller-list {
  border-collapse: collapse;
  width: 100%;
}
.seller-list th,
.seller-list td {
  padding: 10px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.seller-list th {
  background-color: #f2f2f2;
}

.seller-list tbody tr:hover {
  background-color: #f5f5f5;
}

</style>