<template>
  <section v-if="method=='create'">
    <!-- Component in Create mode -->


    <h4 class="p-2 border bg-light">Dodawanie nowego filmu<button @click="closeWindow" type="button" class="btn-close" aria-label="Close"></button></h4>

    <!-- Validate Alerts -->
    <div v-if="v$.movietitle.$error" class="alert alert-danger" role="alert">Tytuł filmu jest wymagany. Nie może zawierać więcej niż 200 znaków.</div>
    <div v-if="v$.movieyear.$error" class="alert alert-danger" role="alert">Rok Filmu nie może być mniejszy niż 1900 oraz nie może być większy niż 2100</div>

    <!-- Forms -->
    <div class="mb-3 row">
      <label for="movietitle" class="col-sm-2 col-form-label">Tytuł Filmu</label>
      <div class="col-sm-10">
        <input v-model="movietitle" class="form-control" id="movietitle" placeholder="Maksymalnie 200 znaków" />
      </div>
    </div>
    <div class="mb-3 row">
      <label for="movieyear" class="col-sm-2 col-form-label">Rok Produkcji</label>
      <div class="col-sm-10">
        <input type="number" class="form-control" v-model="movieyear" placeholder="" />
      </div>
    </div>
    
    <button type="button" @click="createMovie()" class="btn btn-info btn-block">Dodaj nowy film</button>
  </section>
  
  <section v-else-if="method=='edit'">
    <!-- Component in Edit mode -->

    <h4 class="p-2 border bg-light">Edycja filmu {{movie.name}}<button @click="closeWindow" type="button" class="btn-close" aria-label="Close"></button></h4>

    <!-- Validate Alerts -->
    <div v-if="v$.movietitle.$error" class="alert alert-danger" role="alert">Tytuł filmu jest wymagany. Nie może zawierać więcej niż 200 znaków.</div>
    <div v-if="v$.movieyear.$error" class="alert alert-danger" role="alert">Rok Filmu nie może być mniejszy niż 1900 oraz nie może być większy niż 2100</div>

    <!-- Forms -->
    <div class="mb-3 row">
      <label for="movietitle" class="col-sm-2 col-form-label">Tytuł Filmu</label>
      <div class="col-sm-10">
        <input v-model="movietitle" class="form-control" id="movietitle" placeholder="Maksymalnie 200 znaków" />
      </div>
    </div>
    <div class="mb-3 row">
      <label for="movieyear" class="col-sm-2 col-form-label">Rok Produkcji</label>
      <div class="col-sm-10">
        <input type="number" class="form-control" v-model="movieyear" placeholder="" />
      </div>
    </div>
    <button type="button" @click="updateMovie()" class="btn btn-info btn-block">Zapisz zmiany</button>
  </section>
  <section v-else-if="method=='show'">
    <!-- Component in Show mode -->

    <!-- Forms -->
    <h4 class="p-2 border bg-light">{{movie.name}}<button @click="closeWindow" type="button" class="btn-close" aria-label="Close"></button></h4>
    <div class="mb-3 row">
      <label for="movietitle" class="col-sm-2 col-form-label">Tytuł Filmu</label>
      <div class="col-sm-10">
        <input v-model="movietitle" readonly class="form-control-plaintext" id="movietitle" placeholder="Maksymalnie 200 znaków" />
      </div>
    </div>
    <div class="mb-3 row">
      <label for="movieyear" class="col-sm-2 col-form-label">Rok Produkcji</label>
      <div class="col-sm-10">
        <input type="number" readonly class="form-control-plaintext" v-model="movieyear" placeholder="" />
      </div>
    </div>
  </section>
</template>

<script>
import axios from 'axios';
import useVuelidate from '@vuelidate/core'
import { required, maxLength, minValue, maxValue } from '@vuelidate/validators'

export default {

  setup () {
    return { v$: useVuelidate() }
  },


  data() {
    return {
      movie: {},
      other: {},
      errored: false,
      movietitle: "",
      movieyear: ""
    };
  },

  //Setting up the validators.
  validations () {
    return {
      movietitle: { required, maxLength: maxLength(200), $autoDirty: true },
      movieyear: { minValue: minValue(1900), maxValue: maxValue(2100), $autoDirty: true }
    }
  },

  //Setting up the props
  props: {
      id: Number,
      method: String
  },


  watch: { 
    method: function() { 
      this.update();
    },
    id: function() { 
      this.update();
    },
    movie: function(){
      if(this.errored == false){
        this.movietitle = this.movie.name;
        this.movieyear = this.movie.year;
        this.$forceUpdate();
      }
      
    },
    other: function() { 
      this.closeWindow();
    }
  },


  methods: {

    //Function createMovie sends api request to create new entry for movie.
    async createMovie() {
      const result = await this.v$.$validate();
      if (!result) {
        return
      }
      let data = {Name: this.movietitle, Year: parseInt(this.movieyear)};
      if(this.movieyear == null){
        data = {Name: this.movietitle}
      }
      axios({
        method: 'post',
        url: 'http://localhost:8080/api/MovieItems/',
        data: data
      })
      .then(response => (this.other = response.data)).catch(error => {
          console.log(error)
          this.errored = true;
      });
    },

    //Function updateMovie sends api request to change value for currentyle edited movie.
    async updateMovie() {
      const result = await this.v$.$validate();
      if (!result) {
        return
      }

      axios({
        method: 'put',
        url: 'http://localhost:8080/api/MovieItems/'+this.movie.id,
        data: {
          ID: this.movie.id,
          Name: this.movietitle,
          Year: parseInt(this.movieyear)
        }
      })
      .then(response => (this.other = response.data)).catch(error => {
          console.log(error)
          this.errored = true;
      });
      
      
      
    },

    //Function Update downloads the data from API if is in show or edit mode. Function Update also clear form for create mode.
    update(){
      if(this.method=='show' || this.method == 'edit'){
        axios({
          method: 'get',
          url: 'http://localhost:8080/api/MovieItems/'+this.id,
          responseType: 'json'
        })
        .then(response => (this.movie = response.data)).catch(error => {
            console.log(error)
            this.errored = true;
        });

        
      }else if(this.method=='create'){
        this.movietitle = "";
        this.movieyear = "";
      }
    },

    //Function closeWindow is emiting signal o parent component to make it dissapear.
    closeWindow(){
      this.$emit('selfclose', true);
    }
  },


  mounted(){
    this.update();
  }
}
</script>

<style scoped>

</style>
