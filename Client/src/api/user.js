import { api } from 'boot/axios'

export function login(data) {
  return api({
    url: '/api/account/login',
    method: 'post',
    data
  })
}

export function getInfo() {
  return api({
    url: '/api/staff/getInfo',
    method: 'get'
  })
}

export function logout() {
  return api({
    url: '/api/account/logout',
    method: 'post'
  })
}

export function refreshToken(data) {
  return api({
    url: '/api/account/refresh',
    method: 'post',
    data: {
      userId: data.userId,
      token: data.refreshToken
    }
  })
}
