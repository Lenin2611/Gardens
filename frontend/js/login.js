const inputs = document.querySelectorAll(".input-field");
const toggle_btn = document.querySelectorAll(".toggle");
const main = document.querySelector("main");
const bullets = document.querySelectorAll(".bullets span");
const images = document.querySelectorAll(".image");
const themeToggler = document.getElementById('theme-toggler');

inputs.forEach((inp) => {
  inp.addEventListener("focus", () => {
    inp.classList.add("active");
  });
  inp.addEventListener("blur", () => {
    if (inp.value != "") return;
    inp.classList.remove("active");
  });
});

toggle_btn.forEach((btn) => {
  btn.addEventListener("click", () => {
    main.classList.toggle("sign-up-mode");
  });
});

function moveSlider() {
  let index = this.dataset.value;

  let currentImage = document.querySelector(`.img-${index}`);
  images.forEach((img) => img.classList.remove("show"));
  currentImage.classList.add("show");

  const textSlider = document.querySelector(".text-group");
  textSlider.style.transform = `translateY(${-(index - 1) * 2.2}rem)`;

  bullets.forEach((bull) => bull.classList.remove("active"));
  this.classList.add("active");
}

bullets.forEach((bullet) => {
  bullet.addEventListener("click", moveSlider);
});


  //change theme
  themeToggler.addEventListener("click", () => {
    document.body.classList.toggle("dark-theme-variables");

    themeToggler.querySelector("span:nth-child(1)").classList.toggle("active");
    themeToggler.querySelector("span:nth-child(2)").classList.toggle("active");
  });



document.querySelector('#submit_register').addEventListener('click', async function(event) {
  event.preventDefault();
  const form = this.closest('form');
  const usernameInputRegister = document.querySelector('#username_register');
  const passwordInputRegister = document.querySelector('#password_register');
  const emailInputRegister = document.querySelector('#email_register');

  if (form.checkValidity()) {
    const usernameRegister = usernameInputRegister.value;
    const passwordRegister = passwordInputRegister.value;
    const emailRegister = emailInputRegister.value;
    console.log(emailRegister, usernameRegister, passwordRegister);
    register(emailRegister, usernameRegister, passwordRegister)
      .then(result => console.log(result))
      .catch(error => console.error(error));
      
  } else {
    form.reportValidity();
  }
});



// FUNCIONS

async function register (email, user, password) {
  const url = "http://localhost:5280/api/Auth/register";
  let update = {
      email: email,
      username: user,
      password: password
      };
  const options = {
      method: 'POST',
      headers: {
          'Content-Type': 'application/json',
          },
          body: JSON.stringify(update)
          };
  try {
      const response = await fetch(url, options);
      const result = await response.text();
      return (result);
  } catch (error) {
      console.error(error);
      return null;
  }
}

async function login (user, password) {
  const url = "http://localhost:5280/api/Auth/login";
  let update = {
      username: user,
      password: password,

      };
  const options = {
      method: 'POST',
      headers: {
          'Content-Type': 'application/json',
          },
          body: JSON.stringify(update)
          };
  try {
      const response = await fetch(url, options);
      const result = await response.text();
      return (result);
  } catch (error) {
      console.error(error);
      return null;
  }
}