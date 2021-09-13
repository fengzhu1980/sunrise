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
      ref="roleForm"
      v-loading="loading"
      :model="roleForm"
      :rules="rules"
      :disabled="!formEditable"
      status-icon
      label-width="150px"
    >
      <el-row>
        <el-col :span="22">
          <el-form-item label="Name" prop="name">
            <el-input v-model="roleForm.name" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="Notes" prop="notes">
            <el-input v-model="roleForm.notes" type="textarea" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item>
            <el-switch
              v-model="roleForm.isActive"
              active-text="Is active"
            />
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <div class="btn__box">
      <el-button class="shadow" @click="handleCancelClicked">Cancel</el-button>
      <el-button
        :disabled="!formEditable"
        class="shadow"
        type="primary"
        @click="dialogStatus === 'create' ? createNewRoleSubmit() : updateRoleSubmit()"
      >
        {{ dialogStatus === 'create' ? 'Create' : 'Update' }}
      </el-button>
    </div>
  </div>
</template>

<script>
import { createNewRole, updateRole } from '@/api/admin'

export default {
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
    return {
      loading: false,
      roleForm: {},
      formEditable: true,
      rules: {
        name: [
          {
            required: true,
            message: 'The name field is required',
            trigger: ['blur', 'change']
          }
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
  },
  methods: {
    checkFormStatus() {
      this.roleForm = Object.assign({}, this.form)
      this.$nextTick(() => {
        this.$refs['roleForm'].clearValidate()
      })
      if (this.dialogStatus === 'create' || this.dialogStatus === 'update') {
        this.formEditable = true
      }
      if (this.dialogStatus === 'browse') {
        this.formEditable = false
      }
    },
    handleSwitchChanged(val) {
      this.formEditable = val
    },
    createNewRoleSubmit() {
      this.$refs.roleForm.validate(valid => {
        this.loading = true
        if (valid) {
          createNewRole(this.roleForm).then(res => {
            this.$message.success('Create new role successfully.')
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
    updateRoleSubmit() {
      this.$refs['roleForm'].validate(valid => {
        this.loading = true
        if (valid) {
          updateRole(this.roleForm).then(res => {
            this.$message.success('Update role information successfully.')
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
