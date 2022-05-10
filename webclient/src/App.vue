<template>
  <div class="container text-center">
    <h1 class="">Moje Filmy</h1>
    <div class="row">
      <div class="col-3">
        <h5 class="text-center">Filmy:</h5>

        <section v-if="errored">
          <p>Wystąpił błąd i nie udało się połączyć z API.</p>
        </section>

        <section v-else>
          <div class="p-1">
          <button type="button" @click="createMovie()" class="btn btn-info btn-block">Dodaj nowy film</button>
          </div>
          
          <div v-for="item in movies" :key="item.name" class="p-2 border bg-light">
            {{item.name}}
            <div class="btn-group" role="group" aria-label="Basic example">
              <button type="button" @click="showMovie(item.id)" class="btn btn-primary" ><i class="bi bi-eye-fill"></i></button>
              <button type="button" @click="editMovie(item.id)" class="btn btn-secondary"><i class="bi bi-pencil-fill"></i></button>
              <button type="button" @click="deleteMovie(item.id)" class="btn btn-danger"><i class="bi bi-trash-fill"></i></button>
            </div>
          </div>
        </section>
        
      </div>
      <div class="col">
        <section v-if="selectedMethod!=null">
          <MovieComp @selfclose="selfClose()" :id="selectedId" :method="selectedMethod"></MovieComp>
        </section>
      </div>
    </div>
  </div>
</template>

<script>
import MovieComp from './components/MovieComp.vue'
import axios from 'axios';

export default {
  name: 'App',
  components: {
    MovieComp
  },

  watch: { 
    movies: function(){
      this.$forceUpdate();
    },
    other: function(){
      this.update();
    }
  },

  data() {
    return {
      movies: [], //Data about movies
      other: [], 
      errored: false,
      selectedId: null,
      selectedMethod: null
    };
  },

  methods: {

    //Function editMovie is setting up MovieComp "window" to create mode
    createMovie() {
      this.selectedId = null;
      this.selectedMethod = "create";
    },

    //Function editMovie is setting up MovieComp "window" to show mode with specific id 
    showMovie(movieID) {
      this.selectedId = movieID;
      this.selectedMethod = "show";
    },

    //Function editMovie is setting up MovieComp "window" to edit mode with specific id 
    editMovie(movieID) {
      this.selectedId = movieID;
      this.selectedMethod = "edit";
    },

    //Function deleteMovie is sending api request to delete a movie with specific id
    deleteMovie(movieID) {
      if(confirm("Czy na pewno chcesz usunąć ten film?")){
        this.selectedId = null;
        this.selectedMethod = null;
        axios({
          method: 'delete',
          url: 'http://localhost:8080/api/MovieItems/'+movieID
        })
        .then(response => (this.other = response.data)).catch(error => {
            console.log(error)
            this.errored = true;
        });
      }
    },

    //Function update is downloading newest data from api
    update(){
      axios({
        method: 'get',
        url: 'http://localhost:8080/api/MovieItems',
        responseType: 'json'
      })
      .then(response => (this.movies = response.data)).catch(error => {
          console.log(error)
          this.errored = true;
      })
    },

    //Function selfClose is closing MovieComp window.
    selfClose() {
      this.selectedId = null;
      this.selectedMethod = null;
      this.update();
    }
  },
  mounted() {
    this.update();
  }
}

</script>

<style>

</style>
