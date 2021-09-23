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
            @keyup.enter.native="getJobs"
          />
        </div>
        <div class="app__search-btn">
          <el-button
            class="filter-item shadow"
            type="primary"
            icon="el-icon-search"
            @click="getJobs"
          >Search</el-button>
        </div>
      </div>
      <div class="app__search-right">
        <div>
          <el-button class="shadow" type="primary" @click="handleGoToCreatePage">Add New Job</el-button>
        </div>
      </div>
    </div>
    <!-- Search row end -->
    <!-- Job list start -->
    <el-table v-loading="listLoading" :max-height="winTableHeight" :data="jobList" border fit>
      <el-table-column type="expand">
        <template slot-scope="props">
          <el-form label-position="left" inline class="demo-table-expand">
            <div class="provider__expand-row">
              <el-form-item label="Job Code">
                <span>{{ props.row.jobCode }}</span>
              </el-form-item>
              <el-form-item label="Address">
                <span>{{ props.row.address }}</span>
              </el-form-item>
              <el-form-item label="Title">
                <span>{{ props.row.title }}</span>
              </el-form-item>
              <el-form-item label="Name">
                <span>{{ props.row.firstName }}</span>
              </el-form-item>
              <el-form-item label="Email">
                <span>{{ props.row.email }}</span>
              </el-form-item>
              <el-form-item label="Mobile">
                <span>{{ props.row.mobile }}</span>
              </el-form-item>
              <el-form-item label="Phone">
                <span>{{ props.row.phone }}</span>
              </el-form-item>
              <el-form-item label="Scheduled Started On">
                <span>{{ utcToLocalDateTimeS(props.row.scheduledStartedOnUtc) }}</span>
              </el-form-item>
              <el-form-item label="Scheduled Ended On">
                <span>{{ utcToLocalDateTimeS(props.row.scheduledEndedOnUtc) }}</span>
              </el-form-item>
              <el-form-item label="Notes">
                <span>{{ props.row.notes }}</span>
              </el-form-item>
              <el-form-item label="Stage">
                <span>{{ props.row.jobStage }}</span>
              </el-form-item>
              <el-form-item label="Form Title">
                <span>{{ props.row.formTitle }}</span>
              </el-form-item>
              <el-form-item label="Form Description">
                <span>{{ props.row.formDescription }}</span>
              </el-form-item>
              <el-form-item label="Assigned To Staff">
                <span>{{ props.row.assignedToStaff }}</span>
              </el-form-item>
              <el-form-item label="Is Active">
                <span>{{ props.row.isActive }}</span>
              </el-form-item>
            </div>
          </el-form>
          <el-table
            :data="props.row.jobLines"
            class="shadow"
            border
            align="center"
            stripe
          >
            <el-table-column label="Name" align="center" prop="name" />
            <el-table-column label="Task Fee" align="center" prop="taskFee" />
            <el-table-column label="Duration" align="center" width="120" prop="duration" />
            <el-table-column label="Is Completed" align="center" width="120" prop="isCompleted">
              <template slot-scope="scope">
                <el-tag :type="scope.row.isCompleted | statusFilter">{{ scope.row.isCompleted | isActiveFilter }}</el-tag>
              </template>
            </el-table-column>
          </el-table>
        </template>
      </el-table-column>
      <el-table-column type="index" width="50" align="center" />
      <el-table-column label="Job Code" width="140" align="center">
        <template slot-scope="scope">
          <span class="link-type" @click="handleUpdateJob(scope.row)">{{ scope.row.jobCode }}</span>
        </template>
      </el-table-column>
      <el-table-column label="First Name" prop="firstName" align="center" />
      <el-table-column label="Mobile" prop="mobile" align="center" />
      <el-table-column label="Stage" prop="jobStage" align="center" />
      <el-table-column label="Scheduled Started On" width="170" align="center">
        <template slot-scope="scope">
          <span>{{ utcToLocalDateTimeS(scope.row.scheduledStartedOnUtc) }}</span>
        </template>
      </el-table-column>
      <el-table-column label="Scheduled Ended On" width="170" align="center">
        <template slot-scope="scope">
          <span>{{ utcToLocalDateTimeS(scope.row.scheduledEndedOnUtc) }}</span>
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
            @click="handleUpdateJob(scope.row)"
          >Edit</el-button>
          <el-button v-if="scope.row.isActive" type="danger" size="mini" @click="changeJobStatus(scope.row, false)">Inactivate</el-button>
          <el-button v-else type="success" size="mini" @click="changeJobStatus(scope.row, true)">Activate</el-button>
        </template>
      </el-table-column>
    </el-table>
    <!-- Job list end -->
    <!-- Pagination start -->
    <pagination
      v-show="total > 0"
      ref="pagination"
      :total="total"
      :page.sync="queryParams.pageNo"
      :limit.sync="queryParams.pageSize"
      :page-sizes="[20, 50, 100]"
      @pagination="getJobs"
    />
    <!-- Pagination end -->
  </div>
