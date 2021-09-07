import Cookies from 'js-cookie'

const TokenKey = 'Admin-Token'

export function getToken() {
  return Cookies.get(TokenKey) ? JSON.parse(Cookies.get(TokenKey)) : null
}

export function setToken(token) {
  return Cookies.set(TokenKey, JSON.stringify(token))
}

export function removeToken() {
  return Cookies.remove(TokenKey)
}
