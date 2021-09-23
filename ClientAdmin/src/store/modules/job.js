const state = {
  jobId: null
}

const mutations = {
  SET_JOB_ID: (state, id) => {
    state.jobId = id
  }
}

export default {
  namespaced: true,
  state,
  mutations
}
