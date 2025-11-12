 
function showLoading(message = "Carregando...") {
  if (document.getElementById("loader")) return; // Evita duplicar

  const loader = document.createElement("div");
  loader.id = "loader";
  loader.innerHTML = `
    <div class="loader-overlay">
      <div class="loader-box">
        <div class="spinner"></div>
        <p>${message}</p>
      </div>
    </div>
  `;
  document.body.appendChild(loader);
}

function hideLoading() {
  const loader = document.getElementById("loader");
  if (loader) loader.remove();
}

// ----- Toast -----
function showToast(message, type = "info") {
  const toast = document.createElement("div");
  toast.className = `toast toast-${type}`;
  toast.textContent = message;
  document.body.appendChild(toast);

  setTimeout(() => toast.classList.add("show"), 100);
  setTimeout(() => {
    toast.classList.remove("show");
    setTimeout(() => toast.remove(), 500);
  }, 3000);
}

// ----- Estilos do Loading e Toast (inline, podem ir no global.css) -----
const style = document.createElement("style");
style.innerHTML = `
  /* Loading */
  .loader-overlay {
    position: fixed;
    inset: 0;
    background: rgba(0,0,0,0.4);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 9999;
  }
  .loader-box {
    background: #fff;
    padding: 20px 40px;
    border-radius: 10px;
    text-align: center;
    box-shadow: 0 4px 15px rgba(0,0,0,0.3);
  }
  .spinner {
    border: 4px solid #eee;
    border-top: 4px solid var(--color-primary);
    border-radius: 50%;
    width: 30px;
    height: 30px;
    margin: 0 auto 10px;
    animation: spin 1s linear infinite;
  }
  @keyframes spin {
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
  }

  /* Toast */
  .toast {
    position: fixed;
    bottom: 20px;
    right: 20px;
    background: var(--color-surface);
    border-left: 5px solid var(--color-primary);
    color: var(--color-text);
    padding: 12px 20px;
    border-radius: 6px;
    box-shadow: 0 2px 8px rgba(0,0,0,0.2);
    opacity: 0;
    transform: translateY(20px);
    transition: opacity 0.4s, transform 0.4s;
    z-index: 9999;
  }
  .toast.show {
    opacity: 1;
    transform: translateY(0);
  }
  .toast-success { border-color: var(--color-success); }
  .toast-error { border-color: var(--color-error); }
  .toast-info { border-color: var(--color-primary); }
`;
document.head.appendChild(style);

window.showToast = showToast;
window.showLoading = showLoading; // Se existir
window.hideLoading = hideLoading; // Se existir
console.log("✅ utils.js carregado e funções globais disponíveis!");