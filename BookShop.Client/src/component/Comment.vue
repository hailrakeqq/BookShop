<template>
  <div class="comment">
    <div class="meta-data">
      <p>{{comment.userName}}</p>
      <div class="meta-data-info">
        <star-rating id="rating" class="rating-book"
                     v-model:rating="comment.rating"
                     v-bind:read-only="true"
                     v-bind:show-rating="false"
                     v-bind:star-size="10"
                     v-bind:border-width="2"></star-rating>
        <p>{{comment.timeWhenCommentWasCreated}}</p>
      </div>
    </div>
    <p id="text">{{comment.text}}</p>
  </div>
</template>

<script>
import StarRating from 'vue-star-rating'
export default {
  components:{StarRating},
name: "Comment",
  props:{
    comment:{
      type: Object,
      required: true
    }
  }
}
</script>

<style scoped>
.comment{
  padding-left: 15px;
  padding-right: 15px;
  border: 1px solid teal;
  margin-right: 180px;
  width: 610px;
  max-width: 610px;
}
.meta-data{
  display: flex;
  justify-content: space-between;
}
.meta-data-info{
  display: flex;
  justify-content: flex-end;
}
#rating{
  margin-right: 15px;
}
#text{
  padding-left: 15px;
}
</style>