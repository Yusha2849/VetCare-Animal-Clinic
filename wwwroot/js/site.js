// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Add this script to your view or layout
$(document).ready(function () {
    $("#AnimalType").change(function () {
        var selectedAnimalType = $(this).val();

        $.get("/Pets/GetBreedsByAnimalType", { animalType: selectedAnimalType }, function (data) {
            var breedDropdown = $("#Breed");
            breedDropdown.empty();
            $.each(data, function (index, item) {
                breedDropdown.append($('<option>', {
                    value: item,
                    text: item
                }));
            });
        });
    });
});