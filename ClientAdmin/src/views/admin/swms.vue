<template>
  <div class="app-container">
    <!-- Search row start -->
    <div ref="search" class="app__search">
      <div class="app__search-left">
        <div class="app__search-keyword">
          <el-input
            v-model="queryParams.keyword"
            class="filter-item"
            placeholder="Name, Notes..."
            @keyup.enter.native="getSWMS"
          />
        </div>
        <div class="app__search-btn">
          <el-button
            class="filter-item shadow"
            type="primary"
            icon="el-icon-search"
            @click="getSWMS"
          >Search</el-button>
        </div>
      </div>
      <div class="app__search-right">
        <div>
          <el-button class="shadow" type="primary" @click="handleAddNewSWMS">Add New SWMS</el-button>
        </div>
      </div>
    </div>
    <!-- Search row end -->
    <!-- SWMS list start -->
    <el-table v-loading="listLoading" :max-height="winTableHeight" :data="swmsList" border fit>
      <el-table-column type="index" width="50" align="center" />
      <el-table-column label="Title" width="250" align="center">
        <template slot-scope="scope">
          <span class="link-type" @click="handleUpdateSWMS(scope.row, 'browse')">{{ scope.row.title }}</span>
        </template>
      </el-table-column>
      <el-table-column label="Notes" prop="notes" align="center" />
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
            @click="handleUpdateSWMS(scope.row, 'update')"
          >Edit</el-button>
          <el-button v-if="scope.row.isActive" type="danger" size="mini" @click="changeSWMSStatus(scope.row, false)">Inactivate</el-button>
          <el-button v-else type="success" size="mini" @click="changeSWMSStatus(scope.row, true)">Activate</el-button>
        </template>
      </el-table-column>
    </el-table>
    <!-- SWMS list end -->
    <!-- Pagination start -->
    <pagination
      v-show="total > 0"
      ref="pagination"
      :total="total"
      :page.sync="queryParams.pageNo"
      :limit.sync="queryParams.pageSize"
      :page-sizes="[20, 50, 100]"
      @pagination="getSWMS"
    />
    <!-- Pagination end -->
    <!-- SWMS dialog start -->
    <el-dialog
      :visible.sync="swmsDialogVisible"
      :title="titleMap[dialogStatus]"
      @closed="handleSWMSDialogClosed"
    >
      <i-create-swms
        :dialog-status="dialogStatus"
        :form="swmsForm"
        @handleCancelClick="handleCancelClick"
        @handleActionBtnClick="handleActionBtnClick"
      />
    </el-dialog>
    <!-- SWMS dialog end -->
  </div>
</template>

<script>
import { getSWMSList, updateSWMS } from '@/api/admin'
import Pagination from '@/components/Pagination'
import CreateSWMS from '@/views/admin/components/createSWMS'

export default {
  name: 'SWMSManagement',
  components: {
    Pagination,
    iCreateSwms: CreateSWMS
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
      swmsForm: {
        id: undefined,
        title: '',
        notes: '',
        isActive: true
      },
      swmsDialogVisible: false,
      swmsList: [],
      winTableHeight: 800
    }
  },
  created() {
    this.getSWMS()
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
    getSWMS() {
      this.listLoading = true
      getSWMSList(this.queryParams).then(res => {
        console.log('res', res)
        this.total = res.count
        this.swmsList = res.data
        this.listLoading = false
      }).catch(e => {
        this.$message.error('Get swms failed ' + e)
        this.listLoading = false
      })
    },
    resetTemp() {
      this.swmsForm = {
        id: undefined,
        title: '',
        notes: '',
        isActive: true
      }
    },
    handleAddNewSWMS() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.swmsDialogVisible = true
    },
    handleUpdateSWMS(row, type) {
      this.swmsForm = Object.assign({}, row) // copy obj
      this.dialogStatus = type
      this.swmsDialogVisible = true
    },
    handleSWMSDialogClosed() {
      this.resetTemp()
    },
    changeSWMSStatus(val, status) {
      const action = status ? 'Activate' : 'Inactivate'
      this.$confirm(
        'Are you sure you want to ' + action + ' swms ' +
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
        updateSWMS(params).then(res => {
          this.$message({
            type: 'success',
            message: action + ' successfully!'
          })
          this.getSWMS()
        }).catch(err => {
          this.$message({
            type: 'error',
            message: action + ' Failed. ' + err
          })
        })
      }).catch(() => {})
    },
    handleCancelClick() {
      this.swmsDialogVisible = false
    },
    handleActionBtnClick() {
      this.swmsDialogVisible = false
      this.getSWMS()
    }
  }
}
</script>
<style lang="scss" scoped>
</style>
