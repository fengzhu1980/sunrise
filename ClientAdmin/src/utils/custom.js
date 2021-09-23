export function getImageUrl(fileName) {
  const baseURL = process.env.VUE_APP_BASE_API
  console.log('url', baseURL)
  console.log('fileName', fileName)
  if (!fileName) {
    fileName = 'files/default.png'
  }
  return baseURL + fileName
}

export function utcToLocalDate(utc) {
  let result = ''
  if (utc) {
    result = this.$moment.utc(utc).local().format('Do MMM YYYY')
  }
  return result
}

export function utcToLocalDateSlash(utc) {
  let result = ''
  if (utc) {
    result = this.$moment.utc(utc).local().format('DD/MM/YYYY')
  }
  return result
}

export function utcToLocalDateTimeL(utc) {
  let result = ''
  if (utc) {
    result = this.$moment.utc(utc).local().format('Do MMM YYYY, h:mm A')
  }
  return result
}

export function utcToLocalDateTimeS(utc) {
  let result = ''
  if (utc) {
    result = this.$moment.utc(utc).local().format('Do MMM YYYY, H:mm')
  }
  return result
}

export function utcToLocalDateTimeSmall(utc) {
  let result = ''
  if (utc) {
    result = this.$moment.utc(utc).local().format('DD MMM YYYY H:mm')
  }
  return result
}

export function utcToLocalDateTimeSmallComma(utc) {
  let result = ''
  if (utc) {
    result = this.$moment.utc(utc).local().format('YYYY/MM/DD H:mm')
  }
  return result
}
