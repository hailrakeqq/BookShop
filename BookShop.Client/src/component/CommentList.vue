<template>
  <div class="main" v-if="comments.length > 0">
    <Comment
        class="comment"
        v-for="comment in comments"
        :comment="comment"
        :key="comment.id"/>
  </div>
  <h2 v-else style="color: red">This book don't have any comment</h2>
</template>

<script>
import Comment from '@/component/Comment.vue'
export default {
  components: {
    Comment
  },
name: "CommentList",
  props: {
    comments: {
      type: Array,
      required: true
    },
  }
}
</script>

<style scoped>

</style>