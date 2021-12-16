import { boot } from 'quasar/wrappers'
import axios from 'axios'
import { getToken, removeToken, setToken } from '../utils/auth'
import { refreshToken } from '../api/user'
import router from '../router'
import moment from 'moment'

// Be careful when using SSR for cross-request state pollution
// due to creating a Singleton instance here;
// If any client changes this (global) instance, it might be a
// good idea to move this instance creation inside of the
// "export default () => {}" function below (which runs individually
// for each client)
const api = axios.create({
  timeout: 50000,
  baseURL: 'https://localhost:5001/'
})

// 是否有请求正在刷新token
window.isRefreshing = false
// 被挂起的请求数组
let refreshSubscribers = []

// push所有请求到数组中
function subscribeTokenRefresh (cb) {
  refreshSubscribers.push(cb)
}

// 刷新请求（refreshSubscribers数组中的请求得到新的token之后会自执行，用新的token去请求数据）
function onRefreshed (token) {
  refreshSubscribers.map(cb => cb(token))
}

// 添加请求拦截
api.interceptors.request.use(config => {
  // 在发送请求之前做些什么
  config.headers['Accept'] = 'application/json'
  const token = getToken()
  console.log(token)
  // If has token
  if (token) {
    config.headers['Authorization'] = 'bearer ' + token.accessToken
    // Check token is expired or not
    const expire = moment(token.expiresAt)
    // console.log(expire)
    // Token expired
    if (expire.clone().isSameOrBefore(moment())) {
      removeToken()
      router.push('/login')
    }

    // 判断token是否即将过期
    // `/auth/current`是刷新token的接口，只有当token将要过期且不是请求刷新token的接口，并且不是请求aws地址才会进入
    if (expire.clone().subtract(20, 'm').isSameOrBefore(moment()) && config.url !== '/auth/token/refresh' && config.url !== '/file/aws/info') {
      // 首先所有的请求来了，我们要先判断当前是否正在刷新，如果不是，将刷新的标志置为true并请求刷新token；如果是，将请求存储到数组中
      if (!window.isRefreshing) {
        window.isRefreshing = true
        refreshToken({ userId: token.userId, refreshToken: token.refreshToken }).then(res => {
          window.isRefreshing = false
          // 将刷新的token替代老的token
          // request.headers.Authorization = 'Bearer ' + res.token
          config.headers['Authorization'] = 'bearer ' + res.accessToken
          setToken(res)
          // 更新内存的auth
          // 执行数组里的请求，重新发起被挂起的请求
          onRefreshed(res)
          refreshSubscribers = []
        }).catch(err => {
          console.log(err)
          // window.location.href = '/login'
          router.push('/login')
        })
        let retry = new Promise((resolve, reject) => {
          // (token) => {...}这个函数就是cb
          subscribeTokenRefresh((token) => {
            // console.log(token)
            config.headers['Authorization'] = 'bearer ' + token.accessToken
            // localStore.set('token', token)
            // 将请求挂起
            resolve(config)
          })
        })
        return retry
      }
      // return config
    } else {
      return config
    }
  } else {
    // If there is no token
    router.push('/login')
  }
  if (process.env.NODE_ENV === 'development') {
    console.log(config)
  }
  return config
}, error => {
  // 对请求错误做些什么
  if (process.env.NODE_ENV === 'development') {
    console.warn(error)
  }
  return Promise.reject(error)
})

// 添加响应拦截器
api.interceptors.response.use(response => {
  // 对响应数据做点什么
  const res = response.data
  // if the custom code is not 200, it is judged as an error.
  if (response.status !== 200) {
    const $q = useQuasar()
    $q.notify({
      color: 'negative',
      position: 'top',
      message: response.statusText || 'Error',
      icon: 'report_problem'
    })

    // 50008: Illegal token; 50012: Other clients logged in; 50014: Token expired;
    if (res.code === 50008 || res.code === 50012 || res.code === 50014) {
      // to re-login
      MessageBox.confirm('You have been logged out, you can cancel to stay on this page, or log in again', 'Confirm logout', {
        confirmButtonText: 'Re-Login',
        cancelButtonText: 'Cancel',
        type: 'warning'
      }).then(() => {
        store.dispatch('user/resetToken').then(() => {
          location.reload()
        })
      })
    }
    return Promise.reject(new Error(response.statusText || 'Error'))
  } else {
    return res
  }
}, error => {
  // 对响应错误做点什么
  if (process.env.NODE_ENV === 'development') {
    console.warn(error.response)
    console.log(error)
  }
  return Promise.reject(error)
})

export default boot(({ app }) => {
  // for use inside Vue files (Options API) through this.$axios and this.$api

  app.config.globalProperties.$axios = axios
  // ^ ^ ^ this will allow you to use this.$axios (for Vue Options API form)
  //       so you won't necessarily have to import axios in each vue file

  app.config.globalProperties.$api = api
  // ^ ^ ^ this will allow you to use this.$api (for Vue Options API form)
  //       so you can easily perform requests against your app's API
})

export { api }
