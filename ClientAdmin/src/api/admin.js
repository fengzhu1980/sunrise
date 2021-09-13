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

// Task Management
export function getTaskList(params) {
  return request({
    url: '/api/admin/getTaskList',
    method: 'get',
    params
  })
}

export function addNewTask(data) {
  return request({
    url: '/api/admin/addNewTask',
    method: 'post',
    data: data
  })
}

export function updateTask(data) {
  return request({
    url: '/api/admin/updateTask',
    method: 'post',
    data: data
  })
}

// Role Management
export function getAllRoles() {
  return request({
    url: '/api/admin/getAllRoles',
    method: 'get'
  })
}

export function getAllRolesByFilter(params) {
  return request({
    url: '/api/admin/getAllRolesByFilter',
    method: 'get',
    params
  })
}

export function createNewRole(data) {
  return request({
    url: '/api/admin/createNewRole',
    method: 'post',
    data: data
  })
}

export function updateRole(data) {
  return request({
    url: '/api/admin/updateRole',
    method: 'post',
    data: data
  })
}

// Document Management
export function uploadDocument(uploadRequest) {
  return request({
    url: '/api/admin/addDocument',
    method: 'post',
    data: uploadRequest.data,
    onUploadProgress: uploadRequest.onUploadProgress
  })
}

export function deleteDocumentById(data) {
  return request({
    url: '/api/admin/deleteDocumentById',
    method: 'post',
    data: data
  })
}
