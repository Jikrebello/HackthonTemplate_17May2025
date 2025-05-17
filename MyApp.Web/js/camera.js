// camera.js

window.startCamera = async function (videoElementId) {
    const video = document.getElementById(videoElementId);
    if (!video) return;
    if (window._cameraStream) {
        video.srcObject = window._cameraStream;
        return;
    }
    try {
        const stream = await navigator.mediaDevices.getUserMedia({ video: true });
        video.srcObject = stream;
        window._cameraStream = stream;
    } catch (err) {
        alert("Unable to access camera: " + err);
    }
};

window.stopCamera = function (videoElementId) {
    const video = document.getElementById(videoElementId);
    if (video && video.srcObject) {
        const tracks = video.srcObject.getTracks();
        tracks.forEach(track => track.stop());
        video.srcObject = null;
    }
    if (window._cameraStream) {
        window._cameraStream.getTracks().forEach(track => track.stop());
        window._cameraStream = null;
    }
};

window.takePhoto = function (videoElementId) {
    const video = document.getElementById(videoElementId);
    if (!video) return null;
    const canvas = document.createElement('canvas');
    canvas.width = video.videoWidth || 320;
    canvas.height = video.videoHeight || 240;
    const ctx = canvas.getContext('2d');
    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
    return canvas.toDataURL('image/png');
};
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>MyApp.Web</title>
    <base href="/" />
    <link href="css/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="css/site.css" rel="stylesheet" />
    <link href="MyApp.Web.styles.css" rel="stylesheet" />
</head>
<body>
    <div id="app">Loading...</div>

    <script src="_framework/blazor.webassembly.js"></script>
    <script src="js/camera.js"></script>
</body>
</html>
