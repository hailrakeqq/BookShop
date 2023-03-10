<template>
  <div class="file-upload">
    <div class="file-upload__area">
      <div v-if="!fileInfo.isUploaded">
        <input type="file" name="" id="" @change="handleFileChange($event)" />
        <div v-if="errors.length > 0">
          <div
              class="file-upload__error"
              v-for="(error, index) in errors"
              :key="index"
          >
            <span>{{ error }}</span>
          </div>
        </div>
      </div>
      <div v-if="fileInfo.isUploaded" class="upload-preview">
        <img :src="fileInfo.url" v-if="fileInfo.isImage" class="file-image" alt="" />
        <div v-if="!fileInfo.isImage" class="file-extention">
          {{ fileInfo.fileExtention }}
        </div>
        <span class="file-name">
          {{ fileInfo.name }}{{ fileInfo.isImage ? `.${fileInfo.fileExtention}` : "" }}
        </span>
        <div class="">
          <button @click="resetFileInput">Change file</button>
        </div>
        <div class="" style="margin-top: 10px">
          <button @click="sendDataToParent">Select File</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "FileUpload",
  props: {
    maxSize: {
      type: Number,
      default: 5,
      required: true,
    },
    accept: {
      type: String,
      default: "image/*",
    },
  },
  data() {
    return {
      errors: [],
      isLoading: false,
      uploadReady: true,
      file: null,
      fileInfo: {
        name: "",
        size: 0,
        type: "",
        fileExtention: "",
        url: "",
        isImage: false,
        isUploaded: false,
      },
    };
  },
  methods: {
    handleFileChange(e) {
      this.errors = [];
      if (e.target.files && e.target.files[0]) {
        if (this.isFileValid(e.target.files[0])) {
          this.file = e.target.files[0]
          const file = e.target.files[0],
              fileSize = Math.round((file.size / 1024 / 1024) * 100) / 100,
              fileExtention = file.name.split(".").pop(),
              fileName = file.name.split(".").shift(),
              isImage = ["jpg", "jpeg", "png", "gif"].includes(fileExtention);
          console.log(fileSize, fileExtention, fileName, isImage);
          let reader = new FileReader();
          reader.addEventListener(
              "load",
              () => {
                this.fileInfo = {
                  name: fileName,
                  size: fileSize,
                  type: file.type,
                  fileExtention: fileExtention,
                  isImage: isImage,
                  url: reader.result,
                  isUploaded: true,
                };
              },
              false
          );
          reader.readAsDataURL(file);
        } else {
          console.log("Invalid file");
        }
      }
    },
    isFileSizeValid(fileSize) {
      if (fileSize <= this.maxSize) {
        console.log("File size is valid");
      } else {
        this.errors.push(`File size should be less than ${this.maxSize} MB`);
      }
    },
    isFileTypeValid(fileExtention) {
      if (this.accept.split(",").includes(fileExtention)) {
        console.log("File type is valid");
      } else {
        this.errors.push(`File type should be ${this.accept}`);
      }
    },
    isFileValid(file) {
      this.isFileSizeValid(Math.round((file.size / 1024 / 1024) * 100) / 100);
      this.isFileTypeValid(file.name.split(".").pop());
      if (this.errors.length === 0) {
        return true;
      } else {
        return false;
      }
    },
    resetFileInput() { //TODO: fix it
      this.uploadReady = false;
      this.$nextTick(() => {
        this.uploadReady = true;
        this.file = {
          name: "",
          size: 0,
          type: "",
          data: "",
          fileExtention: "",
          url: "",
          isImage: false,
          isUploaded: false,
        };
      });
    },
    sendDataToParent() {
      this.resetFileInput();
      this.$emit("file-uploaded", this.file, this.fileInfo);
    },
  },
};
</script>

<style scoped>
.file-upload {
  height: 100vh;
  width: 100%;
  display: flex;
  align-items: flex-start;
  justify-content: center;
}
.file-upload .file-upload__area {
  width: 600px;
  min-height: 200px;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 2px dashed #ccc;
  margin-top: 40px;
  padding: 20px;
}
.file-upload .file-upload__error {
  margin-top: 10px;
  color: #f00;
  font-size: 12px;
}
.file-upload .upload-preview {
  text-align: center;
}
.file-upload .upload-preview .file-image {
  width: 100%;
  height: 300px;
  object-fit: contain;
}
.file-upload .upload-preview .file-extention {
  height: 100px;
  width: 100px;
  border-radius: 8px;
  background: #ccc;
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 0.5em auto;
  font-size: 1.2em;
  padding: 1em;
  text-transform: uppercase;
  font-weight: 500;
}
.file-upload .upload-preview .file-name {
  font-size: 1.2em;
  font-weight: 500;
  color: #000;
  opacity: 0.5;
}
</style>