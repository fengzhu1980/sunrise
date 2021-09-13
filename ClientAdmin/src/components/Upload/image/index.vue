<template>
  <div class="app-container">
    <div v-if="documentId" class="avatar__box">
      <img :src="getImageUrl(documentPath)" alt="Image" class="avatar">
      <span v-show="documentId" :class="pictureDelDisplay ? '' : 'no-display'" class="avatar-action">
        <span class="avatar-action__icon">
          <i class="el-icon-delete" @click="handleRemoveImg" />
        </span>
      </span>
    </div>
    <el-upload
      v-show="!documentId"
      :disabled="uploadDisabled"
      :http-request="handlePicUpload"
      :before-upload="beforeAvatarUpload"
      :show-file-list="false"
      :action="`${baseURL}/api/admin/uploadDocument`"
      class="avatar-uploader"
    >
      <i class="el-icon-plus avatar-uploader-icon" />
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
    id: {
      type: String,
      default: () => ''
    },
    path: {
      type: String,
      default: () => ''
    },
    display: {
      type: Boolean,
      required: true
    },
    type: {
      type: String,
      required: true
    }
  },
  data() {
    return {
      documentId: '',
      documentPath: '',
      pictureDelDisplay: true,
      documentType: '',
      uploadDisabled: false,
      startUpload: false,
      uploadPercentage: 0,
      baseURL: process.env.BASE_API
    }
  },
  watch: {
    id() {
      this.checkPicture()
    },
    path() {
      this.checkPicture()
    },
    display() {
      this.checkPicture()
    },
    type() {
      this.checkPicture()
    }
  },
  created() {
    this.checkPicture()
  },
  methods: {
    checkPicture() {
      this.documentId = this.id
      this.documentPath = this.path
      this.pictureDelDisplay = this.display
      this.documentType = this.type
    },
    handleRemoveImg() {
      this.documentId = ''
      this.documentPath = ''
      this.handleGetDocumentChanged(this.documentId, this.documentPath)
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
      uploadRequest.data.append('documentType', this.documentType)
      // uploadRequest.onUploadProgress = this.picUploadOnProgress
      uploadDocument(uploadRequest).then(res => {
        this.documentId = res.id
        this.documentPath = res.relativeFilePath
        this.handleGetDocumentChanged(this.documentId, this.documentPath)
        this.startUpload = false
        this.uploadDisabled = false
      }).catch((err) => {
        console.log('err', err)
        this.uploadDisabled = false
        this.startUpload = false
      })
    },
    handleGetDocumentChanged(id, path) {
      this.$emit('handleGetDocumentChanged', id, path)
    },
    picUploadOnProgress(uploadEvent) {
      const percent = Math.round(uploadEvent.loaded * 100 / uploadEvent.total)
      this.uploadPercentage = percent
    },
    getImageUrl
  }
}
</script>
<style lang="scss">
.app-container {
  .avatar-uploader{
    .el-upload {
      border: 1px dashed #d9d9d9;
      border-radius: 6px;
      cursor: pointer;
      position: relative;
      overflow: hidden;
    }
    .el-upload:hover {
      border-color: #409EFF;
    }
  }
  .avatar-uploader-icon {
    font-size: 1.5rem;
    color: #8c939d;
    width: 10rem;
    height: 10rem;
    line-height: 10rem;
    text-align: center;
  }
  .avatar__box {
    position: relative;
    width: 10rem;
    height: 10rem;
    border: 1px dashed #d9d9d9;
    border-radius: 6px;
    cursor: pointer;
    overflow: hidden;
  }
  .avatar-action {
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
    &__icon {
      cursor: pointer;
    }
  }
  .avatar,
  .avatar:hover {
    width: 10rem;
    height: 10rem;
    display: block;
  }
}

.no-display {
  display: none !important;
}
</style>
