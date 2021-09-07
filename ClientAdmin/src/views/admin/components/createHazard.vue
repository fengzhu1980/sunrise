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
      ref="hazardForm"
      v-loading="loading"
      :model="hazardForm"
      :rules="rules"
      :disabled="!formEditable"
      status-icon
      label-width="150px"
    >
      <el-row>
        <el-col :span="22">
          <el-form-item label="Title" prop="title">
            <el-input v-model="hazardForm.title" size="mini" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="Description" prop="description">
            <el-input
              v-model="hazardForm.description"
              type="textarea"
              size="mini"
            />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="22">
          <el-form-item label="SWMS" prop="safeWorkMethodStatementIds">
            <el-select
              v-model="hazardForm.safeWorkMethodStatementIds"
              v-loading="listLoading"
              multiple
              size="mini"
              placeholder="Please Select"
            >
              <el-option
                v-for="item in swmsOptions"
                :key="item.id"
                :label="item.title"
                :value="item.id"
              />
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="11">
          <el-form-item>
            <el-switch v-model="hazardForm.isActive" active-text="Is active" />
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
        @click="
          dialogStatus === 'create'
            ? addNewHazardSubmit()
            : updateHazardSubmit()
        "
      >
        {{ dialogStatus === "create" ? "Create" : "Update" }}
      </el-button>
    </div>
  </div>
</template>

<script>
import { addNewHazard, updateHazard, getSWMSList } from '@/api/admin'

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
      listLoading: false,
      hazardForm: {},
      formEditable: true,
      swmsOptions: [],
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
    this.getSWMS()
  },
  methods: {
    checkFormStatus() {
      this.hazardForm = Object.assign({}, this.form)
      this.$nextTick(() => {
        this.$refs['hazardForm'].clearValidate()
      })
      if (this.dialogStatus === 'create' || this.dialogStatus === 'update') {
        this.formEditable = true
      }
      if (this.dialogStatus === 'browse') {
        this.formEditable = false
      }
    },
    getSWMS() {
      this.listLoading = true
      getSWMSList().then(res => {
        console.log('res', res)
        this.swmsOptions = res.data
        this.listLoading = false
      }).catch(e => {
        this.$message.error('Get swms failed ' + e)
        this.listLoading = false
      })
    },
    handleSwitchChanged(val) {
      this.formEditable = val
    },
    addNewHazardSubmit() {
      this.$refs.hazardForm.validate(valid => {
        this.loading = true
        if (valid) {
          addNewHazard(this.hazardForm)
            .then(res => {
              this.$message.success('Create new hazard successfully.')
              this.loading = false
              this.$emit('handleActionBtnClick')
            })
            .catch(e => {
              this.loading = false
              this.$message.error('Create new hazard failed. ' + e)
            })
        } else {
          this.loading = false
          return false
        }
      })
    },
    updateHazardSubmit() {
      this.$refs['hazardForm'].validate(valid => {
        this.loading = true
        if (valid) {
          updateHazard(this.hazardForm)
            .then(res => {
              this.$message.success('Update hazard information successfully.')
              this.loading = false
              this.$emit('handleActionBtnClick')
            })
            .catch(e => {
              this.loading = false
              this.$message.error('Update hazard information failed. ' + e)
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
.el-select {
  width: 100%;
}
</style>
