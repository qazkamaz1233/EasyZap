document.addEventListener("DOMContentLoaded", () => {
    const btn = document.getElementById("generateLinkBtn");
    const resultDiv = document.getElementById("linkResult");
    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');

    if (!btn || !resultDiv || !tokenInput) return;

btn.addEventListener("click", async () => {
    const token = tokenInput.value;

    try {
        const response = await fetch("/Link/Generate", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": token
            },
            credentials: "same-origin"
        });

        if (!response.ok) {
            resultDiv.textContent = "Ошибка при генерации ссылки";
            return;
        }

        const data = await response.json();
        resultDiv.innerHTML = `
                <p>Ваша ссылка: <a href="${data.link}" target="_blank">${data.link}</a></p>
                <p>Токен: ${data.token}</p>
                <p>Создано: ${new Date(data.createdAt).toLocaleString()}</p>
            `;
    } catch (err) {
        console.error(err);
        resultDiv.textContent = "Ошибка сети или сервера";
    }
});
});