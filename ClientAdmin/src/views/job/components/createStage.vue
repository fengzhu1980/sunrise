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
      ref="stageForm"
      v-loading="loading"
      :model="stageForm"
      :rules="rules"
      :disabled="!formEditable"
      status-icon
      label-width="150px"
    >
      <el-row>
        <el-col :span="22">
          <el-form-item label="Name" prop="name">
            <el-input v-model="stageForm.name" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="Notes" prop="notes">
            <el-input v-model="stageForm.notes" type="textarea" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="Priority" prop="priority">
            <el-input-number v-model="stageForm.priority" :min="0" size="mini" label="priority" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item>
            <el-switch
              v-model="stageForm.isActive"
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
        @click="dialogStatus === 'create' ? addNewStageSubmit() : updateStageSubmit()"
      >
        {{ dialogStatus === 'create' ? 'Create' : 'Update' }}
      </el-button>
    </div>
  </div>
</template>

<script>
import { createNewJobStage, updateJobStage } from '@/api/job'

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
      stageForm: {},
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
      this.stageForm = Object.assign({}, this.form)
      this.$nextTick(() => {
        this.$refs['stageForm'].clearValidate()
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
    addNewStageSubmit() {
      this.$refs.stageForm.validate(valid => {
        this.loading = true
        if (valid) {
          createNewJobStage(this.stageForm).then(res => {
            this.$message.success('Create new stage successfully.')
            this.loading = false
            this.$emit('handleActionBtnClick')
          }).catch(e => {
            this.loading = false
            this.$message.error('Create new stage failed. ' + e)
          })
        } else {
          this.loading = false
          return false
        }
      })
    },
    updateStageSubmit() {
      this.$refs['stageForm'].validate(valid => {
        this.loading = true
        if (valid) {
          updateJobStage(this.stageForm).then(res => {
            this.$message.success('Update stage information successfully.')
            this.loading = false
            this.$emit('handleActionBtnClick')
          }).catch(e => {
            this.loading = false
            this.$message.error('Update stage information failed. ' + e)
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
