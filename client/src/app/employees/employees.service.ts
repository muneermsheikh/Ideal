import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IEmployee } from '../shared/models/employee';
import { EmployeeParams } from '../shared/models/employeeParams';
import { IPaginationCandidate } from '../shared/models/paginationCand';
import { IPaginationEmployee, PaginationEmployee } from '../shared/models/paginationEmployee';

@Injectable({
  providedIn: 'root'
})

export class EmployeesService {
  private baseUrl = environment.apiUrl;
  employees: IEmployee[] = [];
  paginationEmployee = new PaginationEmployee();
  empParams = new EmployeeParams();


  constructor(private http: HttpClient) { }

  addEmployee(values: any): any {
    return this.http.post(this.baseUrl + 'employees', values).pipe(
      map((emp: IEmployee) => {
        if (emp) {
          console.log(emp);
        }
      }, error => {
        console.log(error);
      })
    );
  }


  /* delete returns integer - correct flg

  deleteEmployee(values: IEmployee) {
    return this.http.delete(this.baseUrl + 'employees', values).pipe(
      map((emp: number) => {
        if (emp !== 0) {
          console.log('employee deleted'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }
*/

  updateCandidate(values: IEmployee): any {
    return this.http.put(this.baseUrl + 'employees', JSON.stringify(values)).pipe(
      map((emp: IEmployee) => {
        if (emp) {
          console.log('emp updated'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }


  getEmployeeById(id: number): any {
    const emp = this.employees.find(p => p.id === id);
    if (emp) {
      return of(emp);
    }
    return this.http.get<IEmployee>(this.baseUrl + 'employees/' + id);
  }

  getEmployees(useCache: boolean): any {

    if (useCache === false) {
      this.employees = [];
    }

    if (this.employees.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.employees.length / this.empParams.pageSize);

      if (this.empParams.pageNumber <= pagesReceived) {
        this.paginationEmployee.data =
          this.employees.slice((this.empParams.pageNumber - 1) * this.empParams.pageSize,
            this.empParams.pageNumber * this.empParams.pageSize);

        return of(this.paginationEmployee);
      }
    }

    let params = new HttpParams();

    if (this.empParams.position !== '') {
      params = params.append('position', this.empParams.position);
    }

    if (this.empParams.cityName !== '') {
      params = params.append('cityName', this.empParams.cityName);
    }

    if (this.empParams.search)
    {
      params = params.append('search', this.empParams.search);
    }

    params = params.append('sort', this.empParams.sort);

    params = params.append('pageIndex', this.empParams.pageNumber.toString());
    params = params.append('pageSize', this.empParams.pageSize.toString());

    return this.http.get<IPaginationEmployee>(this.baseUrl + 'employees', { observe: 'response', params })
      .pipe(
        map(response => {
          this.employees = [...this.employees, ...response.body.data];
          console.log(this.employees);
          this.paginationEmployee = response.body;
          // console.log('getCandidates returned ' + response.body.count);
          return this.paginationEmployee;
        }, error => {
          console.log(error);
        })
      );
  }

  getEmpParams(): EmployeeParams {
    return this.empParams;
  }

  setEmpParams(params: EmployeeParams): void {
    this.empParams = params;
  }

  checkEmployeeExistsByAadharNo(aadharnumber: string): any {
    const exists =  this.http.get(this.baseUrl +
      'employees/empexists?aadharnumber=' + aadharnumber);
    return exists === null ? false : true;
  }


}

