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
                    <div class="d-flex justify-content-center-mt-3">
                        <button class="btn btn-info" @click="editMode = !editMode">Edit task</button>
                    </div>
                </div>
                
                <!-- Edit Mode -->
                <div v-else class="modal-body">
                    <div class="mb-3">
                        <div class="row">
                            <div class="col-6">
                                <label class="fw-bold">Date </label>
                                <input v-model="updatedTask.date" class="form-control" type="Date"> 
                            </div>
                            <div class="col-6">
                                <label class="fw-bold">Time </label>
                                <input v-model="updatedTask.time" class="form-control" type="Time">                    
                            </div>
                        </div>
                    </div>
                    <div class="mb-3">
                        <label class="fw-bold">Repeats</label>
                            <select v-model="updatedTask.repeats" class="form-select" aria-label="Repeats select">
                                <option value="None">None</option>
                                <option value="Daily">Daily</option>
                                <option value="Weekly">Weekly</option>
                                <option value="Monthly">Monthly</option>
                            </select>
                    </div>
                    <div class="mb-3">
                        <label>Summary</label>
                        <textarea class="form-control" v-model="updatedTask.summary"></textarea>
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
import {onMounted, ref, watch} from 'vue'
import axios from 'axios'

export default {
    props: ['task', 'showModal', 'shortDate', 'shortTime'],
    emits: ['close'],
    setup(props, {emit}){
        const editMode = ref(false);
        const taskUsers = ref([]);
        const allUsers = ref([]);

        const updatedTask = ref({
            date: '',
            time: '',
            repeats: '',
            summary: '',
            assignedUsers: [],
        })

        const saveChanges = async () => {
            try{
                if(!props.task) return;
                if(!updatedTask.value.assignedUsers){
                    console.log('users required');
                    return} 
                
                const updatedTaskData = {
                    doDate: updatedTask.value.date || null,
                    doTime: updatedTask.value.time || null,
                    title: props.task.title,
                    repeats: updatedTask.value.repeats || null,
                    summary: updatedTask.value.summary || null,
                }
                const response = await axios.put(`http://localhost:5118/D&D/tasks/${props.task.id}`, updatedTaskData);
                console.log('Task updated!')
                editMode.value = false;
            }
            catch(error){
                console.log(error)
            }
        }
        console.log(props.task)

        watch(() => props.task, (newTask) => {
            if(newTask) {
                updatedTask.value = {
                    date: newTask.doDate ? newTask.doDate.split('T')[0] : '',
                    time: newTask.doTime,
                    repeats: newTask.repeats,
                    summary: newTask.summary,
                    // mapper bare til bruker id, hvs ikke det er brukere blir det tomt array
                    assignedUsers: newTask.assignedUsers ? newTask.assignedUsers.map(user => user.id) : []
                }
            }
            console.log(newTask.doDate)
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

        onMounted(getTaskUsers)
        onMounted(getAllUsers)
        return {closeModal, taskUsers, editMode, allUsers, updatedTask, saveChanges}
    }
}
</script>
