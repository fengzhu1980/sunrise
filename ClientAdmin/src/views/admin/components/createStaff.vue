<template>
  <div class="app-container">
    <div v-show="dialogStatus === 'browse'" class="switch__box">
      <el-switch
        v-model="formEditable"
        active-text="Edit"
        inactive-text="Browse"
        @change="handleSwitchChanged"
      />
    </div>
    <el-form
      ref="staffForm"
      v-loading="loading"
      :model="staffForm"
      :rules="dialogStatus === 'create' ? rules : rulesUpdate"
      :disabled="!formEditable"
      status-icon
      label-width="150px"
    >
      <el-row>
        <el-col :span="11">
          <el-form-item label="First Name" prop="firstName">
            <el-input v-model="staffForm.firstName" size="mini" />
          </el-form-item>
        </el-col>
        <el-col :span="11">
          <el-form-item label="Middle Name" prop="middleName">
            <el-input v-model="staffForm.middleName" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item label="Last Name" prop="lastName">
            <el-input v-model="staffForm.lastName" size="mini" />
          </el-form-item>
        </el-col>
        <el-col :span="11">
          <el-form-item label="Mobile" prop="mobile">
            <el-input v-model="staffForm.mobile" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item label="Phone Number" prop="phoneNumber">
            <el-input v-model="staffForm.phoneNumber" size="mini" />
          </el-form-item>
        </el-col>
        <el-col :span="11">
          <el-form-item label="Staff Code" prop="staffCode">
            <el-input v-model="staffForm.staffCode" disabled size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item label="Roles" prop="roleIds">
            <el-select v-model="staffForm.roleIds" multiple size="mini" placeholder="Please select">
              <el-option
                v-for="item in userRoles"
                :key="item.id"
                :label="item.name"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
        </el-col>
        <el-col :span="11">
          <el-form-item label="Gender" prop="gender">
            <el-select v-model="staffForm.gender" size="mini" placeholder="Please select">
              <el-option
                v-for="item in genderOptions"
                :key="item.value"
                :label="item.label"
                :value="item.value"
              />
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="Email" prop="email">
            <el-input
              v-model="staffForm.email"
              :disabled="!(dialogStatus === 'create')"
              size="mini"
              @change="handleEmailExist"
            />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="Address" prop="address">
            <el-input v-model="staffForm.address" type="password" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="Password" prop="password">
            <el-input v-model="staffForm.password" type="password" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="Confirm Password" prop="confirmPassword">
            <el-input v-model="staffForm.confirmPassword" type="password" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item>
            <el-switch
              v-model="staffForm.isAdmin"
              active-text="Is Admin"
            />
          </el-form-item>
        </el-col>
        <el-col :span="11">
          <el-form-item>
            <el-switch
              v-model="staffForm.isActive"
              active-text="Is active"
            />
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <div>
      <i-upload-image
        :id="staffForm.documentId"
        :path="staffForm.image"
        :display="pictureDelDisplay"
        :type="fileType"
        @handleGetDocumentChanged="handleGetDocumentChanged"
      />
    </div>
    <div class="btn__box">
      <el-button class="shadow" @click="handleCancelClicked">Cancel</el-button>
      <el-button
        :disabled="!formEditable"
        class="shadow"
        type="primary"
        @click="dialogStatus === 'create' ? addNewStaffSubmit() : updateStaffSubmit()"
      >
        {{ dialogStatus === 'create' ? 'Create' : 'Update' }}
      </el-button>
    </div>
  </div>
</template>

<script>
import { addNewStaff, updateStaff } from '@/api/staff'
import { getAllRoles } from '@/api/admin'
import { emailexists } from '@/api/user'
import UploadImage from '@/components/Upload/image'

