<script setup>
import { ref } from "vue";

const msg = ref("hello vue");
</script>
<template>
  <div>
    <h1 class="bg-slate-600 text-3xl font-bold underline">{{ msg }}</h1>
  </div>
</template>
<style scoped></style>
