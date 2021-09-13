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
            @keyup.enter.native="getRole"
          />
        </div>
        <div class="app__search-btn">
          <el-button
            class="filter-item shadow"
            type="primary"
            icon="el-icon-search"
            @click="getRole"
          >Search</el-button>
        </div>
      </div>
      <div class="app__search-right">
        <div>
          <el-button class="shadow" type="primary" @click="handleAddNewRole">Add New Role</el-button>
        </div>
      </div>
    </div>
    <!-- Search row end -->
    <!-- Role list start -->
    <el-table v-loading="listLoading" :max-height="winTableHeight" :data="roleList" border fit>
      <el-table-column type="index" width="50" align="center" />
      <el-table-column label="Name" width="250" align="center">
        <template slot-scope="scope">
          <span class="link-type" @click="handleUpdateRole(scope.row, 'browse')">{{ scope.row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="Note" prop="note" align="center" />
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
            @click="handleUpdateRole(scope.row, 'update')"
          >Edit</el-button>
          <el-button v-if="scope.row.isActive" type="danger" size="mini" @click="changeRoleStatus(scope.row, false)">Inactivate</el-button>
          <el-button v-else type="success" size="mini" @click="changeRoleStatus(scope.row, true)">Activate</el-button>
        </template>
      </el-table-column>
    </el-table>
    <!-- Role list end -->
    <!-- Pagination start -->
    <pagination
      v-show="total > 0"
      ref="pagination"
      :total="total"
      :page.sync="queryParams.pageNo"
      :limit.sync="queryParams.pageSize"
      :page-sizes="[20, 50, 100]"
      @pagination="getRole"
    />
    <!-- Pagination end -->
    <!-- Role dialog start -->
    <el-dialog
      :visible.sync="roleDialogVisible"
      :title="titleMap[dialogStatus]"
      @closed="handleRoleDialogClosed"
    >
      <i-create-role
        :dialog-status="dialogStatus"
        :form="roleForm"
        @handleCancelClick="handleCancelClick"
        @handleActionBtnClick="handleActionBtnClick"
      />
    </el-dialog>
    <!-- Role dialog end -->
  </div>
</template>

<script>
import { getAllRolesByFilter, updateRole } from '@/api/admin'
import Pagination from '@/components/Pagination'
import CreateRole from '@/views/admin/components/createRole'

export default {
  name: 'RoleManagement',
  components: {
    Pagination,
    iCreateRole: CreateRole
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
      roleForm: {
        id: undefined,
        name: '',
        note: '',
        isActive: true
      },
      roleDialogVisible: false,
      roleList: [],
      winTableHeight: 800
    }
  },
  created() {
    this.getRole()
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
    getRole() {
      this.listLoading = true
      getAllRolesByFilter(this.queryParams).then(res => {
        // this.total = res.count
        this.roleList = res.data
        this.listLoading = false
      }).catch(e => {
        this.$message.error('Get role failed ' + e)
        this.listLoading = false
      })
    },
    resetTemp() {
      this.roleForm = {
        id: undefined,
        name: '',
        note: '',
        isActive: true
      }
    },
    handleAddNewRole() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.roleDialogVisible = true
    },
    handleUpdateRole(row, type) {
      this.roleForm = Object.assign({}, row) // copy obj
      this.dialogStatus = type
      this.roleDialogVisible = true
    },
    handleRoleDialogClosed() {
      this.resetTemp()
    },
    changeRoleStatus(val, status) {
      const action = status ? 'Activate' : 'Inactivate'
      this.$confirm(
        'Are you sure you want to ' + action + ' role ' +
          val.name +
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
          name: val.name,
          isActive: status
        }
        updateRole(params).then(res => {
          this.$message({
            type: 'success',
            message: action + ' successfully!'
          })
          this.getRole()
        }).catch(err => {
          this.$message({
            type: 'error',
            message: action + ' Failed. ' + err
          })
        })
      }).catch(() => {})
    },
    handleCancelClick() {
      this.roleDialogVisible = false
    },
    handleActionBtnClick() {
      this.roleDialogVisible = false
      this.getRole()
    }
  }
}
</script>
<style lang="scss" scoped>
</style>
