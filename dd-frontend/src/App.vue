<template>
  
  <myNav/>
  <router-view/>
</template>

<script>
import {provide} from 'vue';
import myNav from './components/myNav.vue';

export default {
  name: 'App',
  components: {
    myNav
  },
  setup(){
    const loggedInUserId = 1;
    provide('loggedInUserId', loggedInUserId);

    return { loggedInUserId }
  }
  
}
</script>


