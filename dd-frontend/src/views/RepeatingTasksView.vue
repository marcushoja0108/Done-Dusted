<template>
<div class="text-center">
  <div class="row m-5">
    <p class="fs-2">Daily tasks</p>
    <div v-if="dailyTasks.length > 0">
      <RepeatList :repeats="dailyTasks"/>
    </div>
    <div v-else>No current daily tasks</div>
  </div>
  <div class="row m-5">
    <p class="fs-2">Weekly tasks</p>
    <div v-if="weeklyTasks.length > 0">
      <RepeatList :repeats="weeklyTasks"/>
    </div>
    <div v-else>
      No current weekly tasks
    </div>
  </div>
  <div class="row m-5">
    <p class="fs-2">Monthly tasks</p>
    <div v-if="monthlyTasks.length > 0">
      <RepeatList :repeats="monthlyTasks"/>
    </div>
    <div v-else>
      No current monthly tasks
    </div>
  </div>
</div>
</template>

<script>
import { ref, onMounted } from 'vue'
import RepeatList from '@/components/RepeatingComps/RepeatList.vue';
import axios from 'axios'

export default {
  name: 'RepeatingTasksView',
  components: { RepeatList },

  setup(){
    const loggedInUserId = localStorage.getItem("userId");
    const dailyTasks = ref([]);
    const weeklyTasks = ref([]);
    const monthlyTasks = ref([]);

    const getTasks = async () => 
    {
    if(!loggedInUserId) return;
    try{
      const response = await axios.get(`http://localhost:5118/D&D/tasks/upcoming/${loggedInUserId}`);
      const allUpcomingTasks = response.data;
        dailyTasks.value = allUpcomingTasks.filter(task => task.repeats == "Daily");
        weeklyTasks.value = allUpcomingTasks.filter(task => task.repeats == "Weekly");
        monthlyTasks.value = allUpcomingTasks.filter(task => task.repeats == "Monthly");
      }
    catch(error){
      console.error(error);
    }
    }

    onMounted(getTasks)

    return { dailyTasks, weeklyTasks, monthlyTasks }
  }
}
</script>

<style>

</style>