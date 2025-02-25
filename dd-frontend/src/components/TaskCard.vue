<template>
  <div class="card text-center">
    <div class="card-header">
        Upcoming task
    </div>
    <div class="card-body">
        <h5 class="card-title">{{ task.title }}</h5>
        <p class="card-text">Date: {{ shortDate }}</p>
        <p class="card-text">Time: {{ shortTime }}</p>
    </div>
    <div class="card-footer text-body-secondary">
      <a href="#" class="btn btn-primary" @click="toggleModal">Details</a>
  </div>
</div>

<CardDetailsModal v-if="showModal" :task="task" :showModal="toggleModal" 
:shortDate="shortDate" :shortTime="shortTime" @close="toggleModal"/>
</template>

<script>
import CardDetailsModal from './CardDetailsModal.vue';
import {ref, computed} from 'vue'
export default {
    props: ['task'],
    components: {CardDetailsModal},
    setup(props){
        const shortDate = computed(() => {
            if(!props.task?.doDate) return "";

            return new Date(props.task.doDate).toLocaleDateString(undefined, {
                year: "numeric",
                month: "short",
                day: "numeric",
            });
        });
        
        const shortTime = computed(() => {
            if(!props.task?.doTime) return "";

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


        return {showModal, toggleModal, shortDate, shortTime}
    },
}
</script>

<style>

</style>