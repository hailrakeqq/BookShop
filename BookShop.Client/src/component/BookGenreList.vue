<template>
  <div class="list">
      <ul :value="modelValue">
        <li v-for="(item, index) in items" :key="index">
          <label>
          <input 
              type="checkbox"
              :value="item.value"
              :checked="isChecked(item.value)"
              @change="handleChange($event, item.value)"
          />
            {{ item.label }}</label>
        </li>
      </ul>
  </div>
</template>

<script>
export default {
name: "BookGenreList",
  props: {
    items: {
      type: Array,
      required: true,
    },
    modelValue: {
      type: Array,
      default: () => [],
    },
  },
  data() {
    return {
      checkedValues: this.modelValue,
    };
  },
  watch: {
    modelValue(newValue) {
      this.checkedValues = newValue;
    },
    checkedValues(newValue) {
      this.$emit("update:modelValue", newValue);
    },
  },
  methods: {
    isChecked(value) {
      return this.checkedValues.includes(value);
    },
    handleChange(event, value) {
      const checked = event.target.checked;
      if (checked) {
        this.checkedValues.push(value);
      } else {
        const index = this.checkedValues.indexOf(value);
        if (index !== -1) {
          this.checkedValues.splice(index, 1);
        }
      }
    },
  },
}
</script>

<style scoped>

</style>