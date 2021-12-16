<template>
  <q-layout class="q-pa-sm bg-primary">
    <div class="row justify-center q-mt-xl">
      <h5 class="text-h4 q-my-md text-white">Sunrise</h5>
    </div>
    <q-card class="my-card">
      <q-card-section class="q-subheading">
        <q-form
          @submit="onSubmit"
          @reset="onReset"
          class="q-gutter-md"
        >
          <q-input
            filled
            v-model="email"
            lable="Email"
            type="email"
            lazy-rules
            :rules="[ val => val && val.length > 0 || 'Email is required']"
          >
            <template v-slot:prepend>
              <q-icon name="email" color="primary" />
            </template>
            <template v-if="email" v-slot:append>
              <q-icon name="cancel" @click.stop="email = null" class="cursor-pointer" />
            </template>
          </q-input>

          <q-input
            filled
            v-model="password"
            lable="Password"
            lazy-rules
            :type="isPwd ? 'password' : 'text'"
            :rules="[
              val => val => val && val.length > 0 || 'Please type your password'
            ]"
          >
            <template v-slot:prepend>
              <q-icon name="password" color="primary" />
            </template>
            <template v-slot:append>
              <q-icon
                :name="isPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="isPwd = !isPwd"
              />
            </template>
          </q-input>

          <q-toggle v-model="remember" label="Remember me" />

          <div class="flex flex-center">
            <q-btn label="Submit" type="submit" color="primary"/>
          </div>
        </q-form>
      </q-card-section>
      <!-- <q-inner-loading :visible="loginFormLoading">
        <q-spinner size="50px" color="blue"></q-spinner>
      </q-inner-loading> -->
    </q-card>
  </q-layout>
</template>

<script>
import { login } from 'src/api/user'
import { getToken, removeToken, setToken } from '../utils/auth'

export default {
  name: 'Login',
  data() {
    return {
      isPwd: true,
      email: '',
      password: '',
      remember: false,
      loginFormLoading: false,
    }
  },
  created () {
    // Check if remember me
    // if (this.$localStore.get('USER_LOGIN_INFO')) {
    //   this.loginForm = this.$localStore.get('USER_LOGIN_INFO')
    // }
  },
  methods: {
    onSubmit () {
      this.loginFormLoading = true
      const loginForm = {
        email: this.email,
        password: this.password
      }
      login(loginForm).then(response => {
        const tempToken = {}
        tempToken.expireAt = this.$moment().add(response.expiresIn, 'm').format()
        tempToken.accessToken = response.token
        tempToken.expiresIn = response.expiresIn
        tempToken.refreshToken = response.refreshToken
        tempToken.userId = response.userId
        commit('SET_TOKEN', tempToken)
        setToken(tempToken)
        // this.$router.push({ path: this.redirect || '/', query: this.otherQuery })
        this.loginFormLoading = false
      }).catch(() => {
        this.loginFormLoading = false
      })
      // $q.notify({
      //   color: 'red-5',
      //   textColor: 'white',
      //   icon: 'warning',
      //   message: 'You need to remember the license and terms first'
      // })
      // $q.notify({
      //   color: 'green-4',
      //   textColor: 'white',
      //   icon: 'cloud_done',
      //   message: 'Submitted'
      // })
    },
    onReset () {
      this.email = null
      this.password = null
      this.remember = false
    }
  },
}
</script>

<style lang="scss" scoped>
.my-card {
  width: 100%;
  height: 100%;
  // max-width: 250px
}
</style>
