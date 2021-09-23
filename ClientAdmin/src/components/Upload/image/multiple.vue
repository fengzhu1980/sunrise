<template>
  <div class="app-container">
    <div v-if="documentsReceived && documentsReceived.length > 0">
      <div class="document__border">
        <div
          v-for="(item, index) in documentsReceived"
          :key="index"
          class="document__image"
        >
          <img :src="getImageUrl(item.relativeFilePath)" alt="Photo" class="document__image-item">
          <div class="document__image-select">
            <i class="el-icon-circle-check document__image-select--icon" />
          </div>
          <div class="document__image-delete">
            <i class="el-icon-delete document__image-delete--icon" @click="handleRemoveDocumentGet(item)" />
          </div>
        </div>
      </div>
    </div>
    <el-upload
      ref="upload"
      :disabled="uploadDisabled"
      :on-remove="handleRemove"
      :http-request="handlePicUpload"
      :before-upload="beforeAvatarUpload"
      :show-file-list="true"
      :action="`${baseURL}/api/admin/uploadDocument`"
      :multiple="true"
      list-type="picture-card"
    >
      <i class="el-icon-plus" />
    </el-upload>
    <el-progress
      v-if="startUpload"
      :percentage="uploadPercentage"
    />
  </div>
</template>

<script>
import { uploadDocument } from '@/api/admin'
import { getImageUrl } from '@/utils/custom'

export default {
  props: {
    documentsGet: {
      type: Array,
      default: () => []
    },
    type: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      documentsReceived: [],
      documentType: '',
      uploadDisabled: false,
      startUpload: false,
      uploadPercentage: 0,
      baseURL: process.env.BASE_API
    }
  },
  watch: {
    documentsGet() {
      this.checkDocument()
    },
    type() {
      this.checkDocument()
    }
  },
  created() {
    this.checkDocument()
  },
  methods: {
    clearFiles() {
      if (this.$refs.upload) {
        this.$refs.upload.clearFiles()
      }
    },
    checkDocument() {
      this.clearFiles()
      this.documentsReceived = this.documentsGet
      this.documentType = this.type
    },
    beforeAvatarUpload(file) {
      const isLt2M = file.size / 1024 / 1024 < 2
      const isJPG = file.type === 'image/jpeg'
      const isPNG = file.type === 'image/png'
      const isGIF = file.type === 'image/gif'

      if (!isJPG && !isPNG && !isGIF) {
        this.$message.error('Allowed JPG/PNG/GIF format only!')
      }
      if (!isLt2M) {
        this.$message.error('Image size should be less than 2MB!')
      }
      return (isJPG || isPNG || isGIF) && isLt2M
    },
    handlePicUpload(request) {
      this.uploadPercentage = 0
      this.uploadDisabled = true
      this.startUpload = true
      const uploadRequest = {}
      uploadRequest.data = new FormData()
      uploadRequest.data.append('file', request.file)
      uploadRequest.data.append('documentType', this.type)
      // uploadRequest.onUploadProgress = this.picUploadOnProgress
      uploadDocument(uploadRequest).then(res => {
        // Add into picturesUploadedNew array
        const tempDocument = {
          id: res.id,
          uid: request.file.uid
        }
        this.handleAddNewDocument(tempDocument)
        this.startUpload = false
        this.uploadDisabled = false
      }).catch((e) => {
        this.$message.error('Upload image failed ' + e)
        this.uploadDisabled = false
        this.startUpload = false
      })
    },
    handleAddNewDocument(document) {
      this.$emit('handleAddNewDocument', document)
    },
    handleRemoveDocumentGet(item) {
      this.$emit('handleRemoveUploadedImg', item)
    },
    handleRemove(file, fileList) {
      this.$emit('handleRemove', file, fileList)
    },
    picUploadOnProgress(uploadEvent) {
      const percent = Math.round(uploadEvent.loaded * 100 / uploadEvent.total)
      this.uploadPercentage = percent
    },
    getImageUrl
  }
}
</script>
<style lang="scss" scoped>
.app-container {
  .document {
    &__border {
      display: flex;
      flex-wrap: wrap;
      width: 100%;
    }
    &__image {
      position: relative;
      margin: 0 0.5rem 0.5rem 0;
      cursor: pointer;
      overflow: hidden;
      border: 1px solid;
      border-color: transparent;

      display: flex;
      align-items: center;

      border-radius: 0.5rem;

      &:hover {
        border: 1px solid #409EFF;
        transition: 0.8s;
        background-color: #409EFF;
      }

      &-item {
        height: 7rem;
      }

      &-select {
        height: 1rem;
        width: 1rem;
        position: absolute;
        right: 0.5rem;
        bottom: 0.5rem;
        color: #409EFF;
        font-size: 1.5rem;
        display: none;

        &--icon {
          position: absolute;
        }
      }

      &-delete {
        display: flex;
        justify-content: center;
        align-items: center;
        position: absolute;
        width: 100%;
        height: 100%;
        left: 0;
        top: 0;
        cursor: default;
        text-align: center;
        color: #fff;
        opacity: 0;
        font-size: 1.2rem;
        background-color: rgba(0, 0, 0, .5);
        transition: opacity .3s;
        &:hover {
          opacity: 1;
        }
        &--icon {
          cursor: pointer;
        }
      }
    }
  }
}
</style>
