(() => {
    const chartElement = document.getElementById("categoryChart");
    if (!chartElement || typeof Chart === "undefined") {
        return;
    }

    const labels = JSON.parse(chartElement.dataset.labels ?? "[]");
    const values = JSON.parse(chartElement.dataset.values ?? "[]");

    new Chart(chartElement, {
        type: "bar",
        data: {
            labels,
            datasets: [
                {
                    label: "Success Rate (%)",
                    data: values,
                    backgroundColor: "rgba(13, 110, 253, 0.65)",
                    borderColor: "rgba(13, 110, 253, 1)",
                    borderWidth: 1
                }
            ]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true,
                    max: 100
                }
            },
            plugins: {
                legend: {
                    display: false
                }
            }
        }
    });
})();
