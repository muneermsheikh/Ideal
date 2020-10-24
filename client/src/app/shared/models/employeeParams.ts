export class EmployeeParams {
    position: string;
    cityName: string;
    active: boolean;
    sort = 'asc';
    pageNumber = 1;
    pageSize = 6;
    search: string;
}