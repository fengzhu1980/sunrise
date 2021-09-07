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
      ref="swmsForm"
      v-loading="loading"
      :model="swmsForm"
      :rules="rules"
      :disabled="!formEditable"
      status-icon
      label-width="150px"
    >
      <el-row>
        <el-col :span="22">
          <el-form-item label="Title" prop="title">
            <el-input v-model="swmsForm.title" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="Notes" prop="notes">
            <el-input v-model="swmsForm.notes" type="textarea" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item>
            <el-switch
              v-model="swmsForm.isActive"
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
        @click="dialogStatus === 'create' ? addNewSWMSSubmit() : updateSWMSSubmit()"
      >
        {{ dialogStatus === 'create' ? 'Create' : 'Update' }}
      </el-button>
    </div>
  </div>
</template>

<script>
import { addNewSWMS, updateSWMS } from '@/api/admin'

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
      swmsForm: {},
      formEditable: true,
      rules: {
        title: [
          {
            required: true,
            message: 'The title field is required',
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
      this.swmsForm = Object.assign({}, this.form)
      this.$nextTick(() => {
        this.$refs['swmsForm'].clearValidate()
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
    addNewSWMSSubmit() {
      this.$refs.swmsForm.validate(valid => {
        this.loading = true
        if (valid) {
          addNewSWMS(this.swmsForm).then(res => {
            this.$message.success('Create new swms successfully.')
            this.loading = false
            this.$emit('handleActionBtnClick')
          }).catch(e => {
            this.loading = false
            this.$message.error('Create new swms failed. ' + e)
          })
        } else {
          this.loading = false
          return false
        }
      })
    },
    updateSWMSSubmit() {
      this.$refs['swmsForm'].validate(valid => {
        this.loading = true
        if (valid) {
          updateSWMS(this.swmsForm).then(res => {
            this.$message.success('Update swms information successfully.')
            this.loading = false
            this.$emit('handleActionBtnClick')
          }).catch(e => {
            this.loading = false
            this.$message.error('Update swms information failed. ' + e)
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
