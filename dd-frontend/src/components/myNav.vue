<template>
    <nav class="navbar navbar-expand-md navbar-light rounded border border-light bg-secondary shadow-sm m-4">
        <div class="container-xl d-flex justify-content-between align-items-center">
            <a href="#" class="navbar-brand d-flex align-items-center text-white fw-bold">
                <span class="fs-4">
                    <i class="bi bi-archive"></i>
                    <router-link class="text-decoration-none text-light" to="/">Done & Dusted</router-link>
                </span>
            </a>

            <button class="navbar-toggler" type="button" data-bs-toggle="collapse"
            data-bs-target="#main-nav" aria-controls="main-nav" aria-expanded="false"
            aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse justify-content-end align-center" id="main-nav">
                <ul class="navbar-nav">
                    <li class="nav-item mx-2">
                        <router-link :to="{ name: 'addTask' }" class="btn btn-success fw-bold">
                            <i class="bi bi-plus-circle"></i> Add Task</router-link>
                    </li>
                    <li class="nav-item mx-2">
                        <router-link :to="{ name: 'repeatingTasks' }" class="nav-link fw-bold text-light">
                            <i class="bi bi-repeat"></i> Repeating tasks</router-link>
                    </li>
                    <li class="nav-item mx-2">
                        <router-link :to="{ name: 'completedTasks' }" class="nav-link fw-bold text-light">
                            <i class="bi bi-card-checklist"></i> Completed tasks</router-link>
                    </li>
                    <li>
                        <button class="btn btn-danger fw-bold text-light" @click="logOut">Log out</button>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
</template>

<script>
export default {
    name: 'myNav',
    emits: ['log-out'],
    setup(_, { emit }) {

        const logOut = () => {
            localStorage.removeItem("jwt");
            localStorage.removeItem("userId");
            emit('log-out')
        }

        return { logOut }
    }
}
</script>

<style>

</style>