const BASE_URL = "http://localhost:5033";

const user = JSON.parse(localStorage.getItem("user"));
if (!user) window.location.href = "index.html";

document.getElementById("welcome").innerText = `Hello ${user.userName}`;

function logout() {
    localStorage.removeItem("user");
    window.location.href = "index.html";
}

// 
async function loadLockers() {
    const res = await fetch(`${BASE_URL}/api/Locker`);
    const lockers = await res.json();

    let html = "<h3>Chọn tủ</h3>";      

    lockers.forEach(l => {
        if (l.status) {
            html += `<div onclick="rent(${l.id})" style="cursor:pointer;background:#90EE90;margin:5px;padding:10px">
                        Locker ${l.id} (Available)
                     </div>`;
        } else {
            html += `<div style="background:#FF7F7F;margin:5px;padding:10px">
                        Locker ${l.id} (In Use)
                     </div>`;
        }
    });

    document.getElementById("content").innerHTML = html;
}

async function rent(lockerId) {
    if (!confirm("Thuê tủ này?")) return;

    const res = await fetch(`${BASE_URL}/api/Usage/start?userId=${user.id}&lockerId=${lockerId}`, {
        method: "POST"
    });

    alert(await res.text());
    loadLockers();
}

// 📚 LỊCH SỬ
async function loadHistory() {
    const res = await fetch(`${BASE_URL}/api/Usage/user/${user.id}`);
    const list = await res.json();

    let html = "<h3>Lịch sử</h3>";

    list.forEach(u => {
        html += `<div style="border:1px solid black;margin:5px;padding:10px">
            Locker ${u.lockerId} <br>
            Start: ${u.startTime} <br>
            End: ${u.endTime || "Đang dùng"} <br>
            Price: ${u.totalPrice}
        </div>`;
    });

    document.getElementById("content").innerHTML = html;
}

// 🔓 MỞ / KẾT THÚC
async function loadMyLocker() {
    const res = await fetch(`${BASE_URL}/api/Usage`);
    const list = await res.json();

    const activeList = list.filter(u => u.userId === user.id && u.endTime == null);

    const content = document.getElementById("content");

    if (activeList.length === 0) {
        content.innerHTML = "Không có tủ đang dùng";
        return;
    }

    content.innerHTML = `
        <h3>Tủ đang dùng</h3>
        ${activeList.map(active => `
            <div style="margin-bottom:10px">
                Locker ${active.lockerId} <br>
                <button onclick="calculatePrice(${active.lockerId})">
                    Kết thúc
                </button>
            </div>
        `).join("")}
    `;
}

async function endUsage(lockerId) {
    if (!confirm("Kết thúc thuê?")) return;

    const res = await fetch(`${BASE_URL}/api/Usage/end?lockerId=${lockerId}`, {
        method: "POST"
    });

    alert(await res.text());
    loadMyLocker();
}

async function calculatePrice(lockerId) {
    const res = await fetch(`${BASE_URL}/api/Usage/calculate/${lockerId}`);
    const data = await res.json();

    let timeLeft = 10;

    document.getElementById("content").innerHTML = `
        <h3>Thanh toán</h3>
        Price: ${data.totalPrice} <br>
        <b id="timer">Hết hạn sau: 10s</b><br><br>
        <button onclick="pay(${lockerId})">Thanh toán</button>
    `;

    const interval = setInterval(() => {
        timeLeft--;
        document.getElementById("timer").innerText = `Hết hạn sau: ${timeLeft}s`;

        if (timeLeft <= 0) {
            clearInterval(interval);
            alert("Hết hạn, vui lòng tính lại");
            loadMyLocker();
        }
    }, 1000);
}
async function pay(lockerId) {
    alert("Thanh toán thành công ");

    const res = await fetch(`${BASE_URL}/api/Usage/end?lockerId=${lockerId}`, {
        method: "POST"
    });

    alert(await res.text());

    loadMyLocker();
}