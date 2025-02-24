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

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #4842b9;
}
</style>
