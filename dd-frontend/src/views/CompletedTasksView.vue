<template>
<div class="row justify-content-around text-center">
  <label class="fw-bold fs-4 mb-3">Tasks Completed</label>
  <p class="lead">Total {{ totalCompletedUserTasks }}</p>

  <div class="row mb-5">
      <TaskCard v-for="task in completedUserTasks" :key="task.id" :task="task"
      class="col-4 gx-5 mb-5"/>
  </div>
</div>

<div class="row justify-content-around text-center">
  <label class="fw-bold fs-4 mb-3">Missed tasks</label>
  <p class="lead">Total {{ totalMissedUserTasks }}</p>

  <div class="row mb-5">
    <TaskCard v-for="task in missedUserTasks" :key="task.id" :task="task"
    class="col-4 gx-5 mb-5"/>
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
    const totalCompletedUserTasks = ref(0);
    const totalMissedUserTasks = ref(0);

    //gets all tasks, but sorts to show the 6 most recent and splices out the rest
    const getCompletedTasks = async () => {
      try{
        if(!loggedInUserId) return;
        const response = await axios.get(`http://localhost:5118/D&D/tasks/completed/${loggedInUserId}`);
        totalCompletedUserTasks.value = response.data.length;
        completedUserTasks.value = response.data
        .sort((a, b) => {
          const dateA = new Date(`${a.doneDate}T${a.doneTime}`);
          const dateB = new Date(`${b.doneDate}T${b.doneTime}`);
          return dateB - dateA;
        }).slice(0, 6);
      }
      catch(error){
        console.error(error);
      }
    }

    //gets all tasks, but sorts to show the 6 most recent and splices out the rest
    const getMissedTasks = async () => {
      try{
        if(!loggedInUserId) return;
        const response = await axios.get(`http://localhost:5118/D&D/tasks/missed/${loggedInUserId}`);
        totalMissedUserTasks.value = response.data.length;
        missedUserTasks.value = response.data
        .sort((a, b) => {
          const dateA = new Date(`${a.doDate}T${a.doTime}`);
          const dateB = new Date(`${b.doDate}T${b.doTime}`);
          return dateB - dateA;
        }).slice(0, 6);
      }
      catch(error){
        console.error(error)
      }
    }

    onMounted(getCompletedTasks)
    onMounted(getMissedTasks)
    return{ completedUserTasks, missedUserTasks, totalCompletedUserTasks, totalMissedUserTasks }
  }
}
</script>

<style>

</style>