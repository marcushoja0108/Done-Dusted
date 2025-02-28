<template>
  <label class="fw-bold fs-5">Assigned users</label>
        <div class="text-start">
            <ul class="list-group">
                <li class="list-group-item lead">
                    <input type="text" class="form-control" v-model="userFilter" @input="filterUsers"
                    placeholder="Search for user">
                </li>
                <li v-for="user in filteredUsers" :key="user.id" class="list-group-item">
                    <input type="checkbox" class="form-check-input me-3" :id="'userCheckbox' + user.id">
                    <label :for="'userCheckbox' + user.id" class="form-check-label"> {{ user.userName }}</label>
                </li>
            </ul>
        </div>

</template>

<script>
import {onMounted, ref} from 'vue'
import axios from 'axios'

export default {
    name: 'usersSelect',
    setup(){

        const allUsers = ref([])
        const userFilter = ref()
        const filteredUsers = ref([])

        const getAllUsers = async () => {
            try{
                const response = await axios.get(`http://localhost:5118/D&D/users`);
                allUsers.value = response.data;
                filteredUsers.value = allUsers.value;
            }
            catch(error){
                console.error(error)
            }
        }

        const filterUsers = () => {
            filteredUsers.value = allUsers.value.filter(user => 
            user.userName.toLowerCase().includes(userFilter.value.toLowerCase()));
        }

        onMounted(getAllUsers);

        return { allUsers, userFilter, filterUsers, filteredUsers }
    }
}
</script>

<style>

</style>