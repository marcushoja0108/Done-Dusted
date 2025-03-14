<template>
<div class="container-fluid d-flex justify-content-center align-items-center w-100 vh-100 bg-dark">
  <div class="row w-100">
    <div class="col-8 col-md-6 col-lg-4 mx-auto">
        <div class="text-center">
            <h2 class="text-white">Done & Dusted</h2>
            <img src="../assets/largeLogo.png" alt="MainLogo" class="img-fluid mb-3 rounded">
        </div>
        <div class="col text-center" v-if="!signUp">
          <loginInputs @user-logged-in="$emit('user-logged-in')" v-if="!signUp" />
          <button class="btn btn-info" @click="signUp=!signUp">Sign up</button>
        </div>
        <signupInputs @cancelSignup="signUp=false" v-else/>
    </div>
  </div>
</div>
</template>

<script>
import loginInputs from '@/components/loginComps/loginInputs.vue';
import signupInputs from '@/components/loginComps/signupInputs.vue';
import { ref} from 'vue'

export default {
    name: 'LoginView',
    components: { loginInputs, signupInputs},
    setup(){
        const signUp = ref(false);

        return { signUp }
    }
}
</script>

<style>

</style>