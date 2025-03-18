<template>
  <div class="modal-body">
        <div class="mb-3">
            <div class="row">
                <div class="col-6">
                    <p><strong>Date: </strong> {{ shortDate }}</p> 
                </div>
                <div class="col-6">
                    <p><strong>Time: </strong> {{ shortTime }}</p>                    
                </div>
            </div>
        </div>
        <div class="mb-3">
            <p><strong>Repeats: </strong> {{ task.repeats }}</p>
        </div>
        <div class="mb-3">
            <p><strong>Summary: </strong> {{ task.summary }}</p>
        </div>
        <div class="mb-3">
            <ul class="list-group">
                <li class="list-group-item lead">Assigned users</li>
                <li v-for="user in assignedUsers" :key="user.id" class="list-group-item">
                    {{ user.userName }}
                </li>
            </ul>
        </div>
        <div v-if="!task.done && !task.missed" class="d-flex justify-content-between mt-3 fw-bold">
            <button class="btn btn-primary" @click="$emit('edit')"> 
                <i class="bi bi-pencil-square"></i> 
                Edit task</button>
            <button class="btn btn-success" @click="markTaskAsDone"> 
                <i class="bi bi-check-circle"></i> 
                Mark as Done</button>
        </div>
    </div>
</template>

<script>
import { ref, onMounted, computed} from 'vue'
import axios from 'axios'

export default {
    name: 'CardDetailsView',
    props: {
        task: Object,
        },
    emits: ['edit', 'closeModal', 'showToast'],
    setup(props, { emit }){
        const assignedUsers = ref([])

        const getAssignedUsers = async () => {
            if(!props.task.id) return;
            try{
                const response = await axios.get(`http://localhost:5118/D&D/users/${props.task.id}`);
                assignedUsers.value = response.data;
            }
            catch(error){
                console.log("Error getting assigned users", error)
            }
        }
        onMounted(getAssignedUsers)
        
        const shortDate = computed(() => {
            if(!props.task.doDate) return "";
            let dateToChange = props.task.doDate;

            if(props.task.doneDate) dateToChange = props.task.doneDate;
            
            return new Date(dateToChange).toLocaleDateString(undefined, {
                year: "numeric",
                month: "short",
                day: "numeric",
            });
        })

        const shortTime = computed(() => {
            if(!props.task.doTime) return "";
            let timeToChange = props.task.doTime;

            if(props.task.doneTime) timeToChange = props.task.doneTime;

            return new Date(`2000-01-01T${timeToChange}`).toLocaleTimeString(undefined, {
                hour: "2-digit",
                minute: "2-digit",
                hour12: false,
            });
        })

        const markTaskAsDone = async () => {
            if(props.task.done) return;
            try{
                await axios.put(`http://localhost:5118/D&D/tasks/done/${props.task.id}`);
                props.task.done = true;
                emit('showToast', 'Task marked as done!');
            }
            catch(error){
                console.error("Error marking task as done", error)
            }

        }

        return { assignedUsers, shortDate, shortTime, markTaskAsDone }
    }
}
</script>

<style>

</style>