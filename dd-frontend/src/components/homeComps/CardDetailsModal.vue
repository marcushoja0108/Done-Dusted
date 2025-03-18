<template>
    <div v-if="showModal" class="modal fade show" tabindex="-1" 
    aria-labelledby="CardDetailsModalLabel" style="display: block">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div v-if="!task.done" class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="CardDetailsModalLabel">{{ task.title }}</h5>
                    <button type="button" class="btn-close btn-light" 
                    @click="closeModal" aria-label="Close"></button>
                </div>
                <div v-else class="modal-header bg-success text-white">
                    <h5 class="modal-title" id="CardDetailsModalLabel">{{ task.title }}</h5>
                    <button type="button" class="btn-close btn-light" 
                    @click="closeModal" aria-label="Close"></button>
                </div>
                
                <CardDetailsView v-if="!editMode" 
                :task="task" 
                @edit="editMode = true"
                @showToast="showToastMessage"/>

                <CardDetailsEdit v-if="editMode" 
                :task="task" 
                @task-updated="taskUpdated" 
                @cancel="editMode = false"/>
            </div>
        </div>
    </div>
    <div v-if="showModal" class="modal-backdrop fade show"></div>

    <div class="toast-container position-fixed bottom-0 end-0 p-3" 
    style="z-index: 1050;" id="toast-container">
        <div v-if="toastMessage" class="toast show bg-success text-white p-4 w-100 p-4 fs-5" 
        role="alert" 
        aria-live="assertive" aria-atomic="true">
            <div class="toast-body fw-bold">
                {{ toastMessage }}
            </div>
        </div>
    </div>
</template>

<script>
import {onMounted, ref} from 'vue'
import axios from 'axios'
import CardDetailsView from './CardDetailsView.vue'
import CardDetailsEdit from './CardDetailsEdit.vue'

export default {
    name: 'CardDetailsModal',
    components: {
        CardDetailsView, 
        CardDetailsEdit
    },
    props: ['task', 'showModal'],
    emits: ['close', 'task-updated'],
    setup(props, {emit}){
        
        const editMode = ref(false);
        const toastMessage = ref(null);

        
        const getTaskUsers = async () => {
                try{
                    if(!props.task) return;
                const response = await axios.get(`http://localhost:5118/D&D/users/${props.task.id}`);
                props.task.assignedUsers = response.data
            }
            catch(error){
                console.error('Error getting task users', error);
            }
        }

        const closeModal = () => {
            emit('close');
        }

        const taskUpdated = (updatedTask) => {
            Object.assign(props.task, updatedTask);
            emit('task-updated', updatedTask);
            editMode.value = false;
        }

        const showToastMessage = (message) => {
            toastMessage.value = message;
                setTimeout(() => {
                    toastMessage.value = null;
                }, 3000);
        }
        
        onMounted(() => {
            getTaskUsers();
        })

        return { editMode, closeModal, taskUpdated, showToastMessage, toastMessage };
    }
}
</script>
