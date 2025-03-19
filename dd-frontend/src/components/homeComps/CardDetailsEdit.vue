<template>
  <div class="modal-body">
        <div class="mb-3">
            <div class="row">
                <div class="col-6">
                    <label class="fw-bold">Date </label>
                    <input v-model="formattedDate" class="form-control" type="Date"> 
                </div>
                <div class="col-6">
                    <label class="fw-bold">Time </label>
                    <input v-model="updatedTask.doTime" class="form-control" type="Time">                    
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

        <UserSelect v-model:assignedUsers="updatedTask.assignedUsers" />

        <div v-show="!checks.assignedUsers" class="alert alert-danger mt-3">
            Task must have at least one selected user!</div>
        <div v-show="!checks.dateAndTime" class="alert alert-danger mt-3">
            Task must have a valid date and time!</div>
        
        <div class="d-flex justify-content-center">
            <button class="btn btn-success my-3 mx-1" @click="saveChanges">Save changes</button>
            <button class="btn btn-danger my-3 mx-1" @click="cancelChanges">Cancel changes</button>
        </div>
    </div>
</template>

<script>
import { ref, computed } from 'vue'
import UserSelect from '../UserSelect.vue';
import axios from 'axios'

export default {
    name: 'CardDetailsEdit',
    components: {UserSelect},
    props: {
        task: Object,
    },
    setup(props, { emit }) {
        //sets default values in inputs, also maps users to just their id's
        const updatedTask = ref({
            id: props.task.id,
            doDate: props.task.doDate,
            doTime: props.task.doTime,
            title: props.task.title,
            repeats: props.task.repeats,
            summary: props.task.summary,
            assignedUsers: props.task.assignedUsers 
            ? props.task.assignedUsers.map(user => user.id) : []
        });

        const checks = ref({
            assignedUsers: true,
            dateAndTime: true,
        })

        const saveChanges = async () => {
            if(updatedTask.value.assignedUsers.length === 0){
                checks.value.assignedUsers = false
                return;
            }
            else{checks.value.assignedUsers = true}

            if(updatedTask.value.doDate === '' || updatedTask.value.doTime === ''){
                checks.value.dateAndTime = false;
                return;
            }
            else{checks.value.dateAndTime = true}

            try{
                if(!updatedTask.value) return;
                if(!updatedTask.value.assignedUsers){
                    console.log('users required');
                    return;
                } 
                
                const updatedTaskData = {
                    doDate: updatedTask.value.doDate || null,
                    doTime: updatedTask.value.doTime || null,
                    title: props.task.title,
                    repeats: updatedTask.value.repeats || null,
                    summary: updatedTask.value.summary || null,
                }

                await axios.put(`http://localhost:5118/D&D/tasks/${props.task.id}`, updatedTaskData);

                // update users
                const pastUsers = props.task.assignedUsers.map(user => user.id);
                const newUsers = updatedTask.value.assignedUsers;

                const usersToRemove = pastUsers.filter(userId => !newUsers.includes(userId));
                const usersToAdd = newUsers.filter(userId => !pastUsers.includes(userId));

                await Promise.all(
                    usersToRemove.map(userId => 
                    axios.delete(`http://localhost:5118/D&D/tasks/${updatedTask.value.id}/user/${userId}`)
                    )
                ),
                await Promise.all(
                    usersToAdd.map(userId =>
                    axios.post(`http://localhost:5118/D&D/tasks/${updatedTask.value.id}/user/${userId}`)
                    )
                )

                //This emits the updated task to the taskUpdated function in parent. Also maps assignedusers id's to objects
                emit('task-updated', {
                    ...updatedTask.value,
                    assignedUsers: newUsers.map(id => ({ id }))
                });

            }
            catch(error){
                console.log(error);
            }
        };

        const formattedDate = computed({
            get: () => updatedTask.value.doDate ? updatedTask.value.doDate.split('T')[0] : '',
            set: (newDate) => updatedTask.value.doDate = newDate
        });

        const cancelChanges = () => {
            updatedTask.value = { ...props.task };
            emit('cancel');
        };
        
        return { updatedTask, checks, saveChanges, cancelChanges, formattedDate };
    }

}
</script>

<style>

</style>