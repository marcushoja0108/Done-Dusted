<template>
    <div v-if="errorMessage" class="alert alert-danger" role="alert">
        {{ errorMessage }}
    </div>
  <div>
    <div class="input-group mb-3">
        <span class="input-group-text">@</span>
        <input type="text" class="form-control" placeholder="Username"
        v-model="loginInput.UserName">
    </div>
    <div class="input-group">
        <span class="input-group-text">
            <i class="bi bi-key"></i>
        </span>
        <input type="password" class="form-control" placeholder="password"
        v-model="loginInput.Password">
    </div>
    <div class="m-2 d-flex justify-content-evenly">
        
        <button class="btn btn-success" @click="login">Login</button>
    </div>
</div>
</template>

<script>
import { ref } from 'vue'
import axios from 'axios'

export default {
    name: 'loginInputs',
    emits: ['user-logged-in'],

    setup(_, { emit }){
        let errorMessage = ref();
        const loginInput = ref({
        UserName: '',
        Password: '',
        });
        
        const login = async () => {
            try {
                const inputs = loginInput.value
                console.log("Sending login request:", inputs)
                const response = await axios.post(
                    `http://localhost:5118/api/auth/login`, inputs, 
                    { headers: {"Content-Type": "application/json"}}
                );

                const token = response.data.token
                localStorage.setItem("jwt", token);

                const decodedToken = parseJwt(token);
                if(!decodedToken){
                    console.error("Error: Unable to parse token");
                }

                //gets spesifically id from payload
                const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] 
                || decodedToken.sub || decodedToken.userId || decodedToken.id;
                if(!userId){ console.log("Error: userId not found in token")};

                localStorage.setItem("userId", userId);

                emit('user-logged-in');
            }
            catch(error){
                errorMessage.value = "Wrong username or password";
                console.error(error);
            }
        };

        //splits jwt and returns the payload
        const parseJwt = (token) => {
            try {
                return JSON.parse(atob(token.split('.')[1]));

                // special characters. I dont understand this
                // const base64Url = token.split('.')[1];
                // const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
                // const jsonPayload = decodeURIComponent(
                //     atob(base64)
                //     .split('')
                //     .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                //     .join('')
                // );
                // return JSON.parse(jsonPayload);
            }
            catch(error){
                console.error("Error decoding JWT: ", error);
                return null
            }
        }


        return { loginInput, login, errorMessage }
    }
}
</script>

<style>

</style>