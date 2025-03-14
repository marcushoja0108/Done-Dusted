<template>
  <div class="row m-5 justify-content-around text-center">
    <div class="col">
      <label class="fw-bold fs-4 mb-2">Tasks Completed</label>
      <p class="lead">Total {{ completedUserTasks.length }}</p>
        <div class="row g-4">
          <div v-for="task in completedUserTasks" :key="task.id">
            <TaskCard :task="task"/>
          </div>
        </div>
    </div>
    <div class="col">
      <label class="fw-bold fs-4">Missed tasks</label>
    </div>
    <div class="col">
      <label class="fw-bold fs-4">Assigned tasks</label>
    </div>
  </div>
</template>

<script>
import { ref, onMounted} from 'vue'
import TaskCard from '@/components/TaskCard.vue'
import axios from 'axios'

export default {
  name: 'CompletedTasks',
  components: { TaskCard },
  setup(){
    const loggedInUserId = localStorage.getItem("userId");
    const completedUserTasks = ref([]);

    const getCompletedTasks = async () => {
      try{
        if(!loggedInUserId) return;
        const response = await axios.get(`http://localhost:5118/D&D/tasks/completed/${loggedInUserId}`);
        completedUserTasks.value = response.data;
      }
      catch(error){
        console.error(error)
      }
    }

    onMounted(getCompletedTasks)
    return{ completedUserTasks }
  }
}
</script>

<style>

</style>