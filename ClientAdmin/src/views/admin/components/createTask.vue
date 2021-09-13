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
      ref="taskForm"
      v-loading="loading"
      :model="taskForm"
      :rules="rules"
      :disabled="!formEditable"
      status-icon
      label-width="150px"
    >
      <el-row>
        <el-col :span="22">
          <el-form-item label="Name" prop="name">
            <el-input v-model="taskForm.name" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="Task Fee($)" prop="taskFee">
            <el-input-number v-model="taskForm.taskFee" :min="0" size="mini" label="Task Fee" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="Duration(Hour)" prop="duration">
            <el-input-number v-model="taskForm.duration" :min="0" size="mini" label="Duration" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item>
            <el-switch
              v-model="taskForm.isActive"
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
        @click="dialogStatus === 'create' ? addNewTaskSubmit() : updateTaskSubmit()"
      >
        {{ dialogStatus === 'create' ? 'Create' : 'Update' }}
      </el-button>
    </div>
  </div>
</template>

<script>
import { addNewTask, updateTask } from '@/api/admin'

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
      taskForm: {},
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
      this.taskForm = Object.assign({}, this.form)
      this.$nextTick(() => {
        this.$refs['taskForm'].clearValidate()
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
    addNewTaskSubmit() {
      this.$refs.taskForm.validate(valid => {
        this.loading = true
        if (valid) {
          addNewTask(this.taskForm).then(res => {
            this.$message.success('Create new task successfully.')
            this.loading = false
            this.$emit('handleActionBtnClick')
          }).catch(e => {
            this.loading = false
            this.$message.error('Create new task failed. ' + e)
          })
        } else {
          this.loading = false
          return false
        }
      })
    },
    updateTaskSubmit() {
      this.$refs['taskForm'].validate(valid => {
        this.loading = true
        if (valid) {
          updateTask(this.taskForm).then(res => {
            this.$message.success('Update task information successfully.')
            this.loading = false
            this.$emit('handleActionBtnClick')
          }).catch(e => {
            this.loading = false
            this.$message.error('Update task information failed. ' + e)
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
