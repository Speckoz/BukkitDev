$(function () {
  $('[data-toggle="tooltip"]').tooltip()
});

$(".btn-email").click(() => {
  let input = document.createElement('input');

  input.value = 'marcopandolfo577@gmail.com';

  document.body.appendChild(input);

  input.select();

  document.execCommand("Copy");

  input.remove();
});

$(".btn-wpp").click(() => {
  let input = document.createElement('input');

  input.value = '(11) 97441-8942';

  document.body.appendChild(input);

  input.select();

  document.execCommand("Copy");

  input.remove();
});

$('#btn-ver-mais').click(() => {
    $(".ver-mais-deck").css("display", "flex")
    .hide()
    .slideDown();

     $('#btn-ver-mais').remove();
});