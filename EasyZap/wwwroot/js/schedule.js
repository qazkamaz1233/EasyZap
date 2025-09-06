console.log("schedule.js loaded");

const monthNames = [
    "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
    "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
];

let currentDate = new Date();
const appointments = {};

// Получаем все рабочие дни месяца
async function getMonthWorkDays(year, month) {

    console.log("Start getMonthWorkDays");

    try {
        const response = await fetch(`/Schedule/GetDays?year=${year}&month=${month + 1}`);
        console.log("Fetch response:", response);

        if (!response.ok) {
            console.warn("Respopnse not OK!")
            return [];
        }

        const days = await response.json();
        console.log("Days from server:", days);
        return days;
    } catch (err) {
        console.error(err);
        return [];
    }
}

async function renderCalendar(date) {

    console.log("Start renderCalendar");

    const month = date.getMonth();
    const year = date.getFullYear();

    document.getElementById("monthName").textContent = `${ monthNames[month] } ${ year }`;

    const firstDayOfMonth = new Date(year, month, 1).getDay();
    const daysInMonth = new Date(year, month + 1, 0).getDate();

    const calendarGrid = document.getElementById("calendarGrid");
    calendarGrid.innerHTML = "";

    const weekDays = ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"];
    weekDays.forEach(day => {
        const weekDayCell = document.createElement("div");
        weekDayCell.classList.add("day-name");
        weekDayCell.textContent = day;
        calendarGrid.appendChild(weekDayCell);
    });

    let startDay = firstDayOfMonth === 0 ? 6 : firstDayOfMonth - 1;
    for (let i = 0; i < startDay; i++) {
        const emptyCell = document.createElement("div");
        emptyCell.classList.add("day", "empty");
        calendarGrid.appendChild(emptyCell);
    }

    for (let day = 1; day <= daysInMonth; day++) {
        const dayCell = document.createElement('div');
        dayCell.classList.add('day', 'calendar-day');
        dayCell.textContent = day;

        const dayStr = String(day).padStart(2, '0');
        const monthStr = String(month + 1).padStart(2, '0');
        dayCell.dataset.date = `${year}-${monthStr}-${dayStr}`;

        const today = new Date();
        if (day === today.getDate() && month === today.getMonth() && year === today.getFullYear()) {
            dayCell.classList.add("today");
        }

        dayCell.addEventListener("click", () => openDayPanel(day, month, year));

        calendarGrid.appendChild(dayCell);
    }

    // Загружаем рабочие дни месяца и подсвечиваем
    const workDays = await getMonthWorkDays(year, month);
    highlightWorkDays(workDays);
}

function openDayPanel(day, month, year) {

    console.log("Start openDayPanel");

    const panel = document.getElementById("dayPanel");
    const date = new Date(year, month, day);
    panel.classList.add("active");
    document.getElementById('selectedDate').textContent = date.toLocaleDateString();
    document.getElementById('selectedDate').dataset.date = date.toISOString().split('T')[0];

    loadWorkDays(year, month); // если хочешь сразу загружать данные дня

    const selectedDate = document.getElementById("selectedDate");
    selectedDate.textContent = `Расписание на ${day}.${month + 1}.${year}`;

    const startInput = document.getElementById("startTime");
    const endInput = document.getElementById("endTime");
    const notesInput = document.getElementById("notes");
    const appointmentsList = document.getElementById("appointmentsList");

    const key = `${year}-${month + 1}-${day}`;
    const data = appointments[key] || { start: "", end: "", notes: "", entries: [] };

    startInput.value = data.start;
    endInput.value = data.end;
    notesInput.value = data.notes;

    appointmentsList.innerHTML = "";
    data.entries.forEach(entry => {
        const li = document.createElement("li");
        li.textContent = entry;
        appointmentsList.appendChild(li);
    });

    document.getElementById('saveDay').addEventListener('click', async () => {
        const date = document.getElementById('selectedDate').dataset.date; // YYYY-MM-DD
        const startTime = document.getElementById('startTime').value;
        const endTime = document.getElementById('endTime').value;
        const notes = document.getElementById('notes').value;

        const workDay = {
            date: date,
            startTime: startTime,
            endTime: endTime,
            notes: notes
        };

        try {
            const response = await fetch('/Schedule/SaveDay', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include', // 👈 важно для куков (авторизации)
                body: JSON.stringify(workDay)
            });

            console.log("Response status:", response.status);

            if (!response.ok) {
                const errorText = await response.text(); // текст ответа (HTML/JSON/ошибка)
                console.error("Ошибка ответа:", errorText);
                throw new Error('Ошибка при сохранении: ' + response.status);
            }

            const result = await response.json();
            console.log("Ответ сервера:", result);

            if (result.success) {
                alert('Рабочий день сохранён!');
                if (typeof closeDayPanel === "function") {
                    closeDayPanel();
                }
                renderCalendar(currentDate); // перерисуем календарь
            } else {
                alert('Сервер вернул ошибку при сохранении');
            }
        } catch (err) {
            console.error("Catch:", err);
            alert('Ошибка при сохранении рабочего дня');
        }
    });
}

async function loadWorkDays(year, month) {
    try {
        const response = await fetch(`/Schedule/GetDays?year=${year}&month=${month + 1}`);
        if (!response.ok) return;

        const days = await response.json();
        highlightWorkDays(days); // подсветка ячеек
    } catch (err) {
        console.error(err);
    }
}

function highlightWorkDays(days) {

    console.log("Highlighting days:", days);

    document.querySelectorAll('.calendar-day').forEach(dayEl => {
        dayEl.classList.remove('has-workday');
    });

    days.forEach(day => {
        const dataStr = day.date || day.Date;
        if (!dataStr) return;

        const dateParts = dataStr.split('-');
        if (dateParts.length !== 3) return;

        const year = parseInt(dateParts[0], 10);
        const month = parseInt(dateParts[1], 10) - 1;
        const dayNum = parseInt(dateParts[2], 10);
        if (isNaN(year) || isNaN(month) || isNaN(dayNum)) return;

    const date = new Date(year, month, dayNum);

    const dayEl = document.querySelector(`.calendar-day[data-date='${date.toISOString().split('T')[0]}']`);
    if (dayEl) {
        dayEl.classList.add('has-workday');
    }
});
}

document.getElementById("prevMonth").addEventListener("click", () => {
    currentDate.setMonth(currentDate.getMonth() - 1);
    renderCalendar(currentDate);
});

document.getElementById("nextMonth").addEventListener("click", () => {
    currentDate.setMonth(currentDate.getMonth() + 1);
    renderCalendar(currentDate);
});

// Рендерим календарь при загрузке
renderCalendar(currentDate);

document.addEventListener("DOMContentLoaded", () => {
    console.log("schedule.js loaded, rendering calendar...");
    renderCalendar(new Date());
});