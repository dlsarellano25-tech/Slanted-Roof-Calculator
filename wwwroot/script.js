const form     = document.getElementById("calc-form");
const resultEl = document.getElementById("result");
const errorEl  = document.getElementById("error");
const errorMsg = document.getElementById("error-msg");

form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const payload = {
        length:      parseFloat(document.getElementById("length").value),
        width:       parseFloat(document.getElementById("width").value),
        baseHeight:  parseFloat(document.getElementById("baseHeight").value),
        roofHeight1: parseFloat(document.getElementById("roofHeight1").value),
        roofHeight2: parseFloat(document.getElementById("roofHeight2").value),
    };

    const unit = document.getElementById("unit").value;

    hide(errorEl);
    hide(resultEl);

    try {
        const res = await fetch("/api/calculate", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload),
        });

        if (!res.ok) {
            const err = await res.json().catch(() => ({ error: "Request failed." }));
            showError(err.error || "Could not calculate volume.");
            return;
        }

        const data = await res.json();
        document.getElementById("r-base").textContent  = `${data.basePrismVolume} ${unit}\u00B3`;
        document.getElementById("r-roof").textContent  = `${data.slantedRoofVolume} ${unit}\u00B3`;
        document.getElementById("r-total").textContent = `${data.totalVolume} ${unit}\u00B3`;
        document.getElementById("r-formula").textContent = data.formula;
        show(resultEl);
    } catch (err) {
        showError("Could not reach the server. Make sure the app is running.");
    }
});

form.addEventListener("reset", () => {
    hide(resultEl);
    hide(errorEl);
});

function show(el) { el.classList.remove("hidden"); }
function hide(el) { el.classList.add("hidden"); }

function showError(msg) {
    errorMsg.textContent = msg;
    show(errorEl);
}
