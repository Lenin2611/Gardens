function togglePopup() {
    const popup = document.getElementById('popup');
    const overlay = document.getElementById('overlay');
    if (popup.style.display === 'none' || popup.style.display === '') {
        popup.style.display = 'block';
        overlay.style.display = 'block';
        setTimeout(() => {
            popup.style.opacity = '1';
            overlay.style.opacity = '1';
        }, 10); 
    } else {
        popup.style.opacity = '0';
        overlay.style.opacity = '0';
        setTimeout(() => {
            popup.style.display = 'none';
            overlay.style.display = 'none';
        }, 300);
    }
}