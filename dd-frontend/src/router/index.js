import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import AddTaskView from '../views/AddTaskView.vue'
import CompletedTasksView from '../views/CompletedTasksView.vue'
import RepeatingTasksView from '../views/RepeatingTasksView.vue'
import LoginView from '@/views/LoginView.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/addTask',
    name: 'addTask',
    component: AddTaskView
  },
  {
    path: '/repeatingTasks',
    name: 'repeatingTasks',
    component: RepeatingTasksView
  },
  {
    path: '/completedTasks',
    name: 'completedTasks',
    component: CompletedTasksView
  },
  {
    path: '/login',
    name: 'login',
    component: LoginView
  }
  
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
