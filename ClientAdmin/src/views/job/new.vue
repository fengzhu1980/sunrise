<template>
  <div v-loading="loading" class="app-container">
    <el-form ref="jobForm" :model="jobForm" class="job__form">
      <!-- Details start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Job Details</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right--normal">
          <el-row class="job__row-right--top">
            <el-col :lg="10" :md="10">
              <el-form-item
                :rules="{ required: true, message: 'Can not be empty', trigger: ['change', 'blur']}"
                prop="title"
                label="Title"
                label-width="62px"
              >
                <el-input v-model="jobForm.title" size="mini" />
              </el-form-item>
            </el-col>
            <el-col :lg="7" :md="11">
              <el-form-item
                :rules="[{ required: true, message: 'Can not be empty', trigger: ['change', 'blur']}]"
                prop="stageId"
                label="Current Stage"
                label-width="165px"
              >
                <el-select v-model="jobForm.stageId" placeholder="Select Option" size="mini" clearable>
                  <el-option
                    v-for="item in stageList"
                    :key="item.id"
                    :label="item.name"
                    :value="item.id"
                  />
                </el-select>
              </el-form-item>
            </el-col>
          </el-row>
        </el-col>
      </el-row>
      <!-- Details end -->
      <!-- Site Address start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Site Address</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right">
          <el-row class="job__col-item">
            <el-form-item
              :rules="{ required: true, message: 'Please input a address'}"
              prop="address"
              class="job__col-right--empty"
            >
              <el-input v-model="jobForm.address" class="job__col-right--empty" size="mini" />
            </el-form-item>
          </el-row>
        </el-col>
      </el-row>
      <!-- Site Address end -->
      <!-- Scheduled Time start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Scheduled Time</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right--normal">
          <el-row class="job__row-right--top">
            <el-col :lg="7" :md="10">
              <el-form-item
                :rules="{ required: true, message: 'Can not be empty', trigger: ['change', 'blur']}"
                prop="scheduledDate"
                label="Date"
                label-width="60px"
              >
                <el-date-picker
                  v-model="jobForm.scheduledDate"
                  type="date"
                  placeholder="Date"
                />
              </el-form-item>
            </el-col>
            <el-col :lg="7" :md="11">
              <el-form-item
                :rules="{ required: true, message: 'Can not be empty', trigger: ['change', 'blur']}"
                prop="scheduledTime"
                label="Time"
                label-width="65px"
              >
                <el-time-picker
                  v-model="jobForm.scheduledTime"
                  is-range
                  range-separator="To"
                  start-placeholder="Start On"
                  end-placeholder="End On"
                  placeholder="Time Range"
                />
              </el-form-item>
            </el-col>
          </el-row>
        </el-col>
      </el-row>
      <!-- Scheduled Time end -->
      <!-- Main Contact start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Main Contact</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right--buyer">
          <div class="job__delivery-address">
            <el-row class="job__delivery-address-top">
              <el-col :lg="7" :md="10" class="job__col-item">
                <el-form-item
                  :rules="{ required: true, message: 'Please input a name'}"
                  prop="firstName"
                  label="Name"
                  label-width="65px"
                >
                  <el-input
                    v-model="jobForm.firstName"
                    size="mini"
                  />
                </el-form-item>
              </el-col>
              <el-col :lg="8" :md="12" class="job__col-item">
                <el-form-item
                  label="Contact Phone"
                  label-width="120px"
                >
                  <el-input v-model="jobForm.phone" size="mini" />
                </el-form-item>
              </el-col>
            </el-row>
            <el-row class="job__delivery-address-bottom">
              <el-col :lg="7" :md="10" class="job__col-item">
                <el-form-item
                  label="Contact Mobile"
                  label-width="120px"
                >
                  <el-input v-model="jobForm.mobile" size="mini" />
                </el-form-item>
              </el-col>
              <el-col :lg="8" :md="12" class="job__col-item">
                <el-form-item
                  label="Email Address"
                  label-width="120px"
                >
                  <el-input v-model="jobForm.email" size="mini" />
                </el-form-item>
              </el-col>
            </el-row>
          </div>
        </el-col>
      </el-row>
      <!-- Main Contact end -->
      <!-- Select task row start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Select Task</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right">
          <!-- Task row -->
          <el-row
            v-for="(task, index) in jobForm.jobLines"
            :key="task.id"
            class="job__task-row shadow-mouse-over"
          >
            <el-col :span="21" class="job__task-row--left">
              <el-row class="job__task-row--left--top">
                <el-col :lg="10" :md="15" class="job__col-item">
                  <el-form-item
                    :label="'task ' + (index + 1)"
                    :prop="'jobLines.' + index + '.name'"
                    :rules="[{
                      required: true, message: 'Can not be empty', trigger: ['blur', 'change']
                    }]"
                    label-width="70px"
                  >
                    <el-input v-model="task.name" size="mini" placeholder="Select Task" @focus="showTaskDialog(index)" />
                  </el-form-item>
                </el-col>
                <el-col :lg="5" :md="8" class="job__col-item">
                  <el-form-item
                    :prop="'jobLines.' + index + '.taskFee'"
                    :rules="[
                      { required: true, message: 'Can not be empty', trigger: ['change', 'blur']},
                      { type: 'number', message: 'Must be a numeric value', trigger: ['change', 'blur']},
                      { type: 'number', min: 0, message: 'Task Fee should greater than 0', trigger: ['change', 'blur']}
                    ]"
                    label="Task Fee"
                    label-width="100px"
                  >
                    <el-input-number
                      v-model="task.taskFee"
                      :min="0"
                      size="mini"
                    />
                  </el-form-item>
                </el-col>
                <el-col :lg="5" :md="8" class="job__col-item">
                  <el-form-item
                    :prop="'jobLines.' + index + '.duration'"
                    :rules="[
                      { required: true, message: 'Can not be empty', trigger: ['change', 'blur']},
                      { type: 'number', message: 'Must be a numeric value', trigger: ['change', 'blur']},
                      { type: 'number', min: 0, message: 'Duration should greater than 0', trigger: ['change', 'blur']}
                    ]"
                    label="Duration"
                    label-width="110px"
                  >
                    <el-input-number
                      v-model="task.duration"
                      :min="0"
                      size="mini"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
            </el-col>
            <el-col :span="3" class="job__task-row--right">
              <el-button v-show="jobForm.jobLines.length > 1" class="shadow" icon="el-icon-delete" type="warning" size="mini" @click="removeTask(task)">Remove</el-button>
            </el-col>
          </el-row>
          <el-row class="job__btn-add">
            <el-col :lg="3" :md="5">
              <el-button icon="el-icon-plus" class="shadow" type="primary" size="mini" @click="nextTask">Next Task</el-button>
            </el-col>
          </el-row>
        </el-col>
      </el-row>
      <!-- Select task row end -->
      <!-- Assigned To start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Assigned To</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right">
          <el-row class="job__col-item">
            <el-form-item
              :rules="{ required: true, message: 'Please select a staff'}"
              prop="assignedToStaffId"
              class="job__col-right--empty"
            >
              <el-select v-model="jobForm.assignedToStaffId" placeholder="Select Staff" size="mini" clearable>
                <el-option
                  v-for="item in staffList"
                  :key="item.id"
                  :label="item.firstName"
                  :value="item.id"
                />
              </el-select>
            </el-form-item>
          </el-row>
        </el-col>
      </el-row>
      <!-- Assigned To end -->
      <!-- Job Notes start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <div class="job__col-title">Notes</div>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right">
          <el-input v-model="jobForm.notes" class="job__col-right--empty" type="textarea" size="mini" />
        </el-col>
      </el-row>
      <!-- Job Notes end -->
      <!-- Form start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Form</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right--normal">
          <el-row class="job__row-right--top">
            <el-col :lg="7" :md="10">
              <el-form-item label="Title" label-width="55px">
                <el-input v-model="jobForm.formTitle" size="mini" />
              </el-form-item>
            </el-col>
            <el-col :lg="13" :md="14">
              <el-form-item label="Description" label-width="100px">
                <el-input v-model="jobForm.formDescription" size="mini" />
              </el-form-item>
            </el-col>
          </el-row>
        </el-col>
      </el-row>
      <!-- Form end -->
      <!-- Job Hazards start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Job Hazards</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right">
          <el-select v-model="jobForm.jobHazardIds" multiple clearable placeholder="Select Hazard" class="job__col-right--empty" size="mini">
            <el-option
              v-for="item in hazardList"
              :key="item.id"
              :label="item.title"
              :value="item.id"
            />
          </el-select>
        </el-col>
      </el-row>
      <!-- Job Hazards end -->
      <!-- Status start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Status</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right">
          <el-switch
            v-model="jobForm.isActive"
            class="job__col-right--empty"
            active-text="Is active"
          />
        </el-col>
      </el-row>
      <!-- Status end -->
    </el-form>
    <div class="job__btn">
      <el-button class="shadow" type="primary" @click="handleConfirm">Confirm</el-button>
      <el-button class="job__btn-reset shadow" type="warning" @click="resetForm('jobForm')">Reset Form</el-button>
    </div>
    <!--TaskList Dialog start -->
    <el-dialog
      :visible.sync="dialogTaskListFormVisible"
      :title="dialogTitle"
      :width="dialogWidth"
      @closed="handleDialogClosed"
      @opened="handleFocusInput"
    >
      <i-task-list
        :keyword-input-re-focus="keywordInputReFocus"
        @returnTask="responseTask"
      />
    </el-dialog>
    <!--TaskList Edit dialog end -->
    <!-- Back to top -->
    <el-tooltip placement="top" content="Back to top">
      <i-back-to-top :visibility-height="250" :back-position="50" transition-name="fade" class="shadow" />
    </el-tooltip>
  </div>
