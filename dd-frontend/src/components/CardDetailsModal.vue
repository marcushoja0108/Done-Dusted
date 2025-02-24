<template>
    <div v-if="showModal" class="modal fade show" tabindex="-1" 
    aria-labelledby="CardDetailsModalLabel" aria-hidden="true" style="display: block">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="CardDetailsModalLabel">{{ task.title }}</h5>
                    <button type="button" class="btn-close btn-danger" 
                    @click="closeModal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p><strong>Date: </strong> {{ task.date }}</p>
                    <p><strong>Time: </strong> {{ task.time }}</p>
                    <p><strong>Summary </strong> {{ task.summary }}</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    props: ['task', 'showModal'],
    methods: {
        closeModal(){
            this.$emit('close')
        }
    }
}
</script>
