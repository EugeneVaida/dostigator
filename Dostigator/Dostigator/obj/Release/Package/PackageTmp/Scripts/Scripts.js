function ShowImagePreview(Img, previewImage) {
    if (Img.files && Img.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(Img.files[0]);
    }
}