<template>
  <div class="main">
    <div class="item">
      <h3>Username</h3>
      <h4>{{user.username}}</h4>
      <my-button class="btn" @click="showDialogChangeUsername">Change Username</my-button>
      <my-dialog v-model:show="dialogVisibleChangeUsername" @click="hideDialog">
        <p>New Username:</p>
        <my-input v-model="newUsername" class="new-username"/>
        <p>Confirm Password:</p>
        <my-input type="password" v-model="confirmPassword" class="confirm-password"/>
        <my-button @click="sendUserDataRequest">Submit</my-button>
      </my-dialog>
    </div>
    <div class="item">
      <h3>Email</h3>
      <h4>{{user.email}}</h4> 
      <my-button class="btn" @click="showDialogChangeEmail">Change Email</my-button>
      <my-dialog v-model:show="dialogVisibleChangeEmail" @click="hideDialog">
        <p>New Email:</p>
        <my-input v-model="newEmail" class="new-email"/>
        <p>Confirm Password:</p>
        <my-input type="password" v-model="confirmPassword" class="confirm-password"/>
        <my-button @click="sendUserDataRequest">Submit</my-button>
      </my-dialog>
    </div>
    <div class="item">
      <h3>Password</h3>
      <my-button class="btn" @click="showDialogPassword">Change Password</my-button>
      <my-dialog v-model:show="dialogVisiblePasswordChanger" @click="hideDialog">
        <p>Old Password:</p>
        <my-input type="password" v-model="confirmPassword" class="confirm-password"/>
        <p>New Password:</p>
        <my-input type="password" v-model="newPassword" class="new-password"/>
        <my-button @click="sendUserDataRequest">Submit</my-button>
      </my-dialog>
    </div>
    <div class="item">
      <h3>Delete Account</h3>
      <my-button class="btn" @click="showDialogDeleteAccount">Delete Account</my-button>
      <my-dialog v-model:show="dialogVisibleDeleteAccount" @click="hideDialog">
        <h4>Delete Account?</h4>
        <p>Confirm password:</p>
        <my-input type="password" v-model="confirmPassword" class="confirm-password"/>
        <my-button @click="deleteAccount">Submit</my-button>
      </my-dialog>
    </div>
  </div>
</template>

<script>
import axios from 'axios'
import MyButton from '@/component/UI/MyButton.vue'
import MyDialog from '@/component/UI/MyDialog.vue'
import MyInput from '@/component/UI/MyInput.vue'
export default {
name: "UserSettings",
  components:{
    MyButton, MyDialog, MyInput
  },
  data(){
    return{
      user:{
        id: localStorage.getItem('id'),
        username: localStorage.getItem('username'),
        email: this.changeUserEmailForm(),
      },
      confirmPassword: '',
      newEmail: '',
      newPassword: '',
      newUsername: '',
      dialogVisibleChangeUsername: false,
      dialogVisibleChangeEmail: false,
      dialogVisibleDeleteAccount: false,
      dialogVisiblePasswordChanger: false
    }
  },
  computed:{
  },
  methods: {
    changeUserEmailForm(){
      const email = localStorage.getItem('email')
      let splitEmail = email.split('@')
      const domain = splitEmail[0].replace(`${splitEmail[0]}`, '*'.repeat(splitEmail[0].length))
      return `${domain}@${splitEmail[1]}`
    },
    showDialogPassword(){
      this.dialogVisiblePasswordChanger = true;
      this.confirmPassword = ''
    },
    showDialogDeleteAccount(){
      this.dialogVisibleDeleteAccount = true;
      this.confirmPassword = ''
    },
    showDialogChangeEmail(){
      this.dialogVisibleChangeEmail = true;
      this.confirmPassword = ''
    },
    showDialogChangeUsername(){
      this.dialogVisibleChangeUsername = true;
      this.confirmPassword = ''
    },
    sendUserDataRequest(){
      if(this.confirmPassword != null || this.confirmPassword !== '') {
        const newUserData = {
          email: this.newEmail,
          username: this.newUsername,
          password: this.newPassword,
          confirmPassword: this.confirmPassword
        }
        console.log(newUserData)
        axios.put(`http://localhost:5045/api/User/ChangeUserData/${localStorage.getItem('id')}`, 
            JSON.stringify(newUserData),{
            headers:{
              'Content-Type': 'application/json',
              'Authorization': `bearer ${localStorage.getItem('jwtToken')}`,
            }
        }).then(response => {
          console.log(response.status)
          if(response.status === 200){
            this.dialogVisibleChangeUsername = false
            this.dialogVisibleChangeEmail = false
            this.dialogVisibleDeleteAccount= false
            this.dialogVisiblePasswordChanger= false
          }
        })
      }
    },
    deleteAccount(){
      if(this.confirmPassword != null || this.confirmPassword !== '') {
        axios.delete(`http://localhost:5045/api/User/${localStorage.getItem('id')}`,{
            headers:{
              'Content-Type': 'application/json',
              'Authorization': `bearer ${localStorage.getItem('jwtToken')}`,
            }
          }).then(response => {
            if(response.status === 200){
              localStorage.clear()
              this.$router.go()
              this.$router.push('/')
            }
        })
      }
      else
        alert("You should enter password")
    },
  },
 
}
</script>

<style scoped>
.main{ 
  padding: 50px;
  margin-left: 50px;
}
</style>