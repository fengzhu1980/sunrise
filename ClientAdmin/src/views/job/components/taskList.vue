<template>
  <div class="app-container">
    <!-- Search row start -->
    <div ref="search" class="app__search">
      <div class="app__search-left">
        <div class="app__search-keyword">
          <el-input
            ref="keyword"
            v-model="queryParams.keyword"
            clearable
            class="filter-item"
            placeholder="Name, Notes..."
            @keyup.enter.native="getTask"
          />
        </div>
        <div class="app__search-btn">
          <el-button
            class="filter-item shadow"
            type="primary"
            icon="el-icon-search"
            @click="getTask"
          >Search</el-button>
        </div>
      </div>
      <div class="app__search-right">
        <div>
          <el-button class="shadow" type="primary" @click="handleAddNewTask">Add New Task</el-button>
        </div>
      </div>
    </div>
    <!-- Search row end -->
    <!-- Task list start -->
    <el-table v-loading="listLoading" :max-height="winTableHeight" :data="taskList" border fit>
      <el-table-column type="index" width="50" align="center" />
      <el-table-column label="Name" width="250" align="center">
        <template slot-scope="scope">
          <span class="link-type" @click="handleUpdateTask(scope.row, 'browse')">{{ scope.row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="Task Fee($)" prop="taskFee" align="center" />
      <el-table-column label="Duration(Hour)" prop="duration" align="center" />
      <el-table-column label="Is Active" width="100" align="center">
        <template slot-scope="scope">
          <el-tag :type="scope.row.isActive | statusFilter">{{ scope.row.isActive | isActiveFilter }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column fixed="right" label="Operations" align="center" width="200">
        <template slot-scope="scope">
          <el-button
            type="success"
            size="mini"
            @click="selectTask(scope.row)"
          >Select</el-button>
        </template>
      </el-table-column>
      <el-table-column fixed="right" label="Operations" align="center" width="200">
        <template slot-scope="scope">
          <el-button
            type="primary"
            size="mini"
            @click="handleUpdateTask(scope.row, 'update')"
          >Edit</el-button>
          <el-button v-if="scope.row.isActive" type="danger" size="mini" @click="changeTaskStatus(scope.row, false)">Inactivate</el-button>
          <el-button v-else type="success" size="mini" @click="changeTaskStatus(scope.row, true)">Activate</el-button>
        </template>
      </el-table-column>
    </el-table>
    <!-- Task list end -->
    <!-- Pagination start -->
    <pagination
      v-show="total > 0"
      ref="pagination"
      :total="total"
      :page.sync="queryParams.pageNo"
      :limit.sync="queryParams.pageSize"
      :page-sizes="[20, 50, 100]"
      @pagination="getTask"
    />
    <!-- Pagination end -->
    <!-- Task dialog start -->
    <el-dialog
      :visible.sync="taskDialogVisible"
      :title="titleMap[dialogStatus]"
      @closed="handleTaskDialogClosed"
    >
      <i-create-task
        :dialog-status="dialogStatus"
        :form="taskForm"
        @handleCancelClick="handleCancelClick"
        @handleActionBtnClick="handleActionBtnClick"
      />
    </el-dialog>
    <!-- Task dialog end -->
  </div>
</template>

<script>
import { getTaskList, updateTask } from '@/api/admin'
import Pagination from '@/components/Pagination'
import CreateTask from '@/views/admin/components/createTask'

export default {
  name: 'TaskManagement',
  components: {
    Pagination,
    iCreateTask: CreateTask
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
  props: {
    keywordInputReFocus: {
      default: false,
      type: Boolean,
      required: false
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
      taskForm: {
        id: undefined,
        name: '',
        taskFee: 0,
        duration: 0,
        isActive: true
      },
      taskDialogVisible: false,
      taskList: [],
      winTableHeight: 800
    }
  },
  watch: {
    keywordInputReFocus() {
      this.reFocusInput()
    }
  },
  created() {
    this.getTask()
    this.reFocusInput()
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
    getTask() {
      this.listLoading = true
      getTaskList(this.queryParams).then(res => {
        this.total = res.count
        this.taskList = res.data
        this.listLoading = false
      }).catch(e => {
        this.$message.error('Get task failed ' + e)
        this.listLoading = false
      })
    },
    reFocusInput() {
      this.$nextTick(_ => {
        if (this.$refs.keyword.$el) {
          this.$refs.keyword.$el.querySelector('input').focus()
        }
      })
    },
    resetTemp() {
      this.taskForm = {
        id: undefined,
        name: '',
        taskFee: 0,
        duration: 0,
        isActive: true
      }
    },
    selectTask(row) {
      this.$emit('returnTask', row)
    },
    handleAddNewTask() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.taskDialogVisible = true
    },
    handleUpdateTask(row, type) {
      this.taskForm = Object.assign({}, row) // copy obj
      this.dialogStatus = type
      this.taskDialogVisible = true
    },
    handleTaskDialogClosed() {
      this.resetTemp()
    },
    changeTaskStatus(val, status) {
      const action = status ? 'Activate' : 'Inactivate'
      this.$confirm(
        'Are you sure you want to ' + action + ' task ' +
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
        updateTask(params).then(res => {
          this.$message({
            type: 'success',
            message: action + ' successfully!'
          })
          this.getTask()
        }).catch(err => {
          this.$message({
            type: 'error',
            message: action + ' Failed. ' + err
          })
        })
      }).catch(() => {})
    },
    handleCancelClick() {
      this.taskDialogVisible = false
    },
    handleActionBtnClick() {
      this.taskDialogVisible = false
      this.getTask()
    }
  }
}
</script>
<style lang="scss" scoped>
</style>
