import { login, logout, getInfo, refreshToken } from '@/api/user'
import { getToken, setToken, removeToken } from '@/utils/auth'
import router, { resetRouter } from '@/router'
import moment from 'moment'

const state = {
  token: getToken(),
  name: '',
  avatar: '',
  introduction: '',
  roles: []
}

const mutations = {
  SET_TOKEN: (state, token) => {
    state.token = token
  },
  SET_INTRODUCTION: (state, introduction) => {
    state.introduction = introduction
  },
  SET_NAME: (state, name) => {
    state.name = name
  },
  SET_AVATAR: (state, avatar) => {
    state.avatar = avatar
  },
  SET_ROLES: (state, roles) => {
    state.roles = roles
  }
}

const actions = {
  // user login
  login({ commit }, userInfo) {
    const { email, password } = userInfo
    return new Promise((resolve, reject) => {
      login({ email: email.trim(), password: password }).then(response => {
        const tempToken = {}
        tempToken.expireAt = moment().add(response.expiresIn, 'm').format()
        tempToken.accessToken = response.token
        tempToken.expiresIn = response.expiresIn
        tempToken.refreshToken = response.refreshToken
        tempToken.userId = response.userId
        commit('SET_TOKEN', tempToken)
        setToken(tempToken)
        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // get user info
  getInfo({ commit, state }) {
    return new Promise((resolve, reject) => {
      getInfo().then(response => {
        console.log('ressss', response)

        if (!response) {
          reject('Verification failed, please Login again.')
        }

        const { roles, firstName, image, note } = response

        // roles must be a non-empty array
        if (!roles || roles.length <= 0) {
          reject('getInfo: roles must be a non-null array!')
        }

        commit('SET_ROLES', roles)
        commit('SET_NAME', firstName)
        commit('SET_AVATAR', image)
        commit('SET_INTRODUCTION', note)
        resolve(response)
      }).catch(error => {
        reject(error)
      })
    })
  },

  // user logout
  logout({ commit, state, dispatch }) {
    return new Promise((resolve, reject) => {
      logout().then(() => {
        commit('SET_TOKEN', '')
        commit('SET_ROLES', [])
        removeToken()
        resetRouter()

        // reset visited views and cached views
        // to fixed https://github.com/PanJiaChen/vue-element-admin/issues/2485
        dispatch('tagsView/delAllViews', null, { root: true })

        resolve()
      }).catch(error => {
        reject(error)
      })
    })
  },

  // remove token
  resetToken({ commit }) {
    return new Promise(resolve => {
      commit('SET_TOKEN', '')
      commit('SET_ROLES', [])
      removeToken()
      resolve()
    })
  },

  // Refresh token
  refreshToken({ state, commit }) {
    return new Promise((resolve, reject) => {
      const token = state.token
      const refresh = {
        refreshToken: token.refreshToken,
        userId: token.userId
      }
      refreshToken(refresh).then(res => {
        console.log('refresh data')
        console.log(res)
        token.expireAt = moment().add(res.expiresIn, 'm').format()
        token.expiresIn = res.expiresIn
        token.accessToken = res.accessToken
        token.refreshToken = res.refreshToken
        setToken(token)
        commit('SET_TOKEN', token)
        resolve(token)
      }).catch(err => {
        reject(err)
      })
    })
  },

  // dynamically modify permissions
  async changeRoles({ commit, dispatch }, role) {
    const token = role + '-token'

    commit('SET_TOKEN', token)
    setToken(token)

    const { roles } = await dispatch('getInfo')

    resetRouter()

    // generate accessible routes map based on roles
    const accessRoutes = await dispatch('permission/generateRoutes', roles, { root: true })
    // dynamically add accessible routes
    router.addRoutes(accessRoutes)

    // reset visited views and cached views
    dispatch('tagsView/delAllViews', null, { root: true })
  }
}

export default {
  namespaced: true,
  state,
  mutations,
  actions
}
