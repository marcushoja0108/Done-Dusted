<template>
<div class="container-fluid d-flex justify-content-center align-items-center w-100 vh-100 bg-dark">
  <div class="row w-100">
    <div class="col-8 col-md-6 col-lg-4 mx-auto">
        <div class="text-center">
            <h2 class="text-white">Done & Dusted</h2>
            <img src="../assets/largeLogo.png" alt="MainLogo" class="img-fluid mb-3 rounded">
        </div>
        <div v-if="!signUp">
            <div class="input-group mb-3">
                <span class="input-group-text">@</span>
                <input type="text" class="form-control" placeholder="Username">
            </div>
            <div class="input-group">
                <span class="input-group-text">
                    <i class="bi bi-key"></i>
                </span>
                <input type="password" class="form-control" placeholder="password">
            </div>
            <div class="m-2 d-flex justify-content-evenly">
                <button class="btn btn-info" @click="signUp=!signUp">Sign up</button>
                <button class="btn btn-success">Login</button>
            </div>
        </div>
        <div v-else>
            <div class="input-group mb-3">
                <input type="text" class="form-control" placeholder="Select your username">
            </div>
            <div class="input-group mb-3">
                <input type="password" class="form-control" placeholder="Choose your password">
            </div>
            <div class="input-group mb-3">
                <input type="password" class="form-control" placeholder="Repeat your password">
            </div>
            <div class="m-2 d-flex justify-content-evenly">
                <button class="btn btn-danger" @click="signUp=!signUp">Cancel</button>
                <button class="btn btn-success">Register</button>
            </div>
        </div>
    </div>
  </div>
</div>
</template>

<script>
import { ref} from 'vue'

export default {
    name: 'LoginView',
    setup(){
        const signUp = ref(false)

        return { signUp }
    }
}
</script>

<style>

</style>