<template>
  <div v-if="loggedInUserId">
    <myNav/>
    <router-view/>
  </div>
  <div v-else>
    <router-view @user-logged-in="handleLogin"/>
  </div>
</template>

<script>
import { ref, } from 'vue';
import myNav from './components/myNav.vue';
import { useRouter } from 'vue-router';

export default {
  name: 'App',
  components: {
    myNav
  },
  setup(){
    const loggedInUserId = ref(null);
    const router = useRouter();

    const parseJwt = (token) =>  {
      try{
        return JSON.parse(atob(token.split('.')[1]));
      }
      catch(error){
        return null;
      }
    }
    
    const checkAuth = () => {
      const token = localStorage.getItem("jwt");

      if(token) {
        const userData = parseJwt(token)
        loggedInUserId.value = userData?.nameidentifier || null;

      } else{
          router.push('/login');
          console.log('No token detected, redirected to login.');
        }
      };

      const handleLogin = () => {
        checkAuth();
      };

      // kanskje ikke nødvendig
      // watch(() => localStorage.getItem("jwt"), () => {
      //   checkAuth
      // });
      
    checkAuth();


    return { loggedInUserId, handleLogin }
  }
  
}
</script>


