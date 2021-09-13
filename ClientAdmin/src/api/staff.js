import request from '@/utils/request'

// Staff Management
export function getAllStaffs() {
  return request({
    url: '/api/staff/getAllStaffs',
    method: 'get'
  })
}

export function getStaffsByFilter(params) {
  return request({
    url: '/api/staff/getStaffsByFilter',
    method: 'get',
    params
  })
}

export function getStaffById(params) {
  return request({
    url: '/api/staff/getStaffById',
    method: 'get',
    params
  })
}

export function addNewStaff(data) {
  return request({
    url: '/api/staff/addNewStaff',
    method: 'post',
    data: data
  })
}

export function updateStaff(data) {
  return request({
    url: '/api/staff/updateStaff',
    method: 'post',
    data: data
  })
}
