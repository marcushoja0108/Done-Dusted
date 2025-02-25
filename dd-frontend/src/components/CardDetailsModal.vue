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
                        <p><strong>Summary </strong> {{ task.summary }}</p>
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
                
                <div v-else class="modal-body">
                    <div class="mb-3">
                        <label for="taskSummary" class="form-label">Task summary</label>
                        <textarea id="taskSummary" class="form-control"></textarea>
                    </div>
                    <div class="d-flex justify-content-center">
                        <button class="btn btn-success my-3" @click="editMode = !editMode">Save changes</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div v-if="showModal" class="modal-backdrop fade show"></div>
</template>

<script>
import {onMounted, ref} from 'vue'
import axios from 'axios'

export default {
    props: ['task', 'showModal', 'shortDate', 'shortTime'],
    emits: ['close'],
    setup(props, {emit}){
        const editMode = ref(false);
        const taskUsers = ref([]);
        const getTaskUsers = async () => {
                try{
                    if(!props.task) return;
                const response = await axios.get(`http://localhost:5118/D&D/users/${props.task.id}`);
                taskUsers.value = response.data
            }
            catch(error){
                console.error(error)
            }
        }

        const closeModal = () => {
            emit('close');
        }

        onMounted(getTaskUsers)
        return {closeModal, taskUsers, editMode}
    }
}
</script>