</template>
<script>
import { getAllJobStages, createNewJob } from '@/api/job'
import { getAllStaffs } from '@/api/staff'
import { getAllHazards } from '@/api/admin'
import waves from '@/directive/waves/index'
import TaskList from '@/views/job/components/taskList'
import BackToTop from '@/components/BackToTop'

export default {
  name: 'NewJob',
  directives: {
    waves
  },
  components: {
    ITaskList: TaskList,
    iBackToTop: BackToTop
  },
  data() {
    return {
      loading: false,
      dialogTaskListFormVisible: false,
      selectedTaskIndex: null,
      dialogTitle: 'Select Task',
      branchList: [],
      stageList: [],
      staffList: [],
      hazardList: [],
      keywordInputReFocus: false,
      jobForm: {
        jobLines: [{
          id: '',
          name: '',
          taskFee: 0,
          duration: 0.0
        }],
        address: '',
        title: '',
        firstName: '',
        email: '',
        mobile: '',
        phone: '',
        scheduledDate: this.$moment(),
        scheduledTime: [new Date(2021, 9, 10, 8, 40), new Date(2021, 9, 10, 9, 40)],
        notes: '',
        stageId: '',
        formTitle: '',
        formDescription: '',
        assignedToStaffId: '',
        jobHazardIds: [],
        isActive: true
      }
    }
  },
  computed: {
    dialogWidth() {
      let tempWidth = '70%'
      if (window.innerWidth <= 1024) {
        tempWidth = '100%'
      }
      return tempWidth
    }
  },
  created() {
    this.getStages()
    this.getStaffs()
    this.getHazardList()
  },
  methods: {
    getStages() {
      this.loading = true
      getAllJobStages().then(res => {
        if (res) {
          this.stageList = res.filter(stage => stage.isActive !== false)
        }
        this.loading = false
      }).catch(e => {
        this.loading = false
      })
    },
    getStaffs() {
      this.loading = true
      getAllStaffs().then(res => {
        if (res) {
          this.staffList = res.filter(stage => stage.isActive !== false)
        }
        this.loading = false
      }).catch(e => {
        this.loading = false
      })
    },
    getHazardList() {
      this.loading = true
      getAllHazards().then(res => {
        this.hazardList = res
        this.loading = false
      }).catch(e => {
        this.loading = false
      })
    },
    resetForm(formName) {
      this.$confirm(
        'Are you sure to clear everything you have done on this job?',
        'Warning',
        {
          confirmButtonText: 'Confirm',
          cancelButtonText: 'Close',
          type: 'warning'
        }
      ).then(() => {
        this.jobForm = {
          jobLines: [{
            id: '',
            name: '',
            taskFee: 0,
            duration: 0.0
          }],
          address: '',
          title: '',
          firstName: '',
          email: '',
          mobile: '',
          phone: '',
          scheduledDate: this.$moment(),
          scheduledTime: [new Date(2021, 9, 10, 8, 40), new Date(2021, 9, 10, 9, 40)],
          notes: '',
          stageId: '',
          formTitle: '',
          formDescription: '',
          assignedToStaffId: '',
          jobHazardIds: [],
          isActive: true
        }
        this.$refs[formName].resetFields()
      }).catch(() => {})
    },
    responseTask(val) {
      this.jobForm.jobLines[this.selectedTaskIndex].name = val.name.trim()
      this.jobForm.jobLines[this.selectedTaskIndex].taskFee = val.taskFee
      this.jobForm.jobLines[this.selectedTaskIndex].duration = val.duration
      this.handleDialogClosed()
    },
    handleDialogClosed() {
      this.dialogTaskListFormVisible = false
    },
    handleFocusInput() {
      this.keywordInputReFocus = !this.keywordInputReFocus
    },
    showTaskDialog(index) {
      this.selectedTaskIndex = index
      this.dialogTaskListFormVisible = true
    },
    handleConfirm() {
      this.$refs['jobForm'].validate(valid => {
        if (valid) {
          // Check address
          if (!this.jobForm.address) {
            this.$message({ type: 'warning', message: `Please input address` })
            return
          }
          // TODO: scheduled time, date,
          const tempDate = this.$moment(this.jobForm.scheduledDate).format('YYYYMMDD')
          const tempStartHours = this.jobForm.scheduledTime[0].getHours()
          const tempStartMinutes = this.jobForm.scheduledTime[0].getMinutes()
          const tempEndHours = this.jobForm.scheduledTime[1].getHours()
          const tempEndMinutes = this.jobForm.scheduledTime[1].getMinutes()
          const tempStartOn = this.$moment(`${tempDate} ${tempStartHours}:${tempStartMinutes}`, 'YYYYMMDD hh:mm').format()
          const tempEndOn = this.$moment(`${tempDate} ${tempEndHours}:${tempEndMinutes}`, 'YYYYMMDD hh:mm').format()
          this.jobForm.scheduledStartedOn = tempStartOn
          this.jobForm.scheduledEndedOn = tempEndOn
          this.loading = true
          const params = this.jobForm
          createNewJob(params).then(res => {
            this.$message({ type: 'success', message: 'Job created successfully!' })
            this.loading = false
          }).catch(_ => {
            this.loading = false
          })
        } else {
          this.$message.error('Please check input!')
        }
      })
    },
    nextTask() {
      this.jobForm.jobLines.push({
        id: Date.now().toString(),
        name: '',
        taskFee: 0,
        duration: 0.0
      })
    },
    removeTask(item) {
      var index = this.jobForm.jobLines.indexOf(item)
      if (index !== -1) {
        this.jobForm.jobLines.splice(index, 1)
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.job {
  &__form {
    border: 5px solid #DCDFE6;
    border-radius: .5rem;
  }
  &__row {
    display: flex;
  }
  &__note {
    margin-left: .5rem;
  }
  &__member-row {
    border: 1px solid #DCDFE6;
    border-radius: .8rem;
    margin: .5rem .2rem .5rem 3rem;
    display: flex;
    margin-top: 0.625rem;
    &--main {
      overflow: hidden;
      // height: 7.8125rem;
      &--top {
        margin-top: 0.3125rem;
      }
      &--center {
        // background-color: aquamarine;
        // float: left;
        // margin: -1.5625rem 0;
      }
    }
  }
  &__task-row {
    border: 1px solid #DCDFE6;
    border-radius: .8rem;
    margin: .3rem .1rem;
    display: flex;
    &--left {
      padding-top: 0.625rem;
      &--center {
        float: left;
        width: 100%;
        // margin: -1.25rem 0 -0.25rem 0.3125rem;
      }
      &--bottom {
        float: left;
        width: 100%;
        // margin-bottom: -0.9375rem;
      }
      &--combination {
        float: left;
        // margin-top: -0.9375rem;
      }
    }
    &--right {
      display: flex;
      justify-content: center;
      align-items: center;
    }
    &--top {
      float: left;
      width: 100%;
      // margin-bottom: -15px;
    }
  }
  &__discount-item {
    margin-left: .5rem;
  }
  &__col-left {
    background-color: #EBEEF5;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    padding: .2rem;
    border-bottom: 1px solid #DCDFE6;
  }
  &__col-right {
    padding: .2rem .1rem;
    display: flex;
    flex-direction: column;
    border-bottom: 1px solid #DCDFE6;
    &--total {
      // flex: 0;
      align-self: flex-start;
      font-size: 1rem;
      font-weight: 600;
    }
    &--note {
      height: 5rem;
      padding: 0.9375rem 0 0rem .7rem;
      border-bottom: 1px solid #DCDFE6;
    }
    &--normal {
      height: 3.75rem;
      padding: 0.625rem 0 0rem .2rem;
      border-bottom: 1px solid #DCDFE6;
    }
    &--buyer {
      border-bottom: 1px solid #DCDFE6;
    }
    &--btn {
      margin: 0 0 0.625rem .5rem;
    }
    &--name {
      margin-top: .3rem;
    }
    &--empty {
      padding: .2rem;
    }
  }
  &__col-title {
    font-size: .9rem;
    font-weight: 600;
    color: #606266;
  }
  &__col-item {
    margin-right: .5rem;
    .el-form-item {
      margin-bottom: 0;
    }
  }
  &__divider {
    margin-top: 0;
  }
  &__delivery {
    &-top {
      margin: .5rem;
    }
    &-item {
      border-bottom: 1px solid #DCDFE6;
    }
    &-table {
      margin: .2rem;
      border-bottom: 1px solid #DCDFE6;

      &--row {
        padding: .5rem;
        border: 1px solid #DCDFE6;
        border-bottom: none;
      }

      &--text {
        font-size: .9rem;
      }
    }
    &-title {
      font-size: .9rem;
      font-weight: 600;
      color: #606266;
    }
    &-address {
      margin: 0.625rem 0;
      // &-bottom {
      //   float: left;
      //   width: 100%;
      //   // margin-bottom: -0.9375rem;
      // }
    }
  }
  &__btn {
    display: flex;
    justify-content: center;
    margin: 2rem;

    &-reset {
      margin-left: 6rem;
    }
  }
}
.job {
  &__btn {
    &-add {
      margin: .5rem 0.2rem;
    }
    &-clear {
      padding: 1rem .2rem;
      margin-bottom: 1.5rem;
    }
  }
}
</style>
