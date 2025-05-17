namespace MyApp.Web.js
{
    public class scan
    {

            window.startCamera = async function (videoElementId) {
            const video = document.getElementById(videoElementId);
            if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
                window._blazorStream = await navigator.mediaDevices.getUserMedia({ video: true });
                video.srcObject = window._blazorStream;
                video.play();
            }
        };
    window.stopCamera = function (videoElementId) {
            const video = document.getElementById(videoElementId);
            if (window._blazorStream) {
                let tracks = window._blazorStream.getTracks();
                tracks.forEach(track => track.stop());
                window._blazorStream = null;
            }
            if (video) {
                video.srcObject = null;
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

    }
}
