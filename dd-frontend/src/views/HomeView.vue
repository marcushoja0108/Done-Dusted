<template>
    <div v-if="upcomingTasks.length > 0" class="row row-cols-md-4 g-5 justify-content-center">
        <div v-for="task in upcomingTasks" :key="task.id"
        class="col-8 col-lg-5 col-md-8 col-sm-8 m-2 my-5">
          <TaskCard :task="task"/>
        </div>
    </div>
    <!-- <div v-else-if="upcomingTasks.length == 0">No tasks to show</div> -->
    <div v-else>Loading tasks</div>
</template>

<script>
import TaskCard from '@/components/TaskCard.vue';
import {onMounted, ref} from 'vue';
import axios from 'axios';



export default {
  name: 'HomeView',
  components: {TaskCard},
  
  setup(){
    const loggedInUserId = localStorage.getItem("userId");
    const upcomingTasks= ref([]);

    const getTasks = async () => {
      if(!loggedInUserId) return;
      try {
        if(!loggedInUserId) return;

        const response = await axios.get(`http://localhost:5118/D&D/tasks/${loggedInUserId}`)
        const allUserTasks = response.data;
        
        upcomingTasks.value = allUserTasks.filter(task => !task.done)
        await getMissedTasks();
        upcomingTasks.value = upcomingTasks.value.filter(task => !task.missed)
        console.log(upcomingTasks)
      }
      catch(error){
        console.error(error)
      }
    }

    const getMissedTasks = async () => {
      const today = Date.now();
      const missedTasks = upcomingTasks.value.filter(task => {
        const taskTime = new Date(task.doDate).getTime();
        return taskTime < today;
    });
      console.log(missedTasks);
      await Promise.all(
        missedTasks.map(task => 
          axios.put(`http://localhost:5118/D&D/tasks/missed/${task.id}`)
        )
      )
    }
    onMounted(getTasks);
    return { upcomingTasks, getTasks }
  }
}
</script>
