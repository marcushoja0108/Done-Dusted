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
          <!-- error messages -->
          <div v-show="!checks.assignedUsers" class="alert alert-danger">Task must have at least one selected user!</div>
          <div v-show="checks.titleEmpty" class="alert alert-danger">Task must have a title!</div>
          <div v-show="!checks.dateAndTime" class="alert alert-danger">Task must have a valid date and time!</div>
          <div v-show="errorMessage" class="alert alert-warning" role="alert">{{ errorMessage }}</div>
          <div class="position-relative">
            <button class="btn btn-success fw-bold" @click="addTask"><i class="bi bi-plus-circle">
            </i> Add to Tasks</button>

            <!-- toast -->
            <div ref="taskToast" class="toast align-items-center text-bg-success mt-3" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                  <div class="toast-body fs-5">
                    Task added!
                  </div>
                  <button type="button" class="btn-close me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
              </div>
          </div>
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
import UserSelect from '@/components/UserSelect.vue';
import axios from 'axios'
import { ref } from 'vue'
import { Toast } from 'bootstrap'

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
      repeats: 'None',
      summary: '',
      assignedUsers: [],
    })

    const checks = ref({
      assignedUsers: true,
      titleEmpty: false,
      dateAndTime: true
    })

    const taskToast = ref(null)

    const errorMessage = null;

    const showToast = () => {
      if(taskToast.value) {
        const toastInstance = new Toast(taskToast.value);
        toastInstance.show();
      }
    };

    const addTask = async () => {
      if(taskToAdd.value.assignedUsers.length === 0){
        checks.value.assignedUsers = false
        return;
      }
      else{checks.value.assignedUsers = true}
      
      if(taskToAdd.value.title === ''){
        checks.value.titleEmpty = true
        return;
      }
      else{checks.value.titleEmpty = false}

      if(taskToAdd.value.date === '' || taskToAdd.value.time === ''){
        checks.value.dateAndTime = false
        return;
      }
      else{
        checks.value.dateAndTime = true
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
          const taskId = response.data.id || response.data;
          console.log("TaskId: ", taskId)
          await Promise.all(
            taskToAdd.value.assignedUsers.map(user => 
              axios.post(`http://localhost:5118/D&D/tasks/${taskId}/user/${user.id}`),
            )
          )
          console.log(response.data)
        }
        showToast()
        resetValues()
  // må bruke promise siden await ikke fungerer i forEach
      }
      catch(error) {
        console.error("Error adding task", error)
        errorMessage = "Error adding task, please try again later.";

      }
    }
    const resetValues = () => {
      taskToAdd.value = {
          title: '',
          date: '',
          time: '',
          repeats: 'None',
          summary: '',
          assignedUsers: [],
        }

        checks.value = {
          assignedUsers: true,
          titleEmpty: false,
          dateAndTime: true
        }
    }

    return {taskToAdd, addTask, checks, taskToast, showToast, errorMessage }
  }
}
</script>