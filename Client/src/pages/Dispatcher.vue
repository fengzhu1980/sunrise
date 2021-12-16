<template>
  <div></div>
</template>

<script>
import { getToken } from '../utils/auth'

export default {
  created () {
    let token = getToken()
    console.log('token', token)
    if (token) {
      this.$router.replace('/layout')
    } else {
      this.$router.replace('/login')
    }
  }
}
</script>
