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
            @keyup.enter.native="getStage"
          />
        </div>
        <div class="app__search-btn">
          <el-button
            class="filter-item shadow"
            type="primary"
            icon="el-icon-search"
            @click="getStage"
          >Search</el-button>
        </div>
      </div>
      <div class="app__search-right">
        <div>
          <el-button class="shadow" type="primary" @click="handleAddNewStage">Add New Stage</el-button>
        </div>
      </div>
    </div>
    <!-- Search row end -->
    <!-- Stage list start -->
    <el-table v-loading="listLoading" :max-height="winTableHeight" :data="stageList" border fit>
      <el-table-column type="index" width="50" align="center" />
      <el-table-column label="Name" width="250" align="center">
        <template slot-scope="scope">
          <span class="link-type" @click="handleUpdateStage(scope.row, 'browse')">{{ scope.row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="Notes" prop="notes" align="center" />
      <el-table-column label="Priority" prop="priority" align="center" />
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
            @click="handleUpdateStage(scope.row, 'update')"
          >Edit</el-button>
          <el-button v-if="scope.row.isActive" type="danger" size="mini" @click="changeStageStatus(scope.row, false)">Inactivate</el-button>
          <el-button v-else type="success" size="mini" @click="changeStageStatus(scope.row, true)">Activate</el-button>
        </template>
      </el-table-column>
    </el-table>
    <!-- Stage list end -->
    <!-- Pagination start -->
    <pagination
      v-show="total > 0"
      ref="pagination"
      :total="total"
      :page.sync="queryParams.pageNo"
      :limit.sync="queryParams.pageSize"
      :page-sizes="[20, 50, 100]"
      @pagination="getStage"
    />
    <!-- Pagination end -->
    <!-- Stage dialog start -->
    <el-dialog
      :visible.sync="stageDialogVisible"
      :title="titleMap[dialogStatus]"
      @closed="handleStageDialogClosed"
    >
      <i-create-stage
        :dialog-status="dialogStatus"
        :form="stageForm"
        @handleCancelClick="handleCancelClick"
        @handleActionBtnClick="handleActionBtnClick"
      />
    </el-dialog>
    <!-- Stage dialog end -->
  </div>
</template>

<script>
import { getAllJobStagesByFilter, updateJobStage } from '@/api/job'
import Pagination from '@/components/Pagination'
import CreateStage from '@/views/job/components/createStage'

export default {
  name: 'StageManagement',
  components: {
    Pagination,
    iCreateStage: CreateStage
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
      stageForm: {
        id: undefined,
        name: '',
        notes: '',
        priority: 0,
        isActive: true
      },
      stageDialogVisible: false,
      stageList: [],
      winTableHeight: 800
    }
  },
  created() {
    this.getStage()
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
    getStage() {
      this.listLoading = true
      getAllJobStagesByFilter(this.queryParams).then(res => {
        this.total = res.count
        this.stageList = res.data
        this.listLoading = false
      }).catch(e => {
        this.listLoading = false
      })
    },
    resetTemp() {
      this.stageForm = {
        id: undefined,
        name: '',
        notes: '',
        priority: 0,
        isActive: true
      }
    },
    handleAddNewStage() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.stageDialogVisible = true
    },
    handleUpdateStage(row, type) {
      this.stageForm = Object.assign({}, row) // copy obj
      this.dialogStatus = type
      this.stageDialogVisible = true
    },
    handleStageDialogClosed() {
      this.resetTemp()
    },
    changeStageStatus(val, status) {
      const action = status ? 'Activate' : 'Inactivate'
      this.$confirm(
        'Are you sure you want to ' + action + ' stage ' +
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
        updateJobStage(params).then(res => {
          this.$message({
            type: 'success',
            message: action + ' successfully!'
          })
          this.getStage()
        }).catch(err => {
          this.$message({
            type: 'error',
            message: action + ' Failed. ' + err
          })
        })
      }).catch(() => {})
    },
    handleCancelClick() {
      this.stageDialogVisible = false
    },
    handleActionBtnClick() {
      this.stageDialogVisible = false
      this.getStage()
    }
  }
}
</script>
<style lang="scss" scoped>
</style>
