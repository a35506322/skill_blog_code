<script setup>
import { computed, onMounted, ref, watch } from "vue";
import { useIntersectionObserver } from "@vueuse/core";

// http://localhost:5173/src/pages/waterfalls_flow/index.html

const images = ref([]);
const currentLength = ref(0);
const loading = ref(null);
const loadingIsVisible = ref(false);

const init = () => {
  currentLength.value = 6;
  loadingIsVisible.value = true;
  const imageUrls = [
    "https://images.chinatimes.com/newsphoto/2022-11-15/656/20221115000866_20221115074915.jpg",
    "https://img.news.ebc.net.tw/EbcNews/news/2022/05/08/1652017609_26100.jpg",
    "https://innews.com.tw/wp-content/uploads/2023/03/328908964_1314300452446166_4152011679063943075_n.jpg",
    "https://attach.setn.com/newsimages/2023/07/25/4254885-PH.jpg",
    "https://images.chinatimes.com/newsphoto/2023-03-13/656/20230313004653.jpg",
    "https://attach.setn.com/newsimages/2024/03/07/4559041-PH.jpg",
    "https://img.news.ebc.net.tw/EbcNews/news/2022/11/15/1668525688_44484.jpg",
    "https://img.news.ebc.net.tw/EbcNews/news/2023/05/25/1685022040_71120.jpg",
    "https://images.chinatimes.com/newsphoto/2022-12-22/656/20221222003289.png",
    "https://cdn-next.cybassets.com/s/files/7445/ckeditor/pictures/content_a9aa1ec1-6af0-41da-938b-afbc6bfa75ed.png",
    "https://img.news.ebc.net.tw/EbcNews/news/2022/02/23/1645608006_92140.jpg",
    "https://images.chinatimes.com/newsphoto/2022-11-16/656/20221116004018.jpg",
  ];
  images.value = [
    ...Array.from({ length: 100 }, (value, index) => imageUrls[index % 8]).sort(
      () => Math.random() - 0.5,
    ),
  ];
};

init();

/*
  1. 監聽loading元素，isIntersecting 為當看到loading時候
  2. 當看到loading元素時候就可以增加6張圖片
  3. 當前 currentLength.value >= images.value.length，loading即可消失，監聽也就會消失
*/

useIntersectionObserver(loading, ([{ isIntersecting }]) => {
  console.log("loading畫面是否出現", isIntersecting);
  if (isIntersecting) {
    if (currentLength.value < images.value.length) {
      setTimeout(() => (currentLength.value += 6), 1000);
    } else {
      loadingIsVisible.value = false;
    }
  }
});
const filterImages = computed(() => images.value.slice(0, currentLength.value));
</script>
<template>
  <div class="container relative mx-auto p-4">
    <div class="grid grid-cols-2 justify-items-center gap-4">
      <div
        class="max-w-lg rounded-lg border border-gray-200 bg-white shadow"
        v-for="(url, index) in filterImages"
      >
        <img class="rounded-lg" :src="url" alt="" />
      </div>
    </div>
    <!--loading -->
    <div
      class="absolute bottom-0 right-1/2 flex translate-x-1/2 translate-y-1/2 items-center justify-center"
      ref="loading"
      v-show="loadingIsVisible"
    >
      <div role="status">
        <svg
          aria-hidden="true"
          class="h-12 w-12 animate-spin fill-blue-600 text-gray-200"
          viewBox="0 0 100 101"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z"
            fill="currentColor"
          />
          <path
            d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z"
            fill="currentFill"
          />
        </svg>
        <span class="sr-only">Loading...</span>
      </div>
    </div>
  </div>
</template>
<style scoped></style>
