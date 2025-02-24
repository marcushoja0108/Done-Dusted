<template>
  <div class="card text-center">
    <div class="card-header">
        Upcoming task
    </div>
    <div class="card-body">
        <h5 class="card-title">{{ task.title }}</h5>
        <p class="card-text">Date: {{ task.doDate }}</p>
        <p class="card-text">Time: {{ task.doTime }}</p>
    </div>
    <div class="card-footer text-body-secondary">
      <a href="#" class="btn btn-primary" @click="toggleModal">Details</a>
  </div>
</div>

<CardDetailsModal v-if="showModal" :task="task" :showModal="toggleModal" @close="toggleModal"/>
</template>

<script>
import CardDetailsModal from './CardDetailsModal.vue';
import {ref} from 'vue'
export default {
    props: ['task'],
    components: {CardDetailsModal},
    setup(){
        const showModal = ref(false);
        
        const toggleModal = () => {
            showModal.value = !showModal.value
            console.log(showModal.value)
        }

        return {showModal, toggleModal}
    },
}
</script>

<style>

</style>