document.addEventListener('DOMContentLoaded', function () {
	const openBtn = document.getElementById('addClassBtn');
	const overlay = document.getElementById('customOverlay');
	const closeBtn = document.getElementById('closeModal');

	if (openBtn) {
		openBtn.addEventListener('click', () => {
			overlay.style.display = 'flex';
			document.body.classList.add('modal-open');
		});
	}

	const close = () => {
		overlay.style.display = 'none';
		document.body.classList.remove('modal-open');
	};

	closeBtn.addEventListener('click', close);
	overlay.addEventListener('click', (e) => { if (e.target === overlay) close(); });
});