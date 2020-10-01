class DannyPaginator {

    
    constructor(options) {
        debugger;
        this.id = options.id;
        this.element = document.getElementById(this.id);
        this.tableID = options.tableID;
        this.tableBody = document.querySelector("#" + this.tableID + "tbody");
        this.tableElement = document.getElementById(this.tableID);

        this.nextButton = this.element.querySelector('next');
        this.previousButton = this.element.querySelector('previous');

        this.paginationLabel = this.element.querySelector('pagination-label');

        this.noOfRows = options.noOfRows;

        this.start = 1;
        this.end = this.noOfRows;

        this.init();
    }

    init() {
        this.collectingTableInfo();
    }

    collectingTableInfo() {

        this.totalRows = this.tableBody.querySelectorAll('tr');
        console.log(this.totalRows);
        if (this.totalRows <= this.noOfRows) {
            this.element.style.display = 'none';
        }
        //else {
        //    this.showRows(this.totalRows, this.start, this.end);
        //}
    }

    //showRows(rows, start, end) {
    //    start = start - 1
    //    end = end - 1;
    //    this.tableBody.innerHTML = "";
    //    for (var i = 0; i < rows.length; i++) {
    //        if (i >= start && i <= end) {
    //            this.tableBody.appendChild(rows[i]);
    //        }
    //    }

    }
}