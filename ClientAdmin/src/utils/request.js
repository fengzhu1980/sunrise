import axios from 'axios'
import { MessageBox, Message } from 'element-ui'
import store from '@/store'
import { getToken } from '@/utils/auth'
import router from '@/router'
import moment from 'moment'

// 是否有请求正在刷新token
window.isRefreshing = false
// 被挂起的请求数组
let refreshSubscribers = []

// push所有请求到数组中
function subscribeTokenRefresh(cb) {
  // console.log(cb)
  refreshSubscribers.push(cb)
}

// 刷新请求（refreshSubscribers数组中的请求得到新的token之后会自执行，用新的token去请求数据）
function onRefreshed(token) {
  refreshSubscribers.map(cb => cb(token))
}

// create an axios instance
const service = axios.create({
  baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  // withCredentials: true, // send cookies when cross-domain requests
  timeout: 50000 // request timeout
})

// request interceptor
service.interceptors.request.use(
  config => {
    // do something before request is sent
    const tempToken = getToken()
    if (store.getters.token) {
      // let each request carry token
      // ['X-Token'] is a custom headers key
      // please modify it according to the actual situation
      config.headers['Authorization'] = 'bearer ' + tempToken.accessToken
      // Check token is expired or not
      const expireAt = moment(tempToken.expireAt)
      if (expireAt.clone().isSameOrBefore(moment())) {
        // Log out
        store.dispatch('user/resetToken').then(_ => {
          // sessionStorage.removeItem()
          router.push('/login')
          Message({
            message: 'Token expired',
            type: 'error',
            duration: 5 * 1000
          })
        })
        return
      } else {
        // 判断token是否即将过期
        // `/auth/current`是刷新token的接口，只有当token将要过期且不是请求刷新token的接口，并且不是请求aws地址才会进入
        if (expireAt.clone().subtract(10, 'm').isSameOrBefore(moment()) && !config.url.includes('/api/account/login')) {
          // 首先所有的请求来了，我们要先判断当前是否正在刷新，如果不是，将刷新的标志置为true并请求刷新token；如果是，将请求存储到数组中
          if (!window.isRefreshing) {
            window.isRefreshing = true
            store.dispatch('user/refreshToken').then(token => {
              window.isRefreshing = false
              onRefreshed(token)
              refreshSubscribers = []
            }).catch(_ => {
              store.dispatch('user/resetToken').then(() => {
                router.push('/login')
                Message({
                  message: 'Token expired',
                  type: 'error',
                  duration: 5 * 1000
                })
              })
            })
            const retry = new Promise((resolve, reject) => {
              // (token) => {...}这个函数就是cb
              subscribeTokenRefresh((token) => {
                // request.headers.Authorization = 'Bearer ' + token
                config.headers['Authorization'] = 'bearer ' + token.accessToken
                // config.headers['Authorization'] = 'bearer ' + sessionStorage.get('access_token')
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
      }
    }
    return config
  },
  error => {
    // do something with request error
    console.log(error) // for debug
    return Promise.reject(error)
  }
)

// response interceptor
service.interceptors.response.use(
  /**
   * If you want to get http information such as headers or status
   * Please return  response => response
  */

  /**
   * Determine the request status by custom code
   * Here is just an example
   * You can also judge the status by HTTP Status Code
   */
  response => {
    const res = response.data
    console.log('res', res)

    // if the custom code is not 20000, it is judged as an error.
    if (response.status !== 200) {
      Message({
        message: response.statusText || 'Error',
        type: 'error',
        duration: 5 * 1000
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
  },
  error => {
    console.log('err ' + error) // for debug
    console.log('error.response', error.response)
    if (error.response) {
      const resp = error.response
      var errorMessage = error.message
      if (resp.status === 400) {
        if (resp.data.errors) {
          errorMessage = resp.data.errors[0]
        }
      }
      if (resp.status === 401) {
        errorMessage = `${resp.data.message}, ${resp.data.statusCode}`
      }
      if (resp.status === 404) {
        errorMessage = `${resp.data.message}, ${resp.data.statusCode}`
        // this.router.navigateByUrl('/not-found')
      }
      if (resp.status === 500) {
        errorMessage = `${resp.data.message}, ${resp.data.statusCode}`
      }
      Message({
        message: errorMessage,
        type: 'error',
        duration: 5 * 1000
      })
    }
    return Promise.reject(error)
  }
)

export default service
