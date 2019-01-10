// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

var url = window.location.href.replace('http', 'ws');
var chat = new WebSocket(url + "Chat");

chat.onopen = function (ev) {
    chat.send("testing");
}

chat.onmessage = function (ev) {
    console.log(ev.data);
}