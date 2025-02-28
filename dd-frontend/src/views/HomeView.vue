<template>
    <div class="row row-cols-md-4 g-5 justify-content-center">
      
        <div v-for="task in tasks" :key="task.id"
        class="col-8 col-lg-5 col-md-8 col-sm-8 m-2 my-5">
          <TaskCard :task="task"/>
        </div>
      
    </div>
</template>

<script>
import TaskCard from '@/components/homeComps/TaskCard.vue';
import {onMounted, ref, inject} from 'vue';
import axios from 'axios';



export default {
  name: 'HomeView',
  components: {TaskCard},
  
  setup(){
    const loggedInUserId = inject('loggedInUserId')
    const tasks= ref([]);

    const getTasks = async () => {
      try {
        if(!loggedInUserId) return;
        const response = await axios.get(`http://localhost:5118/D&D/tasks/${loggedInUserId}`)
        tasks.value = response.data;
      }
      catch(error){
        console.error(error)
      }
    }
    onMounted(getTasks);
    return { tasks, getTasks }
  }
}
</script>
