<template>
    <div v-if="errorMessage" class="alert alert-danger" role="alert">
        {{ errorMessage }}
    </div>
  <div>
    <div class="input-group mb-3">
        <input type="text" class="form-control" placeholder="Select your username"
        v-model="signUpInput.userName">
    </div>
    <div class="input-group mb-3">
        <input type="password" class="form-control" placeholder="Choose your password"
        v-model="signUpInput.password">
    </div>
    <div class="input-group mb-3">
        <input type="password" class="form-control" placeholder="Repeat your password"
        v-model="signUpInput.secondPassword">
    </div>
    <div class="m-2 d-flex justify-content-evenly">
        <button class="btn btn-danger" @click="$emit('cancelSignup')">Cancel</button>
        <button class="btn btn-success" @click="register">Register</button>
    </div>
</div>
</template>

<script>
import { ref } from 'vue'
import axios from 'axios'
export default {
    name: 'signupInputs',
    emits: ['cancelSignup', 'user-logged-in'],
    setup(_, { emit }){
        let errorMessage = ref();
        const signUpInput = ref({
        userName: '',
        password: '',
        secondPassword: '',
        });

        const register = async () => {
            if(!signUpInput.value.userName || !signUpInput.value.password || !signUpInput.value.secondPassword){
                errorMessage.value = "Fill in all required fields";
                return;
            }
            if(signUpInput.value.password != signUpInput.value.secondPassword){
                errorMessage.value = "Passwords do not match";
                return;
            };
            try{
                const newUser = {   userName: signUpInput.value.userName,
                                    passwordHash: signUpInput.value.password,
                };
                await axios.post(`http://localhost:5118/D&D/users`, newUser,
                    { headers: {"Content-Type": "application/json"}}
                );
    
                console.log("User registered")
                await login(newUser)
            }
            catch(error){
                errorMessage ="Registration failed, please try again later";
                console.error(error);
            }
        }

        const login = async (newUser) => {
            try {
                const user = { UserName: newUser.userName, Password: newUser.passwordHash}
                const response = await axios.post(
                    `http://localhost:5118/api/auth/login`, user, 
                    { headers: {"Content-Type": "application/json"}}
                );

                const token = response.data.token
                localStorage.setItem("jwt", token);

                const decodedToken = parseJwt(token);
                if(!decodedToken){
                    console.error("Error: Unable to parse token");
                }

                const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] 
                || decodedToken.sub || decodedToken.userId || decodedToken.id;
                if(!userId){ console.log("Error: userId not found in token")};

                localStorage.setItem("userId", userId);

                emit('user-logged-in');

            }
            catch(error){
                errorMessage.value = "Wrong username or password";
                console.log("Wrong username or password")
                console.error(error);
            }
        };
        const parseJwt = (token) => {
            try {
                return JSON.parse(atob(token.split('.')[1]));
            }
            catch(error){
                console.error("Error decoding JWT: ", error);
                return null
            }
        }


        return { signUpInput, register, errorMessage }
    }
}
</script>

<style>

</style>