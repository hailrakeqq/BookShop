<template>
  <div class="book">
    <div class="img">
      <img :src="book.imageLink"/>
    </div>
    <div class="text_data">
      <h2>{{book.title}}</h2>
      <h3>{{book.price}} грн.</h3>
      <my-button>Test Button</my-button>
    </div>
  </div>
</template>

<script>
import MyButton from "@/component/UI/MyButton.vue";
export default {
  components: {
    MyButton
  },
  name: "bookItem",
  props:{
    book: {
      type: Object,
      required: true
    },
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