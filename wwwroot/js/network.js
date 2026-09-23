document.addEventListener("DOMContentLoaded", () => {
    initializeIpTable();
});


function initializeIpTable() {
    const filterElements = [
        "ipSearch",
        "typeFilter",
        "statusFilter",
        "screeningFilter"
    ];

    filterElements.forEach(id => {

        const element =
            document.getElementById(id);

        if (!element) {
            return;
        }

        element.addEventListener(
            element.tagName === "INPUT"
                ? "input"
                : "change",
            filterIpTable
        );
    });
}


function filterIpTable() {
    const searchInput =
        document.getElementById("ipSearch");

    const typeFilter =
        document.getElementById("typeFilter");

    const statusFilter =
        document.getElementById("statusFilter");

    const screeningFilter =
        document.getElementById("screeningFilter");

    const table =
        document.getElementById("ipTable");


    if (!table) {
        return;
    }


    const searchValue =
        searchInput?.value
            .toLowerCase()
            .trim() ?? "";


    const selectedType =
        typeFilter?.value ?? "";


    const selectedStatus =
        statusFilter?.value ?? "";


    const selectedScreening =
        screeningFilter?.value ?? "";


    const rows =
        table.querySelectorAll("tbody tr");


    rows.forEach(row => {

        const matchesSearch =
            row.textContent
                .toLowerCase()
                .includes(searchValue);


        const matchesType =
            selectedType === "" ||
            row.dataset.type === selectedType;


        const matchesStatus =
            selectedStatus === "" ||
            row.dataset.status === selectedStatus;


        const matchesScreening =
            selectedScreening === "" ||
            row.dataset.screening === selectedScreening;


        row.hidden =
            !(
                matchesSearch &&
                matchesType &&
                matchesStatus &&
                matchesScreening
            );
    });
}