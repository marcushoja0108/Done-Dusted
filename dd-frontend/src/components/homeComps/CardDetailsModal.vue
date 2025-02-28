<template>
    <div v-if="showModal" class="modal fade show" tabindex="-1" 
    aria-labelledby="CardDetailsModalLabel" aria-hidden="true" style="display: block">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="CardDetailsModalLabel">{{ task.title }}</h5>
                    <button type="button" class="btn-close btn-light" 
                    @click="closeModal" aria-label="Close"></button>
                </div>
                <div v-if="!editMode" class="modal-body">
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
                            <li v-for="user in taskUsers" :key="user.id" class="list-group-item">
                                {{ user.userName }}
                            </li>
                        </ul>
                    </div>
                    <div class="d-flex justify-content-between mt-3 fw-bold">
                        <button class="btn btn-primary" @click="editMode = !editMode">Edit task</button>
                        <button class="btn btn-success">Mark as Done</button>
                    </div>
                </div>
                
                <!-- Edit Mode -->
                <div v-else class="modal-body">
                    <div class="mb-3">
                        <div class="row">
                            <div class="col-6">
                                <label class="fw-bold">Date </label>
                                <input v-model="task.doDate.split('T')[0]" class="form-control" type="Date"> 
                            </div>
                            <div class="col-6">
                                <label class="fw-bold">Time </label>
                                <input v-model="task.doTime" class="form-control" type="Time">                    
                            </div>
                        </div>
                    </div>
                    <div class="mb-3">
                        <label class="fw-bold">Repeats</label>
                            <select v-model="task.repeats" class="form-select" aria-label="Repeats select">
                                <option value="None">None</option>
                                <option value="Daily">Daily</option>
                                <option value="Weekly">Weekly</option>
                                <option value="Monthly">Monthly</option>
                            </select>
                    </div>
                    <div class="mb-3">
                        <label>Summary</label>
                        <textarea class="form-control" v-model="task.summary"></textarea>
                    </div>
                    <div class="mb-3">
                        <label>Assign users</label>
                        <ul class="list-group">
                            <li v-for="user in allUsers" :key="user.id" class="list-group-item">
                                <input class="form-check-input me-1"  type="checkbox" :value="user.id" 
                                :id="'userCheckbox' + user.id" v-model="updatedTask.assignedUsers">
                                <label class="form-check-label stretched-link" 
                                :for="'userCheckbox' + user.id">{{ user.userName }}</label>
                            </li>
                        </ul>
                    </div>
                    <div class="d-flex justify-content-center">
                        <button class="btn btn-success my-3 mx-1" @click="saveChanges">Save changes</button>
                        <button class="btn btn-danger my-3 mx-1" @click="editMode = false">Cancel changes</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div v-if="showModal" class="modal-backdrop fade show"></div>
</template>

<script>
import {onMounted, ref, watch, computed} from 'vue'
import axios from 'axios'

export default {
    name: 'CardDetailsModal',
    props: ['task', 'showModal'],
    emits: ['close', 'task-updated'],
    setup(props, {emit}){
        console.log(props.task)
        if(!props.task){
            console.error("No task passed")
            return
        }
        const editMode = ref(false);
        const taskUsers = ref([]);
        const allUsers = ref([]);

        const updatedTask = ref({
            ...props.task,
            assignedUsers: [],
        });

        const shortDate = computed(() => {
            if(!props.task.doDate) return "";

            return new Date(props.task.doDate).toLocaleDateString(undefined, {
                year: "numeric",
                month: "short",
                day: "numeric",
            });
        });
        
        const shortTime = computed(() => {
            if(!props.task.doTime) return "";

            return new Date(`2000-01-01T${props.task.doTime}`).toLocaleTimeString(undefined, {
                hour: "2-digit",
                minute: "2-digit",
                hour12: false,
            });
        });

        const saveChanges = async () => {
            try{
                if(!updatedTask.value) return;
                if(!updatedTask.value.assignedUsers){
                    console.log('users required');
                    return;} 
                
                const updatedTaskData = {
                    doDate: updatedTask.value.doDate || null,
                    doTime: updatedTask.value.doTime || null,
                    title: props.task.title,
                    repeats: updatedTask.value.repeats || null,
                    summary: updatedTask.value.summary || null,
                }

                await axios.put(`http://localhost:5118/D&D/tasks/${props.task.id}`, updatedTaskData);
                console.log('Task updated!')

                emit('task-updated', updatedTask.value);

                editMode.value = false;
            }
            catch(error){
                console.log(error)
            }
        }

        watch(() => props.task, (newTask) => {
            if(newTask) {
                updatedTask.value = {
                    doDate: newTask.doDate ? newTask.doDate.split('T')[0] : '',
                    doTime: newTask.doTime,
                    repeats: newTask.repeats,
                    summary: newTask.summary,
                    // mapper bare til bruker id, hvis ikke det er brukere blir det tomt array
                    assignedUsers: newTask.assignedUsers ? newTask.assignedUsers.map(user => user.id) : [],
                };
            }
        }, {immediate: true});

        const getTaskUsers = async () => {
                try{
                    if(!props.task) return;
                const response = await axios.get(`http://localhost:5118/D&D/users/${props.task.id}`);
                taskUsers.value = response.data
                updatedTask.value.assignedUsers = response.data.map(user => user.id);
            }
            catch(error){
                console.error(error)
            }
        }
        const getAllUsers = async () => {
            try{
                const response = await axios.get(`http://localhost:5118/D&D/users`);
                allUsers.value = response.data
            }
            catch(error){
                console.log(error)
            }
        }

        const closeModal = () => {
            emit('close');
        }
        
        onMounted(() => {
            getTaskUsers();
            getAllUsers();
        })

        return {closeModal, task: props.task, shortDate, shortTime, taskUsers, editMode, allUsers, updatedTask, saveChanges}
    }
}
</script>
