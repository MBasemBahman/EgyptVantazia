$("#downloadPDF").click(function () {
    downloadPDF();
});

function downloadPDF() {
    // $("#pdfContent").addClass('ml-215'); // JS solution for smaller screen but better to add media queries to tackle the issue
    getScreenshotOfElement(
        $("#pdfContent").get(0),
        0,
        0,
        $("#pdfContent").width() + 45,  // added 45 because the container's (pdfContent) width is smaller than the image, if it's not added then the content from right side will get cut off
        $("#pdfContent").height() + 30, // same issue as above. if the container width / height is changed (currently they are fixed) then these values might need to be changed as well.
        function (data) {
            var pdf = new jsPDF("l", "pt", [
                $("#pdfContent").width(),
                $("#pdfContent").height(),
            ]);

            pdf.addImage(
                "data:image/png;base64," + data,
                "PNG",
                0,
                0,
                $("#pdfContent").width(),
                $("#pdfContent").height()
            );
            pdf.save("profile.pdf");
        }
    );
}

// this function is the configuration of the html2cavas library (https://html2canvas.hertzen.com/)
// $("#pdfContent").removeClass('ml-215'); is the only custom line here, the rest comes from the library.
function getScreenshotOfElement(element, posX, posY, width, height, callback) {
    html2canvas(element, {
        onrendered: function (canvas) {
            // $("#pdfContent").removeClass('ml-215');  // uncomment this if resorting to ml-125 to resolve the issue
            var context = canvas.getContext("2d");
            var imageData = context.getImageData(posX, posY, width, height).data;
            var outputCanvas = document.createElement("canvas");
            var outputContext = outputCanvas.getContext("2d");
            outputCanvas.width = width;
            outputCanvas.height = height;

            var idata = outputContext.createImageData(width, height);
            idata.data.set(imageData);
            outputContext.putImageData(idata, 0, 0);
            callback(outputCanvas.toDataURL().replace("data:image/png;base64,", ""));
        },
        width: width,
        height: height,
        useCORS: true,
        taintTest: false,
        allowTaint: false,
    });
}
