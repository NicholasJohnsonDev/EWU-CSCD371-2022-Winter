function fetchJoke(){
  let sitePath = "https://v2.jokeapi.dev/joke/Programming";
  axios.get(sitePath).then (
    (response) => {
      //for single jokes 
      if(response.data.type == "single"){
        document.getElementById("setup").innerHTML = "";
        document.getElementById("delivery").innerHTML = "";
        document.getElementById("single-joke").innerHTML = response.data.joke;
      }
      //For two part jokes
      if(response.data.setup != null){
        document.getElementById("setup").innerHTML = response.data.setup;
        document.getElementById("single-joke").innerHTML = "";
        document.getElementById("delivery").innerHTML = "";
        setTimeout(function(){
        document.getElementById("delivery").innerHTML = response.data.delivery;
        }, 800);
      }
    },
    (error) => {
      console.log(error);
    }
  );
}
