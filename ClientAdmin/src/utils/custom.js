export function getImageUrl(fileName) {
  const baseURL = process.env.VUE_APP_BASE_API
  if (!fileName) {
    fileName = 'files/default.png'
  }
  return baseURL + fileName
}
