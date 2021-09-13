import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/api/account/login',
    method: 'post',
    data
  })
}

// export function getInfo(id) {
//   return request({
//     url: '/api/staff/getStaffByUserId',
//     method: 'get',
//     params: { id }
//   })
// }

export function getInfo() {
  return request({
    url: '/api/staff/getInfo',
    method: 'get'
  })
}

export function logout() {
  return request({
    url: '/api/account/logout',
    method: 'post'
  })
}

export function refreshToken(data) {
  return request({
    url: '/api/account/refresh',
    method: 'post',
    data: {
      userId: data.userId,
      token: data.refreshToken
    }
  })
}

// Email
export function emailexists(params) {
  return request({
    url: '/api/account/emailexists',
    method: 'get',
    params
  })
}
