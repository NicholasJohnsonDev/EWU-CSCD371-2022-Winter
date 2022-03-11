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
        }, 4000);
      }
    },
    (error) => {
      console.log(error);
      document.getElementById("setup").innerHTML = "Please try again in a few moments.";
    }
  );
}

//Followed this hamburger tutorial https://dev.to/ljcdev/easy-hamburger-menu-with-js-2do0
//Modified some styling to fit the requirements but other than that the general logic was followed from the above site.

//grab the elements
const menu = document.querySelector(".menu");
const menuItems = document.querySelector(".menuItem");
const hamburger = document.querySelector(".hamburger");
const closeIcon = document.querySelector("#closeIcon");
const menuIcon = document.querySelector("#menuIcon");

function toggleMenu(){
  //change menu element's class to hide it's contents
  if(menu.classList.contains("showMenu")){
    menu.classList.remove("showMenu");
    //change favicon back to a hamburger
    closeIcon.style.display = "none";
    menuIcon.style.display = "inline";
  } 
  //change menu element's class to show it's contents
  else{
    menu.classList.add("showMenu");
    //change favicon to an x
    closeIcon.style.display = "inline";
    menuIcon.style.display = "none";
  }
}

//listen for the mouse click on the hamburger icon.
hamburger.addEventListener("click", toggleMenu);

menuItems.forEach( 
  function(menuItem) { 
    menuItem.addEventListener("click", toggleMenu);
  }
);