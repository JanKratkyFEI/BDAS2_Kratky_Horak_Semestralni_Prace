// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//skript na to aby při stavu vypůjčeno nešlo nastavit půjčení na déle než 3 měsíce

document.querySelector("form").addEventListener("submit", function (event) {
    const stav = document.querySelector("#Stav").value;
    const zacatek = new Date(document.querySelector("#ZacatekStav").value);
    const konec = new Date(document.querySelector("#KonecStav").value);

    if (stav === "vypůjčeno" && (konec - zacatek) > 90 * 24 * 60 * 60 * 1000) {
        event.preventDefault();
        alert("Doba vypůjčení nesmí být delší než 3 měsíce!");
    }
});





