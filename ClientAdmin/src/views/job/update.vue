<template>
  <div v-loading="loading" class="app-container">
    <el-form ref="jobForm" :model="jobForm" class="job__form">
      <!-- Show Operator start-->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Created By</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right">
          {{ jobForm.createdByStaff }}
        </el-col>
      </el-row>
      <!-- Show Operator end-->
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
                label="Date"
                label-width="60px"
              >
                <el-date-picker
                  v-model="selectedDate"
                  type="date"
                  placeholder="Date"
                />
              </el-form-item>
            </el-col>
            <el-col :lg="7" :md="11">
              <el-form-item
                :rules="{ required: true, message: 'Can not be empty', trigger: ['change', 'blur']}"
                label="Time"
                label-width="65px"
              >
                <el-time-picker
                  v-model="selectedTime"
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
      <!-- Before Photos start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Before Photos</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right">
          <i-upload-image-multiple
            ref="uploadDocument"
            :documents-get="jobForm.beforePhotos"
            :type="`BeforePhoto`"
            @handleRemoveUploadedImg="handleRemoveUploadedBeforePhoto"
            @handleRemove="handleRemoveBeforePhoto"
            @handleAddNewDocument="handleAddNewBeforePhoto"
          />
        </el-col>
      </el-row>
      <!-- Before Photos end -->
      <!-- After Photos start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">After Photos</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right">
          <i-upload-after-image-multiple
            ref="uploadDocument"
            :documents-get="jobForm.afterPhotos"
            :type="`AfterPhoto`"
            @handleRemoveUploadedImg="handleRemoveUploadedAfterPhoto"
            @handleRemove="handleRemoveAfterPhoto"
            @handleAddNewDocument="handleAddNewAfterPhoto"
          />
        </el-col>
      </el-row>
      <!-- After Photos end -->
      <!-- Download start -->
      <el-row class="job__row">
        <el-col :xs="6" :sm="5" :md="4" :lg="3" :xl="2" class="job__col-left">
          <span class="job__col-title">Download</span>
        </el-col>
        <el-col :xs="18" :sm="19" :md="20" :lg="21" :xl="22" class="job__col-right">
          <el-row class="job__row-right--top">
            <el-col :lg="7" :md="10">
              <el-button :disabled="downloadBtnDisable(jobForm.beforePhotos)" icon="el-icon-plus" class="shadow" type="primary" size="mini" @click="downloadPhotos('before')">Download Before Photos</el-button>
            </el-col>
            <el-col :lg="7" :md="10">
              <el-button :disabled="downloadBtnDisable(jobForm.afterPhotos)" icon="el-icon-plus" class="shadow" type="primary" size="mini" @click="downloadPhotos('after')">Download After Photos</el-button>
            </el-col>
          </el-row>
        </el-col>
      </el-row>
      <!-- Download end -->
    </el-form>
    <div class="job__btn">
      <el-button class="shadow" type="primary" @click="handleConfirm">Update</el-button>
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
import { getAllJobStages, getJobById, updateJob } from '@/api/job'
import { getAllStaffs } from '@/api/staff'
import { getAllHazards } from '@/api/admin'
import { getImageUrl, utcToLocalDateTimeSmall, utcToLocalDateTimeSmallComma } from '@/utils/custom'
import UploadImageMultiple from '@/components/Upload/image/multiple'
import waves from '@/directive/waves/index'
import TaskList from '@/views/job/components/taskList'
import BackToTop from '@/components/BackToTop'
import JSZip from 'jszip'
import FileSaver from 'file-saver'

