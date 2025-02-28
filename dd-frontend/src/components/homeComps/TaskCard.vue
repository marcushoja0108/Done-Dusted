<template>
  <div class="card text-center">
    <div class="card-header">
        Upcoming task
    </div>
    <div class="card-body">
        <h5 class="card-title">{{ task.title }}</h5>
        <p class="card-text"><strong>Date: </strong>{{ shortDate }}</p>
        <p class="card-text"><strong>Time: </strong>{{ shortTime }}</p>
    </div>
    <div class="card-footer text-body-secondary">
      <a href="#" class="btn btn-primary" @click="toggleModal">Details</a>
  </div>
</div>

<CardDetailsModal v-if="showModal" :task="task" :showModal="toggleModal" 
@close="toggleModal" @task-updated="refreshTask"/>
</template>

<script>
import CardDetailsModal from '../homeComps/CardDetailsModal.vue';
import {ref, computed} from 'vue'
export default {
    name: 'TaskCard',
    components: {CardDetailsModal},
    props: ['task'],
    setup(props){
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

        const showModal = ref(false);
        
        const toggleModal = () => {
            showModal.value = !showModal.value
        }

        const refreshTask = async (updatedTask) => {
            props.task = updatedTask
        }

        return {showModal, toggleModal, shortDate, shortTime, refreshTask}
    },
}
</script>

<style>

</style>