<template>
  <div class="row m-5 justify-content-around text-center">
    <div class="col-5 col-sm-6 col-lg-4">
      <label class="fw-bold fs-4 mb-3">Tasks Completed</label>
      <p class="lead">Total {{ completedUserTasks.length }}</p>
        <div class="row g-4">
          <div v-for="task in completedUserTasks" :key="task.id">
            <TaskCard :task="task"/>
          </div>
        </div>
    </div>
    <div class="col-5 col-sm-6 col-lg-4">
      <label class="fw-bold fs-4 mb-3">Missed tasks</label>
      <p class="lead">Total {{ missedUserTasks.length }}</p>
        <div class="row g-4">
          <div v-for="task in missedUserTasks" :key="task.id">
            <TaskCard :task="task"/>
          </div>
        </div>
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
    const missedUserTasks = ref([]);

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

    const getMissedTasks = async () => {
      try{
        if(!loggedInUserId) return;
        const response = await axios.get(`http://localhost:5118/D&D/tasks/missed/${loggedInUserId}`);
        missedUserTasks.value = response.data;
      }
      catch(error){
        console.error(error)
      }
    }

    onMounted(getCompletedTasks)
    onMounted(getMissedTasks)
    return{ completedUserTasks, missedUserTasks }
  }
}
</script>

<style>

</style>