export default {
  name: 'UpdateJob',
  directives: {
    waves
  },
  components: {
    iUploadImageMultiple: UploadImageMultiple,
    iUploadAfterImageMultiple: UploadImageMultiple,
    ITaskList: TaskList,
    iBackToTop: BackToTop
  },
  data() {
    return {
      jobId: '',
      loading: false,
      dialogTaskListFormVisible: false,
      selectedTaskIndex: null,
      dialogTitle: 'Select Task',
      branchList: [],
      stageList: [],
      staffList: [],
      hazardList: [],
      keywordInputReFocus: false,
      beforePhotosUploadedNew: [],
      afterPhotosUploadedNew: [],
      selectedDate: '',
      selectedTime: [],
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
        notes: '',
        stageId: '',
        formTitle: '',
        formDescription: '',
        assignedToStaffId: '',
        beforePhotos: [],
        afterPhotos: [],
        jobHazardIds: []
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
    if (this.$store.state.job.jobId) this.getJobDetails()
    this.getStages()
    this.getStaffs()
    this.getHazardList()
  },
  methods: {
    getJobDetails() {
      if (this.$store.state.job.jobId) this.jobId = this.$store.state.job.jobId
      this.loading = true
      const param = { id: this.jobId }
      getJobById(param).then(res => {
        this.jobForm = JSON.parse(JSON.stringify(res))
        this.selectedDate = this.$moment()
        this.selectedTime = []
        if (res.scheduledStartedOnUtc) {
          this.selectedDate = this.$moment(this.utcToLocalDateTimeSmall(res.scheduledStartedOnUtc))
          this.selectedTime.push(new Date(this.utcToLocalDateTimeSmallComma(res.scheduledStartedOnUtc)))
        }
        if (res.scheduledEndedOnUtc) {
          this.selectedTime.push(new Date(this.utcToLocalDateTimeSmallComma(res.scheduledEndedOnUtc)))
        }
        this.loading = false
      }).catch(e => {
        this.loading = false
      })
    },
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
          notes: '',
          stageId: '',
          formTitle: '',
          formDescription: '',
          assignedToStaffId: '',
          beforePhotos: [],
          afterPhotos: [],
          jobHazardIds: []
        }
        this.selectedDate = this.$moment()
        this.selectedTime = []
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
    handleRemoveUploadedBeforePhoto(item) {
      this.$confirm('Are you sure you want to delete this picture?', 'Warning', {
        confirmButtonText: 'Delete',
        cancelButtonText: 'Cancel',
        type: 'warning'
      }).then(() => {
        const tempPicturesUploaded = this.jobForm.beforePhotos
        const indexFound = tempPicturesUploaded.findIndex(
          p => p.id === item.id
        )
        if (indexFound !== -1) {
          this.jobForm.beforePhotos.splice(indexFound, 1)
        }
      })
    },
    handleRemoveUploadedAfterPhoto(item) {
      this.$confirm('Are you sure you want to delete this picture?', 'Warning', {
        confirmButtonText: 'Delete',
        cancelButtonText: 'Cancel',
        type: 'warning'
      }).then(() => {
        const tempPicturesUploaded = this.jobForm.afterPhotos
        const indexFound = tempPicturesUploaded.findIndex(
          p => p.id === item.id
        )
        if (indexFound !== -1) {
          this.jobForm.afterPhotos.splice(indexFound, 1)
        }
      })
    },
    handleRemoveBeforePhoto(file, fileList) {
      // Delete file from beforePhotosUploadedNew
      const tempFile = file
      const tempPictures = this.beforePhotosUploadedNew
      if (tempPictures.length > 0) {
        const tempIndex = tempPictures.findIndex(
          p => p.uid === tempFile.uid
        )
        if (tempIndex !== -1) {
          this.beforePhotosUploadedNew.splice(tempIndex, 1)
        }
      }
    },
    handleRemoveAfterPhoto(file, fileList) {
      // Delete file from afterPhotosUploadedNew
      const tempFile = file
      const tempPictures = this.afterPhotosUploadedNew
      if (tempPictures.length > 0) {
        const tempIndex = tempPictures.findIndex(
          p => p.uid === tempFile.uid
        )
        if (tempIndex !== -1) {
          this.afterPhotosUploadedNew.splice(tempIndex, 1)
        }
      }
    },
    handleAddNewBeforePhoto(photo) {
      this.beforePhotosUploadedNew.push(photo)
    },
    handleAddNewAfterPhoto(photo) {
      this.afterPhotosUploadedNew.push(photo)
    },
    handleConfirm() {
      this.$refs['jobForm'].validate(valid => {
        if (valid) {
          if (!this.selectedDate || !this.selectedTime || !this.selectedTime.length === 0) {
            this.$message.warning('Please select date and time')
          }
          const tempDate = this.$moment(this.selectedDate).format('YYYYMMDD')
          const tempStartHours = this.selectedTime[0].getHours()
          const tempStartMinutes = this.selectedTime[0].getMinutes()
          const tempEndHours = this.selectedTime[1].getHours()
          const tempEndMinutes = this.selectedTime[1].getMinutes()
          const tempStartOn = this.$moment(`${tempDate} ${tempStartHours}:${tempStartMinutes}`, 'YYYYMMDD hh:mm').format()
          const tempEndOn = this.$moment(`${tempDate} ${tempEndHours}:${tempEndMinutes}`, 'YYYYMMDD hh:mm').format()
          this.jobForm.scheduledStartedOn = tempStartOn
          this.jobForm.scheduledEndedOn = tempEndOn
          // Before photos
          // Add old before photo ids
          this.jobForm.beforePhotoIds = []
          if (this.jobForm.beforePhotos.length > 0) {
            this.jobForm.beforePhotos.forEach(photo => {
              this.jobForm.beforePhotoIds.push(photo.id)
            })
          }
          // Add new before photo ids
          if (this.beforePhotosUploadedNew.length > 0) {
            this.beforePhotosUploadedNew.forEach(pic => {
              this.jobForm.beforePhotoIds.push(pic.id)
            })
          }
          // After photos
          // Add old after photo ids
          this.jobForm.afterPhotoIds = []
          if (this.jobForm.afterPhotos.length > 0) {
            this.jobForm.afterPhotos.forEach(photo => {
              this.jobForm.afterPhotoIds.push(photo.id)
            })
          }
          // Add new after photo ids
          if (this.afterPhotosUploadedNew.length > 0) {
            this.afterPhotosUploadedNew.forEach(pic => {
              this.jobForm.afterPhotoIds.push(pic.id)
            })
          }
          const params = this.jobForm
          this.loading = true
          updateJob(params).then(res => {
            this.$message({ type: 'success', message: 'Job updated successfully!' })
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
    },
    downloadBtnDisable(photos) {
      let result = true
      if (photos && photos.length > 0) {
        result = false
      }
      return result
    },
    downloadPhotos(type) {
      const imgList = []
      if (type === 'before') {
        if (this.jobForm.beforePhotos && this.jobForm.beforePhotos.length > 0) {
          this.jobForm.beforePhotos.forEach(photo => {
            const tempImg = {
              name: photo.fileName,
              path: photo.relativeFilePath
              // path: photo.documentUrl
            }
            imgList.push(tempImg)
          })
        }
      } else {
        if (this.jobForm.afterPhotos && this.jobForm.afterPhotos.length > 0) {
          this.jobForm.afterPhotos.forEach(photo => {
            const tempImg = {
              name: photo.fileName,
              path: photo.relativeFilePath
            }
            imgList.push(tempImg)
          })
        }
      }
      if (imgList.length > 0) {
        this.downloadZip(imgList, `${this.jobForm.jobCode}${type}`)
      }
    },
    getImgArrayBuffer(url) {
      return new Promise((resolve, reject) => {
        const xmlHttp = new XMLHttpRequest()
        xmlHttp.open('GET', url, true)
        // xmlHttp.setRequestHeader('Access-Control-Allow-Origin', 'https://localhost:9527')
        // xmlHttp.setRequestHeader('X-PINGOTHER', 'pingpong')
        // xmlHttp.setRequestHeader('Content-Type', 'application/xml')
        // xmlHttp.setRequestHeader('ontent-Type', 'application/json')
        xmlHttp.withCredentials = false
        xmlHttp.responseType = 'blob'
        xmlHttp.onload = function() {
          if (this.status === 200) {
            resolve(this.response)
          } else {
            reject(this.status)
          }
        }
        xmlHttp.send()
      })
    },
    downloadZip(imgList, title) {
      const zip = new JSZip()
      const cache = {}
      const promises = []
      this.title = 'Compressing'

      for (const item of imgList) {
        const promise = this.getImgArrayBuffer(item.path).then(data => {
        // const promise = getImgByUrlFromServer(item.path).then(data => {
          zip.file(item.name, data, { binary: true })
          cache[item.name] = data
        })
        promises.push(promise)
      }

      Promise.all(promises).then(_ => {
        zip.generateAsync({ type: 'blob' }).then(content => {
          this.title = 'Compressing'
          FileSaver.saveAs(content, title)
          this.title = 'Finished'
        })
      }).catch(res => {
        this.$message.error('Failed to compress files')
      })
    },
    // downloadZip(imgList, title) {
    //   const blogTitle = title
    //   const zip = new JSZip()
    //   const imgs = zip.folder(blogTitle)
    //   const baseList = []
    //   for (let i = 0; i < imgList.length; i++) {
    //     const name = imgList[i].name
    //     const image = new Image()
    //     // 解决跨域 Canvas 污染问题
    //     // image.setAttribute('crossOrigin', 'anonymous')
    //     image.onload = function() {
    //       const canvas = document.createElement('canvas')
    //       canvas.width = image.width
    //       canvas.height = image.height
    //       const context = canvas.getContext('2d')
    //       context.drawImage(image, 0, 0, image.width, image.height)
    //       const url = canvas.toDataURL() // 得到图片的base64编码数据 let url =
    //       canvas.toDataURL('image/png')
    //       baseList.push({ name: name, img: url.substring(22) })
    //       if (baseList.length === imgList.length) {
    //         if (baseList.length > 0) {
    //           for (let k = 0; k < baseList.length; k++) {
    //             imgs.file(baseList[k].name + '.png', baseList[k].img, {
    //               base64: true
    //             })
    //           }
    //           zip.generateAsync({ type: 'blob' }).then(function(content) {
    //             FileSaver.saveAs(content, blogTitle + '.zip')
    //           })
    //         }
    //       }
    //     }
    //     // image.src = imgList[i].path = `data:image/png;base64,${imgList[i].path}`
    //     image.src = imgList[i].path = `${imgList[i].path}`
    //   }
    // },
    getImageUrl,
    utcToLocalDateTimeSmall,
    utcToLocalDateTimeSmallComma
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
  &__task-row {
    border: 1px solid #DCDFE6;
    border-radius: .8rem;
    margin: .3rem .1rem;
    display: flex;
    &--left {
      padding-top: 0.625rem;
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
    &--normal {
      height: 3.75rem;
      padding: 0.625rem 0 0rem .2rem;
      border-bottom: 1px solid #DCDFE6;
    }
    &--buyer {
      border-bottom: 1px solid #DCDFE6;
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
  &__delivery {
    &-title {
      font-size: .9rem;
      font-weight: 600;
      color: #606266;
    }
    &-address {
      margin-top: 0.625rem;
      &-bottom {
        float: left;
        width: 100%;
        // margin-bottom: -0.9375rem;
      }
    }
  }
  &__btn {
    display: flex;
    justify-content: center;
    margin: 2rem;
    &-add {
      margin: .5rem 0;
    }

    &-reset {
      margin-left: 6rem;
    }
  }
}
</style>
