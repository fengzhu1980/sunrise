import moment from 'moment'

export default boot(({ app }) => {
    app.config.globalProperties.$moment = moment
  })
  
  export { moment }