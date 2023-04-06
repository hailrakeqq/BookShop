import { createApp } from 'vue'
import { createPinia } from 'pinia'
import axios from 'axios'
import App from './App.vue'
import router from './router'
import components from '@/component/UI'

const app = createApp(App)

components.forEach(component => app.component(component.name, component))

app.use(createPinia())
app.use(router)

axios.interceptors.response.use(response => response, error => {
    //if user unauthorized and error about if user have current book, don't redirect to login
    if(error.includes("CheckIfUserHaveThisBook"))
        return
    
    if(error.response.status === 401 && (localStorage.getItem('accessToken') == undefined))
        router.push('/login');
    else 
        axios.post(`http://localhost:5045/api/Auth/refresh-token`, {}, {
            headers: {
                'UserId': localStorage.getItem('id'),
                'refreshToken': `${localStorage.getItem('refreshToken')}`,
            }
        }).then(response => {
            localStorage.setItem('accessToken', response.data)
            return axios(error.config)
        })
})
axios.interceptors.request.use(config => {
    if (localStorage.getItem('accessToken')) {
        config.headers.Authorization = `Bearer ${localStorage.getItem('accessToken')}`
    }
    return config
})

app.mount('#app')
