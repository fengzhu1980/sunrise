import request from '@/utils/request'

export function getJobsByFilter(params) {
  return request({
    url: '/api/jobs/getJobsByFilter',
    method: 'get',
    params
  })
}

export function getJobById(params) {
  return request({
    url: '/api/jobs/getJobById',
    method: 'get',
    params
  })
}

export function createNewJob(data) {
  return request({
    url: '/api/jobs/createNewJob',
    method: 'post',
    data: data
  })
}

export function updateJob(data) {
  return request({
    url: '/api/jobs/updateJob',
    method: 'post',
    data: data
  })
}

// Job Stage
export function getAllJobStages() {
  return request({
    url: '/api/jobs/getAllJobStages',
    method: 'get'
  })
}

export function getAllJobStagesByFilter(params) {
  return request({
    url: '/api/jobs/getAllJobStagesByFilter',
    method: 'get',
    params
  })
}

export function createNewJobStage(data) {
  return request({
    url: '/api/jobs/createNewJobStage',
    method: 'post',
    data: data
  })
}

export function updateJobStage(data) {
  return request({
    url: '/api/jobs/updateJobStage',
    method: 'post',
    data: data
  })
}

export function getImgByUrlFromServer(url) {
  return request({
    url,
    method: 'get',
    responseType: 'blob'
  })
}
