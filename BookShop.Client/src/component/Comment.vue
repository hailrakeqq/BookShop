<template>
  <div class="comment">
    <h4>{{comment.user}}</h4>
    <p>{{comment.time}}</p>
    <p>{{comment.rating}}</p>
    <p>{{comment.description}}</p>
  </div>
</template>

<script>
export default {
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

</style>