import http from "k6/http";
import { check, sleep } from "k6";

export let options = {
  vus: 500, // 虛擬用戶數
  duration: "20s", // 測試持續時間
};

export default function () {
  // 模擬對使用 SemaphoreSlim 的 API 進行請求
  let response = http.get(
    "https://localhost:7290/WeatherForecast/SemaphoreTest"
  );

  // 檢查響應是否成功
  check(response, {
    "status is 200": (r) => r.status === 200,
    "response time < 500ms": (r) => r.timings.duration < 500,
  });

  // 模擬用戶思考時間
  sleep(1);
}
