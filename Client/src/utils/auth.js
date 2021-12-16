const TokenKey = 'Admin-Token'

export function getToken() {
  return localStorage.getItem(TokenKey) ? JSON.parse(localStorage.getItem(TokenKey)) : null
}

export function setToken(token) {
  return localStorage.setItem(TokenKey, JSON.stringify(token))
}

export function removeToken() {
  return localStorage.removeItem(TokenKey)
}
