// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const vType = document.querySelector(".vType");
const sn1 = document.querySelector(".sn1");
const sn2 = document.querySelector(".sn2");
const sn3 = document.querySelector(".sn3");



vType.addEventListener("change", () => 
{
    const selectedOption = vType.options[vType.selectedIndex].text;
    console.log(selectedOption);
    
    if (selectedOption == "Truck")
    {
        sn2.style.display = "inline";
        sn3.style.display = "inline";
    } 
    else if (selectedOption == "Van") 
    {
        sn2.style.display = "inline";
        sn3.style.display = "none";
    }
    else {
        sn2.style.display = "none";
        sn3.style.display = "none";
    }
        
})

sn1.addEventListener("input", (e) =>{
    console.log(e.target.value);
})
