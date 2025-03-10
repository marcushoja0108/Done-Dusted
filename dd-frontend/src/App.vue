<template>
  <div v-if="loggedInUserId">
    <myNav/>
  </div>
  <router-view/>
</template>

<script>
import {provide, ref, onMounted} from 'vue';
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

    
    onMounted(() => {
      const pHUserId = 1;

      if(pHUserId){
        loggedInUserId.value = pHUserId;
        provide('loggedInUserId', loggedInUserId.value);
      }
      else{
        router.push('/login');
      }
      console.log(loggedInUserId.value)
    });

    return { loggedInUserId }
  }
  
}
</script>


