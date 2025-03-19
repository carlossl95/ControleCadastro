
    < !--Toast Notification(aparece no canto inferior direito)-- >
    <div class="toast-container position-fixed bottom-0 end-0 p-3">
        <div id="toastNotification" class="toast" role="alert" aria-live="assertive" aria-atomic="true" data-bs-delay="3000">
            <div class="toast-header">
                <strong class="me-auto">Mensagem</strong>
                <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body" id="toastMessage"></div>
        </div>
    </div>

    <script>
        function showToast(message, isSuccess) {
            var toastMessage = document.getElementById("toastMessage");

            var decodedMessage = decodeHtml(message);

            toastMessage.textContent = decodedMessage;

            var toast = document.getElementById('toastNotification');
            if (isSuccess) {
                toast.classList.remove("bg-danger");
                toast.classList.add("bg-success"); // Cor verde para sucesso
            } else {
                toast.classList.remove("bg-success");
                toast.classList.add("bg-danger"); // Cor vermelha para erro
            }

            var bootstrapToast = new bootstrap.Toast(toast);
            bootstrapToast.show(); // Exibe o toast
        }

        function decodeHtml(html) {
            var txt = document.createElement('textarea');
            txt.innerHTML = html;
            return txt.value;
        }

        // Verifica se existe uma mensagem de sucesso ou erro e exibe a mensagem
        @if (TempData["Message"] != null)
        {
            <text>
                    var isSuccess = '@TempData["Codigo"]' == '200'; // Checa se o código é 200 (sucesso)
                    showToast('@TempData["Message"]', isSuccess);
            </text>
        }
    </script>