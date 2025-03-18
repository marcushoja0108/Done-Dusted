<template>
  <div  class="row justify-content-center ">
    <TaskCard v-for="task in tasks" :key="task.id" :task="task" class="col-lg-4 col-sm-5 m-3"/>
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