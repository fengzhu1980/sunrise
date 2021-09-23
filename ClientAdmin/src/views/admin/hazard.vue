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
            @keyup.enter.native="getHazard"
          />
        </div>
        <div class="app__search-btn">
          <el-button
            class="filter-item shadow"
            type="primary"
            icon="el-icon-search"
            @click="getHazard"
          >Search</el-button>
        </div>
      </div>
      <div class="app__search-right">
        <div>
          <el-button class="shadow" type="primary" @click="handleAddNewHazard">Add New Hazard</el-button>
        </div>
      </div>
    </div>
    <!-- Search row end -->
    <!-- Hazard list start -->
    <el-table v-loading="listLoading" :max-height="winTableHeight" :data="hazardList" border fit>
      <el-table-column type="index" width="50" align="center" />
      <el-table-column label="Title" width="250" align="center">
        <template slot-scope="scope">
          <span class="link-type" @click="handleUpdateHazard(scope.row, 'browse')">{{ scope.row.title }}</span>
        </template>
      </el-table-column>
      <el-table-column label="Description" prop="description" align="center" />
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
            @click="handleUpdateHazard(scope.row, 'update')"
          >Edit</el-button>
          <el-button v-if="scope.row.isActive" type="danger" size="mini" @click="changeHazardStatus(scope.row, false)">Inactivate</el-button>
          <el-button v-else type="success" size="mini" @click="changeHazardStatus(scope.row, true)">Activate</el-button>
        </template>
      </el-table-column>
    </el-table>
    <!-- Hazard list end -->
    <!-- Pagination start -->
    <pagination
      v-show="total > 0"
      ref="pagination"
      :total="total"
      :page.sync="queryParams.pageNo"
      :limit.sync="queryParams.pageSize"
      :page-sizes="[20, 50, 100]"
      @pagination="getHazard"
    />
    <!-- Pagination end -->
    <!-- Hazard dialog start -->
    <el-dialog
      :visible.sync="hazardDialogVisible"
      :title="titleMap[dialogStatus]"
      @closed="handleHazardDialogClosed"
    >
      <i-create-hazard
        :dialog-status="dialogStatus"
        :form="hazardForm"
        @handleCancelClick="handleCancelClick"
        @handleActionBtnClick="handleActionBtnClick"
      />
    </el-dialog>
    <!-- Hazard dialog end -->
  </div>
</template>

<script>
import { getHazardList, updateHazard } from '@/api/admin'
import Pagination from '@/components/Pagination'
import CreateHazard from '@/views/admin/components/createHazard'

export default {
  name: 'HazardManagement',
  components: {
    Pagination,
    iCreateHazard: CreateHazard
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
      hazardForm: {
        id: undefined,
        title: '',
        notes: '',
        isActive: true
      },
      hazardDialogVisible: false,
      hazardList: [],
      winTableHeight: 800
    }
  },
  created() {
    this.getHazard()
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
    getHazard() {
      this.listLoading = true
      getHazardList(this.queryParams).then(res => {
        this.total = res.count
        this.hazardList = res.data
        this.listLoading = false
      }).catch(e => {
        this.$message.error('Get hazard failed ' + e)
        this.listLoading = false
      })
    },
    resetTemp() {
      this.hazardForm = {
        id: undefined,
        title: '',
        notes: '',
        isActive: true
      }
    },
    handleAddNewHazard() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.hazardDialogVisible = true
    },
    handleUpdateHazard(row, type) {
      this.hazardForm = Object.assign({}, row) // copy obj
      this.dialogStatus = type
      this.hazardDialogVisible = true
    },
    handleHazardDialogClosed() {
      this.resetTemp()
    },
    changeHazardStatus(val, status) {
      const action = status ? 'Activate' : 'Inactivate'
      this.$confirm(
        'Are you sure you want to ' + action + ' hazard ' +
          val.title +
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
          title: val.title,
          isActive: status
        }
        updateHazard(params).then(res => {
          this.$message({
            type: 'success',
            message: action + ' successfully!'
          })
          this.getHazard()
        }).catch(err => {
          this.$message({
            type: 'error',
            message: action + ' Failed. ' + err
          })
        })
      }).catch(() => {})
    },
    handleCancelClick() {
      this.hazardDialogVisible = false
    },
    handleActionBtnClick() {
      this.hazardDialogVisible = false
      this.getHazard()
    }
  }
}
</script>
<style lang="scss" scoped>
</style>
