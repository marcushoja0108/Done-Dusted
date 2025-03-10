<template>
  <div class="card text-center">
    <div v-if="!task.done" class="card-header">
        Upcoming task
    </div>
    <div v-else class="card-header bg-success text-white fs-5 fw-bold">
        Finished task
    </div>

    <div v-if="!task.done" class="card-body">
        <h5 class="card-title">{{ task.title }}</h5>
        <p class="card-text"><strong>Date: </strong>{{ shortDate }}</p>
        <p class="card-text"><strong>Time: </strong>{{ shortTime }}</p>
    </div>
    <div v-else>
        <h5 class="card-title">{{ task.title }}</h5>
        <p class="card-text"><strong>Date done: </strong>{{ shortDate }}</p>
        <p class="card-text"><strong>Time done: </strong>{{ shortTime }}</p>
    </div>
    <div class="card-footer text-body-secondary">
      <a href="#" class="btn btn-primary" @click="toggleModal">Details</a>
  </div>
</div>

<CardDetailsModal v-if="showModal" :task="task" :showModal="toggleModal" 
@close="toggleModal" @task-updated="refreshTask"/>
</template>

<script>
import CardDetailsModal from './homeComps/CardDetailsModal.vue';
import {ref, computed} from 'vue'
export default {
    name: 'TaskCard',
    components: {CardDetailsModal},
    props: ['task'],
    setup(props){
        const shortDate = computed(() => {
            if(!props.task.doDate) return "";
            let dateToChange = '';
            
            if(!props.task.doneDate){
                dateToChange = props.task.doDate
            }
            else{
                dateToChange = props.task.doneDate
            }
            return new Date(dateToChange).toLocaleDateString(undefined, {
                year: "numeric",
                month: "short",
                day: "numeric",
            });
        });
        
        const shortTime = computed(() => {
            if(!props.task.doTime) return "";
            let timeToChange = '';

            if(!props.task.doneDate){
                timeToChange = props.task.doTime
            }
            else{
                timeToChange = props.task.doneTime
            }
            return new Date(`2000-01-01T${timeToChange}`).toLocaleTimeString(undefined, {
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