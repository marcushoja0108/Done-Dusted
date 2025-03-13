<template>
  <div>
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
</template>

<script>
export default {
    name: 'signupInputs',

    setup(){
        const signUpInput = ref({
        userName: '',
        password: '',
        secondPassword: '',
        });

        return {  }
    }
}
</script>

<style>

</style>