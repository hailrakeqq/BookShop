<template>
  <div class="list">
    <label v-for="(item, index) in items" :key="index">
      <input type="checkbox" :value="item.value" v-model="selectedItems">{{ item.label }}
    </label>
<!--    <input type="checkbox" id="Action" value="action" v-model="genre">-->
<!--    <label for="Action">Action</label>-->
<!--    <input type="checkbox" id="Detective" value="detective" v-model="genre">-->
<!--    <label for="Detective">Detective</label>-->
<!--    <input type="checkbox" id="Historical-novel" value="historical-novel" v-model="genre">-->
<!--    <label for="Historical-novel">Historical novel</label>-->
<!--    <input type="checkbox" id="Love-novel" value="love-novel" v-model="genre">-->
<!--    <label for="Love-novel">Love novel</label>-->
<!--    <input type="checkbox" id="Mystic" value="mystic" v-model="genre">-->
<!--    <label for="Mystic">Mystic</label>-->
<!--    <input type="checkbox" id="Adventures" value="adventures" v-model="genre">-->
<!--    <label for="Adventures">Adventures</label>-->
<!--    <input type="checkbox" id="Thriller-horror" value="thriller-horror" v-model="genre">-->
<!--    <label for="Thriller-horror">Thriller/horror</label>-->
<!--    <input type="checkbox" id="Fantastic" value="fantastic" v-model="genre">-->
<!--    <label for="Fantastic">Fantastic</label>-->
<!--    <input type="checkbox" id="Fantasy" value="fantasy" v-model="genre">-->
<!--    <label for="Fantasy">Fantasy</label>-->
<!--    <input type="checkbox" id="Manga" value="manga" v-model="genre">-->
<!--    <label for="Manga">Manga</label>-->
  </div>
</template>

<script>
export default {
name: "BookGenreList",
  props: {
    items: [
      {label: 'Action', value: 'action'},
      {label: 'Detective', value: 'detective'},
      {label: 'Historical Novel', value: 'historical-novel'},
      {label: 'Love novel', value: 'love-novel'},
      {label: 'Mystic', value: 'mystic'},
      {label: 'Adventures', value: 'adventures'},
      {label: 'Thriller horror', value: 'thriller-horror'},
      {label: 'Fantastic', value: 'fantastic'},
      {label: 'Manga', value: 'manga'},
      {label: 'Fantasy', value: 'fantasy'},
    ],
    selectedItems: {
      type: Array,
      default: () => []
    }
  },
  data() {
    return {
      selectedItems: this.selectedItems
    }
  },
  // watch: {
  //   selectedItems: function() {
  //     this.$emit('update:selectedItems', this.selectedItems)
  //   }
  // }
}
</script>

<style scoped>

</style>