export default {
  components: {
    iUploadImage: UploadImage
  },
  props: {
    dialogStatus: {
      type: String,
      required: true
    },
    form: {
      type: Object,
      required: true,
      default: () => {}
    }
  },
  data() {
    var validateEmail = (rule, value, callback) => {
      this.handleEmailExist().then(res => {
        if (res) callback(new Error('This email already exist'))
        callback()
      }).catch(() => {
        this.$message.error('Check email failed.')
        callback()
      })
    }
    var validateConfirmPwd = (rule, value, callback) => {
      if (!value) {
        callback(new Error('Please input Confirm Password'))
      } else if (value !== this.staffForm.password) {
        callback(new Error('Confirm Password does not match'))
      } else {
        callback()
      }
    }
    var validateConfirmPwdUpdate = (rule, value, callback) => {
      if (value && value !== this.staffForm.password) {
        callback(new Error('Confirm Password does not match'))
      } else {
        callback()
      }
    }
    return {
      loading: false,
      staffForm: {},
      formEditable: true,
      pictureDelDisplay: true,
      userRoles: [],
      fileType: 'Avatar',
      fileList: [],
      genderOptions: [{
        value: 'Male',
        label: 'Male'
      }, {
        value: 'Female',
        label: 'Female'
      }, {
        value: 'Other',
        label: 'Other'
      }],
      rules: {
        firstName: [
          {
            required: true,
            message: 'The FirstName field is required',
            trigger: ['blur', 'change']
          }
        ],
        lastName: [
          {
            required: true,
            message: 'The LastName field is required',
            trigger: 'blur'
          }
        ],
        mobile: [
          {
            pattern: /(^[0]\d{1})(\d{7}$)|(^[0][2]\d{1})(\d{6,8}$)|([0][8][0][0])([\s])(\d{5,8}$)/,
            message: 'Please input valid phone number',
            trigger: ['blur', 'change']
          }
        ],
        roleIds: [
          {
            type: 'array',
            required: true,
            message: 'The Roles field is required',
            trigger: ['change']
          }
        ],
        password: [
          {
            required: true,
            // pattern: '^(?![a-zA-Z]+$)(?![A-Z0-9]+$)(?![A-Z\\W_]+$)(?![a-z0-9]+$)(?![a-z\\W_]+$)(?![0-9\\W_]+$)[a-zA-Z0-9\\W_]{8,}$',
            pattern:
              '^(?![A-Za-z0-9]+$)(?![a-z0-9\\W]+$)(?![A-Za-z\\W]+$)(?![A-Z0-9\\W]+$)[a-zA-Z0-9\\W]{6,}$',
            message: `Passwords must have at least one non letter or digit character, one digit ('0'-'9'), one uppercase ('A'-'Z').`,
            trigger: 'blur'
          }
        ],
        confirmPassword: [
          { validator: validateConfirmPwd, trigger: 'blur' }
        ],
        email: [
          {
            required: true,
            message: 'The Email field is required',
            trigger: 'blur'
          },
          {
            type: 'email',
            message: 'Email not correct',
            trigger: ['blur']
          },
          {
            validator: validateEmail,
            trigger: 'blur'
          }
        ]
      },
      rulesUpdate: {
        firstName: [
          {
            required: true,
            message: 'The FirstName field is required',
            trigger: 'blur'
          }
        ],
        lastName: [
          {
            required: true,
            message: 'The LastName field is required',
            trigger: 'blur'
          }
        ],
        roleIds: [
          {
            type: 'array',
            required: true,
            message: 'The Roles field is required',
            trigger: ['change']
          }
        ],
        password: [
          {
            pattern:
              '^(?![A-Za-z0-9]+$)(?![a-z0-9\\W]+$)(?![A-Za-z\\W]+$)(?![A-Z0-9\\W]+$)[a-zA-Z0-9\\W]{6,}$',
            message: `Passwords must have at least one non letter or digit character, one digit ('0'-'9'), one uppercase ('A'-'Z').`,
            trigger: ['blur']
          }
        ],
        confirmPassword: [
          { validator: validateConfirmPwdUpdate, trigger: ['blur'] }
        ]
      }
    }
  },
  watch: {
    form(val) {
      this.checkFormStatus()
    }
  },
  created() {
    this.checkFormStatus()
    this.getRoleList()
  },
  methods: {
    checkFormStatus() {
      this.staffForm = Object.assign({}, this.form)
      this.$nextTick(() => {
        this.$refs['staffForm'].clearValidate()
      })
      if (this.dialogStatus === 'create' || this.dialogStatus === 'update') {
        this.formEditable = true
        this.pictureDelDisplay = true
      }
      if (this.dialogStatus === 'browse') {
        this.formEditable = false
        this.pictureDelDisplay = false
      }
    },
    getRoleList() {
      this.loading = true
      getAllRoles().then(res => {
        this.userRoles = res
        this.loading = false
      }).catch(e => {
        this.$message.error('Get role list failed ' + e)
        this.loading = false
      })
    },
    handleSwitchChanged(val) {
      this.formEditable = val
      this.pictureDelDisplay = val
    },
    handleGetDocumentChanged(id, path) {
      this.staffForm.documentId = id
      this.staffForm.image = path
    },
    addNewStaffSubmit() {
      this.$refs.staffForm.validate(valid => {
        this.loading = true
        if (valid) {
          addNewStaff(this.staffForm).then(res => {
            this.$message.success('Create new staff successfully.')
            this.loading = false
            this.$emit('handleActionBtnClick')
          }).catch(e => {
            this.loading = false
          })
        } else {
          this.loading = false
          return false
        }
      })
    },
    updateStaffSubmit() {
      this.$refs['staffForm'].validate(valid => {
        this.loading = true
        if (valid) {
          updateStaff(this.staffForm).then(res => {
            this.$message.success('Update staff information successfully.')
            this.loading = false
            this.$emit('handleActionBtnClick')
          }).catch(e => {
            this.loading = false
          })
        } else {
          this.loading = false
          return false
        }
      })
    },
    async handleEmailExist() {
      const param =
      {
        email: this.staffForm.email
      }
      let result = false
      if (this.staffForm.email) {
        result = (await emailexists(param))
      }
      return result
    },
    handleCancelClicked() {
      this.$emit('handleCancelClick')
    }
  }
}
</script>

<style lang="scss" scoped>
.app-container {
  .switch {
    &__box {
      position: absolute;
      right: 5.5rem;
      top: 2.5rem;
    }
  }
}
</style>
