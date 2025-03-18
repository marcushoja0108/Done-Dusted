<template>
  <div v-for="task in tasks" :key="task.id" class="row">
    <TaskCard class="col-3"/>
  </div>
</template>

<script>
import TaskCard from '../TaskCard.vue';
import { ref } from 'vue'

export default {
    name: 'RepeatList',
    components: { TaskCard },
    props: ['repeats'],

    setup(props){
        const tasks = ref(props.repeats)


        return { tasks }
    }
}
</script>

<style>

</style>