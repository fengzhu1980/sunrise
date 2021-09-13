<template>
  <div class="app-container">
    <!-- Search row start -->
    <div ref="search" class="app__search">
      <div class="app__search-left">
        <div class="app__search-keyword">
          <el-input
            v-model="queryParams.keyword"
            clearable
            class="filter-item"
            placeholder="Name, Notes..."
            @keyup.enter.native="getStaff"
          />
        </div>
        <div class="app__search-btn">
          <el-button
            class="filter-item shadow"
            type="primary"
            icon="el-icon-search"
            @click="getStaff"
          >Search</el-button>
        </div>
      </div>
      <div class="app__search-right">
        <div>
          <el-button class="shadow" type="primary" @click="handleAddNewStaff">Add New Staff</el-button>
        </div>
      </div>
    </div>
    <!-- Search row end -->
    <!-- Staff list start -->
    <el-table v-loading="listLoading" :max-height="winTableHeight" :data="staffList" border fit>
      <el-table-column type="index" width="50" align="center" />
      <el-table-column label="First Name" width="150" align="center">
        <template slot-scope="scope">
          <span class="link-type" @click="handleUpdateStaff(scope.row, 'browse')">{{ scope.row.firstName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="Last Name" prop="lastName" align="center" />
      <el-table-column label="Email" prop="email" align="center" />
      <el-table-column label="Mobile" prop="mobile" align="center" />
      <el-table-column label="Phone Number" prop="phoneNumber" align="center" />
      <el-table-column label="Roles" prop="roles" align="center">
        <template slot-scope="scope">
          <el-tag
            v-for="(role, index) in scope.row.roles"
            :key="index"
            class="staff__tag"
            type="primary"
          >{{ role }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="Is Active" width="100" align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.isActive | statusFilter">{{ scope.row.isActive | isActiveFilter }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column fixed="right" label="Operations" align="center" width="200">
        <template slot-scope="scope">
          <el-button
            type="primary"
            size="mini"
            @click="handleUpdateStaff(scope.row, 'update')"
          >Edit</el-button>
          <el-button v-if="scope.row.isActive" type="danger" size="mini" @click="changeStaffStatus(scope.row, false)">Inactivate</el-button>
          <el-button v-else type="success" size="mini" @click="changeStaffStatus(scope.row, true)">Activate</el-button>
        </template>
      </el-table-column>
    </el-table>
    <!-- Staff list end -->
    <!-- Pagination start -->
    <pagination
      v-show="total > 0"
      ref="pagination"
      :total="total"
      :page.sync="queryParams.pageNo"
      :limit.sync="queryParams.pageSize"
      :page-sizes="[20, 50, 100]"
      @pagination="getStaff"
    />
    <!-- Pagination end -->
    <!-- Staff dialog start -->
    <el-dialog
      :visible.sync="staffDialogVisible"
      :title="titleMap[dialogStatus]"
      @closed="handleStaffDialogClosed"
    >
      <i-create-staff
        :dialog-status="dialogStatus"
        :form="staffForm"
        @handleCancelClick="handleCancelClick"
        @handleActionBtnClick="handleActionBtnClick"
      />
    </el-dialog>
    <!-- Staff dialog end -->
  </div>
</template>

<script>
import { getStaffsByFilter, updateStaff } from '@/api/staff'
import Pagination from '@/components/Pagination'
import CreateStaff from '@/views/admin/components/createStaff'

export default {
  name: 'StaffManagement',
  components: {
    Pagination,
    iCreateStaff: CreateStaff
  },
  filters: {
    isActiveFilter(value) {
      const dataMap = {
        false: 'No',
        true: 'Yes'
      }
      return dataMap[value]
    },
    disableFilter(status) {
      const statusMap = {
        true: 'Active',
        false: 'Inactive'
      }
      return statusMap[status]
    },
    statusFilter(status) {
      const statusMap = {
        false: 'danger',
        true: 'success'
      }
      return statusMap[status]
    }
  },
  data() {
    return {
      total: 0,
      listLoading: false,
      queryParams: {
        pageSize: 20,
        pageNo: 1,
        keyword: ''
      },
      dialogStatus: '',
      titleMap: {
        update: 'Edit',
        create: 'Create',
        browse: 'Browse'
      },
      staffForm: {
        id: undefined,
        firstName: '',
        middleName: '',
        lastName: '',
        mobile: '',
        phoneNumber: '',
        email: '',
        staffCode: '',
        gender: '',
        address: '',
        isAdmin: false,
        image: '',
        documentId: '',
        note: '',
        isActive: true,
        password: '',
        roleIds: []
      },
      staffDialogVisible: false,
      staffList: [],
      winTableHeight: 800
    }
  },
  created() {
    this.getStaff()
  },
  updated() {
    this.$nextTick(() => {
      this.winTableHeight =
        window.innerHeight -
        this.$refs.search.clientHeight -
        this.$refs.pagination.$el.clientHeight -
        140
    })
  },
  methods: {
    getStaff() {
      this.listLoading = true
      getStaffsByFilter(this.queryParams).then(res => {
        this.total = res.count
        this.staffList = res.data
        this.listLoading = false
      }).catch(e => {
        this.$message.error('Get staff failed ' + e)
        this.listLoading = false
      })
    },
    resetTemp() {
      this.staffForm = {
        id: undefined,
        firstName: '',
        middleName: '',
        lastName: '',
        mobile: '',
        phoneNumber: '',
        email: '',
        staffCode: '',
        gender: '',
        address: '',
        isAdmin: false,
        image: '',
        documentId: '',
        note: '',
        isActive: true,
        password: '',
        roleIds: []
      }
    },
    handleAddNewStaff() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.staffDialogVisible = true
    },
    handleUpdateStaff(row, type) {
      this.staffForm = Object.assign({}, row) // copy obj
      this.dialogStatus = type
      this.staffDialogVisible = true
    },
    handleStaffDialogClosed() {
      this.resetTemp()
    },
    changeStaffStatus(val, status) {
      const action = status ? 'Activate' : 'Inactivate'
      this.$confirm(
        'Are you sure you want to ' + action + ' staff ' +
          val.firstName +
          ' ?',
        'Warning',
        {
          confirmButtonText: action,
          cancelButtonText: 'Cancel',
          type: 'warning'
        }
      ).then(() => {
        const params = {
          id: val.id,
          firstName: val.firstName,
          email: val.email,
          roleIds: val.roleIds,
          isActive: status
        }
        updateStaff(params).then(res => {
          this.$message({
            type: 'success',
            message: action + ' successfully!'
          })
          this.getStaff()
        }).catch(err => {
          this.$message({
            type: 'error',
            message: action + ' Failed. ' + err
          })
        })
      }).catch(() => {})
    },
    handleCancelClick() {
      this.staffDialogVisible = false
    },
    handleActionBtnClick() {
      this.staffDialogVisible = false
      this.getStaff()
    }
  }
}
</script>
<style lang="scss" scoped>
.staff {
  &__tag {
    margin: .2rem;
  }
}
</style>
