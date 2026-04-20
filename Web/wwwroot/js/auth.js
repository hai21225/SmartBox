const BASE_URL = "http://localhost:5033";

function showResult(data) {
    document.getElementById("result").textContent = JSON.stringify(data, null, 2);
}

async function login() {
    const data = {
        userName: document.getElementById("loginUsername").value,
        password: document.getElementById("loginPassword").value,
        confirmPass: ""
    };

    try {
        const res = await fetch(`${BASE_URL}/api/Auth/login`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });

        if (!res.ok) {
            showResult("Login failed");
            return;
        }

        const user = await res.json();

        // 👉 lưu user vào localStorage
        localStorage.setItem("user", JSON.stringify(user));

        // 👉 chuyển trang
        window.location.href = "home.html";

    } catch (err) {
        showResult(err);
    }
}

async function register() {
    const data = {
        userName: document.getElementById("regUsername").value,
        password: document.getElementById("regPassword").value,
        confirmPass: document.getElementById("regConfirm").value
    };

    try {
        const res = await fetch(`${BASE_URL}/api/Auth/register`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });

        const text = await res.text();
        showResult(text || "Register success");

    } catch (err) {
        showResult(err);
    }
}

function showPickup() {
    document.getElementById("main").style.display = "none";

    document.getElementById("content").innerHTML = `
        <h3>Nhận đồ</h3>
        Nhập mã: <input id="codeInput" /><br><br>
        <button onclick="submitCode()">Xác nhận</button>
        <br><br>
        <button onclick="goBack()">⬅ Quay lại</button>
    `;
}

function goBack() {
    document.getElementById("main").style.display = "block";
    document.getElementById("content").innerHTML = "";
}

async function submitCode() {
    const code = document.getElementById("codeInput").value;

    if (!code) {
        alert("Nhập code đi");
        return;
    }

    const res = await fetch(`${BASE_URL}/api/Usage/calculate-by-code?code=${code}`, {
        method: "POST"
    });

    if (!res.ok) {
        alert("Code không hợp lệ hoặc đã hết hạn");
        return;
    }

    const data = await res.json();

    let timeLeft = 100;

    document.getElementById("content").innerHTML = `
        <h3>Thanh toán</h3>
        Price: ${data.totalPrice} <br>
        <b id="timer">Hết hạn sau: 10s</b><br><br>
        <button onclick="payByCode('${code}')">Thanh toán</button>
        <br><br>
        <button onclick="goBack()">⬅ Quay lại</button>
    `;

    const interval = setInterval(() => {
        timeLeft--;
        document.getElementById("timer").innerText = `Hết hạn sau: ${timeLeft}s`;

        if (timeLeft <= 0) {
            clearInterval(interval);
            alert("Hết hạn, nhập lại code");
            showPickup();
        }
    }, 1000);
}
async function payByCode(code) {

    alert("Thanh toán thành công");
    // 2. kết thúc usage
    let resEnd = await fetch(`${BASE_URL}/api/Usage/end-by-code?code=${code}`, {
        method: "POST"
    });

    if (!resEnd.ok) {
        alert("Kết thúc phiên thất bại");
        return;
    }

    alert("Hoàn tất, tủ đã mở");

    goBack();
}