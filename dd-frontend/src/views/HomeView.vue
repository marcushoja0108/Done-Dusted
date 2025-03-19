<template>
    <div v-if="upcomingTasks.length > 0" class="row row-cols-md-4 g-5 justify-content-center">
        <div v-for="task in upcomingTasks" :key="task.id"
        class="col-8 col-lg-5 col-md-8 col-sm-8 m-2 my-5">
          <TaskCard v-if="!task.done" :task="task"/>
        </div>
    </div>
    <!-- <div v-else-if="upcomingTasks.length == 0">No tasks to show</div> -->
    <div v-else class="text-center">{{ errorMessage }}</div>
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
    const errorMessage = ref();

    const getTasks = async () => {
      if(!loggedInUserId) return;
      try {
        if(!loggedInUserId) return;
        const response = await axios.get(`http://localhost:5118/D&D/tasks/upcoming/${loggedInUserId}`);
        upcomingTasks.value = response.data;

        await getMissedTasks();
        upcomingTasks.value = upcomingTasks.value.filter(task => !task.missed);
        if(upcomingTasks.value.length == 0) errorMessage.value = "No tasks to show. Add one!";
      }
      catch(error){
        console.error(error)
      }
    }

    const getMissedTasks = async () => {
      const today = Date.now();
      const missedTasks = upcomingTasks.value.filter(task => {
        const taskTime = new Date(`${task.doDate} ${task.doTime}`);
        return taskTime < today;
    });
      await Promise.all(
        missedTasks.map(task => 
          axios.put(`http://localhost:5118/D&D/tasks/missed/${task.id}`)
        )
      )
    }
    
    onMounted(getTasks);
    return { upcomingTasks, getTasks, errorMessage }
  }
}
</script>
