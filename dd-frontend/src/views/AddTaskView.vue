<template>
  <div class="row m-5 justify-content-around">
    <div class="col-7">
      <div class="row">
        <div class="col-4">
          <label class="fw-bold fs-5">Date</label>
          <input type="Date" class="form-control">
        </div>
        <div class="col-4">
          <label class="fw-bold fs-5">Time</label>
          <input type="Time" class="form-control">
        </div>
        <div class="col-4">
          <label class="fw-bold fs-5">Repeats</label>
            <select class="form-select" aria-label="Repeats select">
              <option value="None">None</option>
              <option value="Daily">Daily</option>
              <option value="Weekly">Weekly</option>
              <option value="Monthly">Monthly</option>
            </select>
        </div>
      </div>
      <!-- end of row -->
       <div class="row mt-4">
        <div class="col">
          <input type="text" class="form-control fs-5 fw-bold" placeholder="Title">
        </div>
       </div>
       <!-- end of row -->
       <div class="row">
        <div class="col">
          <div class="form-floating my-4">
            <textarea class="form-control" style="height: 200px"></textarea>
            <label class="fw-bold fs-5">Summary</label>
          </div>
        </div>
       </div>
    </div>
    <!-- end of left -->
     <div class="col-4 text-center">
       <UserSelect/>
     </div>
  </div>
</template>

<script>
import UserSelect from '@/components/addComps/UserSelect.vue';


export default {
  props: [],
  components: {UserSelect}

}
</script>

<style>

</style>