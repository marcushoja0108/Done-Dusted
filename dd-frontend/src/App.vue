<template>
<div class="bg-dark text-white min-vh-100 d-flex flex-column">
  <div v-if="loggedInUserId">
    <myNav @log-out="handleLogout"/>
    <router-view/>
  </div>
  <div v-else>
    <router-view @user-logged-in="handleLogin"/>
  </div>
</div>
</template>

<script>
import { ref } from 'vue';
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

    //all below checks if user has a non-expired jwt and redirects either to login or homepage
    const parseJwt = (token) =>  {
      try{
        return JSON.parse(atob(token.split('.')[1]));
      }
      catch(error){
        return null;
      }
    }

    const checkExpiration = (token) => {
      const decoded = parseJwt(token);
      if(!decoded || !decoded.exp) return true;

      const currentTime = Math.floor(Date.now() / 1000);
      return decoded.exp < currentTime;
     }
    
    const checkAuth = () => {
      const token = localStorage.getItem("jwt");

      if(token && !checkExpiration(token)) {
        loggedInUserId.value = localStorage.getItem("userId");
        router.push('/');

      } else{
        localStorage.removeItem("jwt");
          router.push('/login');
          console.log('No token detected, redirected to login.');
        }
      };

      const handleLogin = () => {
        checkAuth();
      };

      const handleLogout = () => {
        loggedInUserId.value = null;
        checkAuth();
      }

      
    checkAuth();


    return { loggedInUserId, handleLogin, handleLogout }
  }
  
}
</script>


