var i = 0;
var files = [];
function move() {
    if (i == 0) {
        i = 1;
        var elem = document.getElementById("myBar");
        var width = 1;
        var id = setInterval(frame, 10);
        function frame() {
            if (width >= 100) {
                clearInterval(id);
                i = 0;
            } else {
                width++;
                elem.style.width = width + "%";
            }
        }
    }
}

document.querySelectorAll(".drop-zone__input").forEach(inputElement => {
    const dropZoneElement = inputElement.closest(".drop-zone");

    
    
    dropZoneElement.addEventListener("click", e => {
        
        inputElement.click();
    });

    inputElement.addEventListener("change", e => {
        if (inputElement.files.length) {
            for (var i = 0; i < inputElement.files.length; i++) {
                files.push(inputElement.files[i]);
            }
            for (var i = 0; i < files.length; i++) {

                inputElement.files[i] = files[i];
            }
            updateThumbnail(dropZoneElement, inputElement.files[0]);
        }
        
    });

    dropZoneElement.addEventListener("dragover", e => {
        e.preventDefault();
        dropZoneElement.classList.add("drop-zone--over");
    });

    ["dragleave", "dragend"].forEach(type => {
        dropZoneElement.addEventListener(type, e => {
            dropZoneElement.classList.remove("drop-zone--over");
        });
    });

    dropZoneElement.addEventListener("drop", e => {
        e.preventDefault();

        if (e.dataTransfer.files.length) {
            inputElement.files = e.dataTransfer.files;
            updateThumbnail(dropZoneElement, e.dataTransfer.files[0]);

            
            dropZoneElement.classList.remove("drop-zone--over");
        }
    });

    

});

function updateThumbnail(dropzoneElement, file) {

    let thumbnailElement = dropzoneElement.querySelector(".drop-zone__thumb");

    if (dropzoneElement.querySelector(".drop-zone__prompt")) {
        dropzoneElement.querySelector(".drop-zone__prompt").remove();
    }

    if (!thumbnailElement) {
        thumbnailElement = document.createElement("div");
        thumbnailElement.classList.add("drop-zone__thumb");
        dropzoneElement.appendChild(thumbnailElement);
    }

    thumbnailElement.dataset.label = file.name;

    if (file.type.startsWith("image/")) {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
            thumbnailElement.style.backgroundImage = `url('${reader.result}')`;
        };
    } else {
        thumbnailElement.style.backgroundImage = null;
    }

    
}

setTimeout(function () {
    document.getElementById("btnDownload").click();
}, 2000);


$(document).ready(function(){
   
});