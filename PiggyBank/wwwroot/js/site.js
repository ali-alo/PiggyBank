// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const balanceElement = document.querySelector("#userBalance");
if (balanceElement)
    getUserBalance();

async function getUserBalance() {
    try {
        const response = await fetch("/User/GetUserBalance", {
            method: "GET",
        });

        if (response.ok) {
            const data = await response.json();
            const balance = data.balance;

            if (balanceElement) {
                balanceElement.textContent = `Ваш Баланс: ${balance} сум`;
                balanceElement.classList.add(balance < 0 ? "text-danger" : "text-success");
            }
        } else {
            console.log("Log in to check your balance");
        }
    } catch (error) {
        console.error("Error:", error);
    }
}

// code to format amount input
const amountInput = document.getElementById('amountInput');

if (amountInput) {
    amountInput.addEventListener('input', function (e) {
        const value = e.target.value.replace(/\D/g, ''); // Remove non-numeric characters
        const formattedValue = formatWithThousandsSeparator(value);
        e.target.value = formattedValue;
    });

    function formatWithThousandsSeparator(value) {
        return Number(value).toLocaleString('en-US');
    }

    amountInput.value = formatWithThousandsSeparator(amountInput.value);
}