import request from '@/utils/request'

// SWMS Management
export function getSWMSList(params) {
  return request({
    url: '/api/admin/getSWMSList',
    method: 'get',
    params
  })
}

export function addNewSWMS(data) {
  return request({
    url: '/api/admin/addNewSWMS',
    method: 'post',
    data: data
  })
}

export function updateSWMS(data) {
  return request({
    url: '/api/admin/updateSWMS',
    method: 'post',
    data: data
  })
}

export function updateSWMSStatus(data) {
  return request({
    url: '/api/admin/updateSWMSStatus',
    method: 'post',
    data: data
  })
}

// Hazard Management
export function getHazardList(params) {
  return request({
    url: '/api/admin/getHazardList',
    method: 'get',
    params
  })
}

export function addNewHazard(data) {
  return request({
    url: '/api/admin/addNewHazard',
    method: 'post',
    data: data
  })
}

export function updateHazard(data) {
  return request({
    url: '/api/admin/updateHazard',
    method: 'post',
    data: data
  })
}