</template>

<script>
import { getJobsByFilter, updateJob } from '@/api/job'
import Pagination from '@/components/Pagination'
import { utcToLocalDateTimeS } from '@/utils/custom'

export default {
  name: 'JobList',
  components: {
    Pagination
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
      jobForm: {
        id: undefined,
        jobCode: '',
        address: '',
        title: '',
        firstName: '',
        middleName: '',
        lastName: '',
        email: '',
        mobile: '',
        phone: '',
        scheduledStartedOn: this.$moment(),
        scheduledEndedOn: this.$moment(),
        assignedToStaffId: '',
        isCompleted: false,
        isRescheduled: false,
        rescheduledReason: '',
        stageId: '',
        notes: '',
        isActive: true,
        formTitle: '',
        formDescription: '',
        beforePhotos: [],
        afterPhotos: [],
        relatedNotes: [],
        jobHazards: [],
        jobTasks: [],
        jobLogs: [],
        jobLines: []
      },
      jobList: [],
      winTableHeight: 800
    }
  },
  created() {
    this.getJobs()
  },
  updated() {
    this.$nextTick(() => {
      this.winTableHeight =
        window.innerHeight -
        this.$refs.search.clientHeight -
        this.$refs.pagination.$el.clientHeight -
        175
    })
  },
  methods: {
    getJobs() {
      this.listLoading = true
      getJobsByFilter(this.queryParams).then(res => {
        this.total = res.count
        this.jobList = res.data
        this.listLoading = false
      }).catch(e => {
        this.listLoading = false
      })
    },
    resetTemp() {
      this.jobForm = {
        id: undefined,
        firstName: '',
        middleName: '',
        lastName: '',
        mobile: '',
        phoneNumber: '',
        email: '',
        jobCode: '',
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
    handleGoToCreatePage() {
      this.$store.commit('job/SET_JOB_ID', '')
      this.$router.push('/job/new')
    },
    handleUpdateJob(row) {
      this.$store.commit('job/SET_JOB_ID', row.id)
      const params = { name: 'UpdateJob' }
      this.$store.dispatch('tagsView/delCachedView', params).then(() => {
        this.$nextTick(() => {
          this.$router.push('/job/update')
        })
      })
      // this.$router.push({ name: 'updateProduct', query: { productId: row.productId }})
    },
    changeJobStatus(val, status) {
      const action = status ? 'Activate' : 'Inactivate'
      this.$confirm(
        'Are you sure you want to ' + action + ' job ' +
          val.jobCode +
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
        updateJob(params).then(res => {
          this.$message({
            type: 'success',
            message: action + ' successfully!'
          })
          this.getJobs()
        }).catch(err => {
          this.$message({
            type: 'error',
            message: action + ' Failed. ' + err
          })
        })
      }).catch(() => {})
    },
    utcToLocalDateTimeS
  }
}
</script>
<style lang="scss" scoped>
.job {
  &__tag {
    margin: .2rem;
  }
}
</style>
