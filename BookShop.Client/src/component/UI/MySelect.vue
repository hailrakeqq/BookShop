<template>
  <select :value="modelValue" @change="changeOption">
    <option disabled value="">{{disabledValue}}</option>
    <option v-for="option in options" :key="option.value" :value="option.value">{{ option.name }}</option>
  </select>
</template>
  
<script>
export default {
name: "my-select",
  props:{
    modelValue: {
      type: String
    },
    options: {
      type: Array,
      default: () => []
    },
    disabledValue: {
      type: String
    }
  },
  methods:{
    changeOption(event) {
      this.$emit('update:modelValue', event.target.value)
    }
  }
}
</script>

<style scoped>

</style>