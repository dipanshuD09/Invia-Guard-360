let menuToggle = document.querySelector('.menu-toggle');
let navigation = document.querySelector('.navigation');

menuToggle.onclick = function () {
    navigation.classList.toggle('active');
}