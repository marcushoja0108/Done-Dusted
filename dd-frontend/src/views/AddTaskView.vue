<template>
  <div class="row m-5 justify-content-around">
    <div class="col-7">
      <div class="row">
        <div class="col-4">
          <label class="fw-bold fs-5">Date</label>
          <input type="Date" class="form-control" v-model="taskToAdd.date">
        </div>
        <div class="col-4">
          <label class="fw-bold fs-5">Time</label>
          <input type="Time" class="form-control" v-model="taskToAdd.time">
        </div>
        <div class="col-4">
          <label class="fw-bold fs-5">Repeats</label>
            <select class="form-select" aria-label="Repeats select" v-model="taskToAdd.repeats">
              <option value="None">None</option>
              <option value="Daily">Daily</option>
              <option value="Weekly">Weekly</option>
              <option value="Monthly">Monthly</option>
            </select>
        </div>
      </div>
      <!-- end of row -->
       <div class="row mt-4">
        <div class="col">
          <input type="text" class="form-control fs-5 fw-bold" placeholder="Title"
          v-model="taskToAdd.title">
        </div>
       </div>
       <!-- end of row -->
       <div class="row">
        <div class="col">
          <div class="form-floating my-4">
            <textarea class="form-control" style="height: 200px"
            v-model="taskToAdd.summary"></textarea>
            <label class="fw-bold fs-5">Summary</label>
          </div>
        </div>
       </div>
       <!-- end of row -->
       <div class="row">
         <div class="col">
            <div v-show="!checks.assignedUsers" class="alert alert-danger">Select a user!</div>
           <button class="btn btn-success fw-bold" @click="addTask"><i class="bi bi-plus-circle">
           </i> Add to Tasks</button>
         </div>
       </div>
    </div>
    <!-- end of left -->
     <div class="col-4 text-center">
       <UserSelect v-model:assignedUsers="taskToAdd.assignedUsers"/>
     </div>
  </div>
</template>

<script>
import UserSelect from '@/components/addComps/UserSelect.vue';
import axios from 'axios'
import { ref } from 'vue'

export default {
  name: 'AddTaskView',
  props: [],
  emits: [''],
  components: {UserSelect},
  setup() {
    const taskToAdd = ref({
      title: '',
      date: '',
      time: '',
      repeats: '',
      summary: '',
      assignedUsers: [],
    })

    const checks = ref({
      assignedUsers: true,
    })

    const addTask = async () => {
      // Denne if en fungerer ikke som den skal
      if(!taskToAdd.value.assignedUsers || taskToAdd.value.assignedUsers == []){
        checks.value.assignedUsers = false
        return;
      }
      try {
        const task = {
          title: taskToAdd.value.title,
          doDate: taskToAdd.value.date,
          doTime: taskToAdd.value.time,
          repeats: taskToAdd.value.repeats,
          summary: taskToAdd.value.summary
        }
        const response = await axios.post(`http://localhost:5118/D&D/tasks`, task)

        if(response.data){
          const taskId = response.data;
          await Promise.all(
            taskToAdd.value.assignedUsers.map(user => 
              axios.post(`http://localhost:5118/D&D/tasks/${taskId}/user/${user.id}`),
            )
          )
          console.log(response.data)
        }

  // må bruke promise siden await ikke fungerer i forEach
      }
      catch(error) {
        console.error("Error adding task", error)
      }
    }

    return {taskToAdd, addTask, checks }
  }

}
</script>