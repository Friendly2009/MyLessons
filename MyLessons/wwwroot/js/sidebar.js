document.addEventListener("DOMContentLoaded", function () {
    const sidebar = document.getElementById("sidebar");
    const toggleBtn = document.getElementById("toggleSidebar");

    let isOpen = false;

    toggleBtn.addEventListener("click", function () {
        if (!isOpen) {
            sidebar.style.left = "0px";
            toggleBtn.style.left = "500px";
            isOpen = true;
        } else {
            sidebar.style.left = "-500px";
            toggleBtn.style.left = "0px";
            isOpen = false;
        }
    });
});